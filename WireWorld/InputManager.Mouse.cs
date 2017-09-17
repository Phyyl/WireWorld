using OpenTK;
using OpenTK.Input;

namespace WireWorld
{
	public static partial class InputManager
	{
		private static MouseState currentState;
		private static MouseState previousState;
		private static int previousMouseScroll;

		public static Vector2 Delta { get; private set; }
		public static Vector2 Position => window.PointToClient(new Point(currentState.X, currentState.Y)).ToVector();
		public static int Scroll => currentState.Wheel - previousMouseScroll;
		public static bool IsCaptured => !window.CursorVisible;
		public static bool IsInsideWindow => window.ClientSize.ToRectangle().Contains(Position.ToPoint());

		private static void InitializeMouse()
		{
			previousState = currentState = Mouse.GetCursorState();
		}
		
		private static void BeginUpdateMouse()
		{
			previousState = currentState;
			currentState = Mouse.GetCursorState();

			if (IsCaptured)
			{
				Delta = (Position - (window.ClientSize.ToVector() / 2).Floor());
				CenterMouse();
			}
			else
			{
				Delta = Vector2.Zero;
			}
		}

		private static void EndUpdateMouse()
		{
			previousMouseScroll = currentState.ScrollWheelValue;
		}

		public static void CaptureMouse()
		{
			window.CursorVisible = false;
			CenterMouse();
		}

		public static void ReleaseMouse()
		{
			window.CursorVisible = true;
		}

		private static void CenterMouse()
		{
			SetMousePosition(window.ClientSize.ToVector() / 2);
		}

		private static void SetMousePosition(Vector2 position)
		{
			Point point = window.PointToScreen(new Point((int)position.X, (int)position.Y));

			Mouse.SetPosition(point.X, point.Y);
		}

		public static bool IsMouseButtonDown(MouseButton button, bool ignoreWindowFocus = false)
		{
			return (ignoreWindowFocus || IsWindowFocused) && currentState.IsButtonDown(button) && IsInsideWindow;
		}

		public static bool WasMouseButtonDown(MouseButton button, bool ignoreWindowFocus = false)
		{
			return (ignoreWindowFocus || IsWindowFocused) && previousState.IsButtonDown(button) && IsInsideWindow;
		}

		public static bool IsMouseButtonPressed(MouseButton button, bool ignoreWindowFocus = false)
		{
			return (ignoreWindowFocus || IsWindowFocused) && IsMouseButtonDown(button) && !WasMouseButtonDown(button);
		}

		public static bool WasMouseButtonReleased(MouseButton button, bool ignoreWindowFocus = false)
		{
			return (ignoreWindowFocus || IsWindowFocused) && !IsMouseButtonDown(button) && WasMouseButtonDown(button);
		}
	}
}