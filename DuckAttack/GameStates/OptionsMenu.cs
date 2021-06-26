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
using ExoGame2D.DuckAttack.GameActors;
using ExoGame2D.DuckAttack.MainMenuActors;
using ExoGame2D.Interfaces;
using ExoGame2D.Renderers;
using ExoGame2D.SceneManagement;
using ExoGame2D.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ExoGame2D.DuckAttack.GameStates
{
    public class OptionsMenu : IGameState
    {
        private readonly Scene _scene = new Scene();

        private readonly Background _background;
        private readonly MenuCursor _crosshair;
        private readonly FrameCounter _frameCounter = new FrameCounter();
        private readonly FontRender _titles;
        private int _fontY;

        private UIContainer _container;
        private IRenderNode _backToMainMenu;
        private IRenderNode _soundEffectsOnOff;

        public OptionsMenu()
        {
            _crosshair = new MenuCursor();
            _background = new Background();

            _titles = new FontRender("Titles");
            _titles.LoadContent("titlescreen");
            _titles.Location = new Vector2(150, _fontY);
            _titles.Shadow = true;


            _container = new UIContainer("OptionsMenu");

            int startYPosition = 600;

            _soundEffectsOnOff = new CheckBox("SoundEffectsOnOff", new SoundEffectsOnOffHandler())
            {
                Width = 500,
                Height = 70,
                Location = new Vector2(700, startYPosition),
                Text = "Sound Effects On/Off",
                DrawWindowChrome = true,
                ControlTextureName = "ButtonBackground"
            };

            ((CheckBox)_soundEffectsOnOff).Checked = SoundEffectPlayer.PlaySoundEffects;

            startYPosition += 80;

            _backToMainMenu = new Button("ExitToMainMenu", new ExitToMainMenuButtonHandler())
            {
                Width = 500,
                Height = 70,
                Location = new Vector2(700, startYPosition),
                Text = "<-- Exit to Main Menu",
                DrawWindowChrome = true,
                ControlTextureName = "ButtonBackground"
            };


            _container.AddControl(_soundEffectsOnOff);
            _container.AddControl(_backToMainMenu);

            _scene.AddSpriteToLayer(RenderLayerEnum.Layer1, _background);
            _scene.AddSpriteToLayer(RenderLayerEnum.Layer2, _titles);
            _scene.AddSpriteToLayer(RenderLayerEnum.Layer5, _crosshair);
            _scene.AddSpriteToLayer(RenderLayerEnum.Layer4, _container);
        }

        public void Remove()
        {
            CollisionManager.RemoveAll();
        }

        public void Draw(GameTime gametime)
        {
            Draw(gametime, Color.White);
        }

        public void Draw(GameTime gametime, Color tint)
        {
            _frameCounter.Update((float)gametime.ElapsedGameTime.TotalSeconds);

            var engine = Engine.Instance;
            engine.BeginRender(_scene);

            _titles.Text = "Duck Attack";

            _titles.Location = new Vector2(150, 200);

            _scene.RenderScene(gametime);

            engine.EndRender();
        }

        public void Update(GameTime gametime)
        {
            if (InputHelper.KeyPressed(Keys.Escape))
            {
                Engine.Instance.Exit();
            }

            _scene.UpdateGameLogic(gametime);
        }
    }
}
