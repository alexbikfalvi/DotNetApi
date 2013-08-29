using System;
using System.Drawing;

namespace DotNetApi.Drawing
{
	/// <summary>
	/// A class representing a polygon.
	/// </summary>
	public sealed class Polygon
	{
		private Point location;
		private readonly Point[] points;

		/// <summary>
		/// Creates a polygon shape form the specified rectangle.
		/// </summary>
		/// <param name="rectangle">The rectangle.</param>
		public Polygon(Rectangle rectangle)
		{
			// Set the location.
			this.location = rectangle.Location;
			// Set the points.
			this.points = new Point[4];
			this.points[0] = new Point(0, 0);
			this.points[1] = new Point(rectangle.Width, 0);
			this.points[2] = new Point(rectangle.Width, rectangle.Height);
			this.points[3] = new Point(0, rectangle.Height);
		}

		/// <summary>
		/// Creates a polygon shape from the specified points.
		/// </summary>
		/// <param name="points">The points.</param>
		public Polygon(Point[] points)
		{
			// Set the location.
			this.location = points.Min();
			// Set the points.
			this.points = new Point[points.Length];
			for (int index = 0; index < points.Length; index++)
			{
				this.points[index] = points[index].Subtract(this.location);
			}
		}

		// Public properties.

		/// <summary>
		/// Gets or sets the polygon location.
		/// </summary>
		public Point Location
		{
			get { return this.location; }
			set { this.location = value; }
		}

		/// <summary>
		/// Gets the number of points the current polygon.
		/// </summary>
		public int Count
		{
			get { return this.points.Length; }
		}

		// Public methods.

		/// <summary>
		/// Checks whether the current polygon has the same shape with the specified polygon.
		/// </summary>
		/// <param name="polygon">The polygon to compare.</param>
		/// <returns><b>True</b> if the polygons have the same shape, <b>false</b> otherwise.</returns>
		public bool IsEqual(Polygon polygon)
		{
			// If the number of points is not the same, return false.
			if (this.points.Length != polygon.points.Length) return false;
			// For all polygon points.
			for (int index = 0; index < this.points.Length; index++)
			{
				if ((this.points[index].X != polygon.points[index].Y) || (this.points[index].X != polygon.points[index].Y)) return false;
			}
			return true;
		}
	}
}
