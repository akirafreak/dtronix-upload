using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace dtxUpload {
	class DtxHttpRequest {
		private Socket server;

		public WebHeaderCollection headers = new WebHeaderCollection();

		public DtxHttpRequest(Uri address){
			server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			//server.Receive(
			while(true) {
				string input = Console.ReadLine();
				if(input == "exit")
					break;
				server.Send(Encoding.ASCII.GetBytes(input));
				byte[] data = new byte[1024];
				int receivedDataLength = server.Receive(data);
				string stringData = Encoding.ASCII.GetString(data, 0, receivedDataLength);
				Console.WriteLine(stringData);
			}

			server.Shutdown(SocketShutdown.Both);
			server.Close();
		}

		public void writeToStream(byte[] buffer, int offset, int count){
			server.Send(buffer, offset, count, SocketFlags.None);
		}

		// Ensure that we close the socket.
		~DtxHttpRequest() {
			if(server != null) {
				server.Close();
			}
		}


	}
}
