<?php

function performRuntimeChecks(){
	
	// This removes the slashes in the request stream that PHP put in.
	// TODO: Figure out a way to let the user know that this needs to be configured properly.
	if(get_magic_quotes_gpc()){
		$process = array(&$_GET, &$_POST, &$_COOKIE, &$_REQUEST);
		while (list($key, $val) = each($process)) {
			foreach ($val as $k => $v) {
				unset($process[$key][$k]);
				if (is_array($v)) {
					$process[$key][stripslashes($k)] = $v;
					$process[] = &$process[$key][stripslashes($k)];
				} else {
					$process[$key][stripslashes($k)] = stripslashes($v);
				}
			}
		}
		unset($process);
	}
}

/**
 * Function to handle the sending of data to the connected client.
 *
 * @global array $USER Variable that contains all the current user information.
 * @param string $client_method Method to call on the connected client.
 * @param mixed $send_data Data to be sent to the client.
 */
function returnClientData($client_method, $send_data = false){
	global $USER;
	header("Call-Client-Method: ". $client_method);
	die(json_encode($send_data));
}

/**
 * Error handler to send the error to the connected client.
 *
 * @param int $number PHP error number
 * 
 * @param string $text Text of the exact error.
 * @param string $file File the error occured in.
 * @param string $line Line number that the error occured on.
 * @param string $raw_error HTML error text.
 * @return bool true on successful handling of the error.
 */
function errorHandler($number, $text, $file, $line, $raw_error = null){
		//if($errno == E_STRICT) return false;
		//if($errno == E_WARNING) return false;
		returnClientData("server_error", array(
			"error_type" => $number,
			"error_info" => $text,
			"error_file" => $file,
			"error_line" => $line,
			"raw_error" => $raw_error
		));

    return true;
}

/**
 * Function to handle the clean shutdown of the script.
 *
 * Will also handle fatal errors and encode them in JSON format and send them to the client.
 *
 * @global array $USER Variable that contains all the current user information.
 */
function shutdownFunction(){
	global $USER;
	
	if($USER["client"] == 1){
		if(($error = error_get_last())){
			// Make sure that we are not erasing anything important...
			$raw = ob_get_contents();
			ob_end_clean();
			ob_start("ob_gzhandler");
			errorHandler($error["type"], $error["message"], $error["file"], $error["line"], $raw);
		}
	}
	ob_flush();
}

/**
 * Function to save the configuration array passed.
 *
 * @param array $config_array Associtive array that contains all the configuration information.
 * @return bool True on successful saving of config file, false on failure.
 */
function saveConfigFile($config_array){
	$new_config = "<?php
// Check to see if this file is included into the main file for security reasons.
if( !defined(\"requireParrent\") ) die(\"Restricted Access\");\n\n";

	foreach($config_array as $key => $value){
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


/**
 * Function to validate that all the keys exist inside the passed array.
 *
 * @param array $array Input array to validate.
 * @param array $keys Array of keys to ensure exist inside the input array.
 * @param bool $allowed_empty True if empty values are valid, False if should return false on an empty
 * @return bool True if all the keys exist, false if one of the keys does not exist.
 */
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


/**
 * Creates a random hash with specified length.
 * 
 * @param int $length Length of string to return.
 * @param bool $uppercase True: Allow uppercase characters to be present in the hash.  False: Just use lowercase letters and numbers.
 * @return string Hash with specified length.
 */
function createHash($length, $uppercase = false){
	$max_value = 35;
	$char_string = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXY";
	$hash_buffer = "";
	
	if($uppercase) {
		$max_value = strlen($char_string) - 1;
	}
	
	for($i = 0; $i < $length; $i++) {
		$hash_buffer .= $char_string[rand(0, $max_value)];
	}
	
	return $hash_buffer;
}

?>