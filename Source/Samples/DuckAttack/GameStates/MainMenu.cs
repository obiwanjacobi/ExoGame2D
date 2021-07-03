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

        private readonly Background _background;
        private readonly MenuCursor _crosshair;
        private readonly FrameCounter _frameCounter = new FrameCounter();
        private readonly FontRender _titles;
        private int _fontY;

        private readonly UIContainer _container;
        private readonly UIControl _playGameButton;
        private readonly UIControl _optionsButton;
        private readonly UIControl _exitButton;

        public MainMenu()
        {
            _crosshair = new MenuCursor();
            _background = new Background();

            MusicPlayer.LoadMusic("banjo");

            _fontY = -200;
            _titles = new FontRender("Titles");
            _titles.LoadContent("titlescreen");
            _titles.Location = new Vector2(150, _fontY);
            _titles.Shadow = true;


            _container = new UIContainer("MainMenu");

            int startYPosition = 450;

            _playGameButton = new Button("PlayGameButton", new NewGameButtonHandler())
            {
                Width = 500,
                Height = 70,
                Location = new Vector2(700, startYPosition),
                Text = "Play Duck Attack",
                DrawControlChrome = true,
                ControlTextureName = "ButtonBackground"
            };

            startYPosition += 80;

            _optionsButton = new Button("OptionsGameButton", new OptionsButtonHandler())
            {
                Width = 500,
                Height = 70,
                Location = new Vector2(700, startYPosition),
                Text = "Options",
                DrawControlChrome = true,
                ControlTextureName = "ButtonBackground"
            };

            startYPosition += 80;

            _exitButton = new Button("ExitGameButton", new ExitButtonHandler())
            {
                Width = 500,
                Height = 70,
                Location = new Vector2(700, startYPosition),
                Text = "Exit Game",
                DrawControlChrome = true,
                ControlTextureName = "ButtonBackground"
            };

            _container.AddControl(_playGameButton);
            _container.AddControl(_optionsButton);
            _container.AddControl(_exitButton);

            _scene.Add(RenderLayer.Layer1, _background);
            _scene.Add(RenderLayer.Layer2, _titles);
            _scene.Add(RenderLayer.Layer5, _crosshair);
            _scene.Add(RenderLayer.Layer4, _container);

            MusicPlayer.Play("banjo");
            MusicPlayer.Looped = true;
        }

        public void Remove()
            => CollisionManager.RemoveAll();

        public void Draw(DrawContext context, GameTime gameTime)
        {
            _frameCounter.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            _fontY += 10;
            if (_fontY >= 200)
            {
                _fontY = 200;
            }

            _titles.Text = "Duck Attack";
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
