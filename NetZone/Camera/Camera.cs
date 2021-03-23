using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NetZone
{
	public class Camera
	{
		protected Viewport view;

		public Matrix Transform;

		float zoom = 1;

		public virtual void Zoom(float amount)
		{
			zoom += amount;

			if (zoom <= 0.2f)
			{
				zoom = 0.2f;
			}
		}

		public virtual void ResetZoom()
		{
			zoom = 1;
		}

		public virtual void FollowPosition(Point position)
		{
			Transform = Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0)) *
						Matrix.CreateScale(Settings.Scale * zoom) *
						Matrix.CreateTranslation(new Vector3(view.Width / 2, view.Height / 2, 0));
		}

		public virtual void Center()
		{
			Transform = Matrix.CreateTranslation(Vector3.Zero) * Matrix.CreateScale(Settings.Scale * zoom);
		}

		public virtual Point GetMousePosition()
		{
			return Vector2.Transform(Mouse.GetState().Position.ToVector2(), Matrix.Invert(Transform)).ToPoint();
		}
	}
}
