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

namespace DotNetApi.Windows.Controls
{
	/// <summary>
	/// An image list box item.
	/// </summary>
	[Serializable]
	public class ImageListBoxItem
	{
		private string text;
		private Image image;

		/// <summary>
		/// Creates a new image list box item.
		/// </summary>
		/// <param name="text">The item text.</param>
		/// <param name="image">The item image.</param>
		public ImageListBoxItem(string text, Image image)
		{
			this.text = text;
			this.image = image;
		}

		/// <summary>
		/// Gets or sets the item text.
		/// </summary>
		public string Text
		{
			get { return this.text; }
			set { this.text = value; }
		}

		/// <summary>
		/// Gets or sets the item image.
		/// </summary>
		public Image Image
		{
			get { return this.image; }
			set { this.image = value; }
		}
	}
}
