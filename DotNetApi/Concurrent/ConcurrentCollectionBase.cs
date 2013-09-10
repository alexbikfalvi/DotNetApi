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
		private readonly ConcurrentList<object> list;

		/// <summary>
		/// Creates an empty concurrent list instance with the default capacity.
		/// </summary>
		public ConcurrentCollectionBase()
		{
			this.list = new ConcurrentList<object>();
		}

		/// <summary>
		/// Creates an empty concurrent list with the specified capacity.
		/// </summary>
		/// <param name="capacity">The capacity.</param>
		public ConcurrentCollectionBase(int capacity)
		{
			this.list = new ConcurrentList<object>(capacity);
		}

		// Public properties.

		///// <summary>
		///// Gets the number of elements contained in the list.
		///// </summary>
		//public int Count
		//{
		//	get
		//	{
		//		// Acquire a reader lock.
		//		this.AcquireReaderLock();
		//		try
		//		{
		//			// Return the list count.
		//			return this.list.Count;
		//		}
		//		finally
		//		{
		//			// Release the reader lock.
		//			this.ReleaseReaderLock();
		//		}
		//	}
		//}

		///// <summary>
		///// Gets a value indicating whether the list is read-only. The property is always <b>false</b>.
		///// </summary>
		//public bool IsReadOnly { get { return false; } }

		///// <summary>
		///// Gets or sets the element at the specified index.
		///// </summary>
		///// <param name="index">The index.</param>
		///// <returns>The element.</returns>
		//public object this[int index]
		//{
		//	get
		//	{
		//		return this.list[index];
		//	}
		//	set
		//	{
		//		// Validate the element.
		//		this.OnValidate(value);
		//		// Get the old value.
		//		object old = this.list[index];
		//		// Call the event handler.
		//		this.OnSet(index, old, value);
		//		// Set the value.
		//		this.list[index] = value;
		//		// Call the event handler.
		//		this.OnSetComplete(index, old, value);
		//	}
		//}

		// Protected properties.

		/// <summary>
		/// Gets a list containing the elements in the concurrent collection base instance.
		/// </summary>
		protected IList List
		{
			get { return this.list; }
		}

		//// Public methods.

		///// <summary>
		///// Adds an item to the list.
		///// </summary>
		///// <param name="item">The item.</param>
		//public void Add(object item)
		//{
		//	// Acquire a writer lock.
		//	LockCookie? cookie = this.AcquireWriterLock();
		//	try
		//	{
		//		// Add the list item.
		//		this.list.Add(item);
		//	}
		//	finally
		//	{
		//		// Release the writer lock.
		//		this.ReleaseWriterLock(cookie);
		//	}
		//}

		///// <summary>
		///// Clears the collection.
		///// </summary>
		//public void Clear()
		//{
		//	// Acquire a writer lock.
		//	LockCookie? cookie = this.AcquireWriterLock();
		//	try
		//	{
		//		// Clear the list.
		//		this.list.Clear();
		//	}
		//	finally
		//	{
		//		// Release the writer lock.
		//		this.ReleaseWriterLock(cookie);
		//	}
		//}

		///// <summary>
		///// Indicates whether the list contains the specified item.
		///// </summary>
		///// <param name="item">The item.</param>
		///// <returns><b>True</b> if the list contains the item, <b>false</b> otherwise.</returns>
		//public bool Contains(object item)
		//{
		//	// Acquire a reader lock.
		//	this.AcquireReaderLock();
		//	try
		//	{
		//		return this.list.Contains(item);
		//	}
		//	finally
		//	{
		//		// Release the reader lock.
		//		this.ReleaseReaderLock();
		//	}
		//}

		///// <summary>
		///// Returns the enumeator for the current list.
		///// </summary>
		///// <returns>The enumerator.</returns>
		//IEnumerator IEnumerable.GetEnumerator()
		//{
		//	return this.GetEnumerator();
		//}

		///// <summary>
		///// Returns the enumerator for the current list.
		///// </summary>
		///// <returns>The enumerator.</returns>
		//public IEnumerator<object> GetEnumerator()
		//{
		//	// The thread must first acquire a lock before retrieving the enumerator.
		//	if (!this.sync.IsReaderLockHeld && !this.sync.IsWriterLockHeld) throw new SynchronizationLockException();
		//	return this.list.GetEnumerator();
		//}

		///// <summary>
		///// Copies all elements to the specified array starting from the index.
		///// </summary>
		///// <param name="array">The array.</param>
		///// <param name="index">The index.</param>
		//public void CopyTo(object[] array, int index)
		//{
		//	// Acquire a reader lock.
		//	this.AcquireReaderLock();
		//	try
		//	{
		//		// Copy the list to the array.
		//		this.list.CopyTo(array);
		//	}
		//	finally
		//	{
		//		// Release the reader lock.
		//		this.ReleaseReaderLock();
		//	}
		//}

		///// <summary>
		///// Determines the index of a specific item in the list.
		///// </summary>
		///// <param name="item">The item.</param>
		///// <returns>The index, or -1 if the item is not found.</returns>
		//public int IndexOf(object item)
		//{
		//	// Acquire a reader lock.
		//	this.AcquireReaderLock();
		//	try
		//	{
		//		// Return the index.
		//		return this.list.IndexOf(item);
		//	}
		//	finally
		//	{
		//		// Release the reader lock.
		//		this.ReleaseReaderLock();
		//	}
		//}

		///// <summary>
		///// Inserts the specified item in the list.
		///// </summary>
		///// <param name="index">The index.</param>
		///// <param name="item">The item.</param>
		//public void Insert(int index, object item)
		//{
		//	// Acquire a writer lock.
		//	LockCookie? cookie = this.AcquireWriterLock();
		//	try
		//	{
		//		// Insert the item.
		//		this.list.Insert(index, item);
		//	}
		//	finally
		//	{
		//		// Release the writer lock.
		//		this.ReleaseWriterLock(cookie);
		//	}
		//}

		///// <summary>
		///// Removes the first occurrence of the specified item from the collection.
		///// </summary>
		///// <param name="item">The item.</param>
		///// <returns><b>True</b> if the item was successfully removed,<b>false</b> otherwise.</returns>
		//public bool Remove(object item)
		//{
		//	// Acquire a writer lock.
		//	LockCookie? cookie = this.AcquireWriterLock();
		//	try
		//	{
		//		// Remove the item.
		//		return this.list.Remove(item);
		//	}
		//	finally
		//	{
		//		// Release the writer lock.
		//		this.ReleaseWriterLock(cookie);
		//	}
		//}

		///// <summary>
		///// Removes the item at the specified index.
		///// </summary>
		///// <param name="index">The index.</param>
		//public void RemoveAt(int index)
		//{
		//	// Acquire a writer lock.
		//	LockCookie? cookie = this.AcquireWriterLock();
		//	try
		//	{
		//		// Remove the item.
		//		this.list.RemoveAt(index);
		//	}
		//	finally
		//	{
		//		// Release the writer lock.
		//		this.ReleaseWriterLock(cookie);
		//	}
		//}

		/// <summary>
		/// Acquires a reader lock for the current list.
		/// </summary>
		public void Lock()
		{
			this.sync.AcquireReaderLock(-1);
		}

		/// <summary>
		/// Tries to acquire a reader lock for the current list.
		/// </summary>
		/// <returns><b>True</b> if the lock has been acquired, <b>false</b> otherwise.</returns>
		public bool TryLock()
		{
			try
			{
				this.sync.AcquireReaderLock(0);
				return true;
			}
			catch (ApplicationException)
			{
				return false;
			}
		}

		/// <summary>
		/// Releases the previous reader lock or the reader lock that has been upgraded to a writer lock.
		/// </summary>
		public void Unlock()
		{
			this.sync.ReleaseReaderLock();
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
