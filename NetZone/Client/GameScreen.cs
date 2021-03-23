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

		public ProjectilePool ProjectilePool;

		public ItemPool GroundItems;

		public ChatBox ChatBox;

		public InventoryUI InventoryUI;

		bool canPickupItem;

		public GameScreen()
		{
			PlayerPool = new PlayerPool(10);

			ProjectilePool = new ProjectilePool(10);

			GroundItems = new ItemPool(10);

			ChatBox = new ChatBox();

			InventoryUI = new InventoryUI(PlayerPool.Players[Client.GetID()].Inventory);
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

			InventoryUI.Update();

			PlayerPool.Update(gameTime);

			ProjectilePool.Update(gameTime);

			GroundItems.Update();

			Main.MainCamera.FollowPosition(PlayerPool.Players[Client.GetID()].Position);
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
				Vector2 mouseDirection = Main.MainCamera.GetMousePosition().ToVector2() - PlayerPool.Players[Client.GetID()].Position.ToVector2();
				mouseDirection.Normalize();

				PacketManager.PlayerAttackSend(mouseDirection);
			}
		}

		void ItemHighlighting()
		{
			for(int i = 0; i < GroundItems.Items.Length; i++)
			{
				if(GroundItems.Items[i].Collider.Contains(Main.MainCamera.GetMousePosition()))
				{
					GroundItems.Items[i].Highlighted = true;
				}
				else
				{
					GroundItems.Items[i].Highlighted = false;
				}
			}
		}

		void ItemPickup()
		{
			if(Mouse.GetState().LeftButton == ButtonState.Pressed)
			{
				if(canPickupItem == true)
				{
					for (int i = 0; i < GroundItems.Items.Length; i++)
					{
						if (GroundItems.Items[i].Collider.Contains(Main.MainCamera.GetMousePosition()))
						{
							PacketManager.ItemPickupSend(i);
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

		public void Draw(SpriteBatch spriteBatch)
		{
			PlayerPool.Draw(spriteBatch);

			ProjectilePool.Draw(spriteBatch);

			GroundItems.Draw(spriteBatch);
		}

		public void DrawUI(SpriteBatch spriteBatch)
		{
			ChatBox.Draw(spriteBatch);

			InventoryUI.Draw(spriteBatch);
		}
	}
}
