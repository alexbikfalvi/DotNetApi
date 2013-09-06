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
	/// <summary>
	/// A class representing a progress legend.
	/// </summary>
	public sealed class ProgressLegend : Component
	{
		public ComponentCollection<ProgressLegendItem> items = new ComponentCollection<ProgressLegendItem>();

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
		public ComponentCollection<ProgressLegendItem> Items { get { return this.items; } }

		// Private method.

		/// <summary>
		/// An event handler called before the item collection has been cleared.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void OnBeforeItemsCleared(object sender, EventArgs e)
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
			if (this.ItemsChanged != null) this.ItemsChanged(this, new ProgressLegendEventArgs(this));
		}

		/// <summary>
		/// An event handler called after an item has been inserted in the collection.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void OnAfterItemInserted(object sender, ComponentCollection<ProgressLegendItem>.ItemChangedEventArgs e)
		{
			// Add the item event handlers.
			if (e.Item != null)
			{
				e.Item.ColorChanged += this.OnItemColorChanged;
				e.Item.TextChanged += this.OnItemTextChanged;
			}

			// Raise the items changed event.
			if (this.ItemsChanged != null) this.ItemsChanged(this, new ProgressLegendEventArgs(this));
		}

		/// <summary>
		/// An event handler called after an item has been removed from the collection.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void OnAfterItemRemoved(object sender, ComponentCollection<ProgressLegendItem>.ItemChangedEventArgs e)
		{
			// Remove the item event handlers.
			if (e.Item != null)
			{
				e.Item.ColorChanged -= this.OnItemColorChanged;
				e.Item.TextChanged -= this.OnItemTextChanged;
			}

			// Raise the items changed event.
			if (this.ItemsChanged != null) this.ItemsChanged(this, new ProgressLegendEventArgs(this));
		}

		/// <summary>
		/// An event handler called after an item has been set in the collection.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void OnAfterItemSet(object sender, ComponentCollection<ProgressLegendItem>.ItemSetEventArgs e)
		{
			// Remove the event handlers from the old item.
			if (e.OldItem != null)
			{
				e.OldItem.ColorChanged -= this.OnItemColorChanged;
				e.OldItem.TextChanged -= this.OnItemTextChanged;
			}
			// Add the event handler for the new item.
			if (e.NewItem != null)
			{
				e.NewItem.ColorChanged += this.OnItemColorChanged;
				e.NewItem.TextChanged += this.OnItemTextChanged;
			}

			// Raise the items changed event.
			if (this.ItemsChanged != null) this.ItemsChanged(this, new ProgressLegendEventArgs(this));
		}

		/// <summary>
		/// An event handler called when the color of a legend item has changed.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void OnItemColorChanged(object sender, ProgressLegendItemEventArgs e)
		{
			// Raise the item changed event.
			if (this.ItemChanged != null) this.ItemChanged(this, new ProgressLegendItemChangedEventArgs(this, e.Item));
		}

		/// <summary>
		/// An event handler called when the text of a legend item has changed.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void OnItemTextChanged(object sender, ProgressLegendItemEventArgs e)
		{
			// Raise the item changed event.
			if (this.ItemChanged != null) this.ItemChanged(this, new ProgressLegendItemChangedEventArgs(this, e.Item));
		}
	}
}
