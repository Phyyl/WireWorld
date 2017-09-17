using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;

namespace WireWorld
{
	public static partial class InputManager
	{
		private static string text;

		private static HashSet<Key> keyStates;

		private static HashSet<Key> keyDowns;
		private static HashSet<Key> keyUps;
		private static List<KeyPressEventArgs> keyPresses;

		public static KeyModifiers Modifiers { get; private set; }

		public static string Text
		{
			get { return text ?? ""; }
			private set { text = value; }
		}

		private static void InitializeKeyboard()
		{
			keyDowns = new HashSet<Key>();
			keyUps = new HashSet<Key>();
			keyStates = new HashSet<Key>();
			keyPresses = new List<KeyPressEventArgs>();

			window.KeyDown += Window_KeyDown;
			window.KeyUp += Window_KeyUp;
			window.KeyPress += Window_KeyPress;
		}

		public static bool IsKeyDown(Key key, bool ignoreWindowFocus = false)
		{
			return (ignoreWindowFocus || IsWindowFocused) && keyStates.Contains(key);
		}

		public static bool IsKeyPressed(Key key, bool ignoreWindowFocus = false)
		{
			return (ignoreWindowFocus || IsWindowFocused) && keyDowns.Contains(key);
		}

		public static bool IsKeyReleased(Key key, bool ignoreWindowFocus = false)
		{
			return (ignoreWindowFocus || IsWindowFocused) && keyUps.Contains(key);
		}

		private static void Window_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsControl(e.KeyChar))
			{
				Text += e.KeyChar;
			}

			keyPresses.Add(e);
		}

		private static void Window_KeyDown(object sender, KeyboardKeyEventArgs e)
		{
			keyDowns.Add(e.Key);
			keyStates.Add(e.Key);
		}

		private static void Window_KeyUp(object sender, KeyboardKeyEventArgs e)
		{
			keyUps.Add(e.Key);
			keyStates.Remove(e.Key);
		}

		private static void BeginUpdateKeyboard()
		{
			Modifiers = KeyModifiers.None;

			if (IsKeyDown(Key.LControl) || IsKeyDown(Key.RControl))
			{
				Modifiers |= KeyModifiers.Control;
			}

			if (IsKeyDown(Key.LAlt) || IsKeyDown(Key.RAlt))
			{
				Modifiers |= KeyModifiers.Alt;
			}

			if (IsKeyDown(Key.LShift) || IsKeyDown(Key.RShift))
			{
				Modifiers |= KeyModifiers.Shift;
			}

			if (IsKeyDown(Key.LWin) || IsKeyDown(Key.RWin))
			{
				Modifiers |= KeyModifiers.Win;
			}
		}

		private static void EndUpdateKeyboard()
		{
			keyDowns.Clear();
			keyUps.Clear();
			keyPresses.Clear();

			Text = "";
		}
	}
}
