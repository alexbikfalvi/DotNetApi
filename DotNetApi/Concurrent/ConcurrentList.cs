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

namespace DotNetApi.Concurrent
{
	/// <summary>
	/// A class representing a concurrent list.
	/// </summary>
	public class ConcurrentList : ConcurrentBase, IList, ICollection, IEnumerable
	{
		private readonly ArrayList list;
		private readonly object syncRoot = new object();

		/// <summary>
		/// Creates an empty concurrent list instance with the default capacity.
		/// </summary>
		public ConcurrentList()
		{
			this.list = new ArrayList();
		}

		/// <summary>
		/// Creates an empty concurrent list with the specified capacity.
		/// </summary>
		/// <param name="capacity">The capacity.</param>
		public ConcurrentList(int capacity)
		{
			this.list = new ArrayList(capacity);
		}

		// Public properties.

		/// <summary>
		/// Gets the number of elements contained in the list.
		/// </summary>
		public int Count
		{
			get
			{
				// Acquire a reader lock.
				LockInfo info = this.AcquireReaderLock();
				try
				{
					// Return the list count.
					return this.list.Count;
				}
				finally
				{
					// Release the reader lock.
					this.ReleaseReaderLock(info);
				}
			}
		}
		/// <summary>
		/// Gets a value indicating whether the list is read-only. The property is always <b>false</b>.
		/// </summary>
		public bool IsReadOnly { get { return false; } }
		/// <summary>
		/// Gets a value indicating whether the list is fixed in size. The property is always <b>false</b>.
		/// </summary>
		public bool IsFixedSize { get { return false; } }
		/// <summary>
		/// Gets a value indicating whether the list is synchronized. The property is always <b>true</b>.
		/// </summary>
		public bool IsSynchronized { get { return true; } }
		/// <summary>
		/// Gets the object used for synchronizing list operations.
		/// </summary>
		public object SyncRoot { get { return this.syncRoot; } }

		/// <summary>
		/// Gets or sets the element at the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns>The element.</returns>
		public object this[int index]
		{
			get
			{
				// Acquire a reader lock.
				LockInfo info = this.AcquireReaderLock();
				try
				{
					// Return the list item.
					return this.list[index];
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
					// Set the list item.
					this.list[index] = value;
				}
				finally
				{
					// Release the writer lock.
					this.ReleaseWriterLock(info);
				}
			}
		}

		// Protected properties.

		/// <summary>
		/// Gets the internal list used for this collection.
		/// </summary>
		protected IList List { get { return this.list; } }

		// Public methods.

		/// <summary>
		/// Adds an item to the list.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns>The position to which the item was added.</returns>
		public int Add(object item)
		{
			// Acquire a writer lock.
			LockInfo info = this.AcquireWriterLock();
			try
			{
				// Add the list item.
				return this.list.Add(item);
			}
			finally
			{
				// Release the writer lock.
				this.ReleaseWriterLock(info);
			}
		}

		/// <summary>
		/// Clears the collection.
		/// </summary>
		public void Clear()
		{
			// Acquire a writer lock.
			LockInfo info = this.AcquireWriterLock();
			try
			{
				// Clear the list.
				this.list.Clear();
			}
			finally
			{
				// Release the writer lock.
				this.ReleaseWriterLock(info);
			}
		}

		/// <summary>
		/// Indicates whether the list contains the specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns><b>True</b> if the list contains the item, <b>false</b> otherwise.</returns>
		public bool Contains(object item)
		{
			// Acquire a reader lock.
			LockInfo info = this.AcquireReaderLock();
			try
			{
				return this.list.Contains(item);
			}
			finally
			{
				// Release the reader lock.
				this.ReleaseReaderLock(info);
			}
		}

		/// <summary>
		/// Returns the enumeator for the current list.
		/// </summary>
		/// <returns>The enumerator.</returns>
		public IEnumerator GetEnumerator()
		{
			// The thread must first acquire a lock before retrieving the enumerator.
			if (!this.HasLock()) throw new SynchronizationLockException();
			return this.list.GetEnumerator();
		}

		/// <summary>
		/// Copies all elements to the specified array starting from the index.
		/// </summary>
		/// <param name="array">The array.</param>
		/// <param name="index">The index.</param>
		public void CopyTo(Array array, int index)
		{
			// Acquire a reader lock.
			LockInfo info = this.AcquireReaderLock();
			try
			{
				// Copy the list to the array.
				this.list.CopyTo(array);
			}
			finally
			{
				// Release the reader lock.
				this.ReleaseReaderLock(info);
			}
		}

		/// <summary>
		/// Determines the index of a specific item in the list.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns>The index, or -1 if the item is not found.</returns>
		public int IndexOf(object item)
		{
			// Acquire a reader lock.
			LockInfo info = this.AcquireReaderLock();
			try
			{
				// Return the index.
				return this.list.IndexOf(item);
			}
			finally
			{
				// Release the reader lock.
				this.ReleaseReaderLock(info);
			}
		}

		/// <summary>
		/// Inserts the specified item in the list.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="item">The item.</param>
		public void Insert(int index, object item)
		{
			// Acquire a writer lock.
			LockInfo info = this.AcquireWriterLock();
			try
			{
				// Insert the item.
				this.list.Insert(index, item);
			}
			finally
			{
				// Release the writer lock.
				this.ReleaseWriterLock(info);
			}
		}

		/// <summary>
		/// Removes the first occurrence of the specified item from the collection.
		/// </summary>
		/// <param name="item">The item.</param>
		public void Remove(object item)
		{
			// Acquire a writer lock.
			LockInfo info = this.AcquireWriterLock();
			try
			{
				// Remove the item.
				this.list.Remove(item);
			}
			finally
			{
				// Release the writer lock.
				this.ReleaseWriterLock(info);
			}
		}

		/// <summary>
		/// Removes the item at the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		public void RemoveAt(int index)
		{
			// Acquire a writer lock.
			LockInfo info = this.AcquireWriterLock();
			try
			{
				// Remove the item.
				this.list.RemoveAt(index);
			}
			finally
			{
				// Release the writer lock.
				this.ReleaseWriterLock(info);
			}
		}
	}
}
