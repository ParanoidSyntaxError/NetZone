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
	struct ConsoleLine
	{
		public string Line;
		public Color Color;
	}

	class DebugConsole
	{
		List<ConsoleLine> ConsoleLines = new List<ConsoleLine>();

		string consoleInput = "";

		Keys lastKey;
		bool canInput;

		public DebugConsole()
		{

		}

		public void NewLine(string line, Color color)
		{
			ConsoleLines.Insert(0, new ConsoleLine() { Line = line, Color = color });
		}

		public void Update()
		{
			//Debug Console / Future use for Server class
			if (Keyboard.GetState().IsKeyUp(lastKey))
			{
				canInput = true;
			}

			if (canInput == true && consoleInput != "" && Keyboard.GetState().IsKeyDown(Keys.Enter))
			{
				NewLine(consoleInput, Color.LightGray);
				consoleInput = "";

				lastKey = Keys.Enter;
				canInput = false;
			}

			if (canInput == true && consoleInput != "" && Keyboard.GetState().IsKeyDown(Keys.Back))
			{
				consoleInput = consoleInput.Remove(consoleInput.Length - 1);

				lastKey = Keys.Back;
				canInput = false;
			}

			if (canInput == true && Keyboard.GetState().GetPressedKeys().Length > 0 &&
				Keyboard.GetState().GetPressedKeys()[0] != Keys.Enter &&
				Keyboard.GetState().GetPressedKeys()[0] != Keys.Back)
			{
				consoleInput += Keyboard.GetState().GetPressedKeys()[0];
				lastKey = Keyboard.GetState().GetPressedKeys()[0];

				canInput = false;
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.DrawString(LoadedContent.Fonts[0], consoleInput, new Vector2(50, 1000), Color.White);

			for (int i = 0; i < 35; i++)
			{
				if (i >= ConsoleLines.Count)
				{
					break;
				}

				spriteBatch.DrawString(LoadedContent.Fonts[0],
					ConsoleLines[i].Line, new Vector2(50, 950 - (i * 25)), ConsoleLines[i].Color);
			}
		}
	}
}
