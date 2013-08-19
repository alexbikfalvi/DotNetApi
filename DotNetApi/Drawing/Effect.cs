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
	/// The base class for drawing effects.
	/// </summary>
	public abstract class Effect : IDisposable
	{
		private bool disposed = false;

		// Public properties.

		public bool IsDisposed { get { return this.disposed; } }

		// Public methods.

		/// <summary>
		/// Disposes the current object.
		/// </summary>
		public void Dispose()
		{
			// Set the disposed flag to true.
			this.disposed = true;
			// Call the event handler.
			this.OnDisposed();
		}

		/// <summary>
		/// Draws the effect on the specified graphics object and at the given rectangle.
		/// </summary>
		/// <param name="graphics">The graphics object.</param>
		/// <param name="rectangle">The rectangle.</param>
		internal abstract void Draw(Graphics graphics, Rectangle rectangle);

		// Protected methods.

		/// <summary>
		/// An event handler called when the object is being disposed.
		/// </summary>
		protected virtual void OnDisposed()
		{
		}
	}
}
