<?php
class Server extends SectionBase{

	public function Server() {
	}

	/**
	 * Used for the client to aquire information about the server.
	 */
	public function info(){
		$this->validateConnection(false);
		
		returnClientData("server_information", array(
			"server_name" => $this->CONFIG["server_name"],
			"maintenance_mode" => $this->CONFIG["server_maintenance_mode"],
			"max_upload_filesize" => $this->CONFIG["server_max_upload_filesize"],
			"allowed_filetypes" => $this->CONFIG["server_allowed_filetypes"],
			"upload_base_url" => $this->CONFIG["upload_base_url"],
			"server_logo" =>  $this->CONFIG["server_logo"]
		));
	}

	/**
	 * Used by the client to keep the session open.
	 */
	public function ping(){
		$this->validateConnection(false);

		returnClientData("ping", array(
			"maintenance_mode" => $this->CONFIG["server_maintenance_mode"]
		));
	}
}

?>