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
using System.IO;
using System.Windows.Forms;

namespace DotNetApi.Windows
{
	/// <summary>
	/// A class for mouse cursors. 
	/// </summary>
	public static class Cursors
	{
		/// <summary>
		/// Static constructor to create the mouse cursors.
		/// </summary>
		static Cursors()
		{
			Cursors.HandGrab = Cursors.FromBytes(Resources.CursorHandGrab);
		}

		// Public properties.

		/// <summary>
		/// Gets a hand cursor.
		/// </summary>
		public static Cursor HandGrab { get; private set; }

		// Private methods.

		/// <summary>
		/// Reads a mouse cursor from the specified byte array.
		/// </summary>
		/// <param name="data">The byte array data.</param>
		/// <returns>The mouse cursor.</returns>
		private static Cursor FromBytes(byte[] data)
		{
			// Use a memory stream to create the cursor.
			using (MemoryStream memoryStream = new MemoryStream(data))
			{
				return new Cursor(memoryStream);
			}
		}
	}
}
