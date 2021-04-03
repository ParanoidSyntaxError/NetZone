using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace NetZone
{
	public enum GameState
	{
		Menu,
		Server,
		Client,
		Offline //Run a Server and Client at the same DAMN time [Future project]
	}

    public class Main : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

		Settings settings;

		static GameState state;

		static Menu menu;

		static Server server;
		static Client client;

		public static MainCamera MainCamera;
		public static UICamera UICamera;

		public static bool GlobalFlash;
		Timer flashTimer;

		public static bool CallEndProgram;

		public static Random Random;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

			graphics.PreferredBackBufferHeight = 1080;
			graphics.PreferredBackBufferWidth = 1920;
			IsMouseVisible = true;

			graphics.ApplyChanges();
		}

		protected override void Initialize()
        {
			SetState(GameState.Menu);

			Random = new Random();

			settings = new Settings(graphics);

			MainCamera = new MainCamera(GraphicsDevice.Viewport);
			MainCamera.Center();

			UICamera = new UICamera(GraphicsDevice.Viewport);
			UICamera.Center();

			PacketManager.Initialize();

			flashTimer = new Timer(250);
			flashTimer.Elapsed += FlashEvent;
			flashTimer.Start();

            base.Initialize();
        }

		void EndProgram()
		{
			//End threads
			//Other safe shutdown calls

			Exit();
		}

		public static void SetState(GameState value)
		{
			state = value;

			switch(state)
			{
				case GameState.Menu:
					if(menu == null)
					{
						menu = new Menu();
					}
					break;
				case GameState.Client:
					client = new Client();
					break;
				case GameState.Server:
					server = new Server();
					break;
			}
		}

		static void FlashEvent(object source, ElapsedEventArgs args)
		{
			GlobalFlash = !GlobalFlash;
		}

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

			LoadedContent.Load(Content);
        }

        protected override void Update(GameTime gameTime)
        {
			if (Keyboard.GetState().IsKeyDown(Keys.Escape) || CallEndProgram == true)
			{
				EndProgram();
			}

			switch (state)
			{
				case GameState.Menu:
					menu.Update(gameTime);
					break;
				case GameState.Server:
					server.Update(gameTime);
					break;
				case GameState.Client:
					client.Update(gameTime);
					break;
			}

			settings.Update();

			ThreadManager.Update();

			base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

			#region Main camera

			spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, MainCamera.Transform);

			switch (state)
			{
				case GameState.Menu:
					menu.Draw(spriteBatch);
					break;
				case GameState.Server:
					server.Draw(spriteBatch);
					break;
				case GameState.Client:
					client.Draw(spriteBatch);
					break;
			}

			spriteBatch.End();
			#endregion

			#region UI camera

			spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, UICamera.Transform);

			switch (state)
			{
				case GameState.Menu:
					menu.DrawUI(spriteBatch);
					break;
				case GameState.Server:
					break;
				case GameState.Client:
					client.DrawUI(spriteBatch);
					break;
			}

			spriteBatch.DrawString(LoadedContent.Fonts[0], state.ToString(), Vector2.Zero, Color.Cyan);

			spriteBatch.End();
			#endregion

			base.Draw(gameTime);
        }
	}
}
