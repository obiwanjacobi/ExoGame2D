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
            => Initialize(1920, 1080, 1920, 1080);

        public void Initialize(int windowWidth, int windowHeight)
            => Initialize(windowWidth, windowHeight, windowWidth, windowHeight);

        public void Initialize(int windowWidth, int windowHeight, int worldWidth, int worldHeight)
        {
            if (_game.GraphicsDevice == null)
                throw new InvalidOperationException("Call Engine.Initialize() after Game.Initialize().");

            DrawContext = new DrawContext(_game.GraphicsDevice);
            CoordinateSpace = new CoordinateSpace(worldWidth, worldHeight);
            CoordinateSpace.ResizeDevice(windowWidth, windowHeight);
            SetFullScreen(false);
        }

        public void Exit()
        {
            _game.Exit();
        }

        public bool IsFullScreen => _graphics.IsFullScreen;

        /// <summary>
        /// Scales the window to the desired size and calculates how the game world should be scaled.
        /// </summary>
        public void SetFullScreen(bool fullScreen = true)
        {
            _isInResize = true;

            var deviceRect = fullScreen
                ? CoordinateSpace.FullScreen
                : CoordinateSpace.Device.Bounds;

            _graphics.IsFullScreen = fullScreen;
            _graphics.PreferredBackBufferWidth = deviceRect.Width;
            _graphics.PreferredBackBufferHeight = deviceRect.Height;
            _graphics.ApplyChanges();
            _graphics.GraphicsDevice.Viewport = new Viewport(CoordinateSpace.Device.Viewport);

            if (DrawContext != null)
                DrawContext.Transformation = CoordinateSpace.WorldToDeviceScale();

            _isInResize = false;
        }

        private bool _isInResize;
        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            if (!_isInResize)
            {
                CoordinateSpace.ResizeDevice(_game.Window.ClientBounds.Width, _game.Window.ClientBounds.Height);
                SetFullScreen(IsFullScreen);
            }
        }
    }
}
