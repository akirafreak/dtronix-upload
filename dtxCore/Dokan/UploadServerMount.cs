using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;
using dtxUpload;
using dtxCore.Json;

namespace dtxCore.Dokan {

	public class UploadServerMount : DokanOperations {

		public ServerConnector connector;
		private string _root;
		private int _count;

		public UploadServerMount() {
			connector = new ServerConnector();
			_count = 1;
		}

		private string GetPath(string filename) {
			string path = _root + filename;
			return path;
		}

		public int CreateFile(String filename, FileAccess access, FileShare share, FileMode mode, FileOptions options, DokanFileInfo info) {
			if(filename.Contains("-") || filename == "\\") {
				//string[] file_id = filename.Split('-');
				return DokanNet.DOKAN_SUCCESS;
			} else {
				return DokanNet.DOKAN_ERROR;
			}
		}

		public int OpenDirectory(String filename, DokanFileInfo info) {
			return DokanNet.DOKAN_SUCCESS;
		}

		public int CreateDirectory(String filename, DokanFileInfo info) {
			return -1;
		}

		public int Cleanup(String filename, DokanFileInfo info) {
			//Console.WriteLine("%%%%%% count = {0}", info.Context);
			return 0;
		}

		public int CloseFile(String filename, DokanFileInfo info) {
			return 0;
		}

		public int ReadFile(String filename, Byte[] buffer, ref uint readBytes,	long offset, DokanFileInfo info) {
			try {
				string[] file_id = Path.GetFileName(filename).Split('-');
				buffer = connector.partialFileDownloadSyncronous(file_id[0], offset, 1024 * 64);
				readBytes = uint.Parse(buffer.LongLength.ToString());
				return 0;
			} catch(Exception e) {
				return -1;
			}
		}

		public int WriteFile(String filename, Byte[] buffer, ref uint writtenBytes, long offset, DokanFileInfo info) {
			return -1;
		}

		public int FlushFileBuffers(String filename, DokanFileInfo info) {
			return -1;
		}

		public int GetFileInformation(String filename, FileInformation fileinfo, DokanFileInfo info) {
			if(info.IsDirectory) {
				fileinfo.Attributes = FileAttributes.Directory;
				fileinfo.Length = 0;
				fileinfo.CreationTime = DateTime.Now;
				fileinfo.FileName = filename;
				fileinfo.LastAccessTime = DateTime.Now;
				fileinfo.LastWriteTime = DateTime.Now;

				return DokanNet.DOKAN_SUCCESS;
			}
			string[] file_id = filename.Split('-');

			 DC_FileInformation file_info = connector.callServerMethodSyncronous<DC_FileInformation>("file_info", true, file_id[0]);

			fileinfo.Attributes = FileAttributes.ReadOnly;
			fileinfo.Length = file_info.file_size;
			fileinfo.CreationTime = DateTime.Parse(file_info.upload_date);
			fileinfo.FileName = file_info.url_id + "-" + file_info.file_name;
			fileinfo.LastAccessTime = DateTime.Parse(file_info.last_accessed);
			fileinfo.LastWriteTime = DateTime.Parse(file_info.upload_date);

			return DokanNet.DOKAN_SUCCESS;
		}



		public int FindFiles(String filename, ArrayList files, DokanFileInfo info) {
			DC_FileInformation[] directory_data = connector.callServerMethodSyncronous<DC_FileInformation[]>("files_in_directory", true, "/");

			if(directory_data.Length < 1) {
				return -1;

			} else if(directory_data.Length == 1 && directory_data[0].file_name == null) {
				return -1;

			} else {
				foreach(DC_FileInformation uploaded_file in directory_data) {
					files.Add(new FileInformation() {
						Attributes = FileAttributes.ReadOnly,
						Length = uploaded_file.file_size,
						CreationTime = DateTime.Parse(uploaded_file.upload_date),
						FileName = uploaded_file.url_id + "-" + uploaded_file.file_name,
						LastAccessTime = DateTime.Parse(uploaded_file.last_accessed),
						LastWriteTime = DateTime.Parse(uploaded_file.upload_date)
					});
				}

				return DokanNet.DOKAN_SUCCESS;
			}
		}


		public int SetFileAttributes(String filename, FileAttributes attr, DokanFileInfo info) {
			return -1;
		}

		public int SetFileTime(String filename, DateTime ctime,	DateTime atime, DateTime mtime, DokanFileInfo info) {
			return -1;
		}

		public int DeleteFile(String filename, DokanFileInfo info) {
			return -1;
		}

		public int DeleteDirectory(String filename, DokanFileInfo info) {
			return -1;
		}

		public int MoveFile(String filename, String newname, bool replace, DokanFileInfo info) {
			return -1;
		}

		public int SetEndOfFile(String filename, long length, DokanFileInfo info) {
			return -1;
		}

		public int SetAllocationSize(String filename, long length, DokanFileInfo info) {
			return -1;
		}

		public int LockFile(String filename, long offset, long length, DokanFileInfo info) {
			return 0;
		}

		public int UnlockFile(String filename, long offset, long length, DokanFileInfo info) {
			return 0;
		}

		public int GetDiskFreeSpace(ref ulong freeBytesAvailable, ref ulong totalBytes,	ref ulong totalFreeBytes, DokanFileInfo info) {
			DC_UserInformation user_info = connector.callServerMethodSyncronous<DC_UserInformation>("load_user_info", true, null);

			freeBytesAvailable = (user_info.max_upload_space - user_info.total_uploaded_filesizes);
			totalBytes = user_info.max_upload_space;
			totalFreeBytes = (user_info.max_upload_space - user_info.total_uploaded_filesizes);
			return 0;
		}

		public int Unmount(DokanFileInfo info) {
			return 0;
		}
	}
}
