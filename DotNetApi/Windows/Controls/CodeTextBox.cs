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
using System.Windows.Forms;
using DotNetApi.Windows.Themes.Code;

namespace DotNetApi.Windows.Controls
{
	/// <summary>
	/// Represents a code text box control.
	/// </summary>
	public sealed class CodeTextBox : RichTextBox
	{
		private CodeColorCollection colorCollection = null;

		/// <summary>
		/// Creates a new code box instance.
		/// </summary>
		public CodeTextBox()
		{
			// Get the collection of currently installed fonts.
			InstalledFontCollection collection = new InstalledFontCollection();
			// Get the code font family.
			FontFamily fontFamily = collection.Families.FirstOrDefault((FontFamily family) =>
				{
					return family.Name == "Consolas";
				});
			// Set the font.
			this.Font = new Font(fontFamily != null ? fontFamily : FontFamily.GenericMonospace, 10);
		}

		// Public methods.

		/// <summary>
		/// Gets or sets the color collection.
		/// </summary>
		[DisplayName("ColorCollection"), Description("The color collection used to color the code tokens.")]
		public CodeColorCollection ColorCollection
		{
			get { return this.colorCollection; }
			set { this.OnSetColorCollection(value); }
		}

		// Protected methods.

		/// <summary>
		/// An event handler called when the text has changed.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnTextChanged(EventArgs e)
		{
			// Call the base class event handler.
			base.OnTextChanged(e);
		}

		// Private methods.

		/// <summary>
		/// Sets the current color collection.
		/// </summary>
		/// <param name="collection"></param>
		private virtual void OnSetColorCollection(CodeColorCollection collection)
		{

		}
	}
}
