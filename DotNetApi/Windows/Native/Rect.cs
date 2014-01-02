/*
 * Copyright (c) 2013 David Hall
 * Copyright (c) 2013 Alex Bikfalvi
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software
 * and associated documentation files (the "Software"), to deal in the Software without restriction,
 * including without limitation the rights to use, copy, modify, merge, publish, distribute,
 * sublicense, and/or sell copies of the Software, and to permit persons to whom the Software
 * is furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all copies or substantial
 * portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
 * BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
 * DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;

namespace DotNetApi.Windows.Native
{
	/// <summary>
	/// A class with native methods.
	/// </summary>
	public static partial class NativeMethods
	{
		/// <summary>
		/// A structure equivalent to the <b>RECT</b> structure from the Windows API.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct Rect
		{
			// Public fields.

			/// <summary>
			/// The left coordinate.
			/// </summary>
			public int Left;
			/// <summary>
			/// The top coordinate.
			/// </summary>
			public int Top;
			/// <summary>
			/// The right coordinate.
			/// </summary>
			public int Right;
			/// <summary>
			/// The bottom coordinate.
			/// </summary>
			public int Bottom;

			/// <summary>
			/// Creates a new structure instance from the specified coordinates.
			/// </summary>
			/// <param name="left">The left coordinate.</param>
			/// <param name="top">The top coordinate.</param>
			/// <param name="right">The right coordinate.</param>
			/// <param name="bottom">The bottom coordinate.</param>
			public Rect(int left, int top, int right, int bottom)
			{
				Left = left;
				Top = top;
				Right = right;
				Bottom = bottom;
			}

			/// <summary>
			/// Creates a new rectangle structure from the specified rectangle.
			/// </summary>
			/// <param name="rect">The rectangle</param>
			public Rect(Rectangle rect)
				: this(rect.Left, rect.Top, rect.Right, rect.Bottom)
			{
			}

			// Public properties.

			/// <summary>
			/// Gets or sets the X coordinate.
			/// </summary>
			public int X
			{
				get { return Left; }
				set { Right -= (Left - value); Left = value; }
			}
			/// <summary>
			/// Gets or sets the Y coordinate.
			/// </summary>
			public int Y
			{
				get { return Top; }
				set { Bottom -= (Top - value); Top = value; }
			}
			/// <summary>
			/// Gets or sets the rectangle height.
			/// </summary>
			public int Height
			{
				get { return Bottom - Top; }
				set { Bottom = value + Top; }
			}
			/// <summary>
			/// Gets or sets the rectangle width.
			/// </summary>
			public int Width
			{
				get { return Right - Left; }
				set { Right = value + Left; }
			}
			/// <summary>
			/// Gets or sets the rectangle location.
			/// </summary>
			public Point Location
			{
				get { return new Point(Left, Top); }
				set { X = value.X; Y = value.Y; }
			}
			/// <summary>
			/// Gets or sets the rectangle height.
			/// </summary>
			public Size Size
			{
				get { return new Size(Width, Height); }
				set { Width = value.Width; Height = value.Height; }
			}

			// Public methods.

			/// <summary>
			/// Creates a managed rectangle from the specified native rectangle.
			/// </summary>
			/// <param name="rect">The native rectangle.</param>
			/// <returns>The managed rectangle.</returns>
			public static implicit operator Rectangle(Rect rect)
			{
				return new Rectangle(rect.Left, rect.Top, rect.Width, rect.Height);
			}

			/// <summary>
			/// Creates a native rectangle from the specified managed rectangle.
			/// </summary>
			/// <param name="rect">The managed rectangle.</param>
			/// <returns>The native rectangle.</returns>
			public static implicit operator Rect(Rectangle rect)
			{
				return new Rect(rect);
			}

			/// <summary>
			/// Compares two rectangles for equality.
			/// </summary>
			/// <param name="left">The left rectangle.</param>
			/// <param name="right">The right rectangle.</param>
			/// <returns><b>True</b> if the two rectangles are equal, <b>false</b> otherwise.</returns>
			public static bool operator ==(Rect left, Rect right)
			{
				return left.Equals(right);
			}

			/// <summary>
			/// Compares two rectangles for inequality.
			/// </summary>
			/// <param name="left">The left rectangle.</param>
			/// <param name="right">The right rectangle.</param>
			/// <returns><b>True</b> if the two rectangles are different, <b>false</b> otherwise.</returns>
			public static bool operator !=(Rect left, Rect right)
			{
				return !left.Equals(right);
			}

			/// <summary>
			/// Compares the current and the specified rectangle for equality.
			/// </summary>
			/// <param name="rect">The rectangle to compare.</param>
			/// <returns><b>True</b> if the two rectangles are equal, <b>false</b> otherwise.</returns>
			public bool Equals(Rect rect)
			{
				return rect.Left == Left && rect.Top == Top && rect.Right == Right && rect.Bottom == Bottom;
			}

			/// <summary>
			/// Compares the current and the specified objects for equality.
			/// </summary>
			/// <param name="rect">The rectangle to compare.</param>
			/// <returns><b>True</b> if the two rectangles are equal, <b>false</b> otherwise.</returns>
			public override bool Equals(object obj)
			{
				if (obj is Rect)
					return this.Equals((Rect)obj);
				else if (obj is Rectangle)
					return this.Equals(new Rect((Rectangle)obj));
				return false;
			}

			/// <summary>
			/// Gets the hash code of the current rectangle.
			/// </summary>
			/// <returns>The hash code.</returns>
			public override int GetHashCode()
			{
				return ((Rectangle)this).GetHashCode();
			}

			/// <summary>
			/// Converts the current rectangle to a string.
			/// </summary>
			/// <returns>The string.</returns>
			public override string ToString()
			{
				return string.Format(CultureInfo.CurrentCulture,
					"{{Left={0},Top={1},Right={2},Bottom={3}}}", Left, Top, Right, Bottom);
			}
		}
	}
}
