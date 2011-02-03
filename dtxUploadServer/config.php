<?php

// Check to see if this file is included into the main file for security reasons.
if( !defined("requireParrent") ) die("Restricted Access");

$_CONFIG["uri"] = "http://192.168.1.40/2010/dtxUpload/dtxUploadServer/";
$_CONFIG["upload_base_url"] = $_CONFIG["uri"];
$_CONFIG["dir"] = "C:\\Projects\\2010\\dtxUpload\\dtxUploadServer\\";
$_CONFIG["upload_dir"] = $_CONFIG["dir"] . "Files\\";

$_CONFIG["mysql_server"] = "localhost";
$_CONFIG["mysql_database"] = "dtxUpload";
$_CONFIG["mysql_password"] = "Qvn2T9ZNv5QLfvN";
$_CONFIG["mysql_user"] = "root";
$_CONFIG["mysql_id"] = false;

$_CONFIG["server_key"] = "test";
$_CONFIG["server_key_required"] = false;
$_CONFIG["email_activaction_required"] = false;
$_CONFIG["server_allowed_filetypes"] = "all";
$_CONFIG["server_max_upload_filesize"] = 1024 * 50;
$_CONFIG["server_maintenance_mode"] = false;
$_CONFIG["server_name"] = "Dtronix.com Hosting";
$_CONFIG["server_email"] = "no-reply@dtronix.com";
$_CONFIG["server_logo"] = $_CONFIG["uri"] . "logo.jpg";

$_CONFIG["html_theme"] = "kiss";



$_CONFIG["registration_allowed"] = true;
$_CONFIG["registration_verify_email"] = false;

$_CONFIG["session_max_life"] = 60 * 2000; // 20 minutes.  The user then has to re-login after the session expires.

?>