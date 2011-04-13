<?php
class User {
	private $_USER, $_CONFIG, $_QUERY;

	public function User(&$user, &$config, $query) {
		$this->_USER = &$user;
		$this->_CONFIG = &$config;
		$this->_QUERY = $query;
	}

	public function verify(){
		validateUser();
		
		callClientMethod("validation_successful");
	}

	public function register(){
		if(count($_GET["args"]) == 3){

			list($username, $password, $email) = $_GET["args"];
			// Validation

			if(strlen($username) < 3){
				callClientMethod("registration_username_short");
			}
			if(strlen($username) > 15){
				callClientMethod("registration_username_long");
			}
			if(!preg_match("/^[a-zA-Z0-9_-]*$/", $username)){
				callClientMethod("registration_username_invalid");
			}
			if(!preg_match("/^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$/i", $email)){
				callClientMethod("registration_email_invalid");
			}

			// Check to see if the user is the first user registered.  If so, make him an admin.
			$is_registered_users = sqlQuery("SELECT `id` FROM `users` LIMIT 1;", null, "has_rows");

			if($is_registered_users){
				$this->create($username, $password, $email);
			}else{
				$this->create($username, $password, $email, 0);
			}
		}
	}

	public function info(){
		validateUser();
		//callClientMethod("user_info", $this->_USER);

		callClientMethod("user_info", array(
			"id" => $this->_USER["id"],
			"username" => $this->_USER["username"],
			"registration_date" => $this->_USER["registration_date"],
			//"email" => $this->_USER["email"],
			"total_files_uploaded" => $this->_USER["total_files_uploaded"],
			"total_uploaded_filesizes" => $this->_USER["total_uploaded_filesizes"],
			"max_upload_space" => getPermission("max_upload_space"),
			"max_upload_size" => getPermission("max_upload_size"),
			"upload_base_url" => $this->_CONFIG["upload_base_url"]
		));
	}

	public function logout(){
		validateUser();
		
		$logged_out = sqlQuery("DELETE FROM `sessions`
			WHERE `session` = '%s'
			LIMIT 1;", array($this->_USER["session"]), "successful");

		if($logged_out){
			callClientMethod("logout_successful");
		}else{
			callClientMethod("logout_failed");
		}
	}

	private function create($username, $password, $email, $permissions = 1){

		$existing_user = sqlQuery("SELECT * FROM `users`
			WHERE `username` = '%s'
			LIMIT 1;", array($username), "assoc");

		if(count($existing_user) > 0){
			callClientMethod("registration_username_existing");
		}

		$existing_email = sqlQuery("SELECT * FROM `users`
			WHERE `email` = '%s'
			LIMIT 1;", array($email), "assoc");

		if(count($existing_email) > 0){
			callClientMethod("registration_email_existing");
		}

		$successful = sqlInsert("users", array(
			"id" => null,
			"username" => $username,
			"password" => $password,
			"registration_date" => "CURDATE()",
			"email" => $email,
			"permissions" => $permissions
		));

		if($successful == 1){
			callClientMethod("registration_success_activated");

		}else{
			callClientMethod("registration_failure_server");
		}
	}
}

?>