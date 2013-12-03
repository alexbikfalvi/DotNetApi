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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DotNetApi.Collections.Generic;
using DotNetApi.Windows.Themes.Code;

namespace DotNetApi.Windows.Controls
{
	/// <summary>
	/// Represents a code text box control.
	/// </summary>
	public sealed class CodeTextBox : RichTextBox
	{
		/// <summary>
		/// An enumeration representing the token bound.
		/// </summary>
		private enum TokenBound
		{
			Begin = 0,
			End = 1
		}

		private const int wmSetRedraw = 0x000B;
		private const int wmUser = 0x400;
		private const int emGetEventMask = (wmUser + 59);
		private const int emSetEventMask = (wmUser + 69);

		private Color defaultForegroundColor = Color.Black;
		private Color defaultBackgroundColor = Color.White;
		private CodeColorCollection colorCollection = null;

		private readonly SortedMap<int, TokenBound> tokenBounds = new SortedMap<int, TokenBound>();

		/// <summary>
		/// Creates a new code box instance.
		/// </summary>
		public CodeTextBox()
		{
			// Set the control style.
			base.SetStyle(
				ControlStyles.OptimizedDoubleBuffer |
				ControlStyles.AllPaintingInWmPaint, true);

			// Get the collection of currently installed fonts.
			InstalledFontCollection collection = new InstalledFontCollection();
			
			// Get the code font family.
			FontFamily fontFamily = collection.Families.FirstOrDefault((FontFamily family) =>
				{
					return family.Name == "Consolas";
				});
			// Set the font.
			this.Font = new Font(fontFamily != null ? fontFamily : FontFamily.GenericMonospace, 10);
			
			// Set the default colors.
			this.ForeColor = defaultForegroundColor;
			this.BackColor = defaultBackgroundColor;
		}

		// Public methods.

		/// <summary>
		/// Gets or sets the color collection.
		/// </summary>
		[DisplayName("ColorCollection"), Description("The color collection used to color the code tokens."), Category("Colors")]
		public CodeColorCollection ColorCollection
		{
			get { return this.colorCollection; }
			set { this.OnSetColorCollection(value); }
		}
		/// <summary>
		/// Gets or sets the text default foreground color.
		/// </summary>
		[DisplayName("DefaultForegroundColor"), Description("The default foreground color."), Category("Colors")]
		public Color DefaultForegroundColor
		{
			get { return this.defaultForegroundColor; }
			set { this.OnSetDefaultForegroundColor(value); }
		}
		/// <summary>
		/// Gets or sets the text default background color.
		/// </summary>
		[DisplayName("DefaultBackgroundColor"), Description("The default background color."), Category("Colors")]
		public Color DefaultBackgroundColor
		{
			get { return this.defaultBackgroundColor; }
			set { this.OnSetDefaultBackgroundColor(value); }
		}

		// Protected methods.

		/// <summary>
		/// An event handler called when the text has changed.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnTextChanged(EventArgs e)
		{
			// Refresh the colors.
			this.OnRefreshColors();
			// Call the base class event handler.
			base.OnTextChanged(e);
		}

		// Private methods.

		/// <summary>
		/// Sets the current color collection.
		/// </summary>
		/// <param name="collection">The color collection.</param>
		private void OnSetColorCollection(CodeColorCollection collection)
		{
			// If the collection has not changed, do nothing.
			if (this.colorCollection == collection) return;
			
			// Else, set the collection.
			this.colorCollection = collection;
			
			// Update the code color.
			this.OnRefreshColors();
		}

		/// <summary>
		/// Sets the default foreground color.
		/// </summary>
		/// <param name="color">The color.</param>
		private void OnSetDefaultForegroundColor(Color color)
		{
			// If the color has not changed, do nothing.
			if (this.defaultForegroundColor == color) return;

			// Set the color.
			this.defaultForegroundColor = color;

			// Update the code color.
			this.OnRefreshColors();
		}

		/// <summary>
		/// Sets the default background color.
		/// </summary>
		/// <param name="color">The color.</param>
		public void OnSetDefaultBackgroundColor(Color color)
		{
			// If the color has not changed, do nothing.
			if (this.defaultBackgroundColor == color) return;

			// Set the color.
			this.defaultBackgroundColor = color;

			// Update the code color.
			this.OnRefreshColors();
		}

		/// <summary>
		/// Refreshes the colors
		/// </summary>
		private void OnRefreshColors()
		{
			// If the color collection is null, do nothing.
			if (null == this.colorCollection) return;

			// If the text is empty, do nothing.
			if (string.IsNullOrWhiteSpace(this.Text)) return;

			// Save the cursor position and selection.
			int selectionStart = this.SelectionStart;
			int selectionLength = this.SelectionLength;

			// The event mask.
			IntPtr eventMask = IntPtr.Zero;

			try
			{
				// Stop redrawing.
				UnsafeNativeMethods.SendMessage(this.Handle, CodeTextBox.wmSetRedraw, new IntPtr(0), IntPtr.Zero);
				// Stop sending of events.
				eventMask = UnsafeNativeMethods.SendMessage(this.Handle, CodeTextBox.emGetEventMask, new IntPtr(0), IntPtr.Zero);

				// Set the color for all text to the default color.
				this.SelectAll();
				this.SelectionColor = this.defaultForegroundColor;
				this.SelectionBackColor = this.defaultBackgroundColor;

				// Clear the token bounds.
				this.tokenBounds.Clear();

				// Set the token colors.
				foreach (CodeColorCollection.Token token in this.colorCollection)
				{
					// Find the matches for the current token.
					MatchCollection matches = Regex.Matches(this.Text, token.Regex);
					// For each match.
					foreach (Match match in matches)
					{
						// Find the token bound equal or greater to the start position.
						TokenBound bound;

						// If the token is not enforced, if such a value is found and the value is an end bound, skip the token
						if ((!token.Enforce) && this.tokenBounds.TryLowerBound(match.Index, out bound) && (bound == TokenBound.End)) continue;

						// Select the corresponding text.
						this.Select(match.Index, match.Length);
						// Color the text selection.
						this.SelectionColor = token.ForegroundColor;
						this.SelectionBackColor = token.BackgroundColor;

						// Add the bounds to the token bounds.
						if (!token.Enforce)
						{
							this.tokenBounds.Add(match.Index, TokenBound.Begin);
							if (match.Length > 1)
							{
								this.tokenBounds.Add(match.Index + match.Length - 1, TokenBound.End);
							}
						}
					}
				}
			}
			finally
			{
				// Turn on events.
				UnsafeNativeMethods.SendMessage(this.Handle, CodeTextBox.emSetEventMask, new IntPtr(0), eventMask);
				// Turn on redrawing.
				UnsafeNativeMethods.SendMessage(this.Handle, CodeTextBox.wmSetRedraw, new IntPtr(1), IntPtr.Zero);
			}

			// Restore the cursor position and selection.
			this.Select(selectionStart, selectionLength);
		}
	}
}
