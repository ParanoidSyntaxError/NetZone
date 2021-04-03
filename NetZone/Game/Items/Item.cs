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

		public Item()
		{
			Collider = new Rectangle(Position, new Point(16, 32));
		}

		public void Update()
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

		public void Draw(SpriteBatch spriteBatch)
		{
			if (flashing == true)
			{
				spriteBatch.Draw(LoadedContent.Pixel, Collider, Color);

				GlyphHelper.DrawGlyph(spriteBatch, Art, Position, 2, Color.Black);
			}
			else
			{
				GlyphHelper.DrawGlyph(spriteBatch, Art, Position, 2, Color);
			}
		}
	}
}
