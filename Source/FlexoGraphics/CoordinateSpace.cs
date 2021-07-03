using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FlexoGraphics
{
    public class CoordinateSpace
    {
        private readonly bool _manualWorldViewport;
        private float _deviceToWorldScale;

        public CoordinateSpace(int worldWidth, int worldHeight)
        {
            _manualWorldViewport = false;
            Device = new CoordinatePlane(FullScreen, Rectangle.Empty);

            var worldBounds = new Rectangle(0, 0, worldWidth, worldHeight);
            World = new CoordinatePlane(worldBounds, Rectangle.Empty);
        }

        public CoordinateSpace(int worldWidth, int worldHeight, int worldViewportWidth, int worldViewportHeight)
        {
            _manualWorldViewport = true;
            Device = new CoordinatePlane(FullScreen, Rectangle.Empty);

            var worldBounds = new Rectangle(0, 0, worldWidth, worldHeight);
            var worldViewport = new Rectangle(0, 0, worldViewportWidth, worldViewportHeight);
            World = new CoordinatePlane(worldBounds, worldViewport);
        }

        public Rectangle FullScreen => new Rectangle(0, 0,
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width,
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);

        public CoordinatePlane Device { get; private set; }
        public CoordinatePlane World { get; private set; }

        public void ResizeDevice(int width, int height)
        {
            var deviceViewport = CalcDeviceViewport(width, height);
            var deviceBounds = new Rectangle(0, 0, width, height);
            Device = new CoordinatePlane(deviceBounds, deviceViewport);

            if (!_manualWorldViewport)
            {
                _deviceToWorldScale = World.Bounds.Width / (float)Device.Viewport.Width;

                var worldViewport = CalcWorldViewport();
                World = new CoordinatePlane(World.Bounds, worldViewport);
            }
            else
            {
                _deviceToWorldScale = World.Viewport.Width / (float)Device.Viewport.Width;
            }
        }

        public void MoveWorldViewport(int deltaX, int deltaY)
        {
            if (!_manualWorldViewport)
                throw new InvalidOperationException("Specify a world viewport (in Engine.Initialize) to be able to move it.");

            var worldX = World.Viewport.X + deltaX;
            var worldY = World.Viewport.Y + deltaY;
            var worldWidth = World.Viewport.Width;
            var worldHeight = World.Viewport.Height;

            if (worldX < World.Bounds.X)
                worldX = World.Bounds.X;
            if (worldY < World.Bounds.Y)
                worldY = World.Bounds.Y;
            if (worldX + worldWidth > World.Bounds.Width)
                worldX = World.Bounds.Width - worldWidth;
            if (worldY + worldHeight > World.Bounds.Height)
                worldY = World.Bounds.Height - worldHeight;

            var viewport = new Rectangle(
                worldX, worldY, worldWidth, worldHeight);
            World = new CoordinatePlane(World.Bounds, viewport);
        }

        public Vector2 DeviceToWorld(float deviceX, float deviceY)
        {
            float x = deviceX - Device.Viewport.X;
            float y = deviceY - Device.Viewport.Y;
            return new Vector2(x * _deviceToWorldScale, y * _deviceToWorldScale);
        }

        public Matrix WorldToDeviceTransform()
        {
            var translation = Matrix.CreateTranslation(-World.Viewport.X, -World.Viewport.Y, 0);
            var scale = Matrix.CreateScale(
                (float)Device.Viewport.Width / World.Viewport.Width,
                (float)Device.Viewport.Height / World.Viewport.Height, 1);

            return scale * translation;
        }

        private Rectangle CalcWorldViewport()
        {
            var viewportSize = DeviceToWorld(Device.Viewport.Width, Device.Viewport.Height);
            return new Rectangle(World.Viewport.X, World.Viewport.Y, (int)viewportSize.X, (int)viewportSize.Y);
        }

        private Rectangle CalcDeviceViewport(int deviceWidth, int deviceHeight)
        {
            if (_manualWorldViewport)
            {
                var world = new Rectangle(World.Viewport.X, World.Viewport.Y, World.Viewport.Width, World.Viewport.Height);
                SizeMatchToRatio(ref world, deviceWidth, deviceHeight);
                World = new CoordinatePlane(World.Bounds, world);
                return new Rectangle(0, 0, deviceWidth, deviceHeight);
            }

            return SizeToRatio(World.Bounds, deviceWidth, deviceHeight);
        }

        private static void SizeMatchToRatio(ref Rectangle source, int targetWidth, int targetHeight)
        {
            float sourceRatio = (float)source.Width / source.Height;
            float targetRatio = (float)targetWidth / targetHeight;

            if (targetRatio > sourceRatio)
            {
                var scale = (float)source.Height / targetHeight;
                source.Width = (int)(targetWidth * scale);
            }
            else
            {
                var scale = (float)source.Width / targetWidth;
                source.Height = (int)(targetHeight * scale);
            }
        }

        private static Rectangle SizeToRatio(Rectangle source, int targetWidth, int targetHeight)
        {
            var sized = new Rectangle();
            float sourceRatio = (float)source.Width / source.Height;
            float targetRatio = (float)targetWidth / targetHeight;

            if (targetRatio > sourceRatio)
            {
                sized.Width = (int)(targetHeight * sourceRatio);
                sized.Height = targetHeight;
            }
            else
            {
                sized.Width = targetWidth;
                sized.Height = (int)(targetWidth / sourceRatio);
            }

            sized.X = source.X + (targetWidth - sized.Width) / 2;
            sized.Y = source.Y + (targetHeight - sized.Height) / 2;

            return sized;
        }
    }

    public class CoordinatePlane
    {
        public CoordinatePlane(Rectangle bounds, Rectangle viewport)
        {
            Bounds = bounds;
            Viewport = viewport;

            if (bounds.Height != 0)
                AspectRatio = (float)bounds.Width / bounds.Height;
        }

        public Rectangle Bounds { get; }
        public Rectangle Viewport { get; }
        public float AspectRatio { get; }
    }
}
