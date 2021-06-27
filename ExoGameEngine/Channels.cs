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
    public static class Channels
    {
        private static readonly Dictionary<string, Queue<IChannelMessage>> _channels =
            new Dictionary<string, Queue<IChannelMessage>>();

        public static void Create(string channelName)
        {
            if (string.IsNullOrEmpty(channelName))
            {
                throw new ArgumentNullException(nameof(channelName));
            }

            if (!_channels.ContainsKey(channelName.ToLowerInvariant()))
            {
                _channels.Add(channelName, new Queue<IChannelMessage>());
            }
        }

        public static void Delete(string channelName)
        {
            if (string.IsNullOrEmpty(channelName))
            {
                throw new ArgumentNullException(nameof(channelName));
            }

            var channelNameLower = channelName.ToLowerInvariant();
            if (!_channels.ContainsKey(channelNameLower))
            {
                throw new InvalidOperationException("The channel <" + channelNameLower + "> does not exist.");
            }
            if (_channels[channelNameLower].Count > 0)
            {
                throw new InvalidOperationException("The channel <" + channelNameLower + "> contains messages.");
            }

            _channels.Remove(channelNameLower);
        }

        public static bool Exists(string channelName)
        {
            if (string.IsNullOrEmpty(channelName))
            {
                throw new ArgumentNullException(nameof(channelName));
            }

            return _channels.ContainsKey(channelName.ToLowerInvariant());
        }

        public static void Clear(string channelName)
        {
            if (string.IsNullOrEmpty(channelName))
            {
                throw new ArgumentNullException(nameof(channelName));
            }

            var channelNameLower = channelName.ToLowerInvariant();
            if (!_channels.ContainsKey(channelNameLower))
            {
                throw new InvalidOperationException("The channel <" + channelNameLower + "> does not exist.");
            }

            _channels[channelNameLower].Clear();
        }

        public static void PostMessage(string channelName, IChannelMessage message)
        {
            if (string.IsNullOrEmpty(channelName))
            {
                throw new ArgumentNullException(nameof(channelName));
            }
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var channelNameLower = channelName.ToLowerInvariant();
            if (!_channels.ContainsKey(channelNameLower))
            {
                throw new InvalidOperationException("The channel <" + channelNameLower + "> does not exist.");
            }

            _channels[channelNameLower].Enqueue(message);
        }

        public static T LastMessageAs<T>(string channelName)
            where T : class, IChannelMessage
        {
            if (string.IsNullOrEmpty(channelName))
            {
                throw new ArgumentNullException(nameof(channelName));
            }

            var channelNameLower = channelName.ToLowerInvariant();
            if (!_channels.ContainsKey(channelNameLower))
            {
                throw new InvalidOperationException("The channel <" + channelNameLower + "> does not exist.");
            }

            if (_channels[channelNameLower].Count > 0)
            {
                return (T)_channels[channelNameLower].Dequeue();
            }

            return null;
        }
    }
}
