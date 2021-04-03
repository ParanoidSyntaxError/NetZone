using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetZone
{
	enum ArmorType
	{
		Head,
		Chest,
		Legs,

		Ring,
		Necklace
	}

	class Armor : Item
	{
		public int Defence;

		public ArmorType Type;

		public Armor()
		{

		}
	}
}
