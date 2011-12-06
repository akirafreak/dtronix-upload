using System;
using System.Collections.Generic;
using System.Text;
using dtxCore;
using dtxCore.Forms;
using System.Threading;
using System.IO;

namespace DtxUpload {

	/// <summary>
	/// Static class to aid in the accessing of classes and methods.
	/// </summary>
	public static class Client {
		public static frmLogin form_Login;
		public static frmQuickUpload form_QuickUpload;
		public static frmManage form_Manage;

		public static Config config;

		public static ServerConnectorServerInformation server_info = new ServerConnectorServerInformation();
		public static ServerConnectorUserInformation user_info = new ServerConnectorUserInformation();

		public static Thread drive_mount_thread;

		public static string directory_temp = Path.Combine(Path.GetTempPath(), "dtxUpload");
		public static string directory_app_data = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "dtxUpload");
	}
}
