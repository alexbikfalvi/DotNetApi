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
using System.Drawing;
using System.Drawing.Imaging;
using DotNetApi.Drawing.Imaging;

namespace DotNetApi.Drawing
{
	/// <summary>
	/// A shadow effect.
	/// </summary>
	public sealed class Shadow : Effect
	{
		private Color color;
		private int distance;
		private int softness;
		private double angle;

		private Bitmap bitmap;
		private Polygon polygon = null;
		private Point offset;
		private Size size;

		private Kernel1D blur;

		/// <summary>
		/// Creates a new shadow of the specified color and size.
		/// </summary>
		/// <param name="color">The shadow color.</param>
		/// <param name="distance">The shadow distance.</param>
		/// <param name="softness">The shadow softness.</param>
		/// <param name="opacity">The shadow opacity (<b>0</b> is fully transparent, and <b>1</b> is fully opaque).</param>
		/// <param name="angle">The shadow angle in radians. The default angle is minus 45 degrees (minus pi/2).</param>
		public Shadow(Color color, int distance, int softness, double opacity = 1.0, double angle = -Math.PI/2)
		{
			// Check arguments.
			if ((opacity < 0.0) || (opacity > 1.0)) throw new ArgumentException("The opacity parameter must be in the interval [0, 1].", "opacity");

			// Set the effect properties.
			this.color = Color.FromArgb((int)(opacity * 255), color);
			this.distance = distance;
			this.softness = softness;
			this.angle = angle;
			// Compute the shadow offset.
			this.offset = new Point(
				(int)(this.distance * Math.Cos(this.angle)),
				(int)(this.distance * Math.Sin(this.angle)));
			// Create the blur kernel.
			this.blur = new Kernel1DGauss(softness % 2 == 0 ? softness + 1 : softness);
		}

		// Public methods.

		/// <summary>
		/// Returns the shadow rectange for the given object rectangle.
		/// </summary>
		/// <param name="rectangle">The rectangle of the foreground object.</param>
		/// <returns>The shadow rectangle.</returns>
		public Rectangle GetShadowRectangle(Rectangle rectangle)
		{
			return new Rectangle(
				rectangle.Location.Subtract(this.softness >> 1).Add(this.offset),
				rectangle.Size.Add(this.softness + 1));
		}

		// Internal methods.

		/// <summary>
		/// Draws a shadow for the specified rectangle.
		/// </summary>
		/// <param name="graphics">The graphics object.</param>
		/// <param name="rectangle">The shadow rectangle.</param>
		internal override void Draw(Graphics graphics, Rectangle rectangle)
		{
			// If the object is disposed, do nothing.
			if (this.IsDisposed) return;

			// Create a new polygon for this rectangle.
			Polygon polygon = new Polygon(rectangle);

			// Draw the polygon.
			this.Draw(graphics, polygon);
		}

		/// <summary>
		/// Draws a shadow for the specified polygon.
		/// </summary>
		/// <param name="graphics">The graphics object.</param>
		/// <param name="points">The polygon points.</param>
		internal override void Draw(Graphics graphics, Point[] points)
		{
			// If the object is disposed, do nothing.
			if (this.IsDisposed) return;

			// Create a new polygon for this set of points.
			Polygon polygon = new Polygon(points);

			// Draw the polygon.
			this.Draw(graphics, polygon);
		}

		// Protected methods.

		/// <summary>
		/// An event handler called when the object is being disposed.
		/// </summary>
		/// <param name="disposed">If <b>true</b>, clean both managed and native resources. If <b>false</b>, clean only native resources.</param>
		protected override void Dispose(bool disposed)
		{
			if (disposed)
			{
				// Dispose current objects.
				if (null != this.bitmap) this.bitmap.Dispose();
			}
			// Call the base class event handler.
			base.Dispose(disposed);
		}

		// Private methods.

		/// <summary>
		/// Draws a shadow centered on the specified polygon.
		/// </summary>
		/// <param name="graphics">The graphics object.</param>
		/// <param name="points">The polygon points.</param>
		private void Draw(Graphics graphics, Polygon polygon)
		{
			// If the object is disposed, do nothing.
			if (this.IsDisposed) return;

			// If the current shadow bitmap is null or if the current poligon is different from the new polygon.
			if ((null == this.bitmap) || (this.polygon != null ? !this.polygon.IsEqual(polygon) : false))
			{
				// Create a new shadow bitmap.
				this.CreateShadow(polygon);
			}
			
			// Compute the shadow location by subtracting half the softness and adding the shadow offset.
			Point location = polygon.Location.Subtract(this.softness >> 1).Add(this.offset);

			// Draw the shadow bitmap.
			graphics.DrawImage(this.bitmap, new Rectangle(location, this.size));
		}

		/// <summary>
		/// Creates a shadow bitmap for the specified polygon.
		/// </summary>
		private void CreateShadow(Polygon polygon)
		{
			// If there exists a current shadow bitmap, dispose the bitmap.
			if (null != this.bitmap)
			{
				this.bitmap.Dispose();
			}

			// Set the polygon as the current polygon.
			this.polygon = polygon;
			// Compute the shadow size as the current polygon size plus the softness.
			this.size = this.polygon.Size.Add(this.softness + 1);
			// Compute the bitmap rectangle.
			Rectangle rectangle = new Rectangle(new Point(0, 0), this.size);
			// Create the shadow bitmap.
			this.bitmap = new Bitmap(this.size.Width, this.size.Height, PixelFormat.Format32bppArgb);

			// Lock the bitmap data.
			BitmapData data = this.bitmap.LockBits(new Rectangle(0, 0, this.size.Width, this.size.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

			try
			{
				// Enter an unsafe region.
				unsafe
				{
					// Get a 32-bit pointer to the bitmap data.
					uint* image = (uint*)data.Scan0;
					// Get the color.
					uint color = ((uint)this.color.A) << 24 | ((uint)this.color.R) << 16 | ((uint)this.color.G << 8) | ((uint)this.color.B);
					uint alpha = color >> 24;
					uint rgb = color & 0x00FFFFFF;
					uint value;
					int offset = this.blur.Size >> 1;
					// Perform a horizontal blur.
					{
						// The column index.
						int i = 0;
						// Set the sum of the gaussian blur.
						double sum = 0.0;
						// Compute the margin of the beginning and end of the gaussian blur.
						int margin = data.Width > (blur.Size << 1) ? blur.Size : (int)Math.Ceiling(data.Width / 2.0);
						// The beginning of the horizontal blur.
						for (; i < margin; i++)
						{
							// Increment the blur sum for each column.
							sum += this.blur[i - offset];
							// Compute the alpha channel for the specified pixel.
							value = ((uint)Math.Ceiling(alpha * sum)) & 0xFF;
							// Set the column blur.
							for (int j = 0; j < data.Height; j++)
							{
								image[j * data.Width + i] = value;
							}
						}
						// The middle of the horizontal blur, all rows and columns have the default value alpha.
						for (; i < data.Width - margin; i++)
						{
							for (int j = 0; j < data.Height; j++)
							{
								image[j * data.Width + i] = alpha;
							}
						}
						// The end of the horizontal blur.
						for (int k = 0; i < data.Width; i++, k++)
						{
							// Compute the alpha channel for the specified pixel.
							value = ((uint)Math.Ceiling(alpha * sum)) & 0xFF;
							// Decrement the blur sum for each column.
							sum -= this.blur[k - offset];
							// Set the column blur.
							for (int j = 0; j < data.Height; j++)
							{
								image[j * data.Width + i] = value;
							}
						}
					}
					// Perform a vertical blur.
					{
						// The line index.
						int j = 0;
						// Set the sum of the gaussian blur.
						double sum = 0.0;
						// Compute the margin of the beginning and end of the gaussian blur.
						int margin = data.Height > (blur.Size << 1) ? blur.Size : (int)Math.Ceiling(data.Height / 2.0);
						// The beginning of the vertical blur.
						for (; j < margin; j++)
						{
							// Increment the blur sum for each line.
							sum += this.blur[j - offset];
							// Compute the alpha channel for each column.
							for (int i = 0; i < data.Width; i++)
							{
								image[j * data.Width + i] = ((((uint)Math.Ceiling(image[j * data.Width + i] * sum)) & 0xFF) << 24) | rgb;
							}
						}
						// The middle of the vertical blur.
						for (; j < data.Height - margin; j++)
						{
							// Compute the alpha channel for each column.
							for (int i = 0; i < data.Width; i++)
							{
								image[j * data.Width + i] = (((image[j * data.Width + i]) & 0xFF) << 24) | rgb;
							}
						}
						// The end of the vertical blur.
						for (int k = 0; j < data.Height; j++, k++)
						{
							// Compute the alpha channel for each column.
							for (int i = 0; i < data.Width; i++)
							{
								image[j * data.Width + i] = ((((uint)Math.Ceiling(image[j * data.Width + i] * sum)) & 0xFF) << 24) | rgb;
							}
							// Decrement the blur sum for each line.
							sum -= this.blur[k - offset];
						}
					}
				}
			}
			finally
			{
				// Unlock the bitmap data.
				this.bitmap.UnlockBits(data);
			}
		}
	}
}
