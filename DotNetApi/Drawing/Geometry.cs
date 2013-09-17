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
using System.Windows.Forms;

namespace DotNetApi.Drawing
{
	/// <summary>
	/// An enumeration representing horizontal alignments.
	/// </summary>
	public enum HorizontalAlign
	{
		LeftOutside,
		LeftInside,
		Center,
		RightOutside,
		RightInside
	}

	/// <summary>
	/// An enumeration representing vertical alignments.
	/// </summary>
	public enum VerticalAlign
	{
		TopOutside,
		TopInside,
		Center,
		BottomOutside,
		BottomInside
	}

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
		/// Adds to the current point the specified offset on both the X and Y axes.
		/// </summary>
		/// <param name="point">The point.</param>
		/// <param name="offset">The offset.</param>
		/// <returns>The new point.</returns>
		public static Point Add(this Point point, int offset)
		{
			return new Point(point.X + offset, point.Y + offset);
		}

		/// <summary>
		/// Adds to the current point the specified X and Y offset values.
		/// </summary>
		/// <param name="point">The point.</param>
		/// <param name="x">The X offset.</param>
		/// <param name="y">The Y offset.</param>
		/// <returns>The new point.</returns>
		public static Point Add(this Point point, int x, int y)
		{
			return new Point(point.X + x, point.Y + y);
		}

		/// <summary>
		/// Increments the current size with the specified delta on both the X and Y axes.
		/// </summary>
		/// <param name="size">The size.</param>
		/// <param name="delta">The delta.</param>
		/// <returns>The new size.</returns>
		public static Size Add(this Size size, int delta)
		{
			return new Size(size.Width + delta, size.Height + delta);
		}

		/// <summary>
		/// Adds to the current size the specified padding.
		/// </summary>
		/// <param name="size">The size.</param>
		/// <param name="padding">The padding.</param>
		/// <returns>The new size.</returns>
		public static Size Add(this Size size, Padding padding)
		{
			return new Size(size.Width + padding.Horizontal, size.Height + padding.Vertical);
		}

		/// <summary>
		/// Adds to the current rectangle location the specified offset value.
		/// </summary>
		/// <param name="rectangle">The rectangle.</param>
		/// <param name="point">The offset point.</param>
		/// <returns>The new rectangle.</returns>
		public static Rectangle Add(this Rectangle rectangle, Point point)
		{
			return new Rectangle(rectangle.Location.Add(point), rectangle.Size);
		}

		/// <summary>
		/// Adds to the current rectangle location the specified X and Y offset values.
		/// </summary>
		/// <param name="rectangle">The rectangle.</param>
		/// <param name="x">The X offset.</param>
		/// <param name="y">The Y offset.</param>
		/// <returns>The new rectangle.</returns>
		public static Rectangle Add(this Rectangle rectangle, int x, int y)
		{
			return new Rectangle(rectangle.Location.Add(x, y), rectangle.Size);
		}

		/// <summary>
		/// Adds to the current size the specfied width.
		/// </summary>
		/// <param name="size">The size.</param>
		/// <param name="width">The width.</param>
		/// <returns>The new size.</returns>
		public static Size AddWidth(this Size size, int width)
		{
			return new Size(size.Width + width, size.Height);
		}

		/// <summary>
		/// Adds to the current size the specified height.
		/// </summary>
		/// <param name="size">The size.</param>
		/// <param name="height">The height.</param>
		/// <returns>The new size.</returns>
		public static Size AddHeight(this Size size, int height)
		{
			return new Size(size.Width, size.Height + height);
		}

		/// <summary>
		/// Adds the specified width to the rectangle left side.
		/// </summary>
		/// <param name="rectangle">The rectangle.</param>
		/// <param name="width">The width to add.</param>
		/// <returns>The new rectangle.</returns>
		public static Rectangle AddToLeft(this Rectangle rectangle, int width)
		{
			return new Rectangle(rectangle.X - width, rectangle.Y, rectangle.Width + width, rectangle.Height);
		}

		/// <summary>
		/// Adds the specified width to the rectangle right side.
		/// </summary>
		/// <param name="rectangle">The rectangle.</param>
		/// <param name="width">The width to add.</param>
		/// <returns>The new rectangle.</returns>
		public static Rectangle AddToRight(this Rectangle rectangle, int width)
		{
			return new Rectangle(rectangle.X, rectangle.Y, rectangle.Width + width, rectangle.Height);
		}

		/// <summary>
		/// Adds the specified height to the rectangle top side.
		/// </summary>
		/// <param name="rectangle">The rectangle.</param>
		/// <param name="height">The height to add.</param>
		/// <returns>The new rectangle.</returns>
		public static Rectangle AddToTop(this Rectangle rectangle, int height)
		{
			return new Rectangle(rectangle.X, rectangle.Y - height, rectangle.Width, rectangle.Height + height);
		}

		/// <summary>
		/// Adds the specified height to the rectangle bottom side.
		/// </summary>
		/// <param name="rectangle">The rectangle.</param>
		/// <param name="height">The height to add.</param>
		/// <returns>The new rectangle.</returns>
		public static Rectangle AddToBottom(this Rectangle rectangle, int height)
		{
			return new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height + height);
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
		/// Subtracts from the current point the specified offset on both the X and Y axes.
		/// </summary>
		/// <param name="point">The point.</param>
		/// <param name="offset">The offset.</param>
		/// <returns>The new point.</returns>
		public static Point Subtract(this Point point, int offset)
		{
			return new Point(point.X - offset, point.Y - offset);
		}

		/// <summary>
		/// Subtracts from the current point the specified X and Y offset values.
		/// </summary>
		/// <param name="point">The point.</param>
		/// <param name="x">The X offset.</param>
		/// <param name="y">The Y offset.</param>
		/// <returns>The new point.</returns>
		public static Point Subtract(this Point point, int x, int y)
		{
			return new Point(point.X - x, point.Y - y);
		}

		/// <summary>
		/// Decrements the current size with the specified delta on both the X and Y axes.
		/// </summary>
		/// <param name="size">The size.</param>
		/// <param name="delta">The delta.</param>
		/// <returns>The new size.</returns>
		public static Size Subtract(this Size size, int delta)
		{
			return new Size(size.Width - delta, size.Height - delta);
		}

		/// <summary>
		/// Subtracts from the current rectangle location the specified offset value.
		/// </summary>
		/// <param name="rectangle">The rectangle.</param>
		/// <param name="point">The offset point.</param>
		/// <returns>The new rectangle.</returns>
		public static Rectangle Subtract(this Rectangle rectangle, Point point)
		{
			return new Rectangle(rectangle.Location.Subtract(point), rectangle.Size);
		}

		/// <summary>
		/// Subtracts to the current rectangle location the specified X and Y offset values.
		/// </summary>
		/// <param name="rectangle">The rectangle.</param>
		/// <param name="x">The X offset.</param>
		/// <param name="y">The Y offset.</param>
		/// <returns>The new rectangle.</returns>
		public static Rectangle Subtract(this Rectangle rectangle, int x, int y)
		{
			return new Rectangle(rectangle.Location.Subtract(x, y), rectangle.Size);
		}


		/// <summary>
		/// Returns the point with the minimum X and Y coodinates between the given number of points.
		/// </summary>
		/// <param name="points">The points.</param>
		/// <returns>The point.</returns>
		public static Point Min(this Point[] points)
		{
			if (null == points) throw new ArgumentNullException("points");

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
		/// Returns the point with the maximum X and Y coordinates between the given number of points.
		/// </summary>
		/// <param name="points">The points.</param>
		/// <returns>The point.</returns>
		public static Point Max(this Point[] points)
		{
			if (null == points) throw new ArgumentNullException("points");

			int x = int.MinValue;
			int y = int.MinValue;

			foreach (Point point in points)
			{
				if (x < point.X) x = point.X;
				if (y < point.Y) y = point.Y;
			}

			return new Point(x, y);
		}

		/// <summary>
		/// Computes the location of a rectangle of the specified size, align to the given anchor.
		/// </summary>
		/// <param name="size">The rectangle size.</param>
		/// <param name="anchor">The anchor rectangle.</param>
		/// <param name="horizontalAlignment">The horizontal alignment.</param>
		/// <param name="verticalAlignment">The vertical alignment.</param>
		/// <returns>The aligned rectangle.</returns>
		public static Rectangle Align(this Size size, Rectangle anchor, HorizontalAlign horizontalAlignment, VerticalAlign verticalAlignment)
		{
			// Declare the location.
			Point location = default(Point);
			// Compute the X coordinate.
			switch (horizontalAlignment)
			{
				case HorizontalAlign.LeftOutside:
					location.X = anchor.Left - size.Width;
					break;
				case HorizontalAlign.LeftInside:
					location.X = anchor.Left;
					break;
				case HorizontalAlign.Center:
					location.X = anchor.Left + (anchor.Width >> 1) - (size.Width >> 1);
					break;
				case HorizontalAlign.RightInside:
					location.X = anchor.Right - size.Width;
					break;
				case HorizontalAlign.RightOutside:
					location.X = anchor.Right;
					break;
			}
			// Compute the Y coordinate.
			switch (verticalAlignment)
			{
				case VerticalAlign.TopOutside:
					location.Y = anchor.Top - size.Height;
					break;
				case VerticalAlign.TopInside:
					location.Y = anchor.Top;
					break;
				case VerticalAlign.Center:
					location.Y = anchor.Top + (anchor.Height >> 1) - (size.Height >> 1);
					break;
				case VerticalAlign.BottomInside:
					location.Y = anchor.Bottom - size.Height;
					break;
				case VerticalAlign.BottomOutside:
					location.Y = anchor.Bottom;
					break;
			}
			// Return the aligned rectangle.
			return new Rectangle(location, size);
		}

		/// <summary>
		/// Returns the rectangle region encompassing the two rectangles.
		/// </summary>
		/// <param name="rectangle1">The first rectangle.</param>
		/// <param name="rectangle2">The second rectangle.</param>
		/// <returns>The merged rectangle region.</returns>
		public static Rectangle Merge(this Rectangle rectangle1, Rectangle rectangle2)
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
				(int)Math.Ceiling(rectangle.Right) - x + 1,
				(int)Math.Ceiling(rectangle.Bottom) - y + 1);
		}

		/// <summary>
		/// Returns the middle point for the specified rectangle.
		/// </summary>
		/// <param name="rectangle">The rectangle.</param>
		/// <returns>The middle point.</returns>
		public static Point Middle(this Rectangle rectangle)
		{
			return new Point(rectangle.X + (rectangle.Width >> 1), rectangle.Y + (rectangle.Height >> 1));
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
