/*
MIT License

Copyright (c) 2020 stephenhaunts

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

using DuckAttack.GameActors;
using DuckAttack.MainMenuActors;
using FlexoGraphics;
using FlexoGraphics.Interfaces;
using FlexoGraphics.Renderers;
using FlexoGraphics.SceneManagement;
using FlexoGraphics.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DuckAttack.GameStates
{
    public class MainMenu : IGameState
    {
        private readonly Scene _scene = new Scene();
        private readonly FontRender _titles;
        private int _fontY;

        public MainMenu()
        {
            var crosshair = new MenuCursor();
            var background = new Background();

            MusicPlayer.LoadMusic("banjo");

            _fontY = -200;
            _titles = new FontRender("titlescreen")
            {
                Location = new Vector2(150, _fontY),
                Shadow = true,
                Text = "Duck Attack"
            };

            var container = new UIContainer();

            int startYPosition = 450;

            var playGameButton = new Button("PlayGameButton", new NewGameButtonHandler())
            {
                Width = 500,
                Height = 70,
                Location = new Vector2(700, startYPosition),
                Text = "Play Duck Attack",
                DrawControlChrome = true,
                ControlTextureName = "ButtonBackground"
            };

            startYPosition += 80;

            var optionsButton = new Button("OptionsGameButton", new OptionsButtonHandler())
            {
                Width = 500,
                Height = 70,
                Location = new Vector2(700, startYPosition),
                Text = "Options",
                DrawControlChrome = true,
                ControlTextureName = "ButtonBackground"
            };

            startYPosition += 80;

            var exitButton = new Button("ExitGameButton", new ExitButtonHandler())
            {
                Width = 500,
                Height = 70,
                Location = new Vector2(700, startYPosition),
                Text = "Exit Game",
                DrawControlChrome = true,
                ControlTextureName = "ButtonBackground"
            };

            container.AddControl(playGameButton);
            container.AddControl(optionsButton);
            container.AddControl(exitButton);

            _scene.Add(1, background);
            _scene.Add(2, _titles);
            _scene.Add(5, crosshair);
            _scene.Add(4, container);

            MusicPlayer.Play("banjo");
            MusicPlayer.Looped = true;
        }

        public void Remove()
            => CollisionManager.RemoveAll();

        public void Draw(DrawContext context, GameTime gameTime)
        {
            _fontY += 10;
            if (_fontY >= 200)
            {
                _fontY = 200;
            }

            _titles.Location = new Vector2(150, _fontY);

            _scene.Draw(context, gameTime);
        }

        public void Update(GameTime gameTime)
        {
            if (InputHelper.KeyPressed(Keys.Escape))
            {
                Engine.Instance.Exit();
            }

            _scene.Update(gameTime);
        }
    }
}
