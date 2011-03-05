using System;
using System.Collections.Generic;
using System.Text;
using Core.Json;
using System.IO;
using dtxUpload;

namespace Core {
	public static class Config {

		private static string settings_file = "settings.cfg";

		private static Dictionary<string, string> properties = new Dictionary<string, string>();

		public static void save() {
			StreamWriter sw = new StreamWriter(settings_file);

			foreach(string key in properties.Keys) {
				sw.Write(key);
				sw.Write("=");
				sw.Write(properties[key]);
				sw.Write(Environment.NewLine);
			}

			sw.Close();
		}

		public static void load() {
			if(!File.Exists(settings_file)) {
				initialSetup();

			} else {
				StreamReader sr = new StreamReader(settings_file);
				string line;

				while((line = sr.ReadLine()) != null) {
					int split = line.IndexOf('=');

					properties.Add(line.Substring(0, split), line.Substring(split + 1));
				}

				sr.Close();
			}

		}

		public static T get<T>(string name) {
			name = name.ToLower();
			if(properties.ContainsKey(name)) {
				JsonReader reader = new JsonReader(properties[name]);
				try {
					return reader.Deserialize<T>();
				} catch {
					return default(T);
				}
			} else {
				return default(T);
			}
		}

		public static int getAndIncrement(string name){
			int current_num = get<int>(name);
			set(name, current_num + 1);
			save();
			return current_num;
		}


		public static void set(string name, object value) {
			name = name.ToLower();
			StringBuilder sb = new StringBuilder();
			JsonWriter writer = new JsonWriter(sb);
			writer.Write(value);

			if(properties.ContainsKey(name)) {
				properties[name] = sb.ToString();
			} else {
				properties.Add(name, sb.ToString());
			}
		}

		private static void initialSetup() {
			set("serverconnector.concurrent_connections_max", 2);

			DC_Server[] server_list = new DC_Server[1];
			server_list[0] = new DC_Server {
				name = "NFGaming Upload Server",
				url = "http://uploads.nfgaming.com",
				times_connected = 0
			};

			set("frmlogin.servers_list", server_list);
			set("uploads.total_screenshots", 0);

			save();

		}
	}


}
