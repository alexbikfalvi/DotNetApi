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
		private Rectangle textRectangle;
		private bool autoAlign = true;
		private Point[] polygon = new Point[7];

		/// <summary>
		/// Creates a new map message.
		/// </summary>
		/// <param name="text">The message.</param>
		/// <param name="boundary">The boundary object.</param>
		/// <param name="translation">The map translation.</param>
		public MapInfoAnnotation(string text, IAnchor boundary, ITranslation translation)
			: base(text, null, translation)
		{
			// Validate the parameters.
			boundary.ValidateNotNull("boundary");
			translation.ValidateNotNull("translation");
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
		/// An event handler called when drawing the annotation on the specified graphics object.
		/// </summary>
		/// <param name="graphics">The graphics object.</param>
		protected override void OnDraw(Graphics graphics)
		{
			// If the message is not visible, do nothing.
			if (!this.Visible) return;

			// Use a normal smoothing mode.
			graphics.SmoothingMode = SmoothingMode.HighQuality;

			// Create the pen.
			using (Pen pen = new Pen(this.BorderColor))
			{
				// Create the brush.
				using (SolidBrush brush = new SolidBrush(this.BackgroundColor))
				{
					// Draw the shadow.
					graphics.DrawShadow(this.Shadow, this.textRectangle);
					// Draw the polygon.
					graphics.FillPolygon(brush, this.polygon);
					// Draw the border.
					graphics.DrawPolygon(pen, this.polygon);
				}
			}

			// Display a message.
			TextRenderer.DrawText(graphics, this.Text, this.Font, this.textRectangle, this.TextColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
		}

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
				// Compute the anchor bounds.
				Rectangle anchorBounds = null != this.Translation ? this.Anchor.AnchorBounds.Add(this.Translation.TranslationDelta) : this.Anchor.AnchorBounds;
				
				// Compute the alignment to the top.
				if (this.boundary.AnchorBounds.Contains(this.BorderRectangle = this.BorderSize.AddHeight(this.tailSize.Height).Align(anchorBounds, HorizontalAlign.Center, VerticalAlign.TopOutside)))
				{
					// Compute the polygon.
					int bottom = this.BorderRectangle.Bottom - this.tailSize.Height;
					int middle = this.BorderRectangle.Middle().X;
					int tail = this.tailSize.Width >> 1;
					this.polygon[0] = new Point(this.BorderRectangle.Left, this.BorderRectangle.Top);
					this.polygon[1] = new Point(this.BorderRectangle.Right, this.BorderRectangle.Top);
					this.polygon[2] = new Point(this.BorderRectangle.Right, bottom);
					this.polygon[3] = new Point(middle + tail, bottom);
					this.polygon[4] = new Point(middle, this.BorderRectangle.Bottom);
					this.polygon[5] = new Point(middle - tail, bottom);
					this.polygon[6] = new Point(this.BorderRectangle.Left, bottom);
					// Compute the text rectangle.
					this.textRectangle = new Rectangle(this.BorderRectangle.Location, this.BorderSize);
				}
				// Compute the alignment to the bottom.
				else if (this.boundary.AnchorBounds.Contains(this.BorderRectangle = this.BorderSize.AddHeight(this.tailSize.Height).Align(anchorBounds, HorizontalAlign.Center, VerticalAlign.BottomOutside)))
				{
					// Compute the polygon.
					int top = this.BorderRectangle.Top + this.tailSize.Height;
					int middle = this.BorderRectangle.Middle().X;
					int tail = this.tailSize.Width >> 1;
					this.polygon[0] = new Point(this.BorderRectangle.Left, top);
					this.polygon[1] = new Point(middle - tail, top);
					this.polygon[2] = new Point(middle, this.BorderRectangle.Top);
					this.polygon[3] = new Point(middle + tail, top);
					this.polygon[4] = new Point(this.BorderRectangle.Right, top);
					this.polygon[5] = new Point(this.BorderRectangle.Right, this.BorderRectangle.Bottom);
					this.polygon[6] = new Point(this.BorderRectangle.Left, this.BorderRectangle.Bottom);
					// Compute the text rectangle.
					this.textRectangle = new Rectangle(this.BorderRectangle.Left, top, this.BorderSize.Width, this.BorderSize.Height);
				}
				// Compute the alignment to the left.
				else if (this.boundary.AnchorBounds.Contains(this.BorderRectangle = this.BorderSize.AddWidth(this.tailSize.Width).Align(anchorBounds, HorizontalAlign.LeftOutside, VerticalAlign.Center)))
				{
					// Compute the polygon.
					int right = this.BorderRectangle.Right - this.tailSize.Width;
					int middle = this.BorderRectangle.Middle().Y;
					int tail = this.tailSize.Height >> 1;
					this.polygon[0] = new Point(this.BorderRectangle.Left, this.BorderRectangle.Top);
					this.polygon[1] = new Point(right, this.BorderRectangle.Top);
					this.polygon[2] = new Point(right, middle - tail);
					this.polygon[3] = new Point(this.BorderRectangle.Right, middle);
					this.polygon[4] = new Point(right, middle + tail);
					this.polygon[5] = new Point(right, this.BorderRectangle.Bottom);
					this.polygon[6] = new Point(this.BorderRectangle.Left, this.BorderRectangle.Bottom);
					// Compute the text rectangle.
					this.textRectangle = new Rectangle(this.BorderRectangle.Location, this.BorderSize);
				}
				// Compute the alignment to the right.
				else if (this.boundary.AnchorBounds.Contains(this.BorderRectangle = this.BorderSize.AddWidth(this.tailSize.Width).Align(anchorBounds, HorizontalAlign.RightOutside, VerticalAlign.Center)))
				{
					// Compute the polygon.
					int left = this.BorderRectangle.Left + this.tailSize.Width;
					int middle = this.BorderRectangle.Middle().Y;
					int tail = this.tailSize.Height >> 1;
					this.polygon[0] = new Point(left, this.BorderRectangle.Top);
					this.polygon[1] = new Point(this.BorderRectangle.Right, this.BorderRectangle.Top);
					this.polygon[2] = new Point(this.BorderRectangle.Right, this.BorderRectangle.Bottom);
					this.polygon[3] = new Point(left, this.BorderRectangle.Bottom);
					this.polygon[4] = new Point(left, middle + tail);
					this.polygon[5] = new Point(this.BorderRectangle.Left, middle);
					this.polygon[6] = new Point(left, middle - tail);
					// Compute the text rectangle.
					this.textRectangle = new Rectangle(left, this.BorderRectangle.Top, this.BorderSize.Width, this.BorderSize.Height);
				}
				else
				{
					// Compute the alignment to the center.
					this.BorderRectangle = this.BorderSize.AddHeight(this.tailSize.Height).Align(anchorBounds, HorizontalAlign.Center, VerticalAlign.Center).Subtract(0, this.tailSize.Height + (this.BorderSize.Height >> 1) + 1);
					// Compute the polygon.
					int bottom = this.BorderRectangle.Bottom - this.tailSize.Height;
					int middle = this.BorderRectangle.Middle().X;
					int tail = this.tailSize.Width >> 1;
					this.polygon[0] = new Point(this.BorderRectangle.Left, this.BorderRectangle.Top);
					this.polygon[1] = new Point(this.BorderRectangle.Right, this.BorderRectangle.Top);
					this.polygon[2] = new Point(this.BorderRectangle.Right, bottom);
					this.polygon[3] = new Point(middle + tail, bottom);
					this.polygon[4] = new Point(middle, this.BorderRectangle.Bottom);
					this.polygon[5] = new Point(middle - tail, bottom);
					this.polygon[6] = new Point(this.BorderRectangle.Left, bottom);
					// Compute the text rectangle.
					this.textRectangle = new Rectangle(this.BorderRectangle.Location, this.BorderSize);
				}
			}
			else
			{
				// Compute the anchor bounds.
				Rectangle anchorBounds = null != this.Anchor ? null != this.Translation ? this.Anchor.AnchorBounds.Add(this.Translation.TranslationDelta) : this.Anchor.AnchorBounds : default(Rectangle);
				// Compute the alignment.
				this.BorderRectangle = this.BorderSize.AddHeight(this.tailSize.Height).Align(anchorBounds, this.HorizontalAlignment, this.VerticalAlignment);
				// Compute the polygon.
				int bottom = this.BorderRectangle.Bottom - this.tailSize.Height;
				int middle = this.BorderRectangle.Middle().X;
				int tail = this.tailSize.Width >> 1;
				this.polygon[0] = new Point(this.BorderRectangle.Left, this.BorderRectangle.Top);
				this.polygon[1] = new Point(this.BorderRectangle.Right, this.BorderRectangle.Top);
				this.polygon[2] = new Point(this.BorderRectangle.Right, bottom);
				this.polygon[3] = new Point(middle + tail, bottom);
				this.polygon[4] = new Point(middle, this.BorderRectangle.Bottom);
				this.polygon[5] = new Point(middle - tail, bottom);
				this.polygon[6] = new Point(this.BorderRectangle.Left, bottom);
				// Compute the text rectangle.
				this.textRectangle = new Rectangle(this.BorderRectangle.Location, this.BorderSize);
			}
			// Compute the shadow rectangle.
			Rectangle shadowRectangle = this.Shadow.GetShadowRectangle(this.BorderRectangle);
			// Compute the paint rectangle.
			this.Bounds = this.BorderRectangle.Merge(shadowRectangle);
		}
	}
}
