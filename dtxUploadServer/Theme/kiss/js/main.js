// Anonymize every action taken on the site.
(function(){

var main = {

	$el: {},

	options:{
		tween_duration: 500,
		request_url: "dtxUpload.php",
		upload_base_url: ""
	},

	debug: "", //"?XDEBUG_SESSION_START=netbeans-xdebug",

	initialize: function(){
		// Setup elements for the script.
		this.addElements(
			"login_form",
			"input_username",
			"input_password",
			"button_register",
			"login_div",
			"login_form",
			"hider",
			"button_cancel_register",
			"files_container",

			"register_div",
			"register_form",
			"register_username",
			"register_password",
			"register_repassword",
			"register_email",
			"register_agree",
			"register_go",

			"user_info_link",
			"uploaded_files_inject",
			"uploaded_file_clone",
			"logout_link",
			"info_used_space",
			"info_total_files",
			"info_max_upload_size"
		);
			
		var self = this;

		page: 0,

		this.registerRequest = new Request.JSON({
			url: "dtxUpload.php" + this.debug,
			noCache: true,
			onSuccess: function(data){
				self.registerActions(data);
			},
			onFailure: function(){
				self.registerActions({"function": "request_failure"});
			}
		});

		this.loginRequest = new Request.JSON({
			url: this.options.request_url + this.debug,
			noCache: true,
			onSuccess: function(data){
				this.loginActions(data);
			}.bind(this),
			onFailure: function(){
				this.loginActions({"function": "request_failure"});
			}.bind(this)
		});

		this.logoutRequest = new Request.JSON({
			url: this.options.request_url + this.debug,
			noCache: true,
			onSuccess: function(data){
				this.logoutActions(data);
			}.bind(this),
			onFailure: function(){
				this.logoutActions({"function": "request_failure"});
			}.bind(this)
		});

		this.loadFilesRequest = new Request.JSON({
			url: this.options.request_url + this.debug,
			noCache: true,
			onSuccess: function(data){
				self.loadFilesActions(data);
			},
			onFailure: function(){
				self.loadFilesActions({"function": "request_failure"});
			}

		});
		
		this.loadUserInfoRequest = new Request.JSON({
			url: this.options.request_url + this.debug,
			noCache: true,
			onSuccess: function(data){
				self.loadUserInfoActions(data);
			},
			onFailure: function(){
				self.loadUserInfoActions({"function": "request_failure"});
			}

		});

		this.fileActionRequest = new Request.JSON({
			url: this.options.request_url + this.debug,
			noCache: true,
			onSuccess: function(data){
				self.fileActionActions(data);
			},
			onFailure: function(){
				self.fileActionActions({"function": "request_failure"});
			}
		});

		this.$el.register_div.setStyles({
			"opacity": 0,
			"display": "block"
		});

		BumpBar.pushNew("Loaded", "blue");

		this.eventRegistration();

		// Attempt to auto login.
		if(Cookie.read("session_key") != null){
			this.loginRequest.get({
				"action": "user_verification"
			});
		}

		this.$el.input_username.focus();
	},

	addElements: function(){
		for(var i = 0; i < arguments.length; i++){
			this.$el[arguments[i]] = $(arguments[i]);
		}
	},

	eventRegistration:function(){

		// Setup variables for the script.
		var self = this;

		this.$el.register_div.set("tween", {
			transition: Fx.Transitions.Expo.easeOut,
			duration: this.options.tween_duration
		});
		this.$el.login_div.set("tween", {
			transition: Fx.Transitions.Expo.easeOut,
			duration: this.options.tween_duration,
			unit:'%'
		});
		this.$el.hider.set("tween", {
			transition: Fx.Transitions.Expo.easeOut,
			duration: this.options.tween_duration
		}).setStyle("opacity", 0);

		// Login submit event.
		this.$el.login_form.addEvent("submit", function(){
			self.login();
			return false;
		});

		// Register button event.
		this.$el.button_register.addEvent("click", function(){
			self.$el.login_div.tween("top", "20%");
			self.$el.hider.inject(self.$el.register_div, "before");
			self.$el.hider.tween("opacity", 0.9);
			self.$el.register_div.tween("opacity", 1);

			return false;
		});

		// Cancel registration event.
		this.$el.button_cancel_register.addEvent("click", function(){
			self.$el.login_div.tween("top", "50%");
			self.$el.hider.tween("opacity", 0);
			self.$el.register_div.tween("opacity", 0);

			return false;
		});

		this.$el.register_form.addEvent("submit", function(){
			self.register();
			
			return false;
		});

		this.$el.logout_link.addEvent("click", function(){
			self.logout();

			return false;
		});



		
	},

	login: function(){
		BumpBar.pushNew("Submitting login information...");
		document.cookie = "";
		Cookie.write("client_username", this.$el.input_username.value);
		Cookie.write("client_password", MD5(this.$el.input_password.value));

		this.loginRequest.get({
			"action": "user_verification"
		})

	},

	loginActions: function(data){
		if(data["function"] == "validation_invalid_password"){
			BumpBar.pushNew("Invalid password provided for username.  Please verify password.", "red");
			this.$el.input_password.focus();

		}else if(data["function"] == "validation_successful"){
			this.displayFileInterface();
			this.$el.login_div.tween("opacity", 0);
			BumpBar.pushNew("Logged in successfully.  Loading interface...", "green");
			
		}else if(data["function"] == "validation_invalid_username"){
			BumpBar.pushNew("Username entered was invalid.  Please enter a valid username or register.", "red");
			this.$el.input_username.focus();

		}else if(data["function"] == "validation_expired_user_session"){
			BumpBar.pushNew("Session has expired.  Please login again.", "red");
			this.logout();

		}else if(data["function"] == "validation_invalid_user_session"){
			BumpBar.pushNew("Session is invalid.  Please login again.", "red");
			this.logout();

		}else if(data["function"] == "validation_failed_no_login"){
			//The automatic login failed due to no cookies being set.

		}else if(data["function"] == "request_failure"){
			BumpBar.pushNew("Can not connect to the server at this time.", "red");
		}else{
			BumpBar.pushNew("Something has gone horribly wrong.  Please try reloading the page.", "red");
		}
	},

	register: function(){
		var username = this.$el.register_username.value;
		var pass = this.$el.register_password.value;
		var repass = this.$el.register_repassword.value;
		var email = this.$el.register_email.value;

		// Username verification.

		if(username.length < 3){
			this.registerActions({"function": "registration_username_short"});
			return;
		}
		if(username.length > 15){
			this.registerActions({"function": "registration_username_long"});
			return;
		}
		if(!username.test("^[a-zA-Z0-9_-]*$")){
			this.registerActions({"function": "registration_username_invalid"});
			return;
		}

		if(pass.length < 8){
			this.registerActions({"function": "registration_password_short"});
			return;
		}
		if(pass != repass){
			BumpBar.pushNew("Passwords entered do not match.", "red");
			this.$el.register_repassword.focus();
			return;
		}
		
		if(!email.test(/^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$/i)){
			this.registerActions({"function": "registration_email_invalid"});
			return;
		}

		if(!this.$el.register_agree.checked){
			BumpBar.pushNew("You must agree to the terms and conditions.", "red");
			this.$el.register_agree.focus();
			return;
		}

		this.registerRequest.get({
			"action": "register",
			"args": [username, MD5(pass), email]
		})
	},

	registerActions: function(data){
		if(data["function"] == "registration_username_long"){
			BumpBar.pushNew("Username provided was too long.  Please enter a username that is between 3 and 15 characters.", "red");
			this.$el.register_username.focus();

		}else if(data["function"] == "registration_username_short"){
			BumpBar.pushNew("Username provided was too short.  Please enter a username that is between 3 and 15 characters.", "red");
			this.$el.register_username.focus();

		}else if(data["function"] == "registration_username_invalid"){
			BumpBar.pushNew("Username contains invalid characters.  You are only allowed to use alphanumeric characters, underscore and the dash.", "red");
			this.$el.register_username.focus();

		}else if(data["function"] == "registration_password_short"){
			BumpBar.pushNew("Password provided was too short.  Please enter a password that is at least 8 characters.", "red");
			this.$el.register_password.focus();

		}else if(data["function"] == "registration_email_invalid"){
			BumpBar.pushNew("Email entered was invalid.", "red");
			this.$el.register_email.focus();

		}else if(data["function"] == "registration_username_existing"){
			BumpBar.pushNew("Username already exists.", "red");
			this.$el.register_username.focus();

		}else if(data["function"] == "registration_email_existing"){
			BumpBar.pushNew("Email already exists.", "red");
			this.$el.register_email.focus();

		}else if(data["function"] == "registration_success_activated"){
			BumpBar.pushNew("Account created and activated.  Logging you in...", "green");

		}else if(data["function"] == "registration_failure_server"){
			BumpBar.pushNew("There was a registration error on the server.  Please contact support about this error.", "red");

		}else if(data["function"] == "request_failure"){
			BumpBar.pushNew("Can not connect to the server at this time.", "red");

		}else{
			BumpBar.pushNew("Something has gone horribly wrong.  Please try reloading the page.", "red");
		}
	},

	loadFiles: function(page){
		if(page == null) page = 0;
		this.$el.uploaded_files_inject.empty();

		this.loadFilesRequest.get({
			"action": "load_files_quick",
			"page": page
		});
	},

	loadFilesActions: function(data){
		if(data["function"] == "files_uploaded_quick"){
			this.displayFiles(data.data)

		}else if(data["function"] == "files_uploaded_no_files"){

		}else if(data["function"] == "request_failure"){
			BumpBar.pushNew("Can not connect to the server at this time.", "red");

		}else{
			BumpBar.pushNew("Something has gone horribly wrong.  Please try reloading the page.", "red");
		}
	},

	displayFiles: function(file_data){
		var self = this;
		this.$el.uploaded_files_inject.empty();
		file_data.forEach(function(file){
			var file_dom = self.$el.uploaded_file_clone.clone();
			var e_tds = file_dom.getElements("td");

			//File link and name.
			e_tds[0].getFirst().set({
				"html": file.file_name,
				"href": self.options.upload_base_url + file.url_id
			});

			//File upload date.
			e_tds[1].set("html", file.upload_date);

			// Actions !!!!!!!
			var action_links = e_tds[2].getElements("a");

			// Delete file
			action_links[1].addEvent("click", function(){
				self.fileAction(file.url_id, "delete");
			});


			file_dom.setStyle("display", "table-row");

			file_dom.inject(self.$el.uploaded_files_inject, "bottom");
		}, this);
	},
	
	fileAction: function(fid, action){
		if(action == "delete"){
			this.fileActionRequest.get({
				"action": "file_delete",
				"file_id": fid
			});
		}
	},

	fileActionActions: function(data){
		if(data["function"] == "file_delete_confirmation"){
			BumpBar.pushNew("File deleted. Reloading files.", "green");
			this.loadFiles(this.page);

		}else if(data["function"] == "file_delete_failure"){
			BumpBar.pushNew("Unable to delete file.  Please contact support.", "red");

		}else if(data["function"] == "request_failure"){
			BumpBar.pushNew("Can not connect to the server at this time.", "red");

		}else{
			BumpBar.pushNew("Something has gone horribly wrong.  Please try reloading the page.", "red");
		}
	},

	loadUserInfo: function(){
		this.loadUserInfoRequest.get({
			"action": "load_user_info"
		});
	},

	loadUserInfoActions: function(data){
		if(data["function"] == "user_info"){
			var info = data.data;

			this.options.upload_base_url = info.upload_base_url;
			this.$el.user_info_link.innerHTML = info.username;
			this.$el.info_used_space.innerHTML = "("+ info.total_uploaded_filesizes + "KB / " + info.max_upload_space + "KB)";
			this.$el.info_total_files.innerHTML = info.total_files_uploaded;
			this.$el.info_max_upload_size.innerHTML = info.max_upload_size + "KB";

			this.loadFiles(0);

		}else if(data["function"] == "request_failure"){
			BumpBar.pushNew("Can not connect to the server at this time.", "red");

		}else{
			BumpBar.pushNew("Something has gone horribly wrong.  Please try reloading the page.", "red");
		}
	},

	displayFileInterface: function(){
		this.loadUserInfo();

		this.$el.user_info_link.innerHTML = ""
		this.$el.files_container.setStyle("display", "block");
	},

	logout:function(){
		this.logoutRequest.get({
			"action": "logout"
		});
	},
	
	logoutActions: function(data){
		if(data["function"] == "logout_successful"){
			Cookie.dispose("session_key");
			this.$el.files_container.setStyle("display", "none");
			this.$el.login_div.tween("opacity", 1);
			BumpBar.pushNew("Logged out.", "green");

		}else if(data["function"] == "logout_failed"){
			BumpBar.pushNew("Unable to log you out from the server.  Please contact support.", "red");

		}else if(data["function"] == "request_failure"){
			BumpBar.pushNew("Can not connect to the server at this time.", "red");

		}else{
			BumpBar.pushNew("Something has gone horribly wrong.  Please try reloading the page.", "red");
		}

	}

};

var BumpBar = {
	$el: {},

	options:{
		speed: 500,
		screen_time: 5000
	},

	initialize: function(){
		if(this.$el.bar_container == null){
			this.$el.bar_container = $("BumpBar");
		}
	},

	pushNew: function(text, color){
		switch(color){
			case "red":color = "#ad0000";break;
			case "green":color = "#00ad01";break;
			case "blue":color = "#0078ad";break;
			default:color = "#0078ad";break;
		}
		var el;
		var self = this;
		var destroy_bar = function(){
			el.morph({
				"opacity": 0,
				"height": 0
			});
			(function(){
				el.destroy();
			}).delay(self.options.speed)
		}
		el = new Element("div", {
			"class": "BumpBarNotification",
			"text": text,
			"styles": {
				"background-color": color,
				"opacity": 0
			},
			"events": {
				"click": function(){
					destroy_bar();
				}
			},
			"morph": {
				"duration": self.options.speed,
				"transition": Fx.Transitions.Quint.easeOut
			}
		}).inject(self.$el.bar_container, "top")

		el.morph({
			"opacity": 1,
			"bottom": 0
		});

		destroy_bar.delay(self.options.screen_time);
	}
}

window.addEvent('load', function() {
	BumpBar.initialize();
    main.initialize();
	
});

})();

