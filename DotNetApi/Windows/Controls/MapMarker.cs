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
using System.ComponentModel;
using System.Drawing;
using MapApi;

namespace DotNetApi.Windows.Controls
{
	/// <summary>
	/// A class representing a round map marker.
	/// </summary>
	public abstract class MapMarker : MapItem
	{
		private MapPoint location;
		private Size size = new Size(8, 8);
		private Color borderColor = Color.FromArgb(255, 153, 0);
		private Color backgroundColor = Color.FromArgb(248, 224, 124);
		private bool emphasis = false;

		/// <summary>
		/// Creates a new map marker instance at the default location.
		/// </summary>
		public MapMarker()
		{
			this.location = default(MapPoint);
		}

		/// <summary>
		/// Creates a new map marker instance at the specified location.
		/// </summary>
		/// <param name="location">The marker location as longitude and latitude in degrees.</param>
		public MapMarker(MapPoint location)
		{
			this.location = location;
		}

		// Public events.

		/// <summary>
		/// An event raised when the marker color has changed.
		/// </summary>
		public event MapMarkerChangedEventHandler ColorChanged;
		/// <summary>
		/// An event raised when the marker emphasis has changed.
		/// </summary>
		public event MapMarkerChangedEventHandler EmphasisChanged;
		/// <summary>
		/// An event raised when the marker coordinates have changed.
		/// </summary>
		public event MapMarkerChangedEventHandler LocationChanged;
		/// <summary>
		/// An event raised when the marker size has changed.
		/// </summary>
		public event MapMarkerChangedEventHandler SizeChanged;

		// Public properties.

		/// <summary>
		/// Gets or sets the marker location.
		/// </summary>
		public MapPoint Location
		{
			get { return this.location; }
			set
			{
				// Save the old coordinates.
				MapPoint old = this.location;
				// Set the new coordinates.
				this.location = value;
				// Call the event handler.
				this.OnLocationChanged(old, value);
			}
		}

		/// <summary>
		/// Gets or sets the marker size.
		/// </summary>
		public Size Size
		{
			get { return this.size; }
			set
			{
				// Save the old size.
				Size old = this.size;
				// Set the new size.
				this.size = value;
				// Call the event handler.
				this.OnSizeChanged(old, value);
			}
		}

		/// <summary>
		/// Gets or sets the border color.
		/// </summary>
		public Color BorderColor
		{
			get { return this.borderColor; }
			set
			{
				// Set the new color.
				this.borderColor = value;
				// Call the event handler.
				this.OnColorChanged();
			}
		}

		/// <summary>
		/// Gets or sets the background color.
		/// </summary>
		public Color BackgroundColor
		{
			get { return this.backgroundColor; }
			set
			{
				// Set the new color.
				this.backgroundColor = value;
				// Call the event handler.
				this.OnColorChanged();
			}
		}

		/// <summary>
		/// Gets or sets whether this marker is emphasized.
		/// </summary>
		public bool Emphasized
		{
			get { return this.emphasis; }
			set
			{
				// Save the old emphasis.
				bool old = this.emphasis;
				// Set the new emphasis.
				this.emphasis = value;
				// Call the event handler.
				this.OnEmphasisChanged(old, value);
			}
		}

		/// <summary>
		/// Gets or sets the marker name.
		/// </summary>
		public override string Name
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the marker tag.
		/// </summary>
		public object Tag
		{
			get;
			set;
		}

		// Protected methods.

		/// <summary>
		/// An event handler called when the marker color has changed.
		/// </summary>
		protected virtual void OnColorChanged()
		{
			// Raise the event.
			if (this.ColorChanged != null) this.ColorChanged(this, new MapMarkerChangedEventArgs(this));
		}

		/// <summary>
		/// An event handler called when the marker coordinates have changed.
		/// </summary>
		/// <param name="oldLocation">The old coordinates.</param>
		/// <param name="newLocation">The new coordinates.</param>
		protected virtual void OnLocationChanged(MapPoint oldLocation, MapPoint newLocation)
		{
			// If the coordinates have not changed, do nothing.
			if (oldLocation == newLocation) return;
			// Raise the event.
			if (this.LocationChanged != null) this.LocationChanged(this, new MapMarkerChangedEventArgs(this));
		}

		/// <summary>
		/// An event handler called when the marker size has changed.
		/// </summary>
		/// <param name="oldSize">The old size.</param>
		/// <param name="newSize">The new size.</param>
		protected virtual void OnSizeChanged(Size oldSize, Size newSize)
		{
			// If the size has not changed, do nothing.
			if (oldSize == newSize) return;
			// Raise the event.
			if (this.SizeChanged != null) this.SizeChanged(this, new MapMarkerChangedEventArgs(this));
		}

		/// <summary>
		/// An event handler called when the marker emphasis has changed.
		/// </summary>
		/// <param name="oldEmphasis">The old emphasis.</param>
		/// <param name="newEmphasis">The new emphasis.</param>
		protected virtual void OnEmphasisChanged(bool oldEmphasis, bool newEmphasis)
		{
			// If the emphasis has not changed, do nothing.
			if (oldEmphasis == newEmphasis) return;
			// Raise the event.
			if (this.EmphasisChanged != null) this.EmphasisChanged(this, new MapMarkerChangedEventArgs(this));
		}
	}
}
