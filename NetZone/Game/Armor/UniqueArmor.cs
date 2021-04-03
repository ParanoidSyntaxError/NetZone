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
	class UniqueArmor
	{
		public static Armor AmuletOfAtuLoss() //HP set to 1 after a killing blow
		{
			return new Armor()
			{
				Art = "A",
				Color = Color.Gold,
				Defence = 1,
				Type = ArmorType.Necklace
			};
		}
	}
}
