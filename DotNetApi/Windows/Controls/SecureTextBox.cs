/* 
 * Copyright (C) 2012-2013 Alex Bikfalvi
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
using System.Security;
using System.Windows.Forms;
using DotNetApi.Security;

namespace DotNetApi.Windows.Controls
{
	/// <summary>
	/// A class representing a secure text box.
	/// </summary>
	public class SecureTextBox : TextBox
	{
		private SecureString secureText = new SecureString();
		private static char passwordChar = '●';
		private bool keyProcessed = false;

		/// <summary>
		/// Creates a new secure text box control instance.
		/// </summary>
		public SecureTextBox()
		{
			this.UseSystemPasswordChar = true;
		}

		// Public properties.

		/// <summary>
		/// Gets or sets the plain text for the current secure text.
		/// </summary>
		public override string Text
		{
			get { return base.Text; }
			set
			{
				this.OnSecureTextSet(value.ConvertToSecureString());
			}
		}

		/// <summary>
		/// Gets or sets the secure text.
		/// </summary>
		public SecureString SecureText
		{
			get { return this.secureText; }
			set
			{
				// Call the event handler.
				this.OnSecureTextSet(value);
			}
		}

		/// <summary>
		/// Clears the text of the current secure text box.
		/// </summary>
		public new void Clear()
		{
			// Clear the secure text.
			this.secureText.Clear();
			// Call the base class method.
			base.Clear();
		}

		// Protected methods.

		/// <summary>
		/// Processes a keyboard message.
		/// </summary>
		/// <param name="m">The keyboard message.</param>
		/// <returns><b>True</b> if the message was processed by the control or <b>false</b> otherwise.</returns>
		protected override bool ProcessKeyMessage(ref Message m)
		{
			// If the last character was not an input character.
			if (this.keyProcessed)
			{
				// Use the base class method to process the message.
				return base.ProcessKeyMessage(ref m);
			}
			else
			{
				// Otherwise, consider the message was processed.
				this.keyProcessed = true;
				return true;
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing"<b>True</b> if managed resources should be disposed; otherwise, <b>false</b>.</param>
		protected override void Dispose(bool disposing)
		{
			// Call the base class disposing function.
			base.Dispose(disposing);
			// Dispose the secure text.
			this.secureText.Dispose();
		}

		/// <summary>
		/// Determines if the character is an input character that the control recognizes.
		/// </summary>
		/// <param name="charCode">The character code.</param>
		/// <returns><b>True</b> if the character is recognized, or <b>false</b> otherwise.</returns>
		protected override bool IsInputChar(char charCode)
		{
			// Call the base class method.
			bool isChar = base.IsInputChar(charCode);
			// Get the cursor position.
			int position = this.SelectionStart;
			// If the current character is recognized.
			if (isChar)
			{
				// Get the current character code.
				int keyCode = (int)charCode;
				// If the current character is not a control/cursor character.
				if (!char.IsControl(charCode) && !char.IsHighSurrogate(charCode) && !char.IsLowSurrogate(charCode))
				{
					// Add the type character to the secure string instance.

					// If there are selected characters, remove the selected characters.
					for (int index = 0; index < this.SelectionLength; index++)
					{
						this.secureText.RemoveAt(this.SelectionStart);
					}

					// Add the new character at the cursor position in the secure text.
					if (position == this.secureText.Length)
					{
						this.secureText.AppendChar(charCode);
					}
					else
					{
						this.secureText.InsertAt(position, charCode);
					}
					// Update the displayed text.
					base.Text = new string(SecureTextBox.passwordChar, this.secureText.Length);
					// Increment the caret position.
					this.SelectionStart = position + 1;
					// Set key processed to false.
					this.keyProcessed = false;
				}
				else
				{
					//  Check which character has been pressed.
					switch (keyCode)
					{
						case (int)Keys.Back: // The user pressed the backspace key.
							// If no text is selected, and the caret position is not at the beginning.
							if ((this.SelectionLength == 0) && (position > 0))
							{
								// Decrement the position.
								position--;
								// Remove the character at the position.
								this.secureText.RemoveAt(position);
							}
							else if (this.SelectionLength > 0)
							{
								// Remove the selection.
								for (int index = 0; index < this.SelectionLength; index++)
								{
									this.secureText.RemoveAt(this.SelectionStart);
								}
							}
							// Update the text.
							base.Text = new string(SecureTextBox.passwordChar, this.secureText.Length);
							// Update the caret position.
							this.SelectionStart = position;
							// Set key processed to false.
							this.keyProcessed = false;
							break;
					}
				}
			}
			else
			{
				this.keyProcessed = true;
			}
			// Return the value.
			return isChar;
		}

		/// <summary>
		/// Determines whether the specified key is an input key or a special key that requires preprocessing.
		/// </summary>
		/// <param name="keyData">The key value.</param>
		/// <returns><b>True</b> if the specified key is an input key; otherwise, <b>false</b>.</returns>
		protected override bool IsInputKey(Keys keyData)
		{
			// If the current key is the delete key.
			if ((keyData & Keys.Delete) == Keys.Delete)
			{
				// Get the caret position.
				int position = this.SelectionStart;
				// If there is selecte text, remove the selected text.
				if (this.SelectionLength > 0)
				{
					for (int index = 0; index < this.SelectionLength; index++)
					{
						this.secureText.RemoveAt(this.SelectionStart);
					}
				}
				else if (this.SelectionStart < this.secureText.Length)
				{
					this.secureText.RemoveAt(this.SelectionStart);
				}
				// Update the text.
				base.Text = new string(SecureTextBox.passwordChar, this.secureText.Length);
				// Set key processed to false.
				this.keyProcessed = false;
				// Set the caret position.
				this.SelectionStart = position;
				// Return true.
				return true;
			}
			// Call the base class method.
			return base.IsInputKey(keyData);
		}

		/// <summary>
		/// An event handler called when the text of the secure text box has been set.
		/// </summary>
		/// <param name="text">The new text.</param>
		protected virtual void OnSecureTextSet(SecureString text)
		{
			// If the secure text is null.
			if (null == text)
			{
				// Clear the current secure text.
				this.secureText.Clear();
				// Clear the displayed text.
				base.Text = string.Empty;
			}
			else
			{
				// Dispose the current secure text.
				this.secureText.Dispose();
				// Set the new secure text.
				this.secureText = text;
				// Update the displayed text.
				base.Text = new string(SecureTextBox.passwordChar, this.secureText.Length);
			}
		}
	}
}
