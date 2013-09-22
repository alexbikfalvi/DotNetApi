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
using System.Drawing.Design;
using System.Windows.Forms;

namespace DotNetApi.Windows.Controls
{
	/// <summary>
	/// A control used for implementing wizard tabs.
	/// </summary>
	public sealed class WizardControl : Control
	{
		private ComponentCollection<WizardPage> pages = new ComponentCollection<WizardPage>();

		/// <summary>
		/// Creates a new control instance.
		/// </summary>
		public WizardControl()
		{
			// Add the collection pages event handler.
			this.pages.BeforeCleared += OnBeforePagesCleared;
			this.pages.AfterItemInserted += OnAfterPageInserted;
			this.pages.AfterItemRemoved += OnAfterPageRemoved;
			this.pages.AfterItemSet += OnAfterPageSet;
		}

		// Public properties.

		/// <summary>
		/// Gets the collection of wizard pages.
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
		public ComponentCollection<WizardPage> Pages { get { return this.pages; } }

		// Private methods.

		private void OnBeforePagesCleared(object sender, EventArgs e)
		{
			throw new NotImplementedException();
		}

		private void OnAfterPageInserted(object sender, ComponentCollection<WizardPage>.ItemChangedEventArgs e)
		{
			throw new NotImplementedException();
		}

		private void OnAfterPageRemoved(object sender, ComponentCollection<WizardPage>.ItemChangedEventArgs e)
		{
			throw new NotImplementedException();
		}

		private void OnAfterPageSet(object sender, ComponentCollection<WizardPage>.ItemSetEventArgs e)
		{
			throw new NotImplementedException();
		}
	}
}
