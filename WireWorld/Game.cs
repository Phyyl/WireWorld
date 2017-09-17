using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WireWorld
{
	public class Game
	{
		public Map Map { get; private set; }

		private string path;
		private int update = 0;
		private int updateSpeed = 4;
		private float scale = 10;
		private Vector2 offset;
		private MouseButtons downButton;
		private MouseMode mouseMode;
		private Point startPoint;
		private Grid clipboard;

		public bool Paused { get; set; }
		public bool ForceUpdate { get; set; }

		public int UpdateSpeed
		{
			get => updateSpeed;
			set => updateSpeed = MathHelper.Clamp(value, 1, 6);
		}

		private Point MousePoint => new Point((int)(InputManager.MousePosition.X / scale), (int)(InputManager.MousePosition.Y / scale));
		private Point PreviousMousePoint => new Point((int)(InputManager.MousePreviousPosition.X / scale), (int)(InputManager.MousePreviousPosition.Y / scale));
		private Rectangle SelectionRectangle => Rectangle.FromLTRB(Math.Min(startPoint.X, MousePoint.X), Math.Min(startPoint.Y, MousePoint.Y), Math.Max(startPoint.X, MousePoint.X) + 1, Math.Max(startPoint.Y, MousePoint.Y) + 1);

		private bool ShouldUpdate => ForceUpdate || (!Paused && (update % MathHelper.Clamp(Math.Pow(2, 6 - updateSpeed), 1, 60) == 0));

		public void CreateNewMap(int width, int height)
		{
			Map = new Map(width, height);
		}

		public Game(string path = null)
		{
			this.path = path;
		}

		public void Update(object delta)
		{
			update++;

			if (Map != null)
			{
				UpdateMap();
			}

			if (InputManager.Modifiers == KeyModifiers.Control && InputManager.IsKeyPressed(Key.S))
			{
				Save();
			}
			else if (InputManager.Modifiers == KeyModifiers.Control && InputManager.IsKeyPressed(Key.O))
			{
				Load();
			}

			if (InputManager.IsKeyPressed(Key.Up))
			{
				UpdateSpeed++;
			}

			if (InputManager.IsKeyPressed(Key.Down))
			{
				UpdateSpeed--;
			}

			if (InputManager.IsKeyPressed(Key.Space))
			{
				Paused = !Paused;
			}

			if (InputManager.IsKeyPressed(Key.Tab))
			{
				ForceUpdate = true;
			}

			if (InputManager.IsKeyPressed(Key.Escape))
			{
				clipboard = null;
			}
		}

		public void Render()
		{
			if (Map != null)
			{
				RenderMap();
			}
		}

		private MouseMode GetMouseMode()
		{
			switch (InputManager.Modifiers)
			{
				case KeyModifiers.Shift: return MouseMode.Line;
				case KeyModifiers.Control: return MouseMode.Rectangle;
				case KeyModifiers.Alt: return MouseMode.Select;
				default: return MouseMode.Points;
			}
		}

		private Tile GetTileFromButtons(MouseButtons buttons)
		{
			switch (buttons)
			{
				case MouseButtons.Left: return Tile.Copper;
				case MouseButtons.Middle: return Tile.CopperHead;
				default: return Tile.Void;
			}
		}

		private void UpdateMap()
		{
			if (ShouldUpdate)
			{
				Map.Update();
				ForceUpdate = false;
			}

			if (downButton == MouseButtons.None && InputManager.MouseButtons != MouseButtons.None)
			{
				downButton = InputManager.MouseButtons;
				startPoint = MousePoint;
				mouseMode = GetMouseMode();
			}

			if (downButton != MouseButtons.None && InputManager.MouseButtons == MouseButtons.None)
			{
				switch (mouseMode)
				{
					case MouseMode.Line:
						DrawLine(startPoint, MousePoint, GetTileFromButtons(downButton));
						break;
					case MouseMode.Rectangle:
						DrawRectangle(startPoint, MousePoint, GetTileFromButtons(downButton));
						break;
					case MouseMode.Select:
						clipboard = SelectionRectangle.IsEmpty ? null : Map.CreateCopy(SelectionRectangle);
						break;
				}

				downButton = MouseButtons.None;
				mouseMode = MouseMode.Points;
			}

			if (mouseMode == MouseMode.Points)
			{
				if (clipboard != null)
				{
					if (InputManager.IsMouseButtonPressed(MouseButton.Left))
					{
						DrawClipboard(MousePoint.X, MousePoint.Y);
					}
				}
				else
				{
					switch (downButton)
					{
						case MouseButtons.None:
							break;
						case MouseButtons.Left:
							DrawLine(PreviousMousePoint, MousePoint, Tile.Copper);
							break;
						case MouseButtons.Right:
							DrawLine(PreviousMousePoint, MousePoint, Tile.Void);
							break;
						case MouseButtons.Middle:
							if (InputManager.IsMouseButtonPressed(MouseButton.Middle))
							{
								Map[MousePoint.X, MousePoint.Y] = Tile.CopperHead;
							}
							break;
						default:
							break;
					}
				}
			}
		}

		private void DrawClipboard(int x, int y)
		{
			Map.Paste(clipboard, x, y, merge: true);
		}

		private void DrawLine(Point start, Point end, Tile tile)
		{
			foreach (var point in Bresenham.GetPointsOnLine(start.X, start.Y, end.X, end.Y))
			{
				if (clipboard != null)
				{
					DrawClipboard(point.X, point.Y);
				}
				else
				{
					Map[point.X, point.Y] = tile;
				}
			}
		}

		private void DrawRectangle(Point start, Point end, Tile tile)
		{
			Rectangle selection = SelectionRectangle;

			for (int x = 0; x < selection.Width; x++)
			{
				for (int y = 0; y < selection.Height; y++)
				{
					if (clipboard != null)
					{
						DrawClipboard(x + selection.X, y + selection.Y);
					}
					else
					{
						Map[x + selection.X, y + selection.Y] = tile;
					}
				}
			}
		}
		
		private void RenderMap()
		{
			GL.LoadIdentity();

			GL.Scale(10, 10, 1);
			GL.Translate(new Vector3(-offset));

			Map.Render();

			switch (mouseMode)
			{
				case MouseMode.Line:
					RenderLine();
					break;
				case MouseMode.Rectangle:
				case MouseMode.Select:
					RenderRectangle();
					break;
				case MouseMode.Points:
					RenderClipboard(MousePoint.X, MousePoint.Y);
					break;
			}
		}

		private void RenderLine()
		{
			foreach (var point in Bresenham.GetPointsOnLine(startPoint.X, startPoint.Y, MousePoint.X, MousePoint.Y))
			{
				if (clipboard != null)
				{
					RenderClipboard(point.X, point.Y);
				}
				else
				{
					TileType.Copper.Render(Tile.Copper, point.X, point.Y, 0.5f);
				}
			}
		}

		private void RenderClipboard(int x, int y)
		{
			if (clipboard != null)
			{
				GL.PushMatrix();
				GL.Translate(x, y, 0);
				clipboard.Render();
				GL.PopMatrix();
			}
		}

		private void RenderRectangle()
		{
			Rectangle selection = SelectionRectangle;

			if (clipboard != null)
			{
				for (int x = 0; x < selection.Width; x++)
				{
					for (int y = 0; y < selection.Height; y++)
					{
						RenderClipboard(x + selection.X, y + selection.Y);
					}
				}
			}
			else
			{
				GL.Begin(PrimitiveType.Quads);

				GL.Color4(Color4.White.ChangeAlpha(0.5f));

				GL.Vertex2(selection.Left, selection.Top);
				GL.Vertex2(selection.Left, selection.Bottom);
				GL.Vertex2(selection.Right, selection.Bottom);
				GL.Vertex2(selection.Right, selection.Top);

				GL.End();
			}
		}

		public void Load()
		{
			if (path != null)
			{
				Map.Load(path);
			}
		}

		public void Save()
		{
			if (path != null)
			{
				Map.Save(path);
			}

			for (int i = 0; i < 1000; i++)
			{
				if (!File.Exists($"map{i}.wire"))
				{
					Map.Save($"map{i}.wire");
					break;
				}
			}
		}
	}
}
