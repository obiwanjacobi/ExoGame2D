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
using System;
using System.Collections.Generic;

namespace ExoGame2D
{
    public class GameStateManager
    {
        private readonly Dictionary<string, IGameState> _registeredGameStates = new Dictionary<string, IGameState>();

        public IGameState CurrentState { get; private set; } = null;
        public IGameState PreviousState { get; private set; } = null;


        public void Register(string name, IGameState gameStateHandler)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (gameStateHandler == null)
            {
                throw new ArgumentNullException(nameof(gameStateHandler));
            }

            var nameUpper = name.ToUpperInvariant();
            if (_registeredGameStates.ContainsKey(nameUpper))
            {
                _registeredGameStates.Remove(nameUpper);
            }

            _registeredGameStates.Add(nameUpper, gameStateHandler);
        }

        public void RemoveState(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            var nameUpper = name.ToUpperInvariant();
            if (!_registeredGameStates.ContainsKey(nameUpper))
            {
                throw new InvalidOperationException("The gamestate manager does not contain an entry for <" + name + ">");
            }

            _registeredGameStates.Remove(nameUpper);
        }

        public void ChangeState(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            var nameUpper = name.ToUpperInvariant();
            if (!_registeredGameStates.ContainsKey(nameUpper))
            {
                throw new InvalidOperationException("The gamestate manager does not contain an entry for <" + name + ">");
            }

            PreviousState = CurrentState;
            CurrentState = _registeredGameStates[nameUpper];
        }
    }
}
