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
using System.Windows.Forms;
using DotNetApi.Drawing;
using MapApi;

namespace DotNetApi.Windows.Controls
{
	/// <summary>
	/// A control that displays a geographic map.
	/// </summary>
	public sealed class MapControl : ThreadSafeControl
	{
		private const string messageNoMap = "No map";
		private const string messageRefreshing = "Refreshing map...";

		// The current map.

		private Map map = null;

		// Colors.

		private Color colorMessageBorder = Color.DarkGray;
		private Color colorMessageFill = Color.LightGray;

		// Bitmaps.

		private Bitmap bitmapBackground = new Bitmap(20, 20);
		private Bitmap bitmap;

		private TextureBrush brushBackground;

		private Shadow shadow = new Shadow(Color.Black, 0, 10);

		// Message.

		private string message = MapControl.messageNoMap;
		private Font messageFont = Window.DefaultFont;
		private Padding messagePadding = new Padding(4);
		private bool messageVisible = true;

		/// <summary>
		/// Creates a new control instance.
		/// </summary>
		public MapControl()
		{
			// Set the default properties.
			this.DoubleBuffered = true;

			// Create the background bitmap.
			using (Graphics graphics = Graphics.FromImage(this.bitmapBackground))
			{
				using (SolidBrush brush = new SolidBrush(Color.LightGray))
				{
					// Draw the gray checker board.
					graphics.FillRectangle(brush, 0, 0, 10, 10);
					graphics.FillRectangle(brush, 10, 10, 20, 20);
					// Draw the white checker board.
					brush.Color = Color.White;
					graphics.FillRectangle(brush, 10, 0, 20, 10);
					graphics.FillRectangle(brush, 0, 10, 10, 20);
				}
			}

			// Create the background brush.
			this.brushBackground = new TextureBrush(this.bitmapBackground);
		}

		// Public properties.

		/// <summary>
		/// Gets or sets the current message.
		/// </summary>
		[DefaultValue(MapControl.messageNoMap)]
		public string Message
		{
			get { return this.message; }
			set { this.OnMessageChanged(value); }
		}

		/// <summary>
		/// Gets or sets whether the message is visible.
		/// </summary>
		[DefaultValue(true)]
		public bool MessageVisible
		{
			get { return this.messageVisible; }
			set { this.OnMessageVisibleChanged(value); }
		}

		/// <summary>
		/// Gets or sets the current map.
		/// </summary>
		[DefaultValue(null)]
		public Map Map
		{
			get { return this.map; }
			set { this.OnMapChanged(value); }
		}

		// Protected methods.

		/// <summary>
		/// Disposes the control.
		/// </summary>
		/// <param name="disposing"><b>True</b> if the object is being disposed.</param>
		protected override void Dispose(bool disposing)
		{
			// Call the base class dispose method.
			base.Dispose(disposing);
			// If the object is being disposed.
			if (disposing)
			{
				// Dispose the brushes.
				this.brushBackground.Dispose();

				// Dispose the bitmaps.
				if (this.bitmap != null) this.bitmap.Dispose();
				this.bitmapBackground.Dispose();

				// Dispose the shadow.
				this.shadow.Dispose();
			}
		}

		/// <summary>
		/// An event handler called when the control is being repainted.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			// If the object has been disposed, do nothing.
			if (this.IsDisposed) return;

			// If the current map bitmap is null.
			if (null == this.bitmap)
			{
				// Draw the checkerboard background.
				e.Graphics.FillRectangle(this.brushBackground, this.ClientRectangle);
			}
			else
			{
				// Draw the map bitmap.
				//e.Graphics.Fil
			}


			// If the current message is not null or empty and the message is visible.
			if (!string.IsNullOrEmpty(this.message) && this.messageVisible)
			{
				// Draw the message.
				this.OnDrawMessage(e.Graphics);
			}

			// Call the base class event handler.
			base.OnPaint(e);
		}

		/// <summary>
		/// An event handler called when the control is being resized.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnResize(EventArgs e)
		{
			// Call the base class event handler.
			base.OnResize(e);
			// Refresh the control.
			this.Refresh();
		}

		// Private methods.

		/// <summary>
		/// Sets the specified message on the control.
		/// </summary>
		/// <param name="message">The new message.</param>
		private void OnMessageChanged(string message)
		{
			// Get the shadow rectangle for the old message.
			Rectangle rectangleOld = this.shadow.GetShadowRectangle(this.MeasureMessage(this.message));
			// Get the shadow rectangle for the new message.
			Rectangle rectangleNew = this.shadow.GetShadowRectangle(this.MeasureMessage(message));
			// Set the new message.
			this.message = message;
			// Invalidate the maximum region between the two rectangles.
			this.Invalidate(Geometry.Merge(rectangleOld, rectangleNew));
		}

		/// <summary>
		/// Set the message visibility.
		/// </summary>
		/// <param name="visible"><b>True</b> if the message is visible or <b>false</b> otherwise.</param>
		private void OnMessageVisibleChanged(bool visible)
		{
			// Get the shadow rectangle for the current message.
			Rectangle rectangle = this.shadow.GetShadowRectangle(this.MeasureMessage(this.message));
			// Set the new message visibility.
			this.messageVisible = visible;
			// Invalidate the maximum region between the two rectangles.
			this.Invalidate(rectangle);
		}

		/// <summary>
		/// Sets the current map.
		/// </summary>
		/// <param name="map">The map.</param>
		private void OnMapChanged(Map map)
		{
			// If the map has not changed, do nothing.
			if (this.map == map) return;

			// Set the current map.
			this.map = map;
			
			// If the curernt bitmap is not null, dispose the current bitmap.
			if (null != this.bitmap)
			{
				this.bitmap.Dispose();
				this.bitmap = null;
			}

			// Set a new message.
			this.message = MapControl.messageRefreshing;

			// Refresh the control.
			this.Refresh();
		}

		/// <summary>
		/// Draws the current message on the control.
		/// </summary>
		/// <param name="graphics">The graphics object.</param>
		private void OnDrawMessage(Graphics graphics)
		{
			// Create the border rectangle.
			Rectangle rectangleBorder = this.MeasureMessage(this.message);
			// Create the fill rectangle.
			Rectangle rectangleFill = new Rectangle(
				rectangleBorder.X + 1,
				rectangleBorder.Y + 1,
				rectangleBorder.Width - 1,
				rectangleBorder.Height - 1);

			// Create the pen.
			using (Pen pen = new Pen(this.colorMessageBorder))
			{
				// Create the brush.
				using (SolidBrush brush = new SolidBrush(this.colorMessageFill))
				{
					// Draw the shadow.
					graphics.DrawShadow(this.shadow, rectangleBorder);
					// Draw the border.
					graphics.DrawRectangle(pen, rectangleBorder);
					// Draw the rectangle.
					graphics.FillRectangle(brush, rectangleFill);
				}
			}
			
			// Display a message.
			TextRenderer.DrawText(graphics, this.message, Window.DefaultFont, rectangleBorder, Color.Black, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
		}

		/// <summary>
		/// Returns the drawing rectangle for the specified message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <returns>The drawing rectangle.</returns>
		private Rectangle MeasureMessage(string message)
		{
			// Measure the message text.
			Size size = TextRenderer.MeasureText(message, this.messageFont);

			// Return the message rectangle.
			return new Rectangle(
				this.ClientRectangle.X + (this.ClientRectangle.Width >> 1) - (size.Width >> 1) - this.messagePadding.Left,
				this.ClientRectangle.Y + (this.ClientRectangle.Height >> 1) - (size.Height >> 1) - this.messagePadding.Top,
				size.Width + this.messagePadding.Left + this.messagePadding.Right,
				size.Height + this.messagePadding.Top + this.messagePadding.Bottom);
		}
	}
}
