using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Core;

namespace dtxUpload {
	static class Program {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {

			Client.config = new Config(delegate(Config current_config) {
				current_config.set("serverconnector.concurrent_connections_max", 2);

				DC_Server[] server_list = new DC_Server[1];
				server_list[0] = new DC_Server {
					name = "NFGaming Upload Server",
					url = "http://uploads.nfgaming.com",
					times_connected = 0
				};

				current_config.set("frmlogin.servers_list", server_list);
				current_config.set("uploads.total_screenshots", 0);
			});

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new frmLogin());
		}
	}
}
