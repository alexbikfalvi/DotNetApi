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
using System.Windows.Forms;

namespace DotNetApi.Windows.Controls
{
	/// <summary>
	/// A class representing a tab page.
	/// </summary>
	public sealed class WizardPage : Panel
	{
		private bool nextEnabled = false;
		private bool backEnabled = true;
		private string nextText = "&Next";
		private string backText = "&Back";
		private string title = string.Empty;
		private string status = string.Empty;

		/// <summary>
		/// Creates a new wizard page instance.
		/// </summary>
		public WizardPage()
		{
			// Set the default properties.
			base.Padding = new Padding(3);
			base.Visible = false;
			base.Dock = DockStyle.Fill;
		}

		// Public properties.

		/// <summary>
		/// Gets or sets whether the next button is enabled.
		/// </summary>
		[DefaultValue(false)]
		public bool NextEnabled
		{
			get { return this.nextEnabled; }
			set { this.OnNextEnabledChanged(value); }
		}
		/// <summary>
		/// Gets or sets whether the back button is enabled.
		/// </summary>
		[DefaultValue(false)]
		public bool BackEnabled
		{
			get { return this.backEnabled; }
			set { this.OnBackEnabledChanged(value); }
		}
		/// <summary>
		/// Gets or sets the text of the next button.
		/// </summary>
		[DefaultValue("&Next")]
		public string NextText
		{
			get { return this.nextText; }
			set { this.OnNextTextChanged(value); }
		}
		/// <summary>
		/// Gets or sets the text of the back button.
		/// </summary>
		[DefaultValue("&Back")]
		public string BackText
		{
			get { return this.backText; }
			set { this.OnBackTextChanged(value); }
		}
		/// <summary>
		/// Gets or sets the wizard page title.
		/// </summary>
		public string Title
		{
			get { return this.title; }
			set { this.OnTitleChanged(value); }
		}
		/// <summary>
		/// Gets or sets the wizard page status.
		/// </summary>
		public string Status
		{
			get { return this.status; }
			set { this.OnStatusChanged(value); }
		}
		/// <summary>
		/// Gets whether the wizard page is visible.
		/// </summary>
		[Browsable(false)]
		public new bool Visible
		{
			get { return base.Visible; }
			set { /* Do nothing */ }
		}
		/// <summary>
		/// Gets the wizard page padding.
		/// </summary>
		[Browsable(false)]
		public new Padding Padding
		{
			get { return base.Padding; }
			set { /* Do nothing */ }
		}
		// Public events.

		/// <summary>
		/// An event raised when the page has changed.
		/// </summary>
		public event WizardPageEventHandler PageChanged;

		// Private methods.

		/// <summary>
		/// An event handler called when the enabled state of the next button has changed.
		/// </summary>
		/// <param name="enabled">The enabled state.</param>
		public void OnNextEnabledChanged(bool enabled)
		{
			// Set the enabled state.
			this.nextEnabled = enabled;
			// Raise the event.
			if (null != this.PageChanged) this.PageChanged(this, new WizardPageEventArgs(this));
		}

		/// <summary>
		/// An event handler called when the enabled state of the back button has changed.
		/// </summary>
		/// <param name="enabled">The enabled state.</param>
		public void OnBackEnabledChanged(bool enabled)
		{
			// Set the enabled state.
			this.backEnabled = enabled;
			// Raise the event.
			if (null != this.PageChanged) this.PageChanged(this, new WizardPageEventArgs(this));
		}

		/// <summary>
		/// An event handler called when the text of the next button has changed.
		/// </summary>
		/// <param name="text">The text.</param>
		public void OnNextTextChanged(string text)
		{
			// Set the button text.
			this.nextText = text;
			// Raise the event.
			if (null != this.PageChanged) this.PageChanged(this, new WizardPageEventArgs(this));
		}

		/// <summary>
		/// An event handler called when the text of the back button has changed.
		/// </summary>
		/// <param name="text"></param>
		public void OnBackTextChanged(string text)
		{
			// Set the button text.
			this.backText = text;
			// Raise the event.
			if (null != this.PageChanged) this.PageChanged(this, new WizardPageEventArgs(this));
		}

		/// <summary>
		/// An event handler called when the page title has changed.
		/// </summary>
		/// <param name="title">The title.</param>
		public void OnTitleChanged(string title)
		{
			// Set the title.
			this.title = title;
			// Raise the event.
			if (null != this.PageChanged) this.PageChanged(this, new WizardPageEventArgs(this));
		}

		/// <summary>
		/// An event handler called when the page status has changed.
		/// </summary>
		/// <param name="status">The status.</param>
		public void OnStatusChanged(string status)
		{
			// Set the status.
			this.status = status;
			// Raise the event.
			if (null != this.PageChanged) this.PageChanged(this, new WizardPageEventArgs(this));
		}
	}
}
