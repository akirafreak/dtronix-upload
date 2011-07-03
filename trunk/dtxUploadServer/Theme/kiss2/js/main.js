var Main =  new Class({
	$el: {},
	
	log: function(){
		if(console != undefined && console != undefined){
			console.log(arguments);
		}
	},
	
	options:{
		tween_duration: 400,
		request_url: "dtxUpload.php",
		upload_base_url: ""
	},

	initialize: function(){
		// Setup elements for the script.
		this.addElements(
			"login_form",
			"input_username",
			"input_password",
			"button_register",
			"login_group",
			"login_form",
			"button_cancel_register",
			"files_container",
			
			"action_logout",
			"action_configure",
			
			"config_container",
			"action_close_config",
			
			"user_info",
			"user_info_username",
			"register_div",
			"register_form",
			"register_username",
			"register_password",
			"register_repassword",
			"register_email",
			"register_agree",
			"register_go",

			"uploaded_files_inject"
		);
			
		this.$el.register_div.setStyles({
			"opacity": 0,
			"display": "block"
		});
		
		this.registerTemplates();
		this.eventRegistration();

		// Attempt to auto login.
		if(Cookie.read("session_key") != null){
			this.loginRequest.get({
				"action": "User:verify"
			});
		}

		this.$el.input_username.focus();
		Notify.alert("System", "Successfully loaded manager.");
	},
	
	
	buildRequest: function(callback){
		new Request.JSON({
			url: Main.options.request_url,
			noCache: true,
			onSuccess: function(data){
				var method = self.http_request.getHeader("Call-Client-Method");
				success_callback(method, data);
			},
			onFailure: function(){
				success_callback("request_failure", null);
			},
			onError: function(text, error){
				Main.log(text, error);
			}
		});
		
		return callback;
	},
	
	
	
	/**
	 * Method to quickly add all parameters to the $el property.  Parameters must be element ids
	 *
	 */
	addElements: function(){
		for(var i = 0; i < arguments.length; i++){
			this.$el[arguments[i]] = $(arguments[i]);
			if(this.$el[arguments[i]] == null)
				console.log("Element '" + arguments[i] + "' is null.");
		}
	},
	
	/**
	 * Handles all the event registrations for the site.
	 */
	eventRegistration:function(){
		
		// Setup variables for the script.
		var self = this;
		
		// Set all element's tween.
		[this.$el.register_div, this.$el.login_group, this.$el.user_info, this.$el.config_container].each(function(el){
			el.set("tween", {
				transition: Fx.Transitions.Expo.easeOut,
				duration: Main.options.tween_duration
			});
		})

		// Login submit event.
		this.$el.login_form.addEvent("submit", function(){
			self.login();
			return false;
		});
		
		
		// Register button event.
		this.$el.button_register.addEvent("click", function(){
			self.$el.login_group.tween("opacity", 0);
			self.$el.register_div.tween("opacity", 1);
			self.$el.register_username.focus();
			
			return false;
		});
		
		// Cancel registration event.
		this.$el.button_cancel_register.addEvent("click", function(){
			self.$el.login_group.tween("opacity", 1);
			self.$el.register_div.tween("opacity", 0);

			return false;
		});

		this.$el.register_form.addEvent("submit", function(){
			self.register();
			
			return false;
		});

		this.$el.action_logout.addEvent("click", function(){
			self.logout();

			return false;
		});
		
		this.$el.action_configure.addEvent("click", function(){
			self.$el.config_container.tween("height", 200);
			self.$el.config_container.setStyle("visibility", "visible");
			return false;
		});
		
		this.$el.action_close_config.addEvent("click", function(){
			self.$el.config_container.tween("height", 0);
			(function(){self.$el.config_container.setStyle("visibility", "hidden");}).delay(Main.options.tween_duration);

			return false;
		});
		
		
	},
	
	showFileInfo: function(file_id){
		console.log(file_id);
		// Request all file information and send it to the user.
	},
	
	
	/**
	 * Login to the site with the credentials provided.
	 */
	login: {
		go: function(){
			Notify.alert("Login", "Submitting login information");
			document.cookie = "";
			Cookie.write("client_username", this.$el.input_username.value);
			Cookie.write("client_password", MD5(this.$el.input_password.value));

			this.loginRequest.get({
				"action": "User:verify"
			});
		},
		
		request: this.buildRequest(this.login.actions),
		
		actions: function(method, data){
			switch(method){
				case "validation_invalid_password":
					Notify.alert("Login", "Invalid password provided for username.  Please verify password.", {bgcolor: "red"});
					this.$el.input_password.focus();
					break;

				case "validation_successful":
					this.displayFileInterface();
					this.$el.login_group.tween("opacity", 0);

					(function(){
						this.$el.user_info.setStyles({
							"opacity": 0,
							"display": "block"
						});
						this.$el.user_info.tween("opacity", 1);
					}).delay(Main.options.tween_duration, this);

					Notify.alert("Login", "Logged in successfully.  Loading interface...", {bgcolor: "green"});
					break;

				case "validation_invalid_username":
					Notify.alert("Login", "Username entered was invalid.  Please enter a valid username or register.", {bgcolor: "red"});
					this.$el.input_username.focus();
					break;

				case "validation_expired_user_session":
					Notify.alert("Login", "Session has expired.  Please login again.", {bgcolor: "red"});
					this.logout();
					break;

				case "validation_invalid_user_session":
					Notify.alert("Login", "Session is invalid.  Please login again.", {bgcolor: "red"});
					this.logout();
					break;

				case "validation_failed_no_login":
					//The automatic login failed due to no cookies being set.
					break;

				default: this.defaultActions(method); break;
			}
		}.bind(this)
	},

	
	/**
	 * Actions that can take place on any request are placed in this method.
	 */
	defaultActions: function(method){
		switch(method){
			case "request_failure":
				Notify.alert("Server", "Can not connect to the server at this time.", {bgcolor: "red"});
				break;
				
			case "maintenance_mode":
				Notify.alert("Server", "The server is currently in maintenance mode and is not allowing logins at this time.", {bgcolor: "red"});
				break;
				
			case "validation_user_connection_dissabled":
				Notify.alert("Server", "You are not allowed to connect to your account at this time.", {duration: false, bgcolor: "red"});
				this.logoutRequest.callback("logout_successful");
				break;
				
			case "validation_account_dissabled":
				Notify.alert("Login", "Your account has been dissabled pending review.  Please contact support if you think this is in error.", {duration: false, bgcolor: "red"});
				this.logoutRequest.callback("logout_successful");
				break;

			default:
				Notify.alert("Login", "Username entered was invalid.  Please enter a valid username or register.", {bgcolor: "red"});
				break;
		}
	},
	
	/**
	 * Register an account on the site.
	 */
	register: function(){
		var username = this.$el.register_username.value;
		var pass = this.$el.register_password.value;
		var repass = this.$el.register_repassword.value;
		var email = this.$el.register_email.value;

		// Username verification.
		if(username.length < 3){
			this.registerRequest.callback("registration_username_short");
			return;
		}
		if(username.length > 15){
			this.registerRequest.callback("registration_username_long");
			return;
		}
		if(!username.test("^[a-zA-Z0-9_-]*$")){
			this.registerRequest.callback("registration_username_invalid");
			return;
		}
		
		// Password validation.
		if(pass.length < 8){
			this.registerRequest.callback("registration_password_short");
			return;
		}
		if(pass != repass){
			Notify.alert("Register", "Passwords entered do not match.", {bgcolor: "red"});
			this.$el.register_repassword.focus();
			return;
		}
		
		if(!email.test(/^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$/i)){
			this.registerRequest.callback("registration_email_invalid");
			return;
		}

		if(!this.$el.register_agree.checked){
			Notify.alert("Register", "You must agree to the terms and conditions.", {bgcolor: "red"});
			this.$el.register_agree.focus();
			return;
		}

		this.registerRequest.get({
			"action": "User:register",
			"args": [username, MD5(pass), email]
		})
	},
	
	registerRequest: new DtxRequest(function(method, data){
		switch(method){
			case "registration_username_long":
				Notify.alert("Register", "Username provided was too long.  Please enter a username that is between 3 and 15 characters.", {bgcolor: "red"});
				this.$el.register_username.focus();
				break;
				
			case "registration_username_short":
				Notify.alert("Register", "Username provided was too short.  Please enter a username that is between 3 and 15 characters.", {bgcolor: "red"});
				this.$el.register_username.focus();
				break;
				
			case "registration_username_invalid":
				Notify.alert("Register", "Username contains invalid characters.  You are only allowed to use alphanumeric characters, underscore and the dash.", {bgcolor: "red"});
				this.$el.register_username.focus();
				break;
				
			case "registration_password_short":
				Notify.alert("Register", "Password provided was too short.  Please enter a password that is at least 8 characters.", {bgcolor: "red"});
				this.$el.register_password.focus();
				break;
				
			case "registration_email_invalid":
				Notify.alert("Register", "Email entered was invalid.", {bgcolor: "red"});
				this.$el.register_email.focus();
				break;
				
			case "registration_username_existing":
				Notify.alert("Register", "Username already exists.", {bgcolor: "red"});
				this.$el.register_username.focus();
				break;
				
			case "registration_email_existing":
				Notify.alert("Register", "Email already in use on another account.", {bgcolor: "red"});
				this.$el.register_email.focus();
				break;
				
			case "registration_success_activated":
				Notify.alert("Register", "Account created and activated.  Please login.", {bgcolor: "green"});
				this.$el.button_cancel_register.fireEvent("click");
				this.$el.input_username.focus();
				break;
				
			case "registration_failure_server":
				Notify.alert("Register", "There was a registration error on the server.  Please contact support about this error.", {bgcolor: "red"});
				break;
				
			default: this.defaultActions(method); break;
		}
	}),

	/**
	 * Display the requested page of files to the user.
	 *
	 * @param page Page to display.
	 */
	loadFiles: function(page){
		if(page == null) page = 0;
		this.$el.uploaded_files_inject.empty();

		this.loadFilesRequest.get({
			"action": "Files:listFiles",
			"args": [page]
		});
	},

	/**
	 * All possible actions to execute for the associated request.
	 */
	loadFilesRequest: new DtxRequest(function(method, data){
		switch(method){
			case "files_uploaded_quick":
				this.displayFiles(data)
				break;
				
			case "files_uploaded_no_files":
				break;
				
			default: this.defaultActions(method); break;
		}
	}),

	/**
	 * Displays all the file information requested to the file container.
	 * 
	 * @param file_data Associative array that contains all the requested file information to display.
	 */
	displayFiles: function(file_data){
		var self = this;
		this.$el.uploaded_files_inject.empty();
		this.$el.files_container.setStyle("display", "block");
		
		var files_dom = Mooml.render("uploaded_file", file_data);
		files_dom.each(function(el){
			el.inject(self.$el.uploaded_files_inject)
		});
		
	},
	
	formatFileSize: function(size){
		if(size <= 0)
			return size;
		if(size < 1000)
			return size + " B";
		if(size < 1000000)
			return Math.round(size / 1000 * 10) / 10 + " KB";
		if(size < 1000000000)
			return Math.round(size / 1000000 * 100) / 100 + " MB";
		if(size >= 1000000000)
			return Math.round(size / 1000000000 * 100) / 100 + " GB";
	},
	
	registerTemplates: function(){
		var self = this;
		Mooml.register("file_info", function(file){
			table(
				tbody(
					tr(
						td({"width": 70, "html": "File Views:"}),
						td({"width": 50, "html": file.total_views}),
						td({"width": 70, "html": "Is Public:"}),
						td({"width": 50, "html": file.is_public}),
						td({"width": 70, "html": "Tags:"}),
						td({"width": 70, "html": file.tags})
					),
					tr(
						td({"html": "Last Viewed"}),
						td({"html": file.last_accessed})
					)
				)
			)
			
		});
		
		Mooml.register("uploaded_file", function(file){
			tr({
				"class": "uploaded_file",
				"id": "file_row_" + file.url_id
			},
				td(
					a({
						"href": file.url_id,
						"target": "_blank",
						"html": file.file_name
					})
				),
				td({
					"html": Main.formatFileSize(file.file_size)
				}),
				td({
					"html": file.upload_date
				}),
				td(
					a({
						"href": "#",
						"html": "(Info)",
						"events": {
							"click": function(e){
								e.stop();
								Main.fileInfoToggle(file.url_id);
							}
						}
					}),
					a({
						"href": "#",
						"html": "(Delete)",
						"events": {
							"click": function(e){
								e.stop();
								Main.fileAction(file.url_id, "delete");
							}
						}
					})
				)
			);
			tr({"class": "uploaded_file_info"},
				td({"colspan": 4},
					div({
						"id": "file_row_info_" + file.url_id,
						"class": "file_row_info"
					})
				)
			);
		});
	},
	
	fileInfoRequest: new DtxRequest(function(method, data){
		switch(method){
			case "file_info":
				parent_tr.setStyle("display", "table-row");
				div.tween("height", 100);
				div.innerHTML = "Loading...";
				var info_dom = Mooml.render("file_info", file_data);
				break;
				
			case "file_delete_failure":
				Notify.alert("File Action", "Unable to delete file.  Please contact support.", {bgcolor: "red"});
				break;
				
			default: this.defaultActions(method); break;
		}
		
		var div = $("file_row_info_" + file_id);
		var parent_tr = div.getParent("tr");
		
		if(parent_tr.getStyle("display") == "none"){

		}
	}),
	
	
	
	/**
	 * ececute an action on a specific file.
	 * 
	 * @param fid File ID.  URL ID.
	 * @param action Action to perform on the file. List: [delete].
	 */
	fileAction: function(fid, action){
		if(action == "delete"){
			this.fileActionRequest.get({
				"action": "Files:delete",
				"args": [fid]
			});
		}
	},

	/**
	 * All possible actions to execute for the associated request.
	 */
	fileActioRequest: new DtxRequest(function(method, data){
		switch(method){
			case "file_delete_confirmation":
				Notify.alert("File Action", "File deleted. Reloading files.", {bgcolor: "green"});
				this.loadFiles(this.page);
				break;
				
			case "file_delete_failure":
				Notify.alert("File Action", "Unable to delete file.  Please contact support.", {bgcolor: "red"});
				break;
				
			default: this.defaultActions(method); break;
		}
	}),
	
	/**
	 * Requests all the connected user's information.
	 */
	loadUserInfo: function(){
		this.loadUserInfoRequest.get({
			"action": "User:info"
		});
	},

	/**
	 * All possible actions to execute for the associated request.
	 */
	loadUserInfoRequest:  new DtxRequest(function(method, data){
		switch(method){
			case "user_info":
			var info = data;

			Main.options.upload_base_url = info.upload_base_url;
			this.$el.user_info_username.innerHTML = info.username;
			//this.$el.info_used_space.innerHTML = "("+ info.total_uploaded_filesizes + "KB / " + info.max_upload_space + "KB)";
			//this.$el.info_total_files.innerHTML = info.total_files_uploaded;
			//this.$el.info_max_upload_size.innerHTML = info.max_upload_size + "KB";

			this.loadFiles(0);
			break;
			
			default: this.defaultActions(method); break;
		}
		
	}),
	
	/**
	 * Displays the file container.
	 */
	displayFileInterface: function(){
		this.loadUserInfo();
	},
	
	/**
	 * Forces the current user to logout and the server to delete the current session.
	 */
	logout:function(){
		this.logoutRequest.get({
			"action": "User:logout"
		});
	},
	
	/**
	 * All possible actions to execute for the associated request.
	 */
	logoutRequest: new DtxRequest(function(method, data){
		switch(method){
			case "logout_successful":
				Cookie.dispose("session_key");
				this.$el.files_container.setStyle("display", "none");
				
				this.$el.user_info.tween("opacity", 0);
				// Delay the hiding if the user info.
				(function(){
					this.$el.user_info.setStyle("display", "none");
					this.$el.login_group.tween("opacity", 1);
				}).delay(Main.options.tween_duration, this);
				
				
				Notify.alert("Logout", "You are now logged out.", {bgcolor: "green"});
				break;
				
			case "logout_failed":
				Notify.alert("Logout", "Unable to log you out from the server.  Please contact support.", {bgcolor: "red"});
				break;
			
			default: this.defaultActions(method); break;
		}
	})
});

var Notify = new Roar({position: "lowerRight"});

window.addEvent("load", function(){
	//new Main();
});