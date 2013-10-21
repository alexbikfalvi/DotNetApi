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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using DotNetApi.Drawing;
using MapApi;

namespace DotNetApi.Windows.Controls
{
	/// <summary>
	/// A class representing a circular map marker.
	/// </summary>
	public class MapBulletMarker : MapMarker
	{
		private readonly GraphicsPath path = new GraphicsPath();
		private Rectangle bounds;
		private bool disposed = false;
		private readonly object sync = new object();

		/// <summary>
		/// Creates a new circular map marker instance at the default location.
		/// </summary>
		public MapBulletMarker()
		{
		}

		/// <summary>
		/// Creates a new circular map marker instance.
		/// </summary>
		/// <param name="location">The marker location as longitude and latitude in degrees.</param>
		public MapBulletMarker(MapPoint location)
			: base(location)
		{
		}

		/// <summary>
		/// Returns a rectangle that bounds this marker.
		/// </summary>
		public override Rectangle Bounds { get { return this.bounds; } }
		/// <summary>
		/// Returns a rectangle that bounds this marker.
		/// </summary>
		public override Rectangle AnchorBounds { get { return this.bounds; } }

		// Internal methods.

		/// <summary>
		/// Indicates whether the specified point is contained within this region.
		/// </summary>
		/// <param name="point">The point.</param>
		/// <returns><b>True</b> if the point is contained within the region, <b>false</b> otherwise.</returns>
		internal override bool IsVisible(Point point)
		{
			// If the object has been disposed throw an exception.
			if (this.disposed) throw new ObjectDisposedException("marker");
			lock (this.sync)
			{
				return this.path.IsVisible(point);
			}
		}

		/// <summary>
		/// Updates the map item geometric characteristics to the specified map bounds and scale.
		/// </summary>
		/// <param name="bounds">The map bounds.</param>
		/// <param name="scale">The map scale.</param>
		internal override void Update(MapRectangle bounds, MapScale scale)
		{
			// If the object has been disposed throw an exception.
			if (this.disposed) throw new ObjectDisposedException("marker");
			// Compute the marker center.
			PointF center = new PointF(
				(float)((this.Location.X - bounds.Left) * scale.Width),
				(float)((bounds.Top - this.Location.Y) * scale.Height)
				);
			// Lock the current path.
			lock (this.sync)
			{
				// Reset the graphics path.
				this.path.Reset();
				// Add a circle to the path.
				this.path.AddEllipse((float)(center.X - this.Size.Width / 2.0), (float)(center.Y - this.Size.Height / 2.0), this.Size.Width, this.Size.Height);
				// Set the region bounds.
				this.bounds = this.path.GetBounds().Ceiling();
			}
		}

		/// <summary>
		/// Draws the item on the specified graphics 
		/// </summary>
		/// <param name="graphics">The graphics object.</param>
		internal override void Draw(Graphics graphics)
		{
			// If the object has been disposed throw an exception.
			if (this.disposed) throw new ObjectDisposedException("marker");
			lock (this.sync)
			{
				// Fill the path.
				using (SolidBrush brush = new SolidBrush(this.BackgroundColor))
				{
					graphics.FillPath(brush, this.path);
				}
				// Draw the path.
				using (Pen pen = new Pen(this.BorderColor))
				{
					graphics.DrawPath(pen, this.path);
				}
			}
		}

		/// <summary>
		/// Draws the item on the specified graphics object.
		/// </summary>
		/// <param name="graphics">The graphics object.</param>
		/// <param name="brush">The brush.</param>
		/// <param name="pen">The pen.</param>
		internal override void Draw(Graphics graphics, Brush brush, Pen pen)
		{
			// If the object has been disposed throw an exception.
			if (this.disposed) throw new ObjectDisposedException("marker");
			lock (this.sync)
			{
				// Fill the path.
				if (null != brush) graphics.FillPath(brush, this.path);
				// Draw the path.
				if (null != pen) graphics.DrawPath(pen, this.path);
			}
		}

		/// <summary>
		/// Draws the item on the specified graphics object within the specified map bounds and scale.
		/// </summary>
		/// <param name="graphics">The graphics.</param>
		/// <param name="bounds">The bounds.</param>
		/// <param name="scale">The scale.</param>
		internal override void Draw(Graphics graphics, MapRectangle bounds, MapScale scale)
		{
			// Create the brush.
			using (SolidBrush brush = new SolidBrush(this.BackgroundColor))
			{
				// Create the pen.
				using (Pen pen = new Pen(this.BorderColor))
				{
					this.Draw(graphics, bounds, scale, brush, pen);
				}
			}
		}

		/// <summary>
		/// Draws the item on the specified graphics object within the specified map bounds and scale.
		/// </summary>
		/// <param name="graphics">The graphics</param>
		/// <param name="bounds">The bounds.</param>
		/// <param name="scale">The scale.</param>
		/// <param name="brush">The brush.</param>
		/// <param name="pen">The pen.</param>
		internal override void Draw(Graphics graphics, MapRectangle bounds, MapScale scale, Brush brush, Pen pen)
		{
			// Create a new graphics path.
			using (GraphicsPath path = new GraphicsPath())
			{
				// Compute the marker center.
				PointF center = new PointF(
					(float)((this.Location.X - bounds.Left) * scale.Width),
					(float)((bounds.Top - this.Location.Y) * scale.Height)
					);
				// Add a circle to the path.
				path.AddEllipse((float)(center.X - this.Size.Width / 2.0), (float)(center.Y - this.Size.Height / 2.0), this.Size.Width, this.Size.Height);
				// Fill the path.
				if (null != brush) graphics.FillPath(brush, path);
				// Draw the path.
				if (null != pen) graphics.DrawPath(pen, path);
			}
		}

		// Protected methods.

		/// <summary>
		/// An event handler called when the object is being disposed.
		/// </summary>
		/// <param name="disposed">If <b>true</b>, clean both managed and native resources. If <b>false</b>, clean only native resources.</param>
		protected override void Dispose(bool disposing)
		{
			// Dispose the managed resources.
			if (disposing)
			{
				this.path.Dispose();
			}
			// Set the disposed flag to true.
			this.disposed = true;
			// Call the base class method.
			base.Dispose(disposing);
		}
	}
}
