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

        //public string Name { get; set; }

        public void Draw(GameTime gameTime)
            => _layer.Draw(gameTime, Color.White);

        public void Draw(GameTime gameTime, Color tint)
            => _layer.Draw(gameTime, tint);

        public void Update(GameTime gameTime)
            => _layer.Update(gameTime);

        //public ISprite GetSprite()
        //    => null;

        //public bool IsAssetOfType(Type type)
        //    => false;
    }
}
