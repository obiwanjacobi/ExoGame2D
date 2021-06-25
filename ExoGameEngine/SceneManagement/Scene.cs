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

        public void AddSpriteToLayer(RenderLayerEnum layer, IRenderNode node)
            => AddSpriteToLayer((int)layer, node);

        public void AddSpriteToLayer(int layerIndex, IRenderNode node)
        {
            var layer = GetLayer(layerIndex);
            layer.Add(node);

            if (node.IsAssetOfType(typeof(Sprite)) || node.IsAssetOfType(typeof(AnimatedSprite)))
            {
                if (string.IsNullOrEmpty(node.Name))
                {
                    throw new InvalidOperationException("A Sprite must have a name.");
                }

                CollisionManager.AddSpriteToCollisionManager(node.GetSprite(), node.Name);
            }
        }

        public bool RemoveSpriteFromLayer(RenderLayerEnum layer, IRenderNode node)
            => RemoveSpriteFromLayer((int)layer, node);

        public bool RemoveSpriteFromLayer(int layerIndex, IRenderNode node)
        {
            var layer = GetLayer(layerIndex);
            return layer.Remove(node);
        }

        public void RenderScene(GameTime gameTime)
        {
            // TODO: replace linq with something faster
            foreach (var node in _layers.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value))
            {
                node.Draw(gameTime);
            }
        }

        public void UpdateGameLogic(GameTime gameTime)
        {
            SoundEffectPlayer.ProcessSoundEvents();

            foreach (var node in _layers.Values)
            {
                node.Update(gameTime);
            }
        }

        private Layer GetLayer(int index)
        {
            if (!_layers.ContainsKey(index))
            {
                _layers[index] = new Layer();
            }

            return _layers[index];
        }
    }
}
