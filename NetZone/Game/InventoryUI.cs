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
	class InventoryUI : UIElement
	{
		ItemPool inventory;

		Rectangle[] items;

		public InventoryUI(ItemPool itemPool)
		{
			Position = new Point(400, 100);

			Size = new Point(25, 30);

			Moveable = true;

			Initialize();

			inventory = itemPool;

			items = new Rectangle[inventory.Items.Length];

			for (int i = 0; i < items.Length; i++)
			{
				items[i] = new Rectangle(0, 400, 20, 20);
			}

			PositionItemColliders();
		}

		public override void Update()
		{
			PositionItemColliders();

			base.Update();
		}

		void PositionItemColliders()
		{
			int x = 0;
			int y = 0;

			for (int i = 0; i < items.Length; i++)
			{
				items[i].Location = new Point(Position.X + 16 + x * 20, Position.Y + 19 + y * 20);

				x += 1;

				if(x % 5 == 0)
				{
					x = 0;
					y += 1;
				}
			}
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);

			for (int i = 0; i < items.Length; i++)
			{
				string icon = "G";
				Color iconColor = Color.White;

				spriteBatch.Draw(LoadedContent.Pixel, items[i], Color.Green);

				if(inventory.Items[i].Active == true)
				{
					icon = inventory.Items[i].Art;
					iconColor = inventory.Items[i].Color;
				}

				spriteBatch.DrawString(LoadedContent.Fonts[0], icon, items[i].Location.ToVector2(), iconColor);
			}
		}
	}
}
