using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ExoGame2D.Renderers
{
    public class DrawContext
    {
        public DrawContext(GraphicsDevice graphicsDevice)
        {
            SpriteBatch = new SpriteBatch(graphicsDevice);
        }

        public SpriteBatch SpriteBatch { get; }

        public Matrix Transformation { get; set; }

        public void BeginDraw()
        {
            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
                null, null, null, null,
                Transformation);
        }

        public void EndDraw()
        {
            SpriteBatch.End();
        }
    }
}
