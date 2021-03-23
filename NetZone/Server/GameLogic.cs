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

		public ProjectilePool ProjectilePool;

		public ItemPool GroundItems;

		public List<ConsoleLine> ConsoleLines;

		public GameLogic()
		{
			ConsoleLines = new List<ConsoleLine>();

			PlayerPool = new PlayerPool(10);

			ProjectilePool = new ProjectilePool(10);

			GroundItems = new ItemPool(10);
		}

		public void Update(GameTime gameTime)
		{
			PlayerPool.Update(gameTime);

			ProjectilePool.Update(gameTime);

			GroundItems.Update();
		}

		public void ItemSpawn(Item item)
		{
			int unusedIndex = GroundItems.GetUnusedIndex();

			GroundItems.SetItem(unusedIndex, item);

			PacketManager.ItemSpawnedSend(unusedIndex, item);
		}

		public void PlayerItemPickup(int playerID, int itemIndex)
		{
			if(JohnsHelper.PointDistance(PlayerPool.Players[playerID].Position, GroundItems.Items[itemIndex].Position) <= 500 &&
				PlayerPool.Players[playerID].Inventory.IsFull() == false)
			{
				int inventoryIndex = PlayerPool.Players[playerID].Inventory.GetUnusedIndex();

				PacketManager.PlayerInventoryAddSend(playerID, itemIndex, inventoryIndex);
			}
		}

		public void PlayerSpawn()
		{
			int unusedIndex = PlayerPool.GetUnusedIndex();

			PlayerPool.SetPlayer(unusedIndex, new Player());

			PacketManager.PlayerSpawnedSend(unusedIndex);
		}

		public void PlayerAttack(int id, Vector2 direction)
		{
			if(PlayerPool.Players[id].CanAttack() == true)
			{
				int unusedIndex = ProjectilePool.GetUnusedIndex();

				Projectile projectile = PlayerPool.Players[id].AttackProjectile();
				projectile.Direction = direction;
				projectile.Position = PlayerPool.Players[id].Position;

				ProjectilePool.SetProjectile(unusedIndex, projectile);

				PacketManager.ProjectileSpawnedSend(unusedIndex, projectile);

				ItemSpawn(new Item()
				{
					Art = "A",
					Color = Color.Gold,
					Position = new Point(200, 200)
				});
			}
		}

		public void PlayerMove(int id, Point direction)
		{
			PlayerPool.Players[id].Position += (PlayerPool.Players[id].Speed * direction.ToVector2()).ToPoint();
		}
	}
}
