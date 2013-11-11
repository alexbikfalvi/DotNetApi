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
using System.Drawing;
using System.Windows.Forms;

namespace DotNetApi.Windows.Themes
{
	/// <summary>
	/// A class representing a blue theme color table.
	/// </summary>
	public class BlueThemeColorTable : ThemeColorTable
	{
		/// <summary>
		/// Initializes a new instance of the System.Windows.Forms.ProfessionalColorTable class.
		/// </summary>
		public BlueThemeColorTable()
		{

		}

		/// <summary>
		/// Gets the starting color of the gradient used when the button is checked.
		/// </summary>
		public override Color ButtonCheckedGradientBegin { get { return Color.FromArgb(253, 244, 191); } }
		/// <summary>
		/// Gets the end color of the gradient used when the button is checked.
		/// </summary>
		public override Color ButtonCheckedGradientEnd { get { return Color.FromArgb(253, 244, 191); } }
		/// <summary>
		/// Gets the middle color of the gradient used when the button is checked.
		/// </summary>
		public override Color ButtonCheckedGradientMiddle { get { return Color.FromArgb(253, 244, 191); } }
		/// <summary>
		/// Gets the solid color used when the button is checked.
		/// </summary>
		public override Color ButtonCheckedHighlight { get { return Color.FromArgb(255, 252, 244); } }
		/// <summary>
		/// Gets the border color to use with System.Windows.Forms.ProfessionalColorTable.ButtonCheckedHighlight.
		/// </summary>
		public override Color ButtonCheckedHighlightBorder { get { return Color.FromArgb(229, 195, 101); } }
		/// <summary>
		/// Gets the border color to use with the System.Windows.Forms.ProfessionalColorTable.ButtonPressedGradientBegin,
		/// System.Windows.Forms.ProfessionalColorTable.ButtonPressedGradientMiddle,
		/// and System.Windows.Forms.ProfessionalColorTable.ButtonPressedGradientEnd colors.
		/// </summary>
		public override Color ButtonPressedBorder { get { return Color.FromArgb(229, 195, 101); } }
		/// <summary>
		/// Gets the starting color of the gradient used when the button is pressed.
		/// </summary>
		public override Color ButtonPressedGradientBegin { get { return Color.FromArgb(255, 242, 157); } }
		/// <summary>
		/// Gets the end color of the gradient used when the button is pressed.
		/// </summary>
		public override Color ButtonPressedGradientEnd { get { return Color.FromArgb(255, 242, 157); } }
		/// <summary>
		/// Gets the middle color of the gradient used when the button is pressed.
		/// </summary>
		public override Color ButtonPressedGradientMiddle { get { return Color.FromArgb(255, 242, 157); } }
		/// <summary>
		/// Gets the solid color used when the button is pressed.
		/// </summary>
		public override Color ButtonPressedHighlight { get { return Color.FromArgb(255, 242, 157); } }
		/// <summary>
		/// Gets the border color to use with System.Windows.Forms.ProfessionalColorTable.ButtonPressedHighlight.
		/// </summary>
		public override Color ButtonPressedHighlightBorder { get { return Color.FromArgb(229, 195, 101); } }
		/// <summary>
		/// Gets the border color to use with the System.Windows.Forms.ProfessionalColorTable.ButtonSelectedGradientBegin,
		/// System.Windows.Forms.ProfessionalColorTable.ButtonSelectedGradientMiddle,
		/// and System.Windows.Forms.ProfessionalColorTable.ButtonSelectedGradientEnd colors.
		/// </summary>
		public override Color ButtonSelectedBorder { get { return Color.FromArgb(229, 195, 101); } }
		/// <summary>
		/// Gets the starting color of the gradient used when the button is selected.
		/// </summary>
		public override Color ButtonSelectedGradientBegin { get { return Color.FromArgb(253, 244, 191); } }
		/// <summary>
		/// Gets the end color of the gradient used when the button is selected.
		/// </summary>
		public override Color ButtonSelectedGradientEnd { get { return Color.FromArgb(253, 244, 191); } }
		/// <summary>
		/// Gets the middle color of the gradient used when the button is selected.
		/// </summary>
		public override Color ButtonSelectedGradientMiddle { get { return Color.FromArgb(253, 244, 191); } }
		/// <summary>
		/// Gets the solid color used when the button is selected.
		/// </summary>
		public override Color ButtonSelectedHighlight { get { return Color.FromArgb(253, 244, 191); } }
		/// <summary>
		/// Gets the border color to use with System.Windows.Forms.ProfessionalColorTable.ButtonSelectedHighlight.
		/// </summary>
		public override Color ButtonSelectedHighlightBorder { get { return Color.FromArgb(229, 195, 101); } }
		/// <summary>
		/// Gets the solid color to use when the button is checked and gradients are being used.
		/// </summary>
		public override Color CheckBackground { get { return Color.FromArgb(253, 244, 191); } }
		/// <summary>
		/// Gets the solid color to use when the button is checked and selected and gradients are being used.
		/// </summary>
		public override Color CheckPressedBackground { get { return Color.FromArgb(255, 252, 244); } }
		/// <summary>
		/// Gets the solid color to use when the button is checked and selected and gradients are being used.
		/// </summary>
		public override Color CheckSelectedBackground { get { return Color.FromArgb(255, 252, 244); } }
		/// <summary>
		/// Gets the color to use for shadow effects on the grip (move handle).
		/// </summary>
		public override Color GripDark { get { return Color.FromArgb(207, 214, 229); } }
		/// <summary>
		/// Gets the color to use for highlight effects on the grip (move handle).
		/// </summary>
		public override Color GripLight { get { return Color.FromArgb(207, 214, 229); } }
		/// <summary>
		/// Gets the starting color of the gradient used in the image margin of a System.Windows.Forms.ToolStripDropDownMenu.
		/// </summary>
		public override Color ImageMarginGradientBegin { get { return Color.FromArgb(242, 244, 254); } }
		/// <summary>
		/// Gets the end color of the gradient used in the image margin of a System.Windows.Forms.ToolStripDropDownMenu.
		/// </summary>
		public override Color ImageMarginGradientEnd { get { return Color.FromArgb(242, 244, 254); } }
		/// <summary>
		/// Gets the middle color of the gradient used in the image margin of a System.Windows.Forms.ToolStripDropDownMenu.
		/// </summary>
		public override Color ImageMarginGradientMiddle { get { return Color.FromArgb(242, 244, 254); } }
		/// <summary>
		/// Gets the starting color of the gradient used in the image margin of a System.Windows.Forms.ToolStripDropDownMenu when an item is revealed.
		/// </summary>
		public override Color ImageMarginRevealedGradientBegin { get { return Color.FromArgb(242, 244, 254); } }
		/// <summary>
		/// Gets the end color of the gradient used in the image margin of a System.Windows.Forms.ToolStripDropDownMenu when an item is revealed.
		/// </summary>
		public override Color ImageMarginRevealedGradientEnd { get { return Color.FromArgb(242, 244, 254); } }
		/// <summary>
		/// Gets the middle color of the gradient used in the image margin of a System.Windows.Forms.ToolStripDropDownMenu when an item is revealed.
		/// </summary>
		public override Color ImageMarginRevealedGradientMiddle { get { return Color.FromArgb(242, 244, 254); } }
		/// <summary>
		/// Gets the color that is the border color to use on a System.Windows.Forms.MenuStrip.
		/// </summary>
		public override Color MenuBorder { get { return Color.FromArgb(155, 167, 183); } }
		/// <summary>
		/// Gets the border color to use with a System.Windows.Forms.ToolStripMenuItem.
		/// </summary>
		public override Color MenuItemBorder { get { return Color.FromArgb(229, 195, 101); } }
		/// <summary>
		/// Gets the starting color of the gradient used when a top-level System.Windows.Forms.ToolStripMenuItem is pressed.
		/// </summary>
		public override Color MenuItemPressedGradientBegin { get { return Color.FromArgb(253, 244, 191); } }
		/// <summary>
		/// Gets the end color of the gradient used when a top-level System.Windows.Forms.ToolStripMenuItem is pressed.
		/// </summary>
		public override Color MenuItemPressedGradientEnd { get { return Color.FromArgb(253, 244, 191); } }
		/// <summary>
		/// Gets the middle color of the gradient used when a top-level System.Windows.Forms.ToolStripMenuItem is pressed.
		/// </summary>
		public override Color MenuItemPressedGradientMiddle { get { return Color.FromArgb(253, 244, 191); } }
		/// <summary>
		/// Gets the solid color to use when a System.Windows.Forms.ToolStripMenuItem other than the top-level System.Windows.Forms.ToolStripMenuItem is selected.
		/// </summary>
		public override Color MenuItemSelected { get { return Color.FromArgb(253, 244, 191); } }
		/// <summary>
		/// Gets the starting color of the gradient used when the System.Windows.Forms.ToolStripMenuItem is selected.
		/// </summary>
		public override Color MenuItemSelectedGradientBegin { get { return Color.FromArgb(253, 244, 191); } }
		/// <summary>
		/// Gets the end color of the gradient used when the System.Windows.Forms.ToolStripMenuItem is selected.
		/// </summary>
		public override Color MenuItemSelectedGradientEnd { get { return Color.FromArgb(253, 244, 191); } }
		/// <summary>
		/// Gets the starting color of the gradient used in the System.Windows.Forms.MenuStrip.
		/// </summary>
		public override Color MenuStripGradientBegin { get { return Color.FromArgb(214, 219, 233); } }
		/// <summary>
		/// Gets the end color of the gradient used in the System.Windows.Forms.MenuStrip.
		/// </summary>
		public override Color MenuStripGradientEnd { get { return Color.FromArgb(214, 219, 233); } }
		/// <summary>
		/// Gets the starting color of the gradient used in the System.Windows.Forms.ToolStripOverflowButton.
		/// </summary>
		public override Color OverflowButtonGradientBegin { get { return Color.FromArgb(220, 224, 236); } }
		/// <summary>
		/// Gets the end color of the gradient used in the System.Windows.Forms.ToolStripOverflowButton.
		/// </summary>
		public override Color OverflowButtonGradientEnd { get { return Color.FromArgb(220, 224, 236); } }
		/// <summary>
		/// Gets the middle color of the gradient used in the System.Windows.Forms.ToolStripOverflowButton.
		/// </summary>
		public override Color OverflowButtonGradientMiddle { get { return Color.FromArgb(220, 224, 236); } }
		/// <summary>
		/// Gets the starting color of the gradient used in the System.Windows.Forms.ToolStripContainer.
		/// </summary>
		public override Color RaftingContainerGradientBegin { get { return Color.FromArgb(41, 57, 85); } }
		/// <summary>
		/// Gets the end color of the gradient used in the System.Windows.Forms.ToolStripContainer.
		/// </summary>
		public override Color RaftingContainerGradientEnd { get { return Color.FromArgb(41, 57, 85); } }
		/// <summary>
		/// Gets the color to use to for shadow effects on the System.Windows.Forms.ToolStripSeparator.
		/// </summary>
		public override Color SeparatorDark { get { return Color.FromArgb(133, 145, 162); } }
		/// <summary>
		/// Gets the color to use to for highlight effects on the System.Windows.Forms.ToolStripSeparator.
		/// </summary>
		public override Color SeparatorLight { get { return Color.FromArgb(214, 219, 233); } }
		/// <summary>
		/// Gets the starting color of the gradient used on the System.Windows.Forms.StatusStrip.
		/// </summary>
		public override Color StatusStripGradientBegin { get { return Color.FromArgb(0, 122, 204); } }
		/// <summary>
		/// Gets the end color of the gradient used on the System.Windows.Forms.StatusStrip.
		/// </summary>
		public override Color StatusStripGradientEnd { get { return Color.FromArgb(0, 122, 204); } }
		/// <summary>
		/// Gets the border color to use on the bottom edge of the System.Windows.Forms.ToolStrip.
		/// </summary>
		public override Color ToolStripBorder { get { return Color.FromArgb(220, 224, 236); } }
		/// <summary>
		/// Gets the starting color of the gradient used in the System.Windows.Forms.ToolStripContentPanel.
		/// </summary>
		public override Color ToolStripContentPanelGradientBegin { get { return Color.FromArgb(41, 58, 86); }  }
		/// <summary>
		/// Gets the end color of the gradient used in the System.Windows.Forms.ToolStripContentPanel.
		/// </summary>
		public override Color ToolStripContentPanelGradientEnd { get { return Color.FromArgb(41, 58, 86); } }
		/// <summary>
		/// Gets the solid background color of the System.Windows.Forms.ToolStripDropDown.
		/// </summary>
		public override Color ToolStripDropDownBackground { get { return Color.FromArgb(234, 240, 255); } }
		/// <summary>
		/// Gets the starting color of the gradient used in the System.Windows.Forms.ToolStrip background.
		/// </summary>
		public override Color ToolStripGradientBegin { get { return Color.FromArgb(207, 214, 229); } }
		/// <summary>
		/// Gets the end color of the gradient used in the System.Windows.Forms.ToolStrip background.
		/// </summary>
		public override Color ToolStripGradientEnd { get { return Color.FromArgb(207, 214, 229); } }
		/// <summary>
		/// Gets the middle color of the gradient used in the System.Windows.Forms.ToolStrip background.
		/// </summary>
		public override Color ToolStripGradientMiddle { get { return Color.FromArgb(207, 214, 229); } }
		/// <summary>
		/// Gets the starting color of the gradient used in the System.Windows.Forms.ToolStripPanel.
		/// </summary>
		public override Color ToolStripPanelGradientBegin { get { return Color.FromArgb(41, 58, 86); } }
		/// <summary>
		/// Gets the end color of the gradient used in the System.Windows.Forms.ToolStripPanel.
		/// </summary>
		public override Color ToolStripPanelGradientEnd { get { return Color.FromArgb(41, 58, 86); } }
		/// <summary>
		/// Gets the border color for a notification box.
		/// </summary>
		public override Color NotificationBoxBorder { get { return Color.FromArgb(51, 153, 255); } }
		/// <summary>
		/// Gets the background color for a notification box.
		/// </summary>
		public override Color NotificationBoxBackground { get { return Color.White; } }
		/// <summary>
		/// Gets the title color for a notification box.
		/// </summary>
		public override Color NotificationBoxTitle { get { return Color.FromArgb(214, 219, 233); } }
		/// <summary>
		/// Gets the title text color for a notification box.
		/// </summary>
		public override Color NotificationBoxTitleText { get { return Color.Black; } }
		/// <summary>
		/// Get the border color for a tool split container.
		/// </summary>
		public override Color ToolSplitContainerBorder { get { return Color.FromArgb(142, 155, 188); } }
		/// <summary>
		/// Gets the selected title gradient begin color.
		/// </summary>
		public override Color PanelTitleGradientBegin { get { return Color.FromArgb(77, 96, 130); } }
		/// <summary>
		/// Gets the selected title gradient end color.
		/// </summary>
		public override Color PanelTitleGradientEnd { get { return Color.FromArgb(77, 96, 130); } }
		/// <summary>
		/// Gets the selected title text color.
		/// </summary>
		public override Color PanelTitleText { get { return Color.White; } }
		/// <summary>
		/// Gets the selected title gradient begin color.
		/// </summary>
		public override Color PanelTitleSelectedGradientBegin { get { return Color.FromArgb(255, 242, 157); } }
		/// <summary>
		/// Gets the selected title gradient end color.
		/// </summary>
		public override Color PanelTitleSelectedGradientEnd { get { return Color.FromArgb(255, 242, 157); } }
		/// <summary>
		/// Gets the selected title text color.
		/// </summary>
		public override Color PanelTitleSelectedText { get { return Color.Black; } }
		/// <summary>
		/// Gets the normal background color of the status strip.
		/// </summary>
		public override Color StatusStripNormalBackground { get { return Color.FromArgb(14, 99, 156); } }
		/// <summary>
		/// Gets the normal text color of the status strip.
		/// </summary>
		public override Color StatusStripNormalText { get { return Color.White; } }
		/// <summary>
		/// Gets the ready background color of the status strip.
		/// </summary>
		public override Color StatusStripReadyBackground { get { return Color.FromArgb(104, 33, 122); } }
		/// <summary>
		/// Gets the ready text color of the status strip.
		/// </summary>
		public override Color StatusStripReadyText { get { return Color.White; } }
		/// <summary>
		/// Gets the busy background color of the status strip.
		/// </summary>
		public override Color StatusStripBusyBackground { get { return Color.FromArgb(202, 81, 0); } }
		/// <summary>
		/// Gets the busy text color of the status strip.
		/// </summary>
		public override Color StatusStripBusyText { get { return Color.White; } }
	}
}
