using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WireWorld
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Tile
    {
        public static readonly Tile Void = new Tile();
        public static readonly Tile Copper = new Tile(TileType.Copper.ID, 0);
        public static readonly Tile CopperHead = new Tile(TileType.Copper.ID, 1);
        public static readonly Tile CopperTail = new Tile(TileType.Copper.ID, 2);

        [FieldOffset(0)]
        public byte ID;
        [FieldOffset(8)]
        public byte Data;

		public bool IsSource => (Data & 1) == 1;
        
        public Tile(byte id, byte data)
        {
            ID = id;
            Data = data;
        }
    }
}
