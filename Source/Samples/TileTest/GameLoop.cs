using FlexoGraphics;
using FlexoGraphics.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TileTest
{
    public class GameLoop : Game
    {
        private readonly LevelTileMaps _levelMaps = new LevelTileMaps();
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
            // TODO: Client Window resizing with manual world viewport does not work correctly yet.
            //Window.AllowUserResizing = true;

            base.Initialize();
            _engine.Initialize(1920, 1080, 3000, 2000, 480, 240);
        }

        protected override void LoadContent()
        {
            _levelMaps.LoadContent(Content);
            _tileMap = _levelMaps.Level1();

            _villageTileSet = new VillagePropsTileSet(Content);
            _villageTileMap = new TileMap(_villageTileSet);

            _villageTileMap.MapTile(_villageTileSet.BigBoxTile, new Vector2(375, 115));
        }

        protected override void Update(GameTime gameTime)
        {
            var delta = 10;
            var keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            if (keyboardState.IsKeyDown(Keys.Down))
                MoveWorldViewport(0, delta);
            if (keyboardState.IsKeyDown(Keys.Up))
                MoveWorldViewport(0, -delta);

            if (keyboardState.IsKeyDown(Keys.Right))
                MoveWorldViewport(delta, 0);
            if (keyboardState.IsKeyDown(Keys.Left))
                MoveWorldViewport(-delta, 0);

            base.Update(gameTime);
        }

        private void MoveWorldViewport(int deltaX, int deltaY)
        {
            _engine.CoordinateSpace.MoveWorldViewport(deltaX, deltaY);
            _engine.UpdateDrawContext();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _engine.DrawContext.BeginDraw();

            _tileMap.Draw(_engine.DrawContext, gameTime);
            _villageTileMap.Draw(_engine.DrawContext, gameTime);

            _engine.DrawContext.EndDraw();
        }
    }
}
