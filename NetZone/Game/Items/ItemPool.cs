using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NetZone
{
	class ItemPool
	{
		public Item[] Items;

		public ItemPool(int size)
		{
			Items = new Item[size];

			for (int i = 0; i < Items.Length; i++)
			{
				Items[i] = new Item();
				Items[i].Active = false;
			}
		}

		public void Update()
		{
			for(int i = 0; i < Items.Length; i++)
			{
				if(Items[i].Active == true)
				{
					Items[i].Update();
				}
			}
		}

		public bool IsFull()
		{
			for (int i = 0; i < Items.Length; i++)
			{
				if (Items[i].Active == false)
				{
					return true;
				}
			}

			return false;
		}

		public void SetItem(int index, Item item)
		{
			Items[index] = item;
			Items[index].Active = true;
		}

		public void SetUnusedItem(Item item)
		{
			for (int i = 0; i < Items.Length; i++)
			{
				if (Items[i].Active == false)
				{
					Items[i] = item;
					Items[i].Active = true;

					break;
				}
			}
		}

		public int GetUnusedIndex()
		{
			for (int i = 0; i < Items.Length; i++)
			{
				if (Items[i].Active == false)
				{
					return i;
				}
			}

			return 0; //Pool overflow !!!
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			for (int i = 0; i < Items.Length; i++)
			{
				if (Items[i].Active == true)
				{
					Items[i].Draw(spriteBatch);
				}
			}
		}
	}
}
