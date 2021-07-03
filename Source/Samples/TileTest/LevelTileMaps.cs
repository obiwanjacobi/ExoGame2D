using FlexoGraphics.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace TileTest
{
    internal class LevelTileMaps
    {
        private readonly static Dictionary<char, Point> _mapping = new Dictionary<char, Point>()
        {
            { '1', new Point(0, 0) },
            { '2', new Point(3, 0) },
            { '3', new Point(4, 0) },
            { '4', new Point(5, 0) },
            { '5', new Point(6, 0) },
            { '6', new Point(7, 0) },
            { '7', new Point(0, 1) },
            { '8', new Point(1, 1) },
            { '9', new Point(2, 1) },
            { '0', new Point(3, 1) },
        };

        private GridTileSet _tileSetJapaneseWallSet;

        public void LoadContent(ContentManager content)
        {
            _tileSetJapaneseWallSet = new GridTileSet(content, "Japanese Wall Set", 32, 32);
        }

        public TileMap Level1()
        {
            return new CharMappingTileMapBuilder(_tileSetJapaneseWallSet, _mapping)
                // set `Copy to output Directoy` on your file (properties).
                .Load("Content/Levels/Level1.txt")
                .Build();
        }
    }
}
