using Microsoft.Xna.Framework.Graphics;

namespace NetZone
{
	public class UICamera : Camera
	{
		public UICamera(Viewport viewport)
		{
			view = viewport;
		}
	}
}