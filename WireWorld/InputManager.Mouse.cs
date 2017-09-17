using OpenTK;
using OpenTK.Input;

namespace WireWorld
{
	public static partial class InputManager
	{
		private static MouseState currentMouseState;
		private static MouseState previousMouseState;
		private static int previousMouseScroll;

		public static Vector2 MousePositionDelta { get; private set; }
		public static Vector2 MousePosition => window.PointToClient(new Point(currentMouseState.X, currentMouseState.Y)).ToVector();
		public static Vector2 MousePreviousPosition => window.PointToClient(new Point(previousMouseState.X, previousMouseState.Y)).ToVector();

		public static int MouseScroll => currentMouseState.Wheel - previousMouseScroll;
		public static bool IsCaptured => !window.CursorVisible;
		public static bool IsInsideWindow => window.ClientSize.ToRectangle().Contains(MousePosition.ToPoint());

		public static MouseButtons MouseButtons { get; private set; }

		private static void InitializeMouse()
		{
			previousMouseState = currentMouseState = Mouse.GetCursorState();

			window.MouseDown += Window_MouseDown;
			window.MouseUp += Window_MouseUp;
		}

		private static void Window_MouseUp(object sender, MouseButtonEventArgs e)
		{
			MouseButtons &= ~e.Button.ToMouseButtons();
		}

		private static void Window_MouseDown(object sender, MouseButtonEventArgs e)
		{
			MouseButtons |= e.Button.ToMouseButtons();
		}

		private static void BeginUpdateMouse()
		{
			previousMouseState = currentMouseState;
			currentMouseState = Mouse.GetCursorState();

			if (IsCaptured)
			{
				MousePositionDelta = (MousePosition - (window.ClientSize.ToVector() / 2).Floor());
				CenterMouse();
			}
			else
			{
				MousePositionDelta = Vector2.Zero;
			}
		}

		private static void EndUpdateMouse()
		{
			previousMouseScroll = currentMouseState.ScrollWheelValue;
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
			return (ignoreWindowFocus || IsWindowFocused) && currentMouseState.IsButtonDown(button) && IsInsideWindow;
		}

		public static bool WasMouseButtonDown(MouseButton button, bool ignoreWindowFocus = false)
		{
			return (ignoreWindowFocus || IsWindowFocused) && previousMouseState.IsButtonDown(button) && IsInsideWindow;
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