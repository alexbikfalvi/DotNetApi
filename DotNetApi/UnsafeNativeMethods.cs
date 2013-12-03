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
using System.Runtime.InteropServices;

namespace DotNetApi
{
	/// <summary>
	/// A class with unsafe native methods.
	/// </summary>
	public static class UnsafeNativeMethods
	{
		/// <summary>
		/// Use the external Windows function to get the length of a Unicode zero-terminated string.
		/// </summary>
		/// <param name="str">The zero-terminated Unicode string.</param>
		/// <returns>The string length.</returns>
		[DllImport("kernel32.dll", EntryPoint="lstrlen", CharSet = CharSet.Unicode)]
		internal static extern unsafe int StrLen(char* str);

		/// <summary>
		/// A method to call directly the send message function of the Windows API.
		/// </summary>
		/// <param name="hWnd">The window handle.</param>
		/// <param name="msg">The message.</param>
		/// <param name="wParam">Parameter.</param>
		/// <param name="lParam">Parameter.</param>
		/// <returns>The operation response.</returns>
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		internal extern static IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
	}
}
