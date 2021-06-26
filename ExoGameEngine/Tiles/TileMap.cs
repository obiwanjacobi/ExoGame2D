using ExoGame2D.Renderers;
using ExoGame2D.SceneManagement;
using Microsoft.Xna.Framework;

namespace ExoGame2D.Tiles
{
    public class TileMap : IRenderNode
    {
        private readonly Layer _layer = new Layer();
        private readonly TileSet _tileSet;

        public TileMap(TileSet tileSet)
            => _tileSet = tileSet;

        public void MapTile(int tileRowIndex, int tileColIndex, Vector2 screenLocation)
        {
            var tile = _tileSet.GetTile(tileRowIndex, tileColIndex);
            _layer.Add(tile);
        }

        public void Draw(DrawContext context, GameTime gameTime)
            => _layer.Draw(context, gameTime);

        public void Update(GameTime gameTime)
            => _layer.Update(gameTime);
    }
}
