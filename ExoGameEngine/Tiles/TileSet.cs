using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ExoGame2D.Tiles
{
    public class TileSet
    {
        private readonly int _tileWidth;
        private readonly int _tileHeight;
        private readonly int _tilesInRow;
        private readonly int _tilesInCol;
        private readonly Texture2D _tiles;
        private readonly SpriteBatch _spriteBatch;

        private TileSet(Texture2D tiles, int tileWidth, int tileHeight)
        {
            _tiles = tiles;
            _tileWidth = tileWidth;
            _tileHeight = tileHeight;

            _tilesInRow = _tiles.Width / tileWidth;
            _tilesInCol = _tiles.Height / tileHeight;

            _spriteBatch = Engine.SpriteBatch;
        }

        public int RowTileCount => _tilesInRow;
        public int ColTileCount => _tilesInCol;

        public Tile GetTile(int tileRowIndex, int tileColIndex)
        {
            if (tileRowIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(tileRowIndex));
            if (tileColIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(tileColIndex));
            if (tileRowIndex >= _tilesInRow)
                throw new ArgumentOutOfRangeException(nameof(tileRowIndex));
            if (tileColIndex >= _tilesInCol)
                throw new ArgumentOutOfRangeException(nameof(tileColIndex));

            var x = tileRowIndex * _tileWidth;
            var y = tileColIndex * _tileHeight;
            var w = x + _tileWidth;
            var h = y + _tileHeight;
            return new Tile(this, new Rectangle(x, y, w, h));
        }

        public static TileSet Load(string name, int tileWidth, int tileHeight)
        {
            var contentManager = Engine.Content;
            var tileSetTexture = contentManager.Load<Texture2D>(name);

            return new TileSet(tileSetTexture, tileWidth, tileHeight);
        }

        internal void DrawTile(Rectangle tileFromSetCoord, Vector2 location, SpriteEffects effect, Color color)
        {
            _spriteBatch.Draw(
                _tiles,
                location,
                tileFromSetCoord,
                color,
                0,
                new Vector2(0, 0),
                1.0f,
                effect,
                0.0f);
        }
    }
}
