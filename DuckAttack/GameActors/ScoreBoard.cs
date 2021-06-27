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
using ExoGame2D.DuckAttack.Messages;
using ExoGame2D.Renderers;
using Microsoft.Xna.Framework;

namespace ExoGame2D.DuckAttack.GameActors
{
    public class ScoreBoard : IRenderNode
    {
        private readonly FontRender _scoreboard;
        private int _score;

        public ScoreBoard(string name)
        {
            Name = name;
            _scoreboard = new FontRender(name);
            _scoreboard.LoadContent("headsup");
            _scoreboard.Location = new Vector2(40, 40);
            _score = 0;
            _scoreboard.Shadow = true;
        }

        public string Name { get; set; }

        public void Draw(DrawContext context, GameTime gameTime)
            => _scoreboard.Draw(context, gameTime);

        public void Update(GameTime gameTime)
        {
            if (Channels.Exists("score"))
            {
                var message = Channels.LastMessageAs<ScoreMessage>("score");

                if (message != null)
                {
                    _score += message.ScoreIncrement;
                }
            }

            _scoreboard.Text = "Score = " + _score;
        }
    }
}
