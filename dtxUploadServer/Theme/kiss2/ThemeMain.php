<?php

class ThemeMain extends ThemeBase{

	public function header(){ ?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN"
	"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en" lang="en">
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>

	<title>dtxUpload File Manager</title>

	<!-- StyleSheet -->
	<link href="<?= $this->T_VAR["theme_url"] ?>default.css" rel="stylesheet" type="text/css" />


	<!-- Scripts -->
	<script src="<?= $this->T_VAR["theme_url"] ?>js/mootools-core-1.3.2-full-nocompat.js" type="text/javascript"></script>
	<script src="<?= $this->T_VAR["theme_url"] ?>js/mooml.js" type="text/javascript"></script>
	<script src="<?= $this->T_VAR["theme_url"] ?>js/roar.js" type="text/javascript"></script>
	<script src="<?= $this->T_VAR["theme_url"] ?>js/md5.js" type="text/javascript"></script>
	<script src="<?= $this->T_VAR["theme_url"] ?>js/main.js" type="text/javascript"></script>
</head>
<body>
<div id="server_info_bar">
	<div id="server_name"><?= $this->CONFIG["server_name"] ?></div>
	<div id="BumpBar" align="center"></div>
	<div id="server_stats">
		
	</div>
	
</div>
<div id="server_action_bar"><?php
	}
	
	public function actionBarFooter(){ ?>

</div><?php
	}


	public function footer(){ ?>

</body>
</html><?php
	}
	
	
	public function actions(){ ?>

	<div id="action_group">
		<a href="#view_files" id="action_view_files" class="action_links">View Files</a>
		<a href="#configure" id="action_configure" class="action_links">Configurations</a>
	</div><?php
	}

	public function login(){ ?>

	<div id="user_info">
		Logged in as <span id="user_info_username"></span>&ensp;
		<a href="#logout" id="action_logout">Logout</a>
	</div>
	<div id="login_group">
		<form action="" id="login_form">
			<label>Username:</label>
			<input type="text" id="input_username" />

			<label>Password:</label>
			<input type="password" id="input_password" />
			<input type="submit" value="Login" />&ensp;

			<a href="#" id="button_register">Register</a> 
		</form>
	</div><?php
	}

	public function register(){ ?>

<div id="register_div" align="center">
	<div style="font-size: 16px; text-decoration: underline; margin: 10px;" align="left">Register New Account</div>
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
						<input type="submit" value="Register" id="register_go"/>
					</td>
				</tr>
			</tbody>
		</table>
	</form>
</div><?php
	}
	
	public function config(){ ?>

<div id="config_container" align="center">
	<div style="width: 800px; color: #ffffff;" align="left">
			THis is a config item.
	<button id="action_close_config">Cancel</button>
	</div>
</div>
		<?php
	}


	public function displayFiles(){ ?>
<div align="center">
	<div id="files_container">
		<table style="width: 100%;" cellpadding="2">
			<thead>
				<tr>
					<td width="400"><u>Filename</u></td>
					<td width="100"><u>Size</u></td>
					<td width="100"><u>Date</u></td>
					<td></td>
				</tr>
			</thead>
			<tbody id="uploaded_files_inject">

			</tbody>
		</table>
	</div>
</div><?php
	}

	public function adminInterface(){ ?>

<div id="admin_container">

</div>
	<?php
	}
}


?>