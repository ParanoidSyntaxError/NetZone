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
	class ChatBox : UIElement
	{
		List<ConsoleLine> ChatLines = new List<ConsoleLine>();

		string consoleInput = string.Empty;

		Keys lastKey;
		bool canInput;

		public ChatBox()
		{
			Title = "CHAT";

			Position = new Point(100, 100);

			Size = new Point(30, 16);

			Scale = 2;

			Moveable = true;

			Focusable = true;

			Initialize();
		}

		public void SendLine(string line, Color color)
		{
			PacketManager.ClientChatSend(line);
		}

		public void NewLine(string line, Color color)
		{
			ChatLines.Insert(0, new ConsoleLine() { Line = line, Color = color });
		}

		public override void Update()
		{
			base.Update();
		}

		public override void InFocus()
		{
			if (Keyboard.GetState().IsKeyUp(lastKey))
			{
				canInput = true;
			}

			if (canInput == true && consoleInput != string.Empty && Keyboard.GetState().IsKeyDown(Keys.Enter))
			{
				SendLine(consoleInput, Color.LightGray);
				consoleInput = string.Empty;

				lastKey = Keys.Enter;
				canInput = false;
			}

			if (canInput == true && consoleInput != string.Empty && Keyboard.GetState().IsKeyDown(Keys.Back))
			{
				consoleInput = consoleInput.Remove(consoleInput.Length - 1);

				lastKey = Keys.Back;
				canInput = false;
			}

			if (canInput == true && Keyboard.GetState().IsKeyDown(Keys.Space))
			{
				consoleInput += " ";

				lastKey = Keys.Space;
				canInput = false;
			}

			if (canInput == true && Keyboard.GetState().GetPressedKeys().Length > 0 &&
				Keyboard.GetState().GetPressedKeys()[0] != Keys.Enter &&
				Keyboard.GetState().GetPressedKeys()[0] != Keys.Back)
			{
				if (Keyboard.GetState().GetPressedKeys()[0].ToString().Length <= 2)
				{
					if (Keyboard.GetState().GetPressedKeys()[0].ToString().StartsWith("D") == true && //Numbers
						Keyboard.GetState().GetPressedKeys()[0].ToString().Length == 2)
					{
						consoleInput += Keyboard.GetState().GetPressedKeys()[0].ToString()[1];
						lastKey = Keyboard.GetState().GetPressedKeys()[0];

						canInput = false;
					}
					else if (Keyboard.GetState().GetPressedKeys()[0].ToString().Length == 1) //Letters
					{
						consoleInput += Keyboard.GetState().GetPressedKeys()[0];
						lastKey = Keyboard.GetState().GetPressedKeys()[0];

						canInput = false;
					}

				}
			}
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);
			
			//Input line
			GlyphHelper.DrawGlyph(spriteBatch, "------------------------------", new Point(Position.X + (8 * Scale), Position.Y + (Size.Y - 3) * (16 * Scale)), Scale, Color.White);

			//Input text
			GlyphHelper.DrawGlyph(spriteBatch, consoleInput, new Point(Position.X + (8 * Scale), Position.Y + (Size.Y - 2) * (16 * Scale)), 1, Color.White);

			//Chat logs
			for (int i = 0; i < 9; i++)
			{
				if (i >= ChatLines.Count)
				{
					break;
				}

				GlyphHelper.DrawGlyph(spriteBatch, ChatLines[i].Line, new Point(Position.X + (8 * Scale), Position.Y + (Size.Y - 4) * (16 * Scale) - (i * 16 * Scale)), 1, ChatLines[i].Color);
			}
		}
	}
}
