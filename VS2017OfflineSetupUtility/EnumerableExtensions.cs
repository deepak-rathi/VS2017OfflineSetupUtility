/*
    Copyright © 2017 Deepak Rathi 
    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VS2017OfflineSetupUtility
{
    public static class EnumerableExtensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> list) => new ObservableCollection<T>(list);

        public static bool TryAdd<K, V>(this IDictionary<K, V> dictionary, K key, V value)
        {
            if (dictionary.ContainsKey(key))
            {
                return false;
            }
            else
            {
                try
                {
                    dictionary.Add(key, value);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Adds the items from an existing <see cref="IEnumerable{T}"/> to this ObservableCollection.
        /// </summary>
        /// <param name="items">The set of objects to be added.</param>
        /// <param name="clearFirst">Specifies whether or not this ObservableCollection will be cleared before the items are added</param>
        /// <remarks>
        /// NOTE: This method will fire the OnCollectionChanged event one time for each item added.
        /// </remarks>
        public static int AddRange<T>(this ObservableCollection<T> list, IEnumerable<T> items, bool clearFirst = false)
        {
            if (clearFirst)
            {
                list.Clear();
            }
            foreach (var item in items)
            {
                list.Add(item);
            }
            return list.Count;
        }

        public static T AddAndReturn<T>(this IList<T> list, T item)
        {
            list.Add(item);
            return item;
        }

        public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (var item in list)
            {
                action?.Invoke(item);
            }
        }
    }
}
