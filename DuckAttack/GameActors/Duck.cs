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
using ExoGame2D.DuckAttack.GameActors.Hud;
using ExoGame2D.DuckAttack.GameStates.Controller;
using ExoGame2D.DuckAttack.Messages;
using ExoGame2D.Interfaces;
using ExoGame2D.Renderers;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace ExoGame2D.DuckAttack.GameActors
{
    public class Duck : IRenderNode, ICollidable
    {
        private readonly AnimatedSprite _duck;
        private readonly CollidableSprite _duckDeath;
        private readonly CollidableSprite _duckDive;

        private readonly Stopwatch _duckClock = new Stopwatch();
        private int _deathCounter = 0;
        private int _duckFlashCounter = 0;

        public Rectangle Crosshair { get; set; }
        public string Name { get; set; }
        public DuckStateEnum State { get; set; }
        public Rectangle BoundingBox => _duck.BoundingBox;
        public bool RenderBoundingBox
        {
            get { return _duck.RenderBoundingBox; }
            set { _duck.RenderBoundingBox = value; }
        }

        public Duck(string name, int x, int y)
        {
            Name = name;

            _duckDeath = new CollidableSprite("DuckDeath");
            _duckDeath.LoadContent("DuckDeath");

            _duckDive = new CollidableSprite("DuckDive");
            _duckDive.LoadContent("DuckDive");

            _duck = new AnimatedSprite(name);
            _duck.LoadFrameTexture("DuckFrame1");
            _duck.LoadFrameTexture("DuckFrame2");
            _duck.LoadFrameTexture("DuckFrame3");

            _duck.Velocity = new Vector2(7, 5);
            _duck.Flip = false;
            _duck.Location = new Vector2(x, y);
            _duck.IsVisible = true;
            _duck.IsEnabled = true;
            _duck.AnimationType = AnimationTypeEnum.PingPong;
            _duck.AnimationSpeed = 5;
            _duck.Play();

            State = DuckStateEnum.Start;

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
            _duck.LoadFrameTexture("DuckFrame1");
            _duck.LoadFrameTexture("DuckFrame2");
            _duck.LoadFrameTexture("DuckFrame3");

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
            _duck.AnimationType = AnimationTypeEnum.PingPong;
            _duck.AnimationSpeed = 5;
            _duck.Play();

            State = DuckStateEnum.Start;

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
                case DuckStateEnum.Start:
                    break;

                case DuckStateEnum.Fying:
                    _duck.Update(gameTime);
                    _duckClock.Start();

                    if (!_playDuckSound)
                    {
                        Channels.PostMessage("soundeffects", new SoundEffectMessage() { SoundEffectToPlay = "quaks" });
                        Channels.PostMessage("soundeffects", new SoundEffectMessage() { SoundEffectToPlay = "flapping" });

                        _playDuckSound = true;
                    }

                    if (_duckClock.ElapsedMilliseconds > 1000)
                    {
                        BoundDuckOffWalls();
                    }

                    if (_duckClock.ElapsedMilliseconds > 6000)
                    {
                        State = DuckStateEnum.FlyAway;
                    }

                    ShootDuck();
                    break;

                case DuckStateEnum.Dead:
                    _duckDeath.X = _duck.X;
                    _duckDeath.Y = _duck.Y;

                    _duckDive.X = _duck.X;
                    _duckDive.Y = _duck.Y;

                    break;

                case DuckStateEnum.FlyAway:
                    _duck.Update(gameTime);

                    if (DuckOutOfBounds(_duck))
                    {
                        State = DuckStateEnum.Finished;
                        DuckHitMessage duckHitMessage = new DuckHitMessage() { State = DuckIndicatorStateEnum.Miss };
                        Channels.PostMessage("duckhit", duckHitMessage);
                    }

                    ShootDuck();
                    break;

                case DuckStateEnum.Dive:
                    _duckDive.Update(gameTime);
                    _duckDive.Velocity = new Vector2(0, 15);

                    if (!_playFallingSound)
                    {
                        Channels.PostMessage("soundeffects", new SoundEffectMessage() { SoundEffectToPlay = "falling" });
                        _playFallingSound = true;
                    }

                    if (DuckOutOfBounds(_duckDive))
                    {
                        State = DuckStateEnum.Finished;
                    }
                    break;

                case DuckStateEnum.Finished:

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

                    DuckHitMessage duckHitMessage = new DuckHitMessage() { State = DuckIndicatorStateEnum.Hit };
                    Channels.PostMessage("duckhit", duckHitMessage);

                    Channels.PostMessage("soundeffects", new SoundEffectMessage() { SoundEffectToPlay = "death" });
                    State = DuckStateEnum.Dead;
                }
            }
        }

        private bool GetCollision()
        {
            if (DifficultySettings.CurrentDifficulty.AimAssist)
            {
                if (InputHelper.MouseLeftButtonPressed() && (CollisionManager.IsBoundingCollision("crosshair", Name)))
                    return true;
            }
            else
            {
                if (InputHelper.MouseLeftButtonPressed() && (CollisionManager.IsPerPixelCollision("crosshair", Name)))
                    return true;
            }

            return false;
        }

        private bool DuckOutOfBounds(ISprite sprite)
        {
            var engine = Engine.Instance;
            if (sprite.X + sprite.Width > engine.ScaledViewPort.X + 100)
            {
                return true;
            }

            if (sprite.X < -100)
            {
                return true;
            }

            if (sprite.Y + (sprite.Height) > engine.ScaledViewPort.Y)
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
            var engine = Engine.Instance;
            if (_duck.X + _duck.Width > engine.ScaledViewPort.X)
            {
                _duck.VX = -_duck.VX;
                _duck.Flip = true;
            }

            if (_duck.X < 0)
            {
                _duck.VX = -_duck.VX;
                _duck.Flip = false;
            }

            if (_duck.Y + (_duck.Height) > engine.ScaledViewPort.Y - 200)
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
                case DuckStateEnum.Start:
                    break;

                case DuckStateEnum.FlyAway:
                case DuckStateEnum.Fying:
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

                case DuckStateEnum.Dead:
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
                        State = DuckStateEnum.Dive;
                    }
                    break;

                case DuckStateEnum.Dive:
                    _duckDive.Draw(context, gameTime);
                    break;
            }
        }

        public bool GetPixelData(Rectangle intersection, out Color[] data)
            => _duck.GetPixelData(intersection, out data);
    }
}
