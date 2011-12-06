using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace DtxUpload {
	public class ServerConnectorServerInformation {
		public string server_name;
		public string server_url;
		public string upload_base_url;
		public string server_logo;

		public int max_upload_filesize;
		public string allowed_filetypes;
		public bool maintenance_mode;

		public bool is_registration_allowed;
		public bool is_key_required;

		public bool is_connected;
		public bool is_connecting;
		public bool is_key_valid;
	}

	public class ServerConnectorResponse {
		/// <summary>
		/// Response headers.  Null on error.
		/// </summary>
		public WebHeaderCollection headers = null;

		/// <summary>
		/// Text body of the response.
		/// </summary>
		public string body = null;

		/// <summary>
		/// True if there was an error on trying to read the response.
		/// </summary>
		public bool error = false;

		/// <summary>
		/// Full error message.
		/// </summary>
		public string error_info = null;

		/// <summary>
		/// Status that contains the exception information.
		/// </summary>
		public WebExceptionStatus error_status;

		/// <summary>
		/// True if the request was canceled.
		/// </summary>
		public bool canceled = false;

		public string getCalledMethod() {
			if(headers == null)
				return null;

			return headers["Call-Client-Method"];
		}
	}


	public class ServerConnectorCacheRequest {
		public DateTime request_time;
		public object data;
	}

	public class ServerConnectorUploadProgressChangedEventArgs {
		public long bytes_received = 0;
		public long bytes_sent = 0;
		public long total_byes_to_receive = 0;
		public long total_bytes_to_send = 0;
	}

	public class ServerConnectorUserInformation {
		public string id;
		public string session_key;
		public string username;
		public string name;
		public string password;
		public string password_md5;
		public string registration_date;

		// Stats
		public ulong total_uploaded_filesizes;
		public int total_files_uploaded;
		public ulong max_upload_space;
		public ulong max_upload_size;

		// Permissions
		public bool can_upload;
		public bool can_connect;

	}

	/// <summary>
	/// Settings to change the way the connector handles concurrent requests.
	/// </summary>
	public enum ServerConnectorQueryHandling {
		/// <summary>
		/// Causes the requests to be queued up in a FIFO manor. 
		/// NOTE: Only applies to the asynchronous methods.
		/// </summary>
		/// <remarks>
		/// If a synchronous method is called simultaneously, the class will still throw a ServerConnectorActiveException.
		/// </remarks>
		queue,

		/// <summary>
		/// This will cause the ServerConnector to throw a ServerConnectorActiveException when
		/// a connection is ongoing and a second request is sent.
		/// </summary>
		throw_error
	}



	/// <summary>
	/// Exception that is raised when the ServerConnector is called to create a second request while a request is already active.
	/// </summary>
	[Serializable()]
	public class ServerConnectorActiveException : Exception {
		public ServerConnectorActiveException() : base() { }
		public ServerConnectorActiveException(string message) : base(message) { }
		public ServerConnectorActiveException(string message, System.Exception inner) : base(message, inner) { }
 
		protected ServerConnectorActiveException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }
	}

	/// <summary>
	/// Exception that is raised when the ServerConnector tries to upload a file when the file can not be read from.
	/// </summary>
	[Serializable()]
	public class ServerConnectorUploadLockedFile : Exception {
		public ServerConnectorUploadLockedFile() : base() { }
		public ServerConnectorUploadLockedFile(string message) : base(message) { }
		public ServerConnectorUploadLockedFile(string message, System.Exception inner) : base(message, inner) { }

		protected ServerConnectorUploadLockedFile(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }
	}
}
