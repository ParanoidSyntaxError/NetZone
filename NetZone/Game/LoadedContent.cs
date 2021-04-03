using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NetZone
{
	class LoadedContent
	{
		public static Texture2D Pixel;

		public static Texture2D GlyphAtlas;

		public static Dictionary<int, SpriteFont> Fonts;

		public static void Load(ContentManager contentManager)
		{
			Pixel = contentManager.Load<Texture2D>("Pixel");

			GlyphAtlas = contentManager.Load<Texture2D>("GlyphAtlasTransparent");

			Fonts = new Dictionary<int, SpriteFont>()
			{
				{ 0, contentManager.Load<SpriteFont>("BIOSFont") }
			};
		}
	}
}
