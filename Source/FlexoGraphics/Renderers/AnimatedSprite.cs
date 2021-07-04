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

namespace FlexoGraphics.Renderers
{
    public class AnimatedSprite : Sprite, ICollidable
    {
        private readonly TextureAnimation _animation = new TextureAnimation();

        public AnimatedSprite(string name = "") : base()
        {
            Name = name;
        }

        public TextureAnimation Animation => _animation;

        public void LoadFrameTexture(string textureName, int length)
        {
            _animation.LoadFrameTexture(textureName, length);
            Texture = _animation.Texture;
        }

        public bool GetPixelData(Rectangle intersection, out Color[] data)
        {
            var length = Width * Height;
            data = new Color[length];
            Texture.GetData(data);
            // TODO: fixme
            //_texture.GetData(0, intersection, data, 0, length);
            return true;
        }

        public override void Draw(DrawContext context, GameTime gameTime)
        {
            Texture = _animation.Texture;
            base.Draw(context, gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            _animation.Update(gameTime);
            base.Update(gameTime);
        }
    }
}
