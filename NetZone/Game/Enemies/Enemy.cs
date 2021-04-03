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
	public enum EnemyType
	{
		CyberFrog,
		WanaWorm,
		VaccineNanoDrone
	}

	class Enemy
	{
		public string Art;

		public Point Position;

		public int Speed;
		public float FrameAdjustedSpeed;

		public int MaxHealth;
		public int Health;

		public float AttackRate;
		public float AttackTimer;

		public Rectangle Collider;

		public Color Color;

		public int TargetedPlayerIndex;

		public PlayerPool PlayerPool;
		public ProjectilePool ProjectilePool;

		public EnemyType Type;

		float targetRate = 1;
		float targetTimer;

		int agroRange = 500;

		public virtual void Initalize()
		{
			Collider = new Rectangle(Position, new Point(16, 32));
		}

		public virtual void LateUpdate(GameTime gameTime)
		{
			Collider.Location = Position;
		}

		public virtual void Update(GameTime gameTime)
		{
			AttackTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

			FrameAdjustedSpeed = Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

			targetTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

			if (targetTimer >= targetRate)
			{
				if(CanTargetPlayer() == true)
				{
					TargetClosestPlayer();
					targetTimer = 0;
				}
			}
		}

		public bool CanAttack()
		{
			if (AttackTimer >= AttackRate)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public void TakeDamage(int damage)
		{
			Health -= damage;
		}

		public Vector2 DirectionToward(Point target)
		{
			Vector2 direction = (target - Position).ToVector2();
			direction.Normalize();

			return direction;
		}

		public bool CanTargetPlayer()
		{
			for (int i = 0; i < PlayerPool.Length(); i++)
			{
				if (PlayerPool.GetActive(i) == true)
				{
					int distance = JohnsHelper.PointDistance(PlayerPool.GetPlayer(i).Position, Position);

					if(distance <= agroRange)
					{
						return true;
					}
				}
			}

			return false;
		}

		public void TargetClosestPlayer()
		{
			int closestIndex = 0;
			int closestDistance = agroRange;

			for(int i = 0; i < PlayerPool.Length(); i++)
			{
				if(PlayerPool.GetActive(i) == true)
				{
					int distance = JohnsHelper.PointDistance(PlayerPool.GetPlayer(i).Position, Position);

					if (distance <= closestDistance)
					{
						closestDistance = distance;
						closestIndex = i;
					}
				}
			}

			TargetedPlayerIndex = closestIndex;
		}

		public virtual void Draw(SpriteBatch spriteBatch)
		{
			GlyphHelper.DrawGlyph(spriteBatch, Art, Position, 2, Color);
		}
	}
}
