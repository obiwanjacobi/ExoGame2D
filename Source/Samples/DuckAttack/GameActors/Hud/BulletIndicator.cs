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
using FlexoGraphics.Renderers;
using Microsoft.Xna.Framework;

namespace DuckAttack.GameActors.Hud
{
    public class BulletIndicator : IRenderNode
    {
        public string Name { get; set; }
        private readonly Sprite _bulletFired;
        private readonly Sprite _bulletNotFired;

        private Sprite _currentSprite;

        BulletIndicatorState _state;
        public BulletIndicatorState State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
                switch (_state)
                {
                    case BulletIndicatorState.Fired:
                        _currentSprite = _bulletFired;
                        break;

                    case BulletIndicatorState.NotFired:
                        _currentSprite = _bulletNotFired;
                        break;
                }
            }
        }

        public BulletIndicator(string name, int x)
        {
            Name = name;
            _bulletFired = new Sprite();
            _bulletFired.LoadContent("bulletspent");

            _bulletNotFired = new Sprite();
            _bulletNotFired.LoadContent("bullet");

            _currentSprite = _bulletFired;
            State = BulletIndicatorState.NotFired;

            var y = 950;
            _bulletFired.X = x;
            _bulletFired.Y = y;
            _bulletNotFired.X = x;
            _bulletNotFired.Y = y;
        }

        public void Update(GameTime gameTime)
        {
            // no-op
        }

        public void Draw(DrawContext context, GameTime gameTime)
            => _currentSprite.Draw(context, gameTime);
    }
}
