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
using System.Text;
using DotNetApi;
using DotNetApi.Windows.Native;

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
					for (char* ptr1 = (char*)ptrLeft.ToPointer(), ptr2 = (char*)ptrRight.ToPointer(); 
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
		/// Inserts the specified string text at a position inside the secure string.
		/// </summary>
		/// <param name="secureString">The secure string.</param>
		/// <param name="index">The index.</param>
		/// <param name="text">The text to insert.</param>
		public static void InsertAt(this SecureString secureString, int index, string text)
		{
			for (int idx = 0; idx < text.Length; idx++)
			{
				secureString.InsertAt(index + idx, text[idx]);
			}
		}

		/// <summary>
		/// Removes the specified number of characters from the secure string, starting at the given index..
		/// </summary>
		/// <param name="secureString">The secure string.</param>
		/// <param name="index">The index from where to remove characters.</param>
		/// <param name="count">The number of characters to remove.</param>
		public static void Remove(this SecureString secureString, int index, int count)
		{
			for (int idx = 0; idx < count; idx++)
			{
				secureString.RemoveAt(index);
			}
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

		/// <summary>
		/// Converts the secure string into an unsecure string.
		/// </summary>
		/// <param name="secureString">The secure string to convert.</param>
		/// <returns>The unsecure string.</returns>
		public static byte[] ConvertToUnsecureByteArray(this SecureString secureString, Encoding encoding)
		{
			// Check the argument is not null.
			if (null == secureString) throw new ArgumentNullException("secureString");

			// Create an unmanaged string to store the secure string data.
			IntPtr unmanagedString = IntPtr.Zero;

			try
			{
				// Get the secure string data into the unmanaged buffer.
				unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureString);
				// Enter an unsafe context. 
				unsafe
				{
					// Get a pointer to the null terminated unmanaged string.
					char* chars = (char*)unmanagedString.ToPointer();
					// Get the length of the null terminated unmanaged string.
					int charCount = NativeMethods.StrLen(chars);
					// Get the number of bytes needed for the byte array.
					int byteCount = encoding.GetByteCount(chars, secureString.Length);
					// Create a byte array to contain the unsecure data.
					byte[] bytes = new byte[byteCount];
					// If the byte count is greater than zero.
					if (byteCount > 0)
					{
						// Use a fixed pointer to the byte array.
						fixed (byte* ptr = bytes)
						{
							// Encode the string characters into the byte array.
							encoding.GetBytes(chars, charCount, ptr, byteCount);
						}
					}
					// Return the byte array.
					return bytes;
				}
			}
			finally
			{
				// Release the unmanaged buffer.
				Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
			}
		}
	}
}
