using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WireWorld
{
	public class Map
	{
		private Grid current;
		private Grid next;
		private int scale;
		private int updateSpeed = 1;
		private int update = 0;
		private bool paused;
		private bool forceUpdate;

		public int Width => current.Width;
		public int Height => current.Height;

		public Grid Clipboard { get; set; }

		public int Scale
		{
			get => scale;
			set => scale = MathHelper.Clamp(value, 0, 20);
		}

		public int UpdateSpeed
		{
			get => updateSpeed;
			set => updateSpeed = MathHelper.Clamp(value, 1, 6);
		}

		public bool Paused
		{
			get => paused;
			set => paused = value;
		}

		public bool ForceUpdate
		{
			get => forceUpdate;
			set => forceUpdate = value;
		}

		public Map(int width, int height)
		{
			current = new Grid(width, height);
			next = new Grid(width, height);
		}

		public Tile this[int x, int y]
		{
			get => next[x, y] = current[x, y];
			set => next[x, y] = current[x, y] = value;
		}

		private void UpdateCell(int x, int y)
		{
			Tile tile = current[x, y];
			TileType type = tile.ID;

			if (type != null)
			{
				type.Update(current, ref current.Tiles[x, y], x, y);

				next[x, y] = current[x, y];
				current[x, y] = tile;
			}
		}

		public void Update()
		{
			Parallel.For(0, next.Length, i =>
			{
				int x = i % Width;
				int y = i / Width;
				UpdateCell(x, y);
			});

			next.CopyTo(current);
		}

		public void Paste(Grid grid, int x = 0, int y = 0, Rectangle? sourceRect = null, bool merge = false)
		{
			current.Paste(grid, x, y, sourceRect, merge);
		}

		public void Render()
		{
			current.Render();
		}
	}
}
