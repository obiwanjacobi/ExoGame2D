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
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace ExoGame2D.Renderers
{
    public class AnimatedSprite : Sprite, ICollidable
    {
        private readonly List<Texture2D> _animationFrames = new List<Texture2D>();
        private int _currentFrame = 0;
        private bool _pingPong = false;
        private int _internalTimer = 0;

        public AnimationTypeEnum AnimationType { get; set; }
        public AnimationPlayingStateEnum PlayState { get; private set; }
        public int AnimationSpeed { get; set; }

        public AnimatedSprite(string name = "") : base()
        {
            Name = name;
            AnimationType = AnimationTypeEnum.Linear;
            PlayState = AnimationPlayingStateEnum.Stopped;
            AnimationSpeed = 10;
        }

        public void LoadFrameTexture(string textureName)
        {
            if (string.IsNullOrEmpty(textureName))
            {
                throw new ArgumentNullException(nameof(textureName));
            }

            _animationFrames.Add(Engine.Instance.Content.Load<Texture2D>(textureName));

            _currentFrame = 0;
            _texture = _animationFrames[_currentFrame];
        }

        public void ResetAnimation()
        {
            _currentFrame = 0;
            _texture = _animationFrames[_currentFrame];
            _pingPong = false;
        }

        public void NextFrame()
        {
            switch (AnimationType)
            {
                case AnimationTypeEnum.Linear:
                    if (_currentFrame < _animationFrames.Count - 1)
                    {
                        _currentFrame++;
                        _texture = _animationFrames[_currentFrame];
                    }
                    else
                    {
                        _currentFrame = 0;
                        _texture = _animationFrames[_currentFrame];
                    }
                    break;

                case AnimationTypeEnum.PingPong:
                    switch (_pingPong)
                    {
                        case false:
                            if (_currentFrame < _animationFrames.Count - 1)
                            {
                                _currentFrame++;
                                _texture = _animationFrames[_currentFrame];
                            }
                            else
                            {
                                _pingPong = true;
                            }
                            break;
                        case true:
                            if (_currentFrame > 0)
                            {
                                _currentFrame--;
                                _texture = _animationFrames[_currentFrame];
                            }
                            else
                            {
                                _pingPong = false;
                            }
                            break;
                    }
                    break;
            }
        }

        public bool GetPixelData(Rectangle intersection, out Color[] data)
        {
            var length = Width * Height;
            data = new Color[length];
            _texture.GetData(0, intersection, data, 0, length);
            return true;
        }

        public void Play()
        {
            PlayState = AnimationPlayingStateEnum.Playing;
        }

        public void Stop()
        {
            PlayState = AnimationPlayingStateEnum.Stopped;
        }

        public override void Update(GameTime gameTime)
        {
            if (PlayState == AnimationPlayingStateEnum.Playing)
            {
                _internalTimer++;

                if (_internalTimer >= AnimationSpeed)
                {
                    NextFrame();
                    _internalTimer = 0;
                }
            }

            base.Update(gameTime);
        }
    }
}
