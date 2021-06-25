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

        public GameLoop()
        {
            Engine.InitializeEngine(this, 1920, 1080);
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _tileSet = TileSet.Load("terrain_atlas", 32, 32);
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
            Engine.SpriteBatch.Begin();

            _tileMap.Draw(gameTime);
            base.Draw(gameTime);

            Engine.SpriteBatch.End();
        }
    }
}
