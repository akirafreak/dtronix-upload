<?php

function _getServerInfo(){
	global $_CONFIG;
	callClientMethod("server_information", array(
		"server_name" => $_CONFIG["server_name"],
		"maintenance_mode" => $_CONFIG["server_maintenance_mode"],
		"max_upload_filesize" => $_CONFIG["server_max_upload_filesize"],
		"allowed_filetypes" => $_CONFIG["server_allowed_filetypes"],
		"upload_base_url" => $_CONFIG["upload_base_url"],
		
		"permissions" => array(
			"registration_allowed" => $_CONFIG["registration_allowed"],
			"key_required" => $_CONFIG["server_key_required"]
		)
	));		
}

function _ping(){
	global $_CONFIG;
	callClientMethod("ping", array(
		"server_name" => $_CONFIG["server_name"],
		"maintenance_mode" => $_CONFIG["server_maintenance_mode"]
	));
}

?>