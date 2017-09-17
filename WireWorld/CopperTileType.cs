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
		public override Color4 GetColor(Grid grid, Tile tile, int x, int y)
		{
			switch (tile.Data)
			{
				case 1: return Color4.White;
				case 2: return Color4.Yellow;
				default: return Color4.Orange;
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
