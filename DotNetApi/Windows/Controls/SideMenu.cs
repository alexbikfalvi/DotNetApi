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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace DotNetApi.Windows.Controls
{
	public class SideMenu : Panel
	{
		// Private members
		private List<SideMenuItem> items = new List<SideMenuItem>();

		private int dockButtonWidth = 18;				// The width of the dock button.
		private int gripHeight = 8;						// The grip height.
		private int? highlightedVisibleIndex = null;	// The index of the highlighted visible item.
		private int? highlightedMinimizedIndex = null;	// The index of the highlighted minimized item.
		private int? selectedIndex = null;				// The index of the selected menu item.
		private int itemHeight = 48;					// The menu item height.
		private int titleHeight = 27;					// The height of the title.
		private int minimumPanelHeight = 50;			// The minimum panel height.
		private int minimizedItemWidth = 25;			// The width of a minimized item.
		private int hiddenItems = 0;					// The number of hidden items. 
		private int minimizedItems = 0;					// The number of minimized items.
		private int visibleItems = 0;					// The number of visible items;
		private float titleFontSize = 12.0f;			// The title font size.

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
				this.toolStripMenuItemShowFewerButtons
			});

			this.toolStripMenuItemShowMoreButtons.Size = new Size(200, 22);
			this.toolStripMenuItemShowMoreButtons.Image = Resources.ArrowUp_16;
			this.toolStripMenuItemShowMoreButtons.Text = "Show &More Buttons";
			this.toolStripMenuItemShowMoreButtons.Click += new EventHandler(this.ShowMoreButtonsClick);

			this.toolStripMenuItemShowFewerButtons.Size = new Size(200, 22);
			this.toolStripMenuItemShowFewerButtons.Image = Resources.ArrowDown_16;
			this.toolStripMenuItemShowFewerButtons.Text = "Show Fe&wer Buttons";
			this.toolStripMenuItemShowFewerButtons.Click += new EventHandler(this.ShowFewerButtonsClick);

			this.DoubleBuffered = true;

			this.Paint += new PaintEventHandler(this.PaintSideMenu);
			this.MouseLeave += new EventHandler(this.MouseLeaveControl);
			this.MouseMove += new MouseEventHandler(this.MouseMoveControl);
			this.MouseDown += new MouseEventHandler(this.MouseDownControl);
			this.MouseUp += new MouseEventHandler(this.MouseUpControl);
			this.Resize += new EventHandler(this.ControlResize);

			this.controlMenu.Closed += new ToolStripDropDownClosedEventHandler(this.ControlMenuClosed);

		}

		/// <summary>
		/// Returns the number of enabled menu items.
		/// </summary>
		[DisplayName("Enabled items"), Description("The number of enabled menu items."), Category("Menu")]
		public int EnabledItems
		{
			get
			{
				int visibleItems = 0;
				foreach(SideMenuItem item in this.items)
					visibleItems += null != item ? item.Visible ? 1 : 0 : 0;
				return visibleItems;
			}
		}
		
		/// <summary>
		/// Gets the grip height.
		/// </summary>
		[DisplayName("Grip height"), Description("The height of the grip zone in pixels."), Category("Menu")]
		public int GripHeight { get { return this.gripHeight; } }
		
		/// <summary>
		/// Gets or sets the number of hidden menu items.
		/// </summary>
		[DisplayName("Hidden items"), Description("The number of hidden menu items."), Category("Menu")]
		public int HiddenItems
		{
			get { return this.hiddenItems; }
			set
			{
				// Save the original number of hidden items.
				int originalItems = this.hiddenItems;
				// Upper-limit the number of hidden items to the enabled without the visible and minimized.
				this.hiddenItems = (this.EnabledItems - this.visibleItems - this.minimizedItems < value) ?
					this.EnabledItems - this.visibleItems - this.minimizedItems : value;
				// If the number of hidden items different from the original.
				if (this.hiddenItems != originalItems)
				{
					// Remove all items from the control menu.
					while(this.controlMenu.Items.Count > 2) this.controlMenu.Items.RemoveAt(2);
					// If there are hidden items.
					if (this.hiddenItems > 0)
					{
						// Add a separator to the control menu.
						this.controlMenu.Items.Add(this.toolStripSeparator);
						// 
						for (int index = 0; index < this.hiddenItems; index++)
						{
							int? idx = this.HiddenToGlobalIndex(index);

							if (null != idx) this.controlMenu.Items.Add(this.items[idx ?? -1].HiddenMenuItem);
						}
					}
				}
			}
		}
		
		/// <summary>
		/// Gets or sets the menu item height.
		/// </summary>
		[DisplayName("Item height"), Description("The height of a menu item in pixels."), Category("Menu")]
		public int ItemHeight
		{
			get { return this.itemHeight; }
			set { this.itemHeight = value; }
		}

		/// <summary>
		/// Gets or sets the minimized item width.
		/// </summary>
		[DisplayName("Minimized item width"), Description("The width of a minimized menu item in pixels."), Category("Menu")]
		public int MinimizedItemWidth
		{
			get { return this.minimizedItemWidth; }
			set { this.minimizedItemWidth = value; }
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
		/// Gets or sets the number of minimized menu items.
		/// </summary>
		[DisplayName("Minimized items"), Description("The number of minimized menu items."), Category("Menu")]
		public int MinimizedItems 
		{
			get { return this.minimizedItems; }
			set
			{
				// Save the original number of minimized items.
				int originalItems = this.minimizedItems;
				// Upper-limit the number of items that can be minimized to the difference between enabled an visible items.
				this.minimizedItems = (this.EnabledItems - this.visibleItems < value) ?
					this.EnabledItems - this.visibleItems : value;
				// Upper-limit the number of minimzed items by the width of the button.
				if (this.minimizedItems * this.minimizedItemWidth + this.dockButtonWidth > this.Width)
					this.minimizedItems = (this.Width - this.dockButtonWidth) / this.minimizedItemWidth;
				// Lower-limit the number of minimzed items to zero.
				if (this.minimizedItems < 0) this.minimizedItems = 0;
				// Set the number of hidden items
				this.HiddenItems = this.EnabledItems - this.visibleItems - this.minimizedItems;
				// Refresh the control.
				if (this.minimizedItems != originalItems) this.Refresh();
			}
		}

		/// <summary>
		/// Get the index of the selected menu item.
		/// </summary>
		[DisplayName("Selected index"), Description("The index of the selected menu item, or null if no item is selected."), Category("Menu")]
		public int? SelectedIndex { get { return this.selectedIndex; } }

		/// <summary>
		/// Gets or sets the current selected item.
		/// </summary>
		[DisplayName("Selecte item"), Description("The selected menu item, or null if no item is selected.")]
		public SideMenuItem SelectedItem
		{
			get { return this.selectedIndex != null ? this.items[this.selectedIndex ?? -1] : null; }
			set
			{
				// Change the selected index, and if the selected index is not -1.
				int index;
				if (-1 != (index = this.items.IndexOf(value)))
				{
					// If the index is different from the selected index.
					if (index != this.selectedIndex)
					{
						// Change the selected index.
						this.selectedIndex = index;
						// Select the menu item.
						value.Select();
						// Refresh the control.
						this.Refresh();
					}
				}
				else
				{
					// Set the selected index to null.
					this.selectedIndex = null;
				}
			}
		}

		/// <summary>
		/// Gets or sets the number of visible items.
		/// </summary>
		[DisplayName("Visible items"), Description("The number of visible menu items."), Category("Menu")]
		public int VisibleItems
		{
			get { return this.visibleItems; }
			set
			{
				// Save the original number of visible items.
				int originalItems = this.visibleItems;
				// Upper-limit the number of visible items to the number of enabled items.
				this.visibleItems = (this.EnabledItems < value) ? this.EnabledItems : value;
				// Upper-limit the number of visible items to the minimum panel height.
				if ((this.visibleItems + 1) * this.itemHeight + this.gripHeight + this.minimumPanelHeight > this.Height)
					this.visibleItems = (this.Height - this.minimumPanelHeight - this.gripHeight) / this.itemHeight - 1;
				// Lower-limit the number of visible items to zero.
				if (this.visibleItems < 0) this.visibleItems = 0;
				// Update the number of minimized items.
				this.MinimizedItems = this.EnabledItems - this.visibleItems;
				// Refresh the control.
				if (this.visibleItems != originalItems) this.Refresh();
				// Update the show more or fewer buttons state.
				this.toolStripMenuItemShowMoreButtons.Enabled = (this.visibleItems == this.EnabledItems) ? false : true;
				this.toolStripMenuItemShowFewerButtons.Enabled = (this.visibleItems == 0) ? false : true;
			}
		}

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
			SideMenuEventHandler handler,
			object tag = null)
		{
			// Create a new side menu item.
			SideMenuItem item = new SideMenuItem(text, imageSmall, imageLarge, this.HiddenItemClick);
			// Add the item tag.
			item.Tag = tag;
			// Add the user event handler.
			item.Click += handler;
			// Add the menu item to the items list.
			this.items.Add(item);
			// Update the number of visible items
			this.VisibleItems = this.items.Count;
			// If this the first item, select it.
			if (this.items.Count == 1)
			{
				item.Select();
				this.selectedIndex = 0;
			}
			// Return the item.
			return item;
		}

		/// <summary>
		/// Computes the global index of a menu item, based on the visible index.
		/// </summary>
		/// <param name="index">The visible index.</param>
		/// <returns>The global index.</returns>
		private int? VisibleToGlobalIndex(int? index)
		{
			if (null == index) return null;
			if (index >= this.items.Count) return null; // Return null when outside the items count.

			int visibleIndex = 0;
			for (int idx = 0; (idx < this.items.Count) && (visibleIndex < this.visibleItems); idx++)
			{
				if (null != this.items[idx])
				{
					if (this.items[idx].Visible)
					{
						if (visibleIndex == index) return idx;
						visibleIndex++;
					}
				}
			}
			return null;
		}

		/// <summary>
		/// Computes the global index of a menu item, based on the minimized index.
		/// </summary>
		/// <param name="index">The minimized index.</param>
		/// <returns>The global index.</returns>
		private int? MinimizedToGlobalIndex(int? index)
		{
			if (null == index) return null;
			if (index >= this.items.Count) return null; // Return null when outside the items count.

			int? lastVisibleIndex = this.VisibleToGlobalIndex(this.visibleItems - 1);
			int minimizedIndex = 0;

			if ((null == lastVisibleIndex) && (this.visibleItems > 0)) return null; // Return null when all items are visible.

			for (int idx = (lastVisibleIndex ?? -1) + 1; (idx < this.items.Count) && (minimizedIndex < this.minimizedItems); idx++)
			{
				if (null != this.items[idx])
				{
					if (this.items[idx].Visible)
					{
						if (minimizedIndex == index)
							return idx;
						minimizedIndex++;
					}
				}
			}
			return null;
		}

		/// <summary>
		/// Computes the global index of a menu item, based on the hidden index.
		/// </summary>
		/// <param name="index">The hidden index.</param>
		/// <returns>The global index.</returns>
		private int? HiddenToGlobalIndex(int? index)
		{
			if (null == index) return null;
			if (index >= this.items.Count) return null; // Return null when outside the items count.

			int? lastIndex = this.MinimizedToGlobalIndex(this.minimizedItems - 1);
			int hiddenIndex = 0;

			if ((null == lastIndex) && (this.minimizedItems > 0)) return null; // Return null when all items are minimized.
			if (null == lastIndex)
			{
				lastIndex = this.VisibleToGlobalIndex(this.visibleItems - 1);
				if ((null == lastIndex) && (this.visibleItems > 0)) return null; // Return null when all items are visible.
			}

			for (int idx = (lastIndex ?? -1) + 1; (idx < this.items.Count) && (hiddenIndex < this.hiddenItems); idx++)
			{
				if (null != this.items[idx])
				{
					if (this.items[idx].Visible)
					{
						if (hiddenIndex == index) return idx;
						hiddenIndex++;
					}
				}
			}
			return null;
		}

		/// <summary>
		/// Refreshes the side menu.
		/// </summary>
		private new void Refresh()
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
		/// Event handler for a paint event.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void PaintSideMenu(object sender, PaintEventArgs e)
		{
			this.PaintTitle(e.Graphics);
			this.PaintItemBackground(e.Graphics, 0, ProfessionalColors.MenuStripGradientEnd, ProfessionalColors.MenuStripGradientBegin);
			this.PaintGrip(e.Graphics);
			for (int index = 0; index < this.visibleItems; index++)
				this.PaintItem(e.Graphics, index);
			for (int index = 0; index < this.minimizedItems; index++)
				this.PaintMinimizedItem(e.Graphics, index);
			this.PaintDockButton(e.Graphics);
		}

		/// <summary>
		/// Paints the background of a side menu item with a single color.
		/// </summary>
		/// <param name="g">The graphics object.</param>
		/// <param name="index">The menu item index.</param>
		/// <param name="color">The color.</param>
		private void PaintItemBackground(Graphics g, int index, Color color)
		{
			Rectangle rect = new Rectangle(this.ClientRectangle.Left, this.ClientRectangle.Bottom - (index + 1) * this.itemHeight, this.ClientRectangle.Width, this.itemHeight);
			Pen pen = new Pen(ProfessionalColors.MenuBorder);
			Brush brush = new SolidBrush(color);

			g.FillRectangle(brush, rect);
			g.DrawLine(pen, rect.Left, rect.Top, rect.Right, rect.Top);
		}

		/// <summary>
		/// Paints the background of a side menu item with a color gradient.
		/// </summary>
		/// <param name="g">The graphics object.</param>
		/// <param name="index">The menu item index.</param>
		/// <param name="colorBegin">The gradient begin color.</param>
		/// <param name="colorEnd">The gradient end color.</param>
		private void PaintItemBackground(Graphics g, int index, Color colorBegin, Color colorEnd)
		{
			Rectangle rect = new Rectangle(this.ClientRectangle.Left, this.ClientRectangle.Bottom - (index + 1) * this.itemHeight, this.ClientRectangle.Width, this.itemHeight);
			Pen pen = new Pen(ProfessionalColors.MenuBorder);
			Brush brush = new LinearGradientBrush(
				rect, 
				colorBegin, 
				colorEnd, 
				LinearGradientMode.Vertical);

			g.FillRectangle(brush, rect);
			g.DrawLine(pen, rect.Left, rect.Top, rect.Right, rect.Top);
		}

		/// <summary>
		/// Paints a minimized menu item with a color gradient.
		/// </summary>
		/// <param name="g">The graphics object.</param>
		/// <param name="index">The menu item index.</param>
		/// <param name="colorBegin">The gradient begin color.</param>
		/// <param name="colorEnd">The gradient end color.</param>
		private void PaintMinimizedItemBackground(Graphics g, int index, Color colorBegin, Color colorEnd)
		{
			Rectangle rect = new Rectangle(this.ClientRectangle.Right - this.dockButtonWidth - index * this.minimizedItemWidth, this.ClientRectangle.Bottom - this.itemHeight + 1, this.minimizedItemWidth, this.itemHeight - 1);
			Brush brush = new LinearGradientBrush(
				rect, 
				colorBegin, 
				colorEnd, 
				LinearGradientMode.Vertical);

			g.FillRectangle(brush, rect);
		}

		/// <summary>
		/// Paints a normal side menu item.
		/// </summary>
		/// <param name="g">The graphics object.</param>
		/// <param name="index">The menu item index.</param>
		private void PaintItem(Graphics g, int index)
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
							this.PaintItemBackground(g, this.visibleItems - index, ProfessionalColors.ButtonPressedHighlight, ProfessionalColors.ButtonPressedHighlight);
						else
							// The side menu item is not pressed.
							this.PaintItemBackground(g, this.visibleItems - index, ProfessionalColors.ButtonSelectedGradientBegin, ProfessionalColors.ButtonSelectedGradientEnd);
					else
						// The side menu item is not selected.
						if (this.itemPressed)
							// The side menu item is pressed.
							this.PaintItemBackground(g, this.visibleItems - index, ProfessionalColors.ButtonPressedGradientBegin, ProfessionalColors.ButtonPressedGradientEnd);
						else
							// The side menu item is not pressed.
							this.PaintItemBackground(g, this.visibleItems - index, ProfessionalColors.ButtonSelectedHighlight, ProfessionalColors.ButtonSelectedHighlight);
				else
					// The side menu item is not highlighted.
					if (this.selectedIndex == this.VisibleToGlobalIndex(index))
						// The side menu item is not selected.
						this.PaintItemBackground(g, this.visibleItems - index, ProfessionalColors.ButtonSelectedGradientBegin, ProfessionalColors.ButtonSelectedGradientEnd);
					else
						// The side menu item is not selected.
						this.PaintItemBackground(g, this.visibleItems - index, ProfessionalColors.MenuStripGradientEnd, ProfessionalColors.MenuStripGradientBegin);
			else
				// The side menu item is disabled.
				if (this.selectedIndex == this.VisibleToGlobalIndex(index))
					// The side menu item is selected.
					this.PaintItemBackground(g, this.visibleItems - index, ProfessionalColors.ButtonSelectedGradientBegin, ProfessionalColors.ButtonSelectedGradientEnd);
				else
					// The side menu item is not selected.
					this.PaintItemBackground(g, this.visibleItems - index, ProfessionalColors.MenuStripGradientBegin);


			Rectangle rectText = new Rectangle(this.ClientRectangle.Left + this.itemHeight, this.ClientRectangle.Bottom - (this.visibleItems - index + 1) * this.itemHeight, this.ClientRectangle.Width - 40, this.itemHeight);

			// Draw the text.
			if (null != sideMenuItem.Text)
				TextRenderer.DrawText(
					g,
					sideMenuItem.Text,
					new System.Drawing.Font(SystemFonts.MenuFont, FontStyle.Bold),
					rectText,
					(this.selectedIndex == this.VisibleToGlobalIndex(index))?SystemColors.MenuText:SystemColors.GrayText,
					TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
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
		/// Paints a minimized side menu item.
		/// </summary>
		/// <param name="g">The graphics object.</param>
		/// <param name="index">The menu item index.</param>
		private void PaintMinimizedItem(Graphics g, int index)
		{
			SideMenuItem sideMenuItem = this.GetMinimizedItem(index);
			if (null == sideMenuItem) return;

			if (sideMenuItem.Enabled)
			{
				// If the side menu item is enabled.
				if (this.highlightedMinimizedIndex == index)
					// If the side menu item is highlighted.
					if (this.selectedIndex == this.MinimizedToGlobalIndex(index))
						// If the side menu item is selected.
						if (this.itemMinimizedPressed)
							// If the side menu item is pressed.
							this.PaintMinimizedItemBackground(g, this.minimizedItems - index, ProfessionalColors.ButtonPressedHighlight, ProfessionalColors.ButtonPressedHighlight);
						else
							// If the side menu item is not pressed.
							this.PaintMinimizedItemBackground(g, this.minimizedItems - index, ProfessionalColors.ButtonSelectedGradientBegin, ProfessionalColors.ButtonSelectedGradientEnd);
					else
						// If the side menu item is not selected.
						if (this.itemMinimizedPressed)
							// If the side menu item is pressed.
							this.PaintMinimizedItemBackground(g, this.minimizedItems - index, ProfessionalColors.ButtonPressedGradientBegin, ProfessionalColors.ButtonPressedGradientEnd);
						else
							// If the side menu item is not pressed.
							this.PaintMinimizedItemBackground(g, this.minimizedItems - index, ProfessionalColors.ButtonSelectedHighlight, ProfessionalColors.ButtonSelectedHighlight);
				else
					// If the side menu item is not highlighted.
					if (this.selectedIndex == this.MinimizedToGlobalIndex(index))
						// If the side menu item is selected.
						this.PaintMinimizedItemBackground(g, this.minimizedItems - index, ProfessionalColors.ButtonSelectedGradientBegin, ProfessionalColors.ButtonSelectedGradientEnd);
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
		private void PaintGrip(Graphics g)
		{
			Rectangle rect = new Rectangle(this.ClientRectangle.Left, this.ClientRectangle.Bottom - (this.visibleItems + 1) * this.itemHeight - this.gripHeight, this.ClientRectangle.Width, this.gripHeight);
			Pen pen = new Pen(ProfessionalColors.MenuBorder);
			Brush brush = new LinearGradientBrush(
				rect, 
				ProfessionalColors.OverflowButtonGradientBegin, 
				ProfessionalColors.OverflowButtonGradientEnd, 
				LinearGradientMode.Vertical);

			g.FillRectangle(brush, rect);
			g.DrawLine(pen, rect.Left, rect.Top, rect.Right, rect.Top);

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
		private void PaintTitle(Graphics g)
		{
			Rectangle rect = new Rectangle(this.ClientRectangle.Left, this.ClientRectangle.Top, this.ClientRectangle.Width, this.titleHeight);

			Pen pen = new Pen(SystemColors.WindowFrame);
			Brush brush = new LinearGradientBrush(
				rect, 
				Color.FromArgb(246, 247, 248),
				Color.FromArgb(218, 233, 230),
				LinearGradientMode.Vertical);
			g.FillRectangle(brush, rect);
			g.DrawLine(pen, rect.Left, rect.Bottom, rect.Right, rect.Bottom);

			pen = new Pen(Color.White);
			g.DrawLine(pen, rect.Left, rect.Top, rect.Left, rect.Bottom-1);
			g.DrawLine(pen, rect.Left, rect.Top, rect.Right, rect.Top);

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
		private void PaintDockButton(Graphics g)
		{
			Rectangle rect = new Rectangle(this.ClientRectangle.Right - this.dockButtonWidth, this.ClientRectangle.Bottom - this.itemHeight + 1, this.dockButtonWidth, this.itemHeight - 1);
			Brush brush;

			if (this.dockButtonSelected)
			{
				if (this.dockButtonPressed)
				{
					brush = new LinearGradientBrush(
						rect, 
						ProfessionalColors.ButtonPressedGradientBegin, 
						ProfessionalColors.ButtonPressedGradientEnd, 
						LinearGradientMode.Vertical);
				}
				else
				{
					brush = new LinearGradientBrush(
						rect, 
						ProfessionalColors.ButtonSelectedHighlight, 
						ProfessionalColors.ButtonSelectedHighlight, 
						LinearGradientMode.Vertical);
				}
				g.FillRectangle(brush, rect);
			}

			// Draw the dock button image.
			g.DrawImage(Resources.DockButton,
				this.ClientRectangle.Right - this.dockButtonWidth / 2 - Resources.DockButton.Width / 2,
				this.ClientRectangle.Bottom - this.itemHeight / 2 - Resources.DockButton.Height / 2,
				Resources.DockButton.Width,
				Resources.DockButton.Height);
		}

		/// <summary>
		/// An event handler called when the mouse leaves the control.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void MouseLeaveControl(object sender, EventArgs e)
		{
			if (-1 != this.highlightedVisibleIndex)
			{
				this.highlightedVisibleIndex = -1;
				this.Refresh();
			}
			if (-1 != this.highlightedMinimizedIndex)
			{
				this.highlightedMinimizedIndex = -1;
				this.Refresh();
			}
			if ((this.dockButtonSelected) && (!this.controlMenu.Visible))
			{
				this.dockButtonSelected = false;
				this.Refresh();
			}
			this.Cursor = Cursors.Arrow;
		}

		/// <summary>
		/// An event handler called when the mouse moves over the control.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void MouseMoveControl(object sender, MouseEventArgs e)
		{
			Rectangle rectGrip = new Rectangle(this.ClientRectangle.Left, this.ClientRectangle.Bottom - (this.visibleItems+1)*this.itemHeight - this.gripHeight, this.Width, this.gripHeight);
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
					if ((e.Y >= this.ClientRectangle.Bottom - (this.visibleItems-idx+1)*this.itemHeight) &&
						(e.Y < this.ClientRectangle.Bottom - (this.visibleItems-idx)*this.itemHeight))
					{
						visibleIndex = idx;
						break;
					}
				}
			}
			if (visibleIndex != this.highlightedVisibleIndex)
			{
				this.highlightedVisibleIndex = visibleIndex;
				this.Refresh();
			}

			// Dock button
			if ((e.X >= this.ClientRectangle.Right-this.dockButtonWidth) && (e.X <= this.ClientRectangle.Right) && (e.Y >= this.ClientRectangle.Bottom-this.itemHeight+1) && (e.Y <= this.ClientRectangle.Bottom))
			{
				if (!this.dockButtonSelected)
				{
					this.dockButtonSelected = true;
					this.Refresh();
				}
			}
			else if (this.dockButtonSelected)
			{
				this.dockButtonSelected = false;
				this.Refresh();
			}

			// Minimized items
			int minimizedIndex = -1;
			if ((e.Y > this.ClientRectangle.Bottom-this.itemHeight) && (e.Y < this.ClientRectangle.Bottom))
				for (int idx = 0; idx < this.minimizedItems; idx++)
					if ((e.X >= this.ClientRectangle.Right - this.dockButtonWidth - (this.minimizedItems-idx)*this.minimizedItemWidth) &&
						(e.X <= this.ClientRectangle.Right - this.dockButtonWidth - (this.minimizedItems-idx-1)*this.minimizedItemWidth))
					{
						minimizedIndex = idx;
						break;
					}
			if (minimizedIndex != this.highlightedMinimizedIndex)
			{
				this.highlightedMinimizedIndex = minimizedIndex;
				this.Refresh();
			}

			// Cursor
			if ((e.X >= rectGrip.Left) && (e.X <= rectGrip.Right) && (e.Y >= rectGrip.Top) && (e.Y <= rectGrip.Bottom))
				this.Cursor = Cursors.SizeNS;
			else if (-1 != visibleIndex)
				this.Cursor = Cursors.Hand;
			else if (-1 != minimizedIndex)
				this.Cursor = Cursors.Hand;
			else
				this.Cursor = Cursors.Arrow;
		}

		/// <summary>
		/// An event handler called when the mouse clicks on the control.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void MouseDownControl(object sender, MouseEventArgs e)
		{
			Rectangle rectGrip = new Rectangle(this.ClientRectangle.Left, this.ClientRectangle.Bottom - (this.visibleItems + 1) * this.itemHeight - this.gripHeight, this.ClientRectangle.Width, this.gripHeight);

			if ((e.X >= rectGrip.Left) && (e.X <= rectGrip.Right) && (e.Y >= rectGrip.Top) && (e.Y <= rectGrip.Bottom))
				this.resizeGrip = true;
			if (-1 != this.highlightedVisibleIndex)
			{
				this.itemPressed = true;
				this.Refresh();
			}
			if (-1 != this.highlightedMinimizedIndex)
			{
				this.itemMinimizedPressed = true;
				this.Refresh();
			}
			if (this.dockButtonSelected)
			{
				this.dockButtonPressed = true;
				this.controlMenu.Show(this, new Point(this.ClientRectangle.Right, this.ClientRectangle.Bottom - this.itemHeight / 2), ToolStripDropDownDirection.Right);
				this.Refresh();
			}
		}
	
		/// <summary>
		/// An event handler called when the mouse releases the click on the control.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void MouseUpControl(object sender, MouseEventArgs e)
		{
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
			this.Refresh();
		}

		/// <summary>
		/// An event handler called when the control is resized.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void ControlResize(object sender, EventArgs e)
		{
			this.VisibleItems = this.visibleItems;
			this.Refresh();
		}

		/// <summary>
		/// An event handler called when the user clicks on a hidden item.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void HiddenItemClick(object sender, EventArgs e)
		{
			// Get the toolstrip item.
			ToolStripMenuItem menuItem = sender as ToolStripMenuItem;

			// Get the side menu item.
			SideMenuItem item = menuItem.Tag as SideMenuItem;

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

				//this.ClearControlMenuCheck(menuItem);

				//menuItem.Checked = true;
				this.Refresh();
			}
		}

		/// <summary>
		/// An event handler called when the user clicks on the show more button.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void ShowMoreButtonsClick(object sender, EventArgs e)
		{
			this.VisibleItems++;
		}

		/// <summary>
		/// An event handler called when the user clicks on the show fewer button.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void ShowFewerButtonsClick(object sender, EventArgs e)
		{
			this.VisibleItems--;
		}

		private void ControlMenuClosed(object sender, ToolStripDropDownClosedEventArgs e)
		{
			this.dockButtonPressed = false;
			this.dockButtonSelected = false;
			this.Refresh();
		}

		/// <summary>
		/// Returns the visible menu item at a specified global index.
		/// </summary>
		/// <param name="index">The global item index.</param>
		/// <returns>The side menu item or null, if the item at the specified index is not visible.</returns>
		private SideMenuItem GetVisibleItem(int index)
		{
			int? idx;
			return null == (idx = this.VisibleToGlobalIndex(index)) ? null : this.items[idx ?? -1];
		}

		/// <summary>
		/// Returns the minimized menu item at a specified global index.
		/// </summary>
		/// <param name="index">The global item index.</param>
		/// <returns>The side menu item or null, if the item at the specified index is not minimized.</returns>
		private SideMenuItem GetMinimizedItem(int index)
		{
			int? idx;
			return null == (idx = this.MinimizedToGlobalIndex(index)) ? null : this.items[idx ?? -1];
		}
	}
}
