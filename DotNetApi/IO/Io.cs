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
	public static class Io
	{
		private const uint bufferSize = 65536;

		/// <summary>
		/// Reads all the data from the specified stream into a buffer.
		/// </summary>
		/// <param name="stream">The input stream.</param>
		/// <returns>The output buffer data.</returns>
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
				byte[] readBuffer = new byte[Io.bufferSize];
				// The output buffer.
				byte[] outputBuffer = null;

				// Read the stream data into the buffer.
				for (int bytesRead; (bytesRead = stream.Read(readBuffer, 0, readBuffer.Length)) > 0; )
				{
					// Allocate a new output buffer.
					byte[] tempBuffer = new byte[(outputBuffer != null ? outputBuffer.Length : 0) + bytesRead];
					// Copy the previous output buffer.
					if (null != outputBuffer)
					{
						Buffer.BlockCopy(outputBuffer, 0, tempBuffer, 0, outputBuffer.Length);
					}
					// 
				}

				// Return the output buffer.
				return outputBuffer;
			}
			finally
			{
				if (stream.CanSeek)
				{
					stream.Position = originalPosition;
				}
			}
		}
	}
}
