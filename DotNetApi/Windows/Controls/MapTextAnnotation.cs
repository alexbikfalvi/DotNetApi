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
	/// A class representing a map text.
	/// </summary>
	public class MapTextAnnotation : MapAnnotation
	{
		private string text;

		private Font font = Window.DefaultFont;
		private Padding padding = new Padding(4);

		private Color borderColor = Color.DarkGray;
		private Color backgroundColor = Color.LightGray;
		private Color textColor = Color.Black;

		private Shadow shadow = new Shadow(Color.Black, 0, 14);

		/// <summary>
		/// Creates a new map message.
		/// </summary>
		/// <param name="text">The message.</param>
		/// <param name="anchor">The anchor object.</param>
		public MapTextAnnotation(string text, IAnchor anchor)
			: base(anchor)
		{
			// Set the message properties.
			this.text = text;
			// Update the text measurements.
			this.OnMeasureText();
			// Update the bounds measurements.
			this.OnMeasureBounds();
		}

		// Public properties.

		/// <summary>
		/// Gets or sets the text of the map message.
		/// </summary>
		public string Text
		{
			get { return this.text; }
			set { this.OnTextChanged(value); }
		}
		/// <summary>
		/// Gets or sets the annotation padding.
		/// </summary>
		public Padding Padding
		{
			get { return this.padding; }
			set { this.OnPaddingChanged(value); }
		}
		/// <summary>
		/// Gets or sets the border color.
		/// </summary>
		public Color BorderColor
		{
			get { return this.borderColor; }
			set { this.borderColor = value; }
		}
		/// <summary>
		/// Gets or sets the background color.
		/// </summary>
		public Color BackgroundColor
		{
			get { return this.backgroundColor; }
			set { this.backgroundColor = value; }
		}
		/// <summary>
		/// Gets or sets the text color.
		/// </summary>
		public Color TextColor
		{
			get { return this.textColor; }
			set { this.textColor = value; }
		}
		/// <summary>
		/// Gets the annotation shadow.
		/// </summary>
		public Shadow Shadow
		{
			get { return this.shadow; }
		}

		// Protected properties.

		/// <summary>
		/// Gets or sets the border size.
		/// </summary>
		protected Size BorderSize { get; set; }
		/// <summary>
		/// Gets or sets the border rectangle
		/// </summary>
		protected Rectangle BorderRectangle { get; set; }

		// Protected methods.

		/// <summary>
		/// An event handler called when the object is being disposed.
		/// </summary>
		/// <param name="disposed">If <b>true</b>, clean both managed and native resources. If <b>false</b>, clean only native resources.</param>
		protected override void Dispose(bool disposing)
		{
			// Call the base class method.
			base.Dispose(disposing);
			// Dispose the shadow.
			this.shadow.Dispose();
		}

		/// <summary>
		/// An event handler called when drawing the annotation on the specified graphics object.
		/// </summary>
		/// <param name="graphics">The graphics object.</param>
		protected override void OnDraw(Graphics graphics)
		{
			// Call the base class method.
			base.OnDraw(graphics);
			
			// If the message is not visible, do nothing.
			if (!this.Visible) return;

			// Use a normal smoothing mode.
			graphics.SmoothingMode = SmoothingMode.Default;

			// Create the pen.
			using (Pen pen = new Pen(this.borderColor))
			{
				// Create the brush.
				using (SolidBrush brush = new SolidBrush(this.backgroundColor))
				{
					// Draw the shadow.
					graphics.DrawShadow(this.shadow, this.BorderRectangle);
					// Draw the rectangle.
					graphics.FillRectangle(brush, this.BorderRectangle);
					// Draw the border.
					graphics.DrawRectangle(pen, this.BorderRectangle);
				}
			}

			// Display a message.
			TextRenderer.DrawText(graphics, this.text, this.font, this.BorderRectangle, this.textColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
		}

		/// <summary>
		/// Refreshes the map annotation.
		/// </summary>
		protected override void OnRefresh()
		{
			// Call the base class methods.
			base.OnRefresh();
			// Update the text measurements.
			this.OnMeasureText();
			// Update the bounds measurements.
			this.OnMeasureBounds();
		}

		/// <summary>
		/// Measures the bounds of the map message.
		/// </summary>
		protected override void OnMeasureBounds()
		{
			// If the layout is suspended, do nothing.
			if (this.LayoutSuspeded) return;
			// Compute the border rectangle.
			this.BorderRectangle = this.BorderSize.Align(null != this.Anchor ? this.Anchor.AnchorBounds : default(Rectangle), this.HorizontalAlignment, this.VerticalAlignment);
			// Compute the shadow rectangle.
			Rectangle shadowRectangle = this.shadow.GetShadowRectangle(this.BorderRectangle);
			// Compute the paint rectangle.
			this.Bounds = this.BorderRectangle.Merge(shadowRectangle);
		}

		/// <summary>
		/// Measures the size of the message text.
		/// </summary>
		protected virtual void OnMeasureText()
		{
			// If the layout is suspended, do nothing.
			if (this.LayoutSuspeded) return;
			// Compute the text size.
			Size sizeText = TextRenderer.MeasureText(this.text, this.font);
			// Compute the border size.
			this.BorderSize = sizeText.Add(padding);
		}

		/// <summary>
		/// An event handler called when the annotation text has changed.
		/// </summary>
		/// <param name="text">The map message text.</param>
		protected virtual void OnTextChanged(string text)
		{
			// Set the text.
			this.text = text;
			// Update the text measurements.
			this.OnMeasureText();
			// Update the bounds measurements.
			this.OnMeasureBounds();
		}

		/// <summary>
		/// An event handler called when the padding has changed.
		/// </summary>
		/// <param name="padding">The padding.</param>
		protected virtual void OnPaddingChanged(Padding padding)
		{
			// Set the padding.
			this.padding = padding;
			// Update the text measurements.
			this.OnMeasureText();
			// Update the bounds measurements.
			this.OnMeasureBounds();
		}
	}
}
