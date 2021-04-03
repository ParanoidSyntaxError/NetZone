using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace NetZone
{
	class VaccineNanoDrone : Enemy
	{
		int swoopCounter;

		Point targetOffset;

		Vector2 direction;

		public VaccineNanoDrone(PlayerPool playerPool, ProjectilePool projectilePool)
		{
			Type = EnemyType.VaccineNanoDrone;

			Art = "V";
			Color = Color.Cyan;

			AttackRate = 0.75f;

			MaxHealth = 2;

			Speed = 400;

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
			if (JohnsHelper.PointDistance(PlayerPool.GetPlayer(TargetedPlayerIndex).Position, Position) <= 50)
			{
				direction = -DirectionToward(PlayerPool.GetPlayer(TargetedPlayerIndex).Position);

				swoopCounter++;
			}

			Vector2 offsetDir = (PlayerPool.GetPlayer(TargetedPlayerIndex).Position + targetOffset).ToVector2() - Position.ToVector2();

			offsetDir.Normalize();

			direction = Vector2.Lerp(direction, offsetDir, 0.01f * FrameAdjustedSpeed);

			Position += (direction * FrameAdjustedSpeed).ToPoint();

			if (JohnsHelper.PointDistance(Position, PlayerPool.GetPlayer(TargetedPlayerIndex).Position + targetOffset) <= 5 || swoopCounter >= 1)
			{
				NewTargetOffset();
				swoopCounter = 0;
			}
		}

		void NewTargetOffset()
		{
			targetOffset = new Point(Main.Random.Next(-150, 151), Main.Random.Next(-150, 151));
		}
	}
}
