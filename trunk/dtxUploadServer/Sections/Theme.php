<?php

class Theme extends SectionBase{

	public function Theme() {
	}

	/**
	 * Outputs default main theme.
	 */
	public function outputHtml(){
		$this->validateConnection(false);

		$file = $this->CONFIG["dir"] . "Theme/" . $this->CONFIG["html_theme"] . "/main.theme.php";
		
		// TODO: Come up with error for theme not existing.
		if(file_exists($file)){
			require_once($this->CONFIG["dir"] . "Theme/" . $this->CONFIG["html_theme"] . "/main.theme.php");
			new ThemeMain($this->USER, $this->CONFIG, $this->QUERY);

		}else{
			die("Required Theme file not found!");
		}
	}
}

?>