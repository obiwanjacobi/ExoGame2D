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
using ExoGame2D.DuckAttack.GameStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ExoGame2D.DuckAttack
{
    public class GameLoop : Game
    {
        public GameLoop()
        {
            Engine.Instance.InitializeEngine(this, 1920, 1080);
            //Engine.InitializeEngine(this, 1280, 720);

            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            var engine = Engine.Instance;
            engine.GameState.Register("MainMenu", new MainMenu());

            engine.GameState.ChangeState("Mainmenu");
            Window.AllowUserResizing = true;
        }

        protected override void Update(GameTime gameTime)
        {
            InputHelper.Update();

            var engine = Engine.Instance;
            if (InputHelper.KeyPressed(Keys.F))
            {
                engine.FullScreen = !engine.FullScreen;
            }

            engine.GameState.CurrentState.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            var engine = Engine.Instance;
            Engine.Instance.GameState.CurrentState.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}