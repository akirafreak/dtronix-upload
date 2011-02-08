<?php

function theme_main_header(){
	global $T_VAR;
?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN"
	"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en" lang="en">
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>

	<title>dtxUpload File Manager</title>

	<!-- StyleSheet -->
	<link href="<?php echo $T_VAR["theme_url"]; ?>default.css" rel="stylesheet" type="text/css" />
	<link href="default.css" type="text/css"/>


	<!-- Scripts -->
	<script src="<?php echo $T_VAR["theme_url"]; ?>js/mootools.js" type="text/javascript"></script>
	<script src="<?php echo $T_VAR["theme_url"]; ?>js/main.js" type="text/javascript"></script>
	<script src="<?php echo $T_VAR["theme_url"]; ?>js/md5.js" type="text/javascript"></script>

</head>
<body>
<div style="position: absolute; top: 0; left: 0; right: 0; bottom: 0; background-color: #303030;" id="hider"></div>
<?php
}


function theme_main_footer(){
	global $T_VAR;
?>
<div id="BumpBar"></div>
</body>
</html>

<?php

}

function theme_main_login(){
	global $_USER, $T_VAR;
?>
<div id="login_div" style="top: 50%;" class="center_div">
	<div class="header_text">Login</div>
	<form action="" id="login_form">
		<table id="form_table" cellpadding="3">
			<tbody>
				<tr>
					<td><label>Username:</label></td>
					<td><input type="text" class="login_inputs" id="input_username" /></td>
				</tr>
				<tr>
					<td><label>Password:</label></td>
					<td><input type="password" class="login_inputs" id="input_password" /></td>
				</tr>

				<tr>
					<td></td>
					<td align="right" height="50">
						<a href="#" id="button_register">Register</a> &ensp; &ensp;
						<input type="submit" value="Login" style="background-color: #303030; border: 2px dashed #535252;" />
					</td>
				</tr>
			</tbody>
		</table>
	</form>
</div>
<?php
}

function theme_main_register(){
	global $_USER, $T_VAR;
?>
<div id="register_div" style="top: 50%; margin-left: -220px;" class="center_div">
	<div class="header_text">Register</div>
	<form action="" id="register_form">
		<table id="form_table" cellpadding="3">
			<tbody>
				<tr>
					<td><label>Username:</label></td>
					<td><input type="text" class="login_inputs" id="register_username" /></td>
				</tr>
				<tr>
					<td><label>Password:</label></td>
					<td><input type="password" class="login_inputs" id="register_password" /></td>
				</tr>
				<tr>
					<td><label>Verify Password:</label></td>
					<td><input type="password" class="login_inputs" id="register_repassword" /></td>
				</tr>
				<tr>
					<td><label>Email:</label></td>
					<td><input type="text" class="login_inputs" id="register_email" /></td>
				</tr>
				<tr>
					<td><label>I Agree to the terms and conditions:</label></td>
					<td align="left"><input type="checkbox" id="register_agree" value="" style="color: #ffffff;"/></td>
				</tr>
				<tr>
					<td></td>
					<td align="right" height="50">
						<a href="#" id="button_cancel_register">Cancel</a> &ensp; &ensp;
						<input type="submit" value="Register" id="register_go" style="background-color: #303030; border: 2px dashed #535252;" />
					</td>
				</tr>
			</tbody>
		</table>
	</form>
</div>
<?php
}


function theme_main_displayFiles(){
	global $_USER, $T_VAR;
?>
<div id="files_container">
	<a href="#" id="user_info_link"></a>
	<a href="#" style="position: absolute; float: right; margin-left: 50px;" id="logout_link">Logout</a>
	<div id="user_info">
		<table cellpadding="5">
			<tbody>
				<tr>
					<td width="100">Used Space:</td>
					<td width="100" id="info_used_space"></td>
					<td width="150">Total Files Uploaded:</td>
					<td width="100" id="info_total_files"></td>
					<td width="150">Maximum File Size:</td>
					<td width="100" id="info_max_upload_size"></td>
				</tr>
			</tbody>
		</table>
	</div><br />
	<table style="width: 100%;" cellpadding="2">
		<thead>
			<tr>
				<td width="400"><u>Filename</u></td>
				<td width="200"><u>Date</u></td>
				<td></td>
			</tr>
			<tr class="uploaded_file" id="uploaded_file_clone">
				<td><a href="#" target="_blank">(DEFAULT)</a></td>
				<td>(DEFAULT)</td>
				<td>
					<a href="#">(Modify)</a>
					<a href="#">(Delete)</a>
				</td>
			</tr>
		</thead>
		<tbody id="uploaded_files_inject">

		</tbody>
	</table>
</div>
<?php
}

function theme_main_adminInterface(){
	global $_USER, $T_VAR;
?>
<div id="admin_container">

</div>
<?php
}



function theme_main_default(){
	global $_USER, $T_VAR;
	
	theme_main_header();

	theme_main_login();
	theme_main_register();
	theme_main_displayFiles();
	theme_main_adminInterface();

	theme_main_footer();
}


?>