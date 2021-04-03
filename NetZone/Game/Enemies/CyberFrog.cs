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
	class CyberFrog : Enemy
	{
		float moveRate = 1.75f;
		float moveTimer;
		Vector2 moveDir;

		public CyberFrog(PlayerPool playerPool, ProjectilePool projectilePool)
		{
			Type = EnemyType.CyberFrog;

			Art = "F";
			Color = Color.LawnGreen;

			AttackRate = 0.75f;

			MaxHealth = 5;

			Speed = 250;

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
			moveTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

			if (moveTimer <= 0.5f)
			{
				Position += (moveDir * FrameAdjustedSpeed).ToPoint();
			}

			if (moveTimer >= moveRate)
			{
				moveDir = DirectionToward(PlayerPool.GetPlayer(TargetedPlayerIndex).Position);

				moveTimer = 0;
			}
		}
	}
}
