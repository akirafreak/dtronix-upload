<?php

// Check to see if this file is included into the main file for security reasons.
if( !defined("requireParrent") ) die("Restricted Access");

set_error_handler("errorHandler"); 

/*          ------------------------------------[CORE Functions]------------------------------------          */

function mysqlQuery($query, $values = null, $type = "assoc"){
	global $_CONFIG;
	
	if( empty($query) ) return false;
	if($_CONFIG["mysql_id"] == false){
 		$_CONFIG["mysql_id"] = mysql_connect($_CONFIG["mysql_server"], $_CONFIG["mysql_user"], $_CONFIG["mysql_password"]) or callClientMethod("server_error_mysql", mysql_error());
 		mysql_select_db($_CONFIG["mysql_database"]) or die("Could not select database");
	}

	if($values !== null && !empty($values)){
		foreach($values as $value){
			$value = mysql_real_escape_string($value);
		}
		$query = vsprintf($query, $values);
	}

	if($type == "view"){
		echo $query;
		die();
	}
	
	$query_result = mysql_query($query) or callClientMethod("server_error_mysql", mysql_error());
	$affected_rows = mysql_affected_rows();
	
	if($type == "assoc"){
		while ($row = mysql_fetch_assoc($query_result)){
			$return[] = $row;
		}
		@mysql_free_result($query_result);
		
		if(!isset($return)) return array();
		return $return;
		
	}else if($type == "query"){
		return $query_result;
		
	}else if($type == "affected"){
		@mysql_free_result($query_result);
		return $affected_rows;
		
	}else if($type == "successful"){
		@mysql_free_result($query_result);
		if($affected_rows > 0){
			return true;
		}else{
			return false;
		}
		
	}else{
		return $query_result;
	}
}



/**
 * @param string Table Name
 * @param array Table Column Name as Key, Value as Value.
 * @return True on success; False on failure.
 * 
 */
function myInsert($table, $name_values){
	$query_builder = array("INSERT INTO `", $table, "`", " (");
	$values_builder = array(") VALUES (");
	
	foreach($name_values as $key => $value){
		$query_builder[] = "`";
		$query_builder[] = $key;
		$query_builder[] = "`";
		$query_builder[] = ",";

		$values_builder[] = myParseValue($value);
		$values_builder[] = ",";
	}

	
	//Drop the last comma off the end of the array. (Love the name of this function. haha.)
	array_pop($values_builder);
	array_pop($query_builder);
	$values_builder[] = ")";
	
	return call_user_func_array('mysqlQuery', array(
		implode("", array_merge($query_builder, $values_builder)),
		null,
		"successful"
	));
}

function myUpdate($table, $name_values, $where){
	$query_builder = array("UPDATE `", $table, "`", " SET ");
	
	foreach($name_values as $key => $value){
		$query_builder[] = "`";
		$query_builder[] = $key;
		$query_builder[] = "` = ";
		$query_builder[] = myParseValue($value);
		$query_builder[] = ",";
	}

	array_pop($query_builder);
	
	$query_builder[] = "WHERE ";
	$query_builder[] = $where;
	
	return call_user_func_array('mysqlQuery', array(
		implode("", $query_builder),
		null,
		"successful"
	));
}

function myParseValue($value){
	$valid_mysql_methods = array("CURTIME()", "CURDATE()", "NOW()", "YEAR()", "TIMESTAMP()");

	if($value === null){
		return "NULL";
		
	}elseif(in_array($value, $valid_mysql_methods)){
		
		// Let the user use a MySQL function if it is valid.
		return $value;
		
	}else{
		return "'" . mysql_real_escape_string($value) . "'";
	}	
}


function callClientMethod($client_function, $array = null){
	global $_USER;
	
	if($_USER["client"] == 1){ // The connected client is the dtxUpload Program. Output call request.
		
		$func_length = strlen($client_function);
		if($func_length < 1) return false;

		// Standard packet.
		// Function String Length; Function String; Function Argument (JSON);
		echo str_pad($func_length, 3);
		echo $client_function;
		if(is_array($array)){
			die(json_encode($array));
		}else{
			die($array);
		}

	}elseif($_USER["client"] == 2){ // The connected client is a Web Client. Output JUST JSON.

		$output = array(
			"function" => $client_function,
			"data" => $array
		);
		die(json_encode($output));
	}

}

function errorHandler($errno, $errstr, $errfile, $errline){
		if($errno == E_STRICT) return false;
		if($errno == E_WARNING) return false;
		callClientMethod("error_server", array(
			"error_type" => $errno,
			"error_info" => $errstr,
			"error_file" => $errfile,
			"error_line" => $errline
		));

    return true;
}

/*          ------------------------------------[USER FUNCTIONS]------------------------------------          */

function validateUser(){
	global $_USER, $_CONFIG;
	if(isset($_COOKIE["client_username"]) && isset($_COOKIE["client_password"])){
		// User is tring to login.
		
		$user_session = mysqlQuery("SELECT *
			FROM `users`
			WHERE `username` = '%s'
			LIMIT 1", array($_COOKIE["client_username"]), "assoc");
		
		if(!empty($user_session)){
			$user = $user_session[0];
			$pass1 = strtoupper($user["password"]);
			$pass2 = strtoupper($_COOKIE["client_password"]);
			if($pass1 == $pass2){
				// User has a valid username and password.
				
				$session_key = md5(microtime());
				
				// Generate a new session id and update the user's last active time.
				mysqlQuery("UPDATE `users` 
					SET `session_key` = '%s',
					`session_last_active` = NOW() 
					WHERE `id` = %f;", array($session_key, $user["id"]), "successful");

				setcookie("client_username", "", time() - 60 * 60 * 24);
				setcookie("client_password", "", time() - 60 * 60 * 24);
				setcookie("session_key", $session_key, time() + (60 * 60 * 24 * 90));

				callClientMethod("validation_successful", $session_key);
					
			}else{
				// Invalid password.
				callClientMethod("validation_invalid_password");
			}

		}else{
			// Query could not find any rows.
			callClientMethod("validation_invalid_username");
		}
	}elseif(isset($_COOKIE["session_key"])){
		// User is attempting to use an exsiting session.

		if(empty($_COOKIE["session_key"])){
			setcookie("session_key", $session_key, time() + (60 * 60 * 24 * 90));
			callClientMethod("validation_invalid_user_session");
		}

		$user_session = mysqlQuery("SELECT *
			FROM `users`
			WHERE `session_key` = '%s'
			LIMIT 1", array($_COOKIE["session_key"]), "assoc");

		if(count($user_session) > 0){
			$user = $user_session[0];
			if(strtotime($user["session_last_active"]) + $_CONFIG["session_max_life"] < time()){

				// Seession has expired.
				setcookie("session_key", $session_key, time() + (60 * 60 * 24 * 90));
				// This will force the client to attempt to reconnect and generate a new session.
				callClientMethod("validation_expired_user_session");

			}else{
				// This is a valid session.  Let the user continue.

				// Update the user's last active time.
				mysqlQuery("UPDATE `users`
					SET `session_last_active` = NOW()
					WHERE `id` = %f;", array($user["id"]), "query");
				$_USER = array_merge($_USER, $user);
			}

		}else{
			// Query could not find any rows.
			callClientMethod("validation_invalid_user_session");
		}

	}else{
		callClientMethod("validation_failed_no_login");
	}
}

function getPermission($permission_check){
	global $_USER, $_PERMISSIONS;

	if(count($_PERMISSIONS) == 0){
		$perms = mysqlQuery("SELECT *
			FROM `users_permissions`
			WHERE `id` = %s
			LIMIT 1;", array($_USER["permissions"]));
		$_PERMISSIONS = $perms[0];
	}

	if(isset($_PERMISSIONS[$permission_check])){
		if($_PERMISSIONS[$permission_check] == 1){
			return true;
			
		}else if($_PERMISSIONS[$permission_check] == 0){
			return false;

		}else{
			return $_PERMISSIONS[$permission_check];
		}
	}

	return null;


}



?>