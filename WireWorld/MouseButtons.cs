using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace WireWorld
{
	[Flags]
	public enum MouseButtons
	{
		None,
		Left = 1,
		Right = 2,
		Middle = 4
	}

	internal static class MouseButtonExtensions
	{
		public static MouseButtons ToMouseButtons(this MouseButton button)
		{
			switch (button)
			{
				case MouseButton.Left: return MouseButtons.Left;
				case MouseButton.Middle: return MouseButtons.Middle;
				case MouseButton.Right: return MouseButtons.Right;
				default: return MouseButtons.None;
			}
		}
	}
}
