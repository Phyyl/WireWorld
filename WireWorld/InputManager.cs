using OpenTK;

namespace WireWorld
{
	public static partial class InputManager
	{
		private static GameWindow window;
		
		public static bool IsWindowFocused => window.Focused;

		public static void Initialize(GameWindow window)
		{
			InputManager.window = window;

			InitializeKeyboard();
			InitializeMouse();
		}

		public static void BeginUpdate()
		{
			BeginUpdateKeyboard();
			BeginUpdateMouse();
		}

		public static void EndUpdate()
		{
			EndUpdateKeyboard();
			EndUpdateMouse();
		}
	}
}
