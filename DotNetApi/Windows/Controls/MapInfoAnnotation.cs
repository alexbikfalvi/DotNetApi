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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DotNetApi.Drawing;

namespace DotNetApi.Windows.Controls
{
	/// <summary>
	/// A class representing a map information annotation.
	/// </summary>
	public class MapInfoAnnotation : MapTextAnnotation
	{
		private IAnchor boundary;
		private Size tailSize = new Size(10, 10);
		private bool autoAlign = true;
		private Point[] polygon = new Point[7];

		/// <summary>
		/// Creates a new map message.
		/// </summary>
		/// <param name="text">The message.</param>
		/// <param name="boundary">The boundary object.</param>
		public MapInfoAnnotation(string text, IAnchor boundary)
			: base(text, null)
		{
			// Validate the parameters.
			boundary.ValidateNotNull("boundary");
			// Set the annotation defaults.
			this.BackgroundColor = Color.White;
			this.Visible = false;
			// Set the boundary anchor.
			this.boundary = boundary;
		}

		// Public properties.

		/// <summary>
		/// Gets or sets whether the annotation is aligned automatically.
		/// </summary>
		public bool AutoAlign
		{
			get { return this.autoAlign; }
			set { this.autoAlign = value; }
		}

		// Protected methods.

		/// <summary>
		/// An event handler called when measuring the annotation bounds.
		/// </summary>
		protected override void OnMeasureBounds()
		{
			// If the layout is suspended, do nothing.
			if (this.LayoutSuspeded) return;
			// If the annotation is aligned automatically and the anchor is not null.
			if (this.autoAlign && (null != this.Anchor))
			{
				if (this.boundary.AnchorBounds.Top + this.BorderSize.Height + this.tailSize.Height <= this.Anchor.AnchorBounds.Top)
				{
					// Compute the alignment to the top.
				}
				else if (this.boundary.AnchorBounds.Bottom - this.BorderSize.Height - this.tailSize.Height >= this.Anchor.AnchorBounds.Bottom)
				{
					// Compute the alignment to the bottom.
				}
				else if (this.boundary.AnchorBounds.Left + this.BorderSize.Width + this.tailSize.Width <= this.Anchor.AnchorBounds.Left)
				{
					// Compute the alignment to the left.
				}
				else if (this.boundary.AnchorBounds.Right - this.BorderSize.Width - this.tailSize.Width >= this.Anchor.AnchorBounds.Right)
				{
					// Compute the alignment to the right.
				}
				else
				{
					// Compute the alignment to the center
				}
			}
			else
			{
			}
			// Compute the border rectangle.
			this.BorderRectangle = this.BorderSize.Align(null != this.Anchor ? this.Anchor.AnchorBounds : default(Rectangle), this.HorizontalAlignment, this.VerticalAlignment);
			// Compute the shadow rectangle.
			Rectangle shadowRectangle = this.Shadow.GetShadowRectangle(this.BorderRectangle);
			// Compute the paint rectangle.
			this.Bounds = this.BorderRectangle.Merge(shadowRectangle);
		}
	}
}
