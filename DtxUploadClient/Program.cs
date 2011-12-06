using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Text;
using dtxCore;
using dtxCore.Json;
using System.IO;
using System.Net;

namespace DtxUpload {
	static class Program {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
#if !DEBUG
			try {
#endif
				// Load all configurations and if the config file does not exist, generate a new one.
			string settings_file = Path.Combine(Client.directory_app_data, "settings.dcf");

				Client.config = new Config(delegate(Config current_config) {
					DC_Server[] server_list = new DC_Server[1];
					server_list[0] = new DC_Server {
						name = "NFGaming Upload Server",
						url = "http://uploads.nfgaming.com",
						times_connected = 0
					};

					current_config.set("frmlogin.servers_list", server_list);
				}, settings_file);
				
				// Set the max connections this program is allowed to have to a HTTP server.
				System.Net.ServicePointManager.DefaultConnectionLimit = Client.config.get<short>("net.default_connection_limit", 4);

				Client.config.save();

				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new frmLogin());

				// Run the cleanup script since the main form has been closed.
				cleanup();
				
#if !DEBUG
			} catch(Exception e) {
				StringBuilder sb = new StringBuilder();
				JsonWriter jw = new JsonWriter(sb);
				DC_Exception exception = new DC_Exception() {
					help_link = e.HelpLink,
					inner_exception_message = (e.InnerException != null) ? e.InnerException.Message : null,
					inner_exception_stack_trace = (e.InnerException != null)? e.InnerException.StackTrace : null,
					source = e.Source,
					stack_trace = e.StackTrace,
					message = e.Message
				};

				jw.Write(exception);

				string args = Utilities.base64Encode(sb.ToString());
				args += " ";
				args += "http://dtronix.com/dtxCrashReporter/";

				System.Diagnostics.Process.Start("dtxCrashReporter.exe", args);
			}
#endif
		}

		/// <summary>
		/// Cleanup method to run at the close of the program.
		/// </summary>
		static void cleanup() {
			// Clean out the temp directory.
			if(Directory.Exists(Client.directory_temp)) { // Check to see if the directory exists first.
				DirectoryInfo tmp_dir = new DirectoryInfo(Client.directory_temp);
				foreach(FileInfo file in tmp_dir.GetFiles()) {
					try {
						file.Delete();

					} catch {
						// Could not delete a file this time around. Try again next time.
					}
				}
			}
		}
	}
}
