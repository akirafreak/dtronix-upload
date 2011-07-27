using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using dtxCore.Json;
using System.Reflection;
using System.Windows.Forms;
using System.Threading;
using System.ComponentModel;
using System.IO;
using dtxCore;
using System.Web;
using System.Diagnostics;

namespace dtxUpload {

	public delegate void D_UploadProgressChanged(DC_UploadProgressChangedEventArgs e);
	public delegate void D_UploadCompleted(DC_ServerResponse e);
	public delegate void D_UploadCanceled();
	public delegate void D_CallServerMethodCallback(DC_ServerResponse response);

	/// <summary>
	/// Class to setup two way communication between HTTP server and client.
	/// </summary>
	public class ServerConnector {

		// Internal Classes
		private class BoundryBytes {
			public string boundry_string;
			public string boundry_start;
			public byte[] boundry_start_bytes;
			public string boundry_end;
			public byte[] boundry_end_bytes;
		}

		// Delegates
		private delegate DC_ServerResponse GetResponseDelegate(Uri address, params object[] arguments);
		private delegate DC_ServerResponse PostFileStreamDelegate(Uri address, FileStream stream);

		// Events.
		/// <summary>
		/// Called after every successful packet is sent. MUST MANUALLY invoke GUI events on GUI thread.
		/// </summary>
		public event D_UploadProgressChanged upload_progress_changed;

		/// <summary>
		/// Called after the upload has completed and the response has been read. MUST MANUALLY invoke GUI events on GUI thread.
		/// </summary>
		public event D_UploadCompleted upload_completed;

		/// <summary>
		/// Called only after a upload has been canceled. MUST MANUALLY invoke GUI events on GUI thread.
		/// </summary>
		public event D_UploadCanceled upload_canceled;

		// Classes
		public DC_ServerInformation server_info;
		public DC_UserInformation user_info;

		/// <summary>
		/// All client actions associated with this connector instance.
		/// </summary>
		public ClientActions actions;

		// Internal Vars.
		private short max_concurrent_connections;
		private BoundryBytes boundry = new BoundryBytes();
		private bool cancel_request = false;

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
		public ServerConnector(DC_ServerInformation server, DC_UserInformation user) {
			actions = new ClientActions(this);
			server_info = server;
			user_info = user;
			max_concurrent_connections = Client.config.get<short>("serverconnector.concurrent_connections_max");
		}


		private string getString(Uri address) {
			HttpWebRequest request = prepareRequest(HttpWebRequest.Create(address));

			// Pass back the second object that contains the body of the response.
			return readServerResponse(request).body;
		}

		private DC_ServerResponse getResponse(Uri address, params object[] arguments) {
			HttpWebRequest request = prepareRequest(HttpWebRequest.Create(address));
			if(arguments != null && arguments.Length > 0)
				writePostData(request, arguments);

			// Pass back the second object that contains the body of the response.
			return readServerResponse(request);
		}

		private void getResponseResult(IAsyncResult result) {
			GetResponseDelegate del = result.AsyncState as GetResponseDelegate;
			DC_ServerResponse server_response = del.EndInvoke(result);
			execServerResponse(server_response);
		}

		private DC_ServerResponse postFileStream(Uri address, FileStream file_stream) {
			HttpWebRequest request = prepareRequest(HttpWebRequest.Create(address));
			createPostBoundry(request);
			byte[] buffer = new byte[1024 * 4];
			int read_length = 0;

			string header = "Content-Disposition: form-data; name=\"file\"; filename=\"{0}\"\r\nContent-Type: application/octet-stream\r\n\r\n";
			header = string.Format(header, Path.GetFileName(file_stream.Name));
			byte[] header_bytes = Encoding.UTF8.GetBytes(header);


			DC_UploadProgressChangedEventArgs upload_args = new DC_UploadProgressChangedEventArgs();
			upload_args.total_bytes_to_send = file_stream.Length;
			request.ContentLength = boundry.boundry_start_bytes.Length + boundry.boundry_end_bytes.Length + file_stream.Length + header_bytes.Length;
			request.AllowWriteStreamBuffering = false;

			Stream write_stream = request.GetRequestStream();

			write_stream.Write(boundry.boundry_start_bytes, 0, boundry.boundry_start_bytes.Length);
			write_stream.Write(header_bytes, 0, header_bytes.Length);


			while((read_length = file_stream.Read(buffer, 0, buffer.Length)) > 0) {
				write_stream.Write(buffer, 0, read_length);
				write_stream.Flush();
				if(cancel_request) {
					upload_canceled.Invoke();
					break;
				}

				// Update any events with this information
				upload_args.bytes_sent += read_length;
				upload_progress_changed.Invoke(upload_args);
			}

			if(!cancel_request) {
				write_stream.Write(boundry.boundry_end_bytes, 0, boundry.boundry_end_bytes.Length);
				write_stream.Close();
			}

			file_stream.Close();

			if(cancel_request) {
				cancel_request = false;
				request.Abort();

				return new DC_ServerResponse();
			} else {
				return readServerResponse(request);
			}
		}

		private void postFileStreamResult(IAsyncResult result) {
			PostFileStreamDelegate del = result.AsyncState as PostFileStreamDelegate;
			DC_ServerResponse server_response = del.EndInvoke(result);

			if(!server_response.canceled) {
				upload_completed.Invoke(server_response);
			}
		}


		/// <summary>
		/// Method to read the contents of the server responce and return the response headers and body text.
		/// </summary>
		/// <param name="request">The request that is already in-progress.</param>
		/// <returns>First object is WebHeadersCollection containing the response headers, the second is the body text.</returns>
		private DC_ServerResponse readServerResponse(HttpWebRequest request) {
			try {
				if(cancel_request) {
					cancel_request = false;
					return new DC_ServerResponse() { canceled = true };
				}

				HttpWebResponse response = request.GetResponse() as HttpWebResponse;
				if(response.StatusCode != HttpStatusCode.OK) {
					Debug.WriteLine("Query[" + request.Address.Query + "] Returned Error: " + response.StatusDescription);
					return null;
				}

				int length = 0;
				byte[] buffer = new byte[512];
				Stream st = response.GetResponseStream();
				StringBuilder sb = new StringBuilder();

				while((length = st.Read(buffer, 0, buffer.Length)) > 0) {
					if(cancel_request)
						break;
					sb.Append(Encoding.UTF8.GetString(buffer, 0, length));
				}

				st.Close();
				response.Close();

				bool is_canceled = false;
				if(cancel_request == true) {
					cancel_request = false;
					is_canceled = true;
				}

				return new DC_ServerResponse() {
					canceled = is_canceled,
					headers = response.Headers,
					body = sb.ToString(),
					error = false
				};
			} catch(WebException e) {
				bool is_canceled = false;
				if(cancel_request == true) {
					cancel_request = false;
					is_canceled = true;
				}

				return new DC_ServerResponse() {
					headers = (e.Response != null) ? e.Response.Headers : null,
					canceled = is_canceled,
					body = null,
					error = true,
					error_info = e.Message,
					error_status = e.Status
				};
			}


		}


		/// <summary>
		/// Execute connection request and login.
		/// </summary>
		public void connect() {
			user_info.password_md5 = user_info.password;
			callServerMethod("User:verify");
		}

		/// <summary>
		/// Method to be called before every post that requres a boundry.
		/// </summary>
		private void createPostBoundry(HttpWebRequest request) {
			boundry.boundry_string = "----------------" + DateTime.Now.Ticks.ToString("x");
			boundry.boundry_start = "\r\n--" + boundry.boundry_string + "\r\n";
			boundry.boundry_start_bytes = Encoding.UTF8.GetBytes(boundry.boundry_start);
			boundry.boundry_end = "\r\n--" + boundry.boundry_string + "--\r\n";
			boundry.boundry_end_bytes = Encoding.UTF8.GetBytes(boundry.boundry_end);

			// Set the boundry string.
			request.ContentType = "multipart/form-data; boundary=" + boundry.boundry_string;
			request.Method = "POST";
		}

		private HttpWebRequest prepareRequest(WebRequest request) {
			HttpWebRequest http_request = request as HttpWebRequest;
			StringBuilder sb = new StringBuilder();

			http_request.Method = "GET";
			http_request.KeepAlive = false;
			http_request.Pipelined = false;
			http_request.Timeout = Timeout.Infinite;
			http_request.Credentials = System.Net.CredentialCache.DefaultCredentials;
			http_request.UserAgent = "dtxClient/0.3";
			http_request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");
			http_request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;


			if(user_info.session_key != null) {
				sb.Append("session_key=");
				sb.Append(user_info.session_key);
				sb.Append(";");
			}
			if(user_info.username != null) {
				sb.Append("client_username=");
				sb.Append(user_info.username);
				sb.Append(";");
			}
			if(user_info.password_md5 != null) {
				sb.Append("client_password=");
				sb.Append(user_info.password_md5);
				sb.Append(";");
			}

			http_request.Headers.Add(HttpRequestHeader.Cookie, sb.ToString());

			return http_request;
		}

		private void writePostData(HttpWebRequest request, params object[] postdata) {
			if(postdata == null)
				return;
			createPostBoundry(request);
			StringBuilder body = new StringBuilder();
			JsonWriter json_writer = new JsonWriter(body);

			body.Append(boundry.boundry_start);
			body.Append("Content-Disposition: form-data; name=\"args\"\r\n\r\n");
			json_writer.Write(postdata);
			body.Append(boundry.boundry_end);

			byte[] body_bytes = Encoding.UTF8.GetBytes(body.ToString());

			request.ContentLength = body_bytes.Length;
			request.AllowWriteStreamBuffering = false;

			Stream write_stream = request.GetRequestStream();
			write_stream.Write(body_bytes, 0, body_bytes.Length);
			write_stream.Close();
		}



		public void disconnect() {
			callServerMethod("User:logout");
		}

		/// <summary>
		/// Request server information to be sent to the client and parsed.
		/// </summary>
		public void getServerInfo() {
			callServerMethod("Server:info");
		}

		/// <summary>
		/// Cancel all current connection requestes to the server.
		/// </summary>
		public void cancelActions() {
			cancel_request = true;
		}

		/// <summary>
		/// Begin upload of a file to the server.
		/// </summary>
		/// <param name="file_location">Local file location on the client computer.</param>
		/// 
		public void uploadFile(string file_location) {
			Uri uri = buildUri("Files:upload");
			FileStream fs = new FileStream(file_location, FileMode.Open, FileAccess.Read, FileShare.Read);
			if(!fs.CanRead) // If we can not read it, what CAN we do?
				return;

			PostFileStreamDelegate post = new PostFileStreamDelegate(postFileStream);
			post.BeginInvoke(uri, fs, new AsyncCallback(postFileStreamResult), post);
		}

		/// <summary>
		/// Asynchronously execute a method on the server.
		/// </summary>
		/// <param name="method"></param>
		public void callServerMethod(string method) {
			callServerMethod(method, new object[0]);
		}

		/// <summary>
		/// Asynchronously execute a method on the server.
		/// </summary>
		/// <param name="method">Method name to execute on the server.</param>
		/// <param name="arguments">Arguments to pass to the method on the server.</param>
		public void callServerMethod(string method, params object[] arguments) {
			// Determine if multiple arguments are being passed or just one array.
			if(!arguments.GetType().Equals(typeof(object[]))) {
				arguments = new object[] { arguments };
			}
			Uri uri = buildUri(method);
			GetResponseDelegate get = new GetResponseDelegate(getResponse);
			get.BeginInvoke(uri, arguments, new AsyncCallback(getResponseResult), get);
		}

		/// <summary>
		/// Asynchronously execute a method on the server and have a callback executed on a completed query.
		/// </summary>
		/// <param name="method">Method name to execute on the server.</param>
		/// <param name="callback">Delegate to call when the server request has been sent and the response has been read.</param>
		/// <param name="arguments">Arguments to pass to the method on the server.</param>
		public void callServerMethod(string method, D_CallServerMethodCallback callback, params object[] arguments) {
			// Determine if multiple arguments are being passed or just one array.
			if(!arguments.GetType().Equals(typeof(object[]))) {
				arguments = new object[] { arguments };
			}
			Uri uri = buildUri(method);
			GetResponseDelegate get = new GetResponseDelegate(getResponse);
			get.BeginInvoke(uri, arguments, new AsyncCallback(delegate(IAsyncResult result) {
				GetResponseDelegate del = result.AsyncState as GetResponseDelegate;
				DC_ServerResponse server_response = del.EndInvoke(result);
				callback(server_response);
			}), get);
		}

		/// <summary>
		/// Execute a method on the server and return the response.  This method is synchronous.
		/// </summary>
		/// <param name="method">Method name to execute on the server.</param>
		public DC_ServerResponse serverResponse(string method) {
			return serverResponse(method, null);
		}

		/// <summary>
		/// Execute a method on the server and return the response.
		/// </summary>
		/// <param name="method">Method name to execute on the server.</param>
		/// <param name="arguments">Arguments to pass to the method on the server.</param>
		public DC_ServerResponse serverResponse(string method, params object[] arguments) {
			Uri uri = buildUri(method);
			return getResponse(uri, arguments);
		}

		/// <summary>
		/// Build valid server URI for the requested command.
		/// </summary>
		/// <param name="method">Method to call on the server.</param>
		/// <returns>Completed URI, or argument exception.</returns>
		private Uri buildUri(string method) {
			StringBuilder sb = new StringBuilder();

			sb.Append(server_info.server_url);
			sb.Append("?action=");
			sb.Append(method);
			try {
				return new Uri(sb.ToString());
			} catch {
				throw new ArgumentException("The build URI for the server was invalid.  This is due to invalid arguments or method called.");
			}

		}

		/// <summary>
		/// Method to handle the execution and error handling of the server response.
		/// </summary>
		/// <param name="response">Server sent response to deal with.</param>
		public void execServerResponse(DC_ServerResponse response) {
			// If a transport error occured, let the user know.
			if(response.error) {
				// We should only alert the user if something is wrong with the request on their system, and not errors with missing files on the server.
				if(response.error_status != WebExceptionStatus.ProtocolError) {
					Client.form_Login.Invoke((MethodInvoker)delegate {
						MessageBox.Show("Encountered error when reading server response.  Details: \n\n" + response.error_info);
					});
				}
			}

			// Check to see if the headers have even been set.
			if(response.headers == null) {
				Client.form_Login.Invoke((MethodInvoker)Client.form_Login.serverInvalid);
				server_info.is_connected = false;
				return;
			}

			string method = response.getCalledMethod();

			// Ensure that the server is calling a method.
			if(method == null) {
				Client.form_Login.Invoke((MethodInvoker)Client.form_Login.serverInvalid);
				server_info.is_connected = false;
				return;
			}

			// Try to get the requested method from the list of valid actions.
			MethodInfo requested_method = typeof(ClientActions).GetMethod(method);

			if(requested_method != null) {
				int param_count = requested_method.GetParameters().Length;
				if(response.body == "false" && param_count == 0) {
					requested_method.Invoke(actions, null);
				} else {
					requested_method.Invoke(actions, new object[] { response.body });
				}

			} else {
				Client.form_Login.Invoke((MethodInvoker)delegate {
					MessageBox.Show("Server requested invalid method.  This is usually due to an outdated client.\n\nError:\n" + response.body);
				});
			}
		}
	}
}
