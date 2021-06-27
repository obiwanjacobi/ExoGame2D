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

using ExoGame2D.Renderers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ExoGame2D.UI
{
    public class Button : UIControl
    {
        private readonly SpriteFont _font;
        private readonly ButtonHandler _handler;

        public string Text { get; set; }
        public Color TextColor { get; set; }
        public Color MouseOverTextColor { get; set; }

        public Button(string name, ButtonHandler handler)
            : base(name)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
            _font = Engine.Instance.Content.Load<SpriteFont>("default");

            Width = 50;
            Height = 20;
            Location = new Vector2(100, 100);
            IsEnabled = true;
            IsVisible = true;
            Text = "My Button";
            Color = Color.LightGray;
            OutlineColor = Color.Gray;
            MouseOverColor = Color.Gray;
            TextColor = Color.Black;
            MouseOverTextColor = Color.Khaki;
            DrawControlChrome = true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (IsMouseOver)
            {
                _handler.OnMouseOver(this);

                if (InputHelper.MouseLeftButtonPressed())
                {
                    _handler.OnMouseClick(this);
                }
            }
        }

        protected override void DrawChrome(DrawContext context, GameTime gameTime)
        {
            if (DrawControlChrome)
            {
                if (IsMouseOver)
                {
                    context.SpriteBatch.FillRectangle(Location.X, Location.Y, Width, Height, MouseOverColor);
                }
                else
                {
                    context.SpriteBatch.FillRectangle(Location.X, Location.Y, Width, Height, Color);
                }

                context.SpriteBatch.DrawRectangle(Location, new Vector2(Width, Height), OutlineColor, 2);
            }
        }

        protected override void DrawContent(DrawContext context, GameTime gameTime)
        {
            if (!string.IsNullOrEmpty(ControlTextureName))
            {
                if (IsMouseOver)
                {
                    context.SpriteBatch.Draw(ControlTexture, Location,
                        new Rectangle(0, 0, ControlTexture.Width, ControlTexture.Height), MouseOverColor, 0.0f,
                        new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.0f);
                }
                else
                {
                    context.SpriteBatch.Draw(ControlTexture, Location,
                        new Rectangle(0, 0, ControlTexture.Width, ControlTexture.Height), Color.White, 0.0f,
                        new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.0f);
                }
            }

            if (IsMouseOver)
            {
                Rectangle bounds = new Rectangle((int)Location.X, (int)Location.Y, Width, Height);
                context.DrawString(_font, Text, bounds, Alignment.Center, MouseOverTextColor);
            }
            else
            {
                Rectangle bounds = new Rectangle((int)Location.X, (int)Location.Y, Width, Height);
                context.DrawString(_font, Text, bounds, Alignment.Center, TextColor);
            }
        }
    }
}
