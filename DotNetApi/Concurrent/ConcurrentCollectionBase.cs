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

namespace DotNetApi.Concurrent
{
	/// <summary>
	/// A class representing a concurrent list.
	/// </summary>
	public abstract class ConcurrentCollectionBase : IList, ICollection, IEnumerable
	{
		private readonly ConcurrentList list;

		/// <summary>
		/// Creates an empty concurrent list instance with the default capacity.
		/// </summary>
		public ConcurrentCollectionBase()
		{
			this.list = new ConcurrentList();
		}

		/// <summary>
		/// Creates an empty concurrent list with the specified capacity.
		/// </summary>
		/// <param name="capacity">The capacity.</param>
		public ConcurrentCollectionBase(int capacity)
		{
			this.list = new ConcurrentList(capacity);
		}

		// Public properties.

		/// <summary>
		/// Gets the number of elements contained in the list.
		/// </summary>
		public int Count { get { return this.list.Count; } }
		/// <summary>
		/// Gets a value indicating whether the list is read-only.
		/// </summary>
		public bool IsReadOnly { get { return this.list.IsReadOnly; } }
		/// <summary>
		/// Gets a value indicating whether the list is fixed in size.
		/// </summary>
		public bool IsFixedSize { get { return this.list.IsFixedSize; } }
		/// <summary>
		/// Gets a value indicating whether the list is synchronized.
		/// </summary>
		public bool IsSynchronized { get { return this.list.IsSynchronized; } }
		/// <summary>
		/// Gets the object used for synchronizing list operations.
		/// </summary>
		public object SyncRoot { get { return this.list.SyncRoot; } }

		/// <summary>
		/// Gets or sets the element at the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns>The element.</returns>
		public object this[int index]
		{
			get 
			{
				return this.list[index];
			}
			set
			{
				// Validate the element.
				this.OnValidate(value);
				// Get the old value.
				object old = this.list[index];
				// Call the event handler.
				this.OnSet(index, old, value);
				// Set the value.
				this.list[index] = value;
				// Call the event handler.
				this.OnSetComplete(index, old, value);
			}
		}

		// Protected properties.

		/// <summary>
		/// Gets a list containing the elements in the concurrent collection base instance.
		/// </summary>
		//protected IList List
		//{
		//	get { return this.list; }
		//}

		//// Public methods.

		/// <summary>
		/// Adds an item to the list.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns>The position to which the item was added.</returns>
		public int Add(object item)
		{
			// Validate the element.
			this.OnValidate(item);
			// Call the event handler.
			this.OnInsert(this.list.Count, item);
			// Add the object to the list.
			int index = this.list.Add(item);
			// Call the event handler.
			this.OnInsertComplete(index, item);
			// Return the index.
			return index;
		}

		/// <summary>
		/// Clears the collection.
		/// </summary>
		public void Clear()
		{
			// Call the event handler.
			this.OnClear();
			// Clear the list.
			this.list.Clear();
			// Call the event handler.
			this.OnClearComplete();
		}

		/// <summary>
		/// Indicates whether the list contains the specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns><b>True</b> if the list contains the item, <b>false</b> otherwise.</returns>
		public bool Contains(object item)
		{
			return this.list.Contains(item);
		}

		/// <summary>
		/// Returns the enumeator for the current list.
		/// </summary>
		/// <returns>The enumerator.</returns>
		public IEnumerator GetEnumerator()
		{
			return this.list.GetEnumerator();
		}

		/// <summary>
		/// Copies all elements to the specified array starting from the index.
		/// </summary>
		/// <param name="array">The array.</param>
		/// <param name="index">The index.</param>
		public void CopyTo(Array array, int index)
		{
			this.list.CopyTo(array, index);
		}

		/// <summary>
		/// Determines the index of a specific item in the list.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns>The index, or -1 if the item is not found.</returns>
		public int IndexOf(object item)
		{
			return this.list.IndexOf(item);
		}

		/// <summary>
		/// Inserts the specified item in the list.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="item">The item.</param>
		public void Insert(int index, object item)
		{
			// Call the event handler.
			this.OnInsert(index, item);
			// Insert the item.
			this.list.Insert(index, item);
			// Call the event handler.
			this.OnInsertComplete(index, item);
		}

		/// <summary>
		/// Removes the first occurrence of the specified item from the collection.
		/// </summary>
		/// <param name="item">The item.</param>
		public void Remove(object item)
		{
			// Get the item index.
			int index = this.IndexOf(item);
			// Call the event handler.
			this.OnRemove(index, item);
			// Remove the item.
			this.Remove(item);
		}

		/// <summary>
		/// Removes the item at the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		public void RemoveAt(int index)
		{
			// Get the item.
			object item = this[index];
			// Call the event handler.
			this.OnRemove(index, item);
			// Remove the item.
			this.RemoveAt(index);
			// Call the event handler.
			this.OnRemoveComplete(index, item);
		}

		/// <summary>
		/// Acquires a lock for the current collection.
		/// </summary>
		public void Lock()
		{
			this.list.Lock();
		}

		/// <summary>
		/// Tries to acquire a reader lock for the current collection.
		/// </summary>
		/// <returns><b>True</b> if the lock has been acquired, <b>false</b> otherwise.</returns>
		public bool TryLock()
		{
			return this.list.TryLock();
		}

		/// <summary>
		/// Releases the previous reader lock or the reader lock that has been upgraded to a writer lock.
		/// </summary>
		public void Unlock()
		{
			this.list.Unlock();
		}

		// Protected methods.

		/// <summary>
		/// An event handler called before clearing the item collection.
		/// </summary>
		protected virtual void OnClear()
		{
		}

		/// <summary>
		/// An event handler called after clearing the item collection.
		/// </summary>
		protected virtual void OnClearComplete()
		{
		}

		/// <summary>
		/// An event handler called before inserting an item into the collection.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="value">The item.</param>
		protected virtual void OnInsert(int index, object value)
		{
		}

		/// <summary>
		/// An event handler called after inserting an item into the collection.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="value">The item.</param>
		protected virtual void OnInsertComplete(int index, object value)
		{
		}

		/// <summary>
		/// An event handler called before removing an item from the collection.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="value">The item.</param>
		protected virtual void OnRemove(int index, object value)
		{
		}

		/// <summary>
		/// An event handler called after removing an item from the collection.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="value">The item.</param>
		protected virtual void OnRemoveComplete(int index, object value)
		{
		}

		/// <summary>
		/// An event handler called before setting the value of an item from the collection.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="oldValue">The old item.</param>
		/// <param name="newValue">The new item.</param>
		protected virtual void OnSet(int index, object oldValue, object newValue)
		{
		}

		/// <summary>
		/// An event handler called after setting the value of an item from the collection.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="oldValue">The old item.</param>
		/// <param name="newValue">The new item.</param>
		protected virtual void OnSetComplete(int index, object oldValue, object newValue)
		{
		}

		/// <summary>
		/// Validates the current object as a collection item.
		/// </summary>
		/// <param name="value">The object.</param>
		protected virtual void OnValidate(object value)
		{
		}
	}
}
