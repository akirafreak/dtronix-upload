<?php

class Main extends SectionBase {
	public function mainDefault(){
		$this->outputTheme("ThemeMain.header");
		$this->outputTheme("ThemeMain.actions");
		$this->outputTheme("ThemeMain.login");
		$this->outputTheme("ThemeMain.actionBarFooter");
		$this->outputTheme("ThemeMain.config");
		$this->outputTheme("ThemeMain.register");
		$this->outputTheme("ThemeMain.displayFiles");
		$this->outputTheme("ThemeMain.adminInterface");
		$this->outputTheme("ThemeMain.footer");
	}
}

?>
