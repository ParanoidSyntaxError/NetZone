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
	class Menu : Screen
	{
		string title;

		string titleShadow;
		string titleFill;

		Button hostButton;
		Button joinButton;
		Button exitButton;

		public Menu()
		{
			title =	"███╗   ██╗███████╗████████╗    ███████╗ ██████╗ ███╗   ██╗███████╗" + "\n" +
					"████╗  ██║██╔════╝╚══██╔══╝    ╚══███╔╝██╔═══██╗████╗  ██║██╔════╝" + "\n" +
					"██╔██╗ ██║█████╗     ██║         ███╔╝ ██║   ██║██╔██╗ ██║█████╗" + "\n" +
					"██║╚██╗██║██╔══╝     ██║        ███╔╝  ██║   ██║██║╚██╗██║██╔══╝" + "\n" +
					"██║ ╚████║███████╗   ██║       ███████╗╚██████╔╝██║ ╚████║███████╗" + "\n" +
					"╚═╝  ╚═══╝╚══════╝   ╚═╝       ╚══════╝ ╚═════╝ ╚═╝  ╚═══╝╚══════╝";

			titleShadow = title.Replace('█', ' ');

			titleFill = title;
			titleFill = titleFill.Replace('╗', ' ');
			titleFill = titleFill.Replace('╝', ' ');
			titleFill = titleFill.Replace('╔', ' ');
			titleFill = titleFill.Replace('╚', ' ');
			titleFill = titleFill.Replace('═', ' ');
			titleFill = titleFill.Replace('║', ' ');

			hostButton = new Button(new Point(800, 600), Color.Gray, Color.White, "H O S T");

			joinButton = new Button(new Point(800, 700), Color.Gray, Color.White, "J O I N");

			exitButton = new Button(new Point(800, 800), Color.Gray, Color.White, "E X I T");
		}

		public override void Update(GameTime gameTime)
		{
			if (hostButton.OnClick() == true)
			{
				Main.SetState(GameState.Server);
			}

			if (joinButton.OnClick() == true)
			{
				Main.SetState(GameState.Client);
			}

			if (exitButton.OnClick() == true)
			{
				Main.CallEndProgram = true;
			}
		}

		public override void DrawUI(SpriteBatch spriteBatch)
		{
			GlyphHelper.DrawGlyph(spriteBatch, titleShadow, new Point(450, 200), 2, Color.SlateGray);
			GlyphHelper.DrawGlyph(spriteBatch, titleFill, new Point(450, 200), 2, Color.Cyan);

			hostButton.Draw(spriteBatch);

			joinButton.Draw(spriteBatch);

			exitButton.Draw(spriteBatch);
		}
	}
}
