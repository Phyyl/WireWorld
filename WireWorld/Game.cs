﻿using OpenTK;
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
    public class Game : GameWindow
    {
        private GameInstance instance;
        private int scale = 10;
        private int update = 0;
        private int updateSpeed = 1;
        private bool paused;
        private bool forceUpdate;

        public Game() : base(800, 600)
        {
            instance = new GameInstance(400, 200);
            
            UpdateTitle();
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
        
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            HandleMouseEvent(e);
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);

            HandleMouseEvent(e);
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Key == Key.Up)
            {
                updateSpeed = Clamp(updateSpeed + 1, 1, 6);
                UpdateTitle();
            }
            else if (e.Key == Key.Down)
            {
                updateSpeed = Clamp(updateSpeed - 1, 1, 6);
                UpdateTitle();
            }
            else if (e.Key == Key.Space)
            {
                paused = !paused;
                UpdateTitle();
            }
            else if (e.Key == Key.Tab)
            {
                forceUpdate = true;
            }
        }

        private void HandleMouseEvent(MouseEventArgs e)
        {
            if (!e.Mouse.IsAnyButtonDown)
            {
                return;
            }

            byte value = (byte)(e.Mouse.LeftButton == ButtonState.Pressed ? 1 : (e.Mouse.MiddleButton == ButtonState.Pressed ? 2 : 0));

            int x = e.Position.X / scale;
            int y = e.Position.Y / scale;

            instance[x, y] = value;
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            int updateMod = Clamp((int)Math.Pow(2, 6 - updateSpeed), 1, 60);

            if (forceUpdate || (!paused && updateSpeed > 0 && update++ % updateMod == 0))
            {
                instance.Iterate();
            }

            forceUpdate = false;
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.Begin(PrimitiveType.Quads);

            for (int x = 0; x < instance.Width; x++)
            {
                for (int y = 0; y < instance.Height; y++)
                {
                    byte value = instance[x, y];

                    if (value == 0) continue;

                    if (value == 1) GL.Color4(Color4.Orange);
                    if (value == 2) GL.Color4(Color4.White);
                    if (value == 3) GL.Color4(Color4.Cyan);

                    GL.Vertex2(x * scale, y * scale);
                    GL.Vertex2(x * scale, y * scale + scale);
                    GL.Vertex2(x * scale + scale, y * scale + scale);
                    GL.Vertex2(x * scale + scale, y * scale);
                }
            }

            GL.End();

            SwapBuffers();
        }

        private void UpdateTitle()
        {
            Title = $"WireWorld ({(paused ? "Paused, " : "")}Update speed: {updateSpeed})";
        }

        private static int Clamp(int x, int min, int max) => x < min ? min : (x > max ? max : x);
    }
}
