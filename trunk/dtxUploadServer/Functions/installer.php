<?php
class Installer{
	private $_USER, $_CONFIG, $_QUERY;

	public function Installer(&$user, &$config, $query) {
		$this->_USER = &$user;
		$this->_CONFIG = &$config;
		$this->_QUERY = $query;

		if(!$this->_CONFIG["update_broadcast"]) die("10|Server does not broadcast updates.");
	}

	public function fileInfoDownload(){
		die(file_get_contents($this->_CONFIG["dir"] . "install.info.php"));
	}

	public function fileDataDownload(){
		// Turn off the default gzip ob since the install.data is already GZ compressed.
		ob_end_clean();
		header("Content-Encoding: gzip");
		die(file_get_contents($this->_CONFIG["dir"] . "install.data"));
	}

	public function version(){
		include($this->_CONFIG["dir"] . "install.info.php");
		die($_INSTALL_INFO["version"]);
	}
}


?>
