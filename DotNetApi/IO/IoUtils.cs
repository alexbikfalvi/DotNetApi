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

namespace DotNetApi.IO
{
	/// <summary>
	/// A class with input-output utility methods.
	/// </summary>
	public static class IoUtils
	{
		private const uint bufferSize = 0x10000;

		/// <summary>
		/// Reads all the data from the specified stream into a buffer.
		/// </summary>
		/// <param name="stream">The input stream.</param>
		/// <returns>The output buffer data or <b>null</b> if no data was read.</returns>
		public static byte[] ReadToEnd(this Stream stream)
		{
			// Create a new read buffer.
			byte[] buffer = new byte[IoUtils.bufferSize];
			// Create a new memory stream.
			using (MemoryStream memoryStream = new MemoryStream())
			{
				// Read the stream data into the buffer.
				for (int bytesRead; (bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0; )
				{
					// Write the buffer data to the memory stream.
					memoryStream.Write(buffer, 0, bytesRead);
				}

				// Return the stream bytes.
				return memoryStream.ToArray();
			}
		}
	}
}
