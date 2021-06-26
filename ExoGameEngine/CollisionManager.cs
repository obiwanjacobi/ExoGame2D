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
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ExoGame2D
{
    public static class CollisionManager
    {
        private static readonly Dictionary<string, ICollidable> _collidablesList = new Dictionary<string, ICollidable>();

        public static void Add(ICollidable collidable, string name)
        {
            if (!_collidablesList.ContainsKey(name))
            {
                _collidablesList.Add(name, collidable);
            }
        }

        public static void Remove(string name)
        {
            if (_collidablesList.ContainsKey(name))
            {
                _collidablesList.Remove(name);
            }
        }

        public static bool IsBoundingCollision(string source, string destination)
        {
            if (_collidablesList.ContainsKey(source) &&
                _collidablesList.ContainsKey(destination))
            {
                var sourceCol = _collidablesList[source];
                var destinationCol = _collidablesList[destination];

                return sourceCol.BoundingBox.Intersects(destinationCol.BoundingBox);
            }
            return false;
        }

        public static bool IsPerPixelCollision(string source, string destination)
        {
            if (_collidablesList.ContainsKey(source) &&
                _collidablesList.ContainsKey(destination))
            {
                var sourceCol = _collidablesList[source];
                var destinationCol = _collidablesList[destination];

                if (sourceCol.BoundingBox.Intersects(destinationCol.BoundingBox))
                {
                    return CollidesWith(sourceCol, destinationCol);
                }
            }
            return false;
        }

        private static bool CollidesWith(ICollidable source, ICollidable destination)
        {
            // Calculate the intersecting rectangle
            int x1 = Math.Max(source.BoundingBox.X, destination.BoundingBox.X);
            int x2 = Math.Min(source.BoundingBox.X + source.BoundingBox.Width, destination.BoundingBox.X + destination.BoundingBox.Width);

            int y1 = Math.Max(source.BoundingBox.Y, destination.BoundingBox.Y);
            int y2 = Math.Min(source.BoundingBox.Y + source.BoundingBox.Height, destination.BoundingBox.Y + destination.BoundingBox.Height);

            Color[] sourceData;
            Color[] destinationData;

            // TODO: adjust algorithm to use smallest region
            // Passing BoundingBox is not valid.
            // rect must be within texture (0,0,w,h) - I think.
            var rect = new Rectangle();
            if (source.GetPixelData(rect, out sourceData) &&
                destination.GetPixelData(rect, out destinationData))
            {
                // For each single pixel in the intersecting rectangle
                for (int y = y1; y < y2; ++y)
                {
                    for (int x = x1; x < x2; ++x)
                    {
                        // Get the color from each texture
                        Color a = sourceData[(x - source.BoundingBox.X) + (y - source.BoundingBox.Y) * source.BoundingBox.Width];
                        Color b = destinationData[(x - destination.BoundingBox.X) + (y - destination.BoundingBox.Y) * destination.BoundingBox.Width];

                        // If both colors are not transparent (the alpha channel is not 0),
                        // then there is a collision
                        if (a.A != 0 && b.A != 0)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public static void RemoveAll()
        {
            _collidablesList.Clear();
        }
    }
}
