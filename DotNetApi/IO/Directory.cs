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
using System.Linq;

namespace DotNetApi.IO
{
	/// <summary>
	/// A class used for directory operations.
	/// </summary>
	public static class Directory
	{
		// Public methods.

		/// <summary>
		/// Indicates whether a directory is empty.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <returns><b>True</b> if the directory is empty, <b>false</b> otherwise.</returns>
		public static bool IsEmpty(string path)
		{
			// Validate the arguments.
			if (null == path) throw new ArgumentNullException("path");
			// Return whether the dierctory contains any entries.
			return !System.IO.Directory.EnumerateFileSystemEntries(path).Any();
		}

		/// <summary>
		/// Moves a directory from the source path to the destination path.
		/// </summary>
		/// <param name="srcPath">The source directory.</param>
		/// <param name="dstPath">The destination directory.</param>
		/// <param name="warning">A warning action handler.</param>
		/// <param name="progress">An optional progress action handler.</param>
		/// <returns><b>True</b> if the move was successful, <b>false</b> otherwise.</returns>
		public static bool Move(string srcPath, string dstPath, OperationWarningAction warning, OperationProgressAction progress = null)
		{
			// Validate the arguments.
			if (null == srcPath) throw new ArgumentNullException("srcPath");
			if (null == dstPath) throw new ArgumentNullException("dstPath");
			if (null == warning) throw new ArgumentNullException("warning");

			// Check the source directory exists.
			if (!System.IO.Directory.Exists(srcPath)) return false;
			// If the source path and destination path are the same, return true.
			if (srcPath == dstPath) return true;
			// If the source path is included in the destination path, return false.
			if (dstPath.Contains(srcPath)) return false;

			try
			{
				// If the destination directory does not exist.
				if (!System.IO.Directory.Exists(dstPath))
				{
					// Try reate the destination directory.
					System.IO.Directory.CreateDirectory(dstPath);
				}

				// Call the action handler.
				if (null != progress) progress(srcPath, dstPath);

				// Move all files from the source directory to the destination directory.
				foreach (string filePath in System.IO.Directory.EnumerateFiles(srcPath))
				{
					// Move the file to the destination directory.
					if (!File.Move(filePath, System.IO.Path.Combine(dstPath, System.IO.Path.GetFileName(filePath)), warning, progress)) return false;
				}

				// For all directories from the source directory.
				foreach (string dirPath in System.IO.Directory.EnumerateDirectories(srcPath))
				{
					// Move the directory to the destination directory.
					if (!Directory.Move(dirPath, System.IO.Path.Combine(dstPath, System.IO.Path.GetFileName(dirPath)), warning, progress)) return false;
				}

				// If the directory is empty.
				if (Directory.IsEmpty(srcPath))
				{
					// Delete the directory.
					System.IO.Directory.Delete(srcPath, false);
				}
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

		/// <summary>
		/// A method than ensures the directory for a file exists.
		/// </summary>
		/// <param name="fileName">The file name.</param>
		/// <returns><b>True</b> if the directory exists.</returns>
		public static bool EnsureFileDirectoryExists(string fileName)
		{
			// Gets the directory name,
			string path = System.IO.Path.GetDirectoryName(fileName);
			// If the directory does not exist.
			if (!System.IO.Directory.Exists(path))
			{
				System.IO.Directory.CreateDirectory(path);
			}
			// Return true.
			return true;
		}
	}
}
