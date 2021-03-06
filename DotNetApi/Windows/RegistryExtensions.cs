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
using System.Security;
using Microsoft.Win32;
using DotNetApi;
using DotNetApi.Security;

namespace DotNetApi.Windows
{
	/// <summary>
	/// A class that simplifies access to the registry for common data types.
	/// </summary>
	public static class RegistryExtensions
	{
		// Static properties.

		/// <summary>
		/// Gets the HKEY_CURRENT_USER registry key.
		/// </summary>
		public static RegistryKey CurrentUser { get { return Microsoft.Win32.Registry.CurrentUser; } }

		// Static methods.

		/// <summary>
		/// Reads a boolean value from the registry.
		/// </summary>
		/// <param name="keyName">The key name.</param>
		/// <param name="valueName">The value name.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The value.</returns>
		public static bool GetBoolean(string keyName, string valueName, bool defaultValue)
		{
			try
			{
				object value = Microsoft.Win32.Registry.GetValue(keyName, valueName, defaultValue);
				return null != value ? Convert.ToBoolean(value) : defaultValue;
			}
			catch { return defaultValue; }
		}

		/// <summary>
		/// Reads a boolean value from the registry key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="name">The value name.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The value.</returns>
		public static bool GetBoolean(this RegistryKey key, string name, bool defaultValue)
		{
			try
			{
				object value = key.GetValue(name, defaultValue, RegistryValueOptions.None);
				return null != value ? Convert.ToBoolean(value) : defaultValue;
			}
			catch { return defaultValue; }
		}

		/// <summary>
		/// Writes a boolean value to the registry.
		/// </summary>
		/// <param name="keyName">The key name.</param>
		/// <param name="valueName">The value name.</param>
		/// <param name="value">The value.</param>
		public static void SetBoolean(string keyName, string valueName, bool value)
		{
			Microsoft.Win32.Registry.SetValue(keyName, valueName, value ? 1 : 0, RegistryValueKind.DWord);
		}

		/// <summary>
		/// Writes a boolean value to the registry key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="name">The value name.</param>
		/// <param name="value">The value.</param>
		public static void SetBoolean(this RegistryKey key, string name, bool value)
		{
			key.SetValue(name, value, RegistryValueKind.DWord);
		}

		/// <summary>
		/// Reads an integer value from the registry.
		/// </summary>
		/// <param name="keyName">The key name.</param>
		/// <param name="valueName">The value name.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The value.</returns>
		public static int GetInteger(string keyName, string valueName, int defaultValue)
		{
			try
			{
				object value = Microsoft.Win32.Registry.GetValue(keyName, valueName, defaultValue);
				return null != value ? Convert.ToInt32(value) : defaultValue;
			}
			catch { return defaultValue; }
		}

		/// <summary>
		/// Reads an integer value from the registry key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="name">The value name.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The value.</returns>
		public static int GetInteger(this RegistryKey key, string name, int defaultValue)
		{
			try
			{
				object value = key.GetValue(name, defaultValue, RegistryValueOptions.None);
				return null != value ? Convert.ToInt32(value) : defaultValue;
			}
			catch { return defaultValue; }
		}

		/// <summary>
		/// Writes an integer value to the registry.
		/// </summary>
		/// <param name="keyName">The key name.</param>
		/// <param name="valueName">The value name.</param>
		/// <param name="value">The value.</param>
		public static void SetInteger(string keyName, string valueName, int value)
		{
			Microsoft.Win32.Registry.SetValue(keyName, valueName, value, RegistryValueKind.DWord);
		}

		/// <summary>
		/// Writes an integer value to the registry key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="name">The value name.</param>
		/// <param name="value">The value.</param>
		public static void SetInteger(this RegistryKey key, string name, int value)
		{
			key.SetValue(name, value, RegistryValueKind.DWord);
		}

		/// <summary>
		/// Reads a string value from the registry.
		/// </summary>
		/// <param name="keyName">The key name.</param>
		/// <param name="valueName">The value name.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The value.</returns>
		public static string GetString(string keyName, string valueName, string defaultValue)
		{
			try
			{
				string value;
				return null != (value = Microsoft.Win32.Registry.GetValue(keyName, valueName, defaultValue) as string) ? value : defaultValue;
			}
			catch { return defaultValue; }
		}

		/// <summary>
		/// Reads a string value from the registry key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="name">The value name.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The value.</returns>
		public static string GetString(this RegistryKey key, string name, string defaultValue)
		{
			try
			{
				string value;
				return null != (value = key.GetValue(name, defaultValue, RegistryValueOptions.None) as string) ? value : defaultValue;
			}
			catch { return defaultValue; }
		}

		/// <summary>
		/// Writes a string value to the registry.
		/// </summary>
		/// <param name="keyName">The key name.</param>
		/// <param name="valueName">The value name.</param>
		/// <param name="value">The value.</param>
		public static void SetString(string keyName, string valueName, string value)
		{
			Microsoft.Win32.Registry.SetValue(keyName, valueName, value, RegistryValueKind.String);
		}

		/// <summary>
		/// Writes a string value to the registry key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="name">The value name.</param>
		/// <param name="value">The value.</param>
		public static void SetString(this RegistryKey key, string name, string value)
		{
			key.SetValue(name, value, RegistryValueKind.String);
		}

		/// <summary>
		/// Reads a multi-string value from the registry.
		/// </summary>
		/// <param name="keyName">The key name.</param>
		/// <param name="valueName">The value name.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The value.</returns>
		public static string[] GetMultiString(string keyName, string valueName, string[] defaultValue)
		{
			try
			{
				string[] value;
				return null != (value = Microsoft.Win32.Registry.GetValue(keyName, valueName, defaultValue) as string[]) ? value : defaultValue;
			}
			catch { return defaultValue; }
		}

		/// <summary>
		/// Reads a multi-string value from the registry key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="name">The value name.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The value.</returns>
		public static string[] GetMultiString(this RegistryKey key, string name, string[] defaultValue)
		{
			try
			{
				string[] value;
				return null != (value = key.GetValue(name, defaultValue, RegistryValueOptions.None) as string[]) ? value : defaultValue;
			}
			catch { return defaultValue; }
		}

		/// <summary>
		/// Writes a multi-string value to the registry.
		/// </summary>
		/// <param name="keyName">The key name.</param>
		/// <param name="valueName">The value name.</param>
		/// <param name="value">The value.</param>
		public static void SetMultiString(string keyName, string valueName, string[] value)
		{
			Microsoft.Win32.Registry.SetValue(keyName, valueName, value, RegistryValueKind.MultiString);
		}

		/// <summary>
		/// Writes a multi-string value to the registry key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="name">The value name.</param>
		/// <param name="value">The value.</param>
		public static void SetMultiString(this RegistryKey key, string name, string[] value)
		{
			key.SetValue(name, value, RegistryValueKind.MultiString);
		}

		/// <summary>
		/// Reads a secure string value from the registry.
		/// </summary>
		/// <param name="keyName">The key name.</param>
		/// <param name="valueName">The value name.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <param name="cryptoKey">The AES cryptographic key.</param>
		/// <param name="cryptoIV">The AES cryptographic initialization vector.</param>
		/// <returns>The value.</returns>
		public static SecureString GetSecureString(string keyName, string valueName, SecureString defaultValue, byte[] cryptoKey, byte[] cryptoIV)
		{
			try
			{
				SecureString value;
				return null != (value = (Microsoft.Win32.Registry.GetValue(keyName, valueName, null) as byte[]).DecryptSecureStringAes(cryptoKey, cryptoIV)) ? value : defaultValue;
			}
			catch (Exception) { return defaultValue; }
		}

		/// <summary>
		/// Reads a secure string value from the registry key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="name">The value name.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The value.</returns>
		public static SecureString GetSecureString(this RegistryKey key, string name, SecureString defaultValue, byte[] cryptoKey, byte[] cryptoIV)
		{
			try
			{
				SecureString value;
				return null != (value = (key.GetValue(name, defaultValue, RegistryValueOptions.None) as byte[]).DecryptSecureStringAes(cryptoKey, cryptoIV)) ? value : defaultValue;
			}
			catch { return defaultValue; }
		}

		/// <summary>
		/// Writes a secure string value to the registry.
		/// </summary>
		/// <param name="keyName">The key name.</param>
		/// <param name="valueName">The value name.</param>
		/// <param name="value">The value.</param>
		/// <param name="cryptoKey">The AES cryptographic key.</param>
		/// <param name="cryptoIV">The AES cryptographic initialization vector.</param>
		public static void SetSecureString(string keyName, string valueName, SecureString value, byte[] cryptoKey, byte[] cryptoIV)
		{
			Microsoft.Win32.Registry.SetValue(keyName, valueName, value.EncryptSecureStringAes(cryptoKey, cryptoIV), RegistryValueKind.Binary);
		}

		/// <summary>
		/// Writes a secure string value to the registry key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="name">The value name.</param>
		/// <param name="value">The value.</param>
		public static void SetSecureString(this RegistryKey key, string name, SecureString value, byte[] cryptoKey, byte[] cryptoIV)
		{
			key.SetValue(name, value.EncryptSecureStringAes(cryptoKey, cryptoIV), RegistryValueKind.Binary);
		}

		/// <summary>
		/// Reads a secure byte array value from the registry.
		/// </summary>
		/// <param name="keyName">The key name.</param>
		/// <param name="valueName">The value name.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <param name="cryptoKey">The AES cryptographic key.</param>
		/// <param name="cryptoIV">The AES cryptographic initialization vector.</param>
		/// <returns>The value.</returns>
		public static byte[] GetSecureByteArray(string keyName, string valueName, byte[] defaultValue, byte[] cryptoKey, byte[] cryptoIV)
		{
			try
			{
				byte[] value;
				return null != (value = (Microsoft.Win32.Registry.GetValue(keyName, valueName, null) as byte[]).DecryptAes(cryptoKey, cryptoIV)) ? value : defaultValue;
			}
			catch (Exception) { return defaultValue; }
		}

		/// <summary>
		/// Reads a secure byte array value from the registry key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="name">The value name.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The value.</returns>
		public static byte[] GetSecureByteArray(this RegistryKey key, string name, byte[] defaultValue, byte[] cryptoKey, byte[] cryptoIV)
		{
			try
			{
				byte[] value;
				return null != (value = (key.GetValue(name, defaultValue, RegistryValueOptions.None) as byte[]).DecryptAes(cryptoKey, cryptoIV)) ? value : defaultValue;
			}
			catch { return defaultValue; }
		}

		/// <summary>
		/// Writes a secure byte array value to the registry.
		/// </summary>
		/// <param name="keyName">The key name.</param>
		/// <param name="valueName">The value name.</param>
		/// <param name="value">The value.</param>
		/// <param name="cryptoKey">The AES cryptographic key.</param>
		/// <param name="cryptoIV">The AES cryptographic initialization vector.</param>
		public static void SetSecureByteArray(string keyName, string valueName, byte[] value, byte[] cryptoKey, byte[] cryptoIV)
		{
			Microsoft.Win32.Registry.SetValue(keyName, valueName, value.EncryptAes(cryptoKey, cryptoIV), RegistryValueKind.Binary);
		}

		/// <summary>
		/// Writes a secure byte array value to the registry key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="name">The value name.</param>
		/// <param name="value">The value.</param>
		public static void SetSecureByteArray(this RegistryKey key, string name, byte[] value, byte[] cryptoKey, byte[] cryptoIV)
		{
			key.SetValue(name, value.EncryptAes(cryptoKey, cryptoIV), RegistryValueKind.Binary);
		}

		/// <summary>
		/// Reads a date-time value from the registry.
		/// </summary>
		/// <param name="keyName">The key name.</param>
		/// <param name="valueName">The value name.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The value.</returns>
		public static DateTime GetDateTime(string keyName, string valueName, DateTime defaultValue)
		{
			try
			{
				object value = Microsoft.Win32.Registry.GetValue(keyName, valueName, defaultValue.Ticks);
				return null != value ? new DateTime(Convert.ToInt64(value)) : defaultValue;
			}
			catch { return defaultValue; }
		}

		/// <summary>
		/// Reads a date-time value from the registry key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="name">The value name.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The value.</returns>
		public static DateTime GetDateTime(this RegistryKey key, string name, DateTime defaultValue)
		{
			try
			{
				object value = key.GetValue(name, defaultValue.Ticks);
				return null != value ? new DateTime(Convert.ToInt64(value)) : defaultValue;
			}
			catch { return defaultValue; }
		}

		/// <summary>
		/// Writes a date-time value to the registry.
		/// </summary>
		/// <param name="keyName">The key name.</param>
		/// <param name="valueName">The value name.</param>
		/// <param name="value">The value.</param>
		public static void SetDateTime(string keyName, string valueName, DateTime value)
		{
			Microsoft.Win32.Registry.SetValue(keyName, valueName, value.Ticks, RegistryValueKind.QWord);
		}

		/// <summary>
		/// Writes a date-time value to the registry key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="name">The value name.</param>
		/// <param name="value">The value.</param>
		public static void SetDateTime(this RegistryKey key, string name, DateTime value)
		{
			key.SetValue(name, value.Ticks, RegistryValueKind.QWord);
		}

		/// <summary>
		/// Reads a time-span value from the registry.
		/// </summary>
		/// <param name="keyName">The key name.</param>
		/// <param name="valueName">The value name.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The value.</returns>
		public static TimeSpan GetTimeSpan(string keyName, string valueName, TimeSpan defaultValue)
		{
			try
			{
				object value = Microsoft.Win32.Registry.GetValue(keyName, valueName, defaultValue.Ticks);
				return null != value ? TimeSpan.FromTicks(Convert.ToInt64(value)) : defaultValue;
			}
			catch { return defaultValue; }
		}

		/// <summary>
		/// Reads a time-span value from the registry key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="name">The value name.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The value.</returns>
		public static TimeSpan GetTimeSpan(this RegistryKey key, string name, TimeSpan defaultValue)
		{
			try
			{
				object value = key.GetValue(name, defaultValue.Ticks);
				return null != value ? new TimeSpan(Convert.ToInt64(value)) : defaultValue;
			}
			catch { return defaultValue; }
		}

		/// <summary>
		/// Writes a time-span value to the registry.
		/// </summary>
		/// <param name="keyName">The key name.</param>
		/// <param name="valueName">The value name.</param>
		/// <param name="value">The value.</param>
		public static void SetTimeSpan(string keyName, string valueName, TimeSpan value)
		{
			Microsoft.Win32.Registry.SetValue(keyName, valueName, value.Ticks, RegistryValueKind.QWord);
		}

		/// <summary>
		/// Writes a time-span value to the registry key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="name">The value name.</param>
		/// <param name="value">The value.</param>
		public static void SetTimeSpan(this RegistryKey key, string name, TimeSpan value)
		{
			key.SetValue(name, value.Ticks, RegistryValueKind.QWord);
		}

		/// <summary>
		/// Reads a byte array value from the registry.
		/// </summary>
		/// <param name="keyName">The key name.</param>
		/// <param name="valueName">The value name.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The value.</returns>
		public static byte[] GetBytes(string keyName, string valueName, byte[] defaultValue)
		{
			try
			{
				byte[] value;
				return null != (value = Microsoft.Win32.Registry.GetValue(keyName, valueName, defaultValue) as byte[]) ? value : defaultValue;
			}
			catch (Exception) { return defaultValue; }
		}

		/// <summary>
		/// Reads a byte array value from the registry key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="name">The value name.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The value.</returns>
		public static byte[] GetBytes(this RegistryKey key, string name, byte[] defaultValue)
		{
			try
			{
				byte[] value;
				return null != (value = key.GetValue(name, defaultValue) as byte[]) ? value : defaultValue;
			}
			catch { return defaultValue; }
		}

		/// <summary>
		/// Writes a byte array value to the registry.
		/// </summary>
		/// <param name="keyName">The key name.</param>
		/// <param name="valueName">The value name.</param>
		/// <param name="value">The value.</param>
		public static void SetBytes(string keyName, string valueName, byte[] value)
		{
			Microsoft.Win32.Registry.SetValue(keyName, valueName, value, RegistryValueKind.Binary);
		}

		/// <summary>
		/// Writes a byte array value to the registry key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="name">The value name.</param>
		/// <param name="value">The value.</param>
		public static void SetByte(this RegistryKey key, string name, byte[] value)
		{
			key.SetValue(name, value, RegistryValueKind.Binary);
		}

		/// <summary>
		/// Reads a 32-bit integer array value from the registry.
		/// </summary>
		/// <param name="keyName">The key name.</param>
		/// <param name="valueName">The value name.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The value.</returns>
		public static Int32[] GetInt32Array(string keyName, string valueName, Int32[] defaultValue)
		{
			try
			{
				byte[] value;
				return null != (value = Microsoft.Win32.Registry.GetValue(keyName, valueName, defaultValue) as byte[]) ? value.ToInt32Array() : defaultValue;
			}
			catch (Exception) { return defaultValue; }
		}

		/// <summary>
		/// Reads a 32-bit integer array value from the registry key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="name">The value name.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The value.</returns>
		public static Int32[] GetInt32Array(this RegistryKey key, string name, Int32[] defaultValue)
		{
			try
			{
				byte[] value;
				return null != (value = key.GetValue(name, defaultValue) as byte[]) ? value.ToInt32Array() : defaultValue;
			}
			catch { return defaultValue; }
		}

		/// <summary>
		/// Writes a 32-bit integer array value to the registry.
		/// </summary>
		/// <param name="keyName">The key name.</param>
		/// <param name="valueName">The value name.</param>
		/// <param name="value">The value.</param>
		public static void SetInt32Array(string keyName, string valueName, Int32[] value)
		{
			Microsoft.Win32.Registry.SetValue(keyName, valueName, value.GetBytes(), RegistryValueKind.Binary);
		}

		/// <summary>
		/// Writes a 32-bit integer array value to the registry key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="name">The value name.</param>
		/// <param name="value">The value.</param>
		public static void SetInt32Array(this RegistryKey key, string name, Int32[] value)
		{
			key.SetValue(name, value.GetBytes(), RegistryValueKind.Binary);
		}

		/// <summary>
		/// Copies recursivelly the registry data from the source to the destination key.
		/// </summary>
		/// <param name="srcKey">The source key.</param>
		/// <param name="srcPath">The source path.</param>
		/// <param name="dstKey">The destination key.</param>
		/// <param name="dstPath">The detination path.</param>
		/// <param name="progress">A delegate to the progress.</param>
		/// <returns><b>True</b> if the copy was successful, <b>false</b> otherwise.</returns>
		public static bool CopyKey(RegistryKey srcKey, string srcPath, RegistryKey dstKey, string dstPath, Action<string, string> progress = null)
		{
			// Open the source key.
			using (RegistryKey src = srcKey.OpenSubKey(srcPath, RegistryKeyPermissionCheck.ReadWriteSubTree))
			{
				// If the source key is null, return.
				if (null == src) return false;

				// Create the destination key.
				using (RegistryKey dst = dstKey.CreateSubKey(dstPath, RegistryKeyPermissionCheck.ReadWriteSubTree))
				{
					// If the destination key is null, return.
					if (null == dst) return false;

					// If the progress delegate is not null, update the progress.
					if (null != progress) progress(src.Name, dst.Name);

					// Copy all values.
					foreach (string value in src.GetValueNames())
					{
						// Copy the values.
						dst.SetValue(value, src.GetValue(value), src.GetValueKind(value));
					}

					// Perform a recursive copy of the registry keys.
					foreach (string subkey in src.GetSubKeyNames())
					{
						// Copy the registry key.
						RegistryExtensions.CopyKey(src, subkey, dst, subkey, progress);
					}

					return true;
				}
			}
		}

		/// <summary>
		/// Copies recursivelly the registry data from the source to the destination key.
		/// </summary>
		/// <param name="srcKey">The source key.</param>
		/// <param name="srcPath">The source path.</param>
		/// <param name="dstKey">The destination key.</param>
		/// <param name="dstPath">The detination path.</param>
		/// <param name="progress">A delegate to the progress.</param>
		/// <returns><b>True</b> if the move was successful, <b>false</b> otherwise.</returns>
		public static bool MoveKey(RegistryKey srcKey, string srcPath, RegistryKey dstKey, string dstPath, Action<string, string> progress = null)
		{
			// Copy the key.
			if (RegistryExtensions.CopyKey(srcKey, srcPath, dstKey, dstPath, progress))
			{
				// Delete the old key.
				srcKey.DeleteSubKeyTree(srcPath);
				// Return true.
				return true;
			}
			// Return false.
			return false;
		}
	}
}
