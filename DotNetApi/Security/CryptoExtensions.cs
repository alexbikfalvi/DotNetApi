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
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace DotNetApi.Security
{
	/// <summary>
	/// A class adding string cryptographic extensions.
	/// </summary>
	public static class CryptoExtensions
	{
		/// <summary>
		/// Encrypts the specified byte array using the AES algorithm.
		/// </summary>
		/// <param name="bytes">The byte array.</param>
		/// <param name="key">The cryptographic key.</param>
		/// <param name="iv">The cryptographic initialization vector.</param>
		/// <returns>The encrypted data buffer.</returns>
		public static byte[] EncryptAes(this byte[] bytes, byte[] key, byte[] iv)
		{
			// Create an AES cryptographic service provider and transform.
			using (AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider())
			{
				ICryptoTransform cryptoTransform = aesProvider.CreateEncryptor(key, iv);

				// Create a memory stream to store the encrypted data. 
				using (MemoryStream encryptedStream = new MemoryStream())
				{
					// Create an encryption stream.
					using (CryptoStream cryptStream = new CryptoStream(encryptedStream, cryptoTransform, CryptoStreamMode.Write))
					{
						// Encrypt the data to the encrypted stream.
						cryptStream.Write(bytes, 0, bytes.Length);
						cryptStream.FlushFinalBlock();

						// Set the position of the encrypted stream to zero.
						encryptedStream.Position = 0;

						// Read the encrypted data inot the result buffer.
						byte[] result = new byte[encryptedStream.Length];
						encryptedStream.Read(result, 0, (int)encryptedStream.Length);

						// Return the result byte array buffer.
						return result;
					}
				}
			}
		}

		/// <summary>
		/// Encrypts the specified string using the AES algorithm.
		/// </summary>
		/// <param name="value">The string to encrypt.</param>
		/// <param name="key">The cryptographic key.</param>
		/// <param name="iv">The cryptographic initialization vector.</param>
		/// <returns>The encrypted data buffer.</returns>
		public static byte[] EncryptStringAes(this string value, byte[] key, byte[] iv)
		{
			// If the buffer to decrypt is null, return null.
			if (null == value) return null;

			return Encoding.UTF8.GetBytes(value).EncryptAes(key, iv);
		}

		/// <summary>
		/// Encrypts the specified secure string using the AES algorithm.
		/// </summary>
		/// <param name="value">The secure string.</param>
		/// <param name="key">The cryptographic key.</param>
		/// <param name="iv">The cryptographic initialization vector.</param>
		/// <returns>The encrypted data buffer.</returns>
		public static byte[] EncryptSecureStringAes(this SecureString value, byte[] key, byte[] iv)
		{
			// If the string to encrypt is null, return null.
			if (null == value) return null;

			// Create an unmanaged string to store the secure string data.
			IntPtr unmanagedString = IntPtr.Zero;

			try
			{
				// Get the secure string data into the unmanaged buffer.
				unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(value);

				// Convert the string to a byte array using UTF8 encoding.
				return Encoding.UTF8.GetBytes(Marshal.PtrToStringUni(unmanagedString)).EncryptAes(key, iv);
			}
			finally
			{
				// Release the unmanaged buffer.
				Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
			}
		}

		/// <summary>
		/// Decrypts the specified byte array buffer using the AES algorithm.
		/// </summary>
		/// <param name="value">The encrypted data.</param>
		/// <param name="key">The cryptographic key.</param>
		/// <param name="iv">The cryptographic initialization vector.</param>
		/// <returns>The decrypted byte array.</returns>
		public static byte[] DecryptAes(this byte[] value, byte[] key, byte[] iv)
		{
			// If the buffer to decrypt is null, return null.
			if (null == value) return null;
			
			// Create a new AES cryptographic provider.
			using (AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider())
			{
				// Create the cryptographic transform for the specified key and initialization vector.
				ICryptoTransform cryptoTransform = aesProvider.CreateDecryptor(key, iv);

				// Create a memory stream to store the decrypted result.
				using (MemoryStream decryptedStream = new MemoryStream())
				{
					// Create a cryptographic stream to decript the data.
					using (CryptoStream cryptStream = new CryptoStream(decryptedStream, cryptoTransform, CryptoStreamMode.Write))
					{
						// Decrypt the data.
						cryptStream.Write(value, 0, value.Length);
						cryptStream.FlushFinalBlock();

						// Reset the position of the decrypted buffer to zero.
						decryptedStream.Position = 0;

						// Create a new byte array buffer to store the result.
						byte[] result = new byte[decryptedStream.Length];
						decryptedStream.Read(result, 0, (int)decryptedStream.Length);

						// Return the result.
						return result;
					}
				}
			}
		}

		/// <summary>
		/// Decrypts the specified byte array buffer using the AES algorithm.
		/// </summary>
		/// <param name="value">The byte array buffer to decrypt.</param>
		/// <param name="key">The cryptographic key.</param>
		/// <param name="iv">The cryptographic initialization vector.</param>
		/// <returns>The decrypted string.</returns>
		public static string DecryptStringAes(this byte[] value, byte[] key, byte[] iv)
		{
			// If the buffer to decrypt is null, return null.
			if (null == value) return null;
			
			return value.DecryptSecureStringAes(key, iv).ConvertToUnsecureString();
		}

		/// <summary>
		/// Decrypts the specified byte array buffer using the AES algorithm.
		/// </summary>
		/// <param name="value">The encrypted data.</param>
		/// <param name="key">The cryptographic key.</param>
		/// <param name="iv">The cryptographic initialization vector.</param>
		/// <returns>The decrypted secure string.</returns>
		public static SecureString DecryptSecureStringAes(this byte[] value, byte[] key, byte[] iv)
		{
			// If the buffer to decrypt is null, return null.
			if (null == value) return null;

			// Return the decrypted data converted to string using UTF8 encoding.
			return Encoding.UTF8.GetString(value.DecryptAes(key, iv)).ConvertToSecureString();
		}
	}
}
