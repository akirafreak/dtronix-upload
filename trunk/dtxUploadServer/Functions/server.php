<?php
class Server{
	private $_USER, $_CONFIG, $_QUERY;
	
	public function Server(&$user, &$config, $query) {
		$this->_USER = &$user;
		$this->_CONFIG = &$config;
		$this->_QUERY = $query;
	}
	
	public function info(){
		global $_CONFIG;
		callClientMethod("server_information", array(
			"server_name" => $_CONFIG["server_name"],
			"maintenance_mode" => $_CONFIG["server_maintenance_mode"],
			"max_upload_filesize" => $_CONFIG["server_max_upload_filesize"],
			"allowed_filetypes" => $_CONFIG["server_allowed_filetypes"],
			"upload_base_url" => $_CONFIG["upload_base_url"],
			"server_logo" => $_CONFIG["server_logo"]
		));
	}

	public function ping(){
		global $_CONFIG;
		callClientMethod("ping", array(
			"maintenance_mode" => $_CONFIG["server_maintenance_mode"]
		));
	}
}

?>