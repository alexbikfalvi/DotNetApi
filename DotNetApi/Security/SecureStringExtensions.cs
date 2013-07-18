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
using System.Security;

namespace DotNetApi.Security
{
	/// <summary>
	/// A class with extensions for secure string objects.
	/// </summary>
	public static class SecureStringExtensions
	{
		/// <summary>
		/// Represents an empty secure string. This fiels is read-only.
		/// </summary>
		public static readonly SecureString Empty = new SecureString();

		public static bool IsEqual(this SecureString left, SecureString right)
		{
			// Check the argumenta are not null.
			if (null == left) throw new ArgumentNullException("left");
			if (null == right) throw new ArgumentNullException("right");

			// Create an unmanaged string to store the secure string data.
			IntPtr ptrLeft = IntPtr.Zero;
			IntPtr ptrRight = IntPtr.Zero;

			try
			{
				// Get the secure string data into the unmanaged buffers.
				ptrLeft = Marshal.SecureStringToGlobalAllocUnicode(left);
				ptrRight = Marshal.SecureStringToGlobalAllocUnicode(right);
				// Compare the two buffers.
				unsafe
				{
					for (Char* ptr1 = (Char*)ptrLeft.ToPointer(), ptr2 = (Char*)ptrRight.ToPointer(); 
						*ptr1 != 0 && *ptr2 != 0;
						ptr1++, ptr2++)
					{
						if (*ptr1 != *ptr2)
						{
							return false;
						}
					}  
				}
			}
			finally
			{
				// Release the unmanaged buffers.
				if (IntPtr.Zero != ptrLeft)
				{
					Marshal.ZeroFreeGlobalAllocUnicode(ptrLeft);
				}
				if (IntPtr.Zero != ptrRight)
				{
					Marshal.ZeroFreeGlobalAllocUnicode(ptrRight);
				}
			}
			return true;
		}

		/// <summary>
		/// Returns whether the specified secure string is empty.
		/// </summary>
		/// <param name="secureString">The secure string.</param>
		/// <returns><b>True</b> if the secure string is empty, or false otherwise.</returns>
		public static bool IsEmpty(this SecureString secureString)
		{
			return secureString.Length == 0;
		}

		/// <summary>
		/// Converts the secure string into an unsecure string.
		/// </summary>
		/// <param name="secureString">The secure string to convert.</param>
		/// <returns>The unsecure string.</returns>
		public static string ConvertToUnsecureString(this SecureString secureString)
		{
			// Check the argument is not null.
			if (null == secureString) throw new ArgumentNullException("secureString");

			// Create an unmanaged string to store the secure string data.
			IntPtr unmanagedString = IntPtr.Zero;

			try
			{
				// Get the secure string data into the unmanaged buffer.
				unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureString);
				// Return the data from the unmanaged buffer as a string.
				return Marshal.PtrToStringUni(unmanagedString);
			}
			finally
			{
				// Release the unmanaged buffer.
				Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
			}
		}

		/// <summary>
		/// Converts the unsecure string into a secure string.
		/// </summary>
		/// <param name="value">The unsecure string.</param>
		/// <returns>The secure string.</returns>
		public static SecureString ConvertToSecureString(this string value)
		{
			// If the value is null, throw an exception.
			if (null == value) throw new ArgumentNullException("value");

			unsafe
			{
				// Create a fixed buffer with the characters of the string value.
				fixed (char* chars = value)
				{
					// Create the secure string.
					SecureString secureString = new SecureString(chars, value.Length);

					// Make the secure string read-only.
					secureString.MakeReadOnly();

					// Return the secure string.
					return secureString;
				}
			}
		}
	}
}
