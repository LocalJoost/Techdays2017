﻿// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
#if UNITY_UWP
using System.Collections.Concurrent;
#endif


namespace Microsoft.Toolkit.Uwp.UI
{
    /// <summary>
    /// Generic in-memory storage of items
    /// </summary>
    /// <typeparam name="T">T defines the type of item stored</typeparam>
    public class InMemoryStorage<T>
    {
        private int _maxItemCount;
#if UNITY_UWP
        private ConcurrentDictionary<string, InMemoryStorageItem<T>> _inMemoryStorage = new ConcurrentDictionary<string, InMemoryStorageItem<T>>();
#else
        private Dictionary<string, InMemoryStorageItem<T>> _inMemoryStorage = new Dictionary<string, InMemoryStorageItem<T>>();
#endif

        /// <summary>
        /// Gets or sets the maximum count of Items that can be stored in this InMemoryStorage instance.
        /// </summary>
        public int MaxItemCount
        {
            get
            {
                return _maxItemCount;
            }

            set
            {
                if (_maxItemCount == value)
                {
                    return;
                }

                _maxItemCount = value;

                lock (this)
                {
                    EnsureStorageBounds(value);
                }
            }
        }

        /// <summary>
        /// Clears all items stored in memory
        /// </summary>
        public void Clear()
        {
            _inMemoryStorage.Clear();
        }

        /// <summary>
        /// Clears items stored in memory based on duration passed
        /// </summary>
        /// <param name="duration">TimeSpan to identify expired items</param>
        public void Clear(TimeSpan duration)
        {
            DateTime expirationDate = DateTime.Now.Subtract(duration);

            var itemsToRemove = _inMemoryStorage.Where(kvp => kvp.Value.LastUpdated <= expirationDate).Select(kvp => kvp.Key);

            if (itemsToRemove.Any())
            {
                Remove(itemsToRemove);
            }
        }

        /// <summary>
        /// Remove items based on provided keys
        /// </summary>
        /// <param name="keys">identified of the in-memory storage item</param>
#if UNITY_UWP
        public void Remove(IEnumerable<string> keys)
        {
            foreach (var key in keys)
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    continue;
                }

                InMemoryStorageItem<T> tempItem = null;

                _inMemoryStorage.TryRemove(key, out tempItem);

                tempItem = null;
            }
        }
#else
        public void Remove(IEnumerable<string> keys)
        {
            foreach (var key in keys)
            {
                if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(key.Trim()))
                {
                    continue;
                }

                if (_inMemoryStorage.ContainsKey(key))
                {
                    _inMemoryStorage[key] = null;
                    _inMemoryStorage.Remove(key);
                }
            }
        }
#endif
        /// <summary>
        /// Add new item to in-memory storage
        /// </summary>
        /// <param name="item">item to be stored</param>
        public void SetItem(InMemoryStorageItem<T> item)
        {
            if (MaxItemCount == 0)
            {
                return;
            }

            _inMemoryStorage[item.Id] = item;

            // ensure max limit is maintained. trim older entries first
            if (_inMemoryStorage.Count > MaxItemCount)
            {
                var itemsToRemove = _inMemoryStorage.OrderBy(kvp => kvp.Value.Created).Take(_inMemoryStorage.Count - MaxItemCount).Select(kvp => kvp.Key);
                Remove(itemsToRemove);
            }
        }

        /// <summary>
        /// Get item from in-memory storage as long as it has not ex
        /// </summary>
        /// <param name="id">id of the in-memory storage item</param>
        /// <param name="duration">timespan denoting expiration</param>
        /// <returns>Valid item if not out of date or return null if out of date or item does not exist</returns>
        public InMemoryStorageItem<T> GetItem(string id, TimeSpan duration)
        {
            InMemoryStorageItem<T> tempItem = null;

            if (!_inMemoryStorage.TryGetValue(id, out tempItem))
            {
                return null;
            }

            DateTime expirationDate = DateTime.Now.Subtract(duration);

            if (tempItem.LastUpdated > expirationDate)
            {
                return tempItem;
            }

            Remove(new []{ id });

            return null;
        }

        private void EnsureStorageBounds(int maxCount)
        {
            if (_inMemoryStorage.Count == 0)
            {
                return;
            }

            if (maxCount == 0)
            {
                _inMemoryStorage.Clear();
                return;
            }

            if (_inMemoryStorage.Count > maxCount)
            {
                Remove(_inMemoryStorage.Keys.Take(_inMemoryStorage.Count - maxCount));
            }
        }
    }
}
