using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Net;

namespace DtxUpload {
	public class DC_Server {
		public string name;
		public string url;

		public int times_connected;
		public bool save_pass;

		public string username;
		public string password;
	}


	public class DC_FileInformation {
		public string url;
		public string url_id;
		public string local_file_location;

		public string total_views;
		public string tag;
		public string file_name;
		public long file_size;
		public string upload_date;
		public string upload_id;
		public string last_accessed;
		public string directory;
		public bool delete_after_upload = false;
		public DC_FileInformationStatus status = DC_FileInformationStatus.UnknownStatus;

		// Permissions
		public bool is_disabled;
	}
	public class DC_DirectoryInformation {
		public string id;
		public string url_id;
		public string name;
		public string owner_id;
		public string files;
		public string permissions;
		public string password;

		// Permissions.
		public string is_public = "0";
		public string is_locked = "0";

		public bool bool_public {
			get {
				return (is_public == "1") ? true : false;
			}
			set {
				is_public = (value) ? "1" : "0";
			}
		}
		public bool bool_locked {
			get {
				return (is_locked == "1") ? true : false;
			}
			set {
				is_locked = (value) ? "1" : "0";
			}
		}
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

	/// <summary>
	/// Genaric thread safe property container
	/// </summary>
	/// <typeparam name="T">This will be the type of the property that the class will contain.</typeparam>
	public class LockedProperty<T> {
		private object thread_lock = new object();
		private T _value;

		/// <summary>
		/// Sets the default value to be default(T)
		/// </summary>
		public LockedProperty() {
			_value = default(T);
		}

		/// <summary>
		/// Defines a custom initial value for the property.
		/// </summary>
		/// <param name="default_value">Inital value for the property.</param>
		public LockedProperty(T default_value){
			_value = default_value;
		}

		/// <summary>
		/// Contains the thread-safe property to get or retrieve. Initially, value will be default(T).
		/// </summary>
		public T value {
			get {
				lock(thread_lock) {
					return _value;
				}
			}
			set {
				lock(thread_lock) {
					_value = value;
				}
			}
		}
	}
}
