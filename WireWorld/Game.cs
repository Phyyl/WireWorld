using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WireWorld
{
	public class Game
	{
		public Map Map { get; private set; }

		private int update = 0;
		private int updateSpeed = 4;
		private float scale = 10;
		private Vector2 offset;

		public bool Paused { get; set; }
		public bool ForceUpdate { get; set; }

		public int UpdateSpeed
		{
			get => updateSpeed;
			set => updateSpeed = MathHelper.Clamp(value, 1, 6);
		}

		private int MouseX => (int)(InputManager.MousePosition.X / scale);
		private int MouseY => (int)(InputManager.MousePosition.Y / scale);
		private int PreviousMouseX => (int)(InputManager.MousePreviousPosition.X / scale);
		private int PreviousMouseY => (int)(InputManager.MousePreviousPosition.Y / scale);
		private Point MousePoint => new Point(MouseX, MouseY);
		private Point PreviousMousePoint => new Point(PreviousMouseX, PreviousMouseY);

		private bool ShouldUpdate => ForceUpdate || (!Paused && (update % MathHelper.Clamp(Math.Pow(2, 6 - updateSpeed), 1, 60) == 0));

		public void CreateNewMap(int width, int height, bool confirmSave = false)
		{
			Map = new Map(width, height);
		}

		public void Update(object delta)
		{
			update++;

			if (Map != null)
			{
				UpdateMap();
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
			
		}

		public void Render()
		{
			if (Map != null)
			{
				RenderMap();
			}
		}

		private void UpdateMap()
		{
			if (ShouldUpdate)
			{
				Map.Update();
				ForceUpdate = false;
			}

			if (InputManager.IsMouseButtonDown(MouseButton.Left))
			{
				if (InputManager.WasMouseButtonDown(MouseButton.Left))
				{
					DrawLine(PreviousMousePoint, MousePoint, Tile.Copper);
				}
				else
				{
					DrawLine(MousePoint, MousePoint, Tile.Copper);
				}
			}
			else if (InputManager.IsMouseButtonDown(MouseButton.Right))
			{
				if (InputManager.WasMouseButtonDown(MouseButton.Right))
				{
					DrawLine(PreviousMousePoint, MousePoint, Tile.Void);
				}
				else
				{
					DrawLine(MousePoint, MousePoint, Tile.Void);
				}
			}
			else if (InputManager.IsMouseButtonPressed(MouseButton.Middle))
			{
				if (Map[MouseX, MouseY].ID == TileType.Copper.ID)
				{
					Map[MouseX, MouseY] = Tile.CopperHead;
				}
			}
		}

		private void DrawLine(Point start, Point end, Tile tile)
		{
			if (start == end)
			{
				Map[start.X, start.Y] = tile;
			}
			else
			{
				foreach (var point in Bresenham.GetPointsOnLine(start.X, start.Y, end.X, end.Y))
				{
					Map[point.X, point.Y] = tile;
				}
			}
		}

		private void RenderMap()
		{
			GL.LoadIdentity();

			GL.Scale(10, 10, 1);
			GL.Translate(new Vector3(-offset));

			Map.Render();
		}

		public void LoadGame(string path)
		{
			//TODO: Load map from file, if path is null, show open file window (*.wire)
		}

		public void SaveGame(string path)
		{
			//TODO: Save map to file, if path is null, show save file window (*.wire)
		}
	}
}
