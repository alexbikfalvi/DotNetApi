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

		/// <summary>
		/// An event handler called when the text has changed.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnTextChanged(EventArgs e)
		{
			// Call the base class event handler.
			base.OnTextChanged(e);
		}
	}
}
