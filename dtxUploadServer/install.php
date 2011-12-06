<?php

// Define this before all the includes to ensure that all the files are actually included through this main file.
define("requireParrent", true);

if(file_exists("functions.php")) require_once("functions.php");

$INSTALL_VAR["download_beta"] = false;
$INSTALL_VAR["continue_prompt_count"] = 0;

// Determine if we need to upgrade or install.
if(key_exists("action", $_GET) == false){
	head(); ?>
<a href="install.php?action=install">Install Server</a><?

}else if($_GET["action"] == "install"){
	// Set the variables for a fresh installation.
	$INSTALL_VAR["upgrade_installation"] = false;

	if(file_exists("config.php") && !isset($_POST["delete_installation_files"])) die("Config file already exists.  Exiting installer.");

	head(); ?>
<span class="main_header">Dtronix Manage Server Installer</span><br/><?
	continueIfTrue(checkPHPVersion());
	continueIfTrue(checkForInstallationFiles());
	continueIfTrue(initialFileChecks());
	continueIfTrue(basicConfigurations());
	continueIfTrue(mysqlConfigurations());
	continueIfTrue(mysqlInstallation());
	continueIfTrue(configFileCreation());

	foot("Delete Installaion Files");
}else{
	die("Unknown action requested.");
}

function checkPHPVersion(){
	$continue_installation = true; ?>
<span style="font-size: 18px;">Checking PHP Version.</span><br /><?php

	if(version_compare(PHP_VERSION, '5.0.0') >= 0){
		writeInfo("green", "You are running PHP ". PHP_VERSION ." which is supported.");

	}else{
		if(!file_exists(".htaccess")){
			writeInfo("red", "You are running PHP ". PHP_VERSION ." which is not supported.  Trying to force PHP 5 support...");

			$stream = @fopen(".htaccess", 'a');
			if (!$stream) {
				$continue_installation = false;
				writeInfo("red", "Unable to create .htaccess file to force PHP5 support.");

			} else {
				$bytes = fwrite($stream, "AddHandler application/x-httpd-php5 .php");
				fclose($stream);
				if($bytes === false){
					$continue_installation = false;
					writeInfo("red", "Unable to create .htaccess file to force PHP5 support.");

				}else{
					$continue_installation = false;
					writeInfo("green", "Created .htaccess file to force PHP5 support.");
				}
			}

		}else{
			$continue_installation = false;
			writeInfo("red", "Unable to force PHP5 support.  Your server will not support Dtronix Manage.");
		}
	}
	return $continue_installation;
}


function checkForInstallationFiles(){
	global $INSTALL_VAR;
	$continue_installation = true;
	$download_server = ($INSTALL_VAR["download_beta"])? "http://upload-beta.dtronix.com/" : "http://upload.dtronix.com/"; ?>
<br /><br />
<span style="font-size: 18px;">Checking for installation files.</span>
<table>
	<tbody valign="top">
		<tr>
			<td>This Directory</td>
			<td><?php
				if(is_writable(getcwd())){
					writeInfo("green", "Writable");
				}else{
					writeInfo("red", "Read Only (Must be writable!)");
					$continue_installation = false;
				}
			?></td>
		</tr>
		<tr>
			<td>Verifying installation files:</td>
			<td><?php

				if((file_exists("install.data.php") || file_exists("install.data")) && file_exists("install.info.php") && !isset($_GET["force_download"])){
					writeInfo("green", "Required files Exist. <a href=\"". $_SERVER["REQUEST_URI"] ."&force_download=true\">(Force download latest version)</a>");
				}else{
					if(!isset($_GET["force_download"])){
						writeInfo("red", "Required installation files do not exist.");
						$continue_installation = false;
					}
					// Add auto-downloader later.
//					if(!saveUrl($download_server ."dtxUpload.php?action=installer:fileDataDownload", "install.data.php")){
//						$continue_installation = false;
//					}
//					if(!saveUrl($download_server ."dtxUpload.php?action=installer:fileInfoDownload", "install.info.php")){
//						$continue_installation = false;
//					}
				}
			?></td>
		</tr><?php
		if($continue_installation){ ?>
		<tr>
			<td>Installation Files Version:</td>
			<td><?php

				require("install.info.php");
				writeInfo("green", "Version ". $_INSTALL_INFO["version"]);
			?></td>
		</tr>
		<tr>
			<td>Verifying Files:</td>
			<td><?php
				if(file_exists("install.data") && !file_exists("install.data.php")){
					ob_start();
					readgzfile("install.data");
					file_put_contents("install.data.php", ob_get_clean());
					ob_end_clean();
				}
				if(md5_file("install.data.php") == $_INSTALL_INFO["MD5_hash"]){
					writeInfo("green", "MD5 hashes match. Valid files.");
				}else{

					$continue_installation = false;
					writeInfo("red", "Installation files corrupted. <a href=\"". $_SERVER["REQUEST_URI"] ."&force_download=true\">(Force download latest version)</a>");
				}
			?></td>
		</tr><?php
		}
		if((isset($_GET["force_download"]) || !file_exists("index.php") || $INSTALL_VAR["upgrade_installation"]) && $continue_installation){ ?>
		<tr>
			<td>Extracting:</td>
			<td><?php
				require("install.data.php");
				writeInfo("green", "Creating folders...");

				$base_dir = dirname($_SERVER["SCRIPT_FILENAME"]) . "/";
				foreach($_INSTALLER_DIRS as $dir){
					if(is_dir($base_dir . $dir)){
						writeInfo("green", "Folder \"". $base_dir . $dir ."\" already exists.");
						
					}else{
						if(mkdir($base_dir . $dir, 0777, true)){
							writeInfo("green", "Created folder \"". $base_dir . $dir ."\"");
							$continue_installation = false;

						}else{
							writeInfo("red", "Failed creating folder \"". $base_dir . $dir ."\"");
						}
					}
				}
				writeInfo("green", "Creating Files.");

				foreach($_INSTALLER_FILE as $file_name => $base64_file){
					$file = base64_decode($base64_file);
					if(file_put_contents($base_dir . $file_name, $file) === false){
						writeInfo("red", "Failed creating file \"". $file_name ."\"");
						$continue_installation = false;

					}else{
						writeInfo("green", "Created file \"". $file_name ."\"");
					}
				}
			?></td>
		</tr><?php
		} ?>
	</tbody>
</table><?php
	return $continue_installation;
}


function initialFileChecks(){
	$continue_installation = true;
	$write_error = false;
	$exist_error = false; ?>
<br /><br />
<span style="font-size: 18px;">Checking File Writing Permissions</span>
<table>
	<tbody valign="top">
		<tr>
			<td>This Directory</td>
			<td><?php
				if(is_writable(getcwd())){
					writeInfo("green", "Writable");

				}else{
					$write_error = true;
					writeInfo("red", "Read-Only");
				}
			?></td>
		</tr>
		<tr>
			<td>Files Directory:</td>
			<td><?php
				if(is_dir(getcwd(). "/Files/")){
					if(is_writable(getcwd(). "/Files/")){
						writeInfo("green", "Writable");

					}else{
						$write_error = true;
						writeInfo("red", "Read-Only");
					}

				}else{
					$exist_error = true;
					writeInfo("red", "Folder does not exist.");
				}
			?></td>
		</tr>
		<tr>
			<td>Theme Directory:</td>
			<td><?php
				if(is_dir(getcwd(). "/Theme/")){
					if(is_writable(getcwd(). "/Theme/")){
						writeInfo("green", "Writable");

					}else{
						$write_error = true;
						writeInfo("red", "Read-Only");
					}

				}else{
					$exist_error = true;
					writeInfo("red", "Folder does not exist.");
				}
			?></td>
		</tr>
		<tr>
			<td>functions.php:</td>
			<td><?php
				if(file_exists("functions.php")){
					writeInfo("green", "functions.php exists.");
					
				}else{
					$exist_error = true;
					writeInfo("red", "functions.php does not exist.");
				}
			?></td>
		</tr>
	</tbody>
</table>
<?php
	if($exist_error){
		$continue_installation = false;
		writeInfo("red; font-size: 15px", "Errors detected with folder structure.  Please check to see that you copied all the install files to the server.");
	}
	if($write_error){
		$continue_installation = false;
		writeInfo("red; font-size: 15px", "Permission have not been set to the folders marked in read above.  <br/>Please ensue the folders marked above have CHMOD 777");
	}
	return $continue_installation;
}

function basicConfigurations(){
	$continue_installation = true; ?>
	<br /><br />
	<span style="font-size: 18px;">Basic Configurations (Default values are most likely correct)</span>
	<table>
		<tbody valign="top">
			<tr>
				<td>Base URL:</td>
				<td><?php writeInputText("basic_baseUrl", "http://" . $_SERVER["HTTP_HOST"] . dirname($_SERVER["SCRIPT_NAME"])); ?></td>
			</tr>

			<tr>
				<td>Base Directory:</td>
				<td><?php writeInputText("basic_baseDir", dirname($_SERVER["SCRIPT_FILENAME"]) . "/"); ?></td>
				<td><?php
				if(!empty($_POST["basic_baseDir"])){
					if(is_dir($_POST["basic_baseDir"])){
						writeInfo("green", "Directory exists.");

					}else{
						$continue_installation = false;
						writeInfo("red", "Directory does not exists.");
					}
				}
				?></td>
			</tr>
			<tr>
				<td>Server Name:</td>
				<td><?php writeInputText("basic_serverName", ""); ?></td>
			</tr>
			<tr>
				<td>Server Timezone <a href="http://en.wikipedia.org/wiki/Time_zone">(+- From UTC)</a>:</td>
				<td><?php writeInputText("basic_serverTimezone", "-5"); ?></td>
			</tr>
			<tr>
				<td>Server Email:</td>
				<td><?php writeInputText("basic_serverEmail", ""); ?></td>
			</tr>
		</tbody>

	</table>
<?php
	// Require this again when the files are just uncompressed.
	if(file_exists("functions.php")) require_once("functions.php");
	
	// Do not continue untill all the required information is filled in.
	if(!array_keys_exists($_POST, array("basic_baseUrl", "basic_baseDir", "basic_serverTimezone"), false)){
		return false;
	}
	return $continue_installation;
}

function mysqlConfigurations(){
	$continue_installation = true; ?>
	<br /><br />
	<span style="font-size: 18px;">MySQL Database Configurations</span>
	<table>
		<tbody valign="top">
			<tr>
				<td>MySQL Server:</td>
				<td><?php writeInputText("mysql_server", ""); ?></td>
				<td>(This is almost always "localhost")</td>
			</tr>

			<tr>
				<td>MySQL Database Username:</td>
				<td><?php writeInputText("mysql_username", ""); ?></td>
				<td></td>
			</tr>

			<tr>
				<td>MySQL Database Password:</td>
				<td><?php writeInputText("mysql_password", ""); ?></td>
				<td><?php
				if(!empty($_POST["mysql_server"]) && !empty($_POST["mysql_username"]) && !empty($_POST["mysql_password"])){
					$database_connection = @mysql_connect($_POST["mysql_server"], $_POST["mysql_username"], $_POST["mysql_password"]);

					if(!$database_connection) {
						$continue_installation = false;
						writeInfo("red", "Connection error: ". mysql_error());

					}else{
						writeInfo("green", "Connected successfully.");
					}
				}
				?></td>
			</tr>

			<tr>
				<td>MySQL Database:</td>
				<td><?php writeInputText("mysql_database", ""); ?></td>
				<td><?php
				if($database_connection){
					$db_link = @mysql_select_db($_POST["mysql_database"]);
					if(!$db_link) {
						$continue_installation = false;
						writeInfo("red", "Database selection error: ". mysql_error());

					}else{
						writeInfo("green", "Database successfully connected.");
					}
				}
				?></td>
			</tr>
			<tr>
				<td>Existing Dtronix Manage DB?</td>
				<td><?php writeInputCheckbox("mysql_existing_db"); ?></td>
			</tr>
		</tbody>
	</table>
<?php
	// Do not continue untill all the required information is filled in.
	if(!array_keys_exists($_POST, array("mysql_server", "mysql_username", "mysql_password", "mysql_database"), false)){
		writeInfo("red; font-size: 15px", "All MySQL information above is required before continuing.");
		return false;
	}
	return $continue_installation;
}

function mysqlInstallation(){
	$continue_installation = true; ?>
<br /><br />
<span style="font-size: 18px;">MySQL Database Creation</span><br/><?php
	if(!isset($_POST["mysql_existing_db"])){
	continueIfTrue(mysqlTableCreate("files", "CREATE TABLE IF NOT EXISTS `files` (
		`id` int(11) NOT NULL AUTO_INCREMENT,
		`owner_id` int(11) NOT NULL,
		`url_id` varchar(32) NOT NULL,
		`directory` varchar(64) NOT NULL DEFAULT '/',
		`tags` text NOT NULL,
		`upload_date` date NOT NULL,
		`upload_id` varchar(32) NOT NULL,
		`last_accessed` date NOT NULL,
		`total_views` int(11) NOT NULL DEFAULT '0',
		`is_public` tinyint(1) NOT NULL,
		`is_visible` tinyint(1) NOT NULL,
		`is_disabled` tinyint(1) NOT NULL,
		`shared_ids` text NOT NULL,
		`file_status` int(11) NOT NULL,
		`is_encrypted` tinyint(1) NOT NULL,
		`file_name` varchar(128) NOT NULL,
		`file_size` int(11) NOT NULL,
		`file_mime` varchar(64) NOT NULL,
		PRIMARY KEY (`id`),
		KEY `url_id` (`url_id`),
		KEY `owner_id` (`owner_id`),
		FULLTEXT KEY `tags` (`tags`)
	) ENGINE=MyISAM  DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;"));

	?><input type="hidden" name="mysql_existing_db" value="true" /><?php
	}else{
		writeInfo("darkorange", "MySQL database creation/insertion skipped due to existing databases.");
	}
	return true;
}


function configFileCreation(){
	$continue_installation = true; ?>
<br /><br />
<span style="font-size: 18px;">Configuration File Creation</span><br/><?php


	$CONFIG["uri"] = $_POST["basic_baseUrl"];
	$CONFIG["upload_base_url"] = $_POST["basic_baseUrl"];
	$CONFIG["dir"] = $_POST["basic_baseDir"];
	$CONFIG["upload_dir"] = $_POST["basic_baseDir"] . "Files/";

	$CONFIG["mysql_server"] = $_POST["mysql_server"];
	$CONFIG["mysql_database"] = $_POST["mysql_database"];
	$CONFIG["mysql_password"] = $_POST["mysql_password"];
	$CONFIG["mysql_user"] = $_POST["mysql_username"];
	$CONFIG["mysql_id"] = false;

	$CONFIG["email_activaction_required"] = false;
	$CONFIG["server_maintenance_mode"] = false;
	$CONFIG["server_name"] = $_POST["basic_serverName"];
	$CONFIG["server_email"] = $_POST["basic_serverEmail"];
	$CONFIG["server_logo"] = false;
	$CONFIG["server_timezone"] = $_POST["basic_serverTimezone"];

	$CONFIG["html_theme"] = "kiss";

	$CONFIG["update_broadcast"] = false;

	$CONFIG["registration_allowed"] = true;
	$CONFIG["registration_verify_email"] = false;

	$CONFIG["session_max_life"] = 60 * 2000;
	$CONFIG["version"] = $_INSTALL_INFO["version"];

	if(saveConfigFile($CONFIG)){
		writeInfo("green", "Successfully created configuration file.");
	}else{
		writeInfo("red", "Failed to create configuration file.");
		return false;
	}

	?><br />
	<input type="hidden" name="delete_installation_files" value="true" />
	<span style="font-size: 18px; color: green;">Setup Complete!</span><br/><?php

	return true;
}

function head(){
?><!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN"
	"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en" lang="en">
<head>
<style>
* {
	font-family:monospace;
	font-size: 11px;
}
.main_header{
	font-size: 20px;
	text-decoration: underline;
}
</style>
</head>
<body>
<div style="width: 100%;" align="center">
<div style="width: 950px;" align="left"><?php
	if(isset($_POST["delete_installation_files"])){ ?>
<form method="post" action="index.php">
<span style="font-size: 20px; text-decoration: underline;">Deleting Installation Files...</span><br/><br/><?php
		deleteFile("install.php");
		deleteFile("install.data.php");
		deleteFile("install.info.php");

		if(file_exists("install.data"))
			deleteFile("install.data");

		if($_GET["action"] == "install"){
			writeInfo("red", "NOTE: Make sure to register immediately.  The first user that registers is admin.");
		}
		foot("Go to Dtronix Manage");
	}else{ ?>
<form method="post" action="<?= $_SERVER["REQUEST_URI"] ?>"><?php
	}

}

function foot($submit_text = "Continue"){ ?>
	<?php echo $html; ?>
	<input type="submit" value="<?php echo $submit_text; ?>" />
</form>
</div>
</div>
</body>
</html><?php
	die();
}

function mysqlTableCreate($name, $sql){
	$query = mysql_query($sql);
	if(!$query){
		writeInfo("red", "Table \"$name\" creation error: ". mysql_error());
		return false;
	}else{
		writeInfo("green", "Table \"$name\" created successfully.");
		return true;
	}
}

function mysqlRowInsert($name, $sql){
	$query = mysql_query($sql);
	if(!$query){
		writeInfo("red", "Could not insert records into \"$name\". ". mysql_error());
		return false;
	}else{
		writeInfo("green", "Successfully inserted records into \"$name\".");
		return true;
	}
}

function mysqlTableDrop($name, $sql){
	$query = mysql_query($sql);
	if(!$query){
		writeInfo("red", "Could not drop table \"$name\". ". mysql_error());
		return false;
	}else{
		writeInfo("green", "Successfully dropped table \"$name\".");
		return true;
	}
}

function mysqlTableAlter($name, $sql){
	$query = mysql_query($sql);
	if(!$query){
		writeInfo("red", "Could not alter table \"$name\". ". mysql_error());
		return false;
	}else{
		writeInfo("green", "Successfully altered table \"$name\".");
		return true;
	}
}

function deleteFile($file){
	if(!file_exists($file)) return false;
	if(unlink($file)){
		writeInfo("green", "Deleted $file");
	}else{
		writeInfo("red", "Could not delete $file");
	}
}

function continueIfTrue($val){
	if($val != true) foot();
}

function writeInfo($color, $info){
	echo"<span style=\"color: $color;\">$info</span><br />";
}

function writeInputText($name, $default_value){
	?><input type="text" name="<?php echo $name; ?>" style="width: 400px;" value="<?php
	if(!empty($_POST[$name])){
		echo $_POST[$name];
	}else{
		echo $default_value;
	}
	?>" /><?php
}

function writeInputCheckbox($name){
	?><input type="checkbox" name="<?php echo $name; ?>" <?php
	if(isset($_POST[$name])){
		echo " checked=\"checked\"";
	}
	?> /><?php
}

function continuePrompt(){
	global $INSTALL_VAR;
	$current_number = $INSTALL_VAR["continue_prompt_count"]++;
	?><input type="hidden" name="continue_prompt_<?= $current_number ?>" /><?php
	if(array_key_exists("continue_prompt_". $current_number, $_POST) == false){
		return false;
		
	}else{
		return true;
	}

}

function saveUrl($url, $file_name){
    $curl = curl_init();

    curl_setopt($curl, CURLOPT_URL, $url);
    curl_setopt($curl, CURLOPT_RETURNTRANSFER, true);
    curl_setopt($curl, CURLOPT_HEADER, false);
	curl_setopt($curl, CURLOPT_HTTPHEADER, array('Accept-Encoding: gzip,deflate'));
	curl_setopt($curl, CURLOPT_ENCODING, 1);


	writeInfo($color, "Downloading (". $file_name .")...");

    $download_string = curl_exec($curl);
    curl_close($curl);

	if($download_string === true){
		writeInfo("red", "Failed downloading ". $file_name .".  Please manually download ". $file_name ." and place it in the same directory as the ". $_SERVER["PHP_SELF"] .".");
		return false;

	}else{
		file_put_contents($file_name, $download_string);
		writeInfo($color, $file_name ." downloaded successfully.");
		return true;
	}
}




function connectToDb(){
	global $CONFIG;

	$database_connection = @mysql_connect($CONFIG["mysql_server"], $CONFIG["mysql_user"], $CONFIG["mysql_password"]);
	if($database_connection){
		$db_link = @mysql_select_db($CONFIG["mysql_database"]);
		if(!$db_link) {
			writeInfo("red", "Database selection error: ". mysql_error());
			return false;

		}else{
			writeInfo("green", "Database successfully connected.");
			return true;
		}
	}else{
		writeInfo("red", "Could not connect to database server.");
		return false;
	}
}

?>