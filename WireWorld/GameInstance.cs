using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace WireWorld
{
    public class GameInstance : GameWindow
    {
        private Game game;

        public GameInstance(string path = null)
            : base(1024, 768, CreateGraphicsMode())
        {
            game = new Game(path);

			if (path != null)
			{
				game.Load();
			}
			else
			{
				game.CreateNewMap(400, 400);
			}
        }

        private static GraphicsMode CreateGraphicsMode()
        {
            GraphicsMode @default = GraphicsMode.Default;
            return new GraphicsMode(@default.ColorFormat, 24, @default.Stencil, 8, @default.AccumulatorFormat, @default.Buffers, @default.Stereo);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            InputManager.Initialize(this);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            Matrix4 projectionMatrix = Matrix4.CreateOrthographicOffCenter(0, ClientSize.Width, ClientSize.Height, 0, 1, -1);

            GL.Viewport(0, 0, ClientSize.Width, ClientSize.Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.LoadMatrix(ref projectionMatrix);
            GL.MatrixMode(MatrixMode.Modelview);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            InputManager.BeginUpdate();
            base.OnUpdateFrame(e);
            
            game.Update((float)e.Time);

            InputManager.EndUpdate();

			Title = $"WireWorld ({(game.Paused ? "Paused, " : "")}Update speed: {game.UpdateSpeed})";
		}

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            game.Render();

            SwapBuffers();
        }
    }
}
