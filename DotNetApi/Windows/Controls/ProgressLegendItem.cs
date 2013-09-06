/* 
 * Copyright (C) 2012-2013 Alex Bikfalvi
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
	/// A class representing a progress legend item.
	/// </summary>
	[DesignTimeVisible(false)]
	public sealed class ProgressLegendItem : Component
	{
		private string text = null;
		private Color color = new Color();

		/// <summary>
		/// Creates a new item instance.
		/// </summary>
		public ProgressLegendItem()
		{
		}

		// Public events.

		/// <summary>
		/// An event raised when the legend item text has changed.
		/// </summary>
		public event ProgressLegendItemEventHandler TextChanged;
		/// <summary>
		/// An event raised when the legent item color has changed.
		/// </summary>
		public event ProgressLegendItemEventHandler ColorChanged;

		// Public properties.

		/// <summary>
		/// Gets or sets the legend item text.
		/// </summary>
		public string Text
		{
			get { return this.text; }
			set
			{
				// Set the text.
				this.text = value;
				// Call the text changed event handler.
				this.OnTextChanged();
			}
		}
		/// <summary>
		/// Gets or sets the legend item color.
		/// </summary>
		public Color Color
		{
			get { return this.color; }
			set
			{
				// Set the color.
				this.color = value;
				// Call the color changed event handler.
				this.OnColorChanged();
			}
		}

		// Private methods.

		/// <summary>
		/// An event handler called when the legend item text has changed.
		/// </summary>
		private void OnTextChanged()
		{
			// Raise the event.
			if (this.TextChanged != null) this.TextChanged(this, new ProgressLegendItemEventArgs(this));
		}

		/// <summary>
		/// An event handler called when the legend item color has changed.
		/// </summary>
		private void OnColorChanged()
		{
			// Raise the event.
			if (this.ColorChanged != null) this.ColorChanged(this, new ProgressLegendItemEventArgs(this));
		}
	}
}
