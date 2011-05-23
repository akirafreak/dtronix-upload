<?php
class Server extends SectionBase{

	public function Server() {
	}

	/**
	 * Used for the client to aquire information about the server.
	 */
	public function info(){
		$this->validateUser(false);
		
		callClientMethod("server_information", array(
			"server_name" => $this->_CONFIG["server_name"],
			"maintenance_mode" => $this->_CONFIG["server_maintenance_mode"],
			"max_upload_filesize" => $this->_CONFIG["server_max_upload_filesize"],
			"allowed_filetypes" => $this->_CONFIG["server_allowed_filetypes"],
			"upload_base_url" => $this->_CONFIG["upload_base_url"],
			"server_logo" =>  $this->_CONFIG["server_logo"]
		));
	}

	/**
	 * Used by the client to keep the session open.
	 */
	public function ping(){
		$this->validateUser(false);

		callClientMethod("ping", array(
			"maintenance_mode" => $this->_CONFIG["server_maintenance_mode"]
		));
	}
}

?>