using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace FlexoGraphics.Renderers
{
    public class TextureAnimation
    {
        private readonly List<AnimationFrame> _animationFrames = new List<AnimationFrame>();
        private int _currentFrame = 0;
        private int _internalTimer = 0;

        public Texture2D Texture
            => _animationFrames.Count > 0 ? _animationFrames[_currentFrame].Texture : null;

        public AnimationType Type { get; set; }
        public AnimationPlayingState PlayState { get; private set; }

        public TextureAnimation()
        {
            _currentFrame = 0;
            Type = AnimationType.Loop;
            PlayState = AnimationPlayingState.Stopped;
        }

        public void LoadFrameTexture(string textureName, int length = 10)
        {
            var texture = Engine.Instance.Content.Load<Texture2D>(textureName
                ?? throw new ArgumentNullException(nameof(textureName)));
            _animationFrames.Add(new AnimationFrame(texture, length));
        }

        public void Reset()
        {
            _currentFrame = 0;
            Stop();
        }

        public void Play()
        {
            PlayState = AnimationPlayingState.Playing;
        }

        public void Stop()
        {
            PlayState = AnimationPlayingState.Stopped;
            _internalTimer = 0;
        }

        public void Update(GameTime gameTime)
        {
            if (_animationFrames.Count == 0)
                return;

            if (PlayState == AnimationPlayingState.Playing)
            {
                _internalTimer++;

                if (_internalTimer >= _animationFrames[_currentFrame].Length)
                {
                    NextFrame();
                    _internalTimer = 0;
                }
            }
        }

        private bool _reverse;

        private void NextFrame()
        {
            switch (Type)
            {
                case AnimationType.Loop:
                    if (_currentFrame < _animationFrames.Count - 1)
                    {
                        _currentFrame++;
                    }
                    else
                    {
                        _currentFrame = 0;
                    }
                    break;

                case AnimationType.PingPong:
                    if (_reverse)
                    {
                        if (_currentFrame > 0)
                        {
                            _currentFrame--;
                        }
                        else
                        {
                            _reverse = false;
                        }
                    }
                    else
                    {
                        if (_currentFrame < _animationFrames.Count - 1)
                        {
                            _currentFrame++;
                        }
                        else
                        {
                            _reverse = true;
                        }
                    }
                    break;
            }
        }

        private struct AnimationFrame
        {
            public AnimationFrame(Texture2D texture, int length)
            {
                Texture = texture;
                Length = length;
            }

            public int Length;
            public Texture2D Texture;
        }
    }
}
