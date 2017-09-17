using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics;

namespace WireWorld
{
	public class CopperTileType : TileType
	{
		public static readonly Color4 Color = Color4.Orange;
		public static readonly Color4 HeadColor = Color4.White;
		public static readonly Color4 TailColor = Color4.Yellow;

		public override Color4 GetColor(Tile tile, int x, int y)
		{
			switch (tile.Data)
			{
				case 1: return HeadColor;
				case 2: return TailColor;
				default: return Color;
			}
		}

		public override void Update(Grid grid, ref Tile tile, int x, int y)
		{
			switch (tile.Data)
			{
				case 0:
					int sources = grid.GetNeighbors(x, y).Count(t => t.IsSource);
					tile.Data = (byte)((((sources + 1) / 2) == 1) ? 1 : 0);
					break;
				case 1:
					tile.Data = 2;
					break;
				case 2:
					tile.Data = 0;
					break;
			}
		}
	}
}
