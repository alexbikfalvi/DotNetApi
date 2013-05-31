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
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DotNetApi.Windows.Controls
{
	public delegate void ImageListBoxItemActivateEventHandler(object sender, ImageListBoxItem item);

	/// <summary>
	/// An image list box control.
	/// </summary>
	public class ImageListBox : ListBox
	{
		private int imageWidth = 64;

		/// <summary>
		/// Creates a new image list box instance.
		/// </summary>
		public ImageListBox()
		{
			// Set the object properties.
			this.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.ItemHeight = 48;
			this.IntegralHeight = false;
		}

		// Public properties.

		/// <summary>
		/// Gets or sets the image width.
		/// </summary>
		public int ImageWidth
		{
			get { return this.imageWidth; }
			set { this.imageWidth = value; this.Invalidate(); }
		}

		// Public events.

		/// <summary>
		/// An event raised when the user activates an item.
		/// </summary>
		public event ImageListBoxItemActivateEventHandler ItemActivate;

		// Public methods.

		/// <summary>
		/// Adds a new item to the image list box.
		/// </summary>
		/// <param name="text">The item text.</param>
		/// <param name="image">The item image.</param>
		public void AddItem(string text, Image image)
		{
			// Create a new image list box item.
			ImageListBoxItem item = new ImageListBoxItem(text, image);

			// Add the item to the list box.
			this.Items.Add(item);
		}

		// Protected methods.

		/// <summary>
		/// Draws the image list box item.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			// Call the base class method.
			base.OnDrawItem(e);

			// If the index is outside the item count, do nothing.
			if ((e.Index < 0) || (e.Index >= this.Items.Count)) return;

			// Get the image list box item.
			ImageListBoxItem item = this.Items[e.Index] as ImageListBoxItem;

			Rectangle rectText = new Rectangle(
				e.Bounds.Left + this.ImageWidth + 5,
				e.Bounds.Top,
				e.Bounds.Width - this.ImageWidth - 5,
				e.Bounds.Height);

			Rectangle rectImage = new Rectangle(
				e.Bounds.Left,
				e.Bounds.Top,
				this.imageWidth,
				e.Bounds.Height);

			using (Brush brush = new SolidBrush(e.BackColor))
			{
				// Draw a background rectangle.
				e.Graphics.FillRectangle(brush, e.Bounds);
			}

			// Draw the text
			if (null != item.Text)
			{
				TextRenderer.DrawText(
					e.Graphics,
					item.Text,
					this.Font,
					rectText,
					e.ForeColor,
					TextFormatFlags.EndEllipsis | TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
			}

			// Draw the image.
			if (null != item.Image)
			{
				e.Graphics.DrawImage(item.Image, rectImage);
			}
		}

		/// <summary>
		/// An event handler called when the user double click on an item.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnDoubleClick(EventArgs e)
		{
			// Call the base class method.
			base.OnDoubleClick(e);
			// Activate the selected item.
			if ((this.SelectedItem != null) && (this.ItemActivate != null))
				this.ItemActivate(this, this.SelectedItem as ImageListBoxItem);
		}
	}
}
