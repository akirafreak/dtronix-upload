<?php
//0.2

// Define this before all the includes to ensure that all the files are actually included through this main file.
define("requireParrent", true);

ob_start();
register_shutdown_function('ob_end_flush');

$_USER = array();
$_PERMISSIONS = array();

include_once("config.php");
include_once("functions.php");

call_user_func(dtx_main());

function dtx_main(){
	global $_CONFIG, $_USER, $_PERMISSIONS;
	
	// Check to see what kind of client is trying to connect.
	if(strpos($_SERVER["HTTP_USER_AGENT"], "dtxUploadClient") !== false){
		// dtxUpload Client Program.
		$_USER["client"] = 1;
	}else{
		// Regular web request.
		$_USER["client"] = 2;
	}

	// Ensure the client does not cashe requests.
	header("Cache-Control: no-cache, must-revalidate"); // HTTP/1.1
	header("Expires: Sat, 26 Jul 1997 05:00:00 GMT"); // Date in the past

	// TODO: Ensure the user is allowed to connect to the server.
	
	
	// TODO: Check to see if the client/user is banned.
	
	//Connect to the database.
	$_CONFIG["mysql_id"] = mysql_connect($_CONFIG["mysql_server"], $_CONFIG["mysql_user"], $_CONFIG["mysql_password"]) or callClientMethod("server_error_mysql", mysql_error());
	mysql_select_db($_CONFIG["mysql_database"]) or die("Could not select database");

	
	/* $valid_calls Template
	(KEY) = string that is called via the action parameter.
	array(
		[string] Class file name
		[string] Function to call that is in the class file
		[bool] TRUE: User must be logged in before proceeding; FALSE: User can proceed without loggin in;
	)
	*/

	$valid_calls = array(
		// Server functions.
		"get_server_info" => array("server", "_getServerInfo", false),
		"ping_logged"  => array("server", "_ping", true),
		"ping"  => array("server", "_ping", false),
		
		// User functions.
		"permissions_verification" => array("user", "_verifyPermission", true),
		"user_verification" => array("user", "_verifyUser", true),
		"register" => array("user", "_registerAccount", false),
		"load_user_info" => array("user", "_getUserInfo", true),
		"logout" => array("user", "_logout", true),

		// File Functions.
		"load_files_quick" => array("files", "_filesUploadedQuick", true),
		"file_delete" => array("files", "_fileDelete", true),
		"upload_file" => array("files", "_uploadNewFile", true),
		"view_file" => array("files", "_viewFile", false),

		// Installer Functions.
		"update_latest_version" => array("installer", "_getLatestInstallerVersion", false),
		"install_file_data" => array("installer", "_getDataFile", false),
		"install_file_info" => array("installer", "_getInfoFile", false),

		//Theme functions.
		"html" => array("theme", "outputHtml", false)
	);
	
	// Ensure the user is calling a valid function.
	if(isset($_GET["action"]) && isset($valid_calls[$_GET["action"]])){
		if($valid_calls[$_GET["action"]][2]){
			validateUser();
		}
		
		require_once("Functions/" . $valid_calls[$_GET["action"]][0] . ".php");
		
		// Send the function to call to the call_user_func method.
		return $valid_calls[$_GET["action"]][1];
	}else{
		if($_USER["client"] == 1){
			callClientMethod("error_client", array(
				"error" => "Unknown method called.",
				"unknown_method" => $_GET["action"]
			));
		}
		require_once("Functions/theme.php");
		$_GET["section"] = "main";
		$_GET["theme"] = "default";
		
		// Send the function to call to the call_user_func method.
		return "outputHtml";
	}
	
}




?>