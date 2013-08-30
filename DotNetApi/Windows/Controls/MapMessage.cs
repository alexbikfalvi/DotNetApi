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
	/// A class representing a map message.
	/// </summary>
	public sealed class MapMessage : Component
	{
		private MapControl parent;

		private string text;

		private Font font;
		private Shadow shadow;
		private Padding padding;

		private Size sizeBorder;
		private Rectangle rectangleText;
		private Rectangle rectangleBorder;
		private Rectangle rectanglePaint;

		private HorizontalAlign horizontalAlignment = HorizontalAlign.Center;
		

		/// <summary>
		/// Creates a new map message.
		/// </summary>
		/// <param name="text">The message.</param>
		/// <param name="anchor">The anchor rectangle.</param>
		/// <param name="horizontalAlignment">The message horizontal alignment with respect to the anchor.</param>
		/// <param name="verticalAlignment">The message vertical alignment with respect to the anchor.</param>
		/// <param name="font">The message font.</param>
		/// <param name="padding">The message padding.</param>
		/// <param name="shadow">The message shadow.</param>
		public MapMessage(string text, IAnchor anchor, HorizontalAlign horizontalAlignment, VerticalAlign verticalAlignment, Font font, Padding padding, Shadow shadow)
		{
			// Set the message properties.
			this.text = text;
			this.font = font;
			this.shadow = shadow;
			this.padding = padding;

			// Update the message geometry.
			//this.Update(anchor, horizontalAlignment, verticalAlignment);
		}

		// Public properties.

		/// <summary>
		/// Gets the text of the current map message.
		/// </summary>
		public string Text { get { return this.text; } }

		// Public methods.

		public void Update(IAnchor anchor, HorizontalAlign horizontalAlignment, VerticalAlign verticalAlignment)
		{
			// Compute the text size.
			Size sizeText = TextRenderer.MeasureText(this.text, this.font);
			// Compute the border size.
			this.sizeBorder = sizeText.Add(padding);
			// Compute the border rectangle.
			this.rectangleBorder = sizeBorder.Align(anchor.AnchorBounds, horizontalAlignment, verticalAlignment);
			// Compute the text rectangle.
			this.rectangleText = this.rectangleBorder.Add(padding.Left, padding.Top);
			// Compute the shadow rectangle.
			Rectangle shadowRectangle = shadow.GetShadowRectangle(this.rectangleBorder);
			// Compute the paint rectangle.
			this.rectanglePaint = this.rectangleBorder.Merge(shadowRectangle);
		}

		/// <summary>
		/// Draws the current message on the specified graphics object.
		/// </summary>
		/// <param name="graphics">The graphics object.</param>
		/// <param name="colorBorder">The border color.</param>
		/// <param name="colorFill">The fill color.</param>
		/// <param name="colorText">The text color.</param>
		public void Draw(Graphics graphics, Color colorBorder, Color colorFill, Color colorText)
		{
			// Use a normal smoothing mode.
			graphics.SmoothingMode = SmoothingMode.Default;

			// Create the pen.
			using (Pen pen = new Pen(colorBorder))
			{
				// Create the brush.
				using (SolidBrush brush = new SolidBrush(colorFill))
				{
					// Draw the shadow.
					graphics.DrawShadow(this.shadow, this.rectangleBorder);
					// Draw the rectangle.
					graphics.FillRectangle(brush, this.rectangleBorder);
					// Draw the border.
					graphics.DrawRectangle(pen, this.rectangleBorder);
				}
			}

			// Display a message.
			TextRenderer.DrawText(graphics, this.text, this.font, this.rectangleText, colorText, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
		}
	}
}
