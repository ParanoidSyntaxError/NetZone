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
	class PlayerPool
	{
		public Player[] Players;

		public PlayerPool(int size)
		{
			Players = new Player[size];

			for (int i = 0; i < Players.Length; i++)
			{
				Players[i] = new Player();
				Players[i].Active = false;
			}
		}

		public void Update(GameTime gameTime)
		{
			for (int i = 0; i < Players.Length; i++)
			{
				if (Players[i].Active == true)
				{
					Players[i].Update(gameTime);
				}
			}
		}

		public void SetPlayer(int index, Player player)
		{
			Players[index] = player;
			Players[index].Active = true;
		}

		public void SetUnusedPlayer(Player player)
		{
			for (int i = 0; i < Players.Length; i++)
			{
				if (Players[i].Active == false)
				{
					Players[i] = player;
					Players[i].Active = true;

					break;
				}
			}
		}

		public int GetUnusedIndex()
		{
			for (int i = 0; i < Players.Length; i++)
			{
				if (Players[i].Active == false)
				{
					return i;
				}
			}

			return 0; //Pool overflow !!!
		}

		public void SetPositions(Point[] positions)
		{
			for (int i = 0; i < Players.Length; i++)
			{
				Players[i].Position = positions[i];
			}
		}

		public Point[] GetAllPositions()
		{
			Point[] positions = new Point[Players.Length];

			for (int i = 0; i < Players.Length; i++)
			{
				positions[i] = Players[i].Position;
			}

			return positions;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			for (int i = 0; i < Players.Length; i++)
			{
				if (Players[i].Active == true)
				{
					Players[i].Draw(spriteBatch);
				}
			}
		}
	}
}