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
	/// A delegate representing a warning for an I/O operation.
	/// </summary>
	/// <param name="source">The source.</param>
	/// <param name="destination">The destination.</param>
	/// <param name="query">The operation query.</param>
	/// <returns>The operation result: <b>true</b> if the operation must continue o</returns>
	public delegate bool OperationWarningAction(string source, string destination, Operations.OperationWarning warning);

	/// <summary>
	/// A delegate representing the outcome of an I/O operation.
	/// </summary>
	/// <param name="source">The source.</param>
	/// <param name="destination">The destination.</param>
	public delegate void OperationProgressAction(string source, string destination);

	/// <summary>
	/// A class used for I/O operations.
	/// </summary>
	public static class Operations
	{
		private static readonly OperationWarningAction actionAlwaysContinue = (string source, string destination, Operations.OperationWarning warning) =>
			{
				return true;
			};
		private static readonly OperationWarningAction actionAlwaysStop = (string source, string destination, Operations.OperationWarning warning) =>
			{
				return false;
			};
		private static readonly OperationWarningAction actionDontOverwriteIgnoreErrors = (string source, string destination, Operations.OperationWarning warning) =>
		{
			return warning == OperationWarning.Exists ? false : true;
		};

		/// <summary>
		/// An enumeration representing the I/O operation request.
		/// </summary>
		public enum OperationWarning
		{
			Exists,
			Unauthorized,
			Other
		}

		// Public properties.

		/// <summary>
		/// Gets an always continue warning action.
		/// </summary>
		public static OperationWarningAction AlwaysContinue { get { return Operations.actionAlwaysContinue; } }
		/// <summary>
		/// Gets an always stop warning action.
		/// </summary>
		public static OperationWarningAction AlwaysStop { get { return Operations.actionAlwaysStop; } }
		/// <summary>
		/// Gets a don't overwrite and ignore errors action.
		/// </summary>
		public static OperationWarningAction DontOverwriteIgnoreErrors { get { return Operations.actionDontOverwriteIgnoreErrors; } }
	}
}
