﻿/*
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
using System;

namespace ExoGame2D.Tutorials.Tutorial6_CollisionDetection
{
    public class GameLoop : Game
    {
        private readonly Scene _scene;
        private readonly CollidableSprite _crosshair;
        private readonly CollidableSprite _logo;
        private readonly Engine _engine;

        public GameLoop()
        {
            _engine = new Engine(this);
            _scene = new Scene();
            _logo = new CollidableSprite("Logo");
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
            _logo.LoadContent("ExoEngineLogo");
            _logo.Location = new Vector2(100, 100);

            _scene.AddSpriteToLayer(RenderLayerEnum.Layer1, _logo);
            _scene.AddSpriteToLayer(RenderLayerEnum.Layer5, _crosshair);
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
                var engine = Engine.Instance;
                engine.SetFullScreen(!engine.IsFullScreen);
            }

            if (CollisionManager.IsPerPixelCollision("Crosshair", "Logo"))
                Console.WriteLine("COLLIDE");
            else
                Console.WriteLine("NOT COLLIDE");

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            var engine = Engine.Instance;
            engine.BeginRender();

            var mouse = engine.ScreenToWorld(new Vector2(InputHelper.MousePosition.X, InputHelper.MousePosition.Y));
            _crosshair.Location = new Vector2(mouse.X - _crosshair.Width / 2, mouse.Y - _crosshair.Height / 2);

            _scene.RenderScene(gameTime);
            base.Draw(gameTime);

            engine.EndRender();
        }
    }
}