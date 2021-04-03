using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace NetZone
{
	public enum SentPacket
	{
		MovementInput,
		AttackInput,
		PlayerPositions,
		EnemyPositions,
		ProjectilePositions,
		ProjectileSpawned,
		ClientChat,
		ServerChat,
		ItemPickup,
		InitalServerConnection,
		ItemSpawned,
		PlayerSpawned,
		EnemySpawned,
		PlayerInventoryAdd,
		InitializePlayers,
		InitializeEnemies,
		InitializeItems,
		EndInitialize,
		SetActiveItem
	}

	class PacketManager
	{
		public delegate void PacketHandler(int listenerID, Packet packet);
		public static Dictionary<SentPacket, PacketHandler> PacketHandlers;

		public static void Initialize()
		{
			PacketHandlers = new Dictionary<SentPacket, PacketHandler>()
			{
				{ SentPacket.MovementInput, PlayerMoveReceived },
				{ SentPacket.AttackInput, PlayerAttackReceived },
				{ SentPacket.ItemPickup, ItemPickupReceived },
				{ SentPacket.PlayerPositions, PlayerPositionsReceived },
				{ SentPacket.EnemyPositions, EnemyPositionsReceived },
				{ SentPacket.ProjectilePositions, ProjectilePositionsReceived },
				{ SentPacket.ProjectileSpawned, ProjectileSpawnedReceived },
				{ SentPacket.ClientChat, ClientChatReceived },
				{ SentPacket.ServerChat, ServerChatReceived },
				{ SentPacket.InitalServerConnection, InitalServerConnectionReceived },
				{ SentPacket.InitializeEnemies, InitializeEnemiesReceived },
				{ SentPacket.ItemSpawned, ItemSpawnedReceived },
				{ SentPacket.PlayerSpawned, PlayerSpawnedReceived },
				{ SentPacket.EnemySpawned, EnemySpawnedReceived },
				{ SentPacket.PlayerInventoryAdd, PlayerInventoryAddReceived },
				{ SentPacket.InitializePlayers, InitializePlayersReceived },
				{ SentPacket.InitializeItems, InitializeItemsReceived },
				{ SentPacket.EndInitialize, EndInitializeReceived },
				{ SentPacket.SetActiveItem, SetItemActiveReceived }
			};
		}

		#region Server packets

		static void SendListenerTCPData(int listenerID, Packet packet)
		{
			packet.WriteLength();
			Server.Listeners[listenerID].Socket.SendData(packet);
		}

		static void SendTCPDataToAllListeners(Packet packet)
		{
			packet.WriteLength();

			for (int i = 0; i < Server.MaxPlayers; i++)
			{
				if (Server.Listeners[i].Socket.TcpClient != null)
				{
					Server.Listeners[i].Socket.SendData(packet);
				}
			}
		}
		#endregion

		#region Client packets

		static void SendServerTCPData(Packet packet)
		{
			packet.WriteLength();
			Client.Socket.SendData(packet);
		}
		#endregion

		#region Initial connection packets

		public static void InitalServerConnectionSend(int clientID) //SERVER SENDS
		{
			using (Packet packet = new Packet(SentPacket.InitalServerConnection))
			{
				packet.Write(clientID);

				SendListenerTCPData(clientID, packet);
			}

			Server.DebugConsole.NewLine("SENT: Client ID " + clientID, Color.Green);
		}

		static void InitalServerConnectionReceived(int listenerID, Packet packet) //CLIENT RECEIVES
		{
			int clientID = packet.ReadInt();

			Client.Socket.ID = clientID;
		}
		#endregion

		#region Initialize player packets

		public static void InitializePlayersSend(int clientID, PlayerPool playerPool) //SERVER SENDS
		{
			using (Packet packet = new Packet(SentPacket.InitializePlayers))
			{
				//Array length
				packet.Write(playerPool.Length());

				for (int i = 0; i < playerPool.Length(); i++)
				{
					//Active
					packet.Write(playerPool.GetActive(i));

					if (playerPool.GetActive(i) == true)
					{
						//Position
						packet.Write(playerPool.GetPlayer(i).Position.X);
						packet.Write(playerPool.GetPlayer(i).Position.Y);

						//Equipped weapon
						packet.Write(playerPool.GetPlayer(i).EquippedWeapon.Art);

						packet.Write(playerPool.GetPlayer(i).EquippedWeapon.Color.R);
						packet.Write(playerPool.GetPlayer(i).EquippedWeapon.Color.G);
						packet.Write(playerPool.GetPlayer(i).EquippedWeapon.Color.B);

						packet.Write(playerPool.GetPlayer(i).EquippedWeapon.Damage);

						packet.Write(playerPool.GetPlayer(i).EquippedWeapon.Speed);

						packet.Write(playerPool.GetPlayer(i).EquippedWeapon.Range);

						packet.Write((int)playerPool.GetPlayer(i).EquippedWeapon.ProjectileType);

						packet.Write(playerPool.GetPlayer(i).EquippedWeapon.Frequency);

						packet.Write(playerPool.GetPlayer(i).EquippedWeapon.Amplitude);
					}
				}

				SendListenerTCPData(clientID, packet);
			}

			Server.DebugConsole.NewLine("SENT: Initialize players on client ID " + clientID, Color.Pink);
		}

		static void InitializePlayersReceived(int listenerID, Packet packet) //CLIENT RECEIVES
		{
			int arrayLength = packet.ReadInt();

			PlayerPool playerPool = new PlayerPool(arrayLength);

			for (int i = 0; i < playerPool.Length(); i++)
			{
				//Active
				bool playerActive = packet.ReadBool();

				playerPool.SetActive(i, playerActive);

				if (playerPool.GetActive(i) == true)
				{
					//Position
					playerPool.GetPlayer(i).Position.X = packet.ReadInt();
					playerPool.GetPlayer(i).Position.Y = packet.ReadInt();

					//Equipped weapon
					playerPool.GetPlayer(i).EquippedWeapon.Art = packet.ReadString();

					playerPool.GetPlayer(i).EquippedWeapon.Color.R = packet.ReadByte();
					playerPool.GetPlayer(i).EquippedWeapon.Color.G = packet.ReadByte();
					playerPool.GetPlayer(i).EquippedWeapon.Color.B = packet.ReadByte();

					playerPool.GetPlayer(i).EquippedWeapon.Damage = packet.ReadInt();

					playerPool.GetPlayer(i).EquippedWeapon.Speed = packet.ReadInt();

					playerPool.GetPlayer(i).EquippedWeapon.Range = packet.ReadFloat();

					playerPool.GetPlayer(i).EquippedWeapon.ProjectileType = (ProjectileType)packet.ReadInt();

					playerPool.GetPlayer(i).EquippedWeapon.Frequency = packet.ReadFloat();

					playerPool.GetPlayer(i).EquippedWeapon.Amplitude = packet.ReadFloat();
				}
			}

			Client.GameScreen.PlayerPool = playerPool;
		}
		#endregion

		#region Initialize enemy packets

		public static void InitializeEnemiesSend(int clientID, EnemyPool enemyPool) //SERVER SENDS
		{
			using (Packet packet = new Packet(SentPacket.InitializeEnemies))
			{
				packet.Write(enemyPool.Length());

				for(int i = 0; i < enemyPool.Length(); i++)
				{
					packet.Write(enemyPool.GetActive(i));

					if(enemyPool.GetActive(i) == true)
					{
						packet.Write((int)enemyPool.GetEnemy(i).Type);

						packet.Write(enemyPool.GetEnemy(i).Position.X);
						packet.Write(enemyPool.GetEnemy(i).Position.Y);
					}
				}

				SendListenerTCPData(clientID, packet);
			}

			Server.DebugConsole.NewLine("SENT: Initialize enemies on client ID " + clientID, Color.SaddleBrown);
		}

		static void InitializeEnemiesReceived(int listenerID, Packet packet) //CLIENT RECEIVES
		{
			int arrayLength = packet.ReadInt();

			EnemyPool enemyPool = new EnemyPool(arrayLength, Client.GameScreen.PlayerPool, Client.GameScreen.ProjectilePool); //BAD NOT GOOD

			for(int i = 0; i < enemyPool.Length(); i++)
			{
				bool active = packet.ReadBool();

				enemyPool.SetActive(i, active);

				if(enemyPool.GetActive(i) == true)
				{
					EnemyType type = (EnemyType)packet.ReadInt();

					int posX = packet.ReadInt();
					int posY = packet.ReadInt();

					enemyPool.SetEnemy(i, type, new Point(posX, posY));
				}
			}

			Client.GameScreen.EnemyPool = enemyPool;
		}
		#endregion

		#region Initialize item packets

		public static void InitializeItemsSend(int clientID, ItemPool itemPool) //SERVER SENDS
		{
			using (Packet packet = new Packet(SentPacket.InitializeItems))
			{
				//Array length
				packet.Write(itemPool.Length());

				for (int i = 0; i < itemPool.Length(); i++)
				{
					//Active
					packet.Write(itemPool.GetActive(i));

					if (itemPool.GetActive(i) == true)
					{
						//Position
						packet.Write(itemPool.GetItem(i).Position.X);
						packet.Write(itemPool.GetItem(i).Position.Y);

						//Art
						packet.Write(itemPool.GetItem(i).Art);

						//Color
						packet.Write(itemPool.GetItem(i).Color.R);
						packet.Write(itemPool.GetItem(i).Color.G);
						packet.Write(itemPool.GetItem(i).Color.B);
					}
				}

				SendListenerTCPData(clientID, packet);
			}

			Server.DebugConsole.NewLine("SENT: Initialize items on client ID " + clientID, Color.Pink);
		}

		static void InitializeItemsReceived(int listenerID, Packet packet) //CLIENT RECEIVES
		{
			int arrayLength = packet.ReadInt();

			ItemPool itemPool = new ItemPool(arrayLength);

			for (int i = 0; i < itemPool.Length(); i++)
			{
				//Active
				bool itemActive = packet.ReadBool();

				itemPool.SetActive(i, itemActive);

				if (itemPool.GetActive(i) == true)
				{
					//Position
					itemPool.GetItem(i).Position.X = packet.ReadInt();
					itemPool.GetItem(i).Position.Y = packet.ReadInt();

					//Art
					itemPool.GetItem(i).Art = packet.ReadString();

					//Color
					itemPool.GetItem(i).Color.R = packet.ReadByte();
					itemPool.GetItem(i).Color.G = packet.ReadByte();
					itemPool.GetItem(i).Color.B = packet.ReadByte();
				}
			}

			Client.GameScreen.GroundItems = itemPool;
		}
		#endregion

		#region End initialize packets (signals end of this clients initialization)

		public static void EndInitializeSend(int clientID) //SERVER SENDS
		{
			using (Packet packet = new Packet(SentPacket.EndInitialize))
			{
				SendListenerTCPData(clientID, packet);
			}

			Server.DebugConsole.NewLine("SENT : End client ID " + clientID + " data initialization", Color.Green);
		}

		static void EndInitializeReceived(int listenerID, Packet packet) //CLIENT RECEIVES
		{
			Client.GameScreen.EndInitialize();
		}

		#endregion

		#region Chat packets

		public static void ClientChatSend(string message) //CLIENT SENDS
		{
			using (Packet packet = new Packet(SentPacket.ClientChat))
			{
				packet.Write(message);

				SendServerTCPData(packet);
			}
		}

		static void ClientChatReceived(int listenerID, Packet packet) //SERVER RECEIVES
		{
			string message = packet.ReadString();

			Server.DebugConsole.NewLine(message, Color.White);

			ServerChatSend(message);
		}

		public static void ServerChatSend(string message) //SERVER SENDS
		{
			using (Packet packet = new Packet(SentPacket.ServerChat))
			{
				packet.Write(message);

				SendTCPDataToAllListeners(packet);
			}

			Server.DebugConsole.NewLine("SENT : New chat message", Color.Cyan);
		}

		static void ServerChatReceived(int listenerID, Packet packet) //CLIENT RECEIVES
		{
			string message = packet.ReadString();

			Client.GameScreen.ChatBox.NewLine(message, Color.Yellow);
		}
		#endregion

		#region Player spawned packets

		public static void PlayerSpawnedSend(int index) //SERVER SENDS
		{
			using (Packet packet = new Packet(SentPacket.PlayerSpawned))
			{
				packet.Write(index);

				SendTCPDataToAllListeners(packet);
			}

			Server.DebugConsole.NewLine("SENT : Spawned player", Color.Cyan);
		}

		static void PlayerSpawnedReceived(int listenerID, Packet packet) //CLIENT RECEIVES
		{
			int index = packet.ReadInt();

			Client.GameScreen.PlayerPool.SetPlayer(index, new Player());
		}
		#endregion

		#region Enemy spawned packets

		public static void EnemySpawnedSend(int index, EnemyType type, Point position) //SERVER SENDS
		{
			using (Packet packet = new Packet(SentPacket.EnemySpawned))
			{
				packet.Write(index);

				packet.Write((int)type);

				packet.Write(position.X);
				packet.Write(position.Y);

				SendTCPDataToAllListeners(packet);
			}

			Server.DebugConsole.NewLine("SENT : Spawned enemy", Color.Pink);
		}

		static void EnemySpawnedReceived(int listenerID, Packet packet) //CLIENT RECEIVES
		{
			int index = packet.ReadInt();

			EnemyType type = (EnemyType)packet.ReadInt();

			int posX = packet.ReadInt();
			int posY = packet.ReadInt();

			Client.GameScreen.EnemyPool.SetEnemy(index, type, new Point(posX, posY));
		}
		#endregion

		#region Item spawned packets

		public static void ItemSpawnedSend(int index, Item item) //SERVER SEND
		{
			using (Packet packet = new Packet(SentPacket.ItemSpawned))
			{
				packet.Write(index);

				packet.Write(item.Position.X);
				packet.Write(item.Position.Y);

				packet.Write(item.Art);

				packet.Write(item.Color.R);
				packet.Write(item.Color.G);
				packet.Write(item.Color.B);

				SendTCPDataToAllListeners(packet);
			}

			Server.DebugConsole.NewLine("SENT : Spawned item", Color.Cyan);
		}

		static void ItemSpawnedReceived(int listenerID, Packet packet) //CLIENT RECEIVES
		{
			int index = packet.ReadInt();

			Item item = new Item();

			item.Position.X = packet.ReadInt();
			item.Position.Y = packet.ReadInt();

			item.Art = packet.ReadString();

			item.Color.R = packet.ReadByte();
			item.Color.G = packet.ReadByte();
			item.Color.B = packet.ReadByte();

			Client.GameScreen.GroundItems.SetItem(index, item);
		}
		#endregion

		#region Item pickup packets

		public static void ItemPickupSend(int index) //CLIENT SENDS
		{
			using (Packet packet = new Packet(SentPacket.ItemPickup))
			{
				packet.Write(index);

				SendServerTCPData(packet);
			}
		}

		static void ItemPickupReceived(int listenerID, Packet packet) //SERVER RECEIVES
		{
			int index = packet.ReadInt();

			Server.DebugConsole.NewLine("GOT : Try pickup", Color.Yellow);

			Server.GameLogic.PlayerItemPickup(listenerID, index);
		}
		#endregion

		#region Player add to inventory

		public static void PlayerInventoryAddSend(int playerID, int itemIndex, int inventoryIndex) //SERVER SENDS
		{
			using (Packet packet = new Packet(SentPacket.PlayerInventoryAdd))
			{
				packet.Write(itemIndex);

				packet.Write(inventoryIndex);

				SendListenerTCPData(playerID, packet);
			}

			Server.DebugConsole.NewLine("SENT : Add to players inventory", Color.Yellow);
		}

		static void PlayerInventoryAddReceived(int listenerID, Packet packet) //CLIENT RECEIVES
		{
			int itemIndex = packet.ReadInt();

			int inventoryIndex = packet.ReadInt();

			Client.GameScreen.PlayerInventoryAdd(listenerID, itemIndex, inventoryIndex);
		}
		#endregion

		#region Set item active packets

		public static void SetItemActiveSend(int itemIndex, bool value) //SERVER SENDS
		{
			using (Packet packet = new Packet(SentPacket.SetActiveItem))
			{
				packet.Write(itemIndex);

				packet.Write(value);

				SendTCPDataToAllListeners(packet);
			}

			Server.DebugConsole.NewLine("SENT : Item index " + itemIndex + " Active set to " + value, Color.Violet);
		}

		static void SetItemActiveReceived(int listenerID, Packet packet) //CLIENT RECEIVES
		{
			int itemIndex = packet.ReadInt();

			bool itemActive = packet.ReadBool();

			Client.GameScreen.GroundItems.SetActive(itemIndex, itemActive);
		}
		#endregion

		#region Player movement input packets

		public static void PlayerMoveSend(int X, int Y) //CLIENT SENDS
		{
			using (Packet packet = new Packet(SentPacket.MovementInput))
			{
				packet.Write(X);
				packet.Write(Y);

				SendServerTCPData(packet);
			}
		}

		static void PlayerMoveReceived(int listenerID, Packet packet) //SERVER RECEIVES
		{
			int X = packet.ReadInt();
			int Y = packet.ReadInt();

			Server.GameLogic.PlayerMove(listenerID, new Point(X, Y));
		}
		#endregion

		#region Player positions packets

		public static void PlayerPositionsSend(Point[] positions) //SERVER SENDS
		{
			using (Packet packet = new Packet(SentPacket.PlayerPositions))
			{
				packet.Write(positions.Length);

				for (int i = 0; i < positions.Length; i++)
				{
					packet.Write(positions[i].X);
					packet.Write(positions[i].Y);
				}

				SendTCPDataToAllListeners(packet);
			}

			//Server.DebugConsole.NewLine("SENT : Player positions.", Color.Cyan);
		}

		static void PlayerPositionsReceived(int listenerID, Packet packet) //CLIENT RECEIVES
		{
			int length = packet.ReadInt();

			Point[] newPlayerPositions = new Point[length];

			for (int i = 0; i < newPlayerPositions.Length; i++) //MAX PLAYER CHANGED PER SERVER, GET FROM INITIAL CONNECTION
			{
				newPlayerPositions[i].X = packet.ReadInt();
				newPlayerPositions[i].Y = packet.ReadInt();
			}

			Client.GameScreen.PlayerPool.SetPositions(newPlayerPositions);

			Main.MainCamera.FollowPosition(Client.GameScreen.PlayerPool.GetPlayer(Client.GetID()).Position);
		}
		#endregion

		#region Enemy positions packets
		
		public static void EnemyPositionsSend(Point[] positions) //SERVER SENDS
		{
			using (Packet packet = new Packet(SentPacket.EnemyPositions))
			{
				packet.Write(positions.Length);

				for(int i = 0; i < positions.Length; i++)
				{
					packet.Write(positions[i].X);
					packet.Write(positions[i].Y);
				}

				SendTCPDataToAllListeners(packet);
			}

			//Server.DebugConsole.NewLine("SENT : Enemy positions.", Color.Gray);
		}

		static void EnemyPositionsReceived(int listenerID, Packet packet) //CLIENT RECEIVES
		{
			int length = packet.ReadInt();

			Point[] positions = new Point[length];

			for(int i = 0; i < positions.Length; i++)
			{
				positions[i].X = packet.ReadInt();
				positions[i].Y = packet.ReadInt();
			}

			Client.GameScreen.EnemyPool.SetAllPositions(positions);
		}
		#endregion

		#region Projectile spawned packets

		public static void ProjectileSpawnedSend(int index, Projectile projectile) //SERVER SEND
		{
			using (Packet packet = new Packet(SentPacket.ProjectileSpawned))
			{
				packet.Write(index);

				packet.Write(projectile.Position.X);
				packet.Write(projectile.Position.Y);

				packet.Write(projectile.Direction.X);
				packet.Write(projectile.Direction.Y);

				packet.Write(projectile.Speed);

				packet.Write(projectile.Range);

				packet.Write((int)projectile.Type);

				packet.Write(projectile.Frequency);
				packet.Write(projectile.Amplitude);

				packet.Write(projectile.Color.R);
				packet.Write(projectile.Color.G);
				packet.Write(projectile.Color.B);

				SendTCPDataToAllListeners(packet);
			}

			Server.DebugConsole.NewLine($"SENT : New projectile (index: {index}).", Color.Cyan);
		}

		static void ProjectileSpawnedReceived(int listenerID, Packet packet) //CLIENT RECEIVES
		{
			int index = packet.ReadInt();

			Projectile projectile = new Projectile();

			projectile.Position.X = packet.ReadInt();
			projectile.Position.Y = packet.ReadInt();

			projectile.Direction.X = packet.ReadFloat();
			projectile.Direction.Y = packet.ReadFloat();

			projectile.Speed = packet.ReadInt();

			projectile.Range = packet.ReadFloat();

			projectile.Type = (ProjectileType)packet.ReadInt();

			projectile.Frequency = packet.ReadFloat();
			projectile.Amplitude = packet.ReadFloat();

			projectile.Color.R = packet.ReadByte();
			projectile.Color.G = packet.ReadByte();
			projectile.Color.B = packet.ReadByte();

			Client.GameScreen.ProjectilePool.SetProjectile(index, projectile);
		}
		#endregion

		#region Projectile positions packets

		public static void ProjectilePositionsSend(Point[] positions) //SERVER SEND
		{
			using (Packet packet = new Packet(SentPacket.ProjectilePositions))
			{
				for (int i = 0; i < positions.Length; i++)
				{
					packet.Write(positions[i].X);
					packet.Write(positions[i].Y);
				}

				SendTCPDataToAllListeners(packet);
			}

			//Server.DebugConsole.NewLine("SENT : Projectile positions.", Color.Cyan);
		}

		static void ProjectilePositionsReceived(int listenerID, Packet packet) //CLIENT RECEIVES
		{
			Point[] newProjectilePositions = new Point[10];

			for (int i = 0; i < newProjectilePositions.Length; i++) //MAX PLAYER CHANGED PER SERVER, GET FROM INITIAL CONNECTION
			{
				newProjectilePositions[i].X = packet.ReadInt();
				newProjectilePositions[i].Y = packet.ReadInt();
			}

			Client.GameScreen.ProjectilePool.SetPositions(newProjectilePositions);
		}
		#endregion

		#region Player attack input packets

		public static void PlayerAttackSend(Vector2 direction) //CLIENT SENDS
		{
			using (Packet packet = new Packet(SentPacket.AttackInput))
			{
				packet.Write(direction.X);
				packet.Write(direction.Y);

				SendServerTCPData(packet);
			}
		}

		static void PlayerAttackReceived(int listenerID, Packet packet) //SERVER RECEIVES
		{
			Vector2 direction;
			direction.X = packet.ReadFloat();
			direction.Y = packet.ReadFloat();

			Server.GameLogic.PlayerAttack(listenerID, direction);
		}
		#endregion
	}
}
