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
using System.Windows.Forms;
using DotNetApi.Windows.Themes;

namespace DotNetApi.Windows.Controls
{
	/// <summary>
	/// A tool split container control.
	/// </summary>
	public class ToolSplitContainer : SplitContainer
	{
		private bool panel1Border = true;
		private bool panel2Border = true;

		private readonly ThemeColorTable colorTable;

		/// <summary>
		/// Creates a new control instance.
		/// </summary>
		public ToolSplitContainer()
		{
			// Set the theme color table.
			this.colorTable = ToolStripManager.Renderer is ThemeRenderer ? (ToolStripManager.Renderer as ThemeRenderer).ColorTable : ThemeColorTable.DefaultColorTable;

			this.Panel1.Padding = new Padding(1);
			this.Panel2.Padding = new Padding(1);

			this.Panel1.Paint += this.OnPanelPaint;
			this.Panel2.Paint += this.OnPanelPaint;

			this.SplitterWidth = 5;
		}

		// Public properties.

		/// <summary>
		/// Gets or sets whether the panel 1 border is enabled.
		/// </summary>
		[DisplayName("Panel1Border"), Description("Indicates whether the panel has a border."), DefaultValue(true)]
		public bool Panel1Border
		{
			get { return this.panel1Border; }
			set { this.OnPanel1BorderSet(value); }
		}

		/// <summary>
		/// Gets or sets whether the panel 1 border is enabled.
		/// </summary>
		[DisplayName("Panel2Border"), Description("Indicates whether the panel has a border."), DefaultValue(true)]
		public bool Panel2Border
		{
			get { return this.panel2Border; }
			set { this.OnPanel2BorderSet(value); }
		}

		// Protected methods.

		protected override void OnPaint(PaintEventArgs e)
		{
			// Create a brush.
			using (SolidBrush brush = new SolidBrush(this.colorTable.ToolStripContentPanelGradientBegin))
			{
				// Fill the content .
				e.Graphics.FillRectangle(brush, new Rectangle(new Point(0, 0), this.ClientSize));
			}
			// Call the base class.
			base.OnPaint(e);
		}

		// Private methods.

		/// <summary>
		/// An event handler called when painting a splitter panel.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void OnPanelPaint(object sender, PaintEventArgs e)
		{
			// Draw the border.
			ControlPaint.DrawBorder(e.Graphics, e.ClipRectangle, this.colorTable.ToolSplitContainerBorder, ButtonBorderStyle.Solid);
		}

		/// <summary>
		/// Sets whether the panel 1 border is enabled.
		/// </summary>
		/// <param name="value">The enabled state.</param>
		private void OnPanel1BorderSet(bool value)
		{
			// If the enabled state has not changed, do nothing.
			if (value == this.panel1Border) return;
			// Set the enabled state.
			this.panel1Border = value;
			// If the border is enabled.
			if (this.panel1Border)
			{
				this.Panel1.Padding = new Padding(1);
				this.Panel1.Paint += this.OnPanelPaint;
			}
			else
			{
				this.Panel1.Padding = new Padding(0);
				this.Panel1.Paint -= this.OnPanelPaint;
			}
			// Refresh the control.
			this.Refresh();
		}

		/// <summary>
		/// Sets whether the panel 2 border is enabled.
		/// </summary>
		/// <param name="value">The enabled state.</param>
		private void OnPanel2BorderSet(bool value)
		{
			// If the enabled state has not changed, do nothing.
			if (value == this.panel2Border) return;
			// Set the enabled state.
			this.panel2Border = value;
			// If the border is enabled.
			if (this.panel2Border)
			{
				this.Panel2.Padding = new Padding(1);
				this.Panel2.Paint += this.OnPanelPaint;
			}
			else
			{
				this.Panel2.Padding = new Padding(0);
				this.Panel2.Paint -= this.OnPanelPaint;
			}
			// Refresh the control.
			this.Refresh();
		}
	}
}
