<?php

head();

initialFileWriteChecks();
basicConfigurations();
mysqlConfigurations();

foot();



function head(){
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
<div style="width: 950px;" align="left">
<form method="post" action="install.php">
<?php }


function foot(){ ?>
	<input type="submit" value="Continue" />
</form>
</div>
</div>
</body>
</html>
<?php 
die();
}

function initialFileWriteChecks(){ 
	$write_error = false;
	$exist_error = false;
	$die_error = false;
	?>
<span style="font-size: 18px;">Checking File Writing Permissions</span>
<table>
	<tbody>
		<tr>
			<td>This Directory</td>
			<td><?php
				if(is_writable(getcwd())){
					?><span style="color: darkgreen;">Read-Write</span><?php
				}else{
					?><span style="color: darkred;">Read Only</span><?php
				}
			?></td>
		</tr>
		<tr>
			<td>Files Directory:</td>
			<td><?php

				if(is_dir(getcwd(). "/Files/")){
					if(is_writable(getcwd(). "/Files/")){
						?><span style="color: darkgreen;">Read-Write</span><?php
					}else{
						$write_error = true;
						?><span style="color: darkred;">Read Only</span><?php
					}

				}else{
					$exist_error = true;
					?><span style="color: darkred;">Folder Does Not Exist</span><?php
				}
			?></td>
		</tr>
		<tr>
			<td>Themes Directory:</td>
			<td><?php

				if(is_dir(getcwd(). "/Themes/")){
					if(is_writable(getcwd(). "/Themes/")){
						?><span style="color: darkgreen;">Read-Write</span><?php
					}else{
						$write_error = true;
						?><span style="color: darkred;">Read Only</span><?php
					}

				}else{
					$exist_error = true;
					?><span style="color: darkred;">Folder Does Not Exist</span><?php
				}
			?></td>
		</tr>

		<tr>
			<td>Config File:</td>
			<td><?php

				if(file_exists(getcwd(). "/config.php")){
					?><span style="color: darkred;">Configuration File Already Exists.  Aborting...</span><?php
					$die_error = true;
				}else{
					?><span style="color: darkgreen;">Config file does not exist.</span><?php
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
	$error = false;
	if($_POST["installation_level"] == "1"){
	?><input type="hidden" name="installation_level" value="2" /><?php
	} ?>
	<br /><br />
	<span style="font-size: 18px;">Basic Configurations (Default values are most likely correct)</span>
	<table>
		<tbody>
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
					echo dirname($_SERVER["SCRIPT_FILENAME"]);
				}else{
					echo $_POST["basic_baseDir"];
				}
				?>" /></td>
				<td><?php
				if(!empty($_POST["basic_baseDir"])){
					if(is_dir($_POST["basic_baseDir"])){
						?><span style="color: darkgreen;">Directory Exists</span><?php

					}else{
						$error = true;
						?><span style="color: darkred;">Directory Does Not Exists</span><?php
					}
				}
				?></td>
			</tr>

		</tbody>
	</table>
<?php 
	if($error) foot();
}

function mysqlConfigurations(){
	if($_POST["installation_level"] < 2) return false;
	$error = false;

	if($_POST["installation_level"] == "2"){
	?><input type="hidden" name="installation_level" value="3" /><?php
	} ?>
	<br /><br />
	<span style="font-size: 18px;">MySQL Database Configurations</span>
	<table>
		<tbody>
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
						?><span style="color: darkred;">Connection Error: <?php echo mysql_error(); ?></span><?php
					}else{
						?><span style="color: darkgreen;">Connected Successfully</span><?php
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
						?><span style="color: darkred;">Database Selection Error: <?php echo mysql_error(); ?></span><?php
					}else{
						?><span style="color: darkgreen;">Database Successfully Connected</span><?php
					}
				}
				?></td>
			</tr>
		</tbody>
	</table>
<?php 
	if($error) foot();
}


?>