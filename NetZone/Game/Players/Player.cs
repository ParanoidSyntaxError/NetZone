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

		public int Speed;

		public float AttackTimer;

		public ItemPool Inventory;

		public Weapon EquippedWeapon;

		public Player()
		{
			Position = new Point(100, 100);

			Speed = 4;

			AttackTimer = 0;

			Inventory = new ItemPool(50);
			EquippedWeapon = UniqueWeapon.PhasedPlasmaRifle();
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
			return EquippedWeapon.GetProjectile();
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			GlyphHelper.DrawGlyph(spriteBatch, "@", Position, 2, Color.White);
		}
	}
}
