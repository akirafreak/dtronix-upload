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
using dtxCore.IO;
using System.Web;
using System.Diagnostics;

namespace DtxUpload {

	public delegate void ServerConnectorUploadProgressDelegate(ServerConnectorUploadProgressChangedEventArgs e);
	public delegate void ServerConnectorUploadCompletedDelegate(ServerConnectorResponse e);
	public delegate void ServerConnectorUploadCanceledDelegate();
	public delegate void ServerConnectorMethodCallback(ServerConnectorResponse response);

	/// <summary>
	/// Class to setup two way communication between HTTP server and client.
	/// </summary>
	/// <remarks>
	/// One instance of the class can only have one one active connection at a time.
	/// </remarks>
	public class ServerConnector {

		// Internal Classes
		private class PostBodyBoundry {
			public string boundry_string;
			public string boundry_start;
			public byte[] boundry_start_bytes;
			public string boundry_end;
			public byte[] boundry_end_bytes;
		}

		private class RequestQuery {
			public bool upload;
			public string file_location;
			public string method;
			public ServerConnectorMethodCallback callback;
			public object[] arguments;
		}

		// Delegates
		private delegate ServerConnectorResponse GetResponseDelegate(Uri address, params object[] arguments);
		private delegate ServerConnectorResponse PostFileStreamDelegate(Uri address, FileStream stream);

		// Events.
		/// <summary>
		/// Called after every successful packet is sent. MUST MANUALLY invoke GUI events on GUI thread.
		/// </summary>
		public event ServerConnectorUploadProgressDelegate upload_progress;

		/// <summary>
		/// Called after the upload has completed and the response has been read. MUST MANUALLY invoke GUI events on GUI thread.
		/// </summary>
		public event ServerConnectorUploadCompletedDelegate upload_completed;

		/// <summary>
		/// Called only after a upload has been canceled. MUST MANUALLY invoke GUI events on GUI thread.
		/// </summary>
		public event ServerConnectorUploadCanceledDelegate upload_canceled;

		// Classes
		public ServerConnectorServerInformation server_info { get; set; }
		public ServerConnectorUserInformation user_info { get; set; }

		/// <summary>
		/// All client actions associated with this connector instance.
		/// </summary>
		public ClientActions actions { get; private set; }

		// Internal Vars.
		/// <summary>
		/// User defined data associated with the ClientActions class.
		/// </summary>
		private object tag;
		private PostBodyBoundry boundry = new PostBodyBoundry();
		private bool cancel_request = false;
		private bool throttle_changed = false;
		private Queue<RequestQuery> request_queue = new Queue<RequestQuery>();

		private ServerConnectorQueryHandling _query_handling = ServerConnectorQueryHandling.queue;
		/// <summary>
		/// Defines the say that the connector handles multiple concurrent queries.
		/// </summary>
		public ServerConnectorQueryHandling query_handling {
			get {
				return _query_handling;
			}
			set {
				_query_handling = value;
			}
		}

		private long _throttle_bytes_down = ThrottledStream.Infinite;
		/// <summary>
		/// Sets the maximum bytes per second that the connector can transfer.  0(Default) = Unthrottled.
		/// </summary>
		/// <remarks>
		/// This will only throttle the next request that is sent to the connector.
		/// </remarks>
		public long throttle_bytes_down {
			get {
				return _throttle_bytes_down;
			}
			set {
				throttle_changed = true;
				_throttle_bytes_down = value;
			}
		}

		private long _throttle_bytes_up = ThrottledStream.Infinite;
		/// <summary>
		/// Sets the maximum bytes per second that the connector can upload. 0(Default) = Unthrottled.
		/// </summary>
		/// <remarks>
		/// This will only throttle the next request that is sent to the connector.
		/// </remarks>
		public long throttle_bytes_up {
			get {
				return _throttle_bytes_up;
			}
			set {
				throttle_changed = true;
				_throttle_bytes_up = value;
			}

		}

		private bool _is_active = false;
		/// <summary>
		/// If a POST or GET request is running, will return true.
		/// </summary>
		public bool is_active {
			get {
				return _is_active;
			}
		}

		/// <summary>
		/// Uses global client information for connecting and authenticating.
		/// </summary>
		public ServerConnector() : this(Client.server_info, Client.user_info) { }

		/// <summary>
		/// Uses global client information for connecting and authenticating and defines tag.
		/// </summary>
		/// <param name="tag">User defined data associated with the ClientActions class</param>
		public ServerConnector(object tag) : this(Client.server_info, Client.user_info, tag) { }

		/// <summary>
		/// Uses custom connection information to authenticate and communicate with the server.
		/// </summary>
		/// <param name="server">Server information to use.</param>
		/// <param name="user">User connection information to use.</param>
		public ServerConnector(ServerConnectorServerInformation server, ServerConnectorUserInformation user) : this(server, user, null) { }

		/// <summary>
		/// Uses custom connection information to authenticate and communicate with the server and defines tag.
		/// </summary>
		/// <param name="server">Server information to use.</param>
		/// <param name="user">User connection information to use.</param>
		/// <param name="tag">User defined data associated with the ClientActions class.</param>
		public ServerConnector(ServerConnectorServerInformation server, ServerConnectorUserInformation user, object tag) {
			if(Client.config == null) // NullReferenceException with designer...  This is to prevent errors/crashes.
				return;

			this.actions = new ClientActions(this, tag);
			this.server_info = server;
			this.user_info = user;
			this.tag = tag;

			Client.config.addValueChangedEvent("connector.max_download_kbps", config_changed_max_download_kbps);
			Client.config.addValueChangedEvent("connector.max_upload_kbps", config_changed_max_upload_kbps);
			Client.config.addValueChangedEvent("connector.max_download_enabled", config_changed_max_download_enabled);
			Client.config.addValueChangedEvent("connector.max_upload_enabled", config_changed_max_upload_enabled);

			// Go ahead and fix the upload values.
			config_changed_max_upload_kbps();
			config_changed_max_download_kbps();
		}

		#region Event methods to be called when certian configurations change.
		private void config_changed_max_download_kbps() {
			if(Client.config.get<bool>("connector.max_download_enabled", false) == true) {
				throttle_bytes_down = Client.config.get<long>("connector.max_download_kbps");

			} else { // User has chosen not to throttle.
				throttle_bytes_down = ThrottledStream.Infinite;
			}
		}

		private void config_changed_max_upload_kbps() {
			if(Client.config.get<bool>("connector.max_upload_enabled", false) == true) {
				throttle_bytes_up = Client.config.get<long>("connector.max_upload_kbps");

			} else { // User has chosen not to throttle.
				throttle_bytes_up = ThrottledStream.Infinite;
			}
		}

		private void config_changed_max_download_enabled() {
			if(Client.config.get<bool>("connector.max_download_enabled", false) == false) {
				throttle_bytes_down = ThrottledStream.Infinite;
			}
		}

		private void config_changed_max_upload_enabled() {
			if(Client.config.get<bool>("connector.max_upload_enabled", false) == false) {
				throttle_bytes_up = ThrottledStream.Infinite;
			}
		}
		#endregion

		/// <summary>
		/// Execute connection request and login.
		/// </summary>
		public void connect() {
			user_info.password_md5 = user_info.password;
			callServerMethod("User:verify");
		}


		/// <summary>
		/// Send a GET request and retrieve the body of the response in a string.  Synchronous.
		/// </summary>
		/// <param name="address">Address of the server to send the request to.</param>
		/// <returns>Body of the response.</returns>
		private string getString(Uri address) {
			// Pass back the second object that contains the body of the response.
			return getResponse(address, null).body;
		}

		/// <summary>
		/// Gets a response from a GET request sent to a server.  Synchronous.
		/// </summary>
		/// <param name="address">Address of the server to send the request to.</param>
		/// <param name="arguments">Any post data arguments to send.  Automatically turned into JSON and posted with a Name of "args"</param>
		/// <returns>The full response that the server sent.</returns>
		private ServerConnectorResponse getResponse(Uri address, params object[] arguments) {
			HttpWebRequest request = prepareRequest(address);
			if(arguments != null && arguments.Length > 0)
				writePostData(request, arguments);

			// Pass back the second object that contains the body of the response.
			return readServerResponse(request);
		}

		/// <summary>
		/// Asynchronous method to execute the server response.
		/// </summary>
		/// <param name="result">Contains the DC_ServerResponse that the server sent.</param>
		private void getResponseResult(IAsyncResult result) {
			GetResponseDelegate del = result.AsyncState as GetResponseDelegate;
			ServerConnectorResponse server_response = del.EndInvoke(result);
			execServerResponse(server_response);
		}

		private ServerConnectorResponse postFileStream(Uri address, FileStream file_stream) {
			byte[] buffer = new byte[512];
			byte[] header_bytes;
			Stream write_stream;
			int read_length = 0;
			ThrottledStream throttled_steam = null;
			var upload_args = new ServerConnectorUploadProgressChangedEventArgs();

			if(!file_stream.CanRead) // If we can not read it, what CAN we do?
				throw new ServerConnectorUploadLockedFile();

			HttpWebRequest request = prepareRequest(address);
			createPostBoundry(request);

			string content_split = "Content-Disposition: form-data; name=\"file\"; filename=\"{0}\"\r\nContent-Type: application/octet-stream\r\n\r\n";
			content_split = string.Format(content_split, Path.GetFileName(file_stream.Name));
			header_bytes = Encoding.UTF8.GetBytes(content_split);

			upload_args.total_bytes_to_send = file_stream.Length;
			request.ContentLength = boundry.boundry_start_bytes.Length + boundry.boundry_end_bytes.Length + file_stream.Length + header_bytes.Length;
			request.AllowWriteStreamBuffering = false;

			write_stream = request.GetRequestStream();

			// If the user wants to throttle, throttle the upload stream.
			if(_throttle_bytes_up != ThrottledStream.Infinite) {
				throttled_steam = new ThrottledStream(write_stream, _throttle_bytes_up * 1024);
				write_stream = throttled_steam;
			}

			write_stream.Write(boundry.boundry_start_bytes, 0, boundry.boundry_start_bytes.Length);
			write_stream.Write(header_bytes, 0, header_bytes.Length);

			while((read_length = file_stream.Read(buffer, 0, buffer.Length)) > 0) {
				write_stream.Write(buffer, 0, read_length);
				write_stream.Flush();

				if(throttle_changed && throttled_steam != null) { // If the upload throttle has changed, update the bandwidth.
					throttled_steam.MaximumBytesPerSecond = _throttle_bytes_up * 1024;
					throttle_changed = false;
				}

				if(cancel_request) {
					upload_canceled.Invoke();
					break;
				}

				// Update any events with this information
				upload_args.bytes_sent += read_length;
				upload_progress.Invoke(upload_args);
			}

			if(cancel_request == false) {
				write_stream.Write(boundry.boundry_end_bytes, 0, boundry.boundry_end_bytes.Length);
				write_stream.Close();
			}

			file_stream.Close();

			if(cancel_request) {
				cancel_request = false;
				request.Abort();

				_is_active = false;

				// Send out the queued query if any.
				requestQueueAdvance();
				return new ServerConnectorResponse();

			} else {
				return readServerResponse(request);
			}
		}


		/// <summary>
		/// Asynchronous method to handle the completion of the file upload process.
		/// </summary>
		/// <param name="result">Contains the DC_ServerResponse that the server sent.</param>
		private void postFileStreamResult(IAsyncResult result) {
			PostFileStreamDelegate del = result.AsyncState as PostFileStreamDelegate;
			ServerConnectorResponse server_response = del.EndInvoke(result);

			if(!server_response.canceled) {
				upload_completed.Invoke(server_response);
			}
		}

		/// <summary>
		/// Method to read the contents of the server response and return the response headers and body text.
		/// </summary>
		/// <param name="request">The request that is already in-progress.</param>
		/// <returns>First object is WebHeadersCollection containing the response headers, the second is the body text.</returns>
		private ServerConnectorResponse readServerResponse(HttpWebRequest request) {
			int length = 0;
			byte[] buffer = new byte[512];
			Stream response_stream;
			StringBuilder response_string = new StringBuilder();
			bool is_canceled = false;

			try {
				if(cancel_request) {
					cancel_request = false;
					return new ServerConnectorResponse() { canceled = true };
				}
				ThrottledStream throttled_steam = null;
				HttpWebResponse response = request.GetResponse() as HttpWebResponse;

				if(response.StatusCode != HttpStatusCode.OK) {
					Debug.WriteLine("Query[" + request.Address.Query + "] Returned Error: " + response.StatusDescription);
					return null;
				}

				response_stream = response.GetResponseStream();

				if(_throttle_bytes_down != ThrottledStream.Infinite){
					throttled_steam = new ThrottledStream(response_stream, _throttle_bytes_down * 1024);
					response_stream = throttled_steam;
				}
				

				while((length = response_stream.Read(buffer, 0, buffer.Length)) > 0) {
					if(throttle_changed && throttled_steam != null) { // Check to see if the throttle has changed.  If so, change the speed.
						throttled_steam.MaximumBytesPerSecond = _throttle_bytes_down * 1024;
						throttle_changed = false;
					}

					if(cancel_request)
						break;
					response_string.Append(Encoding.UTF8.GetString(buffer, 0, length));
				}

				response_stream.Close();
				response.Close();
				_is_active = false;

				// Send out the queued query if any.
				requestQueueAdvance();


				if(cancel_request == true) {
					cancel_request = false;
					is_canceled = true;
				}

				return new ServerConnectorResponse() {
					canceled = is_canceled,
					headers = response.Headers,
					body = response_string.ToString(),
					error = false
				};
			} catch(WebException e) {
				if(cancel_request == true) {
					cancel_request = false;
					is_canceled = true;
				}

				return new ServerConnectorResponse() {
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
		/// Method to be called before every post that has post data.
		/// </summary>
		/// <param name="request">The request to add the post boundary to.</param>
		private void createPostBoundry(HttpWebRequest request) {
			boundry.boundry_string = "----------------" + DateTime.Now.Ticks.ToString("x");
			boundry.boundry_start = "\r\n--" + boundry.boundry_string + "\r\n";
			boundry.boundry_start_bytes = Encoding.UTF8.GetBytes(boundry.boundry_start);
			boundry.boundry_end = "\r\n--" + boundry.boundry_string + "--\r\n";
			boundry.boundry_end_bytes = Encoding.UTF8.GetBytes(boundry.boundry_end);

			// Set the boundary string.
			request.ContentType = "multipart/form-data; boundary=" + boundry.boundry_string;
		}

		/// <summary>
		/// Sets all the required login information for the user to have a continuous login.
		/// </summary>
		/// <param name="address">Address of the server to send the request to.</param>
		/// <returns>The completed HttpWebRequest ready to send and request information.</returns>
		private HttpWebRequest prepareRequest(Uri address) {
			StringBuilder sb = new StringBuilder();

			HttpWebRequest http_request = HttpWebRequest.Create(address) as HttpWebRequest;
			
			http_request.Method = "POST";
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

		/// <summary>
		/// Creates a body that has JSON post data for the server to parse.
		/// </summary>
		/// <param name="request">The request that is to be altered with the post data.</param>
		/// <param name="postdata">Data to send to the server.  Must be parsable by JsonWriter.Write</param>
		private void writePostData(HttpWebRequest request, params object[] postdata) {
			if(postdata == null)
				return;
			createPostBoundry(request);
			StringBuilder body = new StringBuilder();
			JsonWriter json_writer = new JsonWriter(body);
			Stream write_stream;
			byte[] body_bytes;

			body.Append(boundry.boundry_start);
			body.Append("Content-Disposition: form-data; name=\"args\"\r\n\r\n");
			json_writer.Write(postdata);
			body.Append(boundry.boundry_end);

			body_bytes = Encoding.UTF8.GetBytes(body.ToString());

			request.ContentLength = body_bytes.Length;
			request.AllowWriteStreamBuffering = false;

			write_stream = request.GetRequestStream();
			write_stream.Write(body_bytes, 0, body_bytes.Length);
			write_stream.Close();
		}

		private void requestQueueAdvance() {
			if(request_queue.Count == 0) // Queue is empty.  Do nothing.
				return;

			RequestQuery query = request_queue.Dequeue();

			if(query.upload == true) { // A file has been queued to upload.
				uploadFile(query.file_location);

			} else { // A normal request has been queued.
				callServerMethod(query.method, query.callback, query.arguments);
			}
		}

		/// <summary>
		/// Disconnect this and all session on this client from the server.
		/// </summary>
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
		/// Cancel all current connection requests to the server.
		/// </summary>
		public void cancelActions() {
			cancel_request = true;
			request_queue.Clear(); // Clear out the queue.
		}

		/// <summary>
		/// Begin upload of a file to the server.
		/// </summary>
		/// <param name="file_location">Local file location on the client computer.</param>
		/// 
		public void uploadFile(string file_location) {
			RequestQuery queue_query = new RequestQuery() {
				upload = true,
				file_location = file_location
			};

			if(requestAlowed(queue_query) == false)
				return;

			FileStream fs = new FileStream(file_location, FileMode.Open, FileAccess.Read, FileShare.Read);
			Uri uri = buildUri("Files:upload");
			PostFileStreamDelegate post = new PostFileStreamDelegate(postFileStream);
			post.BeginInvoke(uri, fs, new AsyncCallback(postFileStreamResult), post);
		}

		/// <summary>
		/// Asynchronously execute a method on the server.
		/// </summary>
		/// <param name="method">Method name to execute on the server.</param>
		public void callServerMethod(string method) {
			callServerMethod(method, new object[0]);
		}

		/// <summary>
		/// Asynchronously execute a method on the server.
		/// </summary>
		/// <param name="method">Method name to execute on the server.</param>
		/// <param name="arguments">Arguments to pass to the method on the server.</param>
		public void callServerMethod(string method, params object[] arguments) {
			callServerMethod(method, null, arguments);
		}

		/// <summary>
		/// Asynchronously execute a method on the server and have a callback executed on a completed query.
		/// </summary>
		/// <param name="method">Method name to execute on the server.</param>
		/// <param name="callback">Delegate to call when the server request has been sent and the response has been read.</param>
		/// <param name="arguments">Arguments to pass to the method on the server.</param>
		public void callServerMethod(string method, ServerConnectorMethodCallback callback, params object[] arguments) {
			// Determine if multiple arguments are being passed or just one array.
			if(arguments.GetType().Equals(typeof(object[])) == false) {
				arguments = new object[] { arguments };
			}
			RequestQuery queue_query = new RequestQuery() {
				upload = false,
				method = method,
				callback = callback,
				arguments = arguments
			};

			if(requestAlowed(queue_query) == false)
				return;

			Uri uri = buildUri(method);
			GetResponseDelegate get = new GetResponseDelegate(getResponse);

			if(callback == null) {
				get.BeginInvoke(uri, arguments, new AsyncCallback(getResponseResult), get);

			} else {
				get.BeginInvoke(uri, arguments, new AsyncCallback(delegate(IAsyncResult result) {
					GetResponseDelegate del = result.AsyncState as GetResponseDelegate;
					ServerConnectorResponse server_response = del.EndInvoke(result);
					callback(server_response);
				}), get);
			}

		}

		/// <summary>
		/// Synchronously execute a method on the server and return the response.
		/// </summary>
		/// <param name="method">Method name to execute on the server.</param>
		public ServerConnectorResponse serverResponse(string method) {
			return serverResponse(method, null);
		}

		/// <summary>
		/// Synchronously execute a method on the server and return the response.
		/// </summary>
		/// <param name="method">Method name to execute on the server.</param>
		/// <param name="arguments">Arguments to pass to the method on the server.</param>
		public ServerConnectorResponse serverResponse(string method, params object[] arguments) {
			if(requestAlowed(null) == false)
				return null;

			Uri uri = buildUri(method);
			return getResponse(uri, arguments);
		}

		/// <summary>
		/// Method to handle multiple queries.
		/// </summary>
		/// <param name="query">Query to queue if the connector class accepts queuing.</param>
		/// <returns>True if the query is allowed to continue; False if the query must stop.</returns>
		/// <remarks>If the query argument is left null, the request is synchronous and an exception
		/// will be thrown if there is an active query.</remarks>
		private bool requestAlowed(RequestQuery query) {
			// Queue this up to be executed after the current request has finished.
			if(_is_active && _query_handling == ServerConnectorQueryHandling.queue) {
				request_queue.Enqueue(query);
				return false;

			} else if(_is_active && (_query_handling == ServerConnectorQueryHandling.throw_error || query == null)) {
				throw new ServerConnectorActiveException("Connector is currently executing a request.  Can only have one request running at a time.");
			
			} else {
				_is_active = true;
				return true;
			}
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
			} catch (Exception e){
				throw new ArgumentException("The build URI for the server was invalid.  This is due to invalid arguments or method called.", e);
			}

		}


		/// <summary>
		/// Method to handle the execution and error handling of the server response.
		/// </summary>
		/// <param name="response">Server sent response to deal with.</param>
		public void execServerResponse(ServerConnectorResponse response) {
			// If a transport error occurred, let the user know.
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
				try {
					if(response.body == "false" && param_count == 0) {
						requested_method.Invoke(actions, null);
					} else {
						requested_method.Invoke(actions, new object[] { response.body });
					}
				} catch(ClientActionsInternalException) { // Internal exception.  Add to a log list?

				} catch (Exception e){ // All other exceptions.
					Client.form_Login.Invoke((MethodInvoker)delegate {
						MessageBox.Show("Client unhandled exception.\n\nException:\n" + e.Message);
					});
				}

			} else {
				Client.form_Login.Invoke((MethodInvoker)delegate {
					MessageBox.Show("Server requested invalid method.  This is usually due to an outdated client.\n\nError:\n" + response.body);
				});
			}
		}

		~ServerConnector() {
			if(Client.config == null) // NullReferenceException with designer...  This is to prevent errors/crashes.
				return;

			// Remove all events added to the config.
			Client.config.removeValueChangedEvent("connector.max_download_kbps", config_changed_max_download_kbps);
			Client.config.removeValueChangedEvent("connector.max_upload_kbps", config_changed_max_upload_kbps);
			Client.config.removeValueChangedEvent("connector.max_download_enabled", config_changed_max_download_enabled);
			Client.config.removeValueChangedEvent("connector.max_upload_enabled", config_changed_max_upload_enabled);
		}
	}
}
