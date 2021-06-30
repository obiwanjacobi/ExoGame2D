using ExoGame2D;
using ExoGame2D.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TileTest
{
    public class GameLoop : Game
    {
        private GridTileSet _tileSet;
        private TileMap _tileMap;

        private VillagePropsTileSet _villageTileSet;
        private TileMap _villageTileMap;

        private readonly Engine _engine;

        public GameLoop()
        {
            _engine = new Engine(this);
        }

        protected override void Initialize()
        {
            base.Initialize();
            _engine.Initialize(1920, 1080, 480, 240);
        }

        protected override void LoadContent()
        {
            _tileSet = new GridTileSet("Japanese Wall Set", 32, 32);
            _tileMap = new TileMap(_tileSet);

            int delta = 16;
            for (int c = 0; c < _tileSet.ColTileCount; c++)
            {
                for (int r = 0; r < _tileSet.RowTileCount; r++)
                {
                    int x = c * (_tileSet.TileWidth + delta) + delta;
                    int y = r * (_tileSet.TileHeight + delta) + delta;
                    _tileMap.MapTile(c, r, new Vector2(x, y));
                }
            }

            _villageTileSet = new VillagePropsTileSet();
            _villageTileMap = new TileMap(_villageTileSet);

            _villageTileMap.MapTile(_villageTileSet.BigBoxTile, new Vector2(400, 100));
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _engine.DrawContext.BeginDraw();

            _tileMap.Draw(_engine.DrawContext, gameTime);
            //_tileSet.Draw(_engine.DrawContext, gameTime);

            _villageTileMap.Draw(_engine.DrawContext, gameTime);

            _engine.DrawContext.EndDraw();
        }
    }
}
