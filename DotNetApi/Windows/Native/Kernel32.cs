﻿/* 
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
using System.Runtime.InteropServices;

namespace DotNetApi.Windows.Native
{
	/// <summary>
	/// A class with native methods.
	/// </summary>
	public static partial class NativeMethods
	{
		private const string dllKernel32 = "kernel32.dll";

		/// <summary>
		/// Use the external Windows function to get the length of a Unicode zero-terminated string.
		/// </summary>
		/// <param name="str">The zero-terminated Unicode string.</param>
		/// <returns>The string length.</returns>
		[DllImport(NativeMethods.dllKernel32, EntryPoint = "lstrlen", CharSet = CharSet.Unicode)]
		internal static extern unsafe int StrLen(char* str);
	}
}
