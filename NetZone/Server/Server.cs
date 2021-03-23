using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NetZone
{
	class Server
	{
		public static int MaxPlayers;

		public static int Port;

		public static Dictionary<int, Listener> Listeners = new Dictionary<int, Listener>();

		static TcpListener tcpListener;

		public static GameLogic GameLogic;

		public static DebugConsole DebugConsole;

		public Server()
		{
			DebugConsole = new DebugConsole();

			MaxPlayers = 10;
			Port = 8080;

			DebugConsole.NewLine("Starting server...", Color.Yellow);

			for (int i = 0; i < MaxPlayers; i++)
			{
				Listeners.Add(i, new Listener(i));
			}

			tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), Port);
			tcpListener.Start();
			tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallBack), null);

			DebugConsole.NewLine($"Server started on {IPAddress.Parse("127.0.0.1")} : {Port}.", Color.LawnGreen);

			GameLogic = new GameLogic();
		}

		static void TCPConnectCallBack(IAsyncResult result)
		{
			TcpClient tcpClient = tcpListener.EndAcceptTcpClient(result);
			tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallBack), null);

			DebugConsole.NewLine($"Incoming connection from {tcpClient.Client.RemoteEndPoint}...", Color.Yellow);

			for (int i = 0; i < MaxPlayers; i++)
			{
				if(Listeners[i].Socket.TcpClient == null)
				{
					Listeners[i].Socket.Connect(tcpClient);

					DebugConsole.NewLine($"{tcpClient.Client.RemoteEndPoint} connected to the server.", Color.LawnGreen);

					PacketManager.InitalServerConnectionSend(i);

					return;
				}
			}

			DebugConsole.NewLine($"{tcpClient.Client.RemoteEndPoint} failed to connect: Server full!", Color.Red);
		}

		public void Update(GameTime gameTime)
		{
			GameLogic.Update(gameTime);

			DebugConsole.Update();
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			DebugConsole.Draw(spriteBatch);
		}
	}
}
