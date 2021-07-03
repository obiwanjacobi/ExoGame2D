using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace FlexoGraphics.Renderers
{
    public class RenderNodeContainer : IRenderNode
    {
        private readonly List<IRenderNode> _nodes = new List<IRenderNode>();

        public int Count => _nodes.Count;

        public void Clear() => _nodes.Clear();

        public void Add(IRenderNode node)
            => _nodes.Add(node ?? throw new ArgumentNullException(nameof(node)));

        public bool Remove(IRenderNode node)
        {
            if (_nodes.Contains(node))
            {
                _nodes.Remove(node);
                return true;
            }

            return false;
        }
        public void Draw(DrawContext context, GameTime gameTime)
        {
            foreach (var control in _nodes)
            {
                control.Draw(context, gameTime);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (var control in _nodes)
            {
                control.Update(gameTime);
            }
        }
    }
}
