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

namespace DotNetApi.Windows.Controls
{
	/// <summary>
	/// A collection of progress navigator item items.
	/// </summary>
	public class MapMarkerCollection : CollectionBase
	{
		// Public delegates.
		public delegate void ClearedEventHandler();
		public delegate void ChangedEventHandler(int index, MapMarker item);
		public delegate void SetEventHandler(int index, MapMarker oldItem, MapMarker newItem);

		// Public properties.

		/// <summary>
		/// Gets or sets the item at the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns>The item.</returns>
		public MapMarker this[int index]
		{
			get { lock (this) { return this.List[index] as MapMarker; } }
			set { lock (this) { this.List[index] = value; } }
		}

		// Public events.

		/// <summary>
		/// An event raised before the collection is cleared.
		/// </summary>
		public event ClearedEventHandler BeforeCleared;
		/// <summary>
		/// An event raised after the collection is cleared.
		/// </summary>
		public event ClearedEventHandler AfterCleared;
		/// <summary>
		/// An event raised before an item is inserted into the collection.
		/// </summary>
		public event ChangedEventHandler BeforeItemInserted;
		/// <summary>
		/// An event raised after an item is inserted into the collection.
		/// </summary>
		public event ChangedEventHandler AfterItemInserted;
		/// <summary>
		/// An event raised before an item is removed from the collection.
		/// </summary>
		public event ChangedEventHandler BeforeItemRemoved;
		/// <summary>
		/// An event raised after an item is removed from the collection.
		/// </summary>
		public event ChangedEventHandler AfterItemRemoved;
		/// <summary>
		/// An event raised before an item is set in the collection.
		/// </summary>
		public event SetEventHandler BeforeItemSet;
		/// <summary>
		/// An event raised after an item is set in the collection.
		/// </summary>
		public event SetEventHandler AfterItemSet;

		// Public methods.

		/// <summary>
		/// Adds an item to the collection.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns>The position into which the new item was inserted,
		/// or -1 to indicate that the item was not inserted into the collection.</returns>
		public int Add(MapMarker item)
		{
			lock (this)
			{
				// Add the item.
				int result = this.List.Add(item);
				// Return the result.
				return result;
			}
		}

		/// <summary>
		/// Adds a range of items to the collection.
		/// </summary>
		/// <param name="items">The range of items.</param>
		public void AddRange(MapMarker[] items)
		{
			lock (this)
			{
				// Add the items.
				foreach (MapMarker item in items)
				{
					this.Add(item);
				}
			}
		}

		/// <summary>
		/// Determines the index of the specific item in the collection.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns>The index of value if found in the list; otherwise, -1.</returns>
		public int IndexOf(MapMarker item)
		{
			lock (this) { return this.List.IndexOf(item); }
		}

		/// <summary>
		/// Inserts the item in the collection at the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="item">The item</param>
		public void Insert(int index, MapMarker item)
		{
			// Insert the item.
			lock (this) { this.List.Insert(index, item); }
		}

		/// <summary>
		/// Removes the item from the collection.
		/// </summary>
		/// <param name="item">The item.</param>
		public void Remove(MapMarker item)
		{
			// Remove the item.
			lock (this) { this.List.Remove(item); }
		}

		/// <summary>
		/// Verifies if the specified item is found in the collection.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <returns><b>True</b> if the element is found in the collection, or <b>false</b> otherwise.</returns>
		public bool Contains(MapMarker item)
		{
			lock (this) { return this.List.Contains(item); }
		}

		// Protected methods.

		/// <summary>
		/// Validates the specified value as an item for this collection.
		/// </summary>
		/// <param name="value">The value to validate.</param>
		protected override void OnValidate(Object value)
		{
			if (value.GetType().BaseType != typeof(MapMarker))
				throw new ArgumentException("Value must be a map marker.", "value");
		}


		/// <summary>
		/// An event handler called before clearing the item collection.
		/// </summary>
		protected override void OnClear()
		{
			// Call the base class method.
			base.OnClear();
			// Raise the event.
			if (this.BeforeCleared != null) this.BeforeCleared();
		}

		/// <summary>
		/// An event handler called after clearing the item collection.
		/// </summary>
		protected override void OnClearComplete()
		{
			// Call the base class method.
			base.OnClearComplete();
			// Raise the event.
			if (this.AfterCleared != null) this.AfterCleared();
		}

		/// <summary>
		/// An event handler called before inserting an item into the collection.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="value">The item.</param>
		protected override void OnInsert(int index, object value)
		{
			// Call the base class method.
			base.OnInsert(index, value);
			// Raise the event.
			if (this.BeforeItemInserted != null) this.BeforeItemInserted(index, value as MapMarker);
		}

		/// <summary>
		/// An event handler called after inserting an item into the collection.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="value">The item.</param>
		protected override void OnInsertComplete(int index, object value)
		{
			// Call the base class method.
			base.OnInsertComplete(index, value);
			// Raise the event.
			if (this.AfterItemInserted != null) this.AfterItemInserted(index, value as MapMarker);
		}

		/// <summary>
		/// An event handler called before removing an item from the collection.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="value">The item.</param>
		protected override void OnRemove(int index, object value)
		{
			// Call the base class method.
			base.OnRemove(index, value);
			// Raise the event.
			if (this.BeforeItemRemoved != null) this.BeforeItemRemoved(index, value as MapMarker);
		}

		/// <summary>
		/// An event handler called after removing an item from the collection.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="value">The item.</param>
		protected override void OnRemoveComplete(int index, object value)
		{
			// Call the base class method.
			base.OnRemoveComplete(index, value);
			// Raise the event.
			if (this.AfterItemRemoved != null) this.AfterItemRemoved(index, value as MapMarker);
		}

		/// <summary>
		/// An event handler called before setting the value of an item from the collection.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="oldValue">The old item.</param>
		/// <param name="newValue">The new item.</param>
		protected override void OnSet(int index, object oldValue, object newValue)
		{
			// Call the base class method.
			base.OnSet(index, oldValue, newValue);
			// Raise the event.
			if (this.BeforeItemSet != null) this.BeforeItemSet(index, oldValue as MapMarker, newValue as MapMarker);
		}

		/// <summary>
		/// An event handler called after setting the value of an item from the collection.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="oldValue">The old item.</param>
		/// <param name="newValue">The new item.</param>
		protected override void OnSetComplete(int index, object oldValue, object newValue)
		{
			// Call the base class method.
			base.OnSetComplete(index, oldValue, newValue);
			// Raise the event.
			if (this.AfterItemSet != null) this.AfterItemSet(index, oldValue as MapMarker, newValue as MapMarker);
		}
	}
}
