#region --- License ---
/* Licensed under the MIT/X11 license.
 * Copyright (c) 2006-2008 the OpenTK Team.
 * This notice may not be removed from any source distribution.
 * See license.txt for licensing detailed licensing details.
 */
#endregion

#region --- Using Directives ---

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

#endregion

namespace G
{
    /// <summary>
    /// Demonstrates immediate mode rendering.
    /// </summary>
    public class T03_Immediate_Mode_Cube : GameWindow
    {
        #region --- Fields ---
        Vector3 up = new Vector3(0.0f, 0.0f, 1.0f);
        Camera _Camera;
        List<Color> colors = new List<Color>();
        Random rand = new Random();
        #endregion

        #region --- Constructor ---

        public T03_Immediate_Mode_Cube()
            : base(800, 600, new GraphicsMode(16, 16))
        { }

        #endregion

        #region OnLoad

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GetColors();
            GL.ClearColor(Color.Black);
            GL.Enable(EnableCap.DepthTest);
            this._Camera = new Camera();
            this.Mouse.WheelChanged += Mouse_WheelChanged;
        }

        /// <summary>
        /// Handles change in zoom using the mouse wheel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Mouse_WheelChanged(object sender, OpenTK.Input.MouseWheelEventArgs e)
        {
            this._Camera.ZoomVelocity -= 5.0 * e.DeltaPrecise;
        }

        #endregion

        #region OnResize

        /// <summary>
        /// Called when the user resizes the window.
        /// </summary>
        /// <param name="e">Contains the new width/height of the window.</param>
        /// <remarks>
        /// You want the OpenGL viewport to match the window. This is the place to do it!
        /// </remarks>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Width, Height);
        }

        #endregion

        #region OnUpdateFrame

        /// <summary>
        /// Prepares the next frame for rendering.
        /// </summary>
        /// <remarks>
        /// Place your control logic here. This is the place to respond to user input,
        /// update object positions etc.
        /// </remarks>
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            this._Camera.Update(e.Time);
            if (this.Focused)
            {
                Cursor.Hide();
                if (!this._Camera.IsResetting) //Disable mouse control if the camera's orientation is being reset.
                {
                    Point center = new Point(this.Bounds.Left + this.Bounds.Width / 2,
                                             this.Bounds.Top + this.Bounds.Height / 2);
                    Point cursor = Cursor.Position;
                    int dX = cursor.X - center.X;
                    int dY = cursor.Y - center.Y;
                    Cursor.Position = center;
                    this._Camera.Theta -= ((double)dX / 500.0);
                    this._Camera.Phi -= ((double)dY / 500.0);
                    this._Camera.Phi = Math.Min(Math.PI / 2.0000001, Math.Max(Math.PI / -2.000001, this._Camera.Phi));
                }
                Console.Clear();
                Console.WriteLine("Theta: {0}\nPhi: {1}\nZoom: {2}\nRoll: {3}", this._Camera.Theta, this._Camera.Phi, this._Camera.Radius, this._Camera.Roll % 360.0);
                if (Keyboard[OpenTK.Input.Key.Escape])
                {
                    this.Exit();
                    return;
                }
                if(Keyboard[OpenTK.Input.Key.Q])
                {
                    this._Camera.RollVelocity += Camera.RollDelta;
                }
                if(Keyboard[OpenTK.Input.Key.E])
                {
                    this._Camera.RollVelocity -= Camera.RollDelta;
                }
                if(Keyboard[OpenTK.Input.Key.Space])
                {
                    this._Camera.Reset();
                }
            }
            else Cursor.Show();
        }

        #endregion

        #region OnRenderFrame

        /// <summary>
        /// Place your rendering code here.
        /// </summary>
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            double aspectRatio = Width / (double)Height;

            OpenTK.Matrix4 perspective = 
                OpenTK.Matrix4.CreatePerspectiveFieldOfView((float)(Math.PI / 2.0), (float)aspectRatio, 0.1f, 1000);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perspective);

            Matrix4 lookat = Matrix4.LookAt(this._Camera.Position, Vector3.Zero, up);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref lookat);
            GL.Rotate(this._Camera.Roll, (Vector3d)this._Camera.Position);

            DrawCube();

            this.SwapBuffers();
        }

        #endregion

        #region DrawCube()
        private void GetColors()
        {
            foreach (string colorName in Enum.GetNames(typeof(KnownColor)))
            {
                KnownColor knownColor = (KnownColor)Enum.Parse(typeof(KnownColor), colorName);
                if (knownColor > KnownColor.Transparent)
                {
                    this.colors.Add(Color.FromKnownColor(knownColor));
                }
            }
        }
        private Color RandomColor()
        {
            return colors[(rand.Next(colors.Count))];
        }
        private void DrawCube()
        {
            Vector3 nnn = new Vector3(-1.0f, -1.0f, -1.0f);
            Vector3 nnp = new Vector3(-1.0f, -1.0f, 1.0f);
            Vector3 npn = new Vector3(-1.0f, 1.0f, -1.0f);
            Vector3 npp = new Vector3(-1.0f, 1.0f, 1.0f);
            Vector3 pnn = new Vector3(1.0f, -1.0f, -1.0f);
            Vector3 pnp = new Vector3(1.0f, -1.0f, 1.0f);
            Vector3 ppn = new Vector3(1.0f, 1.0f, -1.0f);
            Vector3 ppp = new Vector3(1.0f, 1.0f, 1.0f);

            //GL.ClearColor(RandomColor());
            GL.Begin(BeginMode.Quads);
            GL.Color3(Color.Yellow);
            GL.Vertex3(nnn);
            GL.Vertex3(npn);
            GL.Vertex3(ppn);
            GL.Vertex3(pnn);

            GL.Color3(Color.OrangeRed);
            GL.Vertex3(nnn);
            GL.Vertex3(pnn);
            GL.Vertex3(pnp);
            GL.Vertex3(nnp);

            GL.Color3(Color.Green);
            GL.Vertex3(nnn);
            GL.Vertex3(nnp);
            GL.Vertex3(npp);
            GL.Vertex3(npn);

            GL.Color3(Color.White);
            GL.Vertex3(nnp);
            GL.Vertex3(pnp);
            GL.Vertex3(ppp);
            GL.Vertex3(npp);

            GL.Color3(Color.Red);
            GL.Vertex3(npn);
            GL.Vertex3(npp);
            GL.Vertex3(ppp);
            GL.Vertex3(ppn);

            GL.Color3(Color.Blue);
            GL.Vertex3(pnn);
            GL.Vertex3(ppn);
            GL.Vertex3(ppp);
            GL.Vertex3(pnp);

            GL.End();
        }
        #endregion
    }
}
