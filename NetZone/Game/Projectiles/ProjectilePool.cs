using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace NetZone
{
	class ProjectilePool
	{
		Projectile[] projectiles;

		public ProjectilePool(int size)
		{
			projectiles = new Projectile[size];

			for (int i = 0; i < projectiles.Length; i++)
			{
				projectiles[i] = new Projectile();
				projectiles[i].Active = false;
			}
		}

		public void Update(GameTime gameTime)
		{
			for (int i = 0; i < projectiles.Length; i++)
			{
				if (projectiles[i].Active == true)
				{
					projectiles[i].Update(gameTime);
				}
			}
		}

		public void SetProjectile(int index, Projectile projectile)
		{
			projectiles[index] = projectile;
			projectiles[index].Active = true;
		}

		public void SetUnusedProjectile(Projectile projectile)
		{
			for (int i = 0; i < projectiles.Length; i++)
			{
				if (projectiles[i].Active == false)
				{
					projectiles[i] = projectile;
					projectiles[i].Active = true;

					break;
				}
			}
		}

		public int GetUnusedIndex()
		{
			for (int i = 0; i < projectiles.Length; i++)
			{
				if (projectiles[i].Active == false)
				{
					return i;
				}
			}

			return -1; //Pool overflow !!!
		}

		public void SetPositions(Point[] positions)
		{
			for (int i = 0; i < projectiles.Length; i++)
			{
				projectiles[i].Position = positions[i];
			}
		}

		public Point[] GetAllPositions()
		{
			Point[] positions = new Point[projectiles.Length];

			for (int i = 0; i < projectiles.Length; i++)
			{
				positions[i] = projectiles[i].Position;
			}

			return positions;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			for (int i = 0; i < projectiles.Length; i++)
			{
				if (projectiles[i].Active == true)
				{
					projectiles[i].Draw(spriteBatch);
				}
			}
		}
	}
}