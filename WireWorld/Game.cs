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
		private int updateSpeed = 6;
		private float scale = 10;
		private Vector2 offset;

		public bool Paused { get; set; }
		public bool ForceUpdate { get; set; }

		public int UpdateSpeed
		{
			get => updateSpeed;
			set => updateSpeed = MathHelper.Clamp(value, 1, 6);
		}

		private int MouseX => (int)(InputManager.Position.X / scale);
		private int MouseY => (int)(InputManager.Position.Y / scale);

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
				Map[MouseX, MouseY] = Tile.Copper;
			}
			else if (InputManager.IsMouseButtonDown(MouseButton.Right))
			{
				Map[MouseX, MouseY] = Tile.Void;
			}
			else if (InputManager.IsMouseButtonDown(MouseButton.Middle))
			{
				if (Map[MouseX, MouseY].ID == TileType.Copper.ID)
				{
					Map[MouseX, MouseY] = Tile.CopperHead;
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
