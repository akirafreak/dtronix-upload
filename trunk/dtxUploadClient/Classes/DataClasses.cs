using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

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
		public DC_FileInformationStatus status = DC_FileInformationStatus.UnknownStatus;

		// Permissions
		public bool is_public;
		public bool is_visible;
		public bool is_shared;
		public bool is_disabled;
		public string[] shared_ids;
	}

	public enum DC_FileInformationStatus {
		/// <summary>
		/// Do not know what the status of the file is.  Default value.
		/// </summary>
		UnknownStatus = -1,

		/// <summary>
		/// File is currently being uploaded to the server.
		/// </summary>
		Uploading = 1,

		/// <summary>
		/// File is uploaded to the server.
		/// </summary>
		Uploaded = 2,

		/// <summary>
		/// File has been deleted from the server.
		/// </summary>
		Deleted = 3,

		/// <summary>
		/// The file has been put in queue to be deleted from the server.  (Not implimented yet.)
		/// </summary>
		PendingDeletion = 4,

		/// <summary>
		/// The file is waiting to be uploaded to the server.
		/// </summary>
		PendingUpload = 5,

		/// <summary>
		/// File exists on the server, but is not avalible for viewing.
		/// </summary>
		Disabled = 6,

		/// <summary>
		/// Server could not handle the upload.
		/// </summary>
		UploadFailed = 7,

		/// <summary>
		/// User canceled the upload of the file.
		/// </summary>
		UploadCanceled = 8
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

	public class DC_UploadProgressChangedEventArgs{
		public long bytes_received = 0;
		public long bytes_sent = 0;
		public long total_byes_to_receive = 0;
		public long total_bytes_to_send = 0;
	}

}
