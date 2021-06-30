using Microsoft.Xna.Framework;
using System;

namespace ExoGame2D.Tiles
{
    public class GridTileSet : TileSet
    {
        private readonly int _tileWidth;
        private readonly int _tileHeight;
        private readonly int _colCount;
        private readonly int _rowCount;

        public GridTileSet(string name, int tileWidth, int tileHeight)
            : base(name)
        {
            _tileWidth = tileWidth;
            _tileHeight = tileHeight;
            _colCount = TilesTexture.Width / tileWidth;
            _rowCount = TilesTexture.Height / tileHeight;
        }

        public int TileWidth => _tileWidth;
        public int TileHeight => _tileHeight;
        public int ColTileCount => _colCount;
        public int RowTileCount => _rowCount;

        public Tile CreateTile(Point tileLocation)
            => CreateTile(tileLocation.X, tileLocation.Y);

        public override Tile CreateTile(int tileColIndex, int tileRowIndex)
        {
            if (tileRowIndex < 0 || tileRowIndex >= _rowCount)
                throw new ArgumentOutOfRangeException(nameof(tileRowIndex));
            if (tileColIndex < 0 || tileColIndex >= _colCount)
                throw new ArgumentOutOfRangeException(nameof(tileColIndex));

            var x = tileColIndex * _tileWidth;
            var y = tileRowIndex * _tileHeight;
            return new Tile(this, new Rectangle(x, y, _tileWidth, _tileHeight));
        }
    }
}
