using ExoGame2D.Renderers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ExoGame2D.SceneManagement
{
    public class Layer
    {
        private readonly List<IRenderNode> _nodes = new List<IRenderNode>();

        public void Add(IRenderNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            _nodes.Add(node);
        }

        public bool Remove(IRenderNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            if (_nodes.Contains(node))
            {
                _nodes.Remove(node);
                return true;
            }

            return false;
        }

        public void Draw(GameTime gameTime)
        {
            foreach (var node in _nodes)
            {
                node.Draw(gameTime);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (var node in _nodes)
            {
                node.Update(gameTime);
            }
        }
    }
}
