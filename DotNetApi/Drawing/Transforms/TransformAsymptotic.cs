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

namespace DotNetApi.Drawing.Transforms
{
	/// <summary>
	/// A class representing an asymptotic transform.
	/// </summary>
	public sealed class TransformAsymptotic : Transform
	{
		/// <summary>
		/// Creates a new asymptotic transform with the specified maximum deflection.
		/// </summary>
		/// <param name="horizontal">The maximum horizontal deflection.</param>
		/// <param name="vertical">The maximum vertical deflection.</param>
		public TransformAsymptotic(int horizontal, int vertical)
		{
			// Validate the parameters.
			if (horizontal <= 0) throw new ArgumentException("The argument must be greater than zero.", "horizontal");
			if (vertical <= 0) throw new ArgumentException("The argument must be greater than zero.", "vertical");
			// Set the deflection.
			this.Deflection = new Size(horizontal, vertical);
		}

		// Public properties.

		/// <summary>
		/// Gets the maximum deflection.
		/// </summary>
		public Size Deflection { get; private set; }

		// Public methods.

		/// <summary>
		/// Transforms the specified point.
		/// </summary>
		/// <param name="point">The point.</param>
		/// <returns>The transformed point.</returns>
		public override Point GetAbsolute(Point point)
		{
			return this.Anchor.Add(this.GetRelative(point));
		}

		/// <summary>
		/// Transforms the specified point relative to the current anchor.
		/// </summary>
		/// <param name="point">The point.</param>
		/// <returns>The transformed point.</returns>
		public override Point GetRelative(Point point)
		{
			double dx = point.X - this.Anchor.X;
			double dy = point.Y - this.Anchor.Y;
			int sx = Math.Sign(dx);
			int sy = Math.Sign(dy);
			return new Point(
				(int)Math.Round(sx * this.Deflection.Width * (1.0 - Math.Exp(-Math.Abs(dx) / this.Deflection.Width))),
				(int)Math.Round(sy * this.Deflection.Height * (1.0 - Math.Exp(-Math.Abs(dy) / this.Deflection.Height)))
				);
		}
	}
}
