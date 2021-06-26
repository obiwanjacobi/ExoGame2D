using ExoGame2D;
using ExoGame2D.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TileTest
{
    public class GameLoop : Game
    {
        private TileSet _tileSet;
        private TileMap _tileMap;
        private readonly Engine _engine;

        public GameLoop()
        {
            _engine = new Engine(this);
        }

        protected override void Initialize()
        {
            base.Initialize();
            _engine.Initialize();
        }

        protected override void LoadContent()
        {
            _tileSet = new TileSet("TX Village Props", 32, 32);
            _tileMap = new TileMap(_tileSet);

            _tileMap.MapTile(0, 0, new Vector2(100, 100));
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
            _engine.DrawContext.EndDraw();
        }
    }
}
