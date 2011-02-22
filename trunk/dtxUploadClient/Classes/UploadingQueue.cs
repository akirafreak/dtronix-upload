using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Net;
using System.IO;
using dtxCore.Json;
using System.Windows.Forms;

namespace dtxUpload.Classes {

	class UploadFileItem {
		public int id = 0;
		public string web_location = null;
		public string location = null;
		public int status = 0;
		public int percent_complete = 0;
	}

	class UploadingQueue {

		private List<UploadFileItem> file_list = new List<UploadFileItem>();
		private ServerConnector connector = new ServerConnector();
		public int concurrent_upload_limit = 1;
		private bool uploading = false;

		/// <summary>
		/// Add a file to the queue to be uploaded.
		/// </summary>
		/// <param name="location">Location of the file to upload.</param>
		/// <returns>True on successful addition; False on failure;</returns>
		public void addFile(string location) {
			if(!File.Exists(location)) return;
		}

		public void startUpload(){
			if(uploading) return;


		}
	}
}
