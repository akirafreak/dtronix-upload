<?php

function _verifyUser(){
	callClientMethod("validation_successful");
}

function userCreate($username, $password, $email, $permissions = 1){
	global $_CONFIG;
	
	$existing_user = mysqlQuery("SELECT * FROM `users` WHERE
		`username` = '%s'
		LIMIT 1;", array($username), "assoc");

	if(count($existing_user) > 0){
		callClientMethod("registration_username_existing");
	}

	$existing_email = mysqlQuery("SELECT * FROM `users` WHERE
		`email` = '%s'
		LIMIT 1;", array($email), "assoc");

	if(count($existing_email) > 0){
		callClientMethod("registration_email_existing");
	}

	$successful = myInsert("users", array(
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

function _registerAccount(){
	if(count($_GET["args"]) == 3){
		$username = $_GET["args"][0];
		$password = $_GET["args"][1]; // Already MD5 hashed.
		$email = $_GET["args"][2];
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
		
		userCreate($username, $password, $email);
	}
}

function _getUserInfo(){
	global $_USER, $_CONFIG;

	callClientMethod("user_info", array(
		"id" => $_USER["id"],
		"username" => $_USER["username"],
		"registration_date" => $_USER["registration_date"],
		//"email" => $_USER["email"],
		"total_files_uploaded" => $_USER["total_files_uploaded"],
		"total_uploaded_filesizes" => $_USER["total_uploaded_filesizes"],
		"max_upload_space" => getPermission("max_upload_space"),
		"max_upload_size" => getPermission("max_upload_size"),
		"upload_base_url" => $_CONFIG["upload_base_url"]
	));
}

function _logout(){
	global $_USER;

	$logged_out = myUpdate("users", array(
		"session_key" => ""
	), "`id` = " . $_USER["id"]);

	if($logged_out){
		callClientMethod("logout_successful");
	}else{
		callClientMethod("logout_failed");
	}
}


?>