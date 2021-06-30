using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ExoGame2D
{
    public class CoordinateSpace
    {
        private float _deviceToWorldScale;

        public CoordinateSpace(int worldWidth, int worldHeight)
        {
            var deviceBounds = new Rectangle(0, 0,
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width,
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);

            Device = new CoordinatePlane(deviceBounds, Rectangle.Empty);

            var worldBounds = new Rectangle(0, 0, worldWidth, worldHeight);
            World = new CoordinatePlane(worldBounds, Rectangle.Empty);
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

            _deviceToWorldScale = World.Bounds.Width / (float)Device.Viewport.Width;

            var worldViewport = CalcWorldViewport();
            World = new CoordinatePlane(World.Bounds, worldViewport);
        }

        public Vector2 DeviceToWorld(int x, int y)
            => DeviceToWorld(new Vector2(x, y));

        public Vector2 DeviceToWorld(Vector2 screenPosition)
        {
            var viewportTopLeft = new Vector2(Device.Viewport.X, Device.Viewport.Y);
            return (screenPosition - viewportTopLeft) * _deviceToWorldScale;
        }

        public Matrix WorldToDeviceScale()
        {
            return Matrix.CreateScale(
                    (float)Device.Viewport.Width / World.Bounds.Width,
                    (float)Device.Viewport.Height / World.Bounds.Height, 1);
        }

        private Rectangle CalcWorldViewport()
        {
            var viewportSize = DeviceToWorld(Device.Viewport.Width, Device.Viewport.Height);
            return new Rectangle(World.Viewport.X, World.Viewport.Y, (int)viewportSize.X, (int)viewportSize.Y);
        }

        private Rectangle CalcDeviceViewport(int deviceWidth, int deviceHeight)
        {
            var viewport = new Rectangle();
            float worldRatio = World.AspectRatio;
            float deviceRatio = (float)deviceWidth / deviceHeight;

            // if the window is relatively wide, use the full window height
            if (deviceRatio > worldRatio)
            {
                viewport.Width = (int)(deviceHeight * worldRatio);
                viewport.Height = deviceHeight;
            }
            // if the window is relatively high, use the full window width
            else
            {
                viewport.Width = deviceWidth;
                viewport.Height = (int)(deviceWidth / worldRatio);
            }

            // calculate and store the top-left corner of the viewport
            viewport.X = (deviceWidth - viewport.Width) / 2;
            viewport.Y = (deviceHeight - viewport.Height) / 2;

            return viewport;
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
