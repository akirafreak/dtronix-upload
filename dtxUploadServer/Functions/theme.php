<?php

require_once("./functions.php");

// Define Theme Variables.


function outputHtml(){
	global $_USER, $_CONFIG, $T_VAR;
	if(!array_key_exists("section", $_GET)) return;

	$T_VAR["theme_url"] = $_CONFIG["uri"] . "Theme/" . $_CONFIG["html_theme"]. "/";

	$valid_files = array("main", "status", "uploads", "settings");
	if(in_array($_GET['section'], $valid_files)){
		$file = $_CONFIG["dir"] . "Theme/" . $_CONFIG["html_theme"] . "/" . $_GET["section"] . ".theme.php";

		// TODO: Come up with error for theme not existing.
		if(file_exists($file)){
			require_once($_CONFIG["dir"] . "Theme/" . $_CONFIG["html_theme"] . "/main.theme.php");
			require_once($file);

			$call_func = "theme_" . $_GET["section"]. "_" . $_GET["theme"];
			if(function_exists($call_func)){
				call_user_func($call_func);
			}else{
				die("Requested theme action is not valid.");
			}
		}else{
			die("Required Theme file not found!");
		}
	}
}

?>