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
using System.Drawing.Drawing2D;
using DotNetApi.Drawing;
using MapApi;

namespace DotNetApi.Windows.Controls
{
	/// <summary>
	/// A class representing a map region.
	/// </summary>
	public class MapRegion : MapItem
	{
		private readonly MapShapePolygon shape;
		private readonly PointF[][] points;
		private readonly GraphicsPath path;

		private Rectangle bounds;
		private string name;

		private Color colorBackground = Color.Green;
		private Color colorBorder = Color.White;

		private readonly object sync = new object();
		private bool disposed = false;

		/// <summary>
		/// Creates a new map region from the specified map shape, given the geographic map bounds and map scale.
		/// </summary>
		/// <param name="shape">The map shape.</param>
		public MapRegion(MapShapePolygon shape)
		{
			// Validate the parameters.
			if (null == shape) throw new ArgumentNullException("shape");

			// Save the map shape.
			this.shape = shape;
			// Create the map points.
			this.points = new PointF[shape.Parts.Count][];
			for (int index = 0; index < shape.Parts.Count; index++)
			{
				this.points[index] = new PointF[shape.Parts[index].Points.Count];
			}
			// Create the graphics path.
			this.path = new GraphicsPath();
			// Get the region metadata.
			this.name = shape.Metadata["admin"];
		}

		// Public properties.

		/// <summary>
		/// Returns a rectangle that bounds this region.
		/// </summary>
		public override Rectangle Bounds { get { return this.bounds; } }
		/// <summary>
		/// Returns a rectangle that bounds this region.
		/// </summary>
		public override Rectangle AnchorBounds { get { return this.bounds; } }
		/// <summary>
		/// Returns the region name.
		/// </summary>
		public override string Name
		{
			get { return this.name; }
			set { this.name = value; }
		}

		// Internal methods.

		/// <summary>
		/// Indicates whether the specified point is contained within this region.
		/// </summary>
		/// <param name="point">The point.</param>
		/// <returns><b>True</b> if the point is contained within the region, <b>false</b> otherwise.</returns>
		internal override bool IsVisible(Point point)
		{
			// If the object has been disposed throw an exception.
			if (this.disposed) throw new ObjectDisposedException("region");
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
			if (this.disposed) throw new ObjectDisposedException("region");
			lock (this.sync)
			{
				// Reset the graphics path.
				this.path.Reset();
				// For all shape parts.
				for (int indexPart = 0; indexPart < shape.Parts.Count; indexPart++)
				{
					// Update the list of points for each part polygon.
					for (int indexPoint = 0; indexPoint < shape.Parts[indexPart].Points.Count; indexPoint++)
					{
						this.points[indexPart][indexPoint].X = (float)((shape.Parts[indexPart].Points[indexPoint].X - bounds.Left) * scale.Width);
						this.points[indexPart][indexPoint].Y = (float)((bounds.Top - shape.Parts[indexPart].Points[indexPoint].Y) * scale.Height);
					}
					// Add the polygon to the graphics path.
					this.path.AddPolygon(this.points[indexPart]);
				}
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
			if (this.disposed) throw new ObjectDisposedException("region");
			lock (this.sync)
			{
				// Fill the path.
				using (SolidBrush brush = new SolidBrush(this.colorBackground))
				{
					graphics.FillPath(brush, this.path);
				}
				// Draw the path.
				using (Pen pen = new Pen(this.colorBackground))
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
			if (this.disposed) throw new ObjectDisposedException("region");
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
			using (SolidBrush brush = new SolidBrush(this.colorBackground))
			{
				// Create the pen.
				using (Pen pen = new Pen(this.colorBackground))
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
				// For all shape parts.
				for (int indexPart = 0; indexPart < shape.Parts.Count; indexPart++)
				{
					// Update the list of points for each part polygon.
					for (int indexPoint = 0; indexPoint < shape.Parts[indexPart].Points.Count; indexPoint++)
					{
						this.points[indexPart][indexPoint].X = (float)((shape.Parts[indexPart].Points[indexPoint].X - bounds.Left) * scale.Width);
						this.points[indexPart][indexPoint].Y = (float)((bounds.Top - shape.Parts[indexPart].Points[indexPoint].Y) * scale.Height);
					}
					// Add the polygon to the graphics path.
					path.AddPolygon(this.points[indexPart]);
				}
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
