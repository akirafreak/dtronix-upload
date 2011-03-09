using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace dtxUpload {
	public class DtxHttpRequest {
		private Socket server;

		public WebHeaderCollection headers = new WebHeaderCollection();
		public Dictionary<string, string> post_data = new Dictionary<string,string>();

		private string nl = "\r\n";

		public DtxHttpRequest(Uri address){
			// Create default headers.
			/*
			
POST http://nfgaming.com/forum/index.php?action=post2;start=0;board=10 HTTP/1.1
Host: nfgaming.com
User-Agent: Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.15) Gecko/20110303 Firefox/3.6.15
Accept: text/html
Accept-Language: en-us,en;q=0.5
Accept-Encoding: gzip,deflate
Accept-Charset: ISO-8859-1,utf-8;q=0.7,*;q=0.7
Keep-Alive: 115
Connection: keep-alive
Referer: http://nfgaming.com/forum/index.php?action=post;board=10.0
Cookie: SMFCookie463=a%3A4%3A%7Bi%3A0%3Bs%3A1%3A%224%22%3Bi%3A1%3Bs%3A40%3A%223dc9175373a05b1293c3d29b75e542d1549a8048%22%3Bi%3A2%3Bi%3A1488168095%3Bi%3A3%3Bi%3A3%3B%7D; PHPSESSID=41436971bd691a4d10d105ededf360b1
Content-Type: multipart/form-data; boundary=---------------------------288301069040
Content-Length: 110458
			 */
			headers.Add(




			server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			server.Blocking = true;
		}

		public void writeToStream(byte[] buffer, int offset, int count){
			server.Send(buffer, offset, count, SocketFlags.None);
		}

		public DtxHttpResponse getResponse() {

			return new DtxHttpResponse(server);
		}

		public void close(){
			server.Shutdown(SocketShutdown.Both);
			server.Close();
		}

		// Ensure that we close the socket.
		~DtxHttpRequest() {
			if(server != null) {
				server.Close();
			}
		}


	}

	public class DtxHttpResponse {
		private Socket server;
		public WebHeaderCollection headers;

		public DtxHttpResponse(Socket socket) {
			server = socket;
		}

		private void readHeaders() {

		}

		public int readFromStream(byte[] buffer, int offset, int count) {
			return server.Receive(buffer, offset, count, SocketFlags.None);
		}
	}
}
