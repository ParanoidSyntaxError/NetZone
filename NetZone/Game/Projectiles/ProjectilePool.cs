using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace NetZone
{
	struct ProjectilePoolObject
	{
		public Projectile Projectile;
		public bool Active;
	}

	class ProjectilePool
	{
		ProjectilePoolObject[] pool;

		public ProjectilePool(int size)
		{
			pool = new ProjectilePoolObject[size];

			for (int i = 0; i < pool.Length; i++)
			{
				pool[i].Projectile = new Projectile();
				pool[i].Active = false;
			}
		}

		public void Update(GameTime gameTime)
		{
			for (int i = 0; i < pool.Length; i++)
			{
				if (pool[i].Active == true)
				{
					pool[i].Projectile.Update(gameTime);

					if(pool[i].Projectile.Deactivate == true)
					{
						pool[i].Projectile.Deactivate = false;
						pool[i].Active = false;
					}
				}
			}
		}

		public int Length()
		{
			return pool.Length;
		}

		public Projectile GetProjectile(int index)
		{
			return pool[index].Projectile;
		}

		public void SetProjectile(int index, Projectile projectile)
		{
			pool[index].Projectile = projectile;
			pool[index].Active = true;
		}

		public int GetUnusedIndex()
		{
			for (int i = 0; i < pool.Length; i++)
			{
				if (pool[i].Active == false)
				{
					return i;
				}
			}

			return 0; //Pool overflow !!!
		}

		public void SetUnusedProjectile(Projectile projectile)
		{
			for (int i = 0; i < pool.Length; i++)
			{
				if (pool[i].Active == false)
				{
					pool[i].Projectile = projectile;
					pool[i].Active = true;
					break;
				}
			}
		}

		public Point[] GetAllPositions()
		{
			Point[] positions = new Point[pool.Length];

			for (int i = 0; i < pool.Length; i++)
			{
				positions[i] = pool[i].Projectile.Position;
			}

			return positions;
		}

		public void SetPositions(Point[] positions)
		{
			for (int i = 0; i < pool.Length; i++)
			{
				pool[i].Projectile.Position = positions[i];
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			for (int i = 0; i < pool.Length; i++)
			{
				if (pool[i].Active == true)
				{
					pool[i].Projectile.Draw(spriteBatch);
				}
			}
		}
	}
}