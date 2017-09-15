using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WireWorld
{
    public class GameInstance
    {
        private byte[,] currentData;
        private byte[,] nextData;

        public int Width { get; }
        public int Height { get; }

        public GameInstance(int width, int height)
        {
            currentData = new byte[width, height];
            nextData = new byte[width, height];

            Width = width;
            Height = height;
        }

        public byte this[int x, int y]
        {
            get => IsValidPosition(x, y) ? currentData[x, y] : default;
            set
            {
                if (IsValidPosition(x, y)) currentData[x, y] = value;
            }
        }

        public bool IsValidPosition(int x, int y) => x >= 0 && y >= 0 && x < Width && y < Height;

        private byte GetNextValue(int x, int y)
        {
            byte value = this[x, y];

            if (value == 0) return 0;
            if (value == 2) return 3;
            if (value == 3) return 1;

            if (value != 1)
            {
                return 4;
            }

            int GetIsHead(int xx, int yy) => this[xx, yy] == 2 ? 1 : 0;

            int n =
                GetIsHead(x - 1, y - 1) +
                GetIsHead(x - 1, y) +
                GetIsHead(x - 1, y + 1) +
                GetIsHead(x, y - 1) +
                GetIsHead(x, y + 1) +
                GetIsHead(x + 1, y - 1) +
                GetIsHead(x + 1, y) +
                GetIsHead(x + 1, y + 1);

            if (n > 0 && n <= 2)
            {
                return 2;
            }

            return 1;
        }

        private void UpdateCell(int x, int y)
        {
            nextData[x, y] = GetNextValue(x, y);
        }

        public void Iterate()
        {
            Parallel.For(0, nextData.Length, i =>
            {
                int x = i % Width;
                int y = i / Width;
                UpdateCell(x, y);
            });

            Array.Copy(nextData, currentData, currentData.Length);
        }
    }
}
