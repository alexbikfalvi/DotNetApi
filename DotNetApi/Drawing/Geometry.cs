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

namespace DotNetApi.Drawing
{
	/// <summary>
	/// A class with useful geometric methods.
	/// </summary>
	public static class Geometry
	{
		/// <summary>
		/// Adds to the current point the specified offset.
		/// </summary>
		/// <param name="point">The point.</param>
		/// <param name="offset">The offset.</param>
		/// <returns>The new point.</returns>
		public static Point Add(this Point point, Point offset)
		{
			return new Point(point.X + offset.X, point.Y + offset.Y);
		}

		/// <summary>
		/// Subtracts from the current point the specified offset.
		/// </summary>
		/// <param name="point">The point.</param>
		/// <param name="offset">The offset.</param>
		/// <returns>The new point.</returns>
		public static Point Subtract(this Point point, Point offset)
		{
			return new Point(point.X - offset.X, point.Y - offset.Y);
		}

		/// <summary>
		/// Returns the point with the minimum X and Y coodinates between the given number of points.
		/// </summary>
		/// <param name="points">The points.</param>
		/// <returns>The point.</returns>
		public static Point Min(this Point[] points)
		{
			points.ValidateNotNull("points");

			int x = int.MaxValue;
			int y = int.MaxValue;

			foreach (Point point in points)
			{
				if (x > point.X) x = point.X;
				if (y > point.Y) y = point.Y;
			}

			return new Point(x, y);
		}

		/// <summary>
		/// Returns the rectangle region encompassing the two rectangles.
		/// </summary>
		/// <param name="rectangle1">The first rectangle.</param>
		/// <param name="rectangle2">The second rectangle.</param>
		/// <returns>The merged rectangle region.</returns>
		public static Rectangle Merge(Rectangle rectangle1, Rectangle rectangle2)
		{
			int left = rectangle1.X < rectangle2.X ? rectangle1.X : rectangle2.X;
			int top = rectangle1.Y < rectangle2.Y ? rectangle1.Y : rectangle2.Y;
			int right = rectangle1.Right > rectangle2.Right ? rectangle1.Right : rectangle2.Right;
			int bottom = rectangle1.Bottom > rectangle2.Bottom ? rectangle1.Bottom : rectangle2.Bottom;

			return new Rectangle(left, top, right - left, bottom - top);
		}

		/// <summary>
		/// Rounds the the specified rectangle to the closest integral rectangle greater than the original.
		/// </summary>
		/// <param name="rectangle">The orginal rectangle.</param>
		/// <returns>The integral rectangle.</returns>
		public static Rectangle Ceiling(this RectangleF rectangle)
		{
			int x = (int)Math.Floor(rectangle.X);
			int y = (int)Math.Floor(rectangle.Y);
			return new Rectangle(
				x,
				y,
				(int)Math.Ceiling(rectangle.Right) - x,
				(int)Math.Ceiling(rectangle.Bottom) - y);
		}

		/// <summary>
		/// Returns whether the points specifying a polygon are clockwise.
		/// </summary>
		/// <param name="points">The points defining the polygon.</param>
		/// <returns><b>True</b> if the polygon points are clockwise, <b>false</b> otherwise.</returns>
		public static bool IsPolygonClockwise(this PointF[] points)
		{
			float area = 0;
			for (int i = 0, j = points.Length - 1; i < points.Length; i++)
			{
				area += (points[j].X + points[i].X) * (points[j].Y - points[i].Y); 
			}
			return area >= 0;
		}
	}
}
