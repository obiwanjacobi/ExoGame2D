using ExoGame2D.Interfaces;
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
            => Draw(gameTime, Color.White);

        public void Draw(GameTime gameTime, Color tint)
        {
            foreach (var node in _nodes)
            {
                node.Draw(gameTime, tint);
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
