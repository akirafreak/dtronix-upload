using System;
using System.Collections.Generic;
using System.Text;
using dtxCore.Json;
using System.Windows.Forms;

namespace dtxUpload {
	public partial class ClientActions {
		private class ping_class {
			public bool maintenance_mode;
		}

		public void ping(string input) {
			JsonReader reader = new JsonReader(input);
			ping_class info = reader.Deserialize<ping_class>();

			if(info.maintenance_mode) {
				mainenance_mode();
				return;
			}

			Client.form_Login.last_ping_time = DateTime.Now;
		}


		/// <summary>
		/// Updates the settings of the server.
		/// </summary>
		/// <param name="input">JSON string with the approprately formatted server information.</param>
		public void server_information(string input) {
			DC_ServerInformation info = deserializeJson<DC_ServerInformation>(input);
			if(info == null)
				Client.form_Login.Invoke((MethodInvoker)Client.form_Login.serverInvalid);

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

		public void validation_invalid_server() {
			clearSession();
			Client.form_Login.Invoke((MethodInvoker)Client.form_Login.serverInvalid);
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

		public void validation_successful(string input) {
			connector.user_info.username = null;
			connector.user_info.password = null;
			connector.user_info.session_key = deserializeJson<string>(input);
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

		public void mainenance_mode() {
			Client.form_Login.Invoke((MethodInvoker)Client.form_Login.serverMaintenanceMode);
		}
	}
	
}
