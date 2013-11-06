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

namespace DotNetApi.IO
{
	/// <summary>
	/// A class used for file operations.
	/// </summary>
	public static class File
	{
		/// <summary>
		/// Moves a file from the source path to the destination path.
		/// </summary>
		/// <param name="srcPath">The source file.</param>
		/// <param name="dstPath">The destination file.</param>
		/// <param name="warning">A warning action handler.</param>
		/// <param name="progress">An optional progress action handler.</param>
		/// <returns><b>True</b> if the move was successful, <b>false</b> otherwise.</returns>
		public static bool Move(string srcPath, string dstPath, OperationWarningAction warning, OperationProgressAction progress = null)
		{
			// Validate the arguments.
			if (null == srcPath) throw new ArgumentNullException("srcPath");
			if (null == dstPath) throw new ArgumentNullException("dstPath");
			if (null == warning) throw new ArgumentNullException("warning");

			// Check the source file exists.
			if (!System.IO.File.Exists(srcPath)) return false;
			// If the source path and destination path are the same, return true.
			if (srcPath == dstPath) return true;
			// If the source path is included in the destination path, return false.
			if (dstPath.Contains(srcPath)) return false;

			try
			{
				// Call the action handler.
				if (null != progress) progress(srcPath, dstPath);

				// If the destination file exists.
				if (System.IO.File.Exists(dstPath))
				{
					// Ask whether the file should be overwritten.
					if (warning(srcPath, dstPath, Operations.OperationWarning.Exists))
					{
						// Delete the destination file.
						System.IO.File.Delete(dstPath);
					}
					else return true; // Otherwise, return true.
				}

				// Move the file.
				System.IO.File.Move(srcPath, dstPath);
			}
			catch (UnauthorizedAccessException)
			{
				return warning(srcPath, dstPath, Operations.OperationWarning.Unauthorized);
			}
			catch (Exception)
			{
				return warning(srcPath, dstPath, Operations.OperationWarning.Other);
			}

			// Return true.
			return true;
		}
	}
}
