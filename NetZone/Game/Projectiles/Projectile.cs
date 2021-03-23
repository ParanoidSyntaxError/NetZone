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
	public enum ProjectileType
	{
		Line,
		Sine
	}

	class Projectile
	{
		public Point Position;

		public Vector2 Direction;

		public int Damage;

		public float Speed;

		public float Range;

		public ProjectileType Type;

		public float Frequency;
		public float Amplitude;

		float rotation;

		float frameAdjustedSpeed;

		float timer;

		public Color Color;

		public bool Active;

		public Projectile()
		{

		}

		public Projectile(int damage, float speed, float range, Color color)
		{
			Damage = damage;

			Speed = speed;

			Range = range;

			Type = ProjectileType.Line;

			Color = color;
		}

		public Projectile(int damage, float speed, float range, float frequency, float amplitude, Color color)
		{
			Damage = damage;

			Speed = speed;

			Range = range;

			Frequency = frequency;

			Amplitude = amplitude;

			Type = ProjectileType.Sine;

			Color = color;
		}

		public void Update(GameTime gameTime)
		{
			timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

			frameAdjustedSpeed = Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

			rotation += frameAdjustedSpeed * 0.07f;

			switch (Type)
			{
				case ProjectileType.Line:
					Position += (Direction * frameAdjustedSpeed).ToPoint();
					break;
				case ProjectileType.Sine:
					Vector2 up = new Vector2(-Direction.Y, Direction.X);

					float upSpeed = (float)Math.Cos(timer * Frequency) * Amplitude * Frequency;

					Position += (up * upSpeed + Direction * frameAdjustedSpeed).ToPoint();
					break;
			}

			if (timer >= Range)
			{
				timer = 0;

				Active = false;
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.DrawString(LoadedContent.Fonts[0], "*", Position.ToVector2() + new Vector2(5, 5), Color, rotation, new Vector2(6, 6), 1, SpriteEffects.None, 0);
		}
	}
}
