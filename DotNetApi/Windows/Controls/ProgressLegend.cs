/* 
 * Copyright (C) 2012-2013 Alex Bikfalvi
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
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;

namespace DotNetApi.Windows.Controls
{
	public delegate void ProgressLegendEventHandler(ProgressLegend legend);
	public delegate void ProgressLegendItemChangedEventHandler(ProgressLegend legend, ProgressLegendItem item);

	/// <summary>
	/// A class representing a progress legend.
	/// </summary>
	public class ProgressLegend : Component
	{
		public ProgressLegendItem.Collection items = new ProgressLegendItem.Collection();

		/// <summary>
		/// Creates a new component instance.
		/// </summary>
		public ProgressLegend()
		{
			// Set collection event handlers.
			this.items.BeforeCleared += this.OnBeforeItemsCleared;
			this.items.AfterItemInserted += this.OnAfterItemInserted;
			this.items.AfterItemRemoved += this.OnAfterItemRemoved;
			this.items.AfterItemSet += this.OnAfterItemSet;
		}

		// Public events.

		/// <summary>
		/// An event raised when the collection of legend items has changed.
		/// </summary>
		public event ProgressLegendEventHandler ItemsChanged;
		/// <summary>
		/// An event raised when a legend item in the collection has changed.
		/// </summary>
		public event ProgressLegendItemChangedEventHandler ItemChanged;

		// Public properties.

		/// <summary>
		/// Gets the collection of legend items.
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
		public ProgressLegendItem.Collection Items { get { return this.items; } }

		// Protected method.

		/// <summary>
		/// An event handler called before the item collection has been cleared.
		/// </summary>
		protected virtual void OnBeforeItemsCleared()
		{
			// Remove the event handlers for all items.
			foreach (ProgressLegendItem item in this.items)
			{
				if (item != null)
				{
					item.ColorChanged -= this.OnItemColorChanged;
					item.TextChanged -= this.OnItemTextChanged;
				}
			}

			// Raise the items changed event.
			if (this.ItemsChanged != null) this.ItemsChanged(this);
		}

		/// <summary>
		/// An event handler called after an item has been inserted in the collection.
		/// </summary>
		/// <param name="index">The item index.</param>
		/// <param name="item">The legend item.</param>
		protected virtual void OnAfterItemInserted(int index, ProgressLegendItem item)
		{
			// Add the item event handlers.
			if (item != null)
			{
				item.ColorChanged += this.OnItemColorChanged;
				item.TextChanged += this.OnItemTextChanged;
			}

			// Raise the items changed event.
			if (this.ItemsChanged != null) this.ItemsChanged(this);
		}

		/// <summary>
		/// An event handler called after an item has been removed from the collection.
		/// </summary>
		/// <param name="index">The item index.</param>
		/// <param name="item">The legend item.</param>
		protected virtual void OnAfterItemRemoved(int index, ProgressLegendItem item)
		{
			// Remove the item event handlers.
			if (item != null)
			{
				item.ColorChanged -= this.OnItemColorChanged;
				item.TextChanged -= this.OnItemTextChanged;
			}

			// Raise the items changed event.
			if (this.ItemsChanged != null) this.ItemsChanged(this);
		}

		/// <summary>
		/// An event handler called after an item has been set in the collection.
		/// </summary>
		/// <param name="index">The item index.</param>
		/// <param name="oldItem">The old legend item.</param>
		/// <param name="newItem">The new legend item.</param>
		protected virtual void OnAfterItemSet(int index, ProgressLegendItem oldItem, ProgressLegendItem newItem)
		{
			// Remove the event handlers from the old item.
			if (oldItem != null)
			{
				oldItem.ColorChanged -= this.OnItemColorChanged;
				oldItem.TextChanged -= this.OnItemTextChanged;
			}
			// Add the event handler for the new item.
			if (newItem != null)
			{
				newItem.ColorChanged += this.OnItemColorChanged;
				newItem.TextChanged += this.OnItemTextChanged;
			}

			// Raise the items changed event.
			if (this.ItemsChanged != null) this.ItemsChanged(this);
		}

		/// <summary>
		/// An event handler called when the color of a legend item has changed.
		/// </summary>
		/// <param name="item">The legend item.</param>
		protected virtual void OnItemColorChanged(ProgressLegendItem item)
		{
			// Raise the item changed event.
			if (this.ItemChanged != null) this.ItemChanged(this, item);
		}

		/// <summary>
		/// An event handler called when the text of a legend item has changed.
		/// </summary>
		/// <param name="item">The legend item.</param>
		protected virtual void OnItemTextChanged(ProgressLegendItem item)
		{
			// Raise the item changed event.
			if (this.ItemChanged != null) this.ItemChanged(this, item);
		}
	}
}
