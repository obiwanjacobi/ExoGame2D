/*
MIT License

Copyright (c) 2020

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using ExoGame2D.Interfaces;
using ExoGame2D.Renderers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExoGame2D.SceneManagement
{
    public class Scene
    {
        private readonly Dictionary<int, Layer> _layers = new Dictionary<int, Layer>();
        private IEnumerable<Layer> _orderedLayers;

        public void AddSpriteToLayer(RenderLayer layer, IRenderNode node)
            => AddToLayer((int)layer, node);

        public void AddToLayer(int layerIndex, IRenderNode node)
        {
            var layer = GetLayer(layerIndex);
            layer.Add(node);

            if (node is ICollidable collidable)
            {
                if (string.IsNullOrEmpty(collidable.Name))
                {
                    throw new InvalidOperationException("A Sprite must have a name.");
                }

                CollisionManager.Add(collidable, collidable.Name);
            }
        }

        public bool RemoveSpriteFromLayer(RenderLayer layer, IRenderNode node)
            => RemoveFromLayer((int)layer, node);

        public bool RemoveFromLayer(int layerIndex, IRenderNode node)
        {
            var layer = GetLayer(layerIndex);
            return layer.Remove(node);
        }

        public void Draw(DrawContext context, GameTime gameTime)
        {
            foreach (var node in _orderedLayers)
            {
                node.Draw(context, gameTime);
            }
        }

        public void UpdateGameLogic(GameTime gameTime)
        {
            SoundEffectPlayer.ProcessSoundEvents();

            foreach (var node in _orderedLayers)
            {
                node.Update(gameTime);
            }
        }

        private Layer GetLayer(int index)
        {
            if (!_layers.ContainsKey(index))
            {
                _layers[index] = new Layer();
                // TODO: replace linq with something faster
                _orderedLayers = _layers.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value);
            }

            return _layers[index];
        }
    }
}
