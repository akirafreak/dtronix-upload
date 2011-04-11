using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;


namespace dtxPhPacker {
	class Program {
		
		static void Main(string[] args) {
			// Get all the files in
			new GenerateOutput(Directory.GetCurrentDirectory());
		}
	}

	class GenerateOutput {
		private List<string[]> pack_items = new List<string[]>();
		private string base_dir;
		private string[] ignore_folders = new string[] { ".svn", "nbproject"};
		private string[] ignore_files = new string[] { "config.php", "install.data.php", "install.data", "install.info.php" };
		private StreamWriter data_stream;
		private DateTime project_start_date;
		private string project_version;

		public GenerateOutput(string base_dir) {
			this.base_dir = base_dir + "\\";

			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("Adding Files---------------------------------------");
			Console.ResetColor();
			crawlFolder(new DirectoryInfo(base_dir));
			Console.WriteLine("Added " + pack_items.Count.ToString() + " Files.");

			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("Compiling Files------------------------------------");
			Console.ResetColor();
			generateInstaller();

			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("Creating Version File------------------------------");
			Console.ResetColor();
			generateVersionFile();

			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("Compressing Data File------------------------------");
			Console.ResetColor();
			compressFile(base_dir + "\\install.data.php", base_dir + "\\install.data");
			File.Delete(base_dir + "\\install.data.php");

			Console.ForegroundColor = ConsoleColor.DarkGreen;
			Console.WriteLine("Complete");
			Console.ResetColor();
			
			Console.ReadLine();
		}


		private void generateInstaller() {
			data_stream = new StreamWriter(base_dir + "install.data.php");
			List<string> directories = new List<string>();

			data_stream.WriteLine("<?PHP");
			
			data_stream.WriteLine("// Check to see if this file is included into the main file for security reasons.");
			data_stream.WriteLine("if( !defined(\"requireParrent\") ) die(\"Restricted Access\");");

			foreach(string[] item in pack_items){
				if(item[1] == null) {
					// See if this is an empty folder
					if(!directories.Contains(item[0])) {
						directories.Add(item[0]);
					}

				} else {
					// This item is a normal file.
					int dir_end = item[0].LastIndexOf('/');
					if(dir_end == -1) {
						writeVarType(data_stream, "_INSTALLER_FILE", "/" + item[0], item[1]);

					} else {
						string tmp_dir = item[0].Substring(0, dir_end);

						if(dir_end != -1 && tmp_dir != "" && !directories.Contains(tmp_dir)) {
							directories.Add(tmp_dir);
						}

						writeVarType(data_stream, "_INSTALLER_FILE", "/" + item[0], item[1]);
					}
				}
			}

			foreach(string dir in directories) {
				writeVarType(data_stream, "_INSTALLER_DIRS", null, "/" + dir);
			}

			data_stream.WriteLine("?>");
			data_stream.Close();
			
		}

		
		private void compressFile(string source_file, string destination_file) {

			FileStream original_file = new FileStream(source_file, FileMode.Open, FileAccess.Read, FileShare.Read);
			byte[] buffer = new byte[original_file.Length];

			// Make sure the file can be read first.
			int count = original_file.Read(buffer, 0, buffer.Length);
			if(count != buffer.Length) {
				original_file.Close();
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Error: Could not open " + source_file +"For reading.");
				Console.ResetColor();
				return;
			}
			original_file.Close();

			Stream out_stream = File.Create(destination_file);
			GZipStream gZip = new GZipStream(out_stream, CompressionMode.Compress, true);

			gZip.Write(buffer, 0, buffer.Length);

			gZip.Close();
			Console.WriteLine("Original size: {0}, Compressed size: {1}", buffer.Length, out_stream.Length);
			out_stream.Close();
		}

		private void generateVersionFile() {
			if(project_start_date == null) {
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Error: Could not detetrmine file version");
				Console.ResetColor();
				return;
			}

			StreamWriter version_stream = new StreamWriter(base_dir + "install.info.php");

			version_stream.WriteLine("<?PHP");

			// Ensure that the file is not run by itself.
			version_stream.WriteLine("// Check to see if this file is included into the main file for security reasons.");
			version_stream.WriteLine("if( !defined(\"requireParrent\") ) die(\"Restricted Access\");");

			if(project_start_date != null) {				
				// Add the build to the end of the version.
				writeVarType(version_stream, "_INSTALL_INFO", "version", project_version + "." + ((int)DateTime.Now.Subtract(project_start_date).TotalDays).ToString());

				Console.WriteLine("Generating MD5 hash for install.data...");
				// Generate MD5 Hash for installation file.
				writeVarType(version_stream, "_INSTALL_INFO", "MD5_hash", fileMD5(base_dir + "install.data.php"));
			}

			version_stream.WriteLine("?>");
			version_stream.Close();

			Console.ForegroundColor = ConsoleColor.DarkGreen;
			Console.WriteLine("Version file created.");
			Console.ResetColor();

		}



		private string fileMD5(string file_name) {
			FileStream file = new FileStream(file_name, FileMode.Open);
			byte[] retVal = new MD5CryptoServiceProvider().ComputeHash(file);
			file.Close();

			StringBuilder sb = new StringBuilder();
			for(int i = 0; i < retVal.Length; i++) {
				sb.Append(retVal[i].ToString("x2"));
			}
			return sb.ToString();
		}





		private void writeVarType(StreamWriter stream, string type, string prop, string value) {
			stream.Write("$");
			stream.Write(type);
			stream.Write("[");
			if(prop != null)
				stream.Write("\"");
			stream.Write(prop);
			if(prop != null)
				stream.Write("\"");
			stream.Write("] = \"");
			stream.Write(value);
			stream.WriteLine("\";");
		}

		private void crawlFolder(DirectoryInfo dir) {
			FileInfo[] files = dir.GetFiles();
			DirectoryInfo[] directories = dir.GetDirectories();

			foreach(DirectoryInfo curr_dir in directories) {
				bool allowed = true;
				foreach(string ignore_folder in ignore_folders) {
					if(curr_dir.FullName.Contains(ignore_folder)){
						allowed = false;
						break;
					}
				}

				if(allowed) {
					crawlFolder(curr_dir);
				} else {
					Console.WriteLine("Skipping: " + curr_dir.FullName);
				}
			}

			if(files.Length == 0) {
				pack_items.Add(new string[]{
					dir.FullName.Replace(base_dir, "").Replace('\\', '/'),
					null
				});
			}

			foreach(FileInfo file in files) {
				bool allowed = true;

				if(file.Name == "dtxUpload.php") {
					Console.WriteLine("Detecting Version...");
					StreamReader version_file = file.OpenText();
					version_file.ReadLine();
					project_start_date = DateTime.Parse(version_file.ReadLine().Replace("//Start Date:", ""));
					project_version = version_file.ReadLine().Replace("//Version:", "");
					Console.WriteLine("Version: " + project_start_date);
				}

				foreach(string ignore_file in ignore_files) {
					if(file.Name.ToLower() == ignore_file) {
						allowed = false;
						break;
					}
				}

				if(file.Extension != ".exe" && allowed) {
					Console.WriteLine("Adding: " + file.FullName);
					addFile(file);
				}
				
			}
		}

		private void addFile(FileInfo file) {
			pack_items.Add(new string[]{
				file.FullName.Replace(base_dir, "").Replace('\\', '/'),
				fileToBase64(file)
			});
		}

		private string fileToBase64(FileInfo file) {
			byte[] file_bytes = File.ReadAllBytes(file.FullName);
			return Convert.ToBase64String(file_bytes);
		}
	}


}
