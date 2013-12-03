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
	public class CodeColorCollection : IEnumerable<CodeColorCollection.Token>
	{
		private readonly List<Token> tokens = new List<Token>();

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
			/// <param name="enforce">Indicates whether the token is always enforced. The default is <b>false</b>.</param>
			public Token(string regex, Color foregroundColor, Color backgroundColor, bool enforce = false)
				: this()
			{
				this.Regex = regex;
				this.ForegroundColor = foregroundColor;
				this.BackgroundColor = backgroundColor;
				this.Enforce = enforce;
			}

			// Properties.

			/// <summary>
			/// The token regular expression string.
			/// </summary>
			public string Regex { get; set; }
			/// <summary>
			/// The token foreground color.
			/// </summary>
			public Color ForegroundColor { get; set; }
			/// <summary>
			/// The token background color.
			/// </summary>
			public Color BackgroundColor { get; set; }
			/// <summary>
			/// Indicates whether the token is always enforced.
			/// </summary>
			public bool Enforce { get; set; }
		}

		/// <summary>
		/// Creates a new code collor collection with the specified list of tokens.
		/// </summary>
		public CodeColorCollection()
		{
		}

		// Public methods.

		/// <summary>
		/// Gets the enumerator for the code color collection.
		/// </summary>
		/// <returns>The enumerator.</returns>
		public IEnumerator<Token> GetEnumerator()
		{
			return this.tokens.GetEnumerator();
		}

		/// <summary>
		/// Gets the enumerator for the code color collection.
		/// </summary>
		/// <returns>The enumerator.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		/// <summary>
		/// Adds the specified token to the collection.
		/// </summary>
		/// <param name="token">The token.</param>
		public void Add(Token token)
		{
			this.tokens.Add(token);
		}

		/// <summary>
		/// Adds the specified range of tokens to the collection.
		/// </summary>
		/// <param name="tokens">The tokens.</param>
		public void AddRange(IEnumerable<Token> tokens)
		{
			this.tokens.AddRange(tokens);
		}
	}
}
