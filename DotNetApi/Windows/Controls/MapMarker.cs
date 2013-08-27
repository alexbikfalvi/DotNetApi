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
using System.ComponentModel;
using System.Drawing;

namespace DotNetApi.Windows.Controls
{
	public delegate void MarkerChangedEventHandler(MapMarker marker);
	public delegate void MarkerCoordinatesChangedEventHandler(MapMarker marker, PointF oldCoordinates, PointF newCoordinates);
	public delegate void MarkerSizeChangedEventHandler(MapMarker marker, Size oldSize, Size newSize);
	public delegate void MarkerEmphasisChangedEventHandler(MapMarker marker, bool oldEmphasis, bool newEmphasis);

	/// <summary>
	/// A class representing a geo marker.
	/// </summary>
	public abstract class MapMarker : Component
	{
		/// <summary>
		/// A collection of progress navigator item items.
		/// </summary>
		public class Collection : CollectionBase
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
				get { return this.List[index] as MapMarker; }
				set { this.List[index] = value; }
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
				// Add the item.
				int result = this.List.Add(item);
				// Return the result.
				return result;
			}

			/// <summary>
			/// Adds a range of items to the collection.
			/// </summary>
			/// <param name="items">The range of items.</param>
			public void AddRange(MapMarker[] items)
			{
				// Add the items.
				foreach (MapMarker item in items)
				{
					this.Add(item);
				}
			}

			/// <summary>
			/// Determines the index of the specific item in the collection.
			/// </summary>
			/// <param name="item">The item.</param>
			/// <returns>The index of value if found in the list; otherwise, -1.</returns>
			public int IndexOf(MapMarker item)
			{
				return this.List.IndexOf(item);
			}

			/// <summary>
			/// Inserts the item in the collection at the specified index.
			/// </summary>
			/// <param name="index">The index.</param>
			/// <param name="item">The item</param>
			public void Insert(int index, MapMarker item)
			{
				// Insert the item.
				this.List.Insert(index, item);
			}

			/// <summary>
			/// Removes the item from the collection.
			/// </summary>
			/// <param name="item">The item.</param>
			public void Remove(MapMarker item)
			{
				// Remove the item.
				this.List.Remove(item);
			}

			/// <summary>
			/// Verifies if the specified item is found in the collection.
			/// </summary>
			/// <param name="item">The item.</param>
			/// <returns><b>True</b> if the element is found in the collection, or <b>false</b> otherwise.</returns>
			public bool Contains(MapMarker item)
			{
				return this.List.Contains(item);
			}

			// Protected methods.

			/// <summary>
			/// Validates the specified value as an item for this collection.
			/// </summary>
			/// <param name="value">The value to validate.</param>
			protected override void OnValidate(Object value)
			{
				if (value.GetType().BaseType != typeof(MapMarker))
					throw new ArgumentException("Value must be a geo marker.", "value");
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

		private PointF coordinates;
		private Size size = new Size(8, 8);
		private Color colorLine = Color.FromArgb(255, 153, 0);
		private Color colorFill = Color.FromArgb(248, 224, 124);
		private bool emphasis = false;

		/// <summary>
		/// Creates a new geo marker instance.
		/// </summary>
		/// <param name="coordinates">The marker coordinates as longitude and latitude in degrees.</param>
		public MapMarker(PointF coordinates)
		{
			this.coordinates = coordinates;
		}

		// Public events.

		/// <summary>
		/// An event raised when the marker has been changed.
		/// </summary>
		public event MarkerChangedEventHandler Changed;
		/// <summary>
		/// An event raised when the marker emphasis has changed.
		/// </summary>
		public event MarkerEmphasisChangedEventHandler EmphasisChanged;
		/// <summary>
		/// An event raised when the marker coordinates have changed.
		/// </summary>
		public event MarkerCoordinatesChangedEventHandler CoordinatesChanged;
		/// <summary>
		/// An event raised when the marker size has changed.
		/// </summary>
		public event MarkerSizeChangedEventHandler SizeChanged;

		// Public properties.

		/// <summary>
		/// Gets or sets the marker coordinates.
		/// </summary>
		public PointF Coordinates
		{
			get { return this.coordinates; }
			set
			{
				// Save the old coordinates.
				PointF old = this.coordinates;
				// Set the new coordinates.
				this.coordinates = value;
				// Call the event handler.
				this.OnCoordinatesChanged(old, value);
			}
		}

		/// <summary>
		/// Gets or sets the marker size.
		/// </summary>
		public Size Size
		{
			get { return this.size; }
			set
			{
				// Save the old size.
				Size old = this.size;
				// Set the new size.
				this.size = value;
				// Call the event handler.
				this.OnSizeChanged(old, value);
			}
		}

		/// <summary>
		/// Gets or sets the line color.
		/// </summary>
		public Color ColorLine
		{
			get { return this.colorLine; }
			set
			{
				// Set the new color.
				this.colorLine = value;
				// Call the event handler.
				this.OnChanged();
			}
		}

		/// <summary>
		/// Gets or sets the fill color.
		/// </summary>
		public Color ColorFill
		{
			get { return this.colorFill; }
			set
			{
				// Set the new color.
				this.colorFill = value;
				// Call the event handler.
				this.OnChanged();
			}
		}

		/// <summary>
		/// Gets or sets whether this marker is emphasized.
		/// </summary>
		public bool Emphasis
		{
			get { return this.emphasis; }
			set
			{
				// Save the old emphasis.
				bool old = this.emphasis;
				// Set the new emphasis.
				this.emphasis = value;
				// Call the event handler.
				this.OnEmphasisChanged(old, value);
			}
		}

		// Internal methods.

		/// <summary>
		/// Paints the current marker.
		/// </summary>
		/// <param name="g">The graphics object.</param>
		/// <param name="rectangle">The rectangle.</param>
		internal abstract void Paint(Graphics g, Rectangle rectangle);

		// Protected methods.

		/// <summary>
		/// An event handler called when the marker has changed.
		/// </summary>
		protected virtual void OnChanged()
		{
			// Raise the event.
			if (this.Changed != null) this.Changed(this);
		}

		/// <summary>
		/// An event handler called when the marker coordinates have changed.
		/// </summary>
		/// <param name="oldCoordinates">The old coordinates.</param>
		/// <param name="newCoordinates">The new coordinates.</param>
		protected virtual void OnCoordinatesChanged(PointF oldCoordinates, PointF newCoordinates)
		{
			// If the coordinates have not changed, do nothing.
			if (oldCoordinates == newCoordinates) return;
			// Raise the event.
			if (this.CoordinatesChanged != null) this.CoordinatesChanged(this, oldCoordinates, newCoordinates);
		}

		/// <summary>
		/// An event handler called when the marker size has changed.
		/// </summary>
		/// <param name="oldSize">The old size.</param>
		/// <param name="newSize">The new size.</param>
		protected virtual void OnSizeChanged(Size oldSize, Size newSize)
		{
			// If the size has not changed, do nothing.
			if (oldSize == newSize) return;
			// Raise the event.
			if (this.SizeChanged != null) this.SizeChanged(this, oldSize, newSize);
		}

		/// <summary>
		/// An event handler called when the marker emphasis has changed.
		/// </summary>
		/// <param name="oldEmphasis">The old emphasis.</param>
		/// <param name="newEmphasis">The new emphasis.</param>
		protected virtual void OnEmphasisChanged(bool oldEmphasis, bool newEmphasis)
		{
			// If the emphasis has not changed, do nothing.
			if (oldEmphasis == newEmphasis) return;
			// Raise the event.
			if (this.EmphasisChanged != null) this.EmphasisChanged(this, oldEmphasis, newEmphasis);
		}
	}
}
