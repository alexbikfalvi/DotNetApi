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

namespace DotNetApi.Windows.Controls
{
	/// <summary>
	/// A class representing a circular geo marker.
	/// </summary>
	public class GeoMarkerCircle : GeoMarker
	{
		/// <summary>
		/// Creates a new circular geo marker instance.
		/// </summary>
		/// <param name="coordinates">The marker coordinates as longitude and latitude in degrees.</param>
		public GeoMarkerCircle(PointF coordinates)
			: base(coordinates)
		{
		}

		// Internal methods.

		/// <summary>
		/// Paints the current marker.
		/// </summary>
		/// <param name="g">The graphics object.</param>
		/// <param name="rectangle">The rectangle.</param>
		internal override void Paint(Graphics g, Rectangle rectangle)
		{
			using (SolidBrush brush = new SolidBrush(this.ColorFill))
			{
				g.FillEllipse(brush, rectangle);
			}
			using (Pen pen = new Pen(this.ColorLine))
			{
				g.DrawEllipse(pen, rectangle);
			}
		}
	}
}
