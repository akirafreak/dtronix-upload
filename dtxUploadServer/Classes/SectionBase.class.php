<?php

/**
 * Base class to handle setup of section classes and common methods that they use.
 */
Class SectionBase{

	/**
	 * @var array Associative array that contains the entire user row from the database.
	 */
	protected $USER;

	/**
	 * @var array Associative array that contains the entire config.php array.
	 */
	protected $CONFIG;

	/**
	 * Internal variable.  Not to be accessed by external methods.
	 * 
	 * This is an assocative array that contains one or more user permission sets.<BR>
	 * The key of the array is the User ID.  The current User ID is used to access<BR>
	 * the current users permission set.
	 *
	 * @var array Variable that contains the user's and others us current permission set.
	 */
	private $PERMISSIONS = array();
	
	/**
	 *
	 * @var array Contains all the loaded theme classes. 
	 */
	private $THEME = array();

	/**
	 * @var SQL Reference to the active MySQL or PostgreSQL server.
	 */
	protected $SQL;
	
	/**
	 * Method to assign references to their objects.
	 * 
	 * @param array $user User variable that contains the entire user row in the database.
	 * @param array $config Contains server configuration file array.
	 * @param SQL $sql_server The reference to the SQL server instance.
	 */
	public function setVariables(&$user, &$config, &$sql_server){
		$this->USER = &$user;
		$this->CONFIG = &$config;
		$this->SQL = &$sql_server;
	}

	/**
	 * Method to control the incoming connections to the server.  Handles user logging in.
	 *
	 * @param bool $required True if the script should halt on user not being logged in. <BR>
	 * False if the script should continue even if the user is not logged in.
	 * @return ReturnData <code>
	 * "validation_invalid_username"
	 * "validation_invalid_password"
	 * "maintenance_mode"
	 * "validation_successful"
	 * "validation_invalid_user_session"
	 * "validation_expired_user_session"
	 * "validation_failed_no_login"
	 * "validation_account_dissabled"
	 * "validation_user_connection_dissabled"
	 * </code>
	 */
	protected function validateConnection($required = true){
		if(array_key_exists("session", $this->USER))
			return new ReturnData(true);
		
		if(isset($_COOKIE["client_username"]) && isset($_COOKIE["client_password"])){
			// User is tring to login.

			$user_row = $this->SQL->fetchRow("SELECT *
				FROM users
				WHERE username = '%s'
				LIMIT 1;", array($_COOKIE["client_username"]));

			if($user_row == false)
				returnClientData("validation_invalid_username");
			
			$pass1 = strtolower($user_row["password"]);
			$pass2 = strtolower($_COOKIE["client_password"]); // Hash the hashed password again.

			// Validate the user's password.
			if($pass1 != $pass2)
				returnClientData("validation_invalid_password");
			
			$session_key = createHash(32, true);

			// Limit the total number of logins on one account to 5.
			$this->SQL->insert("sessions", array(
				"session" => $session_key,
				"user_id" => $user_row["id"],
				"last_active" => time(),
				"date_created" => time()
			));
			

			$all_user_sessions = $this->SQL->fetchRows("SELECT session
				FROM sessions
				WHERE user_id = '%s'
				ORDER BY last_active DESC
				LIMIT 6;", array($user_row["id"]));

			if($all_user_sessions == false){
				errorHandler(0, "Server did not create a user session.  Please contact support.", __FILE__, __LINE__);
			}

			// If the user has too many sessions, delete the one last accessed.
			if(count($all_user_sessions) > 5){
				$this->SQL->query("DELETE FROM sessions
					WHERE session = '%s'", array(
						$all_user_sessions[5]["session"]
					));
			}

			// Update the client cookies.
			$this->deleteCookies();
			setcookie("session_key", $session_key, time() + (60 * 60 * 24 * 90));

			// TODO: Check to see if the user is admin before not allowing him in.
			if($this->CONFIG["server_maintenance_mode"]){
				returnClientData("maintenance_mode");

			}else{
				// Return the current session key.
				returnClientData("validation_successful", $session_key);
			}
			
			
		}elseif(isset($_COOKIE["session_key"])){
			// User is attempting to use an exsiting session.
			if(empty($_COOKIE["session_key"])){
				$this->deleteCookies();
				returnClientData("validation_invalid_user_session");
			}
			
			$user_session = $this->SQL->fetchRow("SELECT *
				FROM active_sessions
				WHERE session = '%s'", array($_COOKIE["session_key"]));
			
			if($user_session == false){
				$this->deleteCookies();
				returnClientData("validation_invalid_user_session");
			}

			// Check to see if the session has expired
			if($user_session["last_active"] + $this->CONFIG["session_max_life"] < time()){
				$this->SQL->query("DELETE FROM sessions
					WHERE session = '%s'", array($_COOKIE["session_key"]));

				$this->deleteCookies();
				returnClientData("validation_expired_user_session");
			}
			

			$this->SQL->update("sessions", array(
				"last_active" => time()
			), "WHERE session = '%s'", array(
				$_COOKIE["session_key"]
			));

			$user_row = $this->SQL->fetchRow("SELECT *
				FROM users
				WHERE id = '%s'
				LIMIT 1", array($user_session["user_id"]));

			if($this->CONFIG["server_maintenance_mode"]){
				// Check to see if the user is an admin.  If so, then let him on in.
				if($user_row["permissions"] != 0)
					returnClientData("maintenance_mode");

			}else{
				$this->USER = array_merge($this->USER, $user_row);
				$this->USER["session"] = $_COOKIE["session_key"];
			}
			
			// Ensure the user is not banned and other connection verifications.
			$validate = $this->checkConnectionPermissions();
			if($validate->successful == false)
				return $validate;
			
			// Session is valid.
			return new ReturnData(true);
			
		}else{
			if($required)
				returnClientData("validation_failed_no_login");
		}
	}
	
	/**
	 * Removes all server created cookies on the client.
	 * @return void
	 */
	private function deleteCookies(){
		setcookie("client_username", "", time() - 60 * 60 * 24);
		setcookie("client_password", "", time() - 60 * 60 * 24);
		setcookie("session_key", "", time() - 60 * 60 * 24);
	}
	
	/**
	 * Private method to ensure the client is allowed to connect.
	 * @return ReturnData <code>
	 * "validation_user_connection_dissabled"
	 * "validation_account_dissabled"
	 * </code>
	 */
	private function checkConnectionPermissions(){
		$can_connect = $this->getPermission("can_connect");
		if($can_connect->successful == false || $can_connect->data == false){
			$this->deleteCookies();
			returnClientData("validation_user_connection_dissabled");
		}
		
		$is_disabled = $this->getPermission("is_disabled");
		if($is_disabled->successful == false || $is_disabled->data == true){
			$this->deleteCookies();
			returnClientData("validation_account_dissabled");
		}
		
		return new ReturnData(true);
	}

	/**
	 * Verifies the current user's permission.
	 * 
	 * Stores the information in the reference variable $PERMISSIONS.  If the <BR>
	 * User ID parameter is specified, the current user's manage_user permission is verified. <BR>
	 * If the check fails, a permission error is thrown to the client.
	 *
	 * @param string $permission_check Permission to verify.
	 * @param int $uid User ID of the account to check the permission of.
	 * @return ReturnData <code>
	 * "validation_manage_users"
	 * "validation_error"
	 * null: True if the permission is set to 1, false if it is set to 0, mixed for all other values.
	 * </code>
	 */
	protected function getPermission($permission_check, $uid = false){
		$this->validateConnection();
		
		// Ensure that the requested User ID does not belong to the current user.
		if($uid != false && $this->USER["id"] != $uid){
			//Verify that the connected user can manage other peoples info.
			if(!$this->getPermission("manage_users"))
				return new ReturnData(false, "validation_manage_users");
			
			$permission_level = $this->SQL->fetchRow("SELECT permissions
				FROM users
				WHERE id = '%s'", array(
					$uid
				));
			$permission_level = $permission_level["permissions"];
			
		}else{
			$uid = $this->USER["id"];
			$permission_level = $this->USER["permissions"];
		}

		if(!array_key_exists($uid, $this->PERMISSIONS)){
			$this->PERMISSIONS[$uid] = $this->SQL->fetchRow("SELECT *
				FROM users_permissions
				WHERE id = '%s'",
				array(
					$permission_level
				));

			if($this->PERMISSIONS[$uid] == false)
				return new ReturnData(false, "validation_error");
		}
		
		if(isset($this->PERMISSIONS[$uid][$permission_check])){
			if($this->PERMISSIONS[$uid][$permission_check] == 1){
				return new ReturnData(true, null, true);

			}else if($this->PERMISSIONS[$uid][$permission_check] == 0){
				return new ReturnData(true, null, false);

			}else{
				return new ReturnData(true, null, $this->PERMISSIONS[$uid][$permission_check]);
			}
		}
		return new ReturnData(true);
	}
	
	/**
	 * Loads the requested theme file and outputs it.
	 * 
	 * @param string $name Name of the theme file to load.
	 * @param array $arguments An array of arguments to pass to the called function.
	 * @return void
	 * 
	 * <b>Example:</b><br />
	 * This will load the ThemeMain.theme.php file in the current theme directory and output the contents in the header() function.
	 * <code>
	 * $this->outputTheme("ThemeMain.header", array($arg_one, $arg_two));
	 * </code>
	 */
	protected function outputTheme($name, $arguments = null){
		list($class, $method) = explode(".", $name);
		if(array_key_exists($class, $this->THEME)){
			$called_class = $this->THEME[$class];
			
		}else{
			$file = $this->CONFIG["dir"] . "Theme/" . $this->CONFIG["html_theme"] . "/". $class .".php";
			require_once($file);

			$called_class = new $class();
			$called_class->setVariables($this->USER, $this->CONFIG);
			$this->THEME[$class] = $called_class;
		}
		// Finally, call the requested method in the new class instance.
		call_user_func_array(array($called_class, $method), ($arguments == null)? array() : $arguments);
	}
	
	/**
	 * Saves a notification in the database to be read by the client on the next notification request.
	 * 
	 * @param string $type Notification type that tells the client how to handle this notification.
	 * @param string $user_id User ID of the person to send the notification to.
	 * @param mixed $data Data to send to the client that pertains to the notification.
	 * @return void
	 */
	protected function saveNotification($type, $user_id, $data){
		$this->SQL->insert("notifications", array(
			"user_id" => $user_id,
			"type" => $type,
			"data" => json_encode($data),
			"time" => time()
		));
	}
	
	/**
	 * Method to save settins server-side in a JSON array.
	 * 
	 * @param string $name Key of the name value pairing.
	 * @param string $value Value to set for the name.
	 * @param int $uid User ID of the account to modify.  The current user must have the manage_users permission.
	 * @return ReturnData <code>
	 * "user_setting_set_successful"
	 * "user_setting_set_failure"
	 * </code>
	 */
	protected function setVariable($name, $value, $uid = false){
		$this->validateConnection();
		$settings = array();
		$account = $this->getAccount($uid);

		// Ensure that the data is not empty.
		if(!empty($account["settings"])){
			$settings = json_decode($account["settings"]);
		}
		
		$settings[$name] = $value;
		$json_string = json_encode($settings);
		
		$successful = $this->SQL->update("users", array(
			"settings" => $json_string
		), "WHERE id = '%s'", array(
			$account["id"]
		));

		if($successful){
			return new ReturnData(true, "user_setting_set_successful");
		}else{
			return new ReturnData(false, "user_setting_set_failure");
		}
	}
	
	/**
	 * Retrieve a user setting saved in a JSON array.
	 * 
	 * @param string $name Key of the value to retrieve.
	 * @param int $uid User ID of the account to modify.  The current user must have the manage_users permission.
	 * @return ReturnData <code>
	 * "user_setting_get_successful": array($name, Setting Value)
	 * "user_setting_get_failure"
	 * </code>
	 */
	protected function getVariable($name, $uid = false){
		$this->validateConnection();
		$settings = array();
		$account = $this->getAccount($uid);

		// Ensure that the data is not empty.
		if(!empty($account["settings"])){
			$settings = json_decode($account["settings"]);
		}

		if($successful){
			return new ReturnData(true, "user_setting_get_successful", array($name, $settings[$name]));
		}else{
			return new ReturnData(false, "user_setting_get_failure");
		}
	}
	
		
	/**
	 * Internal method that retrieves the account that is associated with the User ID.
	 * 
	 * Checks permissions if the current user is attempting to access another user's account.
	 * User must have the manage_users permission set to true.
	 * 
	 * @param int $uid false to retrieve the current user's account.
	 * @return ReturnData <code>
	 * "validation_manage_users"
	 * "user_info_invalid_user"
	 * </code>
	 */
	private function getAccount($uid){
		if($uid == false || $this->USER["id"] == $uid){
			$user_lookup = $this->USER;
			
		}else{
			if(!$this->getPermission("manage_users"))
				return new ReturnData(false, "validation_manage_users");
					
			$user_lookup = $this->SQL->fetchRow("SELECT * 
				FROM users 
				WHERE id = '%s'", array(
					$uid
				));
			
			if($user_lookup == false)
				return new ReturnData(false, "user_info_invalid_user");
		}
		
		return new ReturnData(false, null, $user_lookup);
	}
}
?>