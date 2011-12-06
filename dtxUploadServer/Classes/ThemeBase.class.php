<?php

/**
 * Base class to all theme classes.
 */
class ThemeBase {
	/**
	 * @var array Associative array that contains the entire user row from the database.
	 */
	protected $USER;

	/**
	 * @var array Associative array that contains the entire config.php array.
	 */
	protected $CONFIG;
	
	/**
	 * @var array Contains all theme variables such as urls and directories. 
	 */
	protected $T_VAR;
	
	/**
	 * Method to assign variables their values.
	 */
	public function setVariables(&$user, &$config){
		$this->USER = &$user;
		$this->CONFIG = &$config;
		
		$this->T_VAR["theme_url"] = $this->CONFIG["uri"] . "Theme/" . $this->CONFIG["html_theme"]. "/";
	}
}

?>
