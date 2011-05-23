<?php
class Installer extends SectionBase{
	public function Installer() {
	}

	/**
	 * Send the info file to the client.
	 */
	public function fileInfoDownload(){
		$this->isBroadcasting();
		
		die(file_get_contents($this->_CONFIG["dir"] . "install.info.php"));
	}

	/**
	 * Send the data file in a gzipped format.
	 */
	public function fileDataDownload(){
		$this->isBroadcasting();
		
		// Turn off the default gzip ob since the install.data is already GZ compressed.
		ob_end_clean();
		header("Content-Encoding: gzip");
		die(file_get_contents($this->_CONFIG["dir"] . "install.data"));
	}

	/**
	 * Retrieves the current version of the latest installer package.
	 */
	public function version(){
		$this->isBroadcasting();
		
		include($this->_CONFIG["dir"] . "install.info.php");
		die($_INSTALL_INFO["version"]);
	}
	
	/**
	 * 
	 */
	private function isBroadcasting(){
		if(!$this->_CONFIG["update_broadcast"]) die("10|Server does not broadcast updates.");
	}
}


?>
