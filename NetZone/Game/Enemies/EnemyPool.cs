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
	struct EnemyPoolObject
	{
		public Enemy Enemy;
		public bool Active;
	}

	class EnemyPool
	{
		EnemyPoolObject[] pool;

		PlayerPool playerPool;
		ProjectilePool projectilePool;

		public EnemyPool(int size, PlayerPool players, ProjectilePool projectiles)
		{
			playerPool = players;
			projectilePool = projectiles;

			pool = new EnemyPoolObject[size];

			for (int i = 0; i < pool.Length; i++)
			{
				pool[i].Enemy = new Enemy();
				pool[i].Active = false;
			}
		}

		public void Update(GameTime gameTime)
		{
			for (int i = 0; i < pool.Length; i++)
			{
				if (pool[i].Active == true)
				{
					pool[i].Enemy.Update(gameTime);
				}
			}
		}

		public int Length()
		{
			return pool.Length;
		}

		public bool GetActive(int index)
		{
			return pool[index].Active;
		}

		public void SetActive(int index, bool value)
		{
			pool[index].Active = value;
		}

		public Enemy GetEnemy(int index)
		{
			return pool[index].Enemy;
		}

		public void SetEnemy(int index, EnemyType type, Point position)
		{
			pool[index].Enemy = ParseEnemyType(type);

			pool[index].Enemy.Position = position;
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

		public void SetUnusedEnemy(EnemyType type, Point position)
		{
			for (int i = 0; i < pool.Length; i++)
			{
				if (pool[i].Active == false)
				{
					pool[i].Enemy = ParseEnemyType(type);

					pool[i].Enemy.Position = position;
					pool[i].Active = true;
					break;
				}
			}
		}

		Enemy ParseEnemyType(EnemyType type)
		{
			switch (type)
			{
				case EnemyType.CyberFrog:
					return new CyberFrog(playerPool, projectilePool);
				case EnemyType.WanaWorm:
					return new WanaWorm(playerPool, projectilePool);
				case EnemyType.VaccineNanoDrone:
					return new VaccineNanoDrone(playerPool, projectilePool);
			}

			return null;
		}

		public bool IsFull()
		{
			for (int i = 0; i < pool.Length; i++)
			{
				if (pool[i].Active == false)
				{
					return false;
				}
			}

			return true;
		}

		public Point[] GetAllPositions()
		{
			Point[] positions = new Point[pool.Length];

			for (int i = 0; i < pool.Length; i++)
			{
				positions[i] = pool[i].Enemy.Position;
			}

			return positions;
		}

		public void SetAllPositions(Point[] positions)
		{
			for (int i = 0; i < pool.Length; i++)
			{
				pool[i].Enemy.Position = positions[i];
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			for (int i = 0; i < pool.Length; i++)
			{
				if (pool[i].Active == true)
				{
					pool[i].Enemy.Draw(spriteBatch);
				}
			}
		}
	}
}
