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
			Position = new Point(100, 100);

			Size = new Point(30, 16);

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
			spriteBatch.DrawString(LoadedContent.Fonts[0], "------------------------------", new Vector2(InnerCollider.Location.X, InnerCollider.Location.Y + ((Size.Y - 5) * 19) + 5), Color.White);

			//Input text
			spriteBatch.DrawString(LoadedContent.Fonts[0], consoleInput, new Vector2(InnerCollider.Location.X, InnerCollider.Location.Y + ((Size.Y - 4) * 19) + 5), Color.Cyan); 

			//Chat logs
			for (int i = 0; i < 9; i++)
			{
				if (i >= ChatLines.Count)
				{
					break;
				}

				spriteBatch.DrawString(LoadedContent.Fonts[0],
					ChatLines[i].Line, new Vector2(InnerCollider.Location.X, (InnerCollider.Location.Y + (Size.Y - 6) * 19) - (i * 25) + 5), ChatLines[i].Color);
			}
		}
	}
}
