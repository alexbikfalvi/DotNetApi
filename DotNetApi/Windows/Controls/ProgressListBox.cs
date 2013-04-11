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
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace DotNetApi.Windows.Controls
{
	//public delegate void ImageListBoxItemActivateEventHandler(object sender, ImageListBoxItem item);

	/// <summary>
	/// An progress list box control.
	/// </summary>
	public class ProgressListBox : ListBox
	{
		private ProgressListBoxItem.Collection items = new ProgressListBoxItem.Collection();

		/// <summary>
		/// Creates a new progress list box instance.
		/// </summary>
		public ProgressListBox()
		{
			// Set control properties.
			this.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.ItemHeight = 48;
			this.IntegralHeight = false;

			// Set collection event handlers.
			this.items.AfterCleared += this.OnItemsCleared;
			this.items.AfterItemInserted += this.OnItemInserted;
			this.items.AfterItemRemoved += this.OnItemRemoved;
			this.items.AfterItemSet += this.OnItemSet;
		}

		/// <summary>
		/// An event raised when the user activates an item.
		/// </summary>
		//public event ImageListBoxItemActivateEventHandler ItemActivate;

		// Public properties.

		/// <summary>
		/// Gets the collection of list box items.
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
		public new ProgressListBoxItem.Collection Items { get { return this.items; } }

		/// <summary>
		/// Adds a new item to the image list box.
		/// </summary>
		/// <param name="text">The item text.</param>
		/// <param name="image">The item image.</param>
		//public void AddItem(string text, Image image)
		//{
		//	// Create a new image list box item.
		//	ImageListBoxItem item = new ImageListBoxItem(text, image);

		//	// Add the item to the list box.
		//	this.Items.Add(item);
		//}

		private void OnItemsCleared()
		{
			base.Items.Clear();
		}

		private void OnItemInserted(int index, ProgressListBoxItem item)
		{
			base.Items.Insert(index, item);
		}

		private void OnItemRemoved(int index, ProgressListBoxItem item)
		{
			base.Items.RemoveAt(index);
		}

		private void OnItemSet(int index, ProgressListBoxItem oldItem, ProgressListBoxItem newItem)
		{
			base.Items[index] = newItem;
		}

		/// <summary>
		/// Draws the image list box item.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			base.OnDrawItem(e);

			if ((e.Index < 0) || (e.Index >= this.Items.Count)) return;

			// Get the image list box item.
			ProgressListBoxItem item = this.Items[e.Index] as ProgressListBoxItem;

			Rectangle rectText = new Rectangle(
				e.Bounds.Left + 5,
				e.Bounds.Top,
				e.Bounds.Width - 5,
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
		}
	}
}
