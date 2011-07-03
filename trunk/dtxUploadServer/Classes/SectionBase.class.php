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
	 */
	protected function validateConnection($required = true){
		if(array_key_exists("session", $this->USER))
			return true;
		
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
				// Return the current session
				returnClientData("validation_successful", $session_key);
			}
			
			
		}elseif(isset($_COOKIE["session_key"])){
			// User is attempting to use an exsiting session.
			if(empty($_COOKIE["session_key"])){
				$this->deleteCookies();
				if($required){
					returnClientData("validation_invalid_user_session");

				}else{
					return false;
				}
			}
			
			$user_session = $this->SQL->fetchRow("SELECT *
				FROM active_sessions
				WHERE session = '%s'", array($_COOKIE["session_key"]));
			
			if($user_session == false){
				$this->deleteCookies();
				if($required){
					returnClientData("validation_invalid_user_session");

				}else{
					return false;
				} 
			}

			// Check to see if the session has expired
			if($user_session["last_active"] + $this->CONFIG["session_max_life"] < time()){
				$this->SQL->query("DELETE FROM sessions
					WHERE session = '%s'", array($_COOKIE["session_key"]));

				$this->deleteCookies();
				if($required){
					returnClientData("validation_expired_user_session");

				}else{
					return false;
				}
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
			$this->checkConnectionPermissions();
		}else{
			
			if($required){
				returnClientData("validation_failed_no_login");

			}else{
				return false;
			}
		}
	}
	
	/**
	 * Removes all server created cookies on the client.
	 */
	private function deleteCookies(){
		setcookie("client_username", "", time() - 60 * 60 * 24);
		setcookie("client_password", "", time() - 60 * 60 * 24);
		setcookie("session_key", "", time() - 60 * 60 * 24);
	}
	
	/**
	 * Private method to ensure the client is allowed to connect.
	 */
	private function checkConnectionPermissions(){
		if(!$this->getPermission("can_connect")){
			$this->deleteCookies();
			returnClientData("validation_user_connection_dissabled");
		}
		
		if($this->getPermission("is_disabled")){
			$this->deleteCookies();
			returnClientData("validation_account_dissabled");
		}
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
	 * @return mixed True if the permission is set to 1, false if it is set to 0, mixed for all other values.
	 */
	protected function getPermission($permission_check, $uid = false){
		$this->validateConnection();
		
		// Ensure that the requested User ID does not belong to the current user.
		if($uid != false && $this->USER["id"] != $uid){
			//Verify that the connected user can manage other peoples info.
			if(!$this->getPermission("manage_users"))
				returnClientData("validation_manage_users");
			
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
				returnClientData("validation_error");
		}
		
		if(isset($this->PERMISSIONS[$uid][$permission_check])){
			if($this->PERMISSIONS[$uid][$permission_check] == 1){
				return true;

			}else if($this->PERMISSIONS[$uid][$permission_check] == 0){
				return false;

			}else{
				return $this->PERMISSIONS[$uid][$permission_check];
			}
		}
		return null;
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
	 */
	protected function saveNotification($type, $user_id, $data){
		$this->SQL->insert("notifications", array(
			"user_id" => $user_id,
			"type" => $type,
			"data" => json_encode($data),
			"time" => time()
		));
	}
}
?>