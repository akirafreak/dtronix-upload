<?php

// Check to see if this file is included into the main file for security reasons.
if( !defined("requireParrent") ) die("Restricted Access");

if(!$_CONFIG["update_broadcast"]) die("10|Server does not broadcast updates.");

function _getLatestInstallerVersion(){
	global $_CONFIG;
	include($_CONFIG["dir"] . "install.version.php");

	die($_INSTALL_VERSION[0]);
}

function _getDataFile(){
	global $_CONFIG;
	die(file_get_contents($_CONFIG["dir"] . "install.data.php"));
}

function _getVersionFile(){
	global $_CONFIG;
	die(file_get_contents($_CONFIG["dir"] . "install.version.php"));
}


?>
