using FlexoGraphics.Renderers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FlexoGraphics.Tiles
{
    public abstract class TileSet
    {
        private readonly Texture2D _tilesTexture;

        protected TileSet(ContentManager content, string name)
        {
            _tilesTexture = content.Load<Texture2D>(name);
        }

        protected Texture2D TilesTexture => _tilesTexture;

        public Tile CreateTile(Point tileIndex)
            => CreateTile(tileIndex.X, tileIndex.Y);

        public abstract Tile CreateTile(int tileColIndex, int tileRowIndex);

        public void Draw(DrawContext context, GameTime gameTime)
            => context.SpriteBatch.Draw(_tilesTexture, _tilesTexture.Bounds, Color.White);

        internal void DrawTile(DrawContext context, Rectangle tileOrigin, Vector2 location, SpriteEffects effect, Color color)
        {
            context.SpriteBatch.Draw(
                _tilesTexture,
                location,
                tileOrigin,
                color,
                0,
                Vector2.Zero,
                1.0f,
                effect,
                0.0f);
        }
    }
}
