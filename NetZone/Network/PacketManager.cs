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
		ProjectilePositions,
		ProjectileSpawned,
		ClientChat,
		ServerChat,
		ItemPickup,
		InitalServerConnection,
		ItemSpawned,
		PlayerSpawned,
		PlayerInventoryAdd
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
				{ SentPacket.PlayerPositions, PlayerPositionsReceived },
				{ SentPacket.ProjectilePositions, ProjectilePositionsReceived },
				{ SentPacket.ProjectileSpawned, ProjectileSpawnedReceived },
				{ SentPacket.ClientChat, ClientChatReceived },
				{ SentPacket.ServerChat, ServerChatReceived },
				{ SentPacket.InitalServerConnection, InitalServerConnectionReceived },
				{ SentPacket.ItemSpawned, ItemSpawnedReceived },
				{ SentPacket.PlayerSpawned, PlayerSpawnedReceived },
				{ SentPacket.PlayerInventoryAdd, PlayerInventoryAddReceived }
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
				if(Server.Listeners[i].Socket.TcpClient != null)
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

			Server.GameLogic.PlayerSpawn();
		}

		static void InitalServerConnectionReceived(int listenerID, Packet packet) //CLIENT RECEIVES
		{
			int clientID = packet.ReadInt();

			Client.Socket.ID = clientID;
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

			Server.GameLogic.ConsoleLines.Add(new ConsoleLine() { Line = message, Color = Color.Yellow });

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
		}

		static void PlayerInventoryAddReceived(int listenerID, Packet packet) //CLIENT RECEIVES
		{
			int itemIndex = packet.ReadInt();

			int inventoryIndex = packet.ReadInt();

			//Maybe move to a new GameScreen method
			Client.GameScreen.PlayerPool.Players[listenerID].Inventory.SetItem(itemIndex, Client.GameScreen.GroundItems.Items[itemIndex]);
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

			PlayerPositionsSend(Server.GameLogic.PlayerPool.GetAllPositions());
		}
		#endregion

		#region Player positions packets

		public static void PlayerPositionsSend(Point[] positions) //SERVER SENDS
		{
			using (Packet packet = new Packet(SentPacket.PlayerPositions))
			{
				for (int i = 0; i < positions.Length; i++)
				{
					packet.Write(positions[i].X);
					packet.Write(positions[i].Y);
				}

				SendTCPDataToAllListeners(packet);
			}

			Server.DebugConsole.NewLine("SENT : Player positions.", Color.Cyan);
		}

		static void PlayerPositionsReceived(int listenerID, Packet packet) //CLIENT RECEIVES
		{
			Point[] newPlayerPositions = new Point[10];

			for (int i = 0; i < newPlayerPositions.Length; i++) //MAX PLAYER CHANGED PER SERVER, GET FROM INITIAL CONNECTION
			{
				newPlayerPositions[i].X = packet.ReadInt();
				newPlayerPositions[i].Y = packet.ReadInt();
			}

			Client.GameScreen.PlayerPool.SetPositions(newPlayerPositions);
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

			projectile.Speed = packet.ReadFloat();

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

			Server.DebugConsole.NewLine("SENT : Projectile positions.", Color.Cyan);

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
