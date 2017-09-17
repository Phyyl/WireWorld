using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WireWorld
{
	public abstract class TileType
	{
		private static int typeCount = 1;
		private static TileType[] types = new TileType[256];
		
		public static readonly CopperTileType Copper = new CopperTileType();
		
		public byte ID { get; internal set; }

		public abstract void Update(Grid grid, ref Tile tile, int x, int y);
		public abstract Color4 GetColor(Grid grid, Tile tile, int x, int y);

		public TileType()
		{
			Register(this);
		}

		public virtual void Render(Grid grid, Tile tile, int x, int y)
		{
			GL.Begin(PrimitiveType.Quads);

			GL.Color4(GetColor(grid, tile, x, y));

			GL.Vertex2(x, y);
			GL.Vertex2(x, y + 1);
			GL.Vertex2(x + 1, y + 1);
			GL.Vertex2(x + 1, y);

			GL.End();
		}

		private static void Register(TileType type)
		{
			type.ID = (byte)(typeCount);
			types[type.ID] = type;
		}

		public static implicit operator TileType(byte id) => types[id];
	}
}
