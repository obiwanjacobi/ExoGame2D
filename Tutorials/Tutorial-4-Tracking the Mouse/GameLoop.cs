/*
MIT License

Copyright (c) 2020

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
using ExoGame2D.Renderers;
using ExoGame2D.SceneManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ExoGame2D.Tutorials.Tutorial4_TrackingTheMouse
{
    public class GameLoop : Game
    {
        private readonly Scene _scene;
        private readonly CollidableSprite _crosshair;
        private readonly Engine _engine;

        public GameLoop()
        {
            _engine = new Engine(this);
            _scene = new Scene();
            _crosshair = new CollidableSprite("Crosshair");
        }

        protected override void Initialize()
        {
            base.Initialize();
            _engine.Initialize();
        }

        protected override void LoadContent()
        {
            Window.AllowUserResizing = true;

            _crosshair.LoadContent("crosshair");

            _scene.Add(RenderLayer.Layer5, _crosshair);
        }

        protected override void Update(GameTime gameTime)
        {
            InputHelper.Update();

            if (InputHelper.KeyPressed(Keys.Escape))
            {
                Exit();
            }

            if (InputHelper.KeyPressed(Keys.F))
            {
                _engine.SetFullScreen(!_engine.IsFullScreen);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _engine.DrawContext.BeginDraw();

            var mouse = _engine.CoordinateSpace.DeviceToWorld(InputHelper.MousePosition.X, InputHelper.MousePosition.Y);
            _crosshair.Location = new Vector2(mouse.X - _crosshair.Width / 2, mouse.Y - _crosshair.Height / 2);

            _scene.Draw(_engine.DrawContext, gameTime);
            _engine.DrawContext.EndDraw();
        }
    }
}