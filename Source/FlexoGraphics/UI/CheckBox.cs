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
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FlexoGraphics.UI
{
    public class CheckBox : UIControl
    {
        private readonly SpriteFont _font;
        private readonly ButtonHandler _handler;
        private int _checkBoxWidth;

        public string Text { get; set; }
        public Color TextColor { get; set; }
        public Color MouseOverTextColor { get; set; }
        public Color MouseOverCheckBoxColor { get; set; }
        public bool IsChecked { get; set; }

        public CheckBox(string name, ButtonHandler handler)
            : base(name)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
            _font = Engine.Instance.Content.Load<SpriteFont>("default");

            _checkBoxWidth = 20;
            Width = 70;
            Height = 20;
            IsChecked = true;
            Location = new Vector2(100, 100);
            IsEnabled = true;
            IsVisible = true;
            Text = "My Button";
            Color = Color.LightGray;
            OutlineColor = Color.Gray;
            MouseOverColor = Color.DimGray;
            MouseOverCheckBoxColor = Color.LightGray;
            TextColor = Color.Black;
            MouseOverTextColor = Color.DarkKhaki;
            DrawControlChrome = true;
        }

        public override void Update(GameTime gameTime)
        {
            // We want to make sure the check box area is square so make the width equal the height.
            _checkBoxWidth = Height;

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
            if (IsMouseOver)
            {
                context.SpriteBatch.FillRectangle(Location.X, Location.Y, _checkBoxWidth, Height, MouseOverColor);
                context.SpriteBatch.FillRectangle(Location.X + _checkBoxWidth, Location.Y, Width - _checkBoxWidth, Height, MouseOverCheckBoxColor);
            }
            else
            {
                context.SpriteBatch.FillRectangle(Location.X, Location.Y, _checkBoxWidth, Height, Color);
                context.SpriteBatch.FillRectangle(Location.X + _checkBoxWidth, Location.Y, Width - _checkBoxWidth, Height, Color);
            }

            context.SpriteBatch.DrawRectangle(Location, new Vector2(Width, Height), OutlineColor, 2);
        }

        protected override void DrawContent(DrawContext context, GameTime gameTime)
        {
            if (!string.IsNullOrEmpty(ControlTextureName))
            {
                var color = IsMouseOver ? MouseOverColor : Color.White;
                context.SpriteBatch.Draw(ControlTexture, Location,
                    new Rectangle(0, 0, ControlTexture.Width, ControlTexture.Height), color, 0.0f,
                    new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.0f);
            }

            if (IsChecked)
            {
                // \
                context.SpriteBatch.DrawLine(
                    new Vector2((int)Location.X + 4, (int)Location.Y),
                    new Vector2((int)(Location.X + _checkBoxWidth), (int)Location.Y + Height - 2), OutlineColor, 6);
                // /
                context.SpriteBatch.DrawLine(
                    new Vector2((int)Location.X + 2 + _checkBoxWidth, (int)Location.Y + 4),
                    new Vector2((int)(Location.X + 2), (int)Location.Y + Height + 2), OutlineColor, 6);
            }

            context.SpriteBatch.DrawRectangle(Location, new Vector2(_checkBoxWidth, Height), OutlineColor, 2);

            var bounds = new Rectangle((int)(Location.X + _checkBoxWidth), (int)Location.Y, (Width - _checkBoxWidth), Height);
            context.DrawString(_font, Text, bounds, Alignment.Center, IsMouseOver ? MouseOverTextColor : TextColor);
        }
    }
}
