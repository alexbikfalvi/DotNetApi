/* 
 * Copyright (C) 2013 Alex Bikfalvi
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 3 of the License, or (at
 * your option) any later version.
 *
 * This program is distributed in the hope that it will be useful, but
 * WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301, USA.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using DotNetApi.Concurrent;

namespace DotNetApi.Concurrent.Generic
{
	/// <summary>
	/// A class representing a concurrent generic dictionary.
	/// </summary>
	/// <typeparam name="T">The list element type.</typeparam>
	public class ConcurrentDictionary<TKey, TValue> : ConcurrentBase, IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IDictionary, ICollection, IEnumerable
	{
		private readonly Dictionary<TKey, TValue> dictionary;
		private readonly object syncRoot = new object();

		// Constructors.

		/// <summary>
		/// Creates a new dictionary instance that is empty, has the default initial capacity, and uses the default equality comparer for the key type.
		/// </summary>
		public ConcurrentDictionary()
		{
			this.dictionary = new Dictionary<TKey, TValue>();
		}

		/// <summary>
		/// Creates a new dictionary instance that contains elements copied from the specified dictionary and uses the default equality comparer for the key type.
		/// </summary>
		/// <param name="dictionary">The dictionary from where to copy the elements.</param>
		public ConcurrentDictionary(IDictionary<TKey, TValue> dictionary)
		{
			this.dictionary = new Dictionary<TKey, TValue>(dictionary);
		}

		/// <summary>
		/// Creates a new dictionary instance that is empty, has the default initial capacity, and uses the specified comparer.
		/// </summary>
		/// <param name="comparer">The comparer.</param>
		public ConcurrentDictionary(IEqualityComparer<TKey> comparer)
		{
			this.dictionary = new Dictionary<TKey, TValue>(comparer);
		}

		/// <summary>
		/// Creates a new dictionary instance that is empty, has the specified initial capacity, and uses the default equality comparer for the key type.
		/// </summary>
		/// <param name="capacity">The capacity.</param>
		public ConcurrentDictionary(int capacity)
		{
			this.dictionary = new Dictionary<TKey, TValue>(capacity);
		}

		/// <summary>
		/// Creates a new dictionary instance that is empty, has the specified initial capacity, and uses the specified comparer.
		/// </summary>
		/// <param name="capacity">The capacity.</param>
		/// <param name="comparer">The comparer.</param>
		public ConcurrentDictionary(int capacity, IEqualityComparer<TKey> comparer)
		{
			this.dictionary = new Dictionary<TKey, TValue>(capacity, comparer);
		}

		// Public properties.

		/// <summary>
		/// Gets or sets the value associated with the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>The value.</returns>
		public object this[object key]
		{
			get
			{
				// Acquire a reader lock.
				LockInfo info = this.AcquireReaderLock();
				try
				{
					// Return the dictionary item.
					return (this as IDictionary)[key];
				}
				finally
				{
					// Release the reader lock.
					this.ReleaseReaderLock(info);
				}
			}
			set
			{
				// Acquire a writer lock.
				LockInfo info = this.AcquireWriterLock();
				try
				{
					// Set the dictionary item.
					(this.dictionary as IDictionary)[key] = value;
				}
				finally
				{
					// Release the writer lock.
					this.ReleaseWriterLock(info);
				}
			}
		}

		/// <summary>
		/// Gets or sets the value associated with the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>The value.</returns>
		public TValue this[TKey key]
		{
			get
			{
				// Acquire a reader lock.
				LockInfo info = this.AcquireReaderLock();
				try
				{
					// Return the dictionary item.
					return this.dictionary[key];
				}
				finally
				{
					// Release the reader lock.
					this.ReleaseReaderLock(info);
				}
			}
			set
			{
				// Acquire a writer lock.
				LockInfo info = this.AcquireWriterLock();
				try
				{
					// Set the dictionary item.
					this.dictionary[key] = value;
				}
				finally
				{
					// Release the writer lock.
					this.ReleaseWriterLock(info);
				}
			}
		}
		/// <summary>
		/// Gets a collection containing the keys in the dictionary.
		/// </summary>
		ICollection IDictionary.Keys
		{
			get
			{
				// The thread must first acquire a lock before retrieving the enumerator.
				if (!this.HasLock()) throw new InvalidOperationException("Thread must own a lock on this concurrent collection.");
				return (this.dictionary as IDictionary).Keys;
			}
		}
		/// <summary>
		/// Gets a collection containing the keys in the dictionary.
		/// </summary>
		public ICollection<TKey> Keys
		{
			get
			{
				// The thread must first acquire a lock before retrieving the enumerator.
				if (!this.HasLock()) throw new InvalidOperationException("Thread must own a lock on this concurrent collection.");
				return this.dictionary.Keys;
			}
		}
		/// <summary>
		/// Gets a collection containing the values in the dictionary.
		/// </summary>
		ICollection IDictionary.Values
		{
			get
			{
				// The thread must first acquire a lock before retrieving the enumerator.
				if (!this.HasLock()) throw new InvalidOperationException("Thread must own a lock on this concurrent collection.");
				return (this.dictionary as IDictionary).Values;
			}
		}
		/// <summary>
		/// Gets a collection containing the values in the dictionary.
		/// </summary>
		public ICollection<TValue> Values
		{
			get
			{
				// The thread must first acquire a lock before retrieving the enumerator.
				if (!this.HasLock()) throw new InvalidOperationException("Thread must own a lock on this concurrent collection.");
				return this.dictionary.Values;
			}
		}
		/// <summary>
		/// Gets the number of key/value pairs contained in the dictionary.
		/// </summary>
		public int Count
		{
			get
			{
				// Acquire a reader lock.
				LockInfo info = this.AcquireReaderLock();
				try
				{
					// Return the dictionary count.
					return this.dictionary.Count;
				}
				finally
				{
					// Release the reader lock.
					this.ReleaseReaderLock(info);
				}
			}
		}
		/// <summary>
		/// Gets a value indicating whether the dictionary is read-only. The property is always <b>false</b>.
		/// </summary>
		public bool IsReadOnly { get { return false; } }
		/// <summary>
		/// Gets a value indicating whether the list is synchronized. The property is always <b>true</b>.
		/// </summary>
		public bool IsSynchronized { get { return true; } }
		/// <summary>
		/// Gets a value indicating whether the dictionary is fixed in size. The property is always <b>false</b>.
		/// </summary>
		public bool IsFixedSize { get { return false; } }
		/// <summary>
		/// Gets the object used for synchronizing list operations.
		/// </summary>
		public object SyncRoot { get { return this.syncRoot; } }

		// Public methods.

		/// <summary>
		/// Returns an enumerator that iterates through the dictionary.
		/// </summary>
		/// <returns>The enumerator.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			// The thread must first acquire a lock before retrieving the enumerator.
			if (!this.HasLock()) throw new InvalidOperationException("Thread must own a lock on this concurrent collection.");
			return (this.dictionary as IEnumerable).GetEnumerator();
		}

		/// <summary>
		/// Returns an enumerator that iterates through the dictionary.
		/// </summary>
		/// <returns>The enumerator.</returns>
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			// The thread must first acquire a lock before retrieving the enumerator.
			if (!this.HasLock()) throw new InvalidOperationException("Thread must own a lock on this concurrent collection.");
			return (this.dictionary as IDictionary).GetEnumerator();
		}

		/// <summary>
		/// Returns an enumerator that iterates through the dictionary.
		/// </summary>
		/// <returns>The enumerator.</returns>
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			// The thread must first acquire a lock before retrieving the enumerator.
			if (!this.HasLock()) throw new InvalidOperationException("Thread must own a lock on this concurrent collection.");
			return this.dictionary.GetEnumerator();
		}

		/// <summary>
		/// Adds the specified key and value to the dictionary.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		public void Add(object key, object value)
		{
			// Acquire a writer lock.
			LockInfo info = this.AcquireWriterLock();
			try
			{
				// Add the dictionary item.
				(this.dictionary as IDictionary).Add(key, value);
			}
			finally
			{
				// Release the writer lock.
				this.ReleaseWriterLock(info);
			}
		}

		/// <summary>
		/// Adds an item to the dictionary collection.
		/// </summary>
		/// <param name="item">The item.</param>
		public void Add(KeyValuePair<TKey, TValue> item)
		{
			// Acquire a writer lock.
			LockInfo info = this.AcquireWriterLock();
			try
			{
				// Add the dictionary item.
				(this.dictionary as ICollection<KeyValuePair<TKey, TValue>>).Add(item);
			}
			finally
			{
				// Release the writer lock.
				this.ReleaseWriterLock(info);
			}
		}

		/// <summary>
		/// Adds the specified key and value to the dictionary.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The value.</param>
		public void Add(TKey key, TValue value)
		{
			// Acquire a writer lock.
			LockInfo info = this.AcquireWriterLock();
			try
			{
				// Add the dictionary item.
				this.dictionary.Add(key, value);
			}
			finally
			{
				// Release the writer lock.
				this.ReleaseWriterLock(info);
			}
		}

		/// <summary>
		/// Removes all keys and values from the dictionary.
		/// </summary>
		public void Clear()
		{
			// Acquire a writer lock.
			LockInfo info = this.AcquireWriterLock();
			try
			{
				// Clear the dictionary.
				this.dictionary.Clear();
			}
			finally
			{
				// Release the writer lock.
				this.ReleaseWriterLock(info);
			}
		}

		/// <summary>
		/// Determines whether the dictionary object contains an element with the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns><b>True</b> if the dictionary contains an element with the key; otherwise, <b>false</b>.</returns>
		public bool Contains(object key)
		{
			// Acquire a reader lock.
			LockInfo info = this.AcquireReaderLock();
			try
			{
				// Return whether the dictionary contains the key.
				return (this.dictionary as IDictionary).Contains(key);
			}
			finally
			{
				// Release the reader lock.
				this.ReleaseReaderLock(info);
			}
		}

		/// <summary>
		/// Determines whether the collection contains the specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns><b>True</b> if item is found in the collection; otherwise <b>false</b>.</returns>
		public bool Contains(KeyValuePair<TKey, TValue> item)
		{
			// Acquire a reader lock.
			LockInfo info = this.AcquireReaderLock();
			try
			{
				// Return whether the dictionary contains the item.
				return (this.dictionary as ICollection<KeyValuePair<TKey, TValue>>).Contains(item);
			}
			finally
			{
				// Release the reader lock.
				this.ReleaseReaderLock(info);
			}
		}

		/// <summary>
		/// Copies the elements of the dictionary to an array, starting at a particular array index.
		/// </summary>
		/// <param name="array">The array.</param>
		/// <param name="index">The index.</param>
		public void CopyTo(Array array, int index)
		{
			// Acquire a reader lock.
			LockInfo info = this.AcquireReaderLock();
			try
			{
				// Copy the dictionary collection to the array.
				(this.dictionary as ICollection).CopyTo(array, index);
			}
			finally
			{
				// Release the reader lock.
				this.ReleaseReaderLock(info);
			}
		}

		/// <summary>
		/// Copies the elements of the dictionary to an array, starting at a particular array index.
		/// </summary>
		/// <param name="array">The array.</param>
		/// <param name="index">The index.</param>
		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
		{
			// Acquire a reader lock.
			LockInfo info = this.AcquireReaderLock();
			try
			{
				// Copy the dictionary collection to the array.
				(this.dictionary as ICollection<KeyValuePair<TKey, TValue>>).CopyTo(array, index);
			}
			finally
			{
				// Release the reader lock.
				this.ReleaseReaderLock(info);
			}
		}

		/// <summary>
		/// Removes the value with the specified key from the dictionary.
		/// </summary>
		/// <param name="key">The key.</param>
		public void Remove(object key)
		{
			// Acquire a writer lock.
			LockInfo info = this.AcquireWriterLock();
			try
			{
				// Remove the key from the dictionary.
				this.Remove(key);
			}
			finally
			{
				// Release the writer lock.
				this.ReleaseWriterLock(info);
			}
		}

		/// <summary>
		/// Removes the specified item from the dictionary.
		/// </summary>
		/// <param name="key">The item.</param>
		public bool Remove(KeyValuePair<TKey, TValue> item)
		{
			// Acquire a writer lock.
			LockInfo info = this.AcquireWriterLock();
			try
			{
				// Remove the item from the dictionary.
				return (this as ICollection<KeyValuePair<TKey, TValue>>).Remove(item);
			}
			finally
			{
				// Release the writer lock.
				this.ReleaseWriterLock(info);
			}
		}

		/// <summary>
		/// Determines whether the dictionary contains the specified key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns><b>True</b> if the dictionary contains an element with the specified key; otherwise <b>false</b>.</returns>
		public bool ContainsKey(TKey key)
		{
			// Acquire a reader lock.
			LockInfo info = this.AcquireReaderLock();
			try
			{
				// Return whether the dictionary contains the specified key.
				return this.dictionary.ContainsKey(key);
			}
			finally
			{
				// Release the reader lock.
				this.ReleaseReaderLock(info);
			}
		}

		/// <summary>
		/// Removes the value with the specified key from the dictionary.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns><b>True</b> if the element is successfully found and removed; otherwise, <b>false.</b> This method returns <b>false</b> if key is not found in the dictionary.</returns>
		public bool Remove(TKey key)
		{
			// Acquire a writer lock.
			LockInfo info = this.AcquireWriterLock();
			try
			{
				// Add the key and value to the dictionary.
				return this.dictionary.Remove(key);
			}
			finally
			{
				// Release the writer lock.
				this.ReleaseWriterLock(info);
			}
		}

		/// <summary>
		/// Gets the value associated with the specified key.
		/// </summary>
		/// <param name="key">The key of the value to get.</param>
		/// <param name="value">When this method returns, contains the value associated with the specified key, if the key is found; otherwise, the default value for the type of the value parameter. This parameter is passed uninitialized.</param>
		/// <returns><b>True</b> if the dictionary contains an element with the specified key; otherwise <b>false</b>.</returns>
		public bool TryGetValue(TKey key, out TValue value)
		{
			// Acquire a reader lock.
			LockInfo info = this.AcquireReaderLock();
			try
			{
				// Try and get the value from the dictionary.
				return this.dictionary.TryGetValue(key, out value);
			}
			finally
			{
				// Release the reader lock.
				this.ReleaseReaderLock(info);
			}
		}
	}
}
