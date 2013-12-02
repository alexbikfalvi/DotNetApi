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
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace DotNetApi.Windows.Themes.Code
{
	/// <summary>
	/// A class representing a collection of tokens for coloring code.
	/// </summary>
	public abstract class CodeColorCollection : IEnumerable<CodeColorCollection.Token>
	{
		/// <summary>
		/// A structure representing a color collection token.
		/// </summary>
		public struct Token
		{
			/// <summary>
			/// Creates a new color collection token.
			/// </summary>
			/// <param name="regex">The token regular expression.</param>
			/// <param name="foregroundColor">The foreground color.</param>
			/// <param name="backgroundColor">The background color.</param>
			public Token(string regex, Color foregroundColor, Color backgroundColor)
			{
				this.Regex = regex;
				this.ForegroundColor = foregroundColor;
				this.BackgroundColor = backgroundColor;
			}

			// Properties.

			/// <summary>
			/// The token regular expression string.
			/// </summary>
			public string Regex { get; private set; }
			/// <summary>
			/// The token foreground color.
			/// </summary>
			public Color ForegroundColor { get; private set; }
			/// <summary>
			/// The token background color.
			/// </summary>
			public Color BackgroundColor { get; private set; }
		}

		// Public methods.

		/// <summary>
		/// Gets the enumerator for the code color collection.
		/// </summary>
		/// <returns>The enumerator.</returns>
		public IEnumerator<Token> GetEnumerator()
		{
			return this.Tokens.GetEnumerator() as IEnumerator<Token>;
		}

		/// <summary>
		/// Gets the enumerator for the code color collection.
		/// </summary>
		/// <returns>The enumerator.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Abstract methods.

		/// <summary>
		/// Gets the list of tokens.
		/// </summary>
		protected abstract Token[] Tokens { get; }
	}
}
