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
using System.Security;
using Microsoft.Win32;
using DotNetApi;
using DotNetApi.Security;

namespace DotNetApi.Windows
{
	/// <summary>
	/// A class that simplifies access to the registry for common data types.
	/// </summary>
	public class Registry
	{
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
		/// Writes an integer value to the registry.
		/// </summary>
		/// <param name="keyName">The key name.</param>
		/// <param name="valueName">The value name.</param>
		/// <param name="value">The value.</param>
		public static void SetBoolean(string keyName, string valueName, bool value)
		{
			Microsoft.Win32.Registry.SetValue(keyName, valueName, value ? 1 : 0, RegistryValueKind.DWord);
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
			catch (Exception) { return defaultValue; }
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
			catch (Exception) { return defaultValue; }
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
		/// Writes an array value to the registry.
		/// </summary>
		/// <param name="keyName">The key name.</param>
		/// <param name="valueName">The value name.</param>
		/// <param name="value">The value.</param>
		public static void SetInt32Array(string keyName, string valueName, Int32[] value)
		{
			Microsoft.Win32.Registry.SetValue(keyName, valueName, value.GetBytes(), RegistryValueKind.Binary);
		}
	}
}
