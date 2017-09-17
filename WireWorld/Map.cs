using OpenTK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WireWorld
{
	public class Map
	{
		private Grid current;
		private Grid next;

		public int Width => current.Width;
		public int Height => current.Height;

		public Map(int width, int height)
		{
			InitSize(width, height);
		}

		private void InitSize(int width, int height)
		{
			current = new Grid(width, height);
			next = new Grid(width, height);
		}

		public Tile this[int x, int y]
		{
			get => current[x, y];
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
			next.Paste(grid, x, y, sourceRect, merge);
		}

		public Grid CreateCopy(Rectangle? sourceRect = null)
		{
			return next.CreateCopy(sourceRect);
		}

		public void Render()
		{
			current.Render();
		}

		//TODO: Add tile type dictionary serialization

		public void Save(string path)
		{
			try
			{
				using (FileStream fs = File.OpenWrite(path))
				{
					using (BinaryWriter writer = new BinaryWriter(fs))
					{
						writer.Write(Width);
						writer.Write(Height);

						for (int x = 0; x < Width; x++)
						{
							for (int y = 0; y < Height; y++)
							{
								Tile tile = this[x, y];

								writer.Write(tile.ID);
								writer.Write(tile.Data);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				Console.WriteLine(ex.StackTrace);
			}
		}

		public void Load(string path)
		{
			try
			{
				using (FileStream fs = File.OpenRead(path))
				{
					using (BinaryReader reader = new BinaryReader(fs))
					{
						int width = reader.ReadInt32();
						int height = reader.ReadInt32();

						InitSize(width, height);

						for (int x = 0; x < width; x++)
						{
							for (int y = 0; y < height; y++)
							{
								this[x, y] = new Tile(reader.ReadByte(), reader.ReadByte());
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				Console.WriteLine(ex.StackTrace);
			}
		}
	}
}
