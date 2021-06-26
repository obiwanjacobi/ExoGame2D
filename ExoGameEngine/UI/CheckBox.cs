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

using ExoGame2D.Renderers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ExoGame2D.UI
{
    public class CheckBox : UIControlBase, IRenderNode
    {
        private readonly SpriteFont _font;
        private readonly ICheckBoxHandler _handler;
        private int _checkBoxWidth;

        public string Text { get; set; }
        public Color TextColor { get; set; }
        public Color MouseOverTextColor { get; set; }
        public Color CheckBoxMouseOverColor { get; set; }
        public bool Checked { get; set; }

        public CheckBox(string name, ICheckBoxHandler handler)
            : base(name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
            _checkBoxWidth = 70;
            _font = Engine.Instance.Content.Load<SpriteFont>("default");

            Width = 70;
            Height = 20;
            Checked = true;
            Location = new Vector2(100, 100);
            Enabled = true;
            Visible = true;
            Text = "My Button";
            Color = Color.LightGray;
            OutlineColor = Color.Gray;
            MouseOverColor = Color.DimGray;
            CheckBoxMouseOverColor = Color.LightGray;
            TextColor = Color.Black;
            MouseOverTextColor = Color.DarkKhaki;
            DrawWindowChrome = true;
        }

        public void Update(GameTime gameTime)
        {
            var mouse = Engine.Instance.ScreenToWorld(new Vector2(InputHelper.MousePosition.X, InputHelper.MousePosition.Y));
            var mouseCursor = new Rectangle((int)mouse.X, (int)mouse.Y, 1, 1);
            var bounds = new Rectangle((int)Location.X, (int)Location.Y, _checkBoxWidth, Height);

            if (bounds.Intersects(mouseCursor))
            {
                MouseOver = true;

                _handler.OnMouseOver(this);

                if (InputHelper.MouseLeftButtonPressed())
                {
                    _handler.OnMouseClick(this);
                }
            }
            else
            {
                MouseOver = false;
            }
        }

        public void Draw(GameTime gameTime)
        {
            Draw(gameTime, Color.White);
        }

        public void Draw(GameTime gameTime, Color tint)
        {
            if (!Visible)
            {
                return;
            }

            // We want to make sure the check box area is square so make the width equal the height.
            _checkBoxWidth = Height;

            if (DrawWindowChrome)
            {
                if (MouseOver)
                {
                    SpriteBatch.FillRectangle(Location.X, Location.Y, _checkBoxWidth, Height, MouseOverColor);
                    SpriteBatch.FillRectangle(Location.X + _checkBoxWidth, Location.Y, Width - _checkBoxWidth, Height, CheckBoxMouseOverColor);
                }
                else
                {
                    SpriteBatch.FillRectangle(Location.X, Location.Y, _checkBoxWidth, Height, Color);
                    SpriteBatch.FillRectangle(Location.X + _checkBoxWidth, Location.Y, Width - _checkBoxWidth, Height, Color);

                }

                SpriteBatch.DrawRectangle(Location, new Vector2(Width, Height), OutlineColor, 2);
            }

            if (!string.IsNullOrEmpty(ControlTextureName))
            {
                if (MouseOver)
                {
                    SpriteBatch.Draw(ControlTexture, Location,
                        new Rectangle(0, 0, ControlTexture.Width, ControlTexture.Height), MouseOverColor, 0.0f,
                        new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.0f);
                }
                else
                {
                    SpriteBatch.Draw(ControlTexture, Location,
                        new Rectangle(0, 0, ControlTexture.Width, ControlTexture.Height), Color.White, 0.0f,
                        new Vector2(0, 0), 1.0f, SpriteEffects.None, 0.0f);
                }
            }

            if (Checked)
            {
                SpriteBatch.DrawLine(new Vector2((int)Location.X, (int)Location.Y), new Vector2((int)(Location.X + _checkBoxWidth), (int)Location.Y + Height), OutlineColor, 2);
                SpriteBatch.DrawLine(new Vector2((int)Location.X + 1 + _checkBoxWidth, (int)Location.Y + 1), new Vector2((int)(Location.X), (int)Location.Y + Height), OutlineColor, 2);

            }

            SpriteBatch.DrawRectangle(Location, new Vector2(_checkBoxWidth, Height), OutlineColor, 2);

            var bounds = new Rectangle((int)(Location.X + _checkBoxWidth), (int)Location.Y, (Width - _checkBoxWidth), Height);

            if (MouseOver)
            {
                DrawString(_font, Text, bounds, AlignmentEnum.Center, MouseOverTextColor);
            }
            else
            {
                DrawString(_font, Text, bounds, AlignmentEnum.Center, TextColor);
            }
        }

        public ISprite GetSprite()
        {
            throw new NotImplementedException();
        }

        public bool IsAssetOfType(Type type)
        {
            return GetType() == type;
        }
    }
}
