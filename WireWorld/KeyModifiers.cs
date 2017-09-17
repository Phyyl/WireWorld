using System;
using System.Collections.Generic;
using System.Text;

namespace WireWorld
{
	[Flags]
	public enum KeyModifiers
	{
		None,
		Control = 1,
		Alt = 2,
		Shift = 4,
		Win = 8
	}
}
