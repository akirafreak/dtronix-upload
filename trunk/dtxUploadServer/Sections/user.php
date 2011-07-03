<?php
class User extends SectionBase{

	/**
	 * Constructor for user class.
	 */
	public function User(){
	}

	/**
	 * Validate that the user is logged in.
	 */
	public function verify(){
		$this->validateConnection();
		
		returnClientData("validation_successful");
	}

	
	/**
	 * Verifies user submitted information and registers an account on the server.
	 * 
	 * @param string $username User requested username.
	 * @param string $password MD5 Hashed password.
	 * @param string $email User email address.
	 */
	public function register($username, $password, $email){
		if($username == null || $password == null || $email == null)
			returnClientData("registration_failure_server");

		if(strlen($username) < 3)
			returnClientData("registration_username_short");

		if(strlen($username) > 15)
			returnClientData("registration_username_long");
		
		if(!preg_match("/^[a-zA-Z0-9_-]*$/", $username))
			returnClientData("registration_username_invalid");
		
		if(!preg_match("/^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$/i", $email))
			returnClientData("registration_email_invalid");

		// Check to see if the user is the first user registered.  If so, make him an admin.
		$is_registered_users = $this->SQL->hasRows("SELECT `id`
			FROM `users`
			LIMIT 1;");

		if($is_registered_users){
			$this->create($username, $password, $email);
			
		}else{
			$this->create($username, $password, $email, 0);
		}
	}
	
	/**
	 * Method to save settins server-side in a JSON array.
	 * 
	 * @param string $name Key of the name value pairing.
	 * @param string $value Value to set for the name.
	 * @param int $uid User ID of the account to modify.  The current user must have the `manage_users` permission.
	 */
	public function setSetting($name, $value, $uid = false){
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
		), "WHERE `id` = '%s'", array(
			$account["id"]
		));

		if($successful){
			returnClientData("user_setting_set_successful");
		}else{
			returnClientData("user_setting_set_failure");
		}
	}
	
	/**
	 * Retrieve a user setting saved in a JSON array.
	 * 
	 * @param string $name Key of the value to retrieve.
	 * @param int $uid User ID of the account to modify.  The current user must have the `manage_users` permission.
	 */
	public function getSetting($name, $uid = false){
		$this->validateConnection();
		$settings = array();
		$account = $this->getAccount($uid);

		// Ensure that the data is not empty.
		if(!empty($account["settings"])){
			$settings = json_decode($account["settings"]);
		}

		if($successful){
			returnClientData("user_setting_get_successful", array($name, $settings[$name]));
		}else{
			returnClientData("user_setting_get_failure");
		}
	}
	
	/**
	 * Change a user's password.
	 * 
	 * @param string $new_password Password to set the current user's password.  Password must already be MD5 hashed.
	 * @param int $uid User ID of the account to modify.  The current user must have the `manage_users` permission.
	 */
	public function changePassword($new_password, $uid = false){
		$this->validateConnection();
		$settings = array();
		$account = $this->getAccount($uid);
		if(strlen($new_password) < 16)
			returnClientData("invalid_request");
		
		$successful = $this->SQL->update("users", array(
			"password" => md5($new_password)
		), "WHERE `id` = '%s'", array(
			$account["id"]
		));
		
		if($successful){
			returnClientData("user_change_password_successful", array($name, $settings[$name]));
		}else{
			returnClientData("user_change_password_failure");
		}
	}
	
	/**
	 * Delete a user account.
	 * 
	 * @param int $uid User ID of the account to modify.  The current user must have the `manage_users` permission if deleting another account.
	 */
	public function deleteAccount($uid = false){
		returnClientData("method_not_implemented", array(__CLASS__, __FUNCTION__));
	}
	
	/**
	 * Delete a user account.
	 * 
	 * @param int $uid User ID of the account to delete and remove all uploads of.
	 * @param int $uid User ID of the account to modify.  The current user must have the `manage_users` permission.
	 */
	public function banAccount($length, $uid = false){
		returnClientData("method_not_implemented", array(__CLASS__, __FUNCTION__));
	}

	/**
	 * Method to retrieve user information.
	 * 
	 * If user is logged in and can manage users, then the user can get other users information.
	 * If not, then this method will only retrieve the current user's info.
	 * 
	 * @param int $uid User ID of the account to retrieve the info of.  The current user must have the `manage_users` permission.
	 */
	public function info($uid = false){
		$this->validateConnection();
		
		// Retrieve the requested user's account information.
		$account = $this->getAccount($uid);

		returnClientData("user_info", array(
			"id" => $account["id"],
			"username" => $account["username"],
			"registration_date" => $account["registration_date"],
			"email" => $this->USER["email"],
			"total_files_uploaded" => $account["total_files_uploaded"],
			"total_uploaded_filesizes" => $account["total_uploaded_filesizes"],
			"max_upload_space" => $this->getPermission("max_upload_space", $account["id"]),
			"max_upload_size" => $this->getPermission("max_upload_size", $account["id"]),
			"upload_base_url" => $this->CONFIG["upload_base_url"]
		));
	}
	
	/**
	 * Method to logout the current user and remove the session from the active session list.
	 */
	public function logout(){
		$this->validateConnection();
		
		$logged_out = $this->SQL->successful("DELETE FROM `sessions`
			WHERE `session` = '%s'
			LIMIT 1;", array($this->USER["session"]));

		if($logged_out){
			returnClientData("logout_successful");
		}else{
			returnClientData("logout_failed");
		}
	}
	
	/**
	 * Internal method to create a user and add the associated information to the database.
	 * 
	 * Note: The password should already be hashed by the MD5 algorithm before sent<BR>
	 * to this method. This method re-hashes the password after receiving the password.<BR>
	 * So the password is hashed twice before being stored in the database.
	 *
	 * @param type $username Username to resiter. 
	 * @param type $password Hashed password in MD5 format.
	 * @param type $email Email address to register. NOTE: Does not verify validity of the email address.
	 * @param type $permissions Permission level to set the user at. Corrisponds to `users_permissions`.
	 */
	private function create($username, $password, $email, $permissions = 1){

		$existing_user = $this->SQL->hasRows("SELECT * FROM `users`
			WHERE `username` = '%s'
			LIMIT 1;", array($username));

		if($existing_user != false){
			returnClientData("registration_username_existing");
		}

		$existing_email = $this->SQL->hasRows("SELECT * FROM `users`
			WHERE `email` = '%s'
			LIMIT 1;", array($email));

		if($existing_email != false){
			returnClientData("registration_email_existing");
		}

		$successful = $this->SQL->insert("users", array(
			"id" => null,
			"username" => $username,
			"password" => md5($password),
			"registration_date" => time(),
			"email" => $email,
			"permissions" => $permissions
		));

		if($successful == 1){
			returnClientData("registration_success_activated");

		}else{
			returnClientData("registration_failure_server");
		}
	}
	
	/**
	 * Internal method that retrieves the account that is associated with the User ID.
	 * 
	 * Checks permissions if the current user is attempting to access another user's account.
	 * User must have the `manage_users` permission set to true.
	 * 
	 * @param int $uid false to retrieve the current user's account.
	 */
	private function getAccount($uid){
		if($uid == false || $this->USER["id"] == $uid){
			$user_lookup = $this->USER;
			
		}else{
			if(!$this->getPermission("manage_users"))
				returnClientData("validation_manage_users");
					
			$user_lookup = $this->SQL->fetchRow("SELECT * 
				FROM `users` 
				WHERE `id` = '%s' 
				LIMIT 1;", array(
					$uid
				));
			
			if($user_lookup == false)
				returnClientData("user_info_invalid_user");
		}
		
		return $user_lookup;
	}
}

?>