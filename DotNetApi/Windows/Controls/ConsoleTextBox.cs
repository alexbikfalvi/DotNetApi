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

namespace DotNetApi.Windows.Controls
{
	/// <summary>
	/// A control representing a console text box.
	/// </summary>
	public sealed class ConsoleTextBox : RichTextBox
	{
		private Color defaultForegroundColor = Color.LightGray;
		private Color defaultBackgroundColor = Color.Black;

		/// <summary>
		/// Create a new console text box instance.
		/// </summary>
		public ConsoleTextBox()
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

			// Set the colors.
			this.ForeColor = this.defaultForegroundColor;
			this.BackColor = this.defaultBackgroundColor;
		}

		#region Public properties

		/// <summary>
		/// Gets or sets the text default foreground color.
		/// </summary>
		[DisplayName("DefaultForegroundColor"), Description("The default foreground color."), Category("Colors")]
		public Color DefaultForegroundColor
		{
			get { return this.defaultForegroundColor; }
			set { this.defaultForegroundColor = value; }
		}
		/// <summary>
		/// Gets or sets the text default background color.
		/// </summary>
		[DisplayName("DefaultBackgroundColor"), Description("The default background color."), Category("Colors")]
		public Color DefaultBackgroundColor
		{
			get { return this.defaultBackgroundColor; }
			set { this.defaultBackgroundColor = value; }
		}

		#endregion

		#region Public methods

		/// <summary>
		/// Appends the specified text to the console text area using the default color.
		/// </summary>
		/// <param name="text">The text.</param>
		public new void AppendText(string text)
		{
			this.AppendText(this.defaultForegroundColor, text);
		}

		/// <summary>
		/// Appends the specified text to the console text area using the given color.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="color">The color.</param>
		public void AppendText(Color color, string text)
		{
			// Save the start position.
			int start = base.TextLength;
			// Append the text.
			base.AppendText(text);
			// Select the text.
			base.Select(start, text.Length);
			// Set the color.
			base.SelectionColor = color;
			// Clear the selection.
			base.Select(this.TextLength, 0);
		}

		/// <summary>
		/// Appends the specified formatted text to the console text area using the default color.
		/// </summary>
		/// <param name="format">The text format.</param>
		/// <param name="args">The arguments.</param>
		public void AppendText(string format, params object[] parameters)
		{
			this.AppendText(this.defaultForegroundColor, format.FormatWith(parameters));
		}

		/// <summary>
		/// Appends the specified formatted text to the console text area using the given color.
		/// </summary>
		/// <param name="format">The text format.</param>
		/// <param name="args">The arguments.</param>
		public void AppendText(Color color, string format, params object[] parameters)
		{
			this.AppendText(color, format.FormatWith(parameters));
		}

		/// <summary>
		/// Appends a line to the console text area.
		/// </summary>
		public void AppendLine()
		{
			base.AppendText(Environment.NewLine);
		}

		/// <summary>
		/// Appends the specified line to the console text area using the default color.
		/// </summary>
		/// <param name="text">The text.</param>
		public void AppendLine(string text)
		{
			this.AppendText(this.defaultForegroundColor, text + Environment.NewLine);
		}

		/// <summary>
		/// Appends the specified line to the console text area using the default color.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="color">The color.</param>
		public void AppendLine(Color color, string text)
		{
			this.AppendText(color, text + Environment.NewLine);
		}

		/// <summary>
		/// Appends the specified formatted text to the console text area using the given color.
		/// </summary>
		/// <param name="format">The text format.</param>
		/// <param name="args">The arguments.</param>
		public void AppendLine(Color color, string format, params object[] parameters)
		{
			this.AppendText(color, format.FormatWith(parameters) + Environment.NewLine);
		}

		/// <summary>
		/// Appends the specified line to the console text area using the default color.
		/// </summary>
		/// <param name="text">The text.</param>
		public void AppendDoubleLine(string text)
		{
			this.AppendText(this.defaultForegroundColor, text + Environment.NewLine + Environment.NewLine);
		}

		/// <summary>
		/// Appends the specified line to the console text area using the default color.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="color">The color.</param>
		public void AppendDoubleLine(Color color, string text)
		{
			this.AppendText(color, text + Environment.NewLine + Environment.NewLine);
		}

		#endregion
	}
}
