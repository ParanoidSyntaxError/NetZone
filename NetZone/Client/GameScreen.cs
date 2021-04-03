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
	class GameScreen
	{
		public PlayerPool PlayerPool;

		public EnemyPool EnemyPool;

		public ProjectilePool ProjectilePool;

		public ItemPool GroundItems;

		public ChatBox ChatBox;

		public InventoryUI InventoryUI;

		bool canPickupItem;

		bool endInitializeCalled;

		public GameScreen()
		{
			PlayerPool = new PlayerPool(10);

			ProjectilePool = new ProjectilePool(10);

			GroundItems = new ItemPool(10);

			EnemyPool = new EnemyPool(10, PlayerPool, ProjectilePool);

			ChatBox = new ChatBox();
		}

		public void EndInitialize() //Called after getting ALL data initalization packets from server
		{
			InventoryUI = new InventoryUI(PlayerPool.GetPlayer(Client.GetID()).Inventory);

			endInitializeCalled = true;
		}

		public void Update(GameTime gameTime)
		{
			if(ChatBox.Focused == false)
			{
				Movement();

				Attack();

				ItemHighlighting();

				ItemPickup();
			}

			ChatBox.Update();

			PlayerPool.Update(gameTime);

			ProjectilePool.Update(gameTime);

			GroundItems.Update();

			Main.MainCamera.FollowPosition(PlayerPool.GetPlayer(Client.GetID()).Position);

			if(endInitializeCalled == true) //Not sure if bad
			{
				InventoryUI.Update();
			}
		}

		void Movement()
		{
			Vector2 direction = Vector2.Zero;

			if (KeyBindings.KeyPressing(KeyType.MoveUp))
			{
				direction.Y += -1;
			}
			if (KeyBindings.KeyPressing(KeyType.MoveDown))
			{
				direction.Y += 1;
			}
			if (KeyBindings.KeyPressing(KeyType.MoveLeft))
			{
				direction.X += -1;
			}
			if (KeyBindings.KeyPressing(KeyType.MoveRight))
			{
				direction.X += 1;
			}

			if (direction != Vector2.Zero)
			{
				PacketManager.PlayerMoveSend((int)direction.X, (int)direction.Y);
			}
		}

		void Attack()
		{
			if (KeyBindings.KeyPressing(KeyType.Attack))
			{
				Vector2 mouseDirection = Main.MainCamera.GetMousePosition().ToVector2() - PlayerPool.GetPlayer(Client.GetID()).Position.ToVector2();
				mouseDirection.Normalize();

				PacketManager.PlayerAttackSend(mouseDirection);
			}
		}

		void ItemHighlighting()
		{
			for(int i = 0; i < GroundItems.Length(); i++)
			{
				if(GroundItems.GetItem(i).Collider.Contains(Main.MainCamera.GetMousePosition()))
				{
					GroundItems.GetItem(i).Highlighted = true;
				}
				else
				{
					GroundItems.GetItem(i).Highlighted = false;
				}
			}
		}

		void ItemPickup()
		{
			if(Mouse.GetState().LeftButton == ButtonState.Pressed)
			{
				if(canPickupItem == true)
				{
					for (int i = 0; i < GroundItems.Length(); i++)
					{
						if(GroundItems.GetActive(i) == true)
						{
							if (GroundItems.GetItem(i).Collider.Contains(Main.MainCamera.GetMousePosition()))
							{
								PacketManager.ItemPickupSend(i);
							}
						}
					}
				}

				canPickupItem = false;
			}
			else
			{
				canPickupItem = true;
			}
		}

		public void PlayerInventoryAdd(int playerID, int itemIndex, int inventoryIndex)
		{
			PlayerPool.GetPlayer(playerID).Inventory.SetItem(inventoryIndex, GroundItems.GetItem(itemIndex));

			GroundItems.SetActive(itemIndex, false);
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			PlayerPool.Draw(spriteBatch);

			ProjectilePool.Draw(spriteBatch);

			GroundItems.Draw(spriteBatch);

			EnemyPool.Draw(spriteBatch);
		}

		public void DrawUI(SpriteBatch spriteBatch)
		{
			ChatBox.Draw(spriteBatch);

			if(endInitializeCalled == true)
			{
				InventoryUI.Draw(spriteBatch);
			}
		}
	}
}
