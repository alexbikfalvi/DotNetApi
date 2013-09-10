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
using DotNetApi.Concurrent;

namespace DotNetApi.Concurrent.Generic
{
	/// <summary>
	/// A class representing a concurrent list.
	/// </summary>
	/// <typeparam name="T">The list element type.</typeparam>
	public class ConcurrentList<T> : ConcurrentList, IList<T>, ICollection<T>, IEnumerable<T>
	{
		/// <summary>
		/// The generic enumerator class for this collection.
		/// </summary>
		public sealed class Enumerator : IEnumerator<T>
		{
			private readonly IEnumerator enumerator;

			/// <summary>
			/// Creates a new enumerator instance from a non-generic enumerator.
			/// </summary>
			/// <param name="enumerator">The non-generic enumerator.</param>
			public Enumerator(IEnumerator enumerator)
			{
				this.enumerator = enumerator;
			}

			// Public properties.
			
			/// <summary>
			/// Gets the element in the collection at the current position of the enumerator.
			/// </summary>
			public T Current { get { return (T)this.enumerator.Current; } }
			/// <summary>
			/// Gets the element in the collection at the current position of the enumerator.
			/// </summary>
			object IEnumerator.Current { get { return this.Current; } }

			// Public methods.

			/// <summary>
			/// Disposes the current object.
			/// </summary>
			public void Dispose()
			{
				// Suppress the finalizer.
				GC.SuppressFinalize(this);
			}

			/// <summary>
			/// Advances the enumerator to the next element of the collection.
			/// </summary>
			/// <returns><b>True</b> if the enumerator was successfully advanced to the next element; <b>false</b> if the enumerator has passed the end of the collection.</returns>
			public bool MoveNext()
			{
				return this.enumerator.MoveNext();
			}

			/// <summary>
			/// Sets the enumerator to its initial position, which is before the first element in the collection.
			/// </summary>
			public void Reset()
			{
				this.enumerator.Reset();
			}
		}

		/// <summary>
		/// Creates an empty concurrent list instance with the default capacity.
		/// </summary>
		public ConcurrentList()
		{
		}

		/// <summary>
		/// Creates an empty concurrent list with the specified capacity.
		/// </summary>
		/// <param name="capacity">The capacity.</param>
		public ConcurrentList(int capacity)
			: base(capacity)
		{
		}

		// Public properties.

		/// <summary>
		/// Gets or sets the element at the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns>The element.</returns>
		public new T this[int index]
		{
			get { return (T)base[index]; }
			set { base[index] = value; }
		}

		// Public methods.

		/// <summary>
		/// Adds an item to the list.
		/// </summary>
		/// <param name="item">The item.</param>
		public void Add(T item)
		{
			base.Add(item);
		}

		/// <summary>
		/// Indicates whether the list contains the specified item.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns><b>True</b> if the list contains the item, <b>false</b> otherwise.</returns>
		public bool Contains(T item)
		{
			return base.Contains(item);
		}

		/// <summary>
		/// Returns the enumerator for the current list.
		/// </summary>
		/// <returns>The enumerator.</returns>
		public new IEnumerator<T> GetEnumerator()
		{
			return new Enumerator(base.GetEnumerator());
		}

		/// <summary>
		/// Copies all elements to the specified array starting from the index.
		/// </summary>
		/// <param name="array">The array.</param>
		/// <param name="index">The index.</param>
		public void CopyTo(T[] array, int index)
		{
			base.CopyTo(array, index);
		}

		/// <summary>
		/// Determines the index of a specific item in the list.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns>The index, or -1 if the item is not found.</returns>
		public int IndexOf(T item)
		{
			return base.IndexOf(item);
		}

		/// <summary>
		/// Inserts the specified item in the list.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="item">The item.</param>
		public void Insert(int index, T item)
		{
			base.Insert(index, item);
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
				if ((index = this.List.IndexOf(item)) != -1)
				{
					this.List.RemoveAt(index);
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
	}
}
