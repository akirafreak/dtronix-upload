<?php
// Check to see if this file is included into the main file for security reasons.
if( !defined("requireParrent") ) die("Restricted Access");
$CONFIG["uri"] = '';
$CONFIG["upload_base_url"] = '';
$CONFIG["dir"] = '';
$CONFIG["upload_dir"] = '';
$CONFIG["sql_server_select"] = "mysql";
$CONFIG["mysql_server"] = 'localhost';
$CONFIG["mysql_database"] = '';
$CONFIG["mysql_password"] = '';
$CONFIG["mysql_user"] = '';
$CONFIG["mysql_id"] = false;
$CONFIG["email_activaction_required"] = false;
$CONFIG["server_allowed_filetypes"] = 'all';
$CONFIG["server_max_upload_filesize"] = 51200;
$CONFIG["server_maintenance_mode"] = false;
$CONFIG["server_name"] = '';
$CONFIG["server_email"] = '';
$CONFIG["server_logo"] = false;
$CONFIG["html_theme"] = 'kiss2';
$CONFIG["update_broadcast"] = false;
$CONFIG["registration_allowed"] = true;
$CONFIG["registration_verify_email"] = false;
$CONFIG["session_max_life"] = 120000;
?>