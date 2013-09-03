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
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace DotNetApi.Windows.Controls
{
	public delegate void SideMenuEventHandler(SideMenu sideMenu);

	/// <summary>
	/// A class representing a side menu.
	/// </summary>
	public sealed class SideMenu : ContainerControl
	{
		// Private members
		private SideMenuItem.Collection items = new SideMenuItem.Collection();

		private int dockButtonWidth = 18;				// The width of the dock button.
		private int gripHeight = 8;						// The grip height.
		private int itemHeight = 48;					// The menu item height.
		private int titleHeight = 27;					// The height of the title.
		private int minimumPanelHeight = 50;			// The minimum panel height.
		private int minimizedItemWidth = 25;			// The width of a minimized item.
		private float titleFontSize = 12.0f;			// The title font size.
		private int toolTipTopOffset = -20;				// The top offset of a minimized item tooltip.

		private int maximumVisibleItems = 0;			// The maximum number of visible items.
		private int maximumMinimizedItems = 0;			// The maximum number of minimized items.

		private int hiddenItems = 0;					// The number of hidden items. 
		private int minimizedItems = 0;					// The number of minimized items.
		private int visibleItems = 0;					// The number of visible items.

		private int? highlightedVisibleIndex = null;	// The index of the highlighted visible item.
		private int? highlightedMinimizedIndex = null;	// The index of the highlighted minimized item.
		private int? selectedIndex = null;				// The index of the selected menu item.

		private bool itemPressed;
		private bool itemMinimizedPressed;
		private bool resizeGrip;
		private bool dockButtonSelected;
		private bool dockButtonPressed;

		private ContextMenuStrip controlMenu = new ContextMenuStrip();
		private ToolStripMenuItem toolStripMenuItemShowMoreButtons = new ToolStripMenuItem();
		private ToolStripMenuItem toolStripMenuItemShowFewerButtons = new ToolStripMenuItem();
		private ToolStripSeparator toolStripSeparator = new ToolStripSeparator();

		private ImageAttributes imageAttributesGrayscale = new ImageAttributes();

		/// <summary>
		/// Creates a new side menu control.
		/// </summary>
		public SideMenu()
		{
			// Set the color matrix
			this.imageAttributesGrayscale.SetColorMatrix(new ColorMatrix(
				new float[][]{
					new float[] { 0.3f, 0.3f, 0.3f, 0, 0 },	
					new float[] { 0.59f, 0.59f, 0.59f, 0, 0 },
					new float[] { 0.11f, 0.11f, 0.11f, 0, 0 },
					new float[] { 0, 0, 0, 1, 0, 0 },
					new float[] { 0, 0, 0, 0, 1, 0},
					new float[] { 0, 0, 0, 0, 0, 1}
				}));

			this.controlMenu.Items.AddRange(new ToolStripItem[] {
				this.toolStripMenuItemShowMoreButtons, 
				this.toolStripMenuItemShowFewerButtons,
				this.toolStripSeparator
			});

			this.toolStripMenuItemShowMoreButtons.Size = new Size(200, 22);
			this.toolStripMenuItemShowMoreButtons.Image = Resources.ArrowUp_16;
			this.toolStripMenuItemShowMoreButtons.Text = "Show &More Buttons";
			this.toolStripMenuItemShowMoreButtons.Click += new EventHandler(this.OnShowMoreButtonsClick);

			this.toolStripMenuItemShowFewerButtons.Size = new Size(200, 22);
			this.toolStripMenuItemShowFewerButtons.Image = Resources.ArrowDown_16;
			this.toolStripMenuItemShowFewerButtons.Text = "Show Fe&wer Buttons";
			this.toolStripMenuItemShowFewerButtons.Click += new EventHandler(this.OnShowFewerButtonsClick);

			this.DoubleBuffered = true;

			this.controlMenu.Closed += new ToolStripDropDownClosedEventHandler(this.OnControlMenuClosed);

			// Set the items collection event handlers.
			this.items.BeforeCleared += this.OnBeforeItemsCleared;
			this.items.AfterCleared += this.OnAfterItemsCleared;
			this.items.AfterItemInserted += this.OnAfterItemInserted;
			this.items.AfterItemRemoved += this.OnAfterItemRemoved;
			this.items.AfterItemSet += this.OnAfterItemSet;
		}

		// Public properties.

		/// <summary>
		/// Gets the collection of side menu items.
		/// </summary>
		[DisplayName("Items"), Description("The collection of side menu items"), Category("Menu")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
		public SideMenuItem.Collection Items { get { return this.items; } }
		/// <summary>
		/// Gets the grip height.
		/// </summary>
		[DisplayName("Grip height"), Description("The height of the grip zone in pixels."), Category("Menu")]
		public int GripHeight { get { return this.gripHeight; } }	
		/// <summary>
		/// Gets or sets the menu item height.
		/// </summary>
		[DisplayName("Item height"), Description("The height of a menu item in pixels."), Category("Menu")]
		public int ItemHeight
		{
			get { return this.itemHeight; }
			set { this.OnSetItemHeight(value); }
		}
		/// <summary>
		/// Gets or sets the minimized item width.
		/// </summary>
		[DisplayName("Minimized item width"), Description("The width of a minimized menu item in pixels."), Category("Menu")]
		public int MinimizedItemWidth
		{
			get { return this.minimizedItemWidth; }
			set { this.OnSetMinimizedItemWidth(value); }
		}
		/// <summary>
		/// Gets or sets the minimum panel height.
		/// </summary>
		[DisplayName("Minimum panel height"), Description("The minimum heigth ot the side panel in pixels."), Category("Menu")]
		public int MinimumPanelHeight
		{
			get { return this.minimumPanelHeight; }
			set { this.minimumPanelHeight = value; }
		}
		/// <summary>
		/// Get the index of the selected menu item.
		/// </summary>
		[DisplayName("Selected index"), Description("The index of the selected menu item, or null if no item is selected."), Category("Menu")]
		public int? SelectedIndex
		{
			get { return this.selectedIndex; }
			set { this.OnSetSelectedIndex(value); }
		}
		/// <summary>
		/// Gets or sets the current selected item.
		/// </summary>
		[DisplayName("Selected item"), Description("The selected menu item, or null if no item is selected.")]
		public SideMenuItem SelectedItem
		{
			get { return this.selectedIndex != null ? this.items[this.selectedIndex ?? -1] : null; }
			set { this.OnSetSelectedItem(value); }
		}
		/// <summary>
		/// Gets or sets the number of visible items. Negative values are ignored.
		/// </summary>
		[DisplayName("Visible items"), Description("The number of visible menu items."), Category("Menu")]
		public int VisibleItems
		{
			get { return this.visibleItems; }
			set { this.OnSetVisibleItems(value); }
		}
		/// <summary>
		/// Gets or sets the number of minimized menu items. Negative values are ignored.
		/// </summary>
		[DisplayName("Minimized items"), Description("The number of minimized menu items."), Category("Menu")]
		public int MinimizedItems
		{
			get { return this.minimizedItems; }
			private set { this.OnSetMinimizedItems(value); }
		}
		/// <summary>
		/// Gets or sets the number of hidden menu items. Negative values are ignored.
		/// </summary>
		[DisplayName("Hidden items"), Description("The number of hidden menu items."), Category("Menu")]
		public int HiddenItems
		{
			get { return this.hiddenItems; }
		}
		// Public methods.

		/// <summary>
		/// Adds a new menu item to the side menu.
		/// </summary>
		/// <param name="text">The menu item text.</param>
		/// <param name="imageSmall">The menu item small image.</param>
		/// <param name="imageLarge">The menu item large image.</param>
		/// <param name="handler">The event handler of the menu item.</param>
		/// <param name="tag">The tag of the menu item.</param>
		/// <returns>The side menu item.</returns>
		public SideMenuItem AddItem(
			string text,
			Image imageSmall,
			Image imageLarge,
			SideMenuItemEventHandler handler,
			object tag = null)
		{
			// Create a new side menu item.
			SideMenuItem item = new SideMenuItem(text, imageSmall, imageLarge);
			// Add the item tag.
			item.Tag = tag;
			// Add the user event handler.
			item.Click += handler;
			// Add the menu item to the items list.
			this.items.Add(item);
			// Return the item.
			return item;
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
				// Dispose all menu items.
				foreach (SideMenuItem item in this.items)
				{
					item.Dispose();
				}
				// Dispose the image attributes.
				this.imageAttributesGrayscale.Dispose();
			}
			// Call the base class method.
			base.Dispose(disposing);
		}

		/// <summary>
		/// Event handler for a paint event.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			// Call the base class method.
			base.OnPaint(e);
			// Paint the control.
			this.OnPaintTitle(e.Graphics);
			this.OnPaintItemBackground(e.Graphics, 0, ProfessionalColors.MenuStripGradientEnd, ProfessionalColors.MenuStripGradientBegin);
			this.OnPaintGrip(e.Graphics);
			for (int index = 0; index < this.visibleItems; index++)
				this.OnPaintItem(e.Graphics, index);
			for (int index = 0; index < this.minimizedItems; index++)
				this.OnPaintMinimizedItem(e.Graphics, index);
			this.OnPaintDockButton(e.Graphics);
		}

		/// <summary>
		/// An event handler called when the control is resized.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnResize(EventArgs e)
		{
			// Call the base class method.
			base.OnResize(e);
			
			// Update the maximum number of visible items.
			this.maximumVisibleItems = (this.ClientRectangle.Height - this.titleHeight - this.gripHeight - this.minimumPanelHeight) / this.itemHeight - 1;
			// Update the maximum number of minimized items.
			this.maximumMinimizedItems = (this.ClientRectangle.Width - this.dockButtonWidth) / this.minimizedItemWidth;

			// Update the number of visible items.
			if (this.visibleItems > this.maximumVisibleItems)
			{
				this.OnSetVisibleItems(this.maximumVisibleItems);
			}

			// Update the number of minimized items, if the number is greater than the maximum.
			else if (this.minimizedItems > this.maximumMinimizedItems)
			{
				this.OnSetMinimizedItems(this.maximumMinimizedItems);
			}

			// Update the number of minimized items, if the number of hidden items is greater than zero.
			else if ((this.minimizedItems < this.maximumMinimizedItems) && (this.hiddenItems > 0))
			{
				this.OnSetMinimizedItems(this.minimizedItems + this.hiddenItems);
			}

			// Else, refresh the control
			else
			{
				this.OnRefresh();
			}
		}


		/// <summary>
		/// An event handler called when the mouse leaves the control.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnMouseLeave(EventArgs e)
		{
			// Call the base class methods.
			base.OnMouseLeave(e);
			// Update the control.
			if (null != this.highlightedVisibleIndex)
			{
				this.highlightedVisibleIndex = null;
				this.OnRefresh();
			}
			if (null != this.highlightedMinimizedIndex)
			{
				this.highlightedMinimizedIndex = null;
				this.OnRefresh();
			}
			if ((this.dockButtonSelected) && (!this.controlMenu.Visible))
			{
				this.dockButtonSelected = false;
				this.OnRefresh();
			}
			this.Cursor = System.Windows.Forms.Cursors.Arrow;
		}

		/// <summary>
		/// An event handler called when the mouse moves over the control.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			// Call the base class method.
			base.OnMouseMove(e);
			// Update the control.
			Rectangle rectGrip = new Rectangle(this.ClientRectangle.Left, this.ClientRectangle.Bottom - (this.visibleItems + 1) * this.itemHeight - this.gripHeight, this.Width, this.gripHeight);
			if (this.resizeGrip)
			{
				if ((e.Y >= rectGrip.Bottom + this.itemHeight))
					this.VisibleItems -= 1;
				if ((e.Y <= rectGrip.Bottom - this.itemHeight))
					this.VisibleItems += 1;
				return;
			}
			if (this.itemPressed) return;
			if (this.itemMinimizedPressed) return;
			if (this.dockButtonPressed) return;

			// Visible items
			int visibleIndex = -1;
			if ((e.X >= this.ClientRectangle.Left) && (e.X <= this.ClientRectangle.Right))
			{
				for (int idx = 0; idx < this.visibleItems; idx++)
				{
					if ((e.Y >= this.ClientRectangle.Bottom - (this.visibleItems - idx + 1) * this.itemHeight) &&
						(e.Y < this.ClientRectangle.Bottom - (this.visibleItems - idx) * this.itemHeight))
					{
						visibleIndex = idx;
						break;
					}
				}
			}
			if (visibleIndex != this.highlightedVisibleIndex)
			{
				this.highlightedVisibleIndex = visibleIndex;
				this.OnRefresh();
			}

			// Dock button
			if ((e.X >= this.ClientRectangle.Right - this.dockButtonWidth) && (e.X <= this.ClientRectangle.Right) && (e.Y >= this.ClientRectangle.Bottom - this.itemHeight + 1) && (e.Y <= this.ClientRectangle.Bottom))
			{
				if (!this.dockButtonSelected)
				{
					this.dockButtonSelected = true;
					this.OnRefresh();
				}
			}
			else if (this.dockButtonSelected)
			{
				this.dockButtonSelected = false;
				this.OnRefresh();
			}

			// Minimized items
			int minimizedIndex = -1;
			if ((e.Y > this.ClientRectangle.Bottom - this.itemHeight) && (e.Y < this.ClientRectangle.Bottom))
				for (int idx = 0; idx < this.minimizedItems; idx++)
					if ((e.X >= this.ClientRectangle.Right - this.dockButtonWidth - (this.minimizedItems - idx) * this.minimizedItemWidth) &&
						(e.X <= this.ClientRectangle.Right - this.dockButtonWidth - (this.minimizedItems - idx - 1) * this.minimizedItemWidth))
					{
						minimizedIndex = idx;
						break;
					}
			if (minimizedIndex != this.highlightedMinimizedIndex)
			{
				this.highlightedMinimizedIndex = minimizedIndex;
				this.OnRefresh();
			}

			// Cursor
			if ((e.X >= rectGrip.Left) && (e.X <= rectGrip.Right) && (e.Y >= rectGrip.Top) && (e.Y <= rectGrip.Bottom))
				this.Cursor = System.Windows.Forms.Cursors.SizeNS;
			else if (-1 != visibleIndex)
				this.Cursor = System.Windows.Forms.Cursors.Hand;
			else if (-1 != minimizedIndex)
				this.Cursor = System.Windows.Forms.Cursors.Hand;
			else
				this.Cursor = System.Windows.Forms.Cursors.Arrow;
		}

		/// <summary>
		/// An event handler called when the mouse clicks on the control.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			// Call the base class method.
			base.OnMouseDown(e);
			// Update the control.
			Rectangle rectGrip = new Rectangle(this.ClientRectangle.Left, this.ClientRectangle.Bottom - (this.visibleItems + 1) * this.itemHeight - this.gripHeight, this.ClientRectangle.Width, this.gripHeight);

			if ((e.X >= rectGrip.Left) && (e.X <= rectGrip.Right) && (e.Y >= rectGrip.Top) && (e.Y <= rectGrip.Bottom))
				this.resizeGrip = true;
			if (-1 != this.highlightedVisibleIndex)
			{
				this.itemPressed = true;
				this.OnRefresh();
			}
			if (-1 != this.highlightedMinimizedIndex)
			{
				this.itemMinimizedPressed = true;
				this.OnRefresh();
			}
			if (this.dockButtonSelected)
			{
				this.dockButtonPressed = true;
				this.controlMenu.Show(this, new Point(this.ClientRectangle.Right, this.ClientRectangle.Bottom - this.itemHeight / 2), ToolStripDropDownDirection.Right);
				this.OnRefresh();
			}
		}

		/// <summary>
		/// An event handler called when the mouse releases the on the control.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnMouseUp(MouseEventArgs e)
		{
			// Call the base class method.
			base.OnMouseUp(e);
			// Get the region of normal menu items.
			Rectangle rectButton = new Rectangle(
				this.ClientRectangle.Left,
				this.ClientRectangle.Bottom - (this.visibleItems + 1 - this.highlightedVisibleIndex ?? -1) * this.itemHeight,
				this.ClientRectangle.Width,
				this.itemHeight);
			// Get the region of minimized menu items.
			Rectangle rectMinimizedButton = new Rectangle(
				this.ClientRectangle.Right - this.dockButtonWidth - (this.minimizedItems - this.highlightedMinimizedIndex ?? -1) * this.minimizedItemWidth,
				this.ClientRectangle.Bottom - this.itemHeight,
				this.minimizedItemWidth,
				this.itemHeight);

			// Click on the normal menu items.
			if (this.itemPressed && (e.X >= rectButton.Left) && (e.X <= rectButton.Right) && (e.Y >= rectButton.Top) && (e.Y <= rectButton.Bottom))
			{
				// Compute the index of the selected menu item from the index of the highlighted menu item.
				int? selectedIndex = this.VisibleToGlobalIndex(this.highlightedVisibleIndex);
				// If the selected index is not null.
				if (null != selectedIndex)
				{
					// Deselect the previous selected item, if one was selected.
					if (null != this.selectedIndex)
						this.items[this.selectedIndex ?? -1].Deselect();
					// If the new menu item is enabled.
					if (this.items[selectedIndex ?? -1].Enabled)
					{
						// Change the selected index.
						this.selectedIndex = selectedIndex;
						// Select the menu item.
						this.items[this.selectedIndex ?? -1].Select();
					}
				}
			}
			// Click on the minimized menu items.
			else if (this.itemMinimizedPressed && (e.X >= rectMinimizedButton.Left) && (e.X <= rectMinimizedButton.Right) && (e.Y >= rectMinimizedButton.Top) && (e.Y <= rectMinimizedButton.Bottom))
			{
				// Compute the selected index from the minimized highlighted index.
				int? selectedIndex = this.MinimizedToGlobalIndex(this.highlightedMinimizedIndex);
				// If the selected index is not null.
				if (null != selectedIndex)
				{
					// Deselect the previous selected item.
					if (null != this.selectedIndex)
						this.items[this.selectedIndex ?? -1].Deselect();
					// If the new selected item is enabled.
					if (this.items[selectedIndex ?? -1].Enabled)
					{
						// Change the selected index.
						this.selectedIndex = selectedIndex;
						// Select the menu item.
						this.items[this.selectedIndex ?? -1].Select();
					}
				}
			}

			this.resizeGrip = false;
			this.itemPressed = false;
			this.itemMinimizedPressed = false;
			this.dockButtonPressed = false;
			
			// Refresh the control.
			this.OnRefresh();
		}

		// Private methods.

		/// <summary>
		/// Sets the side menu item height.
		/// </summary>
		/// <param name="itemHeight">The item height.</param>
		private void OnSetItemHeight(int itemHeight)
		{
			// If the item height has not changed, do nothing.
			if (itemHeight == this.itemHeight) return;
			// Set the item height.
			this.itemHeight = itemHeight;
		}

		/// <summary>
		/// Sets the side menu minimized item width.
		/// </summary>
		/// <param name="minimizedItemWidth">The minimized item width.</param>
		private void OnSetMinimizedItemWidth(int minimizedItemWidth)
		{
			// If the item width has not changed, do nothing.
			if (minimizedItemWidth == this.minimizedItemWidth) return;
			// Set the minimized item width.
			this.minimizedItemWidth = minimizedItemWidth;
		}

		/// <summary>
		/// An event handler called when the selected index is set.
		/// </summary>
		/// <param name="index">The selected index.</param>
		private void OnSetSelectedIndex(int? index)
		{
			// If the selected index is greater than the number of items, set the selected index to zero or null.
			if (index > this.items.Count) index = this.items.Count > 0 ? 0 as int? : null;
			// If the selected index is less than zero, set the selected index to zero or null.
			if (index < 0) index = this.items.Count > 0 ? 0 as int? : null;
			// If the current selected index has value.
			if (this.selectedIndex.HasValue)
			{
				// Deselect the menu item at the selected index.
				this.items[this.selectedIndex.Value].Deselect();
			}
			// Set the selected index to the new index.
			this.selectedIndex = index;
			// If the selected index has a value.
			if (this.selectedIndex.HasValue)
			{
				// Select the menu item at the selected index.
				this.items[this.selectedIndex.Value].Select();
			}
			// Refresh the control.
			this.OnRefresh();
		}

		/// <summary>
		/// Changes the current selected item.
		/// </summary>
		/// <param name="item">The new selected item.</param>
		private void OnSetSelectedItem(SideMenuItem item)
		{
			// Get the index of the specified item.
			int index = this.items.IndexOf(item);
			// If the index is not negative, select the specified index.
			if (index >= 0)
			{
				this.OnSetSelectedIndex(index);
			}
		}

		/// <summary>
		/// Sets the number of visible items.
		/// </summary>
		/// <param name="visibleItems">The number of visible items.</param>
		/// <returns><b>True</b> if the control refreshes and generates an item visibility changed event, <b>false</b> otherwise.</returns>
		private bool OnSetVisibleItems(int visibleItems)
		{
			// Ignore negative values.
			if (visibleItems < 0) return false;
			// Save the number of original visible items.
			int originalItems = this.visibleItems;
			// Upper-limit the number of visible items to the maximum number of visible items.
			this.visibleItems = (visibleItems <= this.maximumVisibleItems) ? visibleItems : this.maximumVisibleItems;

			// Update the hidden menu for the visible items.
			for (int index = 0; index < this.visibleItems; index++)
			{
				this.items[index].HiddenMenuItem.Visible = false;
			}
			
			// Set the number of minimized items.
			if (!this.OnSetMinimizedItems(this.items.Count - this.visibleItems))
			{
				//  Refresh the control.
				this.OnRefresh();
			}

			// Update the show more or fewer buttons state.
			this.toolStripMenuItemShowMoreButtons.Enabled = ((this.visibleItems < this.maximumVisibleItems) && (this.visibleItems < this.items.Count));
			this.toolStripMenuItemShowFewerButtons.Enabled = (this.visibleItems > 0);

			return true;
		}

		/// <summary>
		/// Sets the specified number of minimized items.
		/// </summary>
		/// <param name="minimizedItems">The number of minimized items to set.</param>
		/// <return><b>True</b> if the control refreshes and generates an item visibility changed event, <b>false</b> otherwise.</return>
		private bool OnSetMinimizedItems(int minimizedItems)
		{
			// Ignore negative values.
			if (minimizedItems < 0) return false;
			// Save the original number of minimized items.
			int originalItems = this.minimizedItems;
			// Upper-limit the number of minimized items to the maximum number of minimized items.
			this.minimizedItems = (minimizedItems <= this.maximumMinimizedItems) ? minimizedItems : this.maximumMinimizedItems;

			// Update the hidden menu for the minimized items.
			for (int index = this.visibleItems; index < this.visibleItems + this.minimizedItems; index++)
			{
				this.items[index].HiddenMenuItem.Visible = false;
			}

			// Set the number of hidden items.
			if (!this.OnSetHiddenItems())
			{
				// Refresh the control.
				this.OnRefresh();
			}

			return true;
		}

		/// <summary>
		/// Sets the specified number of hidden items.
		/// </summary>
		/// <returns><b>True</b> if the control refreshes and generates an item visibility changed event, <b>false</b> otherwise.</returns>
		private bool OnSetHiddenItems()
		{
			// Ignore negative values.
			if (hiddenItems < 0) return false;
			// Save the original number of hidden items.
			int originalItems = this.hiddenItems;
			// Compute the number of hidden items.
			this.hiddenItems = this.items.Count - this.visibleItems - this.minimizedItems;

			// Update the hidden menu for the hidden items.
			for (int index = this.visibleItems + this.minimizedItems; index < this.items.Count; index++)
			{
				this.items[index].HiddenMenuItem.Visible = true;
			}
			this.toolStripSeparator.Visible = this.visibleItems + this.minimizedItems < this.items.Count;

			// Refresh the control.
			this.OnRefresh();

			return true;
		}

		/// <summary>
		/// Computes the global index of a menu item, based on the visible index.
		/// </summary>
		/// <param name="index">The visible index.</param>
		/// <returns>The global index.</returns>
		private int? VisibleToGlobalIndex(int? index)
		{
			return index;
		}

		/// <summary>
		/// Computes the global index of a menu item, based on the minimized index.
		/// </summary>
		/// <param name="index">The minimized index.</param>
		/// <returns>The global index.</returns>
		private int? MinimizedToGlobalIndex(int? index)
		{
			return index == null ? null : index + this.visibleItems;
		}

		/// <summary>
		/// Computes the global index of a menu item, based on the hidden index.
		/// </summary>
		/// <param name="index">The hidden index.</param>
		/// <returns>The global index.</returns>
		private int? HiddenToGlobalIndex(int? index)
		{
			return index == null ? null : index + this.visibleItems + this.minimizedItems;
		}

		/// <summary>
		/// Returns the visible menu item at a specified global index.
		/// </summary>
		/// <param name="index">The global item index.</param>
		/// <returns>The side menu item or null, if the item at the specified index is not visible.</returns>
		private SideMenuItem GetVisibleItem(int index)
		{
			// Get the global index of the visible index.
			int? idx = this.VisibleToGlobalIndex(index);
			// Return the the side menu item.
			return idx.HasValue ? this.items[idx.Value] : null;
		}

		/// <summary>
		/// Returns the minimized menu item at a specified global index.
		/// </summary>
		/// <param name="index">The global item index.</param>
		/// <returns>The side menu item or null, if the item at the specified index is not minimized.</returns>
		private SideMenuItem GetMinimizedItem(int index)
		{
			// Get th global index of the visible index.
			int? idx = this.MinimizedToGlobalIndex(index);
			// Return the side menu item.
			return idx.HasValue ? this.items[idx.Value] : null;
		}

		/// <summary>
		/// Refreshes the side menu.
		/// </summary>
		private void OnRefresh()
		{
			// Compute the new padding of the side menu;
			Padding padding = new Padding(0, this.titleHeight + 1, 0, this.itemHeight * (this.visibleItems + 1) + this.gripHeight);

			// If the new padding is different from the old padding, update the padding.
			if (this.Padding != padding)
			{
				this.SuspendLayout();
				this.Padding = padding;
				this.ResumeLayout();
			}

			// Invalidate the control area corresponding to user painted buttons.
			this.Invalidate(new Rectangle(0, this.Height - padding.Bottom, this.Width, padding.Bottom));

			// Invalidate the control area corresponding to the title.
			this.Invalidate(new Rectangle(0, 0, this.Width, padding.Top));
		}

		/// <summary>
		/// Paints a normal side menu item.
		/// </summary>
		/// <param name="g">The graphics object.</param>
		/// <param name="index">The menu item index.</param>
		private void OnPaintItem(Graphics g, int index)
		{
			SideMenuItem sideMenuItem = this.GetVisibleItem(index);
			if (null == sideMenuItem) return;

			if (sideMenuItem.Enabled)
				// The side menu item is enabled.
				if (this.highlightedVisibleIndex == index)
					// The side menu item is highlighted.
					if (this.selectedIndex == this.VisibleToGlobalIndex(index))
						// The side menu item is selected.
						if (this.itemPressed)
							// The side menu item is pressed.
							this.OnPaintItemBackground(g, this.visibleItems - index, ProfessionalColors.ButtonPressedHighlight, ProfessionalColors.ButtonPressedHighlight);
						else
							// The side menu item is not pressed.
							this.OnPaintItemBackground(g, this.visibleItems - index, ProfessionalColors.ButtonSelectedGradientBegin, ProfessionalColors.ButtonSelectedGradientEnd);
					else
						// The side menu item is not selected.
						if (this.itemPressed)
							// The side menu item is pressed.
							this.OnPaintItemBackground(g, this.visibleItems - index, ProfessionalColors.ButtonPressedGradientBegin, ProfessionalColors.ButtonPressedGradientEnd);
						else
							// The side menu item is not pressed.
							this.OnPaintItemBackground(g, this.visibleItems - index, ProfessionalColors.ButtonSelectedHighlight, ProfessionalColors.ButtonSelectedHighlight);
				else
					// The side menu item is not highlighted.
					if (this.selectedIndex == this.VisibleToGlobalIndex(index))
						// The side menu item is not selected.
						this.OnPaintItemBackground(g, this.visibleItems - index, ProfessionalColors.ButtonSelectedGradientBegin, ProfessionalColors.ButtonSelectedGradientEnd);
					else
						// The side menu item is not selected.
						this.OnPaintItemBackground(g, this.visibleItems - index, ProfessionalColors.MenuStripGradientEnd, ProfessionalColors.MenuStripGradientBegin);
			else
				// The side menu item is disabled.
				if (this.selectedIndex == this.VisibleToGlobalIndex(index))
					// The side menu item is selected.
					this.OnPaintItemBackground(g, this.visibleItems - index, ProfessionalColors.ButtonSelectedGradientBegin, ProfessionalColors.ButtonSelectedGradientEnd);
				else
					// The side menu item is not selected.
					this.OnPaintItemBackground(g, this.visibleItems - index, ProfessionalColors.MenuStripGradientBegin);


			Rectangle rectText = new Rectangle(this.ClientRectangle.Left + this.itemHeight, this.ClientRectangle.Bottom - (this.visibleItems - index + 1) * this.itemHeight, this.ClientRectangle.Width - 40, this.itemHeight);

			// Draw the text.
			if (null != sideMenuItem.Text)
			{
				TextRenderer.DrawText(
					g,
					sideMenuItem.Text,
					new System.Drawing.Font(SystemFonts.MenuFont, FontStyle.Bold),
					rectText,
					(this.selectedIndex == this.VisibleToGlobalIndex(index)) ? SystemColors.MenuText : SystemColors.MenuText,
					TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
			}
			// Draw the large image.
			if (null != sideMenuItem.ImageLarge)
			{
				int delta = (this.itemHeight - sideMenuItem.ImageLarge.Height) / 2;
				if (sideMenuItem.Enabled)
					g.DrawImage(
						sideMenuItem.ImageLarge,
						new Rectangle(
							this.ClientRectangle.Left + delta,
							this.ClientRectangle.Bottom - (this.visibleItems - index + 1) * this.itemHeight + delta,
							sideMenuItem.ImageLarge.Width,
							sideMenuItem.ImageLarge.Height
							));
				else
					g.DrawImage(
						sideMenuItem.ImageLarge,
						new Rectangle(
							this.ClientRectangle.Left + delta,
							this.ClientRectangle.Bottom - (this.visibleItems - index + 1) * this.itemHeight + delta,
							sideMenuItem.ImageLarge.Width,
							sideMenuItem.ImageLarge.Height),
						0,
						0,
						sideMenuItem.ImageLarge.Width,
						sideMenuItem.ImageLarge.Height,
						GraphicsUnit.Pixel,
						this.imageAttributesGrayscale);
			}
		}

		/// <summary>
		/// Paints the background of a side menu item with a single color.
		/// </summary>
		/// <param name="g">The graphics object.</param>
		/// <param name="index">The menu item index.</param>
		/// <param name="color">The color.</param>
		private void OnPaintItemBackground(Graphics g, int index, Color color)
		{
			Rectangle rect = new Rectangle(this.ClientRectangle.Left, this.ClientRectangle.Bottom - (index + 1) * this.itemHeight, this.ClientRectangle.Width, this.itemHeight);
			using (Pen pen = new Pen(ProfessionalColors.MenuBorder))
			{
				using (Brush brush = new SolidBrush(color))
				{
					g.FillRectangle(brush, rect);
					g.DrawLine(pen, rect.Left, rect.Top, rect.Right, rect.Top);
				}
			}
		}

		/// <summary>
		/// Paints the background of a side menu item with a color gradient.
		/// </summary>
		/// <param name="g">The graphics object.</param>
		/// <param name="index">The menu item index.</param>
		/// <param name="colorBegin">The gradient begin color.</param>
		/// <param name="colorEnd">The gradient end color.</param>
		private void OnPaintItemBackground(Graphics g, int index, Color colorBegin, Color colorEnd)
		{
			Rectangle rect = new Rectangle(
				this.ClientRectangle.Left,
				this.ClientRectangle.Bottom - (index + 1) * this.itemHeight,
				this.ClientRectangle.Width,
				this.itemHeight);
			using (Pen pen = new Pen(ProfessionalColors.MenuBorder))
			{
				using (Brush brush = new LinearGradientBrush(
					rect,
					colorBegin,
					colorEnd,
					LinearGradientMode.Vertical))
				{
					g.FillRectangle(brush, rect);
					g.DrawLine(pen, rect.Left, rect.Top, rect.Right, rect.Top);
				}
			}
		}

		/// <summary>
		/// Paints a minimized menu item with a color gradient.
		/// </summary>
		/// <param name="g">The graphics object.</param>
		/// <param name="index">The menu item index.</param>
		/// <param name="colorBegin">The gradient begin color.</param>
		/// <param name="colorEnd">The gradient end color.</param>
		private void OnPaintMinimizedItemBackground(Graphics g, int index, Color colorBegin, Color colorEnd)
		{
			Rectangle rect = new Rectangle(
				this.ClientRectangle.Right - this.dockButtonWidth - index * this.minimizedItemWidth,
				this.ClientRectangle.Bottom - this.itemHeight + 1,
				this.minimizedItemWidth,
				this.itemHeight - 1);
			using (Brush brush = new LinearGradientBrush(
				rect,
				colorBegin,
				colorEnd,
				LinearGradientMode.Vertical))
			{
				g.FillRectangle(brush, rect);
			}
		}

		/// <summary>
		/// Paints a minimized side menu item.
		/// </summary>
		/// <param name="g">The graphics object.</param>
		/// <param name="index">The menu item index.</param>
		private void OnPaintMinimizedItem(Graphics g, int index)
		{
			SideMenuItem sideMenuItem = this.GetMinimizedItem(index);
			if (null == sideMenuItem) return;

			if (sideMenuItem.Enabled)
			{
				// If the side menu item is enabled.
				if (this.highlightedMinimizedIndex == index)
				{
					// If the side menu item is highlighted.
					if (this.selectedIndex == this.MinimizedToGlobalIndex(index))
						// If the side menu item is selected.
						if (this.itemMinimizedPressed)
							// If the side menu item is pressed.
							this.OnPaintMinimizedItemBackground(g, this.minimizedItems - index, ProfessionalColors.ButtonPressedHighlight, ProfessionalColors.ButtonPressedHighlight);
						else
							// If the side menu item is not pressed.
							this.OnPaintMinimizedItemBackground(g, this.minimizedItems - index, ProfessionalColors.ButtonSelectedGradientBegin, ProfessionalColors.ButtonSelectedGradientEnd);
					else
						// If the side menu item is not selected.
						if (this.itemMinimizedPressed)
							// If the side menu item is pressed.
							this.OnPaintMinimizedItemBackground(g, this.minimizedItems - index, ProfessionalColors.ButtonPressedGradientBegin, ProfessionalColors.ButtonPressedGradientEnd);
						else
							// If the side menu item is not pressed.
							this.OnPaintMinimizedItemBackground(g, this.minimizedItems - index, ProfessionalColors.ButtonSelectedHighlight, ProfessionalColors.ButtonSelectedHighlight);
					// Show the tooltip.
					sideMenuItem.ToolTip.Show(
						sideMenuItem.Text, this,
						this.ClientRectangle.Right - this.dockButtonWidth - (this.minimizedItems - index) * this.minimizedItemWidth,
						this.ClientRectangle.Bottom - this.itemHeight + this.toolTipTopOffset);
				}
				else
				{
					// If the side menu item is not highlighted.
					if (this.selectedIndex == this.MinimizedToGlobalIndex(index))
						// If the side menu item is selected.
						this.OnPaintMinimizedItemBackground(g, this.minimizedItems - index, ProfessionalColors.ButtonSelectedGradientBegin, ProfessionalColors.ButtonSelectedGradientEnd);
					// Hide the tooltip.
					sideMenuItem.ToolTip.Hide(this);
				}
			}

			Rectangle rectText = new Rectangle(this.ClientRectangle.Left + 32, this.ClientRectangle.Bottom - (this.visibleItems-index+1)*this.itemHeight, this.ClientRectangle.Width - 40, this.itemHeight);

			// Draw the small image.
			if (null != sideMenuItem.ImageSmall)
			{
				int deltaLeft = (this.minimizedItemWidth - sideMenuItem.ImageSmall.Width) / 2;
				int deltaTop = (this.itemHeight - sideMenuItem.ImageSmall.Height) / 2;

				if (sideMenuItem.Enabled)
					g.DrawImage(
					sideMenuItem.ImageSmall, 
					this.ClientRectangle.Right - this.dockButtonWidth - (this.minimizedItems - index) * this.minimizedItemWidth + deltaLeft, 
					this.ClientRectangle.Bottom - this.itemHeight + deltaTop, 
					sideMenuItem.ImageSmall.Width, 
					sideMenuItem.ImageSmall.Height);
				else
					g.DrawImage(
						sideMenuItem.ImageSmall,
						new Rectangle(
							this.ClientRectangle.Right - this.dockButtonWidth - (this.minimizedItems - index) * this.minimizedItemWidth + deltaLeft, 
							this.ClientRectangle.Bottom - this.itemHeight + deltaTop, 
							sideMenuItem.ImageSmall.Width,	
							sideMenuItem.ImageSmall.Height),
						0, 0, sideMenuItem.ImageSmall.Width, sideMenuItem.ImageSmall.Height,
						GraphicsUnit.Pixel,
						this.imageAttributesGrayscale);
			}
		}

		/// <summary>
		/// Paints the side menu grip.
		/// </summary>
		/// <param name="g">The graphics object.</param>
		private void OnPaintGrip(Graphics g)
		{
			Rectangle rect = new Rectangle(this.ClientRectangle.Left, this.ClientRectangle.Bottom - (this.visibleItems + 1) * this.itemHeight - this.gripHeight, this.ClientRectangle.Width, this.gripHeight);

			using (Pen pen = new Pen(ProfessionalColors.MenuBorder))
			{
				using (Brush brush = new LinearGradientBrush(
					rect,
					ProfessionalColors.OverflowButtonGradientBegin,
					ProfessionalColors.OverflowButtonGradientEnd,
					LinearGradientMode.Vertical))
				{
					g.FillRectangle(brush, rect);
					g.DrawLine(pen, rect.Left, rect.Top, rect.Right, rect.Top);
				}
			}

			// Draw the grip image.
			g.DrawImage(
				Resources.GripHorizontal,
				rect.Left + rect.Width / 2 - Resources.GripHorizontal.Width / 2,
				rect.Top + 3,
				Resources.GripHorizontal.Width,
				Resources.GripHorizontal.Height
				);
		}

		/// <summary>
		/// Paints the side menu title.
		/// </summary>
		/// <param name="g">The graphics object.</param>
		private void OnPaintTitle(Graphics g)
		{
			Rectangle rect = new Rectangle(this.ClientRectangle.Left, this.ClientRectangle.Top, this.ClientRectangle.Width, this.titleHeight);

			using (Pen pen = new Pen(SystemColors.WindowFrame))
			{
				using (Brush brush = new LinearGradientBrush(
					rect,
					Color.FromArgb(246, 247, 248),
					Color.FromArgb(218, 233, 230),
					LinearGradientMode.Vertical))
				{
					g.FillRectangle(brush, rect);
					g.DrawLine(pen, rect.Left, rect.Bottom, rect.Right, rect.Bottom);

					pen.Color = Color.White;
					g.DrawLine(pen, rect.Left, rect.Top, rect.Left, rect.Bottom - 1);
					g.DrawLine(pen, rect.Left, rect.Top, rect.Right, rect.Top);
				}
			}

			if (null == this.selectedIndex) return;
			if (null == this.items[this.selectedIndex ?? -1]) return;

			Rectangle rectText = new Rectangle(this.ClientRectangle.Left + 5, this.ClientRectangle.Top, this.ClientRectangle.Width - 5, this.titleHeight - 1);

			TextRenderer.DrawText(
				g,
				this.items[this.selectedIndex ?? -1].Text,
				new System.Drawing.Font(SystemFonts.MenuFont.FontFamily, this.titleFontSize, FontStyle.Bold),
				rectText,
				Color.FromArgb(21, 66, 139),
				TextFormatFlags.EndEllipsis | TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
		}

		/// <summary>
		/// Paints the side menu dock button.
		/// </summary>
		/// <param name="g">The graphics object.</param>
		private void OnPaintDockButton(Graphics g)
		{
			Rectangle rect = new Rectangle(this.ClientRectangle.Right - this.dockButtonWidth, this.ClientRectangle.Bottom - this.itemHeight + 1, this.dockButtonWidth, this.itemHeight - 1);

			if (this.dockButtonSelected)
			{
				if (this.dockButtonPressed)
				{
					using (Brush brush = new LinearGradientBrush(
						rect,
						ProfessionalColors.ButtonPressedGradientBegin,
						ProfessionalColors.ButtonPressedGradientEnd,
						LinearGradientMode.Vertical))
					{
						g.FillRectangle(brush, rect);
					}
				}
				else
				{
					using (Brush brush = new LinearGradientBrush(
						rect,
						ProfessionalColors.ButtonSelectedHighlight,
						ProfessionalColors.ButtonSelectedHighlight,
						LinearGradientMode.Vertical))
					{
						g.FillRectangle(brush, rect);
					}
				}			
			}

			// Draw the dock button image.
			g.DrawImage(Resources.DockButton,
				this.ClientRectangle.Right - this.dockButtonWidth / 2 - Resources.DockButton.Width / 2,
				this.ClientRectangle.Bottom - this.itemHeight / 2 - Resources.DockButton.Height / 2,
				Resources.DockButton.Width,
				Resources.DockButton.Height);
		}

		/// <summary>
		/// An event handler called when the side menu item text has changed.
		/// </summary>
		/// <param name="item">The side menu item.</param>
		private void OnItemTextChanged(SideMenuItem item)
		{
			// Refresh the item.
			this.OnRefresh();
		}

		/// <summary>
		/// An event handler called when the side menu item small image has changed.
		/// </summary>
		/// <param name="item">The side menu item.</param>
		private void OnItemSmallImageChanged(SideMenuItem item)
		{
			// Refresh the item.
			this.OnRefresh();
		}

		/// <summary>
		/// An event handler called when the side menu item large image has changed.
		/// </summary>
		/// <param name="item">The side menu item.</param>
		private void OnItemLargeImageChanged(SideMenuItem item)
		{
			// Refresh the item.
			this.OnRefresh();
		}

		/// <summary>
		/// An event handler called when the side menu item control has  changed.
		/// </summary>
		/// <param name="item">The side menu item.</param>
		/// <param name="oldControl">The old control.</param>
		/// <param name="newControl">The new control.</param>
		private void OnItemControlChanged(SideMenuItem item, ISideControl oldControl, ISideControl newControl)
		{
			// Get the index of the side menu item.
			int index = this.items.IndexOf(item);
			// Hide the old control.
			if (null != oldControl)
			{
				oldControl.HideSideControl();
			}
			// Show the new control.
			if (null != newControl)
			{
				if (index == this.selectedIndex)
				{
					newControl.ShowSideControl();
				}
				else
				{
					newControl.HideSideControl();
				}
			}
		}

		/// <summary>
		/// An event handler called when the user clicks on a hidden item.
		/// </summary>
		/// <param name="item">The side menu item.</param>
		private void OnItemHiddenClick(SideMenuItem item)
		{
			// Get the index of the menu item.
			int index = this.items.IndexOf(item);

			//  If the selected index changed.
			if (this.selectedIndex != index)
			{
				// Deselect the currently selected item.
				if (null != this.selectedIndex)
					this.items[this.selectedIndex ?? -1].Deselect();

				// Change the selected index.
				this.selectedIndex = index;

				// Select the currently selected item.
				this.items[index].Select();

				this.OnRefresh();
			}
		}

		/// <summary>
		/// An event handler called when the user clicks on the show more button.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void OnShowMoreButtonsClick(object sender, EventArgs e)
		{
			this.VisibleItems++;
		}

		/// <summary>
		/// An event handler called when the user clicks on the show fewer button.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void OnShowFewerButtonsClick(object sender, EventArgs e)
		{
			this.VisibleItems--;
		}

		/// <summary>
		/// An event handler called when the control menu is closed.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void OnControlMenuClosed(object sender, ToolStripDropDownClosedEventArgs e)
		{
			// Depress and deselect the dock button.
			this.dockButtonPressed = false;
			this.dockButtonSelected = false;
			// Refresh the control.
			this.OnRefresh();
		}

		/// <summary>
		/// An event handler called before the menu item collection has been cleared.
		/// </summary>
		private void OnBeforeItemsCleared()
		{
			// Remove the event handlers and hidden menu for all items.
			foreach (SideMenuItem item in this.items)
			{
				// Remove the event handlers.
				item.TextChanged -= this.OnItemTextChanged;
				item.SmallImageChanged -= this.OnItemSmallImageChanged;
				item.LargeImageChanged -= this.OnItemLargeImageChanged;
				item.ControlChanged -= this.OnItemControlChanged;
				item.HiddenClick -= this.OnItemHiddenClick;

				// Remove the hidden menu.
				this.controlMenu.Items.Remove(item.HiddenMenuItem);
			}
		}

		/// <summary>
		/// An event handler called after the menu item collection has been cleared.
		/// </summary>
		private void OnAfterItemsCleared()
		{
			// Update the number of visible items.
			this.VisibleItems = this.items.Count;
			// Change the selected index.
			this.selectedIndex = null;
			// Refresh the control.
			this.OnRefresh();
		}

		/// <summary>
		/// An event handler called after a new menu item has been inserted.
		/// </summary>
		/// <param name="index">The item index.</param>
		/// <param name="item">The menu item</param>
		private void OnAfterItemInserted(int index, SideMenuItem item)
		{
			// Add the item event handlers.
			item.TextChanged += this.OnItemTextChanged;
			item.SmallImageChanged += this.OnItemSmallImageChanged;
			item.LargeImageChanged += this.OnItemLargeImageChanged;
			item.ControlChanged += this.OnItemControlChanged;
			item.HiddenClick += this.OnItemHiddenClick;
			
			// Add the hidden menu item.
			this.controlMenu.Items.Add(item.HiddenMenuItem);

			// If this is the first item.
			if (this.items.Count == 1)
			{
				// Select the item.
				item.Select();
				// Change the selected index.
				this.selectedIndex = 0;
			}
			// Update the number of visible items.
			this.VisibleItems = this.items.Count;
		}

		/// <summary>
		/// An event handler called after an existing menu item has been removed.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="item">The menu item.</param>
		private void OnAfterItemRemoved(int index, SideMenuItem item)
		{
			// Remove the hidden event handler.
			item.TextChanged -= this.OnItemTextChanged;
			item.SmallImageChanged -= this.OnItemSmallImageChanged;
			item.LargeImageChanged -= this.OnItemLargeImageChanged;
			item.ControlChanged -= this.OnItemControlChanged;
			item.HiddenClick -= this.OnItemHiddenClick;

			// Remove the hidden menu item.
			this.controlMenu.Items.Remove(item.HiddenMenuItem);

			// If this is the selected item.
			if (this.selectedIndex == index)
			{
				// If the item list is not empty.
				if (this.items.Count > 0)
				{
					// Select the first item.
					this.items[0].Select();
					// Change the selected index.
					this.selectedIndex = 0;
				}
				else
				{
					this.selectedIndex = null;
				}
			}
			// Update the number of visible items
			this.VisibleItems = this.items.Count;
		}

		/// <summary>
		/// An event handler called after a menu item has been set.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="oldItem">The old menu item.</param>
		/// <param name="newItem">The new menu item.</param>
		private void OnAfterItemSet(int index, SideMenuItem oldItem, SideMenuItem newItem)
		{
			// Check that the arguments are not null.
			if (null == oldItem) throw new ArgumentNullException("oldItem");
			if (null == newItem) throw new ArgumentNullException("newItem");

			// If the new and the old item are the same, do nothing.
			if (oldItem == newItem) return;

			// Remove the event handlers from the old item.
			oldItem.TextChanged -= this.OnItemTextChanged;
			oldItem.SmallImageChanged -= this.OnItemSmallImageChanged;
			oldItem.LargeImageChanged -= this.OnItemLargeImageChanged;
			oldItem.ControlChanged -= this.OnItemControlChanged;
			oldItem.HiddenClick -= this.OnItemHiddenClick;
			
			// Add the event handlers to the new item.
			newItem.TextChanged += this.OnItemTextChanged;
			newItem.SmallImageChanged += this.OnItemSmallImageChanged;
			newItem.LargeImageChanged += this.OnItemLargeImageChanged;
			newItem.ControlChanged += this.OnItemControlChanged;
			newItem.HiddenClick += this.OnItemHiddenClick;

			// Remove the old hidden menu item from the items list.
			this.controlMenu.Items.Remove(oldItem.HiddenMenuItem);
			// Add the new hidden menu item to the items list.
			this.controlMenu.Items.Add(newItem.HiddenMenuItem);

			// Refresh the control.
			this.OnRefresh();
		}
	}
}
