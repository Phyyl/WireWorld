using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WireWorld
{
	public class Grid
	{
		public Tile[,] Tiles { get; }
		public int Width { get; }
		public int Height { get; }

		public int Length => Tiles.Length;
		public Rectangle Bounds => new Rectangle(0, 0, Width, Height);

		public Grid(int width, int height)
		{
			Tiles = new Tile[width, height];

			Width = width;
			Height = height;
		}

		public Tile this[int x, int y]
		{
			get => IsValidPosition(x, y) ? Tiles[x, y] : Tile.Void;
			set
			{
				if (IsValidPosition(x, y)) Tiles[x, y] = value;
			}
		}

		public void Paste(Grid grid, int x = 0, int y = 0, Rectangle? sourceRect = null, bool merge = false)
		{
			Rectangle finalSourceRect = sourceRect ?? grid.Bounds;

			for (int xx = 0; xx < finalSourceRect.Width; xx++)
			{
				for (int yy = 0; yy < finalSourceRect.Height; yy++)
				{
					Tile value = this[x + xx, y + yy];

					if (!merge || value.ID == 0)
					{
						this[x + xx, y + yy] = grid[xx + finalSourceRect.X, yy + finalSourceRect.Y];
					}
				}
			}
		}

		public IEnumerable<Tile> GetNeighbors(int x, int y)
		{
			yield return this[x - 1, y - 1];
			yield return this[x - 1, y];
			yield return this[x - 1, y + 1];
			yield return this[x, y - 1];
			yield return this[x, y + 1];
			yield return this[x + 1, y - 1];
			yield return this[x + 1, y];
			yield return this[x + 1, y + 1];
		}

		public Grid CreateCopy(Rectangle? sourceRect = null)
		{
			Rectangle finalSourceRect = sourceRect ?? Bounds;

			Grid copy = new Grid(finalSourceRect.Width, finalSourceRect.Height);
			copy.Paste(this, sourceRect: finalSourceRect);

			return copy;
		}

		public void CopyTo(Grid grid)
		{
			Array.Copy(Tiles, grid.Tiles, Math.Min(Tiles.Length, grid.Tiles.Length));
		}

		public bool IsValidPosition(int x, int y) => x >= 0 && y >= 0 && x < Width && y < Height;

		public void Render(float alpha = 1)
		{
			for (int x = 0; x < Width; x++)
			{
				for (int y = 0; y < Height; y++)
				{
					Tile tile = this[x, y];
					TileType type = tile.ID;

					type?.Render(tile, x, y, alpha);
				}
			}
		}
	}
}
