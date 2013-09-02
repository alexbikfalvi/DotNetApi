/* 
 * Copyright (C) 2010-2012 Alex Bikfalvi
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
using System.Windows.Forms;

namespace DotNetApi.Windows.Controls
{
	public delegate void SideMenuItemEventHandler(SideMenuItem menuItem);
	public delegate void SideMenuItemControlChangedEventHandler(SideMenuItem item, ISideControl oldControl, ISideControl newControl);

	/// <summary>
	/// A class representing side menu item.
	/// </summary>
	[DesignTimeVisible(false)]
	public sealed class SideMenuItem : MenuItem
	{
		/// <summary>
		/// A collection of progress navigator item items.
		/// </summary>
		public class Collection : CollectionBase
		{
			// Public delegates.
			public delegate void ClearedEventHandler();
			public delegate void ChangedEventHandler(int index, SideMenuItem item);
			public delegate void SetEventHandler(int index, SideMenuItem oldItem, SideMenuItem newItem);

			// Public properties.

			/// <summary>
			/// Gets or sets the item at the specified index.
			/// </summary>
			/// <param name="index">The index.</param>
			/// <returns>The item.</returns>
			public SideMenuItem this[int index]
			{
				get { return this.List[index] as SideMenuItem; }
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
			public int Add(SideMenuItem item)
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
			public void AddRange(SideMenuItem[] items)
			{
				// Add the items.
				foreach (SideMenuItem item in items)
				{
					this.Add(item);
				}
			}

			/// <summary>
			/// Determines the index of the specific item in the collection.
			/// </summary>
			/// <param name="item">The item.</param>
			/// <returns>The index of value if found in the list; otherwise, -1.</returns>
			public int IndexOf(SideMenuItem item)
			{
				return this.List.IndexOf(item);
			}

			/// <summary>
			/// Inserts the item in the collection at the specified index.
			/// </summary>
			/// <param name="index">The index.</param>
			/// <param name="item">The item</param>
			public void Insert(int index, SideMenuItem item)
			{
				// Insert the item.
				this.List.Insert(index, item);
			}

			/// <summary>
			/// Removes the item from the collection.
			/// </summary>
			/// <param name="item">The item.</param>
			public void Remove(SideMenuItem item)
			{
				// Remove the item.
				this.List.Remove(item);
			}

			/// <summary>
			/// Verifies if the specified item is found in the collection.
			/// </summary>
			/// <param name="item">The item.</param>
			/// <returns><b>True</b> if the element is found in the collection, or <b>false</b> otherwise.</returns>
			public bool Contains(SideMenuItem item)
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
				if (!(value is SideMenuItem))
					throw new ArgumentException("Value must be a progress navigator item.", "value");
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
				if (this.BeforeItemInserted != null) this.BeforeItemInserted(index, value as SideMenuItem);
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
				if (this.AfterItemInserted != null) this.AfterItemInserted(index, value as SideMenuItem);
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
				if (this.BeforeItemRemoved != null) this.BeforeItemRemoved(index, value as SideMenuItem);
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
				if (this.AfterItemRemoved != null) this.AfterItemRemoved(index, value as SideMenuItem);
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
				if (this.BeforeItemSet != null) this.BeforeItemSet(index, oldValue as SideMenuItem, newValue as SideMenuItem);
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
				if (this.AfterItemSet != null) this.AfterItemSet(index, oldValue as SideMenuItem, newValue as SideMenuItem);
			}
		}

		// Private members.

		private Image imageSmall = null;
		private Image imageLarge = null;
		private ToolStripMenuItem hiddenMenuItem = new ToolStripMenuItem();
		private ToolTip toolTip = new ToolTip();
		private ISideControl control = null;

		/// <summary>
		/// Creates a new side menu item instance.
		/// </summary>
		public SideMenuItem()
		{
			// Hidden menu item
			this.hiddenMenuItem.Tag = this;

			// Set the properties.
			this.Visible = true;
			this.Enabled = true;
		}

		/// <summary>
		/// Creates a new side menu item instance.
		/// </summary>
		/// <param name="text">The menu item text.</param>
		/// <param name="imageSmall">The item small image.</param>
		/// <param name="imageLarge">The item large image.</param>
		/// <param name="hiddenItemClick">The event handler.</param>
		public SideMenuItem(
			string text,
			Image imageSmall,
			Image imageLarge
			)
		{
			// Hidden menu item event handler.
			this.hiddenMenuItem.Text = text;
			this.hiddenMenuItem.Image = imageSmall;
			this.hiddenMenuItem.Click += this.OnHiddenClick;
			
			// Set the properties.
			this.Visible = true;
			this.Enabled = true;
			this.ImageSmall = imageSmall;
			this.ImageLarge = imageLarge;
			base.Text = text;
		}

		// Public events.

		/// <summary>
		/// An event raised when the menu item is clicked.
		/// </summary>
		public new event SideMenuItemEventHandler Click;
		/// <summary>
		/// An event raised when the menu item text has changed.
		/// </summary>
		public event SideMenuItemEventHandler TextChanged;
		/// <summary>
		/// An event raised when the menu item small image has changed.
		/// </summary>
		public event SideMenuItemEventHandler SmallImageChanged;
		/// <summary>
		/// An event raised when the menu item large image has changed.
		/// </summary>
		public event SideMenuItemEventHandler LargeImageChanged;
		/// <summary>
		/// An event raised when the menu item control has changed.
		/// </summary>
		public event SideMenuItemControlChangedEventHandler ControlChanged;
		/// <summary>
		/// An event raised when the user clicks on the hidden item.
		/// </summary>
		internal event SideMenuItemEventHandler HiddenClick;

		// Public properties.

		/// <summary>
		/// Gets or sets the item text.
		/// </summary>
		public new string Text
		{
			get { return base.Text; }
			set
			{
				// Call the event handler.
				this.OnTextSet(value);
			}
		}
		/// <summary>
		/// Gets or sets the small menu item image.
		/// </summary>
		public Image ImageSmall
		{
			get { return this.imageSmall; }
			set
			{
				// Call the event handler.
				this.OnSmallImageSet(value);
			}
		}
		/// <summary>
		/// Gets or sets the large menu item image.
		/// </summary>
		public Image ImageLarge
		{
			get { return this.imageLarge; }
			set
			{
				// Call the event handler.
				this.OnLargeImageSet(value);
			}
		}
		/// <summary>
		/// Gets or sets the control currently associated with the current side menu item.
		/// </summary>
		public ISideControl Control
		{
			get { return this.control; }
			set
			{
				// Call the event handler.
				this.OnControlSet(value);
			}
		}
		/// <summary>
		/// Gets the tooltip corresponding to this side menu item.
		/// </summary>
		public ToolTip ToolTip { get { return this.toolTip; } }

		// Internal properties.

		/// <summary>
		/// Gets the hidden side menu item.
		/// </summary>
		internal ToolStripMenuItem HiddenMenuItem { get { return this.hiddenMenuItem; } }

		// Public methods.

		/// <summary>
		/// Selects the current menu item.
		/// </summary>
		public new void Select()
		{
			// Check the hidden menu item. 
			this.hiddenMenuItem.Checked = true;
			// Show the item control, if any.
			if (null != this.control) this.control.ShowSideControl();
			// Call the handler for the side menu.
			if (null != this.Click) this.Click(this);
		}
		/// <summary>
		/// Deselects the current menu item.
		/// </summary>
		public void Deselect()
		{
			// Uncheck the hidden menu item.
			this.hiddenMenuItem.Checked = false;
			// Hide the item control, if any.
			if (null != this.control) this.control.HideSideControl();
		}

		// Protected methods.

		/// <summary>
		/// An event handler called when the object is being disposed.
		/// </summary>
		/// <param name="disposed">If <b>true</b>, clean both managed and native resources. If <b>false</b>, clean only native resources.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.hiddenMenuItem.Dispose();
				this.toolTip.Dispose();
			}
			// Call the base class method.
			base.Dispose(disposing);
		}

		// Private methods.

		/// <summary>
		/// An event handler called when the text is set.
		/// </summary>
		/// <param name="text">The menu item text.</param>
		private void OnTextSet(string text)
		{
			// Set the text for this menu item.
			base.Text = text;
			// Set the text for the hidden tool strip item.
			this.hiddenMenuItem.Text = text;
			// Call the event handler.
			if (this.TextChanged != null) this.TextChanged(this);
		}

		/// <summary>
		/// An event handler called when the small image is set.
		/// </summary>
		/// <param name="image">The small image.</param>
		private void OnSmallImageSet(Image image)
		{
			// Set the image for the menu item.
			this.imageSmall = image;
			// Set the image for the hidden menu item.
			this.hiddenMenuItem.Image = image;
			// Call the event handler.
			if (this.SmallImageChanged != null) this.SmallImageChanged(this);
		}

		/// <summary>
		/// An event handler called when the large image is set.
		/// </summary>
		/// <param name="image">The large image.</param>
		private void OnLargeImageSet(Image image)
		{
			// Set the image for this menu item.
			this.imageLarge = image;
			// Call the event handler.
			if (this.LargeImageChanged != null) this.LargeImageChanged(this);
		}

		/// <summary>
		/// An event handler called when the control is set.
		/// </summary>
		/// <param name="control">The control.</param>
		private void OnControlSet(ISideControl control)
		{
			// Save the old control for this item.
			ISideControl old = this.control;
			// Set the new control for this item.
			this.control = control;
			// Call the event handler.
			if (this.ControlChanged != null) this.ControlChanged(this, old, control);
		}

		/// <summary>
		/// An event handler called when the user clicks on the hidden item.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void OnHiddenClick(object sender, EventArgs e)
		{
			// Call the hidden click event handler to update the item selection..
			if (this.HiddenClick != null) this.HiddenClick(this);
		}
	}
}
