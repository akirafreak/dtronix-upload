<?php

/**
 * Base class to handle setup of section classes and common methods that they use.
 */
Class SectionBase{

	/**
	 * @var array Associative array that contains the entire user row from the database.
	 */
	protected $_USER;

	/**
	 * @var array Associative array that contains the entire config.php array.
	 */
	protected $_CONFIG;
	
	/**
	 * @deprecated Depreciated for passing the query information via method parameters.
	 * @var array Array that contains $_GET["args"] contents.
	 */
	protected $_QUERY;

	/**
	 * Internal variable.  Not to be accessed by external methods.
	 * 
	 * This is an assocative array that contains one or more user permission sets.<BR>
	 * The key of the array is the User ID.  The current User ID is used to access<BR>
	 * the current users permission set.
	 *
	 * @var array Variable that contains the user's and others us current permission set.
	 */
	private $_PERMISSIONS = array();

	/**
	 * @var MySQL Reference to the active MySQL server.
	 */
	protected $_SQL;

	public function SectionBase(){
	}

	/**
	 * Method to assign variables their values.
	 */
	public function setVariables(&$user, &$config, $query, &$sql_server){
		$this->_USER = &$user;
		$this->_CONFIG = &$config;
		$this->_QUERY = $query; // No need to mess with the original $_GET.
		$this->_SQL = &$sql_server;
	}

	/**
	 * Method to control the user's login.
	 *
	 * @param bool $required True if the script should halt on user not being logged in. <BR>
	 * False if the script should continue even if the user is not logged in.
	 */
	protected function validateUser($required = true){
		
		if(array_key_exists("session", $this->_USER))
			return true;
		
		if(isset($_COOKIE["client_username"]) && isset($_COOKIE["client_password"])){
			// User is tring to login.

			$user_row = $this->_SQL->queryFetchRow("SELECT *
				FROM `users`
				WHERE `username` = '%s'
				LIMIT 1;", array($_COOKIE["client_username"]));

			if($user_row == false)
				callClientMethod("validation_invalid_username");

			$pass1 = strtoupper($user_row["password"]);
			$pass2 = strtoupper(md5($_COOKIE["client_password"])); // Hash the hashed password again.

			// Validate the user's password.
			if($pass1 != $pass2)
				callClientMethod("validation_invalid_password");
			
			

			$session_key = createHash(32, true);

			// Limit the total number of logins on one account to 5.
			$this->_SQL->insert("sessions", array(
				"session" => $session_key,
				"user_id" => $user_row["id"],
				"last_active" => MySQL::func("NOW()"),
				"date_created" => MySQL::func("CURDATE()")
			));
			

			$all_user_sessions = $this->_SQL->queryFetchRows("SELECT `session`
				FROM `sessions`
				WHERE `user_id` = '%s'
				ORDER BY `last_active` DESC
				LIMIT 6;", array($user_row["id"]));

			if($all_user_sessions == false){
				errorHandler(0, "Server did not create a user session.  Please contact support.", __FILE__, __LINE__);
			}

			// If the user has too many sessions, delete the one last accessed.
			if(count($all_user_sessions) > 5){
				$this->_SQL->query("DELETE FROM `sessions`
					WHERE `session` = '%s'
					LIMIT 1;", array(
						$all_user_sessions[5]["session"]
					));
			}

			// Update the client cookies.
			$this->deleteCookies();
			setcookie("session_key", $session_key, time() + (60 * 60 * 24 * 90));

			// TODO: Check to see if the user is admin before not allowing him in.
			if($this->_CONFIG["server_maintenance_mode"]){
				callClientMethod("maintenance_mode");

			}else{
				// Return the current session
				callClientMethod("validation_successful", $session_key);
			}
			
			
		}elseif(isset($_COOKIE["session_key"])){
			
			// User is attempting to use an exsiting session.
			if(empty($_COOKIE["session_key"])){
				$this->deleteCookies();
				if($required){
					callClientMethod("validation_invalid_user_session");

				}else{
					return false;
				}
			}
			
			$user_session = $this->_SQL->queryFetchRow("SELECT `user_id`, `last_active`
				FROM `sessions`
				WHERE `session` = '%s'
				ORDER BY `last_active` DESC
				LIMIT 1;", array($_COOKIE["session_key"]));

			if($user_session == false){
				$this->deleteCookies();
				if($required){
					callClientMethod("validation_invalid_user_session");

				}else{
					return false;
				}
			}
			
			$last_active = strtotime($user_session["last_active"]);

			// Check to see if the session has expired
			if($last_active + $this->_CONFIG["session_max_life"] < time()){
				$this->_SQL->query("DELETE FROM `sessions`
					WHERE `session` = '%s'
					LIMIT 1;", array($_COOKIE["session_key"]));

				$this->deleteCookies();
				if($required){
					callClientMethod("validation_expired_user_session");

				}else{
					return false;
				}
			}
			

			$this->_SQL->updateSafe("sessions", array(
				"last_active" => MySQL::func("NOW()")
			), "WHERE session = '%s'", array(
				$_COOKIE["session_key"]
			));

			$user_row = $this->_SQL->queryFetchRow("SELECT *
				FROM `users`
				WHERE `id` = '%s'
				LIMIT 1", array($user_session["user_id"]));

			if($this->_CONFIG["server_maintenance_mode"]){
				// Check to see if the user is an admin.  If so, then let him on in.
				if($user_row["permissions"] != 0)
					callClientMethod("maintenance_mode");

			}else{
				$this->_USER = array_merge($this->_USER, $user_row);
				$this->_USER["session"] = $_COOKIE["session_key"];
			}
			
			// Ensure the user is not banned and other connection verifications.
			$this->checkConnectionPermissions();
		}else{
			
			if($required){
				callClientMethod("validation_failed_no_login");

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
			callClientMethod("validation_user_connection_dissabled");
		}
		
		if($this->getPermission("is_disabled")){
			$this->deleteCookies();
			callClientMethod("validation_account_dissabled");
		}
	}

	/**
	 * Verifies the current user's permission.
	 * 
	 * Stores the information in the reference variable $_PERMISSIONS.  If the <BR>
	 * User ID parameter is specified, the current user's `manage_user` permission is verified. <BR>
	 * If the check fails, a permission error is thrown to the client.
	 *
	 * @param string $permission_check Permission to verify.
	 * @param int $uid User ID of the account to check the permission of.
	 * @return mixed True if the permission is set to 1, false if it is set to 0, mixed for all other values.
	 */
	protected function getPermission($permission_check, $uid = false){
		$this->validateUser();
		
		// Ensure that the requested User ID does not belong to the current user.
		if($uid != false && $this->_USER["id"] != $uid){
			//Verify that the connected user can manage other peoples info.
			if(!$this->getPermission("manage_users"))
				callClientMethod("validation_manage_users");
			
			$permission_level = $this->_SQL->queryFetchRow("SELECT `permissions`
				FROM `users`
				WHERE `id` = '%s'
				LIMIT 1", array(
					$uid
				));
			$permission_level = $permission_level["permissions"];
			
		}else{
			$uid = $this->_USER["id"];
			$permission_level = $this->_USER["permissions"];
		}

		if(!array_key_exists($uid, $this->_PERMISSIONS)){
			$this->_PERMISSIONS[$uid] = $this->_SQL->queryFetchRow("SELECT *
				FROM `users_permissions`
				WHERE `id` = '%s'
				LIMIT 1;",
				array(
					$permission_level
				));
			
			if($this->_PERMISSIONS[$uid] == false)
				callClientMethod("validation_error");
		}
		
		if(isset($this->_PERMISSIONS[$uid][$permission_check])){
			if($this->_PERMISSIONS[$uid][$permission_check] == 1){
				return true;

			}else if($this->_PERMISSIONS[$uid][$permission_check] == 0){
				return false;

			}else{
				return $this->_PERMISSIONS[$uid][$permission_check];
			}
		}
		return null;
	}
}
?>
