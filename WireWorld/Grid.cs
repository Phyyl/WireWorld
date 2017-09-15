using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WireWorld
{
    public class Grid
    {
        private byte[,] data;

        public int Width { get; }
        public int Height { get; }

        public int Length => data.Length;

        public byte this[int x, int y]
        {
            get => IsValidPosition(x, y) ? data[x, y] : default;
            set
            {
                if (IsValidPosition(x, y)) data[x, y] = value;
            }
        }

        public Grid(int width, int height)
        {
            data = new byte[width, height];

            Width = width;
            Height = height;
        }

        public void Merge(Grid grid, int destX, int destY, int sourceX, int sourceY, int width, int height)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    this[destX + x, destY + y] = grid[sourceX + x, sourceY + y];
                }
            }
        }

        public void Merge(Grid grid, int destX, int destY, int sourceX, int sourceY) => Merge(grid, destX, destY, sourceX, sourceY, grid.Width, grid.Height);

        public void Merge(Grid grid, int destX, int destY) => Merge(grid, destX, destY, 0, 0, grid.Width, grid.Height);

        public bool IsValidPosition(int x, int y) => x >= 0 && y >= 0 && x < Width && y < Height;

        public void CopyTo(Grid grid)
        {
            Array.Copy(data, grid.data, Math.Min(data.Length, grid.data.Length));
        }
    }
}
