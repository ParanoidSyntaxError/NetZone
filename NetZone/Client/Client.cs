using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NetZone
{
	class Client
	{
		public static string IP = "127.0.0.1";
		public static int Port = 8080;

		public static TCPSocket Socket;

		public static GameScreen GameScreen;

		public Client()
		{
			Socket = new TCPSocket(IP, Port);

			ConnectToServer();

			GameScreen = new GameScreen();
		}

		public void ConnectToServer()
		{
			Socket.Connect();
		}

		public static int GetID()
		{
			return Socket.ID;
		}

		public void Update(GameTime gameTime)
		{
			GameScreen.Update(gameTime);
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			GameScreen.Draw(spriteBatch);
		}

		public void DrawUI(SpriteBatch spriteBatch)
		{
			GameScreen.DrawUI(spriteBatch);
		}
	}
}
