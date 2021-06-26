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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ExoGame2D.Renderers
{
    public class FontRender : IRenderNode
    {
        private Vector2 _location = new Vector2(0, 0);
        private SpriteFont _font;

        public string Name { get; set; }
        public string Text { get; set; }
        public bool Shadow { get; set; }

        public FontRender(string name)
        {
            Name = name;
        }

        public Vector2 Location
        {
            get => _location;
            set => _location = value;
        }

        public void LoadContent(string fontName)
        {
            _font = Engine.Instance.Content.Load<SpriteFont>(fontName);
        }

        public void Draw(DrawContext context, GameTime gameTime)
        {
            if (!string.IsNullOrEmpty(Text))
            {
                if (Shadow)
                {
                    Vector2 shadowOffset = new Vector2(_location.X - 2, _location.Y + 2);
                    context.SpriteBatch.DrawString(_font, Text, shadowOffset, Color.Black);
                }

                var tint = Color.White;
                context.SpriteBatch.DrawString(_font, Text, _location, tint);
            }
        }

        public void Update(GameTime gameTime)
        {
            // no-op
        }
    }
}
