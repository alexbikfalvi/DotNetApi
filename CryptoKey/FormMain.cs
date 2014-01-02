using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace CryptoKey
{
	public partial class FormMain : Form
	{
		public FormMain()
		{
			InitializeComponent();
		}

		private void OnGenerate(object sender, EventArgs e)
		{
			// Create the AES provider.
			using (AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider())
			{
				// Generate the IV.
				aesProvider.GenerateIV();
				// Generate the key.
				aesProvider.GenerateKey();

				StringBuilder builder = new StringBuilder();

				foreach (byte b in aesProvider.IV)
				{
					builder.AppendFormat("0x{0:X2}, ", b);
				}

				this.textBoxIv.Text = builder.ToString();

				builder.Clear();

				foreach(byte b in aesProvider.Key)
				{
					builder.AppendFormat("0x{0:X2}, ", b);
				}

				this.textBoxKey.Text = builder.ToString();
			}
		}
	}
}
