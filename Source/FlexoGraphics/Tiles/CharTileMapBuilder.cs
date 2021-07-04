using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace FlexoGraphics.Tiles
{
    /// <summary>
    /// CharTileMapBuilder based on a Dictionary mapping.
    /// </summary>
    public class CharMappingTileMapBuilder : CharTileMapBuilder
    {
        private readonly IDictionary<char, Point> _mapping;

        public CharMappingTileMapBuilder(TileSet tileSet, IDictionary<char, Point> mapping)
            : base(tileSet)
        {
            _mapping = mapping ?? throw new ArgumentNullException(nameof(mapping));
        }

        protected override Point? MapToTileIndex(char tileChar)
        {
            if (_mapping.ContainsKey(tileChar))
                return _mapping[tileChar];
            return null;
        }
    }

    /// <summary>
    /// Derive and implement MapToTileIndex or MapToPlacement.
    /// </summary>
    public abstract class CharTileMapBuilder
    {
        private readonly TileSet _tileSet;
        private readonly GridTileSet _gridTileSet;
        private readonly List<TilePlacement> _tiles = new List<TilePlacement>();

        protected CharTileMapBuilder(TileSet tileSet)
        {
            _tileSet = tileSet ?? throw new ArgumentNullException(nameof(tileSet));
            _gridTileSet = tileSet as GridTileSet;
        }

        public CharTileMapBuilder Load(string charTileMapPath)
        {
            using (var fileStream = TitleContainer.OpenStream(charTileMapPath))
                return ReadCharMapFile(fileStream);
        }

        /// <summary>
        /// Reads the characters in the map file and calls MapToPlacement/MapToTileIndex for each character.
        /// </summary>
        /// <param name="fileStream">Will not dispose the <paramref name="fileStream"/>.</param>
        public CharTileMapBuilder ReadCharMapFile(Stream fileStream)
        {
            var reader = new StreamReader(fileStream);

            string line = reader.ReadLine();
            var pos = new Point();
            while (line != null)
            {
                foreach (var c in line)
                {
                    var placement = MapToPlacement(pos, c);
                    if (placement != null)
                        _tiles.Add(placement.Value);

                    pos.X++;
                }

                pos.X = 0;
                pos.Y++;

                line = reader.ReadLine();
            }

            return this;
        }

        /// <summary>
        /// Called while reading the tile character file.
        /// </summary>
        /// <param name="filePosition">X = character on a row (col). Y = line number (zero-based).</param>
        /// <param name="tileChar">A single character read from the file that represents a tile.</param>
        /// <returns>Returns null if not tile is to be displayed for <paramref name="tileChar"/>.</returns>
        protected virtual TilePlacement? MapToPlacement(Point filePosition, char tileChar)
        {
            if (_gridTileSet == null)
                throw new InvalidOperationException("Override MapToPlacement if not working with a GridTileSet.");

            var index = MapToTileIndex(tileChar);
            if (index != null)
            {
                var location = new Point(
                    filePosition.X * _gridTileSet.TileWidth,
                    filePosition.Y * _gridTileSet.TileHeight);
                return new TilePlacement(index.Value, location);
            }

            return null;
        }

        /// <summary>
        /// Called while reading the tile character file.
        /// </summary>
        /// <param name="tileChar">A single character read from the file that represents a tile.</param>
        /// <returns>Returns the logical col-row tileset index of the tile to be mapped.
        /// Returns null when no tile is to be mapped.</returns>
        /// <remarks>Default implementation uses the high and low nibble of the <paramref name="tileChar"/>.</remarks>
        protected virtual Point? MapToTileIndex(char tileChar)
        {
            // x = hi-nibble, y = lo-nibble
            return new Point((tileChar & 0xF0) >> 4, tileChar & 0x0F);
        }

        /// <summary>
        /// Actually create the Tiles and return the TileMap.
        /// </summary>
        /// <returns>Never returns null.</returns>
        public TileMap Build()
        {
            var tileMap = new TileMap(_tileSet);

            foreach (var placement in _tiles)
            {
                tileMap.MapTile(placement.Index, placement.Location.ToVector2());
            }

            return tileMap;
        }

        protected struct TilePlacement
        {
            public TilePlacement(Point index, Point location)
            {
                Index = index;
                Location = location;
            }

            public Point Index;
            public Point Location;
        }
    }
}
