using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;

namespace WireWorld
{
	public static partial class InputManager
	{
		private static string text;
		private static KeyboardState keyboardState;
		private static KeyboardState previousKeyboardState;

		internal static HashSet<Key> KeyDowns { get; private set; }
		internal static HashSet<Key> KeyUps { get; private set; }
		internal static List<KeyPressEventArgs> KeyPresses { get; private set; }

		public static KeyModifiers Modifiers { get; private set; } = new KeyModifiers();

		public static string Text
		{
			get { return text ?? ""; }
			private set { text = value; }
		}

		private static void InitializeKeyboard()
		{
			KeyDowns = new HashSet<Key>();
			KeyUps = new HashSet<Key>();
			KeyPresses = new List<KeyPressEventArgs>();

			window.KeyDown += Window_KeyDown;
			window.KeyUp += Window_KeyUp;
			window.KeyPress += Window_KeyPress;

			previousKeyboardState = keyboardState = Keyboard.GetState();
		}

		public static bool IsKeyDown(Key key, bool ignoreWindowFocus = false)
		{
			return (ignoreWindowFocus || IsWindowFocused) && keyboardState.IsKeyDown(key);
		}

		public static bool WasKeyDown(Key key, bool ignoreWindowFocus = false)
		{
			return (ignoreWindowFocus || IsWindowFocused) && previousKeyboardState.IsKeyDown(key);
		} 

		public static bool IsKeyPressed(Key key, bool ignoreWindowFocus = false)
		{
			return (ignoreWindowFocus || IsWindowFocused) && KeyDowns.Contains(key);
		}

		public static bool IsKeyReleased(Key key, bool ignoreWindowFocus = false)
		{
			return (ignoreWindowFocus || IsWindowFocused) && KeyUps.Contains(key);
		}

		private static void Window_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsControl(e.KeyChar))
			{
				Text += e.KeyChar;
			}

			KeyPresses.Add(e);
		}

		private static void Window_KeyUp(object sender, KeyboardKeyEventArgs e)
		{
			KeyUps.Add(e.Key);
		}

		private static void Window_KeyDown(object sender, KeyboardKeyEventArgs e)
		{
			KeyDowns.Add(e.Key);
		}

        private static void BeginUpdateKeyboard()
		{
			previousKeyboardState = keyboardState;
			keyboardState = Keyboard.GetState();
			
			Modifiers.IsAltDown = IsKeyDown(Key.LAlt) || IsKeyDown(Key.RAlt);
			Modifiers.IsCtrlDown = IsKeyDown(Key.LControl) || IsKeyDown(Key.RControl);
			Modifiers.IsWindowsDown = IsKeyDown(Key.LWin) || IsKeyDown(Key.RWin);
			Modifiers.IsShiftDown = IsKeyDown(Key.LShift) || IsKeyDown(Key.RShift);
		}

		private static void EndUpdateKeyboard()
		{
			KeyDowns.Clear();
			KeyUps.Clear();
			KeyPresses.Clear();

			Text = "";
		}
		
		public class KeyModifiers
		{
			public bool IsCtrlDown { get; internal set; }
			public bool IsAltDown { get; internal set; }
			public bool IsWindowsDown { get; internal set; }
			public bool IsShiftDown { get; internal set; }
		}
	}
}
