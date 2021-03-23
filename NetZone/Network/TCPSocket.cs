using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NetZone
{
	class TCPSocket //Maybe split into two classes ???
	{
		public TcpClient TcpClient;

		NetworkStream networkStream;
		Packet receivedData;
		byte[] receiveBuffer;

		int dataBufferSize = 4096;

		public int ID;

		string ip = "127.0.0.1";
		int port = 8080;

		public TCPSocket(int clientID) //SERVER
		{
			ID = clientID;
		}

		public TCPSocket(string connectingIP, int connectingPort) //CLIENT
		{
			ip = connectingIP;
			port = connectingPort;
		}

		public void Connect(TcpClient tcpClient = null)
		{
			if(tcpClient != null) //SERVER
			{
				TcpClient = tcpClient;
				TcpClient.ReceiveBufferSize = dataBufferSize;
				TcpClient.SendBufferSize = dataBufferSize;

				networkStream = TcpClient.GetStream();

				receivedData = new Packet();

				receiveBuffer = new byte[dataBufferSize];

				networkStream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallBack, null);
			}
			else //CLIENT
			{
				TcpClient = new TcpClient
				{
					ReceiveBufferSize = dataBufferSize,
					SendBufferSize = dataBufferSize
				};

				receiveBuffer = new byte[dataBufferSize];
				TcpClient.BeginConnect(ip, port, ConnectCallBack, TcpClient);
			}
		}

		void ReceiveCallBack(IAsyncResult result)
		{
			try
			{
				int byteLength = networkStream.EndRead(result);

				if (byteLength <= 0)
				{
					//Disconnect
					return;
				}

				byte[] data = new byte[byteLength];
				Array.Copy(receiveBuffer, data, byteLength);

				receivedData.Reset(HandleData(data));

				networkStream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallBack, null);
			}
			catch (Exception exception)
			{
				Server.DebugConsole.NewLine($"{exception}", Color.Red);

				//Disconnect
			}
		}

		void ConnectCallBack(IAsyncResult result)
		{
			TcpClient.EndConnect(result);

			if (TcpClient.Connected == false)
			{
				return;
			}

			networkStream = TcpClient.GetStream();

			receivedData = new Packet();

			networkStream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallBack, null);
		}

		public void SendData(Packet packet)
		{
			try
			{
				if (TcpClient != null)
				{
					networkStream.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null);
				}
			}
			catch (Exception exception)
			{
				Server.DebugConsole.NewLine($"Error sending data: {exception}", Color.Red);
			}
		}

		bool HandleData(byte[] data)
		{
			int packetLength = 0;

			receivedData.SetBytes(data);

			if (receivedData.UnreadLength() >= 4)
			{
				packetLength = receivedData.ReadInt();

				if (packetLength <= 0)
				{
					return true;
				}
			}

			while (packetLength > 0 && packetLength <= receivedData.UnreadLength())
			{
				byte[] packetBytes = receivedData.ReadBytes(packetLength);
				ThreadManager.ExecuteOnMainThread(() =>
				{
					using (Packet packet = new Packet(packetBytes))
					{
						int packetID = packet.ReadInt();
						PacketManager.PacketHandlers[(SentPacket)packetID](ID, packet);
					}
				});

				packetLength = 0;

				if (receivedData.UnreadLength() >= 4)
				{
					packetLength = receivedData.ReadInt();

					if (packetLength <= 0)
					{
						return true;
					}
				}
			}

			if (packetLength <= 1)
			{
				return true;
			}

			return false;
		}
	}
}
