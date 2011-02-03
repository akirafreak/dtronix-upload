﻿using System;
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

			// Load all configurations and if the config file does not exist, generate a new one.
			Client.config = new Config(delegate(Config current_config) {
				DC_Server[] server_list = new DC_Server[1];
				server_list[0] = new DC_Server {
					name = "NFGaming Upload Server",
					url = "http://uploads.nfgaming.com",
					times_connected = 0
				};

				current_config.set("frmlogin.servers_list", server_list);
			});

			// Set configurations if they are not already set.
			Client.config.setIfNotSet("serverconnector.concurrent_connections_max", 2);
			Client.config.setIfNotSet("uploads.total_screenshots", 0);
			Client.config.setIfNotSet("frmquickupload.show_clipboard_confirmation", true);
			Client.config.save();

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new frmLogin());
		}
	}
}