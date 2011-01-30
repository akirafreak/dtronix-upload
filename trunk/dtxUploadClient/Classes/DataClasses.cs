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
		public string session_key;
		public string client_username;
		public string client_password;
		public string client_password_md5;

		public int total_uploaded_size;
		public int total_allowed_upload_size;
		public int upload_size_limit;

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
		public int upload_date;
		public int hits;
		public string upload_id;
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

}
