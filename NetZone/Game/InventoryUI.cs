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
		public ItemPool inventory;

		Rectangle[] itemsBackgrounds;

		public InventoryUI(ItemPool itemPool)
		{
			Title = "INVENTORY";

			Position = new Point(400, 100);

			Size = new Point(19, 5);

			Scale = 2;

			Moveable = true;

			Initialize();

			inventory = itemPool;

			itemsBackgrounds = new Rectangle[itemPool.Length()];

			for (int i = 0; i < itemsBackgrounds.Length; i++)
			{
				itemsBackgrounds[i] = new Rectangle(0, 0, 8 * Scale, 16 * Scale);
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

			for (int i = 0; i < itemsBackgrounds.Length; i++)
			{
				itemsBackgrounds[i].Location = new Point(Position.X + (8 * Scale) + x * (16 * Scale), Position.Y + (16 * Scale) + y * (16 * Scale));

				x += 1;

				if(x % 10 == 0)
				{
					x = 0;
					y += 1;
				}
			}
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);

			for (int i = 0; i < itemsBackgrounds.Length; i++)
			{
				string icon = ".";
				Color iconColor = Color.White;

				if(inventory.GetActive(i) == true)
				{
					icon = inventory.GetItem(i).Art;
					iconColor = inventory.GetItem(i).Color;
				}

				GlyphHelper.DrawGlyph(spriteBatch, icon, itemsBackgrounds[i].Location, Scale, iconColor);
			}
		}
	}
}
