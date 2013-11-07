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
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DotNetApi.Windows.Themes;

namespace DotNetApi.Windows.Controls
{
	/// <summary>
	/// A class representing a theme panel control.
	/// </summary>
	public class ThemeControl : ThreadSafeControl
	{
		private readonly ThemeSettings themeSettings;

		private bool hasBorder = false;
		private bool hasTitle = false;
		private string title = string.Empty;

		public ThemeControl()
		{
			// Set the theme settings.
			this.themeSettings = ToolStripManager.Renderer is ThemeRenderer ? (ToolStripManager.Renderer as ThemeRenderer).Settings : ThemeSettings.Default;

			// Set the default properties.
			this.Padding = new Padding(0);
		}

		// Public properties.

		/// <summary>
		/// Gets or sets whether the control uses a theme border.
		/// </summary>
		[DisplayName("Show border"), Description("Indicates whether the control uses a theme border."), DefaultValue(false)]
		public bool ShowBorder
		{
			get { return this.hasBorder; }
			set { this.OnSetHasBorder(value); }
		}
		/// <summary>
		/// Gets or sets whether the control uses a theme title.
		/// </summary>
		[DisplayName("Show title"), Description("Indicates whether the control uses a theme title."), DefaultValue(false)]
		public bool ShowTitle
		{
			get { return this.hasTitle; }
			set { this.OnSetHasTitle(value); }
		}
		/// <summary>
		/// Gets or sets whether the control title.
		/// </summary>
		[DisplayName("Title"), Description("The control title.")]
		public string Title
		{
			get { return this.title; }
			set { this.OnSetTitle(value); }
		}

		// Protected methods.

		/// <summary>
		/// An event handler used to paint the control.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			// If the title is displayed.
			if (this.hasTitle)
			{
				Rectangle rect = new Rectangle(this.ClientRectangle.Left, this.ClientRectangle.Top + 1, this.ClientRectangle.Width, this.themeSettings.PanelTitleHeight);

				using (Pen pen = new Pen(this.themeSettings.ColorTable.ToolSplitContainerBorder))
				{
					using (Brush brush = new LinearGradientBrush(
						rect,
						this.ContainsFocus ? this.themeSettings.ColorTable.PanelTitleSelectedGradientBegin : this.themeSettings.ColorTable.PanelTitleGradientBegin,
						this.ContainsFocus ? this.themeSettings.ColorTable.PanelTitleSelectedGradientEnd : this.themeSettings.ColorTable.PanelTitleGradientEnd,
						LinearGradientMode.Vertical))
					{
						e.Graphics.FillRectangle(brush, rect);
						if (this.hasBorder)
						{
							e.Graphics.DrawLine(pen, rect.Left, rect.Bottom, rect.Right, rect.Bottom);
						}
						else
						{
							e.Graphics.DrawLine(pen, rect.Left, rect.Top - 1, rect.Right, rect.Top - 1);
							e.Graphics.DrawLine(pen, rect.Left, rect.Top - 1, rect.Left, rect.Bottom);
							e.Graphics.DrawLine(pen, rect.Right - 1, rect.Top - 1, rect.Right - 1, rect.Bottom);
						}
					}
				}

				Rectangle rectText = new Rectangle(this.ClientRectangle.Left + 5, this.ClientRectangle.Top + 1, this.ClientRectangle.Width - 5, this.themeSettings.PanelTitleHeight - 1);

				TextRenderer.DrawText(
					e.Graphics,
					this.title,
					new System.Drawing.Font(Window.DefaultFont, this.themeSettings.PanelTitleFontStyle),
					rectText,
					this.ContainsFocus ? this.themeSettings.ColorTable.PanelTitleSelectedText : this.themeSettings.ColorTable.PanelTitleText,
					TextFormatFlags.EndEllipsis | TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
			}
			// If the border is displayed.
			if (this.hasBorder)
			{
				// Draw the border.
				ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle, this.themeSettings.ColorTable.ToolSplitContainerBorder, ButtonBorderStyle.Solid);
			}
			// Call the base class methods.
			base.OnPaint(e);
		}

		/// <summary>
		/// An event handler called when the control got focus.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnAnyGotFocus(EventArgs e)
		{
			// Call the base class method.
			base.OnAnyGotFocus(e);
			// If the control has title.
			if (this.hasTitle)
			{
				// Refresh the title.
				this.Invalidate(new Rectangle(this.ClientRectangle.Left, this.ClientRectangle.Top + 1, this.ClientRectangle.Width, this.themeSettings.PanelTitleHeight));
			}
		}

		/// <summary>
		/// An event handler called when the control lost focus.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnAnyLostFocus(EventArgs e)
		{
			// Call the base class method.
			base.OnAnyLostFocus(e);
			// If the control has title.
			if (this.hasTitle)
			{
				// Refresh the title.
				this.Invalidate(new Rectangle(this.ClientRectangle.Left, this.ClientRectangle.Top + 1, this.ClientRectangle.Width, this.themeSettings.PanelTitleHeight));
			}
		}

		// Private methods.

		/// <summary>
		/// Sets whether the control displays a theme border.
		/// </summary>
		/// <param name="value"><b>True</b> if the border is displayed, <b>false</b> otherwise.</param>
		private void OnSetHasBorder(bool value)
		{
			// If the border has not changed, do nothing.
			if (this.hasBorder == value) return;
			// Set the border.
			this.hasBorder = value;
			// Update the padding.
			if (this.hasBorder) this.Padding = new Padding(1, 1 + (this.hasTitle ? this.themeSettings.PanelTitleHeight : 0), 1, 1);
			else if (this.hasTitle) this.Padding = new Padding(0, 1 + this.themeSettings.PanelTitleHeight, 0, 0);
			else this.Padding = new Padding(0);
			// Refresh the control.
			this.Refresh();
		}

		/// <summary>
		/// Sets whether the control displays a theme title.
		/// </summary>
		/// <param name="value"><b>True</b> if the title is displayed, <b>false</b> otherwise.</param>
		private void OnSetHasTitle(bool value)
		{
			// If the title has not changed, do nothing.
			if (this.hasTitle == value) return;
			// Set the title.
			this.hasTitle = value;
			// Update the padding.
			if (this.hasBorder) this.Padding = new Padding(1, 1 + (this.hasTitle ? this.themeSettings.PanelTitleHeight : 0), 1, 1);
			else if (this.hasTitle) this.Padding = new Padding(0, 1 + this.themeSettings.PanelTitleHeight, 0, 0);
			else this.Padding = new Padding(0);
			// Refresh the control.
			this.Refresh();
		}

		/// <summary>
		/// Sets the current title.
		/// </summary>
		/// <param name="value">The title.</param>
		private void OnSetTitle(string value)
		{
			// If the title has not changed, do nothing.
			if (this.title == value) return;
			// Set the title.
			this.title = value;
			// Refresh the title.
			this.Invalidate(new Rectangle(this.ClientRectangle.Left, this.ClientRectangle.Top, this.ClientRectangle.Width, this.themeSettings.PanelTitleHeight));
		}
	}
}
