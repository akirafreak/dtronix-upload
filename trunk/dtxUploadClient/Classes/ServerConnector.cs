using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using Core.Json;
using System.Reflection;
using System.Windows.Forms;

namespace dtxUpload {

	/// <summary>
	/// Class to setup two way communication between HTTP server and client.
	/// </summary>
	class ServerConnector {
		public short max_concurrent_connections;

		private List<WebClient> web_clients = new List<WebClient>();
		public UploadProgressChangedEventHandler uploadProgressChanged;
		public UploadFileCompletedEventHandler uploadFileCompleted;

		public DC_ServerInformation server_info;
		public DC_UserInformation user_info;
		private ClientActions actions;

		/// <summary>
		///  Uses global client information for connecting.
		/// </summary>
		public ServerConnector() {
			actions = new ClientActions(this);
			server_info = Client.server_info;
			user_info = Client.user_info;
			max_concurrent_connections = Client.config.get<short>("serverconnector.concurrent_connections_max");
		}

		/// <summary>
		/// Uses custom connection information.
		/// </summary>
		/// <param name="server">Server information to use.</param>
		/// <param name="user">User connection information to use.</param>
		public ServerConnector(DC_ServerInformation server, DC_UserInformation user){
			actions = new ClientActions(this);
			server_info = server;
			user_info = user;
		}

		/// <summary>
		/// Method to get or create an inactive WebClient.  Will create a new WebClient class up to max_concurrent_connections.
		/// </summary>
		/// <returns>Inactive WebClient</returns>
		private WebClient getWebClient() {

			// Try to find an open client first.
			int len = web_clients.Count;
			for(int i = 0; i < len; i++) {
				if(!web_clients[i].IsBusy) {
					return prepareClient(web_clients[i]);
				}
			}

			//No clients are currently avalible.
			if(len < max_concurrent_connections) {
				// We are under the max allowed ammount for the connector.
				WebClient client = new WebClient();
				setupEvents(client);
				web_clients.Add(client);

				return prepareClient(client);

			} else {
				// We have reached the max allowed connections to be open.  TODO: create error report for this.  TODO: Make sure to not cause a null reference exception... LIKE I AM DOING...
				return null;
			}
		}

		private WebClient prepareClient(WebClient client) {
			StringBuilder sb = new StringBuilder();
			client.Headers.Clear();
			client.Headers.Add(HttpRequestHeader.UserAgent, "dtxUploadClient/0.1");

			if(user_info.session_key != null) {
				sb.Append("session_key=");
				sb.Append(user_info.session_key);
				sb.Append(";");
			}
			if(user_info.client_username != null) {
				sb.Append("client_username=");
				sb.Append(user_info.client_username);
				sb.Append(";");
			}
			if(user_info.client_password_md5 != null) {
				sb.Append("client_password=");
				sb.Append(user_info.client_password_md5);
				sb.Append(";");
			}

			client.Headers.Add(HttpRequestHeader.Cookie, sb.ToString());

			return client;
		}

		/// <summary>
		/// Setup all async events for the WebClient.
		/// </summary>
		/// <param name="client">Corrasponding client to add the events to.</param>
		private void setupEvents(WebClient client) {
			client.UploadProgressChanged += new UploadProgressChangedEventHandler(web_client_UploadProgressChanged);
			client.UploadFileCompleted += new UploadFileCompletedEventHandler(web_client_UploadFileCompleted);
			client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(web_client_DownloadStringCompleted);
		}

		private void web_client_UploadFileCompleted(object sender, UploadFileCompletedEventArgs e) {
			uploadFileCompleted(sender, e);
		}

		private void web_client_UploadProgressChanged(object sender, UploadProgressChangedEventArgs e) {
			uploadProgressChanged(sender, e);
		}

		private void web_client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e) {
#if DEBUG
			Client.form_connector.Invoke((MethodInvoker)delegate{
				Client.form_connector.updateRequest((int)e.UserState, e.Result);
			});
#endif
			//string called_method = e.UserState.ToString();
			if(e.Error != null) {
				Client.form_Login.Invoke((MethodInvoker)Client.form_Login.invalidServer);
				server_info.is_connected = false;
			} else {
				execServerResponce(e.Result);
			}		
		}

		/// <summary>
		/// Execute connection request and login.
		/// </summary>
		public void connect() {
			user_info.client_password_md5 = user_info.client_password;
			callServerMethod("user_verification");
		}

		public void disconnect() {
			callServerMethod("logout");
		}

		/// <summary>
		/// Request server information to be sent to the client and parsed.
		/// </summary>
		public void getServerInfo() {
			callServerMethod("get_server_info");
		}

		/// <summary>
		/// Cancel all current connection requestes to the server.
		/// </summary>
		public void cancelActions() {
			int len = web_clients.Count;
			for(int i = 0; i < len; i++) {
				web_clients[i].CancelAsync();
			}
		}

		/// <summary>
		/// Begin upload of a file to the server.
		/// </summary>
		/// <param name="file_location">Local file location on the client computer.</param>
		/// 
		public void uploadFile(string file_location) {
			Uri uri = buildUri("upload_file");
			getWebClient().UploadFileAsync(uri, file_location);
		}

		/// <summary>
		/// Execute a method on the server.
		/// </summary>
		/// <param name="method"></param>
		public void callServerMethod(string method) {
			callServerMethod(method, null);
		}

		/// <summary>
		/// Execute a method on the server.
		/// </summary>
		/// <param name="method">Method name to execute on the server.</param>
		/// <param name="arguments">Arguments to pass to the method on the server.</param>
		public void callServerMethod(string method, params string[] arguments) {
			Uri uri = buildUri(method, arguments);
			WebClient client = getWebClient();
#if DEBUG
			Client.form_connector.Invoke((MethodInvoker)delegate {
				int request_id = Client.form_connector.addRequest(method, uri.Query);
				client.DownloadStringAsync(uri, request_id);
			});
#else
			client.DownloadStringAsync(uri, method);
#endif

		}

		/// <summary>
		/// Build valid server URI for the requested command.
		/// </summary>
		/// <param name="method">Method to call on the server.</param>
		/// <param name="arguments">Array of string arguments to pass to the method on the server.</param>
		/// <returns>Completed URI, or argument exception.</returns>
		private Uri buildUri(string method, params string[] arguments) {
			StringBuilder sb = new StringBuilder();

			sb.Append(server_info.server_url);
			sb.Append("?action=");
			sb.Append(method);
			if(arguments != null) {
				foreach(string arg in arguments) {
					sb.Append("&args[]=");
					sb.Append(arg);
				}
			}
			try {
				return new Uri(sb.ToString());
			} catch {
				throw new ArgumentException("The build URI for the server was invalid.  This is due to invalid arguments or method called.");
			}


		}

		/// <summary>
		/// Data is passed here from the async request to be parsed and executed.
		/// </summary>
		/// <param name="server_data">Raw data that comes from the server</param>
		private void execServerResponce(string server_data) {
			string[] parsed = parseServerData(server_data);

			if(parsed[0] == null) // If the server responce could not be  parsed, then just quit!
				return;

			MethodInfo requested_method = typeof(ClientActions).GetMethod(parsed[0]);
			if(requested_method != null) {
				if(parsed[1] == null) {
					requested_method.Invoke(actions, null);
				} else {
					requested_method.Invoke(actions, new object[] { parsed[1] });
				}

			} else {
				// Server requested a method that we do not support.  TO DO.
			}
		}

		/// <summary>
		/// Exposed method to allow for global parsing of raw server data.
		/// </summary>
		/// <param name="server_data"></param>
		/// <returns>[0]: Function to call; [1]: Arguments to pass;</returns>
		public string[] parseServerData(string server_data) {
			string[] parsed = new string[2];
			try {
				int function_len = int.Parse(server_data.Substring(0, 3));
				parsed[0] = server_data.Substring(3, function_len);
				parsed[1] = null;

				if(server_data.Length > function_len + 3) {
					parsed[1] = server_data.Substring(function_len + 3);
				}

				return parsed;

			} catch {
				// Server information could not be parsed. TO DO.
				return parsed;
			}
		}
	}
}
