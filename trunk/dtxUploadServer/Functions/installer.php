<?php

// Check to see if this file is included into the main file for security reasons.
if( !defined("requireParrent") ) die("Restricted Access");

if(!$_CONFIG["update_broadcast"]) die("10|Server does not broadcast updates.");

function _getLatestInstallerVersion(){
	global $_CONFIG;
	include($_CONFIG["dir"] . "install.info.php");

	die($_INSTALL_INFO["version"]);
}

function _getDataFile(){
	global $_CONFIG;
	die(file_get_contents($_CONFIG["dir"] . "install.data"));
}

function _getInfoFile(){
	global $_CONFIG;
	die(file_get_contents($_CONFIG["dir"] . "install.info.php"));
}


?>
