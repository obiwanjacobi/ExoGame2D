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

using ExoGame2D.Interfaces;
using ExoGame2D.Renderers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ExoGame2D.UI
{
    public abstract class UIControl : IRenderNode, INamedObject
    {
        protected UIControl(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public string Name { get; }

        public int Width { get; set; }
        public int Height { get; set; }
        public Vector2 Location { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsVisible { get; set; }
        public Color Color { get; set; }
        public Color OutlineColor { get; set; }
        public Color MouseOverColor { get; set; }
        public bool DrawControlChrome { get; set; }
        public bool IsMouseOver { get; set; }

        protected Texture2D ControlTexture { get; set; }

        private string _controlTextureName;
        public string ControlTextureName
        {
            get { return _controlTextureName; }
            set
            {
                ControlTexture = Engine.Instance.Content.Load<Texture2D>(value);
                _controlTextureName = value;
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            var mouse = Engine.Instance.CoordinateSpace.DeviceToWorld(InputHelper.MousePosition.X, InputHelper.MousePosition.Y);
            var mouseCursor = new Rectangle((int)mouse.X, (int)mouse.Y, 1, 1);
            var bounds = new Rectangle((int)Location.X, (int)Location.Y, Width, Height);

            IsMouseOver = bounds.Intersects(mouseCursor);
        }

        public virtual void Draw(DrawContext context, GameTime gameTime)
        {
            if (IsVisible)
            {
                if (DrawControlChrome)
                    DrawChrome(context, gameTime);

                DrawContent(context, gameTime);
            }
        }

        protected abstract void DrawContent(DrawContext context, GameTime gameTime);
        protected abstract void DrawChrome(DrawContext context, GameTime gameTime);
    }
}
