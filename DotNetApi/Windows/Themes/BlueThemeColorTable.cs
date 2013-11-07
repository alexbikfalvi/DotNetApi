using System;
using System.Drawing;
using System.Windows.Forms;

namespace DotNetApi.Windows.Themes
{
	/// <summary>
	/// A class representing a blue theme color table.
	/// </summary>
	public class BlueThemeColorTable : ProfessionalColorTable
	{
		// Summary:
		//     Initializes a new instance of the System.Windows.Forms.ProfessionalColorTable
		//     class.
		public BlueThemeColorTable()
		{

		}

		// Summary:
		//     Gets the starting color of the gradient used when the button is checked.
		//
		// Returns:
		//     A System.Drawing.Color that is the starting color of the gradient used when
		//     the button is checked.
		public override Color ButtonCheckedGradientBegin { get { return Color.FromArgb(253, 244, 191); } }
		//
		// Summary:
		//     Gets the end color of the gradient used when the button is checked.
		//
		// Returns:
		//     A System.Drawing.Color that is the end color of the gradient used when the
		//     button is checked.
		public override Color ButtonCheckedGradientEnd { get { return Color.FromArgb(253, 244, 191); } }
		//
		// Summary:
		//     Gets the middle color of the gradient used when the button is checked.
		//
		// Returns:
		//     A System.Drawing.Color that is the middle color of the gradient used when
		//     the button is checked.
		public override Color ButtonCheckedGradientMiddle { get { return Color.FromArgb(253, 244, 191); } }
		//
		// Summary:
		//     Gets the solid color used when the button is checked.
		//
		// Returns:
		//     A System.Drawing.Color that is the solid color used when the button is checked.
		public override Color ButtonCheckedHighlight { get { return Color.FromArgb(255, 252, 244); } }
		//
		// Summary:
		//     Gets the border color to use with System.Windows.Forms.ProfessionalColorTable.ButtonCheckedHighlight.
		//
		// Returns:
		//     A System.Drawing.Color that is the border color to use with System.Windows.Forms.ProfessionalColorTable.ButtonCheckedHighlight.
		public override Color ButtonCheckedHighlightBorder { get { return Color.FromArgb(229, 195, 101); } }
		//
		// Summary:
		//     Gets the border color to use with the System.Windows.Forms.ProfessionalColorTable.ButtonPressedGradientBegin,
		//     System.Windows.Forms.ProfessionalColorTable.ButtonPressedGradientMiddle,
		//     and System.Windows.Forms.ProfessionalColorTable.ButtonPressedGradientEnd
		//     colors.
		//
		// Returns:
		//     A System.Drawing.Color that is the border color to use with the System.Windows.Forms.ProfessionalColorTable.ButtonPressedGradientBegin,
		//     System.Windows.Forms.ProfessionalColorTable.ButtonPressedGradientMiddle,
		//     and System.Windows.Forms.ProfessionalColorTable.ButtonPressedGradientEnd
		//     colors.
		public override Color ButtonPressedBorder { get { return Color.FromArgb(229, 195, 101); } }
		//
		// Summary:
		//     Gets the starting color of the gradient used when the button is pressed.
		//
		// Returns:
		//     A System.Drawing.Color that is the starting color of the gradient used when
		//     the button is pressed.
		public override Color ButtonPressedGradientBegin { get { return Color.FromArgb(255, 242, 157); } }
		//
		// Summary:
		//     Gets the end color of the gradient used when the button is pressed.
		//
		// Returns:
		//     A System.Drawing.Color that is the end color of the gradient used when the
		//     button is pressed.
		public override Color ButtonPressedGradientEnd { get { return Color.FromArgb(255, 242, 157); } }
		//
		// Summary:
		//     Gets the middle color of the gradient used when the button is pressed.
		//
		// Returns:
		//     A System.Drawing.Color that is the middle color of the gradient used when
		//     the button is pressed.
		public override Color ButtonPressedGradientMiddle { get { return Color.FromArgb(255, 242, 157); } }
		//
		// Summary:
		//     Gets the solid color used when the button is pressed.
		//
		// Returns:
		//     A System.Drawing.Color that is the solid color used when the button is pressed.
		public override Color ButtonPressedHighlight { get { return Color.FromArgb(255, 242, 157); } }
		//
		// Summary:
		//     Gets the border color to use with System.Windows.Forms.ProfessionalColorTable.ButtonPressedHighlight.
		//
		// Returns:
		//     A System.Drawing.Color that is the border color to use with System.Windows.Forms.ProfessionalColorTable.ButtonPressedHighlight.
		public override Color ButtonPressedHighlightBorder { get { return Color.FromArgb(229, 195, 101); } }
		//
		// Summary:
		//     Gets the border color to use with the System.Windows.Forms.ProfessionalColorTable.ButtonSelectedGradientBegin,
		//     System.Windows.Forms.ProfessionalColorTable.ButtonSelectedGradientMiddle,
		//     and System.Windows.Forms.ProfessionalColorTable.ButtonSelectedGradientEnd
		//     colors.
		//
		// Returns:
		//     A System.Drawing.Color that is the border color to use with the System.Windows.Forms.ProfessionalColorTable.ButtonSelectedGradientBegin,
		//     System.Windows.Forms.ProfessionalColorTable.ButtonSelectedGradientMiddle,
		//     and System.Windows.Forms.ProfessionalColorTable.ButtonSelectedGradientEnd
		//     colors.
		public override Color ButtonSelectedBorder { get { return Color.FromArgb(229, 195, 101); } }
		//
		// Summary:
		//     Gets the starting color of the gradient used when the button is selected.
		//
		// Returns:
		//     A System.Drawing.Color that is the starting color of the gradient used when
		//     the button is selected.
		public override Color ButtonSelectedGradientBegin { get { return Color.FromArgb(253, 244, 191); } }
		//
		// Summary:
		//     Gets the end color of the gradient used when the button is selected.
		//
		// Returns:
		//     A System.Drawing.Color that is the end color of the gradient used when the
		//     button is selected.
		public override Color ButtonSelectedGradientEnd { get { return Color.FromArgb(253, 244, 191); } }
		//
		// Summary:
		//     Gets the middle color of the gradient used when the button is selected.
		//
		// Returns:
		//     A System.Drawing.Color that is the middle color of the gradient used when
		//     the button is selected.
		public override Color ButtonSelectedGradientMiddle { get { return Color.FromArgb(253, 244, 191); } }
		//
		// Summary:
		//     Gets the solid color used when the button is selected.
		//
		// Returns:
		//     A System.Drawing.Color that is the solid color used when the button is selected.
		public override Color ButtonSelectedHighlight { get { return Color.FromArgb(253, 244, 191); } }
		//
		// Summary:
		//     Gets the border color to use with System.Windows.Forms.ProfessionalColorTable.ButtonSelectedHighlight.
		//
		// Returns:
		//     A System.Drawing.Color that is the border color to use with System.Windows.Forms.ProfessionalColorTable.ButtonSelectedHighlight.
		public override Color ButtonSelectedHighlightBorder { get { return Color.FromArgb(229, 195, 101); } }
		//
		// Summary:
		//     Gets the solid color to use when the button is checked and gradients are
		//     being used.
		//
		// Returns:
		//     A System.Drawing.Color that is the solid color to use when the button is
		//     checked and gradients are being used.
		public override Color CheckBackground { get { return Color.FromArgb(253, 244, 191); } }
		//
		// Summary:
		//     Gets the solid color to use when the button is checked and selected and gradients
		//     are being used.
		//
		// Returns:
		//     A System.Drawing.Color that is the solid color to use when the button is
		//     checked and selected and gradients are being used.
		public override Color CheckPressedBackground { get { return Color.FromArgb(255, 252, 244); } }
		//
		// Summary:
		//     Gets the solid color to use when the button is checked and selected and gradients
		//     are being used.
		//
		// Returns:
		//     A System.Drawing.Color that is the solid color to use when the button is
		//     checked and selected and gradients are being used.
		public override Color CheckSelectedBackground { get { return Color.FromArgb(255, 252, 244); } }
		//
		// Summary:
		//     Gets the color to use for shadow effects on the grip (move handle).
		//
		// Returns:
		//     A System.Drawing.Color that is the color to use for shadow effects on the
		//     grip (move handle).
		public override Color GripDark { get { return Color.FromArgb(207, 214, 229); } }
		//
		// Summary:
		//     Gets the color to use for highlight effects on the grip (move handle).
		//
		// Returns:
		//     A System.Drawing.Color that is the color to use for highlight effects on
		//     the grip (move handle).
		public override Color GripLight { get { return Color.FromArgb(207, 214, 229); } }
		//
		// Summary:
		//     Gets the starting color of the gradient used in the image margin of a System.Windows.Forms.ToolStripDropDownMenu.
		//
		// Returns:
		//     A System.Drawing.Color that is the starting color of the gradient used in
		//     the image margin of a System.Windows.Forms.ToolStripDropDownMenu.
		public override Color ImageMarginGradientBegin { get { return Color.FromArgb(242, 244, 254); } }
		//
		// Summary:
		//     Gets the end color of the gradient used in the image margin of a System.Windows.Forms.ToolStripDropDownMenu.
		//
		// Returns:
		//     A System.Drawing.Color that is the end color of the gradient used in the
		//     image margin of a System.Windows.Forms.ToolStripDropDownMenu.
		public override Color ImageMarginGradientEnd { get { return Color.FromArgb(242, 244, 254); } }
		//
		// Summary:
		//     Gets the middle color of the gradient used in the image margin of a System.Windows.Forms.ToolStripDropDownMenu.
		//
		// Returns:
		//     A System.Drawing.Color that is the middle color of the gradient used in the
		//     image margin of a System.Windows.Forms.ToolStripDropDownMenu.
		public override Color ImageMarginGradientMiddle { get { return Color.FromArgb(242, 244, 254); } }
		//
		// Summary:
		//     Gets the starting color of the gradient used in the image margin of a System.Windows.Forms.ToolStripDropDownMenu
		//     when an item is revealed.
		//
		// Returns:
		//     A System.Drawing.Color that is the starting color of the gradient used in
		//     the image margin of a System.Windows.Forms.ToolStripDropDownMenu when an
		//     item is revealed.
		public override Color ImageMarginRevealedGradientBegin { get { return Color.FromArgb(242, 244, 254); } }
		//
		// Summary:
		//     Gets the end color of the gradient used in the image margin of a System.Windows.Forms.ToolStripDropDownMenu
		//     when an item is revealed.
		//
		// Returns:
		//     A System.Drawing.Color that is the end color of the gradient used in the
		//     image margin of a System.Windows.Forms.ToolStripDropDownMenu when an item
		//     is revealed.
		public override Color ImageMarginRevealedGradientEnd { get { return Color.FromArgb(242, 244, 254); } }
		//
		// Summary:
		//     Gets the middle color of the gradient used in the image margin of a System.Windows.Forms.ToolStripDropDownMenu
		//     when an item is revealed.
		//
		// Returns:
		//     A System.Drawing.Color that is the middle color of the gradient used in the
		//     image margin of a System.Windows.Forms.ToolStripDropDownMenu when an item
		//     is revealed.
		public override Color ImageMarginRevealedGradientMiddle { get { return Color.FromArgb(242, 244, 254); } }
		//
		// Summary:
		//     Gets the color that is the border color to use on a System.Windows.Forms.MenuStrip.
		//
		// Returns:
		//     A System.Drawing.Color that is the border color to use on a System.Windows.Forms.MenuStrip.
		public override Color MenuBorder { get { return Color.FromArgb(155, 167, 183); } }
		//
		// Summary:
		//     Gets the border color to use with a System.Windows.Forms.ToolStripMenuItem.
		//
		// Returns:
		//     A System.Drawing.Color that is the border color to use with a System.Windows.Forms.ToolStripMenuItem.
		public override Color MenuItemBorder { get { return Color.FromArgb(229, 195, 101); } }
		//
		// Summary:
		//     Gets the starting color of the gradient used when a top-level System.Windows.Forms.ToolStripMenuItem
		//     is pressed.
		//
		// Returns:
		//     A System.Drawing.Color that is the starting color of the gradient used when
		//     a top-level System.Windows.Forms.ToolStripMenuItem is pressed.
		public override Color MenuItemPressedGradientBegin { get { return Color.FromArgb(253, 244, 191); } }
		//
		// Summary:
		//     Gets the end color of the gradient used when a top-level System.Windows.Forms.ToolStripMenuItem
		//     is pressed.
		//
		// Returns:
		//     A System.Drawing.Color that is the end color of the gradient used when a
		//     top-level System.Windows.Forms.ToolStripMenuItem is pressed.
		public override Color MenuItemPressedGradientEnd { get { return Color.FromArgb(253, 244, 191); } }
		//
		// Summary:
		//     Gets the middle color of the gradient used when a top-level System.Windows.Forms.ToolStripMenuItem
		//     is pressed.
		//
		// Returns:
		//     A System.Drawing.Color that is the middle color of the gradient used when
		//     a top-level System.Windows.Forms.ToolStripMenuItem is pressed.
		public override Color MenuItemPressedGradientMiddle { get { return Color.FromArgb(253, 244, 191); } }
		//
		// Summary:
		//     Gets the solid color to use when a System.Windows.Forms.ToolStripMenuItem
		//     other than the top-level System.Windows.Forms.ToolStripMenuItem is selected.
		//
		// Returns:
		//     A System.Drawing.Color that is the solid color to use when a System.Windows.Forms.ToolStripMenuItem
		//     other than the top-level System.Windows.Forms.ToolStripMenuItem is selected.
		public override Color MenuItemSelected { get { return Color.FromArgb(253, 244, 191); } }
		//
		// Summary:
		//     Gets the starting color of the gradient used when the System.Windows.Forms.ToolStripMenuItem
		//     is selected.
		//
		// Returns:
		//     A System.Drawing.Color that is the starting color of the gradient used when
		//     the System.Windows.Forms.ToolStripMenuItem is selected.
		public override Color MenuItemSelectedGradientBegin { get { return Color.FromArgb(253, 244, 191); } }
		//
		// Summary:
		//     Gets the end color of the gradient used when the System.Windows.Forms.ToolStripMenuItem
		//     is selected.
		//
		// Returns:
		//     A System.Drawing.Color that is the end color of the gradient used when the
		//     System.Windows.Forms.ToolStripMenuItem is selected.
		public override Color MenuItemSelectedGradientEnd { get { return Color.FromArgb(253, 244, 191); } }
		//
		// Summary:
		//     Gets the starting color of the gradient used in the System.Windows.Forms.MenuStrip.
		//
		// Returns:
		//     A System.Drawing.Color that is the starting color of the gradient used in
		//     the System.Windows.Forms.MenuStrip.
		public override Color MenuStripGradientBegin { get { return Color.FromArgb(214, 219, 233); } }
		//
		// Summary:
		//     Gets the end color of the gradient used in the System.Windows.Forms.MenuStrip.
		//
		// Returns:
		//     A System.Drawing.Color that is the end color of the gradient used in the
		//     System.Windows.Forms.MenuStrip.
		public override Color MenuStripGradientEnd { get { return Color.FromArgb(214, 219, 233); } }
		//
		// Summary:
		//     Gets the starting color of the gradient used in the System.Windows.Forms.ToolStripOverflowButton.
		//
		// Returns:
		//     A System.Drawing.Color that is the starting color of the gradient used in
		//     the System.Windows.Forms.ToolStripOverflowButton.
		public override Color OverflowButtonGradientBegin { get { return Color.FromArgb(220, 224, 236); } }
		//
		// Summary:
		//     Gets the end color of the gradient used in the System.Windows.Forms.ToolStripOverflowButton.
		//
		// Returns:
		//     A System.Drawing.Color that is the end color of the gradient used in the
		//     System.Windows.Forms.ToolStripOverflowButton.
		public override Color OverflowButtonGradientEnd { get { return Color.FromArgb(220, 224, 236); } }
		//
		// Summary:
		//     Gets the middle color of the gradient used in the System.Windows.Forms.ToolStripOverflowButton.
		//
		// Returns:
		//     A System.Drawing.Color that is the middle color of the gradient used in the
		//     System.Windows.Forms.ToolStripOverflowButton.
		public override Color OverflowButtonGradientMiddle { get { return Color.FromArgb(220, 224, 236); } }
/*		//
		// Summary:
		//     Gets the starting color of the gradient used in the System.Windows.Forms.ToolStripContainer.
		//
		// Returns:
		//     A System.Drawing.Color that is the starting color of the gradient used in
		//     the System.Windows.Forms.ToolStripContainer.
		public override Color RaftingContainerGradientBegin { get; }
		//
		// Summary:
		//     Gets the end color of the gradient used in the System.Windows.Forms.ToolStripContainer.
		//
		// Returns:
		//     A System.Drawing.Color that is the end color of the gradient used in the
		//     System.Windows.Forms.ToolStripContainer.
		public override Color RaftingContainerGradientEnd { get; }*/
		//
		// Summary:
		//     Gets the color to use to for shadow effects on the System.Windows.Forms.ToolStripSeparator.
		//
		// Returns:
		//     A System.Drawing.Color that is the color to use to for shadow effects on
		//     the System.Windows.Forms.ToolStripSeparator.
		public override Color SeparatorDark { get { return Color.FromArgb(133, 145, 162); } }
		//
		// Summary:
		//     Gets the color to use to for highlight effects on the System.Windows.Forms.ToolStripSeparator.
		//
		// Returns:
		//     A System.Drawing.Color that is the color to use to for highlight effects
		//     on the System.Windows.Forms.ToolStripSeparator.
		public override Color SeparatorLight { get { return Color.FromArgb(214, 219, 233); } }
		//
		// Summary:
		//     Gets the starting color of the gradient used on the System.Windows.Forms.StatusStrip.
		//
		// Returns:
		//     A System.Drawing.Color that is the starting color of the gradient used on
		//     the System.Windows.Forms.StatusStrip.
		public override Color StatusStripGradientBegin { get { return Color.FromArgb(0, 122, 204); } }
		//
		// Summary:
		//     Gets the end color of the gradient used on the System.Windows.Forms.StatusStrip.
		//
		// Returns:
		//     A System.Drawing.Color that is the end color of the gradient used on the
		//     System.Windows.Forms.StatusStrip.
		public override Color StatusStripGradientEnd { get { return Color.FromArgb(0, 122, 204); } }
		//
		// Summary:
		//     Gets the border color to use on the bottom edge of the System.Windows.Forms.ToolStrip.
		//
		// Returns:
		//     A System.Drawing.Color that is the border color to use on the bottom edge
		//     of the System.Windows.Forms.ToolStrip.
		public override Color ToolStripBorder { get { return Color.FromArgb(220, 224, 236); } }
		//
		// Summary:
		//     Gets the starting color of the gradient used in the System.Windows.Forms.ToolStripContentPanel.
		//
		// Returns:
		//     A System.Drawing.Color that is the starting color of the gradient used in
		//     the System.Windows.Forms.ToolStripContentPanel.
		public override Color ToolStripContentPanelGradientBegin { get { return Color.FromArgb(41, 58, 86); }  }
		//
		// Summary:
		//     Gets the end color of the gradient used in the System.Windows.Forms.ToolStripContentPanel.
		//
		// Returns:
		//     A System.Drawing.Color that is the end color of the gradient used in the
		//     System.Windows.Forms.ToolStripContentPanel.
		public override Color ToolStripContentPanelGradientEnd { get { return Color.FromArgb(41, 58, 86); } }
		//
		// Summary:
		//     Gets the solid background color of the System.Windows.Forms.ToolStripDropDown.
		//
		// Returns:
		//     A System.Drawing.Color that is the solid background color of the System.Windows.Forms.ToolStripDropDown.
		public override Color ToolStripDropDownBackground { get { return Color.FromArgb(234, 240, 255); } }
		//
		// Summary:
		//     Gets the starting color of the gradient used in the System.Windows.Forms.ToolStrip
		//     background.
		//
		// Returns:
		//     A System.Drawing.Color that is the starting color of the gradient used in
		//     the System.Windows.Forms.ToolStrip background.
		public override Color ToolStripGradientBegin { get { return Color.FromArgb(207, 214, 229); } }
		//
		// Summary:
		//     Gets the end color of the gradient used in the System.Windows.Forms.ToolStrip
		//     background.
		//
		// Returns:
		//     A System.Drawing.Color that is the end color of the gradient used in the
		//     System.Windows.Forms.ToolStrip background.
		public override Color ToolStripGradientEnd { get { return Color.FromArgb(207, 214, 229); } }
		//
		// Summary:
		//     Gets the middle color of the gradient used in the System.Windows.Forms.ToolStrip
		//     background.
		//
		// Returns:
		//     A System.Drawing.Color that is the middle color of the gradient used in the
		//     System.Windows.Forms.ToolStrip background.
		public override Color ToolStripGradientMiddle { get { return Color.FromArgb(207, 214, 229); } }
		//
		// Summary:
		//     Gets the starting color of the gradient used in the System.Windows.Forms.ToolStripPanel.
		//
		// Returns:
		//     A System.Drawing.Color that is the starting color of the gradient used in
		//     the System.Windows.Forms.ToolStripPanel.
		public override Color ToolStripPanelGradientBegin { get { return Color.FromArgb(41, 58, 86); } }
		//
		// Summary:
		//     Gets the end color of the gradient used in the System.Windows.Forms.ToolStripPanel.
		//
		// Returns:
		//     A System.Drawing.Color that is the end color of the gradient used in the
		//     System.Windows.Forms.ToolStripPanel.
		public override Color ToolStripPanelGradientEnd { get { return Color.FromArgb(41, 58, 86); } }
	}
}
