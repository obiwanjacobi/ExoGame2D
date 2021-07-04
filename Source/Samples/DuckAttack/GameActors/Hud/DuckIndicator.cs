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
using FlexoGraphics.Renderers;
using Microsoft.Xna.Framework;

namespace DuckAttack.GameActors.Hud
{
    public class DuckIndicator : IRenderNode
    {
        private readonly Sprite _duckhud;
        private readonly Sprite _duckmiss;
        private readonly Sprite _duckshot;

        private Sprite _currentSprite;
        private DuckIndicatorState _state;

        public DuckIndicator(string name, int x)
        {
            Name = name;
            _duckhud = new CollidableSprite();
            _duckhud.LoadContent("duckhud");

            _duckmiss = new CollidableSprite();
            _duckmiss.LoadContent("duckhudmiss");

            _duckshot = new CollidableSprite();
            _duckshot.LoadContent("duckhudshot");

            _currentSprite = _duckhud;
            State = DuckIndicatorState.None;

            var y = 950;
            _duckhud.X = x;
            _duckhud.Y = y;

            _duckmiss.X = x;
            _duckmiss.Y = y;

            _duckshot.X = x;
            _duckshot.Y = y;
        }

        public string Name { get; set; }

        public DuckIndicatorState State
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
                    case DuckIndicatorState.None:
                        _currentSprite = _duckhud;
                        break;

                    case DuckIndicatorState.Miss:
                        _currentSprite = _duckmiss;
                        break;

                    case DuckIndicatorState.Hit:
                        _currentSprite = _duckshot;
                        break;
                }
            }
        }

        public void Update(GameTime gameTime)
        { }

        public void Draw(DrawContext context, GameTime gameTime)
            => _currentSprite.Draw(context, gameTime);
    }
}
