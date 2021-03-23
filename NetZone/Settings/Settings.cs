using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace NetZone
{
	class Settings
	{
		public static int ScreenWidth;
		public static int ScreenHeight;

		public static Vector3 Scale { get; protected set; }

		public static int NativeScreenHeight { get; } = 1080;
		public static int NativeScreenWidth { get; } = 1920;

		public static KeyBindings KeyBindings;

		public Settings(GraphicsDeviceManager graphics)
		{
			ScreenWidth = graphics.PreferredBackBufferWidth;
			ScreenHeight = graphics.PreferredBackBufferHeight;

			Scale = new Vector3(1, 1, 1);

			KeyBindings = new KeyBindings();
		}

		public void Update()
		{
			KeyBindings.Update();
		}
	}
}
