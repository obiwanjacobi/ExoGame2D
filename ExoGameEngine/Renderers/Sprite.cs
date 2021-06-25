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
using ExoGame2D.Interfaces;
using Microsoft.Xna.Framework;
using System;

namespace ExoGame2D.Renderers
{
    public class Sprite : SpriteBase, ISprite
    {
        public Sprite() : base()
        { }

        public Sprite(string name) : base()
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
        }

        public bool CollidesWith(ISprite sprite)
        {
            if (sprite is Sprite)
            {
                return PerPixelCollision(this, (Sprite)sprite);
            }

            if (sprite is AnimatedSprite)
            {
                return PerPixelCollision(this, (AnimatedSprite)sprite);
            }

            return false;
        }

        public new void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public new void Draw(GameTime gameTime)
        {
            base.Draw(gameTime, Color.White);
        }

        public new void Draw(GameTime gameTime, Color tint)
        {
            base.Draw(gameTime, tint);
        }

        public ISprite GetSprite()
        {
            return this;
        }

        public bool IsAssetOfType(Type type)
        {
            return GetType() == type;
        }
    }
}
