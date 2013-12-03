/* 
 * Copyright (C) Ury Jamshy / ujamshy@yahoo.com
 *
 * THIS WORK IS PROVIDED "AS IS", "WHERE IS" AND "AS AVAILABLE", WITHOUT ANY EXPRESS OR
 * IMPLIED WARRANTIES OR CONDITIONS OR GUARANTEES. YOU, THE USER, ASSUME ALL RISK IN ITS
 * USE, INCLUDING COPYRIGHT INFRINGEMENT, PATENT INFRINGEMENT, SUITABILITY, ETC. AUTHOR
 * EXPRESSLY DISCLAIMS ALL EXPRESS, IMPLIED OR STATUTORY WARRANTIES OR CONDITIONS, INCLUDING
 * WITHOUT LIMITATION, WARRANTIES OR CONDITIONS OF MERCHANTABILITY, MERCHANTABLE QUALITY
 * OR FITNESS FOR A PARTICULAR PURPOSE, OR ANY WARRANTY OF TITLE OR NON-INFRINGEMENT, OR
 * THAT THE WORK (OR ANY PORTION THEREOF) IS CORRECT, USEFUL, BUG-FREE OR FREE OF VIRUSES.
 * 
 */

using System;
using System.Collections;
using System.Collections.Generic;

namespace DotNetApi.Collections.Generic
{
	/// <summary>
	/// A sorted map.
	/// </summary>
	/// <typeparam name="TKey">The map key type.</typeparam>
	/// <typeparam name="TValue">The map value type.</typeparam>
    public class SortedMap<TKey, TValue> : 
        IDictionary<TKey, TValue>, IDictionary,
        ICollection<KeyValuePair<TKey, TValue>>, ICollection,
        IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
    {
        #region Public Members

        public SortedMap() : this(null)
        {
        }

        public SortedMap(IComparer<TKey> comparer)
        {
            m_tree = new RedBlackTree<KeyValuePair<TKey, TValue>>(new KeyValuePairComparer(comparer));
        }

		/// <summary>
		/// Returns the first value in the sorted map that is no less than the specified key.
		/// The method throws an exception if such a key is not found.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>The value.</returns>
        public TValue LowerBound(TKey key)
        {
            RedBlackTree<KeyValuePair<TKey, TValue>>.TreeNode node = LowerBoundEqual(key);
            return NodeValue(node);
        }

		/// <summary>
		/// Returns the first value in the sorted map that is greater than the specified key.
		/// The method returns an exception if such a key is not found.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>The value.</returns>
        public TValue UpperBound(TKey key)
        {
            RedBlackTree<KeyValuePair<TKey, TValue>>.TreeNode node = UpperBoundNode(key);
            return NodeValue(node);
        }

		/// <summary>
		/// Tries to return the first value in the sorted map that is no less than the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <returns><b>True</b> if such a value is found, otherwise <b>false</b>.</returns>
		public bool TryLowerBound(TKey key, out TValue value)
		{
			RedBlackTree<KeyValuePair<TKey, TValue>>.TreeNode node = LowerBoundEqual(key);
			if (node != null)
			{
				value = node.Item.Value;
				return true;
			}
			else
			{
				value = default(TValue);
				return false;
			}
		}
		/// <summary>
		/// Tries to return the first value in the sorted map that is greater than the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		/// <returns><b>True</b> if such a value is found, otherwise <b>false</b>.</returns>
		public bool TryUpperBounds(TKey key, out TValue value)
		{
			RedBlackTree<KeyValuePair<TKey, TValue>>.TreeNode node = UpperBoundNode(key);
			if (node != null)
			{
				value = node.Item.Value;
				return true;
			}
			else
			{
				value = default(TValue);
				return false;
			}
		}

        public IEnumerable<KeyValuePair<TKey, TValue>> LowerBoundItems(TKey key)
        {
            RedBlackTree<KeyValuePair<TKey, TValue>>.TreeNode node = LowerBoundEqual(key);
            KeyValuePairEnumerator enumerator = new KeyValuePairEnumerator(
                m_tree, EnumeratorObjectType.KeyValuePairType, node);
            return new GenericEnumerable<KeyValuePair<TKey, TValue>>(enumerator);
        }

        public IEnumerable<KeyValuePair<TKey, TValue>> UpperBoundItems(TKey key)
        {
            RedBlackTree<KeyValuePair<TKey, TValue>>.TreeNode node = UpperBoundNode(key);
            KeyValuePairEnumerator enumerator = new KeyValuePairEnumerator(
                m_tree, EnumeratorObjectType.KeyValuePairType, node);
            return new GenericEnumerable<KeyValuePair<TKey, TValue>>(enumerator);
        }

        #endregion

        #region IDictionary<TKey, TValue> implementation

        ICollection<TKey> IDictionary<TKey, TValue>.Keys 
        {
            get 
            {
                return Keys; 
            }
        }

        ICollection<TValue> IDictionary<TKey, TValue>.Values 
        {
            get 
            {
                return Values;
            }
        }

        public TValue this[TKey key] 
        {
            get
            {
                if (key == null)
                {
                    throw new ArgumentNullException("key");
                }
                RedBlackTree<KeyValuePair<TKey, TValue>>.TreeNode node = FindByKey(key);
                return NodeValue(node);
            }

            set
            {
                if (key == null)
                {
                    throw new ArgumentNullException("key");
                }
                RedBlackTree<KeyValuePair<TKey, TValue>>.TreeNode node = FindByKey(key);
                if (node == null)
                    m_tree.Add(new KeyValuePair<TKey, TValue>(key, value));
                else
                    node.Item = new KeyValuePair<TKey, TValue>(node.Item.Key, value);
            }
        }

        public void Add(TKey key, TValue value)
        {
            m_tree.Add(new KeyValuePair<TKey, TValue>(key, value));
        }

        public bool ContainsKey(TKey key)
        {
            return FindByKey(key) != null;
        }

        public bool Remove(TKey key)
        {
            KeyValuePair<TKey, TValue> item = DefaultItem(key);
            //if (m_tree.Find(item) == null)
            //    return false;

            try
            {
                m_tree.Remove(item);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            RedBlackTree<KeyValuePair<TKey, TValue>>.TreeNode node = FindByKey(key);
            if (node == null)
            {
                value = default(TValue);
                return false;
            }
            else
            {
                value = node.Item.Value;
                return true;
            }
        }

        #endregion

        #region IDictionary implementation

        public bool IsFixedSize 
        {
            get
            {
                return false;
            }
        }

        public bool IsReadOnly 
        {
            get
            {
                return false;
            }
        }

        ICollection IDictionary.Keys 
        {
            get
            {
                return Keys;
            }
        }

        ICollection IDictionary.Values 
        {
            get
            {
                return Values;
            }
        }

        object IDictionary.this[object key] 
        {
            get
            {
                TValue local;
                if (ValidateKey(key) && TryGetValue((TKey)key, out local))
                {
                    return local;
                }
                return null;
            }
            set
            {
                ValidateKey(key);
                ValidateValue(value);
                this[(TKey)key] = (TValue)value;
            }
        }

        public void Add(object key, object value)
        {
            ValidateKey(key);
            ValidateValue(value);
            m_tree.Add(new KeyValuePair<TKey, TValue>((TKey)key, (TValue)value));
        }

        public void Clear()
        {
            m_tree.Clear();
        }

        public bool Contains(object key)
        {
            ValidateKey(key);
            return ContainsKey((TKey)key);
        }

		public IDictionaryEnumerator GetEnumerator()
		{
			return new KeyValuePairEnumerator(m_tree, EnumeratorObjectType.DictionaryEntryType);
		}

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
			return this.GetEnumerator();
        }

        public void Remove(object key)
        {
            ValidateKey(key);
            Remove((TKey)key);
        }

        #endregion

        #region ICollection<T> implementation

        public int Count 
        {
            get
            {
                return m_tree.Count;
            }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly 
        {
            get { return false; } 
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            m_tree.Add(item);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Clear()
        {
            m_tree.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            RedBlackTree<KeyValuePair<TKey, TValue>>.TreeNode node = m_tree.Find(item);
            return node != null && EqualityComparer<TValue>.Default.Equals(node.Item.Value, item.Value);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ValidateCopyToParameters(array, arrayIndex);
            RedBlackTree<KeyValuePair<TKey, TValue>>.TreeNode node = m_tree.First();
            while (node != null)
            {
                array[arrayIndex++] = node.Item;
                node = m_tree.Next(node);
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (!Contains(item))
                return false;

            m_tree.Remove(item);
            return true;
        }

        #endregion

        #region ICollection implementation

        //int ICollection.Count 
        //{
        //    get
        //    {
        //        return m_tree.Count;
        //    }
        //}

        public bool IsSynchronized 
        {
            get { return false; } 
        }

        public object SyncRoot
        {
            get { return m_syncRoot; } 
        }

        public void CopyTo(Array array, int index)
        {
            ValidateCopyToParameters(array, index);
            KeyValuePair<TKey, TValue>[] specificArray = array as KeyValuePair<TKey, TValue>[];
            if (specificArray != null)
                CopyTo(specificArray, index);
            else
            {
                Object[] objectArray = array as Object[];
                if (objectArray == null)
                {
                    throw new ArgumentException("Invalid Array Type");
                }
                else
                {
                    RedBlackTree<KeyValuePair<TKey, TValue>>.TreeNode node = m_tree.First();
                    while (node != null)
                    {
                        objectArray[index++] = node.Item;
                        node = m_tree.Next(node);
                    }
                }
            }
        }

        #endregion

        #region IEnumerable<T> implementation

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return new KeyValuePairEnumerator(m_tree, EnumeratorObjectType.KeyValuePairType);
        }

        #endregion

        #region IEnumerable implementation

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new KeyValuePairEnumerator(m_tree, EnumeratorObjectType.KeyValuePairType);
        }

        #endregion

        #region Internal Classes

        private class KeyValuePairComparer : IComparer<KeyValuePair<TKey, TValue>>
        {
            public KeyValuePairComparer(IComparer<TKey> comparer)
            {
                if (comparer == null)
                    m_comparer = Comparer<TKey>.Default;
                else
                    m_comparer = comparer;
            }
            public int Compare(KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y)
            {
                return m_comparer.Compare(x.Key, y.Key);
            }

            private IComparer<TKey> m_comparer;
        }

        private class KeyValuePairEnumeratorProvider : IGenericEnumeratorProvider<KeyValuePair<TKey, TValue>>
        {
            private RedBlackTree<KeyValuePair<TKey, TValue>>          m_tree;
            private RedBlackTree<KeyValuePair<TKey, TValue>>.TreeNode m_currentNode;
            private RedBlackTree<KeyValuePair<TKey, TValue>>.TreeNode m_firstNode;
            private bool m_afterReset;

            public KeyValuePairEnumeratorProvider(
                RedBlackTree<KeyValuePair<TKey, TValue>> tree, 
                RedBlackTree<KeyValuePair<TKey, TValue>>.TreeNode firstNode)
            {
                m_tree        = tree;
                m_firstNode = firstNode;
                m_currentNode = null;
                m_afterReset  = false;
            }

            public KeyValuePairEnumeratorProvider(
                RedBlackTree<KeyValuePair<TKey, TValue>> tree) : this(tree, tree.First())
            {
            }

            public KeyValuePair<TKey, TValue> Current 
            {
                get
                {
                    if (m_currentNode == null)
                        throw new InvalidOperationException("No current object");
                    return m_currentNode.Item;
                }
            }

            public void Reset()
            {
                m_afterReset = true;
            }

            public bool MoveNext()
            {
                if (m_afterReset)
                {
                    m_currentNode = m_firstNode;
                    m_afterReset = false;
                }
                else
                {
                    if (m_currentNode != null)
                        m_currentNode = m_tree.Next(m_currentNode);
                }
                return m_currentNode != null;
            }
        }

        private class KeyEnumeratorProvider : IGenericEnumeratorProvider<TKey>
        {
            private KeyValuePairEnumeratorProvider m_keyValuePairProvider;

            public KeyEnumeratorProvider(RedBlackTree<KeyValuePair<TKey, TValue>> tree)
            {
                m_keyValuePairProvider = new KeyValuePairEnumeratorProvider(tree);
            }

            public TKey Current
            {
                get { return m_keyValuePairProvider.Current.Key; }
            }

            public void Reset()
            {
                m_keyValuePairProvider.Reset();
            }

            public bool MoveNext()
            {
                return m_keyValuePairProvider.MoveNext();
            }
        }

        private class ValueEnumeratorProvider : IGenericEnumeratorProvider<TValue>
        {
            private KeyValuePairEnumeratorProvider m_ValueValuePairProvider;

            public ValueEnumeratorProvider(RedBlackTree<KeyValuePair<TKey, TValue>> tree)
            {
                m_ValueValuePairProvider = new KeyValuePairEnumeratorProvider(tree);
            }

            public TValue Current
            {
                get { return m_ValueValuePairProvider.Current.Value; }
            }

            public void Reset()
            {
                m_ValueValuePairProvider.Reset();
            }

            public bool MoveNext()
            {
                return m_ValueValuePairProvider.MoveNext();
            }
        }

        private enum EnumeratorObjectType
        {
            DictionaryEntryType, KeyValuePairType
        }
        private class KeyValuePairEnumerator : 
            GenericEnumerator<KeyValuePair<TKey, TValue>>, IEnumerator, IDictionaryEnumerator
        {
            private EnumeratorObjectType m_type;

            public KeyValuePairEnumerator(
                RedBlackTree<KeyValuePair<TKey, TValue>> tree, 
                EnumeratorObjectType type,
                RedBlackTree<KeyValuePair<TKey, TValue>>.TreeNode firstNode
                )
                : base(new KeyValuePairEnumeratorProvider(tree, firstNode))
            {
                m_type = type;
            }

            public KeyValuePairEnumerator(
                RedBlackTree<KeyValuePair<TKey, TValue>> tree, EnumeratorObjectType type)
                : base(new KeyValuePairEnumeratorProvider(tree))
            {
                m_type = type;
            }

            Object IEnumerator.Current
            {
                get
                {
                    KeyValuePair<TKey, TValue> item = GetCurrentItem();
                    if (m_type == EnumeratorObjectType.KeyValuePairType)
                        return item;
                    else
                        return new DictionaryEntry(item.Key, item.Value);
                }
            }

            object IDictionaryEnumerator.Key
            {
                get
                {
                    KeyValuePair<TKey, TValue> item = GetCurrentItem();
                    return item.Key;
                }
            }

            object IDictionaryEnumerator.Value
            {
                get
                {
                    KeyValuePair<TKey, TValue> item = GetCurrentItem();
                    return item.Value;
                }
            }

            DictionaryEntry IDictionaryEnumerator.Entry
            {
                get
                {
                    KeyValuePair<TKey, TValue> item = GetCurrentItem();
                    return new DictionaryEntry(item.Key, item.Value);
                }
            }
        }

        private class KeyEnumerator : GenericEnumerator<TKey>
        {
            public KeyEnumerator(RedBlackTree<KeyValuePair<TKey, TValue>> tree)
                : base(new KeyEnumeratorProvider(tree))
            {
            }
        }

        private class ValueEnumerator : GenericEnumerator<TValue>
        {
            public ValueEnumerator(RedBlackTree<KeyValuePair<TKey, TValue>> tree)
                : base(new ValueEnumeratorProvider(tree))
            {
            }
        }

        public class SortedMapCollectionBase<T> : ICollection<T>, IEnumerable<T>, ICollection, IEnumerable
        {
			protected SortedMap<TKey, TValue> m_sortedMap;

			public SortedMapCollectionBase(SortedMap<TKey, TValue> sortedMap)
			{
				m_sortedMap = sortedMap;
			}

			public int Count
			{
				get { return m_sortedMap.Count; }
			}

			public bool IsReadOnly
			{
				get { return true; }
			}

			public void Add(T item)
			{
				throw new NotSupportedException();
			}

			public void Clear()
			{
				throw new NotSupportedException();
			}

			public bool Remove(T item)
			{
				throw new NotSupportedException();
			}

			public bool Contains(T item)
			{
				throw new NotSupportedException();
			}

			public void CopyTo(T[] array, int arrayIndex)
			{
				throw new NotSupportedException();
			}

			public bool IsSynchronized
			{
				get { return false; }
			}

			public object SyncRoot
			{
				get { return m_sortedMap.SyncRoot; }
			}

			public void CopyTo(Array array, int index)
			{
				throw new NotSupportedException();
			}

			public IEnumerator<T> GetEnumerator()
			{
				throw new NotSupportedException();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				throw new NotSupportedException();
			}
        }

        public class SortedMapKeyCollection :
            SortedMapCollectionBase<TKey>, ICollection<TKey>, IEnumerable<TKey>, ICollection, IEnumerable
        {
            public SortedMapKeyCollection(SortedMap<TKey, TValue> sortedMap) : base(sortedMap)
            {
            }

            bool ICollection<TKey>.Contains(TKey key)
            {
                return m_sortedMap.FindByKey(key) != null;
            }

            void ICollection<TKey>.CopyTo(TKey[] array, int arrayIndex)
            {
                m_sortedMap.ValidateCopyToParameters(array, arrayIndex);
                RedBlackTree<KeyValuePair<TKey, TValue>>.TreeNode node = m_sortedMap.m_tree.First();
                while (node != null)
                {
                    array[arrayIndex++] = node.Item.Key;
                    node = m_sortedMap.m_tree.Next(node);
                }
            }

            public new void CopyTo(Array array, int index)
            {
                m_sortedMap.ValidateCopyToParameters(array, index);
                TKey[] specificArray = array as TKey[];
                if (specificArray == null)
                {
                    throw new ArgumentException("Invalid Array Type");
                }
                else
                {
                    CopyTo(specificArray, index);
                }
            }

            IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator()
            {
                return new KeyEnumerator(m_sortedMap.m_tree);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return new KeyEnumerator(m_sortedMap.m_tree);
            }
        }


        public class SortedMapValueCollection :
            SortedMapCollectionBase<TValue>, ICollection<TValue>, IEnumerable<TValue>, ICollection, IEnumerable
        {
            public SortedMapValueCollection(SortedMap<TKey, TValue> sortedMap)
                : base(sortedMap)
            {
            }

            bool ICollection<TValue>.Contains(TValue value)
            {
                EqualityComparer<TValue> valueComparer = EqualityComparer<TValue>.Default;
                RedBlackTree<KeyValuePair<TKey, TValue>>.TreeNode node = m_sortedMap.m_tree.First();
                while (node != null)
                {
                    if ((value == null && node.Item.Value == null) ||
                        valueComparer.Equals(node.Item.Value, value))
                        return true;

                    node = m_sortedMap.m_tree.Next(node);
                }
                return false;
            }

            void ICollection<TValue>.CopyTo(TValue[] array, int arrayIndex)
            {
                m_sortedMap.ValidateCopyToParameters(array, arrayIndex);
                RedBlackTree<KeyValuePair<TKey, TValue>>.TreeNode node = m_sortedMap.m_tree.First();
                while (node != null)
                {
                    array[arrayIndex++] = node.Item.Value;
                    node = m_sortedMap.m_tree.Next(node);
                }
            }

            public new void CopyTo(Array array, int index)
            {
                m_sortedMap.ValidateCopyToParameters(array, index);
                TValue[] specificArray = array as TValue[];
                if (specificArray == null)
                {
                    throw new ArgumentException("Invalid Array Type");
                }
                else
                {
                    CopyTo(specificArray, index);
                }
            }

            IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
            {
                return new ValueEnumerator(m_sortedMap.m_tree);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return new ValueEnumerator(m_sortedMap.m_tree);
            }
        }

        #endregion

        #region Private Properties

        public SortedMapKeyCollection Keys
        {
            get
            {
                if (m_keys == null)
                    m_keys = new SortedMapKeyCollection(this);
                return m_keys;
            }
        }

        public SortedMapValueCollection Values
        {
            get
            {
                if (m_values == null)
                    m_values = new SortedMapValueCollection(this);
                return m_values;
            }
        }

        #endregion

        #region Private Methods

        RedBlackTree<KeyValuePair<TKey, TValue>>.TreeNode LowerBoundEqual(TKey key)
        {
            return m_tree.FindGreaterEqual(DefaultItem(key));
        }

        private RedBlackTree<KeyValuePair<TKey, TValue>>.TreeNode UpperBoundNode(TKey key)
        {
            RedBlackTree<KeyValuePair<TKey, TValue>>.TreeNode node = LowerBoundEqual(key);

            // If found the exact key - move to next
            if (node != null && Comparer<TKey>.Default.Compare(node.Item.Key, key) == 0)
                node = m_tree.Next(node);

            return node;
        }

        private TValue NodeValue(RedBlackTree<KeyValuePair<TKey, TValue>>.TreeNode node)
        {
            if (node == null)
                throw new KeyNotFoundException();
            else
                return node.Item.Value;
        }

        private KeyValuePair<TKey, TValue> DefaultItem(TKey key)
        {
            return new KeyValuePair<TKey, TValue>(key, default(TValue));
        }

        RedBlackTree<KeyValuePair<TKey, TValue>>.TreeNode FindByKey(TKey key)
        {
            return m_tree.Find(DefaultItem(key));
        }

        private bool ValidateKey(Object key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            if (!(key is TKey))
            {
                throw new ArgumentException("Wrong Key Type");
            }
            return true;
        }

        private bool ValidateValue(Object value)
        {
            if (!(value is TValue) && (value != null))
            {
                throw new ArgumentException("Wrong Value Type");
            }
            return true;
        }

        private void ValidateCopyToParameters(Array array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            if (array.Rank != 1)
            {
                throw new ArgumentException("rank should be 1");
            }
            if (array.GetLowerBound(0) != 0)
            {
                throw new ArgumentException("lower bound should be 1");
            }
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            if ((array.Length - index) < this.Count)
            {
                throw new ArgumentException("Array Too Small");
            }
        }

        #endregion

        #region Private Members

        private RedBlackTree<KeyValuePair<TKey, TValue>> m_tree;
        private SortedMapKeyCollection                   m_keys;
        private SortedMapValueCollection                 m_values;
        private Object m_syncRoot = new Object();

        #endregion
    }
}
