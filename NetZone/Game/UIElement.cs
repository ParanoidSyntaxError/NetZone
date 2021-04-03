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
		public string Title;

		public Point Position;

		public Point Size;

		public int Scale;

		public bool Moveable;

		public bool Focusable;
		public bool Focused;

		public Rectangle OuterCollider;
		public Rectangle InnerCollider;

		Point glyphSize;

		string frame;

		bool followMouse;

		public virtual void Initialize()
		{
			string frameTop = "+";
			string frameSide = "|";

			for(int i = 0; i < Size.X; i++)
			{
				frameTop += "=";
				frameSide += " ";
			}

			frameTop += "+";
			frameSide += "|";

			string frameBottom = frameTop;

			if(Title != null)
			{
				frameTop = frameTop.Remove(1, Title.Length);
				frameTop = frameTop.Insert(1, Title);
			}

			frame = frameTop + "\n";

			for (int i = 0; i < Size.Y; i++)
			{
				frame += frameSide + "\n";
			}

			frame += frameBottom;

			Size.X += 2;
			Size.Y += 2;

			glyphSize = new Point(8 * Scale, 16 * Scale);

			OuterCollider = new Rectangle(Position, new Point(Size.X * glyphSize.X, Size.Y * glyphSize.Y));

			InnerCollider = new Rectangle(Position.X + glyphSize.X, Position.Y + glyphSize.Y, (Size.X - 2) * glyphSize.X, (Size.Y - 2) * glyphSize.Y);
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
					InnerCollider.Location = new Point(Position.X + glyphSize.X, Position.Y + glyphSize.Y);
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
			GlyphHelper.DrawGlyph(spriteBatch, frame, Position, Scale, Color.White);
		}
	}
}
