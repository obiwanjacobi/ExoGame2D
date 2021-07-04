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
using DuckAttack.GameActors.Hud;
using DuckAttack.GameStates.Controller;
using FlexoGraphics;
using FlexoGraphics.Interfaces;
using FlexoGraphics.Renderers;
using FlexoGraphics.SceneManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace DuckAttack.GameStates
{
    public class PlayingGameState : IGameState
    {
        private readonly Scene _scene = new Scene();
        private Duck _duck1;
        private Duck _duck2;
        private Duck _duck3;
        private Duck _duck4;
        private readonly Stopwatch _gameClock = new Stopwatch();
        private readonly LevelController _levelController;
        private readonly Hud _hud;
        private bool Round1Triggered = false;
        private bool Round2Triggered = false;
        private bool Round3Triggered = false;
        private int _hudResetOffsetCounter = 0;
        private readonly BillBoard _billboard = new BillBoard(500, Color.Yellow);


        public PlayingGameState()
        {
            DifficultySettings.SetDifficulty(Difficulty.Easy);

            var crosshair = new Crosshair();
            var background = new Background();
            var fps = new FramesPerSecond("default")
            {
                Location = new Vector2(40, Engine.Instance.CoordinateSpace.World.Viewport.Height - 50)
            };
            var score = new ScoreBoard();
            _hud = new Hud("Hud");
            _levelController = new LevelController(new LevelBuilder().Levels);

            NextLevel();

            _scene.Add(RenderLayer.Layer4, _hud);
            _scene.Add(RenderLayer.Layer1, background);
            _scene.Add(RenderLayer.Layer4, fps);
            _scene.Add(RenderLayer.Layer4, score);
            _scene.Add(RenderLayer.Layer4, _billboard);
            _scene.Add(RenderLayer.Layer5, crosshair);

            Channels.Create("score");
            Channels.Create("duckhit");
            Channels.Create("gunfired");

            _billboard.StartBillBoard("Level " + _levelController.LevelNumber);
        }

        private void NextLevel()
        {
            var level = _levelController.CurrentLevel;

            if (_hud.NumberOfDucksShot == 12)
            {
                _hud.ResetDuckCounter();
                _levelController.NextLevel();
                level = _levelController.CurrentLevel;
                _billboard.StartBillBoard("Level " + _levelController.LevelNumber);
            }

            level.Ducks.Shuffle();

            _duck1 = new Duck("duck", level.Ducks[0].StartX, level.Ducks[0].Flip, level.Ducks[0].HorizontalVelocity, level.Ducks[0].VerticalVelocity);
            _duck2 = new Duck("duck2", level.Ducks[1].StartX, level.Ducks[1].Flip, level.Ducks[1].HorizontalVelocity, level.Ducks[1].VerticalVelocity);
            _duck3 = new Duck("duck3", level.Ducks[2].StartX, level.Ducks[2].Flip, level.Ducks[2].HorizontalVelocity, level.Ducks[2].VerticalVelocity);
            _duck4 = new Duck("duck4", level.Ducks[3].StartX, level.Ducks[3].Flip, level.Ducks[3].HorizontalVelocity, level.Ducks[3].VerticalVelocity);

            _scene.Remove(RenderLayer.Layer2, _duck1);
            _scene.Remove(RenderLayer.Layer2, _duck2);
            _scene.Remove(RenderLayer.Layer2, _duck3);
            _scene.Remove(RenderLayer.Layer2, _duck4);

            CollisionManager.Remove(_duck1.Name);
            CollisionManager.Remove(_duck2.Name);
            CollisionManager.Remove(_duck3.Name);
            CollisionManager.Remove(_duck4.Name);

            _scene.Add(RenderLayer.Layer2, _duck1);
            _scene.Add(RenderLayer.Layer2, _duck2);
            _scene.Add(RenderLayer.Layer2, _duck3);
            _scene.Add(RenderLayer.Layer2, _duck4);

            _gameClock.Reset();
            _gameClock.Start();

            Round1Triggered = false;
            Round2Triggered = false;
            Round3Triggered = false;

            _hud.ResetGun();
        }

        public void Remove()
        {
            CollisionManager.RemoveAll();
        }

        public void Draw(DrawContext context, GameTime gameTime)
            => _scene.Draw(context, gameTime);

        public void Update(GameTime gameTime)
        {
            if (InputHelper.KeyPressed(Keys.Escape))
            {
                var engine = Engine.Instance;
                engine.GameState.CurrentState.Remove();
                engine.GameState.Register("Playing", new PlayingGameState());

                engine.GameState.Register("MainMenu", new MainMenu());
                engine.GameState.ChangeState("MainMenu");
            }

            if ((_hud.NumberOfDucksShot == 4) && (!Round1Triggered))
            {
                _hudResetOffsetCounter++;

                if (_hudResetOffsetCounter == 50)
                {
                    NextLevel();
                    Round1Triggered = true;
                    _hudResetOffsetCounter = 0;
                }
            }

            if ((_hud.NumberOfDucksShot == 8) && (!Round2Triggered))
            {
                _hudResetOffsetCounter++;

                if (_hudResetOffsetCounter == 50)
                {
                    NextLevel();
                    Round2Triggered = true;
                    _hudResetOffsetCounter = 0;
                }
            }

            if ((_hud.NumberOfDucksShot == 12) && (!Round3Triggered))
            {
                _hudResetOffsetCounter++;

                if (_hudResetOffsetCounter == 50)
                {
                    NextLevel();
                    Round3Triggered = true;
                    _hudResetOffsetCounter = 0;
                }
            }

            var level = _levelController.CurrentLevel;

            ProcessDuckState(_duck1, level.Duck1StartTimerOffset);
            ProcessDuckState(_duck2, level.Duck2StartTimerOffset);
            ProcessDuckState(_duck3, level.Duck3StartTimerOffset);
            ProcessDuckState(_duck4, level.Duck4StartTimerOffset);

            _scene.Update(gameTime);
        }

        private void ProcessDuckState(Duck duck, int timerOffset)
        {
            if (duck.State == DuckState.Start &&
                _gameClock.ElapsedMilliseconds > timerOffset)
            {
                duck.State = DuckState.Fying;
            }
        }
    }
}
