using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;

namespace ExoGame2D.Tiles
{
    public abstract class ComplexTileSet : TileSet
    {
        private readonly Dictionary<string, Rectangle> _tileRegister = new Dictionary<string, Rectangle>();

        protected ComplexTileSet(ContentManager content, string name)
            : base(content, name)
        { }

        protected void RegisterTileRegion(Point tileIndex, Rectangle tileOrigin)
            => RegisterTileRegion(tileIndex.X, tileIndex.Y, tileOrigin);

        protected void RegisterTileRegion(int col, int row, Rectangle tileOrigin)
            => _tileRegister[BuildKey(col, row)] = tileOrigin;

        public override Tile CreateTile(int tileColIndex, int tileRowIndex)
        {
            var key = BuildKey(tileColIndex, tileRowIndex);

            if (!_tileRegister.ContainsKey(key))
                throw new ArgumentOutOfRangeException($"Column/Row {key} not found.");

            return new Tile(this, _tileRegister[key]);
        }

        private static string BuildKey(int col, int row)
            => $"{col}-{row}";
    }
}
