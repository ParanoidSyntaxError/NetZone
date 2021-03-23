using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NetZone
{
	class Item
	{
		public string Art;

		public Point Position;

		public Color Color;

		public bool Highlighted;
		bool flashing;

		public Rectangle Collider;

		public bool Active;

		public Item()
		{
			Collider = new Rectangle(Position, new Point(12, 16));
		}

		public virtual void Update()
		{
			if (Highlighted == true)
			{
				flashing = Main.GlobalFlash;
			}
			else
			{
				flashing = false;
			}

			Collider.Location = Position;
		}

		public virtual void Draw(SpriteBatch spriteBatch)
		{
			if (flashing == true)
			{
				spriteBatch.Draw(LoadedContent.Pixel, new Rectangle(new Point(Position.X - 1, Position.Y - 1), new Point(12, 16)), Color);

				spriteBatch.DrawString(LoadedContent.Fonts[0], Art, Position.ToVector2(), Color.Black);
			}
			else
			{
				spriteBatch.DrawString(LoadedContent.Fonts[0], Art, Position.ToVector2(), Color);
			}
		}
	}
}
