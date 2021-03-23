using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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

		public GameState State;

		Server server;
		Client client;

		public static MainCamera MainCamera;
		public static UICamera UICamera;

		public static bool GlobalFlash;
		Timer flashTimer;

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
			settings = new Settings(graphics);

			MainCamera = new MainCamera(GraphicsDevice.Viewport);
			MainCamera.Center();

			UICamera = new UICamera(GraphicsDevice.Viewport);
			UICamera.Center();

			PacketManager.Initialize();

			flashTimer = new Timer(250);
			flashTimer.Elapsed += FlashEvent;
			flashTimer.Start();

			State = GameState.Menu;

            base.Initialize();
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
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
			{
				Exit();
			}

			switch(State)
			{
				case GameState.Menu:
					if(Keyboard.GetState().IsKeyDown(Keys.D1))
					{
						State = GameState.Server;

						server = new Server();
					}
					if (Keyboard.GetState().IsKeyDown(Keys.D2))
					{
						State = GameState.Client;

						client = new Client();
					}
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

			switch (State)
			{
				case GameState.Menu:
					spriteBatch.DrawString(LoadedContent.Fonts[0], "1) Server 2) Client", new Vector2(100, 900), Color.Yellow);
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

			switch (State)
			{
				case GameState.Menu:
					break;
				case GameState.Server:
					break;
				case GameState.Client:
					client.DrawUI(spriteBatch);
					break;
			}

			spriteBatch.DrawString(LoadedContent.Fonts[0], State.ToString(), Vector2.Zero, Color.Cyan);

			spriteBatch.End();
			#endregion

			base.Draw(gameTime);
        }
	}
}
