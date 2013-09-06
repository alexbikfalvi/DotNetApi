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
	/// <summary>
	/// A control representing a side tree view.
	/// </summary>
	public class SideTreeView : TreeView, ISideControl
	{
		private bool visible = false;

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
		public event ControlChangedEventHandler ControlChanged;

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
			// Set control as visible.
			this.visible = true;
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
				if (null != this.ControlChanged) this.ControlChanged(this, new ControlChangedEventArgs(control));
			}
		}

		/// <summary>
		/// Hides the current side control and deactivates its content.
		/// </summary>
		public void HideSideControl()
		{
			// Set control as hidden.
			this.visible = false;
			// Call the base class hide method.
			base.Hide();
		}

		/// <summary>
		/// Indicates whether the control has a selectable item.
		/// </summary>
		/// <returns><b>True</b> if the control has a selectable item, <b>false</b> otherwise.</returns>
		public bool HasSelected()
		{
			return true;
		}

		/// <summary>
		/// Returns the indices of the selected item.
		/// </summary>
		/// <returns>The indices.</returns>
		public int[] GetSelected()
		{
			// If there is no selected node, return null.
			if (null == this.SelectedNode) return null;
			// Create an array corresponding to the node level.
			int[] indices = new int[this.SelectedNode.Level + 1];
			// Add the indices of the tree nodes.
			for (TreeNode node = this.SelectedNode; node != null; node = node.Parent)
			{
				indices[node.Level] = node.Index;
			}
			// Return the indices array.
			return indices;
		}

		/// <summary>
		/// Sets the selected item.
		/// </summary>
		/// <param name="indices">The indices index.</param>
		public void SetSelected(int[] indices)
		{
			// If the indices is null, do nothing.
			if (null == indices) return;
			// The current nodes collection.
			TreeNodeCollection nodes = this.Nodes;
			// For all indices.
			for (int index = 0; index < indices.Length; index++)
			{
				// If an index is out of bounds, do nothing.
				if ((indices[index] < 0) && (indices[index] >= nodes.Count)) return;
				// If the last index.
				if (index == indices.Length - 1)
				{
					// Select the node.
					this.SelectedNode = nodes[indices[index]];
					// Scroll to the selected node.
					this.SelectedNode.EnsureVisible();
				}
				else
				{
					// Select the next collection.
					nodes = nodes[indices[index]].Nodes;
				}
			}
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
			if (this.visible)
			{
				// Raise a control changed event for this control.
				if (null != this.ControlChanged) this.ControlChanged(this, new ControlChangedEventArgs(control));
			}
		}
	}
}
