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
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ExoGame2D
{
    public class Engine
    {
        private readonly GraphicsDeviceManager _graphics;
        private readonly Game _game;

        public Engine(Game game)
        {
            if (Instance != null)
                throw new InvalidOperationException("Create only one instance of the Engine class.");

            _game = game ?? throw new ArgumentNullException(nameof(game));
            _graphics = new GraphicsDeviceManager(game);
            game.Content.RootDirectory = "Content";
            game.IsMouseVisible = true;
            Instance = this;

            _game.Window.ClientSizeChanged += Window_ClientSizeChanged;
        }

        public DrawContext DrawContext { get; private set; }
        public readonly GameStateManager GameState = new GameStateManager();
        public static Engine Instance { get; private set; }
        public ContentManager Content => _game.Content;
        public CoordinateSpace CoordinateSpace { get; private set; }

        public void Initialize()
        {
            Initialize(1920, 1080, 1920, 1080);
        }

        public void Initialize(int windowX, int windowY)
        {
            Initialize(windowX, windowY, windowX, windowY);
        }

        public void Initialize(int windowX, int windowY, int worldX, int worldY)
        {
            if (_game.GraphicsDevice == null)
                throw new InvalidOperationException("Call Engine.Initialize() after Game.Initialize().");

            DrawContext = new DrawContext(_game.GraphicsDevice);
            CoordinateSpace = new CoordinateSpace(worldX, worldY);
            CoordinateSpace.ResizeDevice(windowX, windowY);
            SetFullScreen(false);
        }

        public void Exit()
        {
            _game.Exit();
        }

        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            return CoordinateSpace.DeviceToWorld(screenPosition);
        }

        public bool IsFullScreen => _graphics.IsFullScreen;

        public void SetFullScreen(bool fullScreen = true)
        {
            ApplyResolutionSettings(fullScreen);
        }

        /// <summary>
        /// Scales the window to the desired size, and calculates how the game world should be scaled to fit inside that window.
        /// </summary>
        private void ApplyResolutionSettings(bool fullScreen)
        {
            _isInResize = true;
            // make the game full-screen or not
            _graphics.IsFullScreen = fullScreen;

            var deviceRect = fullScreen
                ? CoordinateSpace.FullScreen
                : CoordinateSpace.Device.Bounds;

            // scale the window to the desired size
            _graphics.PreferredBackBufferWidth = deviceRect.Width;
            _graphics.PreferredBackBufferHeight = deviceRect.Height;
            _graphics.ApplyChanges();

            _game.GraphicsDevice.Viewport = new Viewport(CoordinateSpace.Device.Viewport);

            SetTransformation();
            _isInResize = false;
        }

        private void SetTransformation()
        {
            if (DrawContext != null)
                DrawContext.Transformation = CoordinateSpace.WorldToDeviceScale();
        }

        private bool _isInResize;
        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            if (!_isInResize)
            {
                CoordinateSpace.ResizeDevice(_game.Window.ClientBounds.Width, _game.Window.ClientBounds.Height);
                ApplyResolutionSettings(IsFullScreen);
            }
        }
    }
}
