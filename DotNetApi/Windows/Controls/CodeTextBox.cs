using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace DotNetApi.Windows.Controls
{
	/// <summary>
	/// Represents a code text box control.
	/// </summary>
	public sealed class CodeTextBox : RichTextBox
	{
		public CodeTextBox()
		{
			// Get the collection of currently installed fonts.
			InstalledFontCollection collection = new InstalledFontCollection();
			// Set the color.
			this.ForeColor = Color.Blue;
			// Check if the "Consolas" font is installed.
			foreach (FontFamily fontFamily in collection.Families)
			{
				if (fontFamily.Name == "Consolas")
				{
					this.Font = new Font(fontFamily, 10);
					return;
				}
			}
			// Else, set the default monospace font.
			this.Font = new Font(FontFamily.GenericMonospace, 10);
		}
	}
}
