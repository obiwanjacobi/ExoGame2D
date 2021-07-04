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

using FlexoGraphics.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FlexoGraphics.Renderers
{
    public class Sprite : IRenderNode, IBoundingBox, INamedObject
    {
        private Vector2 _location = new Vector2(0, 0);
        private Vector2 _velocity = new Vector2(0, 0);
        private float _x;
        private float _y;
        private float _velocityX;
        private float _velocityY;

        public Sprite(string name = "")
        {
            Name = name;
            IsEnabled = true;
            IsVisible = true;
        }

        public bool IsVisible { get; set; }
        public bool IsEnabled { get; set; }

        public bool RenderBoundingBox { get; set; }

        public string Name { get; protected set; }

        public float X
        {
            get => _x;
            set
            {
                _x = value;
                _location.X = value;
            }
        }

        public float Y
        {
            get => _y;
            set
            {
                _y = value;
                _location.Y = value;
            }
        }

        public float VX
        {
            get => _velocityX;
            set
            {
                _velocityX = value;
                _velocity.X = value;
            }
        }

        public float VY
        {
            get => _velocityY;
            set
            {
                _velocityY = value;
                _velocity.Y = value;
            }
        }

        public Vector2 Location
        {
            get => _location;
            set
            {
                _location = value;
                X = _location.X;
                Y = _location.Y;
            }
        }

        public Vector2 Velocity
        {
            get => _velocity;
            set
            {
                _velocity = value;
                _velocityX = _velocity.X;
                _velocityY = _velocity.Y;
            }
        }

        public int Width => Texture.Width;

        public int Height => Texture.Height;

        public Rectangle Dimensions => Texture.Bounds;

        public Rectangle BoundingBox => new Rectangle((int)X, (int)Y, Texture.Width, Texture.Height);

        public bool Flip { get; set; } = false;

        public Color Tint { get; set; } = Color.White;

        protected Texture2D Texture { get; set; }

        public void LoadContent(string textureName)
        {
            Texture = Engine.Instance.Content.Load<Texture2D>(textureName
                ?? throw new ArgumentNullException(nameof(textureName)));
        }

        public virtual void Update(GameTime gameTime)
        {
            if (IsEnabled)
            {
                X += VX * (gameTime.ElapsedGameTime.Milliseconds / 16);
                Y += VY * (gameTime.ElapsedGameTime.Milliseconds / 16);
            }
        }

        public virtual void Draw(DrawContext context, GameTime gameTime)
        {
            if (Texture == null || !IsVisible)
                return;

            var effect = Flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            context.SpriteBatch.Draw(Texture, _location,
                new Rectangle(0, 0, Texture.Width, Texture.Height), Tint, 0,
                Vector2.Zero, 1.0f, effect, 0.0f);

            if (RenderBoundingBox)
                context.SpriteBatch.DrawRectangle(BoundingBox, Color.Yellow, 3);
        }
    }
}
