using System;
using System.Globalization;

namespace DotNetApi
{
	/// <summary>
	/// A class used for fast validation.
	/// </summary>
	public static class Validation
	{
		/// <summary>
		/// Validates whether the specified object is not null.
		/// </summary>
		/// <param name="value">The object.</param>
		/// <param name="name">The object name.</param>
		public static void ValidateNotNull(this object value, string name)
		{
			if (value == null) throw new ArgumentNullException(name);
		}
	}
}
