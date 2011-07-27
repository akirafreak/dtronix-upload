<?php
//Start Date:Sat, 01 Jan 2011 00:00:00 GMT
//Version:0.4

ob_start("ob_gzhandler");
define("requireParrent", true);

$USER = array();

require_once("config.php");
require_once("functions.php");
require_once("Classes/SQL.class.php");
require_once("Classes/ReturnData.class.php");
require_once("Classes/SectionBase.class.php");
require_once("Classes/ThemeBase.class.php");

performRuntimeChecks();

if(!array_key_exists("server_timezone", $CONFIG) ){
	$CONFIG["server_timezone"] = "America/New_York";
}

ini_set("magic_quotes_gpc", "0");
date_default_timezone_set($CONFIG["server_timezone"]);
register_shutdown_function("shutdownFunction");
set_error_handler("errorHandler", E_ALL);

// Start a new instance of the SQL class to handle queries.
$SQL = new SQL();

// Determine which type of database to connect to.
if($CONFIG["sql_server_select"] == "postgresql"){
	$SQL->pgsqlConnect($CONFIG["pgsql_server"], $CONFIG["pgsql_server_port"], $CONFIG["pgsql_database"], $CONFIG["pgsql_user"], $CONFIG["pgsql_password"]);

}elseif($CONFIG["sql_server_select"] == "mysql"){
	$SQL->mysqlConnect($CONFIG["mysql_server"], $CONFIG["mysql_database"], $CONFIG["mysql_user"], $CONFIG["mysql_password"]);
}

// Check to see what kind of client is trying to connect.
if(strpos($_SERVER["HTTP_USER_AGENT"], "dtxClient") !== false){
	$USER["client"] = 1; // dtxManageClient Program.

}else{
	$USER["client"] = 2; // Regular web request.
}

// Make sure we have an entire valid call.
if(array_key_exists("action", $_GET) && strpos($_GET["action"], ":") === false){
	returnClientData("error_client", array(
		"error" => "Unknown method called.",
		"method" => $_GET["action"]
	));
}

// If the user did not pass an action, then use the Main default.
if(!array_key_exists("action", $_GET))
	$_GET["action"] = "Main:mainDefault";

// Parse the request.
list($call_class, $call_method) = explode(":", $_GET["action"]);

$requested_file = "Sections/" . $call_class. ".php";

// We don't want somebody accessing a file in another directory now do we?
if(strpos($call_class, ".") !== false){
	returnClientData("error_client", array(
		"error" => "Invalid Request.",
		"method" => $_GET["action"]
	));
}

// Check to see if the requested file actually exists.
if(!file_exists($requested_file)){
	if($USER["client"] == 1){
		returnClientData("error_client", array(
			"error" => "Unknown action called.",
			"unknown_action" => $call_class
		));

	}else{
		die("Unknown method requested.");
	}

}else{
	require_once($requested_file);
	//returnClientData("asf", $call_class);
	$called_class = new $call_class();
	$called_class->setVariables($USER, $CONFIG, $SQL);
	
	// Check to see if the class contains the method requested and that it is not private or protected..
	if(is_callable(array($called_class, $call_method)) == false){
		returnClientData("error_client", array(
			"error" => "Unknown method called.",
			"unknown_method" => $call_method
		));
	}
	
	$args = array();
	if(array_key_exists("args", $_POST)){ // If GET & POST arguemnts are passed, the GET will be ignored.
		$args = json_decode($_POST["args"], true);
		if($args == null)
			errorHandler(0, "Unreadable JSON: ". $_POST["args"], __FILE__, __LINE__);
	
	}else if(array_key_exists("args", $_GET)){ // Check to see if GET arguemtns were passed.
		$args = $_GET["args"];
		
	}else{ // The user did not pass any arguments.
		$args = array();
	}
	
	// Finally, call the requested method in the new class instance.
	$result = call_user_func_array(array($called_class, $call_method), $args);
	
	if($result instanceof ReturnData){
		$result->sendClientInfo();
	}
}
?>