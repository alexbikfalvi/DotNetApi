/* 
 * Copyright (C) 2012 Alex Bikfalvi
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
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DotNetApi.Windows.Controls
{
	/// <summary>
	/// Creates a tool strip drop down checked list.
	/// </summary>
	public sealed class ToolStripDropDownCheckedList : ToolStripDropDown
	{
		private CheckedListBox listBox = new CheckedListBox();
		private int minimumVisibleItems = 10;
		private int totalItemsHeight = 0;
		private readonly List<CheckedListItem> items = new List<CheckedListItem>();

		/// <summary>
		/// Creates a new class instance.
		/// </summary>
		public ToolStripDropDownCheckedList()
		{
			// Set the list box defaults.
			this.listBox.BorderStyle = BorderStyle.FixedSingle;
			this.listBox.CheckOnClick = true;
			this.listBox.MinimumSize = new Size(200, 200);
			this.listBox.ThreeDCheckBoxes = true;
			
			// Add an event handler to the list box.
			this.listBox.ItemCheck += new ItemCheckEventHandler(this.OnItemCheck);

			// Add the list box to the drop down.
			base.Items.Add(new ToolStripControlHost(this.listBox));

			// Set the padding.
			this.Padding = new Padding(4, 2, 4, 0);
		}

		#region Public properties

		/// <summary>
		/// Gets or sets the list box size.
		/// </summary>
		public Size ListSize
		{
			get { return this.listBox.Size; }
			set { this.listBox.Size = value; }
		}
		/// <summary>
		/// Gets or sets the list box minimum size.
		/// </summary>
		public Size ListMinimumSize
		{
			get { return this.listBox.MinimumSize; }
			set { this.listBox.MinimumSize = value; }
		}
		/// <summary>
		/// Returns the value check-state pair at a given index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns>The value check-state pair.</returns>
		public CheckedListItem this[int index]
		{
			get { return this.items[index]; }
		}
		/// <summary>
		/// Returns the items collection.
		/// </summary>
		public new IEnumerable<CheckedListItem> Items
		{
			get { return this.items; }
		}
		/// <summary>
		/// Returns the checked items collection.
		/// </summary>
		public IEnumerable<CheckedListItem> CheckedItems
		{
			get { return this.items.Where(item => item.State == CheckState.Checked); }
		}

		#endregion

		#region Public events

		/// <summary>
		/// An event raised when the checked state of an item has changed.
		/// </summary>
		public event ItemCheckEventHandler ItemCheck;

		#endregion

		#region Public methods

		/// <summary>
		/// Adds a new item to the checked list box.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <param name="name">The item name.</param>
		/// <param name="state">Indicates whether the check state of the item.</param>
		public void AddItem(object item, string name, CheckState state)
		{
			// Add a new item pair.
			this.items.Add(new CheckedListItem(item, name, state));
			// Add the item to the list box.
			this.listBox.Items.Add(name, state);
			// Update the total item height.
			this.totalItemsHeight += this.listBox.GetItemHeight(this.listBox.Items.Count - 1);
			// If the number of items is less than the minimum number of visible items, increase the minimum height.
			if (this.listBox.Items.Count < this.minimumVisibleItems)
			{
				this.listBox.MinimumSize = new Size(this.listBox.MinimumSize.Width, this.totalItemsHeight);
				this.listBox.Size = this.listBox.MinimumSize;
			}
			else if (this.listBox.Items.Count == this.minimumVisibleItems)
			{
				this.listBox.MaximumSize = new Size(this.listBox.MinimumSize.Width, this.totalItemsHeight);
			}
		}

		/// <summary>
		/// Checks all items.
		/// </summary>
		public void CheckAll()
		{
			foreach (CheckedListItem item in this.items)
			{
				item.State = CheckState.Checked;
			}
			for (int index = 0; index < this.listBox.Items.Count; index++)
			{
				this.listBox.SetItemCheckState(index, CheckState.Checked);
			}
			this.Refresh();
		}

		/// <summary>
		/// Unchecks all items.
		/// </summary>
		public void UncheckAll()
		{
			foreach (CheckedListItem item in this.items)
			{
				item.State = CheckState.Unchecked;
			}
			for (int index = 0; index < this.listBox.Items.Count; index++)
			{
				this.listBox.SetItemCheckState(index, CheckState.Unchecked);
			}
			this.Refresh();
		}

		#endregion

		#region Private methods

		/// <summary>
		/// An event handler called when the checked state of an item has changed.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void OnItemCheck(object sender, ItemCheckEventArgs e)
		{
			// Update the items pair at the specified index.
			CheckedListItem pair = this.items[e.Index];
			pair.State = e.NewValue;
			this.items[e.Index] = pair;
			// Raise an item check event.
			if (null != this.ItemCheck) this.ItemCheck(sender, e);
		}

		#endregion
	}

	/// <summary>
	/// A structure representing a value check-state pair.
	/// </summary>
	public sealed class CheckedListItem
	{
		private object item;
		private string name;
		private CheckState state;

		/// <summary>
		/// Creates a new pair.
		/// </summary>
		/// <param name="item">The item.</param>
		/// <param name="name">The item name.</param>
		/// <param name="state">The state.</param>
		public CheckedListItem(object item, string name, CheckState state)
		{
			this.item = item;
			this.name = name;
			this.state = state;
		}

		/// <summary>
		/// Gets the item.
		/// </summary>
		public object Item { get { return this.item; } }

		/// <summary>
		/// Gets the item name.
		/// </summary>
		public string Name { get { return this.name; } }

		/// <summary>
		/// Gets or sets the item checked state.
		/// </summary>
		public CheckState State
		{
			get { return this.state; }
			set { this.state = value; }
		}
	}
}
