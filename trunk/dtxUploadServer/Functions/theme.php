<?php

class Theme{
	private $_USER, $_CONFIG, $_QUERY;

	public function Theme(&$user, &$config, $query) {
		$this->_USER = &$user;
		$this->_CONFIG = &$config;
		$this->_QUERY = $query;
	}

	public function outputHtml(){
		$file = $this->_CONFIG["dir"] . "Theme/" . $this->_CONFIG["html_theme"] . "/main.theme.php";

		// TODO: Come up with error for theme not existing.
		if(file_exists($file)){
			require_once($this->_CONFIG["dir"] . "Theme/" . $this->_CONFIG["html_theme"] . "/main.theme.php");
			new ThemeMain($this->_USER, $this->_CONFIG, $this->_QUERY);

		}else{
			die("Required Theme file not found!");
		}
	}
}

?>