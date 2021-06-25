using ExoGame2D.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ExoGame2D.Tiles
{
    public class Tile : IRenderNode
    {

        private readonly TileSet _tileSet;
        private readonly Rectangle _tileFromSetCoord;

        internal Tile(TileSet tileSet, Rectangle tileFromSetCoord)
        {
            _tileSet = tileSet;
            _tileFromSetCoord = tileFromSetCoord;
        }

        public string Name { get; set; }

        public Vector2 Location { get; set; }

        public void Draw(GameTime gameTime)
            => Draw(gameTime, Color.White);

        public void Draw(GameTime gameTime, Color tint)
        {
            var effect = SpriteEffects.None;
            _tileSet.DrawTile(_tileFromSetCoord, Location, effect, tint);
        }

        public ISprite GetSprite()
        {
            return null;
        }

        public bool IsAssetOfType(Type type)
        {
            return false;
        }

        public void Update(GameTime gameTime)
        {
            // Tiles dont move
        }
    }
}
