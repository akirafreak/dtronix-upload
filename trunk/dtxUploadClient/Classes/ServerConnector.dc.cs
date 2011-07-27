using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace dtxUpload {
	public class DC_ServerInformation {
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

	public class DC_ServerResponse {
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


	public class DC_CacheRequest {
		public DateTime request_time;
		public object data;
	}

	public class DC_UploadProgressChangedEventArgs {
		public long bytes_received = 0;
		public long bytes_sent = 0;
		public long total_byes_to_receive = 0;
		public long total_bytes_to_send = 0;
	}

	public class DC_UserInformation {
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
}
