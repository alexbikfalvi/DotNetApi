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

namespace DotNetApi.Windows.Native
{
	/// <summary>
	/// A class with native methods.
	/// </summary>
	public static partial class NativeMethods
	{
		internal const string dllUser32 = "user32.dll";

		/// <summary>
		/// A method to call directly the <b>SendMessage</b> function of the Windows API.
		/// </summary>
		/// <param name="hWnd">The window handle.</param>
		/// <param name="msg">The message.</param>
		/// <param name="wParam">Parameter.</param>
		/// <param name="lParam">Parameter.</param>
		/// <returns>The operation response.</returns>
		[DllImport(NativeMethods.dllUser32, CharSet = CharSet.Auto)]
		internal static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

		/// <summary>
		/// A method to call directly the <b>SendMessage</b> function of the Windows API.
		/// </summary>
		/// <param name="hWnd">The window handle.</param>
		/// <param name="msg">The message.</param>
		/// <param name="wParam">Parameter.</param>
		/// <param name="lParam">A string parameter.</param>
		/// <returns>The operation response.</returns>
		[DllImport(NativeMethods.dllUser32, CharSet = CharSet.Unicode, SetLastError = false)]
		internal static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);
		
		/// <summary>
		/// A methid to call directly the <b>GetActiveWindow</b> function of the Windows API.
		/// </summary>
		/// <returns>The handle of the active window.</returns>
		[DllImport(NativeMethods.dllUser32, CharSet = CharSet.Unicode, ExactSpelling = true)]
		internal static extern IntPtr GetActiveWindow();

		/// <summary>
		/// Changes the text of the specified window's title bar.
		/// </summary>
		/// <param name="hWnd">A handle to the window or control whose text is to be changed.</param>
		/// <param name="lpString">The new title or control text.</param>
		/// <returns>If the function succeeds, the return value is nonzero.</returns>
		[DllImport(NativeMethods.dllUser32, CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool SetWindowText(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string lpString);
	}
}
