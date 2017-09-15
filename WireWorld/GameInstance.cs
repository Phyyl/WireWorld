using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WireWorld
{
    public class GameInstance
    {
        private Grid current;
        private Grid next;

        public int Width => current.Width;
        public int Height => current.Height;

        public byte this[int x, int y]
        {
            get => current[x, y];
            set => current[x, y] = value;
        }

        public GameInstance(int width, int height)
        {
            current = new Grid(width, height);
            next = new Grid(width, height);
        }

        private byte GetNextValue(int x, int y)
        {
            byte value = current[x, y];

            if (value == 0) return 0;
            if (value == 2) return 3;
            if (value == 3) return 1;

            if (value != 1)
            {
                return 4;
            }

            int GetIsHead(int xx, int yy) => current[xx, yy] == 2 ? 1 : 0;

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
            next[x, y] = GetNextValue(x, y);
        }

        public void Iterate()
        {
            Parallel.For(0, next.Length, i =>
            {
                int x = i % Width;
                int y = i / Width;
                UpdateCell(x, y);
            });

            next.CopyTo(current);
        }

        public void Paste(Grid grid, int x, int y)
        {
            current.Merge(grid, x, y);
        }
    }
}
