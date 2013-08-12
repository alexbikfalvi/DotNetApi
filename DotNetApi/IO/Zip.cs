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
using System.IO.Compression;

namespace DotNetApi.IO
{
	/// <summary>
	/// A class used to handle ZIP compressed data.
	/// </summary>
	public static class Zip
	{
		/// <summary>
		/// Decompresses the data from the specified buffer.
		/// </summary>
		/// <param name="data">The compressed data.</param>
		/// <returns>A buffer with the uncompressed data.</returns>
		public static byte[] Unzip(this byte[] data)
		{
			using (MemoryStream memoryStream = new MemoryStream(data))
			{
				using (GZipStream zipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
				{
					return zipStream.Read
				}
			}
		}
	}
}
