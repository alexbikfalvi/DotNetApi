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
using DotNetApi.Windows.Controls;

namespace DotNetApi.Windows.Controls
{
	/// <summary>
	/// A control that displays the world map along with custom made annotations.
	/// </summary>
	public class WorldMap : ThreadSafeControl
	{
		private static double deltaLong = -0.02794361719535286004815446831389;
		private static double deltaLat = 0.0;
		private static double[] lats = new double[] {
			0.0 / 18.0, 1.0 / 18.0, 2.0 / 18.0, 3.0 / 18.0, 4.0 / 18.0, 5.0 / 18.0, 6.0 / 18.0, 7.0 / 18.0, 8.0 / 18.0,
			10.0 / 18.0, 11.0 / 18.0, 12.0 / 18.0, 13.0 / 18.0, 14.0 / 18.0, 15.0 / 18.0, 16.0 / 18.0, 17.0 / 18.0, 18.0 / 18.0
		};
		private static double[] longs = new double[] {
			1.0 / 18.0, 2.0 / 18.0, 3.0 / 18.0, 4.0 / 18.0, 5.0 / 18.0, 6.0 / 18.0, 7.0 / 18.0, 8.0 / 18.0,
			10.0 / 18.0, 11.0 / 18.0, 12.0 / 18.0, 13.0 / 18.0, 14.0 / 18.0, 15.0 / 18.0, 16.0 / 18.0, 17.0 / 18.0
		};
		private static Image[] maps = new Image[] {
			Resources.WorldMap1000x506,
			Resources.WorldMap2000x1012,
			Resources.WorldMap4000x2024
		};
		private int mapIndex = 0;

		// Property variables.

		private bool gridMajor = true;
		private bool gridMinor = true;
		private Color colorGridMajor = Color.FromArgb(128, Color.Gray);
		private Color colorGridMinor = Color.FromArgb(48, Color.Gray);

		/// <summary>
		/// Creates a new control instance.
		/// </summary>
		public WorldMap()
		{
			// Set control as double buffered.
			this.DoubleBuffered = true;
		}

		// Public properties.

		/// <summary>
		/// Gets or sets whether the major map grid is displayed.
		/// </summary>
		public bool GridMajor
		{
			get { return this.gridMajor; }
			set
			{
				// Set the variable.
				this.gridMajor = value;
				// Refresh the control.
				this.Refresh();
			}
		}
		/// <summary>
		/// Gets or sets whether the minor map grid is displayed.
		/// </summary>
		public bool GridMinor
		{
			get { return this.gridMinor; }
			set
			{
				// Set the variable.
				this.gridMinor = value;
				// Refresh the control.
				this.Refresh();
			}
		}
		/// <summary>
		/// Gets or sets the major grid color.
		/// </summary>
		public Color ColorGridMajor
		{
			get { return this.colorGridMajor; }
			set
			{
				// Set the variable.
				this.colorGridMajor = value;
				// Refresh the control.
				this.Refresh();
			}
		}
		/// <summary>
		/// Gets or sets the minor grid color.
		/// </summary>
		public Color ColorGridMinor
		{
			get { return this.colorGridMinor; }
			set
			{
				// Set the variable.
				this.colorGridMinor = value;
				// Refresh the control.
				this.Refresh();
			}
		}

		// Protected methods.

		/// <summary>
		/// An event handler called when the control is being painted.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			// Call the base class method.
			base.OnPaint(e);
			// Paint the map.
			e.Graphics.DrawImage(WorldMap.maps[this.mapIndex], this.ClientRectangle);
			// Draw the minor grid.
			if (this.gridMinor)
			{
				using (Pen pen = new Pen(this.colorGridMinor))
				{
					float pos;
					// Draw the parallels.
					foreach (double lat in WorldMap.lats)
					{
						pos = (float)Math.Round(this.ClientRectangle.Top + WorldMap.Normalize(lat + WorldMap.deltaLat) * this.ClientRectangle.Height);
						e.Graphics.DrawLine(pen, this.ClientRectangle.Left, pos, this.ClientRectangle.Right, pos);
					}
					// Draw the meridians.
					foreach (double lng in WorldMap.longs)
					{
						pos = (float)Math.Round(this.ClientRectangle.Left + WorldMap.Normalize(lng + WorldMap.deltaLong) * this.ClientRectangle.Width);
						e.Graphics.DrawLine(pen, pos, this.ClientRectangle.Top, pos, this.ClientRectangle.Bottom);
					}
				}
			}
			// Draw the major grid.
			if (this.gridMajor)
			{
				using (Pen pen = new Pen(this.colorGridMajor))
				{
					// Draw the equator.
					float pos = (float)Math.Round(this.ClientRectangle.Top + WorldMap.Normalize(0.5 + WorldMap.deltaLat) * this.ClientRectangle.Height);
					e.Graphics.DrawLine(pen, this.ClientRectangle.Left, pos, this.ClientRectangle.Right, pos);
					// Draw the 0 degrees meridian.
					pos = (float)Math.Round(this.ClientRectangle.Left + WorldMap.Normalize(0.5 + WorldMap.deltaLong) * this.ClientRectangle.Width);
					e.Graphics.DrawLine(pen, pos, this.ClientRectangle.Top, pos, this.ClientRectangle.Bottom);
					// Draw the 180 degrees meridian.
					pos = (float)Math.Round(this.ClientRectangle.Left + WorldMap.Normalize(1.0 + WorldMap.deltaLong) * this.ClientRectangle.Width);
					e.Graphics.DrawLine(pen, pos, this.ClientRectangle.Top, pos, this.ClientRectangle.Bottom);
				}
			}
		}

		/// <summary>
		/// An event handler called when the control is being resized.
		/// </summary>
		/// <param name="e">The even arguments.</param>
		protected override void OnResize(EventArgs e)
		{
			// Call the base class method.
			base.OnResize(e);
			// Update the map index.
			this.mapIndex = (this.ClientSize.Width <= 1000) ? 0 : (this.ClientSize.Width <= 2000) ? 1 : 2;
		}

		// Private methods.

		/// <summary>
		/// Returns the positive fractional part of the specified number.
		/// </summary>
		/// <param name="value">The number.</param>
		/// <returns>The positive fractional part.</returns>
		private static double Normalize(double value)
		{
			return value > 0 ? value - Math.Floor(value) : value + Math.Ceiling(value);
		}
	}
}
