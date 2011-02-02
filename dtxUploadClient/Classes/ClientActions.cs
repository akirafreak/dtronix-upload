using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Core.Json;
using System.Drawing;

namespace dtxUpload {
	class ClientActions {

		private ServerConnector connector;

		private List<server_error_class> server_errors = new List<server_error_class>();

		public ClientActions(ServerConnector connector) {
			this.connector = connector;
		}

		public void validation_expired_user_session() {
			clearSession();
			connector.connect();
		}

		public void validation_invalid_user_session() {
			clearSession();
			Client.form_Login.Invoke((MethodInvoker)Client.form_Login.sessionExpired);
		}

		public void validation_invalid_username() {
			clearSession();
			Client.form_Login.Invoke((MethodInvoker)Client.form_Login.invalidUsername);
		}

		public void validation_invalid_password() {
			clearSession();
			Client.form_Login.Invoke((MethodInvoker)Client.form_Login.invalidPassword);
		}

		public void validation_successful(string session_key) {
			connector.user_info.client_username = null;
			connector.user_info.client_password = null;
			connector.user_info.session_key = session_key;
			connector.server_info.is_connected = true;
			Client.form_Login.Invoke((MethodInvoker)Client.form_Login.serverConnected);
		}

		public void logout_successful() {
			clearSession();
			Client.form_Login.Invoke((MethodInvoker)Client.form_Login.loggedOut);
		}

		private void clearSession() {
			connector.user_info.session_key = null;
			connector.server_info.is_connected = false;
		}

		


		private class server_error_class{
			public string error_type;
			public string error_info;
			public string error_file;
			public string error_line;
		}

		public void server_error(string input) {
			JsonReader reader = new JsonReader(input);
			server_error_class info = reader.Deserialize<server_error_class>();
			server_errors.Add(info);

			Client.form_Login.Invoke((MethodInvoker)delegate {
				MessageBox.Show("The PHP server encountered an error.  Details: \n\n" + 
					"    Type: " + info.error_type + "\n" +
					"    Info: " + info.error_info + "\n" +
					"    File: " + info.error_file + "\n" +
					"    Line: " + info.error_line + "\n");
			});
		}









		/// <summary>
		/// Updates the settings of the server.
		/// </summary>
		/// <param name="input">JSON string with the approprately formatted server information.</param>
		public void server_information(string input) {
			JsonReader reader = new JsonReader(input);
			DC_ServerInformation info = reader.Deserialize<DC_ServerInformation>();

			connector.server_info.server_name = info.server_name;
			connector.server_info.maintenance_mode = info.maintenance_mode;
			connector.server_info.is_key_required = info.is_key_required;
			connector.server_info.is_registration_allowed = info.is_registration_allowed;
			connector.server_info.max_upload_filesize = info.max_upload_filesize;
			connector.server_info.maintenance_mode = info.maintenance_mode;
			connector.server_info.allowed_filetypes = info.allowed_filetypes;
			connector.server_info.upload_base_url = info.upload_base_url;
			connector.server_info.server_logo = info.server_logo;

			Client.form_Login.Invoke((MethodInvoker)Client.form_Login.serverOnline);
		}
	}
}
