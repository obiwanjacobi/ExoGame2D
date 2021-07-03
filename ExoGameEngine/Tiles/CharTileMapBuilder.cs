using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace ExoGame2D.Tiles
{
    /// <summary>
    /// CharTileMapBuilder based on a Dictionary mapping.
    /// </summary>
    public class CharMappingTileMapBuilder : CharTileMapBuilder
    {
        private readonly IDictionary<char, Point> _mapping;

        public CharMappingTileMapBuilder(GridTileSet tileSet, IDictionary<char, Point> mapping)
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
    /// Derive and implement MapToTileIndex.
    /// </summary>
    public abstract class CharTileMapBuilder
    {
        private readonly GridTileSet _tileSet;
        private readonly List<TilePlacement> _tiles = new List<TilePlacement>();

        protected CharTileMapBuilder(GridTileSet tileSet)
        {
            _tileSet = tileSet ?? throw new ArgumentNullException(nameof(tileSet));
        }

        public CharTileMapBuilder Load(string charTileMapPath)
        {
            using (var fileStream = TitleContainer.OpenStream(charTileMapPath))
                return ReadCharMapFile(fileStream);
        }

        /// <summary>
        /// Reads the characters in the map file and calls MapToTileIndex for each character.
        /// </summary>
        /// <param name="fileStream">Will not dispose the <paramref name="fileStream"/>.</param>
        public CharTileMapBuilder ReadCharMapFile(Stream fileStream)
        {
            var reader = new StreamReader(fileStream);

            string line = reader.ReadLine();
            var location = new Point(0, 0);
            while (line != null)
            {
                foreach (var c in line)
                {
                    var index = MapToTileIndex(c);

                    if (index != null)
                        _tiles.Add(new TilePlacement(index.Value, location));

                    location.X += _tileSet.TileWidth;
                }

                location.X = 0;
                location.Y += _tileSet.TileHeight;

                line = reader.ReadLine();
            }

            return this;
        }

        /// <summary>
        /// Called while reading the tile character file.
        /// </summary>
        /// <param name="tileChar">the single character read from the file that represents a tile.</param>
        /// <returns>Returns the logical col-row tileset index of the tile to be mapped.
        /// Returns null when no tile is to be mapped.</returns>
        protected abstract Point? MapToTileIndex(char tileChar);

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

        private struct TilePlacement
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
