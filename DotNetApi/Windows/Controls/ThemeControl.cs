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
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DotNetApi.Windows.Themes;

namespace DotNetApi.Windows.Controls
{
	/// <summary>
	/// A class representing a theme user control.
	/// </summary>
	[Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(IDesigner))]
	public class ThemeControl : ThreadSafeControl
	{
		private readonly ThemeSettings themeSettings;

		private bool hasBorder = false;
		private bool hasTitle = false;
		private bool hasFocus = false;
		private string title = string.Empty;

		private Padding padding;

		public ThemeControl()
		{
			// Set the control style.
			base.SetStyle(
				ControlStyles.UserPaint |
				ControlStyles.OptimizedDoubleBuffer |
				ControlStyles.AllPaintingInWmPaint, true);

			base.AutoScaleMode = AutoScaleMode.Font;

			// Set the theme settings.
			this.themeSettings = ToolStripManager.Renderer is ThemeRenderer ? (ToolStripManager.Renderer as ThemeRenderer).Settings : ThemeSettings.Default;

			// Set the default properties.
			base.Padding = new Padding(0);
		}

		// Public events.

		/// <summary>
		/// An event raised when the control or any child control which is not a theme control gets focus.
		/// </summary>
		public event EventHandler AnyGotFocus;
		/// <summary>
		/// An event raised when the control or any child control which is not a theme control loses focus.
		/// </summary>
		public event EventHandler AnyLostFocus;
		/// <summary>
		/// An event raised when sending the focus to the parent control.
		/// </summary>
		public event EventHandler ChildGotFocus;
		/// <summary>
		/// An event raused when removing the focus from the parent control.
		/// </summary>
		public event EventHandler ChildLostFocus;

		// Public properties.

		/// <summary>
		/// Gets whether the control has focus.
		/// </summary>
		[Browsable(false)]
		public bool HasFocus
		{
			get { return this.hasFocus; }
		}
		/// <summary>
		/// Gets or sets whether the control uses a theme border.
		/// </summary>
		[Browsable(true)]
		[DisplayName("Show border")]
		[Description("Indicates whether the control uses a theme border.")]
		[DefaultValue(false)]
		public bool ShowBorder
		{
			get { return this.hasBorder; }
			set { this.OnSetHasBorder(value); }
		}
		/// <summary>
		/// Gets or sets whether the control uses a theme title.
		/// </summary>
		[Browsable(true)]
		[DisplayName("Show title")]
		[Description("Indicates whether the control uses a theme title.")]
		[DefaultValue(false)]
		public bool ShowTitle
		{
			get { return this.hasTitle; }
			set { this.OnSetHasTitle(value); }
		}
		/// <summary>
		/// Gets or sets whether the control title.
		/// </summary>
		[Browsable(true)]
		[DisplayName("Title")]
		[Description("The control title.")]
		public string Title
		{
			get { return this.title; }
			set { this.OnSetTitle(value); }
		}
		/// <summary>
		/// Gets or sets the control padding.
		/// </summary>
		[Browsable(false)]
		[DisplayName("Padding")]
		[Description("The control padding.")]
		protected new Padding Padding
		{
			get { return base.Padding; }
		}

		// Protected methods.

		/// <summary>
		/// Sets the padding for the theme control.
		/// </summary>
		/// <param name="padding">The padding.</param>
		protected void SetPadding(Padding padding)
		{
			base.Padding = (this.padding = padding);
		}

		/// <summary>
		/// An event handler called when the padding has changed.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnPaddingChanged(EventArgs e)
		{
			// If the padding is different from the padding set by the current control.
			if (base.Padding != padding)
			{
				// Correct the padding.
				base.Padding = padding;
			}
			// Call the base class method.
			base.OnPaddingChanged(e);
		}

		/// <summary>
		/// An event handler called when the control is being resized.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnResize(EventArgs e)
		{
			// If the control has title.
			if (this.hasTitle)
			{
				// Refresh the title.
				this.Invalidate(new Rectangle(this.ClientRectangle.Left, this.ClientRectangle.Top + 1, this.ClientRectangle.Width, this.themeSettings.PanelTitleHeight));
			}
			// If the control has a border.
			if (this.hasBorder)
			{
				// Refresh the control.
				this.Refresh();
			}
			// Call the base class methods.
			base.OnResize(e);
		}

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
						this.hasFocus ? this.themeSettings.ColorTable.PanelTitleSelectedGradientBegin : this.themeSettings.ColorTable.PanelTitleGradientBegin,
						this.hasFocus ? this.themeSettings.ColorTable.PanelTitleSelectedGradientEnd : this.themeSettings.ColorTable.PanelTitleGradientEnd,
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
					this.hasFocus ? this.themeSettings.ColorTable.PanelTitleSelectedText : this.themeSettings.ColorTable.PanelTitleText,
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
		/// An event handler called when a child control has been added.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnControlAdded(ControlEventArgs e)
		{
			// Call the base class method.
			base.OnControlAdded(e);
			// Subscribe to the child control events.
			this.OnSubscribeEvents(e.Control);
		}

		/// <summary>
		/// An event handler called when a child control has been removed.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnControlRemoved(ControlEventArgs e)
		{
			// Call the base classs method.
			base.OnControlRemoved(e);
			// Unsubscribe from the child control events.
			this.OnUnsubscribeEvents(e.Control);
		}

		/// <summary>
		/// An event handler called when the control got focus.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnGotFocus(EventArgs e)
		{
			// Call the base class method.
			base.OnGotFocus(e);
			// Call the any got focus handler.
			this.OnControlGotFocusChild(this, e);
		}

		/// <summary>
		/// An event handler called when the control lost focus.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnLostFocus(EventArgs e)
		{
			// Call the base class method.
			base.OnLostFocus(e);
			// Call the any lost focus handler.
			this.OnControlLostFocusChild(this, e);
		}

		/// <summary>
		/// An event handler called when the control got focus.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected virtual void OnAnyGotFocus(EventArgs e)
		{
			// Raise the event.
			if (null != this.AnyGotFocus) this.AnyGotFocus(this, e);
			// If the control has title.
			if (this.hasTitle)
			{
				// Refresh the title.
				this.Invalidate(new Rectangle(this.ClientRectangle.Left, this.ClientRectangle.Top + 1, this.ClientRectangle.Width, this.themeSettings.PanelTitleHeight));
			}
			else
			{
				// Send the focus to the parent control.
				if (null != this.ChildGotFocus) this.ChildGotFocus(this, e);
			}
		}

		/// <summary>
		/// An event handler called when the control lost focus.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected virtual void OnAnyLostFocus(EventArgs e)
		{
			// Raise the event.
			if (null != this.AnyLostFocus) this.AnyLostFocus(this, e);
			// If the control has title.
			if (this.hasTitle)
			{
				// Refresh the title.
				this.Invalidate(new Rectangle(this.ClientRectangle.Left, this.ClientRectangle.Top + 1, this.ClientRectangle.Width, this.themeSettings.PanelTitleHeight));
			}
			else
			{
				// Remove the focus to the parent control.
				if (null != this.ChildLostFocus) this.ChildLostFocus(this, e);
			}
		}

		/// <summary>
		/// An event handler called when the font has changed.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnFontChanged(EventArgs e)
		{
			// Call the base class method.
			base.OnFontChanged(e);
			// Update the control.
			this.OnUpdate();
		}

		// Private methods.

		/// <summary>
		/// Updates the control.
		/// </summary>
		private void OnUpdate()
		{
			// Update the padding.
			if (this.hasBorder) this.padding = new Padding(1, 1 + (this.hasTitle ? this.themeSettings.PanelTitleHeight + 1: 0), 1, 1);
			else if (this.hasTitle) this.padding = new Padding(0, 1 + this.themeSettings.PanelTitleHeight, 0, 0);
			else this.padding = new Padding(0);
			// Set the base class padding.
			base.Padding = this.padding;
			// Refresh the control.
			this.Refresh();
		}

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
			// Update the control.
			this.OnUpdate();
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
			// Update the control.
			this.OnUpdate();
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

		/// <summary>
		/// Subscribes to the events of a child control.
		/// </summary>
		/// <param name="control">The child control.</param>
		private void OnSubscribeEvents(Control control)
		{
			// If the control is a theme control.
			if (control is ThemeControl)
			{
				ThemeControl themeControl = control as ThemeControl;
				// Add peer control event handlers.
				themeControl.AnyGotFocus += this.OnControlGotFocusPeer;
				themeControl.ChildGotFocus += this.OnControlGotFocusChild;
				themeControl.ChildLostFocus += this.OnControlLostFocusChild;
			}
			else
			{
				// Add child control event handlers.
				control.GotFocus += this.OnControlGotFocusChild;
				control.LostFocus += this.OnControlLostFocusChild;
				control.ControlAdded += this.OnControlControlAdded;
				control.ControlRemoved += this.OnControlControlRemoved;

				// Subscribe to the events of the child controls.
				foreach (Control child in control.Controls)
				{
					this.OnSubscribeEvents(child);
				}
			}
		}

		/// <summary>
		/// Unsubscribes from the events of a child control.
		/// </summary>
		/// <param name="control">The child control.</param>
		private void OnUnsubscribeEvents(Control control)
		{
			// If the control is a theme control.
			if (control is ThemeControl)
			{
				ThemeControl themeControl = control as ThemeControl;
				// Remive peer control event handlers.
				themeControl.AnyGotFocus -= this.OnControlGotFocusPeer;
				themeControl.ChildGotFocus -= this.OnControlGotFocusChild;
				themeControl.ChildLostFocus -= this.OnControlLostFocusChild;
			}
			else
			{
				// Remove child control event handlers.
				control.GotFocus -= this.OnControlGotFocusChild;
				control.LostFocus -= this.OnControlLostFocusChild;
				control.ControlAdded -= this.OnControlControlAdded;
				control.ControlRemoved -= this.OnControlControlRemoved;

				// Unsubscribe from the events of the child controls.
				foreach (Control child in control.Controls)
				{
					this.OnUnsubscribeEvents(child);
				}
			}
		}
		
		/// <summary>
		/// An event handler called when a theme child control gets focus.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void OnControlGotFocusPeer(object sender, EventArgs e)
		{
			if (this.hasFocus)
			{
				// Set the flag to false.
				this.hasFocus = false;
				// Call the lost focus handler.
				this.OnAnyLostFocus(e);
			}
		}

		/// <summary>
		/// An event handler called when a child control got focus.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void OnControlGotFocusChild(object sender, EventArgs e)
		{
			if (!this.hasFocus)
			{
				// Set the flag to true.
				this.hasFocus = true;
				// Call the got focus handler.
				this.OnAnyGotFocus(e);
			}
		}

		/// <summary>
		/// An event handler called when a child control lost focus.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void OnControlLostFocusChild(object sender, EventArgs e)
		{
			if (!this.ContainsFocus)
			{
				// Set the flag to false.
				this.hasFocus = false;
				// Call the lost focus handler.
				this.OnAnyLostFocus(e);
			}
		}

		/// <summary>
		/// An event handler called when a child control added a control.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void OnControlControlAdded(object sender, ControlEventArgs e)
		{
			// Subscribe to the child control events.
			this.OnSubscribeEvents(e.Control);
		}

		/// <summary>
		/// An event handler called when a child control removed a control.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void OnControlControlRemoved(object sender, ControlEventArgs e)
		{
			// Unsubscribe from the child control events.
			this.OnUnsubscribeEvents(e.Control);
		}
	}
}
