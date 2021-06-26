using ExoGame2D.UI;
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

        public void DrawString(SpriteFont font, string text, Rectangle bounds, Alignment align, Color color)
        {
            Vector2 size = font.MeasureString(text);
            Point pos = bounds.Center;
            Vector2 origin = size * 0.5f;

            if (align.HasFlag(Alignment.Left))
            {
                origin.X += bounds.Width / 2 - size.X / 2;
            }

            if (align.HasFlag(Alignment.Right))
            {
                origin.X -= bounds.Width / 2 - size.X / 2;
            }

            if (align.HasFlag(Alignment.Top))
            {
                origin.Y += bounds.Height / 2 - size.Y / 2;
            }

            if (align.HasFlag(Alignment.Bottom))
            {
                origin.Y -= bounds.Height / 2 - size.Y / 2;
            }

            SpriteBatch.DrawString(font, text,
                new Vector2(pos.X, pos.Y), color, 0, origin, 1, SpriteEffects.None, 0);
        }

        public static implicit operator SpriteBatch(DrawContext context)
            => context?.SpriteBatch;
    }
}
