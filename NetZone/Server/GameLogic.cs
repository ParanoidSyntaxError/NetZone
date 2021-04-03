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
	class GameLogic
	{
		public PlayerPool PlayerPool;

		public EnemyPool EnemyPool;

		public ProjectilePool ProjectilePool;

		public ItemPool GroundItems;

		public GameLogic()
		{
			PlayerPool = new PlayerPool(10);

			ProjectilePool = new ProjectilePool(10);

			GroundItems = new ItemPool(10);

			EnemyPool = new EnemyPool(10, PlayerPool, ProjectilePool);

			ItemSpawn(UniqueArmor.AmuletOfAtuLoss());

			EnemySpawn(EnemyType.CyberFrog, new Point(200, 200));
		}

		public void Update(GameTime gameTime)
		{
			PlayerPool.Update(gameTime);

			EnemyPool.Update(gameTime);

			PacketManager.EnemyPositionsSend(EnemyPool.GetAllPositions());

			ProjectilePool.Update(gameTime);

			GroundItems.Update();
		}

		public void EnemySpawn(EnemyType type, Point position)
		{
			int unusedIndex = EnemyPool.GetUnusedIndex();

			EnemyPool.SetEnemy(unusedIndex, type, position);

			PacketManager.EnemySpawnedSend(unusedIndex, type, position);
		}

		public void ItemSpawn(Item item)
		{
			int unusedIndex = GroundItems.GetUnusedIndex();

			GroundItems.SetItem(unusedIndex, item);

			PacketManager.ItemSpawnedSend(unusedIndex, item);
		}

		public void PlayerItemPickup(int playerID, int itemIndex)
		{
			if (GroundItems.GetActive(itemIndex) == true &&
				PlayerPool.GetPlayer(playerID).Inventory.IsFull() == false &&
				JohnsHelper.PointDistance(PlayerPool.GetPlayer(playerID).Position, GroundItems.GetItem(itemIndex).Position) <= 500)
			{
				int inventoryIndex = PlayerPool.GetPlayer(playerID).Inventory.GetUnusedIndex();

				PlayerPool.GetPlayer(playerID).Inventory.SetItem(inventoryIndex, GroundItems.GetItem(itemIndex));

				GroundItems.SetActive(itemIndex, false);

				PacketManager.SetItemActiveSend(itemIndex, false);

				PacketManager.PlayerInventoryAddSend(playerID, itemIndex, inventoryIndex);
			}
		}

		public void PlayerSpawn(int index)
		{
			PlayerPool.SetPlayer(index, new Player());

			PacketManager.PlayerSpawnedSend(index);
		}

		public void PlayerAttack(int id, Vector2 direction)
		{
			if(PlayerPool.GetPlayer(id).CanAttack() == true)
			{
				int unusedIndex = ProjectilePool.GetUnusedIndex();

				Projectile projectile = PlayerPool.GetPlayer(id).AttackProjectile();
				projectile.Direction = direction;
				projectile.Position = PlayerPool.GetPlayer(id).Position;

				ProjectilePool.SetProjectile(unusedIndex, projectile);

				PacketManager.ProjectileSpawnedSend(unusedIndex, projectile);
			}
		}

		public void PlayerMove(int id, Point direction)
		{
			PlayerPool.GetPlayer(id).Position += (PlayerPool.GetPlayer(id).Speed * direction.ToVector2()).ToPoint();

			PacketManager.PlayerPositionsSend(PlayerPool.GetAllPositions());
		}
	}
}
