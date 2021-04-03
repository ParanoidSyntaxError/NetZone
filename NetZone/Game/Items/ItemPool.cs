using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NetZone
{
	struct ItemPoolObject
	{
		public Item Item;
		public bool Active;
	}

	class ItemPool
	{
		ItemPoolObject[] pool;

		public ItemPool(int size)
		{
			pool = new ItemPoolObject[size];

			for (int i = 0; i < pool.Length; i++)
			{
				pool[i].Item = new Item();
				pool[i].Active = false;
			}
		}

		public void Update()
		{
			for(int i = 0; i < pool.Length; i++)
			{
				if(pool[i].Active == true)
				{
					pool[i].Item.Update();
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

		public Item GetItem(int index)
		{
			return pool[index].Item;
		}

		public void SetItem(int index, Item item)
		{
			pool[index].Item = item;
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

		public void SetUnusedItem(Item item)
		{
			for (int i = 0; i < pool.Length; i++)
			{
				if (pool[i].Active == false)
				{
					pool[i].Item = item;
					pool[i].Active = true;
					break;
				}
			}
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

		public void Draw(SpriteBatch spriteBatch)
		{
			for (int i = 0; i < pool.Length; i++)
			{
				if (pool[i].Active == true)
				{
					pool[i].Item.Draw(spriteBatch);
				}
			}
		}
	}
}
