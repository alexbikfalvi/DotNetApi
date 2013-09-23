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

		private int selectedIndex = -1;

		private Button nextButton = null;
		private Button backButton = null;
		private Label titleLabel = null;
		private Label statusLabel = null;

		private readonly string backButtonText = "&Back";
		private readonly string nextButtonText = "&Next";

		/// <summary>
		/// Creates a new control instance.
		/// </summary>
		public WizardControl()
		{
			// Set the default properties.
			base.Padding = new Padding(3);
			// Add the collection pages event handler.
			this.pages.BeforeCleared += this.OnBeforePagesCleared;
			this.pages.AfterItemInserted += this.OnAfterPageInserted;
			this.pages.AfterItemRemoved += this.OnAfterPageRemoved;
			this.pages.AfterItemSet += this.OnAfterPageSet;
		}

		// Public properties.

		/// <summary>
		/// Gets the collection of wizard pages.
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Editor(typeof(CollectionEditor), typeof(UITypeEditor))]
		public ComponentCollection<WizardPage> Pages { get { return this.pages; } }
		/// <summary>
		/// Gets the selected wizard page.
		/// </summary>
		public WizardPage SelectedPage { get { return (this.selectedIndex >= 0) && (this.selectedIndex < this.pages.Count) ? this.pages[this.selectedIndex] : null; } }
		/// <summary>
		/// Gets or sets the selected wizard index.
		/// </summary>
		public int SelectedIndex
		{
			get { return this.selectedIndex; }
			set { this.OnPageSelected(value); }
		}
		/// <summary>
		/// Gets or sets the next button.
		/// </summary>
		public Button NextButton
		{
			get { return this.nextButton; }
			set { this.OnNextButtonSet(value); }
		}
		/// <summary>
		/// Gets or sets the back button.
		/// </summary>
		public Button BackButton
		{
			get { return this.backButton; }
			set { this.OnBackButtonSet(value); }
		}
		/// <summary>
		/// Gets or sets the title label.
		/// </summary>
		public Label TitleLabel
		{
			get { return this.titleLabel; }
			set { this.OnTitleLabelSet(value); }
		}
		/// <summary>
		/// Gets or sets the status label.
		/// </summary>
		public Label StatusLabel
		{
			get { return this.statusLabel; }
			set { this.OnStatusLabelSet(value); }
		}
		/// <summary>
		/// Gets the wizard control padding.
		/// </summary>
		[Browsable(false)]
		public new Padding Padding
		{
			get { return base.Padding; }
			set { /* Do nothing */ }
		}

		// Public events.

		/// <summary>
		/// An event raised when the current page has changed.
		/// </summary>
		public event EventHandler PageChanged;
		/// <summary>
		/// An event raised when the wizard has finished.
		/// </summary>
		public event EventHandler Finished;

		// Public methods.

		/// <summary>
		/// Resets the wizard to the first page. If the wizard is already at the first page, the command is ignored.
		/// </summary>
		public void Reset()
		{
			// Change the selected page.
			this.OnPageSelected(0);
		}

		/// <summary>
		/// Selects the next wizard page. If the wizard is already at the last page, the command is ignorred.
		/// </summary>
		public void SelectNext()
		{
			// If the selected index is at the last page.
			if (this.selectedIndex == this.pages.Count - 1)
			{
				// Call the wizard finished event handler.
				this.OnFinished();
			}
			else
			{
				// Change the selected page.
				this.OnPageSelected(this.selectedIndex + 1);
			}
		}

		/// <summary>
		/// Selects the previous wizard page. If the wizard is already at the first, the command is ignored.
		/// </summary>
		public void SelectBack()
		{
			// Change the selected page.
			this.OnPageSelected(this.selectedIndex - 1);
		}

		// Protected methods.

		/// <summary>
		/// An event handler called when the object is being disposed.
		/// </summary>
		/// <param name="disposed">If <b>true</b>, clean both managed and native resources. If <b>false</b>, clean only native resources.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				// Clear the pages.
				this.pages.Clear();
			}
			// Call the base class method.
			base.Dispose(disposing);
		}

		// Private methods.

		/// <summary>
		/// Changes the selected page at the current index.
		/// </summary>
		/// <param name="selectedIndex">The new selected index.</param>
		private void OnPageSelected(int selectedIndex)
		{
			// If the selected index is less than zero, do nothing.
			if (selectedIndex < -1) selectedIndex = -1;
			// If the selected index is greater than the page count, do nothing.
			if (selectedIndex >= this.pages.Count) selectedIndex = this.pages.Count - 1;
			// If the current index is not -1.
			if (this.selectedIndex >= 0)
			{
				// Hide the current page.
				this.pages[this.selectedIndex].Hide();
			}
			// If the new index is not -1.
			if (selectedIndex >= 0)
			{
				// Show the selected page.
				this.pages[selectedIndex].Show();
			}
			// Change the selected index.
			this.selectedIndex = selectedIndex;
			// Change the controls properties.
			if (null != this.nextButton)
			{
				this.nextButton.Enabled = (this.selectedIndex >= 0) && (this.selectedIndex < this.pages.Count - 1) ? this.pages[this.selectedIndex].NextEnabled : false;
				this.nextButton.Text = this.SelectedPage != null ? this.SelectedPage.NextText : this.nextButtonText;
			}
			if (null != this.backButton)
			{
				this.backButton.Enabled = this.selectedIndex > 0 ? this.pages[this.selectedIndex].BackEnabled : false;
				this.backButton.Text = this.SelectedPage != null ? this.SelectedPage.BackText : this.backButtonText;
			}
			if (null != this.titleLabel)
			{
				this.titleLabel.Text = "Step {0} of {1}: {2}".FormatWith(this.selectedIndex + 1, this.pages.Count, this.SelectedPage != null ? this.SelectedPage.Title : string.Empty);
			}
			if (null != this.statusLabel)
			{
				this.statusLabel.Text = this.SelectedPage != null ? this.SelectedPage.Status : string.Empty;
			}
			// Raise a page changed event.
			if (null != this.PageChanged) this.PageChanged(this, EventArgs.Empty);
		}

		/// <summary>
		/// An event handler called when the wizard has finished.
		/// </summary>
		private void OnFinished()
		{
			// Raise the event.
			if (null != this.Finished) this.Finished(this, EventArgs.Empty);
		}

		/// <summary>
		/// Sets the next button.
		/// </summary>
		/// <param name="button">The next button.</param>
		private void OnNextButtonSet(Button button)
		{
			// If the old button is not null.
			if (null != this.nextButton)
			{
				// Remove the button event handler.
				this.nextButton.Click -= this.OnNextClick;
			}
			// Set the next button.
			this.nextButton = button;
			// If the new button is not null.
			if (null != this.nextButton)
			{
				// Set the button properties.
				this.nextButton.Enabled = (this.selectedIndex >= 0) && (this.selectedIndex < this.pages.Count - 1) ? this.pages[this.selectedIndex].NextEnabled : false;
				this.nextButton.Text = this.SelectedPage != null ? this.SelectedPage.NextText : this.nextButtonText;
				// Add the button event handler.
				this.nextButton.Click += this.OnNextClick;
			}
		}

		/// <summary>
		/// Sets the back button.
		/// </summary>
		/// <param name="button">The back button.</param>
		private void OnBackButtonSet(Button button)
		{
			// If the old button is not null.
			if (null != this.backButton)
			{
				// Remove the button event handler.
				this.backButton.Click -= this.OnBackClick;
			}
			// Set the back button.
			this.backButton = button;
			// If the new button is not null.
			if (null != this.backButton)
			{
				// Set the button properties.
				this.backButton.Enabled = this.selectedIndex > 0 ? this.pages[this.selectedIndex].BackEnabled : false;
				this.backButton.Text = this.SelectedPage != null ? this.SelectedPage.BackText : this.backButtonText;
				// Add the button event handler.
				this.backButton.Click += this.OnBackClick;
			}
		}

		/// <summary>
		/// An event handler called when a new title label is set.
		/// </summary>
		/// <param name="label">The title label.</param>
		private void OnTitleLabelSet(Label label)
		{
			// Set the title label.
			this.titleLabel = label;
			// If the new title label is not null.
			if (null != this.titleLabel)
			{
				// Set the label text.
				this.titleLabel.Text = this.SelectedPage != null ? this.SelectedPage.Title : string.Empty;
			}
		}

		/// <summary>
		/// An event handler called when a new status label is set.
		/// </summary>
		/// <param name="label"></param>
		private void OnStatusLabelSet(Label label)
		{
			// Set the status label.
			this.statusLabel = label;
			// If the new status label is not null.
			if (null != this.statusLabel)
			{
				// Set the label text.
				this.statusLabel.Text = this.SelectedPage != null ? this.SelectedPage.Status : string.Empty;
			}
		}

		/// <summary>
		/// An event handler called when the user clicks on the next button.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void OnNextClick(object sender, EventArgs e)
		{
			// Select the next page.
			this.SelectNext();
		}

		/// <summary>
		/// An event handler called when the user clicks on the back button.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void OnBackClick(object sender, EventArgs e)
		{
			// Select the back page.
			this.SelectBack();
		}

		/// <summary>
		/// An event handler called before the collection of wizard pages is cleared.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void OnBeforePagesCleared(object sender, EventArgs e)
		{
			// Remove and dispose all pages in the collection.
			foreach (WizardPage page in this.pages)
			{
				// Remove the page.
				this.Controls.Remove(page);
			}
		}

		/// <summary>
		/// An event handler called after a wizard page is inserted in the collection.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void OnAfterPageInserted(object sender, ComponentCollection<WizardPage>.ItemChangedEventArgs e)
		{
			// If the item is null, throw an exception.
			if (null == e.Item) throw new NullReferenceException("The wizard page cannot be null.");
			// Set the page properties.
			if (this.selectedIndex == e.Index)
				e.Item.Show();
			else
				e.Item.Hide();
			// Set the page event handlers.
			e.Item.PageChanged += OnPageChanged;
			// Add the page to the control.
			this.Controls.Add(e.Item);
			// If this is the first page, set the selected index to 0.
			if (-1 == this.selectedIndex) this.OnPageSelected(0);
		}

		/// <summary>
		/// An event handler called after a wizard page is removed from the collection.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void OnAfterPageRemoved(object sender, ComponentCollection<WizardPage>.ItemChangedEventArgs e)
		{
			// If the item is null, throw an exception.
			if (null == e.Item) throw new NullReferenceException("The wizard page cannot be null.");
			// Remove the page from the control.
			this.Controls.Remove(e.Item);
			// If this is the last page, set the selected index to -1.
			if (this.pages.Count == 0) this.OnPageSelected(-1);
		}

		/// <summary>
		/// An event handler called after a page was set in the collection.
		/// </summary>
		/// <param name="sender">The sender object.</param>
		/// <param name="e">The event arguments.</param>
		private void OnAfterPageSet(object sender, ComponentCollection<WizardPage>.ItemSetEventArgs e)
		{
			// If the old item is null, throw an exception.
			if (null == e.OldItem) throw new NullReferenceException("The old wizard page cannot be null.");
			// If the new item is null, throw an exception.
			if (null == e.NewItem) throw new NullReferenceException("The new wizard page cannot be null.");
			// If the old and new items are the same, do nothing.
			if (e.OldItem == e.NewItem) return;
			// Remove the old page from the control.
			this.Controls.Remove(e.OldItem);
			// Set the new page properties.
			if (this.selectedIndex == e.Index)
				e.NewItem.Show();
			else
				e.NewItem.Hide();
			// Add the page to the control.
			this.Controls.Add(e.NewItem);
		}

		/// <summary>
		/// An event handler called when a wizard page has changed.
		/// </summary>
		/// <param name="sender">The sender control.</param>
		/// <param name="e">The event arguments.</param>
		private void OnPageChanged(object sender, WizardPageEventArgs e)
		{
			// If the page is different from the current page, do nothing.
			if (e.Page != this.SelectedPage) return;
			// Set the controls properties.
			if (null != this.nextButton)
			{
				this.nextButton.Enabled = (this.selectedIndex >= 0) && (this.selectedIndex < this.pages.Count) ? e.Page.NextEnabled : false;
				this.nextButton.Text = e.Page.NextText;
			}
			if (null != this.backButton)
			{
				this.backButton.Enabled = this.selectedIndex > 0 ? e.Page.BackEnabled : false;
				this.backButton.Text = e.Page.BackText;
			}
			if (null != this.titleLabel)
			{
				this.titleLabel.Text = "Step {0} of {1}: {2}".FormatWith(this.pages.IndexOf(e.Page) + 1, this.pages.Count, e.Page.Title);
			}
			if (null != this.statusLabel)
			{
				this.statusLabel.Text = e.Page.Status;
			}
		}
	}
}
