using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DotNetApi.Security;

namespace SecureTextBoxTest
{
	public partial class FormMain : Form
	{
		public FormMain()
		{
			InitializeComponent();
		}

		private void OnTextChanged(object sender, EventArgs e)
		{
			// Convert to string.
			this.label.Text = this.secureTextBox.SecureText.ConvertToUnsecureString();
			// Convert to byte array.
			byte[] bytes = this.secureTextBox.SecureText.ConvertToUnsecureByteArray(Encoding.UTF8);
			this.textBox.Clear();
			foreach (byte b in bytes)
			{
				this.textBox.AppendText(string.Format("{0:X2} ", b));
			}
		}
	}
}
