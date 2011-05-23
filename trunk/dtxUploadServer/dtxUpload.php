<?php
//Start Date:Sat, 01 Jan 2011 00:00:00 GMT
//Version:0.3

ob_start("ob_gzhandler");
define("requireParrent", true);

$_USER = array();
$_PERMISSIONS = array();

require_once("config.php");
require_once("functions.php");
require_once("Classes/MySQL.class.php");
require_once("Classes/SectionBase.class.php");

if(!array_key_exists("server_timezone", $_CONFIG) ){
	$_CONFIG["server_timezone"] = "America/New_York";
}

date_default_timezone_set($_CONFIG["server_timezone"]);
register_shutdown_function("shutdownFunction");
set_error_handler("errorHandler", E_ALL);

//Connect to the database.
$_SQL = new MySQL($_CONFIG["mysql_server"], $_CONFIG["mysql_user"], $_CONFIG["mysql_password"]);
$_SQL->selectDb($_CONFIG["mysql_database"]);

// Check to see what kind of client is trying to connect.
if(strpos($_SERVER["HTTP_USER_AGENT"], "dtxUploadClient") !== false){
	// dtxUpload Client Program.
	$_USER["client"] = 1;

}else{
	// Regular web request.
	$_USER["client"] = 2;
}

// TODO: Check to see if the client/user is banned.

// Ensure that $_GET["args"] is assigned something.
if(array_key_exists("args", $_GET) == false){
	$_GET["args"] = null;
}

// Verify that a action is being requested.
if(array_key_exists("action", $_GET) == false){
	require_once("Sections/theme.php");
	$theme_instance = new Theme();
	$theme_instance->setVariables($_USER, $_CONFIG, $_GET["args"], $_SQL);
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

$requested_file = "Sections/" . strtolower($call_class). ".php";

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
	$called_class = new $called_class_name();
	$called_class->setVariables($_USER, $_CONFIG, $_GET["args"], $_SQL);

	// Check to see if the class contains the method requested.
	if(method_exists($called_class, $call_method) == false){
		callClientMethod("error_client", array(
			"error" => "Unknown method called.",
			"unknown_method" => $call_method
		));
	}

	// Fianlly, call the requested method in the new class instance.
	call_user_func_array(array($called_class, $call_method), $_GET["args"]);
}
?>