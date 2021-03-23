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
	class UIElement
	{
		public Point Position;

		public Point Size;

		public bool Moveable;

		public bool Focusable;
		public bool Focused;

		public Rectangle OuterCollider;
		public Rectangle InnerCollider;

		string frame;

		string frameTop;
		string frameSide;

		bool followMouse;

		public virtual void Initialize()
		{
			frameTop = "+";
			frameSide = "|";

			for(int i = 0; i < Size.X; i++)
			{
				frameTop += "=";
				frameSide += " ";
			}

			frameTop += "+";
			frameSide += "|";

			frame = frameTop + "\n";

			for (int i = 0; i < Size.Y; i++)
			{
				frame += frameSide + "\n";
			}

			frame += frameTop;

			OuterCollider = new Rectangle(Position, new Point(Size.X * 14, Size.Y * 18));

			InnerCollider = new Rectangle(Position.X + 14, Position.Y + 18, (Size.X - 2) * 14, (Size.Y - 2) * 18);
		}

		public virtual void Update()
		{
			if(Moveable == true)
			{
				if(Mouse.GetState().LeftButton == ButtonState.Pressed)
				{
					if (InnerCollider.Contains(Mouse.GetState().Position))
					{

					}
					else if (OuterCollider.Contains(Mouse.GetState().Position))
					{
						followMouse = true;
					}
				}
				else
				{
					followMouse = false;
				}

				if(followMouse == true)
				{
					Position = Mouse.GetState().Position;

					OuterCollider.Location = Position;
					InnerCollider.Location = new Point(Position.X + 14, Position.Y + 18);

				}
			}

			if(Focusable == true)
			{
				if (Mouse.GetState().LeftButton == ButtonState.Pressed)
				{
					if (InnerCollider.Contains(Mouse.GetState().Position) || OuterCollider.Contains(Mouse.GetState().Position))
					{
						Focused = true;
					}
					else
					{
						Focused = false;
					}
				}

				if(Focused == true)
				{
					InFocus();
				}
			}
		}

		public virtual void InFocus()
		{

		}

		public virtual void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(LoadedContent.Pixel, OuterCollider, Color.Green);

			spriteBatch.DrawString(LoadedContent.Fonts[0], frame, Position.ToVector2(), Color.White);
		}
	}
}
