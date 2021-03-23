using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace NetZone
{
	class JohnsHelper
	{
		public static int PointDistance(Point a, Point b)
		{
			return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
		}
	}
}
