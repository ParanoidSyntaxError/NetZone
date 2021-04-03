using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetZone
{
	class Weapon : Item
	{
		public int Damage;

		public int Speed;

		public float Range;

		public ProjectileType ProjectileType;

		public float Frequency;
		public float Amplitude;

		public Weapon()
		{
			
		}

		public Projectile GetProjectile()
		{
			return new Projectile()
			{
				Damage = Damage,

				Speed = Speed,

				Range = Range,

				Type = ProjectileType,

				Frequency = Frequency,
				Amplitude = Amplitude,

				Color = Color
			};
		}
	}
}
