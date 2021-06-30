using ExoGame2D.Renderers;
using Microsoft.Xna.Framework;

namespace ExoGame2D.Tiles
{
    public class TileMap : IRenderNode
    {
        private readonly RenderNodeContainer _tiles = new RenderNodeContainer();
        private readonly TileSet _tileSet;

        public TileMap(TileSet tileSet)
            => _tileSet = tileSet;

        public TileMap MapTile(int tileColIndex, int tileRowIndex, Vector2 location)
        {
            var tile = _tileSet.CreateTile(tileColIndex, tileRowIndex);
            tile.Location = location;
            _tiles.Add(tile);
            return this;
        }

        public void Draw(DrawContext context, GameTime gameTime)
            => _tiles.Draw(context, gameTime);

        public void Update(GameTime gameTime)
            => _tiles.Update(gameTime);
    }
}
