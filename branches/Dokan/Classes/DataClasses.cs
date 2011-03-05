using System;
using System.Collections.Generic;
using System.Text;

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

	public class DC_Server {
		public string name;
		public string url;

		public int times_connected;
		public bool save_pass;

		public string username;
		public string password;
	}


	public class DC_UserInformation {
		public string id;
		public string session_key;
		public string username;
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


	public class DC_FileInformation {
		public string url;
		public string url_id;
		public string local_file_location;
		public string tag;
		public string file_name;
		public long file_size;
		public string upload_date;
		public int hits;
		public string upload_id;
		public string last_accessed;
		public bool delete_after_upload = false;

		/// <summary>
		/// -1 = Unknown; 1 = Uploading; 2 = Uploaded; 3 = Deleted; 4 = Pending Deletion; 5 = Pending Upload; 5 = Disabled; 6 = Failed Uploading;
		/// </summary>
		public int status = -1;

		// Permissions
		public bool is_public;
		public bool is_visible;
		public bool is_shared;
		public bool is_disabled;
		public int[] shared_ids;
	}

	public class DC_ImageInformation {
		public string full_location;
		public long size;
		public string type;
	}

	public class DC_Exception {
		public string inner_exception_message;
		public string inner_exception_stack_trace;
		public string crashed_program = "dtxUpload.exe";
		public string help_link;
		public string stack_trace;
		public string source;
		public string message;
	}

	public class DC_CacheRequest {
		public DateTime request_time;
		public object data;
	}

}
