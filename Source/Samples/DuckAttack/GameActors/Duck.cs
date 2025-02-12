﻿/*
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

using DuckAttack.GameActors.Hud;
using DuckAttack.GameStates.Controller;
using DuckAttack.Messages;
using FlexoGraphics;
using FlexoGraphics.Interfaces;
using FlexoGraphics.Renderers;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace DuckAttack.GameActors
{
    public class Duck : IRenderNode, ICollidable
    {
        private const int AnimationSpeed = 5;
        private readonly AnimatedSprite _duck;
        private readonly Sprite _duckDeath;
        private readonly Sprite _duckDive;

        private readonly Stopwatch _duckClock = new Stopwatch();
        private int _deathCounter = 0;
        private int _duckFlashCounter = 0;

        public Rectangle Crosshair { get; set; }
        public string Name { get; set; }
        public DuckState State { get; set; }
        public Rectangle BoundingBox => _duck.BoundingBox;

        public bool RenderBoundingBox
        {
            get { return _duck.RenderBoundingBox; }
            set { _duck.RenderBoundingBox = value; }
        }

        public Duck(string name, int x, int y)
        {
            Name = name;

            _duckDeath = new Sprite("DuckDeath");
            _duckDeath.LoadContent("DuckDeath");

            _duckDive = new Sprite("DuckDive");
            _duckDive.LoadContent("DuckDive");

            _duck = new AnimatedSprite(name);
            _duck.LoadFrameTexture("DuckFrame1", AnimationSpeed);
            _duck.LoadFrameTexture("DuckFrame2", AnimationSpeed);
            _duck.LoadFrameTexture("DuckFrame3", AnimationSpeed);

            _duck.Velocity = new Vector2(7, 5);
            _duck.Flip = false;
            _duck.Location = new Vector2(x, y);
            _duck.IsVisible = true;
            _duck.IsEnabled = true;
            _duck.Animation.Type = AnimationType.PingPong;
            _duck.Animation.Play();

            State = DuckState.Start;

            SoundEffectPlayer.LoadSoundEffect("death");
            SoundEffectPlayer.LoadSoundEffect("quaks");
            SoundEffectPlayer.LoadSoundEffect("falling");
            SoundEffectPlayer.LoadSoundEffect("flapping");
        }

        public Duck(string name, int x, bool flip, float vx, float vy)
        {
            Name = name;

            _duckDeath = new CollidableSprite("DuckDeath");
            _duckDeath.LoadContent("DuckDeath");

            _duckDive = new CollidableSprite("DuckDive");
            _duckDive.LoadContent("DuckDive");

            _duck = new AnimatedSprite(name);
            _duck.LoadFrameTexture("DuckFrame1", AnimationSpeed);
            _duck.LoadFrameTexture("DuckFrame2", AnimationSpeed);
            _duck.LoadFrameTexture("DuckFrame3", AnimationSpeed);

            if (vx > 0)
            {
                vx += DifficultySettings.CurrentDifficulty.SpeedDifference;
            }
            else
            {
                vx -= DifficultySettings.CurrentDifficulty.SpeedDifference;
            }

            if (vy > 0)
            {
                vy += DifficultySettings.CurrentDifficulty.SpeedDifference;
            }
            else
            {
                vy -= DifficultySettings.CurrentDifficulty.SpeedDifference;
            }

            _duck.Velocity = new Vector2(vx, vy);
            _duck.Flip = flip;
            _duck.Location = new Vector2(x, 900);
            _duck.IsVisible = true;
            _duck.IsEnabled = true;
            _duck.Animation.Type = AnimationType.PingPong;
            _duck.Animation.Play();

            State = DuckState.Start;

            SoundEffectPlayer.LoadSoundEffect("death");
            SoundEffectPlayer.LoadSoundEffect("quaks");
            SoundEffectPlayer.LoadSoundEffect("falling");
            SoundEffectPlayer.LoadSoundEffect("flapping");
        }

        private bool _playDuckSound = false;
        private bool _playFallingSound = false;

        public void Update(GameTime gameTime)
        {
            switch (State)
            {
                case DuckState.Start:
                    break;

                case DuckState.Fying:
                    _duck.Update(gameTime);
                    _duckClock.Start();

                    if (!_playDuckSound)
                    {
                        Channels.PostMessage(SoundEffectPlayer.ChannelName, new SoundEffectMessage() { SoundEffectToPlay = "quaks" });
                        Channels.PostMessage(SoundEffectPlayer.ChannelName, new SoundEffectMessage() { SoundEffectToPlay = "flapping" });

                        _playDuckSound = true;
                    }

                    if (_duckClock.ElapsedMilliseconds > 1000)
                    {
                        BoundDuckOffWalls();
                    }

                    if (_duckClock.ElapsedMilliseconds > 6000)
                    {
                        State = DuckState.FlyAway;
                    }

                    ShootDuck();
                    break;

                case DuckState.Dead:
                    _duckDeath.X = _duck.X;
                    _duckDeath.Y = _duck.Y;

                    _duckDive.X = _duck.X;
                    _duckDive.Y = _duck.Y;

                    break;

                case DuckState.FlyAway:
                    _duck.Update(gameTime);

                    if (DuckOutOfBounds(_duck))
                    {
                        State = DuckState.Finished;
                        DuckHitMessage duckHitMessage = new DuckHitMessage() { State = DuckIndicatorState.Miss };
                        Channels.PostMessage("duckhit", duckHitMessage);
                    }

                    ShootDuck();
                    break;

                case DuckState.Dive:
                    _duckDive.Update(gameTime);
                    _duckDive.Velocity = new Vector2(0, 15);

                    if (!_playFallingSound)
                    {
                        Channels.PostMessage(SoundEffectPlayer.ChannelName, new SoundEffectMessage() { SoundEffectToPlay = "falling" });
                        _playFallingSound = true;
                    }

                    if (DuckOutOfBounds(_duckDive))
                    {
                        State = DuckState.Finished;
                    }
                    break;

                case DuckState.Finished:

                    break;
            }
        }

        private void ShootDuck()
        {
            // Shoot the duck
            if (GetCollision())
            {
                if (Hud.Hud.NumShotsLeft > 0)
                {
                    // Post message to the score board.
                    ScoreMessage message = new ScoreMessage() { ScoreIncrement = 10 };
                    Channels.PostMessage("score", message);

                    DuckHitMessage duckHitMessage = new DuckHitMessage() { State = DuckIndicatorState.Hit };
                    Channels.PostMessage("duckhit", duckHitMessage);

                    Channels.PostMessage(SoundEffectPlayer.ChannelName, new SoundEffectMessage() { SoundEffectToPlay = "death" });
                    State = DuckState.Dead;
                }
            }
        }

        private bool GetCollision()
        {
            if (DifficultySettings.CurrentDifficulty.AimAssist)
            {
                if (InputHelper.MouseLeftButtonPressed() &&
                    (CollisionManager.IsBoundingCollision("crosshair", Name)))
                    return true;
            }
            else
            {
                if (InputHelper.MouseLeftButtonPressed() &&
                    (CollisionManager.IsBoundingCollision("crosshair", Name)) &&
                    (CollisionManager.IsPerPixelCollision("crosshair", Name)))
                    return true;
            }

            return false;
        }

        private bool DuckOutOfBounds(Sprite sprite)
        {
            var worldViewport = Engine.Instance.CoordinateSpace.World.Viewport;

            if (sprite.X + sprite.Width > worldViewport.Width + 100)
            {
                return true;
            }

            if (sprite.X < -100)
            {
                return true;
            }

            if (sprite.Y + (sprite.Height) > worldViewport.Height)
            {
                return true;
            }

            if (sprite.Y < -100)
            {
                return true;
            }

            return false;
        }

        private void BoundDuckOffWalls()
        {
            var worldViewport = Engine.Instance.CoordinateSpace.World.Viewport;

            if (_duck.X + _duck.Width > worldViewport.Width)
            {
                _duck.VX = -_duck.VX;
                _duck.Flip = true;
            }

            if (_duck.X < 0)
            {
                _duck.VX = -_duck.VX;
                _duck.Flip = false;
            }

            if (_duck.Y + (_duck.Height) > worldViewport.Height - 200)
            {
                _duck.VY = -_duck.VY;
            }

            if (_duck.Y < 0)
            {
                _duck.VY = -_duck.VY;
            }
        }

        private readonly static Color AlternateFlyTint = new Color(100, 100, 100, 255);
        private readonly static Color AlternateDeathTint = new Color(150, 150, 150, 255);

        public void Draw(DrawContext context, GameTime gameTime)
        {
            switch (State)
            {
                case DuckState.Start:
                    break;

                case DuckState.FlyAway:
                case DuckState.Fying:
                    if (CollisionManager.IsBoundingCollision("crosshair", Name))
                    {
                        _duck.Tint = AlternateFlyTint;
                    }
                    else
                    {
                        _duck.Tint = Color.White;
                    }
                    _duck.Draw(context, gameTime);
                    break;

                case DuckState.Dead:
                    _deathCounter++;

                    if (_deathCounter >= 5)
                    {
                        _duckDeath.Tint = AlternateDeathTint;
                    }
                    else
                    {
                        _duckDeath.Tint = Color.White;
                    }
                    _duckDeath.Draw(context, gameTime);

                    if (_deathCounter >= 10)
                    {
                        _deathCounter = 0;
                        _duckFlashCounter++;
                    }

                    if (_duckFlashCounter == 5)
                    {
                        State = DuckState.Dive;
                    }
                    break;

                case DuckState.Dive:
                    _duckDive.Draw(context, gameTime);
                    break;
            }
        }

        public bool GetPixelData(Rectangle intersection, out Color[] data)
            => _duck.GetPixelData(intersection, out data);
    }
}
