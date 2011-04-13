<?php
set_error_handler("errorHandler"); 

/*          ------------------------------------[CORE Functions]------------------------------------          */

function sqlQuery($query, $values = null, $type = "assoc"){
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

	// Debugging purposes only.
	if($type == "view"){
		echo "QUERY: ". $query . "\n\n<BR><BR>";
		$query_result = mysql_query($query) or die(mysql_error());
		while ($row = mysql_fetch_assoc($query_result)){
			print_r($row);
			echo "\n\n<BR><BR>";
		}
		die("\n\n<BR><BR>END QUERY");
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
	}else if($type == "has_rows"){
		$count = mysql_num_rows($query_result);
		@mysql_free_result($query_result);
		return ($count > 0)? true : false;

	}else if($type == "count"){
		$count = mysql_num_rows($query_result);
		@mysql_free_result($query_result);
		return $count;

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
function sqlInsert($table, $name_values, $verbose = false){
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
	
	return call_user_func_array('sqlQuery', array(
		implode("", array_merge($query_builder, $values_builder)),
		null,
		($verbose)? "view" : "successful"
	));
}

function sqlUpdate($table, $name_values, $where, $verbose = false){
	$query_builder = array("UPDATE `", $table, "`", " SET ");
	
	foreach($name_values as $key => $value){
		$query_builder[] = "`";
		$query_builder[] = $key;
		$query_builder[] = "` = ";
		$query_builder[] = myParseValue($value);
		$query_builder[] = ",";
	}

	array_pop($query_builder);
	
	$query_builder[] = " WHERE ";
	$query_builder[] = $where;
	
	return call_user_func_array('sqlQuery', array(
		implode("", $query_builder),
		null,
		($verbose)? "view" : "successful"
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




function validateUser($required = true){
	global $_USER, $_CONFIG;

	if(isset($_COOKIE["client_username"]) && isset($_COOKIE["client_password"])){
		// User is tring to login.

		$user_session = sqlQuery("SELECT *
			FROM `users`
			WHERE `username` = '%s'
			LIMIT 1", array($_COOKIE["client_username"]), "assoc");

		if(!empty($user_session)){
			$user = $user_session[0];
			$pass1 = strtoupper($user["password"]);
			$pass2 = strtoupper($_COOKIE["client_password"]);
			if($pass1 == $pass2){
				// User has a valid username and password.

				// TODO: Generate a completely random hash.
				$session_key = md5(microtime());

				// Limit the total number of logins on one account to 5.
				sqlInsert("sessions", array(
					"session" => $session_key,
					"user_id" => $user["id"],
					"last_active" => "NOW()",
					"date_created" => "CURDATE()"
				));

				$all_user_sessions = sqlQuery("SELECT `session`
					FROM `sessions`
					WHERE `user_id` = %s
					ORDER BY `last_active` DESC
					LIMIT 6", array($user["id"]));

				// If the user has too many sessions, delete the one last accessed.
				if(count($all_user_sessions) > 5){
					sqlQuery("DELETE FROM `sessions`
						WHERE `session` = '%s'
						LIMIT 1;", array($all_user_sessions[5]["session"]), "successful");
				}

				// Update the client cookies.
				setcookie("client_username", "", time() - 60 * 60 * 24);
				setcookie("client_password", "", time() - 60 * 60 * 24);
				setcookie("session_key", $session_key, time() + (60 * 60 * 24 * 90));



				// TODO: Check to see if the user is admin before not allowing him in.
				if($_CONFIG["server_maintenance_mode"]){
					callClientMethod("maintenance_moode");

				}else{
					// Return the current session
					callClientMethod("validation_successful", $session_key);
				}
				

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
			setcookie("session_key", $_COOKIE["session_key"], time() - (60 * 60 * 24 * 90));
			callClientMethod("validation_invalid_user_session");
		}

		$user_session = sqlQuery("SELECT `user_id`, `last_active`
			FROM `sessions`
			WHERE `session` = '%s'
			ORDER BY `last_active` DESC
			LIMIT 1", array($_COOKIE["session_key"]));

		if(count($user_session) == 0){
			setcookie("session_key", $_COOKIE["session_key"], time() - (60 * 60 * 24 * 90));
			callClientMethod("validation_invalid_user_session");
		}

		$last_active = strtotime($user_session[0]["last_active"]);

		// Check to see if the session has expired
		if($last_active + $_CONFIG["session_max_life"] < time()){
			sqlQuery("DELETE FROM `sessions`
				WHERE `session` = '%s'
				LIMIT 1;", array($_COOKIE["session_key"]));

			setcookie("session_key", $_COOKIE["session_key"], time() - (60 * 60 * 24 * 90));
			callClientMethod("validation_expired_user_session");
		}

		sqlUpdate("sessions", array(
			"last_active" => "NOW()"
		), "session = '". $_COOKIE["session_key"] ."'");

		$user_data = sqlQuery("SELECT *
			FROM `users`
			WHERE `id` = '%s'
			LIMIT 1", array($user_session[0]["user_id"]), "assoc");

		if($_CONFIG["server_maintenance_mode"]){
			setcookie("session_key", $_COOKIE["session_key"], time() - (60 * 60 * 24 * 90));
			callClientMethod("maintenance_moode");

		}else{
			$user = $user_data[0];
			$_USER = array_merge($_USER, $user);
			$_USER["session"] = $_COOKIE["session_key"];
		}
	}else{
		callClientMethod("validation_failed_no_login");
	}
}


function getPermission($permission_check){
	global $_USER, $_PERMISSIONS;

	if(count($_PERMISSIONS) == 0){
		$perms = sqlQuery("SELECT *
			FROM `users_permissions`
			WHERE `id` = %s
			LIMIT 1;",
			array($_USER["permissions"]));
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

function saveConfigFile($config_data){
	$new_config = "<?php
// Check to see if this file is included into the main file for security reasons.
if( !defined(\"requireParrent\") ) die(\"Restricted Access\");\n\n";

	foreach($config_data as $key => $value){
		if(is_numeric($value)){
			$value_string = $value;
		}elseif(is_bool($value)){
			$value_string = ($value)? "true": "false";
		}else{
			$value_string = "'". $value ."'";
		}
		$new_config .= '$_CONFIG["'. $key .'"] = '. $value_string .';'. "\n";
	}
	$new_config .= "\n?>";
	return (file_put_contents("config.php", $new_config) === false)? false : true;
}

function array_keys_exists($array, $keys, $allowed_empty = true) {
    foreach($keys as $k) {
        if(!isset($array[$k])) {
			return false;
        }elseif(!$allowed_empty && isset($array[$k]) && empty($array[$k])){
			return false;
		}
    }
    return true;
}

?>