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
			// Set the buffer original position.
			long originalPosition = 0;

			// If the stream allows seek.
			if (stream.CanSeek)
			{
				// Save the original position.
				originalPosition = stream.Position;
				// Set the stream position to zero.
				stream.Position = 0;
			}

			try
			{
				// Create a new read buffer.
				byte[] readBuffer = new byte[IoUtils.bufferSize];
				// The output buffer.
				byte[] outputBuffer = null;
			
				// Read the stream data into the buffer.
				for (int bytesRead, length = 0; (bytesRead = stream.Read(readBuffer, 0, readBuffer.Length)) > 0; )
				{
					// Resize the output buffer to append the data read.
					Array.Resize<byte>(ref outputBuffer, length + bytesRead);
					// Copy the bytes read to the output buffer.
					Buffer.BlockCopy(readBuffer, 0, outputBuffer, length, bytesRead);
					// Save the length of the output buffer.
					length = outputBuffer.Length;
				}

				// Return the output buffer.
				return outputBuffer;
			}
			finally
			{
				// If can seek in the stream.
				if (stream.CanSeek)
				{
					// Restore the original stream position.
					stream.Position = originalPosition;
				}
			}
		}
	}
}
