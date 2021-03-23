using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace NetZone
{
	class Listener
	{
		public int ID;

		public TCPSocket Socket;

		public Listener(int listenerID)
		{
			ID = listenerID;

			Socket = new TCPSocket(listenerID);
		}
	}
}
