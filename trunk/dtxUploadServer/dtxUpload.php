<?php
//Start Date:Sat, 01 Jan 2011 00:00:00 GMT
//Version:0.2

ob_start("ob_gzhandler");
register_shutdown_function('ob_end_flush');
define("requireParrent", true);

$_USER = array();
$_PERMISSIONS = array();

include_once("config.php");
include_once("functions.php");

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

// Ensure that $_GET["args"] is assigned something.
if(key_exists("args", $_GET) == false){
	$_GET["args"] = null;
}

// Verify that a action is being requested.
if(key_exists("action", $_GET) == false){
	require_once("Functions/theme.php");
	$theme_instance = new Theme($_USER, $_CONFIG, $_GET["args"]);
	$theme_instance->outputHtml();
	die();
}

// Make sure we have an entire valid call.
if(strpos($_GET["action"], ":") === false){
	callClientMethod("error_client", array(
		"error" => "Unknown method called."
	));
}

// Parse the request.
list($call_class, $call_method) = explode(":", $_GET["action"]);

$requested_file = "Functions/" . strtolower($call_class). ".php";

// We don't want somebody accessing a file in another directory now do we?
if(strpos($call_class, ".") !== false){
	callClientMethod("error_client", array(
		"error" => "Invalid Request."
	));
}

// Check to see if the requested file actually exists.
if(!file_exists($requested_file)){
	if($_USER["client"] == 1){
		callClientMethod("error_client", array(
			"error" => "Unknown action called.",
			"unknown_action" => $call_class
		));

	}else{
		die("Unknown method requested.");
	}
	
}else{
	require_once($requested_file);
	$called_class_name = ucfirst(strtolower($call_class));
	$called_class = new $called_class_name($_USER, $_CONFIG, $_GET["args"]);

	// Check to see if the class contains the method requested.
	if(method_exists($called_class, $call_method) == false){
		callClientMethod("error_client", array(
			"error" => "Unknown method called.",
			"unknown_method" => $call_method
		));
	}

	// Fianlly, call the requested method in the new class instance.
	$called_class->$call_method();
}
?>