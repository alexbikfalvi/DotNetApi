using System;
using System.Windows.Forms;

namespace DotNetApi.Windows.Themes
{
	/// <summary>
	/// The themes renderer.
	/// </summary>
	public class ThemeRenderer : ToolStripProfessionalRenderer
	{
		public ThemeRenderer(ProfessionalColorTable colorTable)
			: base(colorTable)
		{
			this.RoundedEdges = false;
		}
	}
}
