/* 
 * Copyright (C) 2010-2012 Alex Bikfalvi
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
using System.Drawing;
using System.Windows.Forms;

namespace DotNetApi.Windows.Controls
{
	public delegate void SideMenuEventHandler(SideMenuItem menuItem);

	[Serializable]
	public class SideMenuItem : MenuItem
	{
		//private bool visible = true;
		//private bool enabled = false;
		private Image imageSmall = null;
		private Image imageLarge = null;
		//private string text = string.Empty;

		// Hidden menu item
		private ToolStripMenuItem hiddenMenuItem;

		// Minimized tooltip
		private ToolTip toolTip;

		public SideMenuItem(
			string text,
			Image imageSmall,
			Image imageLarge,
			EventHandler hiddenItemClick
			)
		{
			// Hidden menu item
			this.hiddenMenuItem = new ToolStripMenuItem();
			this.hiddenMenuItem.Tag = this;
			this.hiddenMenuItem.Click += hiddenItemClick;

			this.Visible = true;
			this.Enabled = true;
			this.ImageSmall = imageSmall;
			this.ImageLarge = imageLarge;
			this.Tag = null;
			this.Text = text;
			
			this.toolTip = new ToolTip();
		}

		/// <summary>
		/// An event raised when the menu item is clicked.
		/// </summary>
		public new SideMenuEventHandler Click;

		///// <summary>
		///// Gets or sets whether the item is visible.
		///// </summary>
		//public bool Visible
		//{
		//	get { return this.visible; }
		//	set
		//	{
		//		this.visible = value;
		//	}
		//}
		///// <summary>
		///// Gets or sets whether the item is enabled.
		///// </summary>
		//public bool Enabled
		//{
		//	get { return this.enabled; }
		//	set
		//	{
		//		this.enabled = value;
		//		this.hiddenMenuItem.Enabled = value;
		//	}
		//}
		/// <summary>
		/// Gets or sets the small menu item image.
		/// </summary>
		public Image ImageSmall
		{
			get { return this.imageSmall; }
			set
			{
				this.imageSmall = value;
				this.hiddenMenuItem.Image = value;
			}
		}
		/// <summary>
		/// Gets or sets the large menu item image.
		/// </summary>
		public Image ImageLarge
		{
			get { return this.imageLarge; }
			set { this.imageLarge = value; }
		}
		///// <summary>
		///// Gets or sets the menu item tag.
		///// </summary>
		//public object Tag { get; set; }
		///// <summary>
		///// Gets  or sets the menu item text.
		///// </summary>
		//public string Text
		//{
		//	get { return this.text; }
		//	set
		//	{
		//		this.text = value;
		//		this.hiddenMenuItem.Text = value;
		//	}
		//}

		public ToolStripMenuItem HiddenMenuItem { get { return this.hiddenMenuItem; } }

		public ToolTip ToolTop { get { return this.toolTip; } }

		/// <summary>
		/// Selects the current menu item.
		/// </summary>
		public new void Select()
		{
			// Call the handler for the side menu.
			if (null != this.Click) this.Click(this);
			this.hiddenMenuItem.Checked = true;
		}

		/// <summary>
		/// Deselects the current menu item.
		/// </summary>
		public void Deselect()
		{
			this.hiddenMenuItem.Checked = false;
		}
	}
}
