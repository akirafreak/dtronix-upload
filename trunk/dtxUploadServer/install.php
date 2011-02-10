<?php
// Define this before all the includes to ensure that all the files are actually included through this main file.
define("requireParrent", true);

$INSTALL_VAR["download_beta"] = false;

head();

checkPHPVersion();
checkForInstallationFiles();
initialFileWriteChecks();
basicConfigurations();
mysqlConfigurations();
mysqlInstallation();
configFileCreation();

foot();



function head(){
	if(file_exists("config.php") && !isset($_POST["delete_installation_files"])) die("Config file already exists.  Exiting installer.");
	
	if(empty($_POST["installation_level"])){
		$_POST["installation_level"] = "1";
	}
?><!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN"
	"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en" lang="en">
<head>
<style>
	* {
		font-family:monospace;
		font-size: 11px;
	}
</style>
</head>
<body>
<div style="width: 100%;" align="center">
<div style="width: 950px;" align="left"><?php
	if(isset($_POST["delete_installation_files"])){ ?>
<form method="post" action="dtxUpload.php">
<span style="font-size: 20px; text-decoration: underline;">Deleting Installation Files...</span><br/><br/><?php
		deleteFile("install.php");
		deleteFile("install.data.php");
		deleteFile("install.version.php");
		foot("Go to Dtronix Upload");
	}else{ ?>
<form method="post" action="install.php">
<span style="font-size: 20px; text-decoration: underline;">Dtronix Upload Server Installer</span><br/><br/><?php
	}

}

function deleteFile($file){
	if(!file_exists($file)) return false;
	if(unlink($file)){
		?><span style="color: green;">Deleted <?php echo $file; ?></span><br /><?php
	}else{
		?><span style="color: red;">Could not delete <?php echo $file; ?></span><br /><?php
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

function checkPHPVersion(){ ?>
<span style="font-size: 18px;">Checking PHP Version.</span><br /><?php

	if(version_compare(PHP_VERSION, '5.0.0') >= 0){
		?><span style="color: green;">You are running PHP <?php echo PHP_VERSION; ?> which is supported.</span><br /><br /><?php

	}else{
		if(!file_exists(".htaccess")){
			?><span style="color: red;">You are running PHP <?php echo PHP_VERSION; ?> which is not supported.  Trying to force PHP 5 support...</span><br /><?php

			$stream = @fopen(".htaccess", 'a');
			if (!$stream) {
				?><span style="color: red;">Unable to create .htaccess file to force PHP5 support.</span><br /><?php

			} else {
				$bytes = fwrite($stream, "AddHandler application/x-httpd-php5 .php");
				fclose($stream);
				if($bytes === false){
					?><span style="color: red;">Unable to create .htaccess file to force PHP5 support.</span><br /><?php

				}else{
					?><span style="color: green;">Created .htaccess file to force PHP5 support.</span><br /><?php
				}
			}

		}else{
			?><span style="color: red;">Unable to force PHP5 support.  Your server will not support Dtronix Upload.</span><br /><?php
		}
		foot();
	}
}




function checkForInstallationFiles(){
	global $INSTALL_VAR;
	$download_server = ($INSTALL_VAR["download_beta"])? "http://upload-beta.dtronix.com/" : "http://upload.dtronix.com/"; ?>
<span style="font-size: 18px;">Checking for installation files.</span>
<table>
	<tbody valign="top">
		<tr>
			<td>This Directory</td>
			<td><?php
				if(is_writable(getcwd())){
					?><span style="color: green;">Read-Write</span><?php
				}else{
					?><span style="color: red;">Read Only (Must be read-write!)</span><?php
					die();
				}
			?></td>
		</tr>
		<tr>
			<td>Verifying installation files:</td>
			<td><?php

				if(file_exists("install.data.php") && file_exists("install.version.php") && !isset($_GET["force_download"])){
					?><span style="color: green;">Required files Exist. <a href="install.php?force_download=true">(Force download latest version)</a></span><?php
				}else{
					if(!isset($_GET["force_download"])){
						?><span style="color: red;">Required installation files do not exist.</span><br /><?php
					}
					saveUrl($download_server . "dtxUpload.php?action=update_data_file", "install.data.php");
					saveUrl($download_server . "dtxUpload.php?action=update_version_file", "install.version.php");
				}
			?></td>
		</tr>
		<tr>
			<td>Installation Files Version:</td>
			<td><?php
				require("install.version.php");
				?><span style="color: green;">Version <?php echo $_INSTALL_VERSION[0]; ?></span><?php
			?></td>
		</tr><?php
		if((isset($_GET["force_download"]) && file_exists("dtxUpload.php")) || !file_exists("dtxUpload.php")){ ?>
		<tr>
			<td>Extracting:</td>
			<td><?php
				require("install.data.php");

				?><span style="color: green;">Creating folders...</span><br /><?php

				$base_dir = dirname($_SERVER["SCRIPT_FILENAME"]);
				foreach($_INSTALLER_DIRS as $dir){
					if(is_dir($base_dir . $dir)){
						?><span style="color: green;">Folder "<?php echo $base_dir . $dir; ?>" already exists.</span><br /><?php
						
					}else{
						if(mkdir($base_dir . $dir, 0777, true)){
							?><span style="color: green;">Created folder "<?php echo $base_dir . $dir; ?>"</span><br /><?php

						}else{
							?><span style="color: red;">Failed creating folder "<?php echo $base_dir . $dir; ?>"</span><br /><?php
						}
					}
				}

				?><span style="color: green;">Creating Files...</span><br /><?php

				foreach($_INSTALLER_FILE as $file_name => $base64_file){
					$file = base64_decode($base64_file);
					if(file_put_contents($base_dir . $file_name, $file) === false){
						?><span style="color: red;">Failed creating file "<?php echo $file_name; ?>"</span><br /><?php

					}else{
						?><span style="color: green;">Created file "<?php echo $file_name; ?>"</span><br /><?php
					}
				}
			?></td>
		</tr><?php
			foot();
		}
		?>
	</tbody>
</table><?php
	
}

function saveUrl($url, $file_name){
    $curl = curl_init();

    curl_setopt($curl, CURLOPT_URL, $url);
    curl_setopt($curl, CURLOPT_RETURNTRANSFER, true);
    curl_setopt($curl, CURLOPT_HEADER, false);

	?><span style="color: green;">Downloading (<?php echo $file_name; ?>)...</span><br /><?php

    $str = curl_exec($curl);
    curl_close($curl);

	if($str === true){
		?><span style="color: red;">Failed downloading <?php echo $file_name; ?>.  Please manually download <?php echo $file_name; ?> and place it in the same directory as the install.php.</span><br /><?php
		die();

	}else{
		file_put_contents($file_name, $str);
		?><span style="color: green;"><?php echo $file_name; ?> downloaded successfully.</span><br /><br /><?php
	}
}

function initialFileWriteChecks(){ 
	$write_error = false;
	$exist_error = false;
	$die_error = false; ?>
<span style="font-size: 18px;">Installer Update</span><br/>
<a href="install.php?force_download=true">Force download the latest installation data files.</a><br/><br/>

<span style="font-size: 18px;">Checking File Writing Permissions</span>
<table>
	<tbody valign="top">
		<tr>
			<td>This Directory</td>
			<td><?php
				if(is_writable(getcwd())){
					?><span style="color: green;">Read-Write</span><?php
				}else{
					?><span style="color: red;">Read Only</span><?php
				}
			?></td>
		</tr>
		<tr>
			<td>Files Directory:</td>
			<td><?php

				if(is_dir(getcwd(). "/Files/")){
					if(is_writable(getcwd(). "/Files/")){
						?><span style="color: green;">Read-Write</span><?php
					}else{
						$write_error = true;
						?><span style="color: red;">Read Only</span><?php
					}

				}else{
					$exist_error = true;
					?><span style="color: red;">Folder Does Not Exist</span><?php
				}
			?></td>
		</tr>
		<tr>
			<td>Theme Directory:</td>
			<td><?php

				if(is_dir(getcwd(). "/Theme/")){
					if(is_writable(getcwd(). "/Theme/")){
						?><span style="color: green;">Read-Write</span><?php
					}else{
						$write_error = true;
						?><span style="color: red;">Read Only</span><?php
					}

				}else{
					$exist_error = true;
					?><span style="color: red;">Folder Does Not Exist</span><?php
				}
			?></td>
		</tr>

		<tr>
			<td>Config File:</td>
			<td><?php

				if(file_exists(getcwd(). "/config.php")){
					?><span style="color: red;">Configuration File Already Exists.  Aborting...</span><?php
					$die_error = true;
				}else{
					?><span style="color: green;">Config file does not exist.</span><?php
				}
			?></td>
		</tr>
	</tbody>
</table>
<?php
	if($exist_error){
		?><span style="font-size: 15px; color: red;">Errors detected with folder structure.  Please check to see that you copied all the install files to the server.</span><br /><?php
		foot();
	}
	if($write_error){
		?><span style="font-size: 15px; color: red;">Permission have not been set to the folders marked in read above.  <br/>Please ensue the folders marked above have CHMOD 777</span><br /><?php
		foot();
	}
	if($die_error){
		foot();
	}

}


function basicConfigurations(){ 
	$error = false;?>

		<input type="hidden" name="installation_level" value="2" />
	<br /><br />
	<span style="font-size: 18px;">Basic Configurations (Default values are most likely correct)</span>
	<table>
		<tbody valign="top">
			<tr>
				<td>Base URL:</td>
				<td><input type="text" name="basic_baseUrl" style="width: 400px;" value="<?php
				if(empty($_POST["basic_baseUrl"])){
					echo "http://" . $_SERVER["HTTP_HOST"] . dirname($_SERVER["SCRIPT_NAME"]);
				}else{
					echo $_POST["basic_baseUrl"];
				}
				?>" /></td>
			</tr>

			<tr>
				<td>Base Directory:</td>
				<td><input type="text" name="basic_baseDir" style="width: 400px;" value="<?php
				if(empty($_POST["basic_baseDir"])){
					echo dirname($_SERVER["SCRIPT_FILENAME"]) . "/";
				}else{
					echo $_POST["basic_baseDir"];
				}
				?>" /></td>
				<td><?php
				if(!empty($_POST["basic_baseDir"])){
					if(is_dir($_POST["basic_baseDir"])){
						?><span style="color: green;">Directory Exists</span><?php

					}else{
						$error = true;
						?><span style="color: red;">Directory Does Not Exists</span><?php
					}
				}
				?></td>
			</tr>
			<tr>
				<td>Server Name:</td>
				<td><input type="text" name="basic_serverName" style="width: 400px;" value="<?php
				if(!empty($_POST["basic_serverName"])){
					echo $_POST["basic_serverName"];
				}
				?>" /></td>
			</tr>
			<tr>
				<td>Server Email:</td>
				<td><input type="text" name="basic_serverEmail" style="width: 400px;" value="<?php
				if(!empty($_POST["basic_serverEmail"])){
					echo $_POST["basic_serverEmail"];
				}
				?>" /></td>
			</tr>
		</tbody>
	</table>
<?php 
	if($error) foot();
}

function mysqlConfigurations(){
	if($_POST["installation_level"] < 2) return false;
	$error = false;?>
	
	<input type="hidden" name="installation_level" value="3" />
	<br /><br />
	<span style="font-size: 18px;">MySQL Database Configurations</span>
	<table>
		<tbody valign="top">
			<tr>
				<td>MySQL Server:</td>
				<td><input type="text" name="mysql_server" style="width: 400px;" value="<?php
				if(empty($_POST["mysql_server"])){
					echo "localhost";
				}else{
					echo $_POST["mysql_server"];
				}
				?>" /></td>
				<td>(This is almost always "localhost")</td>
			</tr>

			<tr>
				<td>MySQL Database Username:</td>
				<td><input type="text" name="mysql_username" style="width: 400px;" value="<?php
				if(!empty($_POST["mysql_username"])){
					echo $_POST["mysql_username"];
				}
				?>" /></td>
				<td></td>
			</tr>

			<tr>
				<td>MySQL Database Password:</td>
				<td><input type="text" name="mysql_password" style="width: 400px;" value="<?php
				if(!empty($_POST["mysql_password"])){
					echo $_POST["mysql_password"];
				}
				?>" /></td>
				<td><?php
				if(!empty($_POST["mysql_server"]) && !empty($_POST["mysql_username"]) && !empty($_POST["mysql_password"])){
					$connection = @mysql_connect($_POST["mysql_server"], $_POST["mysql_username"], $_POST["mysql_password"]);
					if(!$connection) {
						$error = true;
						?><span style="color: red;">Connection Error: <?php echo mysql_error(); ?></span><?php
					}else{
						?><span style="color: green;">Connected Successfully</span><?php
					}
				}
				?></td>
			</tr>

			<tr>
				<td>MySQL Database:</td>
				<td><input type="text" name="mysql_database" style="width: 400px;" value="<?php
				if(!empty($_POST["mysql_database"])){
					echo $_POST["mysql_database"];
				}
				?>" /></td>
				<td><?php
				if(!empty($_POST["mysql_database"]) && $connection){
					$db_link = @mysql_select_db($_POST["mysql_database"]);
					if(!$db_link) {
						$error = true;
						?><span style="color: red;">Database Selection Error: <?php echo mysql_error(); ?></span><?php
					}else{
						?><span style="color: green;">Database Successfully Connected</span><?php
					}
				}
				?></td>
			<tr>
				<td>Existing Dtronix Upload DB?</td>
				<td><input type="checkbox" name="mysql_existing_db" <?php
				if(isset($_POST["mysql_existing_db"])){
					echo " checked=\"checked\"";
				}
				?> /></td>
			</tr>
		</tbody>
	</table>
<?php 
	if($error) foot();
}

function mysqlInstallation(){
	if($_POST["installation_level"] < 3 || empty($_POST["mysql_server"]) || empty($_POST["mysql_username"]) || empty($_POST["mysql_password"]) || empty($_POST["mysql_database"])) return false;
	$error = false; ?>
<input type="hidden" name="installation_level" value="4" />
<br /><br />
<span style="font-size: 18px;">MySQL Database Creation</span><br/><?php
	if(!isset($_POST["mysql_existing_db"])){
	mysqlTableCreate("files", "CREATE TABLE IF NOT EXISTS `files` (
		`id` int(11) NOT NULL AUTO_INCREMENT,
		`owner_id` int(11) NOT NULL,
		`url_id` varchar(32) NOT NULL,
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
	) ENGINE=MyISAM  DEFAULT CHARSET=latin1 AUTO_INCREMENT=1;");

	mysqlTableCreate("users", "CREATE TABLE IF NOT EXISTS `users` (
		`id` int(11) NOT NULL AUTO_INCREMENT,
		`username` varchar(32) NOT NULL,
		`password` varchar(32) NOT NULL,
		`session_key` varchar(32) NOT NULL,
		`session_last_active` datetime NOT NULL,
		`registration_date` date NOT NULL,
		`email` varchar(128) NOT NULL,
		`is_email_validated` tinyint(1) NOT NULL,
		`account_verification_code` varchar(32) NOT NULL,
		`permissions` int(11) NOT NULL,
		`total_files_uploaded` int(11) NOT NULL DEFAULT '0',
		`total_uploaded_filesizes` int(11) NOT NULL DEFAULT '0',
		PRIMARY KEY (`id`),
		UNIQUE KEY `id` (`id`),
		KEY `username` (`username`),
		KEY `session_id` (`session_key`)
	) ENGINE=MyISAM  DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;");

	mysqlTableCreate("users_permissions", "CREATE TABLE IF NOT EXISTS `users_permissions` (
		`id` int(11) NOT NULL,
		`name` varchar(64) NOT NULL,
		`is_disabled` tinyint(1) NOT NULL,
		`can_connect` tinyint(1) NOT NULL,
		`can_upload` tinyint(1) NOT NULL,
		`manage_users` tinyint(1) NOT NULL,
		`manage_uploads` tinyint(1) NOT NULL,
		`full_access` tinyint(1) NOT NULL,
		`max_upload_space` int(11) NOT NULL,
		`max_upload_size` int(11) NOT NULL,
		`default_permission_set` tinyint(1) NOT NULL COMMENT 'True if the current record is part of the default permission system.',
		PRIMARY KEY (`id`)
	) ENGINE=MyISAM DEFAULT CHARSET=latin1;");

	mysqlRowInsert("users_permissions", "INSERT INTO `users_permissions` (`id`, `name`, `is_disabled`, `can_connect`, `can_upload`, `manage_users`, `manage_uploads`, `full_access`, `max_upload_space`, `max_upload_size`, `default_permission_set`) VALUES
		(0, 'Admin', 0, 1, 1, 1, 1, 1, 0, 0, 1),
		(1, 'User', 0, 1, 1, 0, 0, 0, 100000, 20000, 1),
		(2, 'Banned', 1, 0, 0, 0, 0, 0, -1, -1, 1);");
	?><input type="hidden" name="mysql_existing_db" value="true" /><?php
	}else{
		?><span style="color: darkorange;">MySQL database creation/insertion skipped due to existing databases.</span><br/><?php
	}

}

function mysqlTableCreate($name, $sql){
	$query = mysql_query($sql);
	if(!$query){
		?><span style="color: red;">Table "<?php echo $name; ?>" creation error: <?php echo mysql_error(); ?></span><br/><?php
		foot();
	}else{
		?><span style="color: green;">Table "<?php echo $name; ?>" created successfully.</span><br /><?php
	}
}

function mysqlRowInsert($name, $sql){
	$query = mysql_query($sql);
	if(!$query){
		?><span style="color: red;">Could not insert records into "<?php echo $name; ?>". <?php echo mysql_error(); ?></span><br/><?php
		foot();
	}else{
		?><span style="color: green;">Successfully inserted records into "<?php echo $name; ?>".</span><br /><?php
	}
}

function configFileCreation(){
	if($_POST["installation_level"] < 4) return false;
	$error = false;
	require("functions.php"); ?>
<br /><br />
<span style="font-size: 18px;">Configuration File Creation</span><br/><?php


	$_CONFIG["uri"] = $_POST["basic_baseUrl"];
	$_CONFIG["upload_base_url"] = $_POST["basic_baseUrl"];
	$_CONFIG["dir"] = $_POST["basic_baseDir"];
	$_CONFIG["upload_dir"] = $_POST["basic_baseDir"] . "Files/";

	$_CONFIG["mysql_server"] = $_POST["mysql_server"];
	$_CONFIG["mysql_database"] = $_POST["mysql_database"];
	$_CONFIG["mysql_password"] = str_replace(array('"', '$'), array('\"', '\$'), $_POST["mysql_password"]);
	$_CONFIG["mysql_user"] = $_POST["mysql_username"];
	$_CONFIG["mysql_id"] = false;

	$_CONFIG["email_activaction_required"] = false;
	$_CONFIG["server_allowed_filetypes"] = "all";
	$_CONFIG["server_max_upload_filesize"] = 1024 * 50;
	$_CONFIG["server_maintenance_mode"] = false;
	$_CONFIG["server_name"] = $_POST["basic_serverName"];
	$_CONFIG["server_email"] = $_POST["basic_serverEmail"];
	$_CONFIG["server_logo"] = false;

	$_CONFIG["html_theme"] = "kiss";

	$_CONFIG["update_broadcast"] = false;

	$_CONFIG["registration_allowed"] = true;
	$_CONFIG["registration_verify_email"] = false;

	$_CONFIG["session_max_life"] = 60 * 2000;

	saveConfigFile($_CONFIG);

	?><span style="color: green;">Successfully created configuration file.</span><br /><br />
	<input type="hidden" name="delete_installation_files" value="true" />
	<span style="font-size: 18px; color: green;">Setup Complete!</span><br/><?php

	foot("Delete Setup Files");
}



?>