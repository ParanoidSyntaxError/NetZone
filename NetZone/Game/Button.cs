using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace NetZone
{
	class Button
	{
		public Rectangle Collider;

		Color color;

		Color idle;
		Color hover;

		string text;
		string frame;

		public Button(Point position, Color idleColor, Color hoverColor, string buttonText = null)
		{
			idle = idleColor;
			hover = hoverColor;

			color = idle;

			text = buttonText;

			Collider = new Rectangle(position, new Point(368, 96));

			frame = "╔═════════════════════╗" + "\n" +
					"║                     ║" + "\n" +
					"╚═════════════════════╝";
		}

		public bool OnClick(bool worldSpace = false)
		{
			bool pressed = false;

			Point mousePosition = Point.Zero;

			if (worldSpace == true)
			{
				mousePosition = Main.MainCamera.GetMousePosition();
			}
			else
			{
				mousePosition = Main.UICamera.GetMousePosition();
			}

			if (Collider.Contains(mousePosition))
			{
				color = hover;

				if (Mouse.GetState().LeftButton == ButtonState.Pressed)
				{
					pressed = true;
				}
			}
			else
			{
				color = idle;
			}

			return pressed;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			GlyphHelper.DrawGlyph(spriteBatch, frame, Collider.Location, 2, color);
			GlyphHelper.DrawGlyph(spriteBatch, text, new Point(Collider.Location.X + 32, Collider.Location.Y + 32), 2, color);
		}
	}
}
