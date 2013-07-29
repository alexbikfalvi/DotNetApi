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
using System.Windows.Forms;

namespace DotNetApi.Windows.Controls
{
	public delegate void SideTreeViewControlChangedEventHandler(Control sender, Control control);

	/// <summary>
	/// A control representing a side tree view.
	/// </summary>
	public class SideTreeView : TreeView, ISideControl
	{
		/// <summary>
		/// Creates a new control instance.
		/// </summary>
		public SideTreeView()
		{
			this.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.Dock = System.Windows.Forms.DockStyle.Fill;
			this.FullRowSelect = true;
			this.HideSelection = false;
			this.ItemHeight = 20;
			this.ShowLines = false;
			this.ShowRootLines = false;
			this.Visible = false;
		}

		// Public events.

		/// <summary>
		/// An event raised when the selected control has changed.
		/// </summary>
		public event SideTreeViewControlChangedEventHandler ControlChanged;

		// Public methods.

		/// <summary>
		/// Initializes the side control.
		/// </summary>
		public void Initialize()
		{
			// Expand all nodes.
			this.ExpandAll();
			// If there are nodes, select the first node.
			if (this.Nodes.Count > 0)
			{
				this.SelectedNode = this.Nodes[0];
			}
		}

		/// <summary>
		/// Shows the current side control and activates its content.
		/// </summary>
		public void ShowSideControl()
		{
			// Call the base class show method.
			base.Show();
			// Select this control.
			base.Select();
			// If there exists a selected node.
			if (null != this.SelectedNode)
			{
				// Get the control tag for the selected tree node.
				Control control = this.SelectedNode.Tag as Control;
				// Raise a control changed event for this control.
				if (null != this.ControlChanged) this.ControlChanged(this, control);
			}
		}

		/// <summary>
		/// Hides the current side control and deactivates its content.
		/// </summary>
		public void HideSideControl()
		{
			// Call the base class hide method.
			base.Hide();
		}

		// Protected methods.

		/// <summary>
		/// An event handler called after a new tree node has been selected.
		/// </summary>
		/// <param name="e">The event arguments.</param>
		protected override void OnAfterSelect(TreeViewEventArgs e)
		{
			// Call the base class event handler.
			base.OnAfterSelect(e);
			// Call the node selection changed event handler.
			this.OnNodeSelectionChanged(e.Node);
		}

		/// <summary>
		/// An event handler called when the tree node selection has changed.
		/// </summary>
		/// <param name="node">The tree node.</param>
		protected virtual void OnNodeSelectionChanged(TreeNode node)
		{
			// If the selected node is null, do nothing.
			if (null == node) return;
			// Get the control tag for the selected tree node.
			Control control = node.Tag as Control;
			// If this control is visible.
			if (this.Visible)
			{
				// Raise a control changed event for this control.
				if (null != this.ControlChanged) this.ControlChanged(this, control);
			}
		}
	}
}
