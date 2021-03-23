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
	class UniqueWeapons
	{
		public static Weapon PhasedPlasmaRifle()
		{
			return new Weapon()
			{
				Art = "P",
				Color = Color.Purple,
				Damage = 1,
				Speed = 150,
				Range = 1,
				ProjectileType = ProjectileType.Line
			};
		}
	}
}
