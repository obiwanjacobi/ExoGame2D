using Microsoft.Xna.Framework.Graphics;

namespace ExoGame2D.Renderers
{
    public class RenderContext
    {
        public RenderContext(GraphicsDevice graphicsDevice)
        {
            SpriteBatch = new SpriteBatch(graphicsDevice);
        }

        public SpriteBatch SpriteBatch { get; }
    }
}
