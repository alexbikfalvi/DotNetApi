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
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DotNetApi.Windows.Controls;

namespace DotNetApi.Windows.Controls
{
	/// <summary>
	/// A control that displays the world map along with custom made annotations.
	/// </summary>
	public class GeoWorldMap : ThreadSafeControl
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

		private GeoMarker.Collection markers = new GeoMarker.Collection();

		// Property variables.

		private bool gridMajor = true;
		private bool gridMinor = true;
		private bool showMarkers = true;
		private Color colorGridMajor = Color.FromArgb(128, Color.Gray);
		private Color colorGridMinor = Color.FromArgb(48, Color.Gray);

		/// <summary>
		/// Creates a new control instance.
		/// </summary>
		public GeoWorldMap()
		{
			// Set control as double buffered.
			this.DoubleBuffered = true;

			// Set the markers event handlers.
			this.markers.BeforeCleared += this.OnBeforeMarkersCleared;
			this.markers.AfterCleared += this.OnAfterMarkersCleared;
			this.markers.AfterItemInserted += this.OnAfterMarkerInserted;
			this.markers.AfterItemRemoved += this.OnAfterMarkerRemoved;
			this.markers.AfterItemSet += this.OnAfterMarkerSet;
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
		/// <summary>
		/// Gets or sets whether the markers are displayed.
		/// </summary>
		public bool ShowMarkers
		{
			get { return this.showMarkers; }
			set
			{
				// Save the old value.
				bool old = this.showMarkers;
				// Set the variable.
				this.showMarkers = value;
				// Call the event handler.
				this.OnShowMarkersChanged(old, value);
			}
		}
		/// <summary>
		/// Gets the collection of geo markers.
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
		public GeoMarker.Collection Markers { get { return this.markers; } }

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
			e.Graphics.DrawImage(GeoWorldMap.maps[this.mapIndex], this.ClientRectangle);
			// Draw the minor grid.
			if (this.gridMinor)
			{
				using (Pen pen = new Pen(this.colorGridMinor))
				{
					float pos;
					// Draw the parallels.
					foreach (double lat in GeoWorldMap.lats)
					{
						pos = (float)Math.Round(this.ClientRectangle.Top + GeoWorldMap.Normalize(lat + GeoWorldMap.deltaLat) * this.ClientRectangle.Height);
						e.Graphics.DrawLine(pen, this.ClientRectangle.Left, pos, this.ClientRectangle.Right, pos);
					}
					// Draw the meridians.
					foreach (double lng in GeoWorldMap.longs)
					{
						pos = (float)Math.Round(this.ClientRectangle.Left + GeoWorldMap.Normalize(lng + GeoWorldMap.deltaLong) * this.ClientRectangle.Width);
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
					float pos = (float)Math.Round(this.ClientRectangle.Top + GeoWorldMap.Normalize(0.5 + GeoWorldMap.deltaLat) * this.ClientRectangle.Height);
					e.Graphics.DrawLine(pen, this.ClientRectangle.Left, pos, this.ClientRectangle.Right, pos);
					// Draw the 0 degrees meridian.
					pos = (float)Math.Round(this.ClientRectangle.Left + GeoWorldMap.Normalize(0.5 + GeoWorldMap.deltaLong) * this.ClientRectangle.Width);
					e.Graphics.DrawLine(pen, pos, this.ClientRectangle.Top, pos, this.ClientRectangle.Bottom);
					// Draw the 180 degrees meridian.
					pos = (float)Math.Round(this.ClientRectangle.Left + GeoWorldMap.Normalize(1.0 + GeoWorldMap.deltaLong) * this.ClientRectangle.Width);
					e.Graphics.DrawLine(pen, pos, this.ClientRectangle.Top, pos, this.ClientRectangle.Bottom);
				}
			}
			// Set smooth drawing.
			e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
			// Draw the non-emphasized markers.
			foreach (GeoMarker marker in this.markers)
			{
				if (!marker.Emphasis) marker.Paint(e.Graphics, this.GetCoordinatesRectangle(marker.Coordinates, marker.Size));
			}
			// Draw the emphasized markers.
			foreach (GeoMarker marker in this.markers)
			{
				if (marker.Emphasis) marker.Paint(e.Graphics, this.GetCoordinatesRectangle(marker.Coordinates, marker.Size));
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
			// Refresh the control.
			this.Refresh();
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

		/// <summary>
		/// Returns the point corresponding to the given coordinates.
		/// </summary>
		/// <param name="coordinates">The coordinates.</param>
		/// <returns>The point.</returns>
		private Point GetCoordinatesPoint(PointF coordinates)
		{
			return new Point(
				(int)(this.ClientRectangle.Left + GeoWorldMap.Normalize((coordinates.X / 360.0) + 0.5 + GeoWorldMap.deltaLong) * this.ClientRectangle.Width) + 1,
				(int)(this.ClientRectangle.Top + GeoWorldMap.Normalize(0.5 - (coordinates.Y / 180.0) + GeoWorldMap.deltaLat) * this.ClientRectangle.Height) + 1
				);
		}

		/// <summary>
		/// Returns the region rectangle corresponding to rectangle centered at the given coordinates.
		/// </summary>
		/// <param name="coordinates">The coordinates.</param>
		/// <param name="size">The rectangle size.</param>
		/// <returns>The region rectangle.</returns>
		private Rectangle GetCoordinatesRectangle(PointF coordinates, Size size)
		{
			Point point = this.GetCoordinatesPoint(coordinates);
			return new Rectangle(
				new Point(point.X - (size.Width/2), point.Y - (size.Height/2) - 1),
				new Size(size.Width, size.Height)
				);
		}

		/// <summary>
		/// An event handler called before the markers are cleared.
		/// </summary>
		private void OnBeforeMarkersCleared()
		{
			// Remove the marker event handlers.
			foreach (GeoMarker marker in this.markers)
			{
				marker.Changed -= this.OnMarkerChanged;
				marker.CoordinatesChanged -= this.OnMarkerCoordinatesChanged;
				marker.SizeChanged -= this.OnMarkerSizeChanged;
				marker.EmphasisChanged -= this.OnMarkerEmphasisChanged;
			}
		}

		/// <summary>
		/// An event handler called after the markers are cleared.
		/// </summary>
		private void OnAfterMarkersCleared()
		{
			// Refresh the control.
			this.Refresh();
		}

		/// <summary>
		/// An event handler called after a marker has been inserted.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="item">The marker.</param>
		private void OnAfterMarkerInserted(int index, GeoMarker item)
		{
			// Add the marker event handler.
			item.Changed += this.OnMarkerChanged;
			item.CoordinatesChanged += this.OnMarkerCoordinatesChanged;
			item.SizeChanged += this.OnMarkerSizeChanged;
			item.EmphasisChanged += this.OnMarkerEmphasisChanged;
			// Refresh the control at the marker rectangle.
			this.Invalidate(this.GetCoordinatesRectangle(item.Coordinates, new Size(item.Size.Width + 2, item.Size.Height + 2)));
		}

		/// <summary>
		/// An event handler called after a marker has been removed.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="item">The marker.</param>
		private void OnAfterMarkerRemoved(int index, GeoMarker item)
		{
			// Remove the marker event handler.
			item.Changed -= this.OnMarkerChanged;
			item.CoordinatesChanged -= this.OnMarkerCoordinatesChanged;
			item.SizeChanged -= this.OnMarkerSizeChanged;
			item.EmphasisChanged -= this.OnMarkerEmphasisChanged;
			// Refresh the control at the marker rectangle.
			this.Invalidate(this.GetCoordinatesRectangle(item.Coordinates, new Size(item.Size.Width + 2, item.Size.Height + 2)));
		}

		/// <summary>
		/// An event handler called when a marker has been set.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="oldItem">The old marker.</param>
		/// <param name="newItem">The new marker.</param>
		private void OnAfterMarkerSet(int index, GeoMarker oldItem, GeoMarker newItem)
		{
			// If the marker has not changed, do nothing.
			if (oldItem == newItem) return;

			// If the old marker is not null.
			if (oldItem != null)
			{
				// Remove the old marker event handler.
				oldItem.Changed -= this.OnMarkerChanged;
				oldItem.CoordinatesChanged -= this.OnMarkerCoordinatesChanged;
				oldItem.SizeChanged -= this.OnMarkerSizeChanged;
				oldItem.EmphasisChanged -= this.OnMarkerEmphasisChanged;
				// Refresh the control at the marker rectangle.
				this.Invalidate(this.GetCoordinatesRectangle(oldItem.Coordinates, new Size(oldItem.Size.Width + 2, oldItem.Size.Height + 2)));
			}
			// If the new marker is not null.
			if (newItem != null)
			{
				// Add the new marker event handler.
				newItem.Changed += this.OnMarkerChanged;
				newItem.CoordinatesChanged += this.OnMarkerCoordinatesChanged;
				newItem.SizeChanged += this.OnMarkerSizeChanged;
				newItem.EmphasisChanged += this.OnMarkerEmphasisChanged;
				// Refresh the control at the marker rectangle.
				this.Invalidate(this.GetCoordinatesRectangle(newItem.Coordinates, new Size(newItem.Size.Width + 2, newItem.Size.Height + 2)));
			}
		}

		/// <summary>
		/// An event handler called when a marker has changed.
		/// </summary>
		/// <param name="marker">The marker.</param>
		private void OnMarkerChanged(GeoMarker marker)
		{
			// Refresh the control at the marker rectangle.
			this.Invalidate(this.GetCoordinatesRectangle(marker.Coordinates, new Size(marker.Size.Width + 2, marker.Size.Height + 2)));
		}

		/// <summary>
		/// An event handler called when the marker coordinates have changed.
		/// </summary>
		/// <param name="marker">The marker.</param>
		/// <param name="oldCoordinates">The old coordinates.</param>
		/// <param name="newCoordinates">The new coordinates.</param>
		private void OnMarkerCoordinatesChanged(GeoMarker marker, PointF oldCoordinates, PointF newCoordinates)
		{
			// If the coordinates have not changed, do nothing.
			if (oldCoordinates == newCoordinates) return;
			// Refresh the control at the marker rectangle.
			this.Invalidate(this.GetCoordinatesRectangle(oldCoordinates, new Size(marker.Size.Width + 2, marker.Size.Height + 2)));
			this.Invalidate(this.GetCoordinatesRectangle(newCoordinates, new Size(marker.Size.Width + 2, marker.Size.Height + 2)));
		}

		/// <summary>
		/// An event handler called when the marker size has changed.
		/// </summary>
		/// <param name="marker">The marker.</param>
		/// <param name="oldSize">The old size.</param>
		/// <param name="newSize">The new size.</param>
		private void OnMarkerSizeChanged(GeoMarker marker, Size oldSize, Size newSize)
		{
			// If the size has not changed, do nothing.
			if (oldSize == newSize) return;
			// Create the greater size.
			Size size = new Size(
				oldSize.Width > newSize.Width ? oldSize.Width + 2 : newSize.Width + 2,
				oldSize.Height > newSize.Height ? oldSize.Height + 2 : newSize.Height + 2
				);
			// Refresh the control at the marker rectangle.
			this.Invalidate(this.GetCoordinatesRectangle(marker.Coordinates, size));
		}

		/// <summary>
		/// An event handler called when the marker emphasis has changed.
		/// </summary>
		/// <param name="marker">The marker.</param>
		/// <param name="oldEmphasis"></param>
		/// <param name="newEmphasis"></param>
		private void OnMarkerEmphasisChanged(GeoMarker marker, bool oldEmphasis, bool newEmphasis)
		{
			// If the emphasis has not changed, do nothing.
			if (oldEmphasis == newEmphasis) return;
			// Refresh the control at the marker rectangle.
			this.Invalidate(this.GetCoordinatesRectangle(marker.Coordinates, new Size(marker.Size.Width + 2, marker.Size.Height + 2)));
		}

		/// <summary>
		/// An event handler called when the display of markers has changed.
		/// </summary>
		/// <param name="oldShow">The old value.</param>
		/// <param name="newShow">The new value.</param>
		private void OnShowMarkersChanged(bool oldShow, bool newShow)
		{
			// If the value has not changed, do nothing.
			if (oldShow == newShow) return;
			//  Invalidate the region for all markers.
			foreach (GeoMarker marker in this.markers)
			{
				// Refresh the control at the marker rectangle.
				this.Invalidate(this.GetCoordinatesRectangle(marker.Coordinates, new Size(marker.Size.Width + 2, marker.Size.Height + 2)));
			}
		}
	}
}
