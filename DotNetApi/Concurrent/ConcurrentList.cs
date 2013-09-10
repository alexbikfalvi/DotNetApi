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
using System.Linq;
using System.Threading;

namespace DotNetApi.Concurrent
{
	/// <summary>
	/// A class representing a concurrent list.
	/// </summary>
	/// <typeparam name="T">The list element type.</typeparam>
	public sealed class ConcurrentList<T> : IList<T>, ICollection<T>, IEnumerable<T>, IList, ICollection, IEnumerable
		where T : class
	{
		/// <summary>
		/// A structure representing the lock info.
		/// </summary>
		public struct LockInfo
		{
			/// <summary>
			/// Creates a new lock info with the <code>Locked</code> field set the specified value and the <code>Cookie</code>
			/// field to <b>false</b>.
			/// </summary>
			/// <param name="locked">The locked value.</param>
			public LockInfo(bool locked)
			{
				this.Locked = locked;
				this.Cookie = null;
			}

			/// <summary>
			/// Creates a new lock info with the <code>Locked</code> field set to <b>true</b> and the <code>Cookie</code>
			/// field set to the specified nullable value.
			/// </summary>
			/// <param name="cookie">The lock cookie.</param>
			public LockInfo(LockCookie? cookie)
			{
				this.Locked = true;
				this.Cookie = cookie;
			}

			/// <summary>
			/// Indicates whether the last operation acquired a lock.
			/// </summary>
			public readonly bool Locked;
			/// <summary>
			/// The lock cookie for the last operation, or <b>null</b> if the last operation has not returned a cookie.
			/// </summary>
			public readonly LockCookie? Cookie;
		}

		private readonly ReaderWriterLock sync = new ReaderWriterLock();
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

		public bool IsFixedSize { get { return false; } }

		public bool IsSynchronized { get { return true; } }

		public object SyncRoot { get { return this.syncRoot; } }

		/// <summary>
		/// Gets or sets the element at the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns>The element.</returns>
		object IList.this[int index]
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

		/// <summary>
		/// Gets or sets the element at the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns>The element.</returns>
		public T this[int index]
		{
			get { return (this as IList)[index] as T; }
			set	{ (this as IList)[index] = value; }
		}

		// Public methods.

		/// <summary>
		/// Adds an item to the list.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns>The possition to which the item was added.</returns>
		int IList.Add(object item)
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
		/// Adds an item to the list.
		/// </summary>
		/// <param name="item">The item.</param>
		public void Add(T item)
		{
			(this as IList).Add(item);
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
		/// Indicates whether the list contains the specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns><b>True</b> if the list contains the item, <b>false</b> otherwise.</returns>
		public bool Contains(T item)
		{
			return (this as IList).Contains(item);
		}

		/// <summary>
		/// Returns the enumeator for the current list.
		/// </summary>
		/// <returns>The enumerator.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			// The thread must first acquire a lock before retrieving the enumerator.
			if (!this.sync.IsReaderLockHeld && !this.sync.IsWriterLockHeld) throw new SynchronizationLockException();
			return this.list.GetEnumerator();
		}

		/// <summary>
		/// Returns the enumerator for the current list.
		/// </summary>
		/// <returns>The enumerator.</returns>
		public IEnumerator<T> GetEnumerator()
		{
			return this.Cast<T>().GetEnumerator();
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
		/// Copies all elements to the specified array starting from the index.
		/// </summary>
		/// <param name="array">The array.</param>
		/// <param name="index">The index.</param>
		public void CopyTo(T[] array, int index)
		{
			(this as ICollection).CopyTo(array, index);
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
		/// Determines the index of a specific item in the list.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns>The index, or -1 if the item is not found.</returns>
		public int IndexOf(T item)
		{
			return (list as IList).IndexOf(item);
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
		/// Inserts the specified item in the list.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="item">The item.</param>
		public void Insert(int index, T item)
		{
			(this as IList).Insert(index, item);
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
				// Insert the item.
				this.list.Remove(item);
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
		/// <returns><b>True</b> if the item was successfully removed,<b>false</b> otherwise.</returns>
		public bool Remove(T item)
		{
			// Acquire a writer lock.
			LockInfo info = this.AcquireWriterLock();
			try
			{
				// Get the index.
				int index;
				if ((index = this.list.IndexOf(item)) != -1)
				{
					this.list.RemoveAt(index);
					return true;
				}
				else return false;
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

		// Private methods.

		/// <summary>
		/// Acquires a reader lock on the list for the current thread.
		/// </summary>
		/// <returns>The lock info.</returns>
		private LockInfo AcquireReaderLock()
		{
			//  If there is a reader lock held.
			if (this.sync.IsReaderLockHeld)
			{
				// Do not acquire a new lock.
				return new LockInfo(false);
			}
			else
			{
				// Acquire a new reader lock.
				this.sync.AcquireReaderLock(-1);
				return new LockInfo(true);
			}
		}

		/// <summary>
		/// Acquires a write lock on the list for the current thread.
		/// </summary>
		/// <returns>The lock info.</returns>
		private LockInfo AcquireWriterLock()
		{
			// If there is a writer lock held.
			if (this.sync.IsWriterLockHeld)
			{
				// Do not acquire a new lock.
				return new LockInfo(false);
			}
			// Else, if there is a reader lock held.
			else if (this.sync.IsReaderLockHeld)
			{
				// Upgrade the reader lock to a writer lock.
				LockCookie cookie = this.sync.UpgradeToWriterLock(-1);
				return new LockInfo(cookie);
			}
			else
			{
				// Acquire a new writer lock/
				this.sync.AcquireWriterLock(-1);
				return new LockInfo(true);
			}
		}

		/// <summary>
		/// Releases the read lock on the list for the current thread.
		/// </summary>
		/// <param name="info">The lock info.</param>
		private void ReleaseReaderLock(LockInfo info)
		{
			// If the a new lock was acquired.
			if (info.Locked)
			{
				// Release the lock.
				this.sync.ReleaseReaderLock();
			}
		}

		/// <summary>
		/// Releases the write lock on the list for the current thread.
		/// </summary>
		/// <param name="info">The lock info.</param>
		private void ReleaseWriterLock(LockInfo info)
		{
			// If the writer lock was upgrader from a reader lock.
			if (info.Cookie.HasValue)
			{
				// Downgrade the writer lock to a reader lock.
				LockCookie cookie = info.Cookie.Value;
				this.sync.DowngradeFromWriterLock(ref cookie);
			}
			// Else, if a new lock was acquired.
			else if(info.Locked)
			{
				// Release the writer lock.
				this.sync.ReleaseWriterLock();
			}
		}
	}
}
