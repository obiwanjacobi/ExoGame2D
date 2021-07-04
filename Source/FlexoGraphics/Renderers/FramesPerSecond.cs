using Microsoft.Xna.Framework;
using System;

namespace FlexoGraphics.Renderers
{
    public class FramesPerSecond : IRenderNode
    {
        private readonly FrameCounter _frameCounter = new FrameCounter();
        private readonly FontRender _fontRender;

        public FramesPerSecond(string fontName)
        {
            _fontRender = new FontRender("FramesPerSecond");
            _fontRender.LoadContent(fontName);
            FormatText = "FPS: {0}";
        }

        public string FormatText { get; set; }

        public Vector2 Location
        {
            get { return _fontRender.Location; }
            set { _fontRender.Location = value; }
        }

        public void Draw(DrawContext context, GameTime gameTime)
            => _fontRender.Draw(context, gameTime);

        public void Update(GameTime gameTime)
        {
            _frameCounter.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            _fontRender.Text = String.Format(FormatText, Math.Round(_frameCounter.AverageFramesPerSecond));
            _fontRender.Update(gameTime);
        }
    }
}
