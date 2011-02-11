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
	file_put_contents("config.php", $new_config);
}

function array_keys_exists($array, $keys) {
    foreach($keys as $k) {
        if(!isset($array[$k])) {
        return false;
        }
    }
    return true;
}




// Common mimes for files.
$GLOBALS["mime_types"] = array(
	"323" => "text/h323",
	"7z" => "application/x-7z-compressed",
	"acx" => "application/internet-property-stream",
	"ai" => "application/postscript",
	"aif" => "audio/x-aiff",
	"aifc" => "audio/x-aiff",
	"aiff" => "audio/x-aiff",
	"asf" => "video/x-ms-asf",
	"asr" => "video/x-ms-asf",
	"asx" => "video/x-ms-asf",
	"au" => "audio/basic",
	"avi" => "video/x-msvideo",
	"axs" => "application/olescript",
	"bas" => "text/plain",
	"bcpio" => "application/x-bcpio",
	"bin" => "application/octet-stream",
	"bmp" => "image/bmp",
	"c" => "text/plain",
	"cat" => "application/vnd.ms-pkiseccat",
	"cdf" => "application/x-cdf",
	"cer" => "application/x-x509-ca-cert",
	"class" => "application/octet-stream",
	"clp" => "application/x-msclip",
	"cmx" => "image/x-cmx",
	"cod" => "image/cis-cod",
	"cpio" => "application/x-cpio",
	"crd" => "application/x-mscardfile",
	"crl" => "application/pkix-crl",
	"crt" => "application/x-x509-ca-cert",
	"csh" => "application/x-csh",
	"css" => "text/css",
	"dcr" => "application/x-director",
	"der" => "application/x-x509-ca-cert",
	"dir" => "application/x-director",
	"dll" => "application/x-msdownload",
	"dms" => "application/octet-stream",
	"doc" => "application/msword",
	"dot" => "application/msword",
	"dvi" => "application/x-dvi",
	"dxr" => "application/x-director",
	"eps" => "application/postscript",
	"etx" => "text/x-setext",
	"evy" => "application/envoy",
	"exe" => "application/octet-stream",
	"fif" => "application/fractals",
	"flr" => "x-world/x-vrml",
	"gif" => "image/gif",
	"gtar" => "application/x-gtar",
	"gz" => "application/x-gzip",
	"h" => "text/plain",
	"hdf" => "application/x-hdf",
	"hlp" => "application/winhlp",
	"hqx" => "application/mac-binhex40",
	"hta" => "application/hta",
	"htc" => "text/x-component",
	"htm" => "text/html",
	"html" => "text/html",
	"htt" => "text/webviewhtml",
	"ico" => "image/x-icon",
	"ief" => "image/ief",
	"iii" => "application/x-iphone",
	"ins" => "application/x-internet-signup",
	"isp" => "application/x-internet-signup",
	"jfif" => "image/pipeg",
	"jpe" => "image/jpeg",
	"jpeg" => "image/jpeg",
	"jpg" => "image/jpeg",
	"js" => "application/x-javascript",
	"latex" => "application/x-latex",
	"lha" => "application/octet-stream",
	"lsf" => "video/x-la-asf",
	"lsx" => "video/x-la-asf",
	"lzh" => "application/octet-stream",
	"m13" => "application/x-msmediaview",
	"m14" => "application/x-msmediaview",
	"m3u" => "audio/x-mpegurl",
	"man" => "application/x-troff-man",
	"mdb" => "application/x-msaccess",
	"me" => "application/x-troff-me",
	"mht" => "message/rfc822",
	"mhtml" => "message/rfc822",
	"mid" => "audio/mid",
	"mny" => "application/x-msmoney",
	"mov" => "video/quicktime",
	"movie" => "video/x-sgi-movie",
	"mp2" => "video/mpeg",
	"mp3" => "audio/mpeg",
	"mpa" => "video/mpeg",
	"mpe" => "video/mpeg",
	"mpeg" => "video/mpeg",
	"mpg" => "video/mpeg",
	"mpp" => "application/vnd.ms-project",
	"mpv2" => "video/mpeg",
	"ms" => "application/x-troff-ms",
	"mvb" => "application/x-msmediaview",
	"nws" => "message/rfc822",
	"oda" => "application/oda",
	"p10" => "application/pkcs10",
	"p12" => "application/x-pkcs12",
	"p7b" => "application/x-pkcs7-certificates",
	"p7c" => "application/x-pkcs7-mime",
	"p7m" => "application/x-pkcs7-mime",
	"p7r" => "application/x-pkcs7-certreqresp",
	"p7s" => "application/x-pkcs7-signature",
	"pbm" => "image/x-portable-bitmap",
	"pdf" => "application/pdf",
	"pfx" => "application/x-pkcs12",
	"pgm" => "image/x-portable-graymap",
	"pko" => "application/ynd.ms-pkipko",
	"pma" => "application/x-perfmon",
	"pmc" => "application/x-perfmon",
	"pml" => "application/x-perfmon",
	"pmr" => "application/x-perfmon",
	"pmw" => "application/x-perfmon",
	"png" => "image/png",
	"pnm" => "image/x-portable-anymap",
	"pot" => "application/vnd.ms-powerpoint",
	"ppm" => "image/x-portable-pixmap",
	"pps" => "application/vnd.ms-powerpoint",
	"ppt" => "application/vnd.ms-powerpoint",
	"prf" => "application/pics-rules",
	"ps" => "application/postscript",
	"pub" => "application/x-mspublisher",
	"qt" => "video/quicktime",
	"ra" => "audio/x-pn-realaudio",
	"ram" => "audio/x-pn-realaudio",
	"ras" => "image/x-cmu-raster",
	"rgb" => "image/x-rgb",
	"rmi" => "audio/mid",
	"roff" => "application/x-troff",
	"rtf" => "application/rtf",
	"rtx" => "text/richtext",
	"scd" => "application/x-msschedule",
	"sct" => "text/scriptlet",
	"setpay" => "application/set-payment-initiation",
	"setreg" => "application/set-registration-initiation",
	"sh" => "application/x-sh",
	"shar" => "application/x-shar",
	"sit" => "application/x-stuffit",
	"snd" => "audio/basic",
	"spc" => "application/x-pkcs7-certificates",
	"spl" => "application/futuresplash",
	"src" => "application/x-wais-source",
	"sst" => "application/vnd.ms-pkicertstore",
	"stl" => "application/vnd.ms-pkistl",
	"stm" => "text/html",
	"svg" => "image/svg+xml",
	"sv4cpio" => "application/x-sv4cpio",
	"sv4crc" => "application/x-sv4crc",
	"t" => "application/x-troff",
	"tar" => "application/x-tar",
	"tcl" => "application/x-tcl",
	"tex" => "application/x-tex",
	"texi" => "application/x-texinfo",
	"texinfo" => "application/x-texinfo",
	"tgz" => "application/x-compressed",
	"tif" => "image/tiff",
	"tiff" => "image/tiff",
	"tr" => "application/x-troff",
	"trm" => "application/x-msterminal",
	"tsv" => "text/tab-separated-values",
	"txt" => "text/plain",
	"uls" => "text/iuls",
	"ustar" => "application/x-ustar",
	"vcf" => "text/x-vcard",
	"vrml" => "x-world/x-vrml",
	"wav" => "audio/x-wav",
	"wcm" => "application/vnd.ms-works",
	"wdb" => "application/vnd.ms-works",
	"wks" => "application/vnd.ms-works",
	"wmf" => "application/x-msmetafile",
	"wps" => "application/vnd.ms-works",
	"wri" => "application/x-mswrite",
	"wrl" => "x-world/x-vrml",
	"wrz" => "x-world/x-vrml",
	"xaf" => "x-world/x-vrml",
	"xbm" => "image/x-xbitmap",
	"xla" => "application/vnd.ms-excel",
	"xlc" => "application/vnd.ms-excel",
	"xlm" => "application/vnd.ms-excel",
	"xls" => "application/vnd.ms-excel",
	"xlt" => "application/vnd.ms-excel",
	"xlw" => "application/vnd.ms-excel",
	"xof" => "x-world/x-vrml",
	"xpm" => "image/x-xpixmap",
	"xwd" => "image/x-xwindowdump",
	"z" => "application/x-compress",
	"zip" => "application/zip");


?>