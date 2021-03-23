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
	class Player
	{
		public Point Position;

		public float Speed;

		public float AttackTimer;

		public bool Active;

		public ItemPool Inventory;

		Weapon equippedWeapon;

		public Player()
		{
			Position = new Point(100, 100);

			Speed = 4;

			AttackTimer = 0;

			Inventory = new ItemPool(10);
			equippedWeapon = UniqueWeapons.PhasedPlasmaRifle();
		}

		public void Update(GameTime gameTime)
		{
			AttackTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
		}

		public bool CanAttack()
		{
			if (AttackTimer >= 0.25f)
			{
				AttackTimer = 0;

				return true;
			}

			return false;
		}

		public Projectile AttackProjectile()
		{
			return equippedWeapon.GetProjectile();
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.DrawString(LoadedContent.Fonts[0], "@", Position.ToVector2(), Color.White);
		}
	}
}
