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
	class WanaWorm : Enemy
	{
		float frequency = 10;
		float amplitude = 0.5f;

		float timer;

		public WanaWorm(PlayerPool playerPool, ProjectilePool projectilePool)
		{
			Type = EnemyType.WanaWorm;

			Art = "W";
			Color = Color.Yellow;

			AttackRate = 1f;

			MaxHealth = 3;

			Speed = 200;

			PlayerPool = playerPool;
			ProjectilePool = projectilePool;
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			if (CanTargetPlayer() == true)
			{
				Movement(gameTime);
			}

			LateUpdate(gameTime);
		}

		void Movement(GameTime gameTime)
		{
			timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

			if (JohnsHelper.PointDistance(PlayerPool.GetPlayer(TargetedPlayerIndex).Position, Position) <= 500)
			{
				if (JohnsHelper.PointDistance(PlayerPool.GetPlayer(TargetedPlayerIndex).Position, Position) >= 30)
				{
					Vector2 direction = DirectionToward(PlayerPool.GetPlayer(TargetedPlayerIndex).Position);

					Vector2 _up = new Vector2(-direction.Y, direction.X);

					float _upSpeed = (float)Math.Cos(timer * frequency) * amplitude * frequency;

					Position += (_up * _upSpeed + direction * FrameAdjustedSpeed).ToPoint();
				}
			}
		}
	}
}
