using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace NetZone
{
	public class KeyBind
	{
		public Keys Key;
		public bool Pressing;
	}

	public enum KeyType
	{
		MoveUp,
		MoveDown,
		MoveRight,
		MoveLeft,

		Attack
	}

	class KeyBindings
	{
		static Dictionary<KeyType, KeyBind> keyBinds;

		public KeyBindings()
		{
			keyBinds = new Dictionary<KeyType, KeyBind>()
			{
				{ KeyType.MoveUp, new KeyBind(){	Key = Keys.W } },
				{ KeyType.MoveDown, new KeyBind(){	Key = Keys.S } },
				{ KeyType.MoveRight, new KeyBind(){ Key = Keys.D } },
				{ KeyType.MoveLeft, new KeyBind(){	Key = Keys.A } },

				{ KeyType.Attack, new KeyBind(){  Key = Keys.Space } }
			};
		}

		public static bool KeyPressed(KeyType keyType)
		{
			if(keyBinds[keyType].Pressing == false)
			{
				if (Keyboard.GetState().IsKeyDown(keyBinds[keyType].Key))
				{
					keyBinds[keyType].Pressing = true;

					return true;
				}
			}

			return false;
		}

		public static bool KeyPressing(KeyType keyType)
		{
			if(Keyboard.GetState().IsKeyDown(keyBinds[keyType].Key))
			{
				return true;
			}

			return false;
		}

		public void Update()
		{
			for (int i = 0; i < keyBinds.Count; i++)
			{
				if(Keyboard.GetState().IsKeyUp(keyBinds[(KeyType)i].Key))
				{
					keyBinds[(KeyType)i].Pressing = false;
				}
			}
		}
	}
}
