namespace SecureTextBoxTest
{
	partial class FormMain
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Security.SecureString secureString3 = new System.Security.SecureString();
			this.secureTextBox = new DotNetApi.Windows.Controls.SecureTextBox();
			this.label = new System.Windows.Forms.Label();
			this.textBox = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// secureTextBox
			// 
			this.secureTextBox.Location = new System.Drawing.Point(12, 12);
			this.secureTextBox.Name = "secureTextBox";
			this.secureTextBox.SecureText = secureString3;
			this.secureTextBox.Size = new System.Drawing.Size(260, 20);
			this.secureTextBox.TabIndex = 0;
			this.secureTextBox.UseSystemPasswordChar = true;
			this.secureTextBox.TextChanged += new System.EventHandler(this.OnTextChanged);
			// 
			// label
			// 
			this.label.AutoSize = true;
			this.label.Location = new System.Drawing.Point(12, 35);
			this.label.Name = "label";
			this.label.Size = new System.Drawing.Size(0, 13);
			this.label.TabIndex = 1;
			// 
			// textBox
			// 
			this.textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textBox.Location = new System.Drawing.Point(12, 51);
			this.textBox.Multiline = true;
			this.textBox.Name = "textBox";
			this.textBox.ReadOnly = true;
			this.textBox.Size = new System.Drawing.Size(260, 199);
			this.textBox.TabIndex = 2;
			// 
			// FormMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 262);
			this.Controls.Add(this.textBox);
			this.Controls.Add(this.label);
			this.Controls.Add(this.secureTextBox);
			this.Name = "FormMain";
			this.Text = "Form1";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private DotNetApi.Windows.Controls.SecureTextBox secureTextBox;
		private System.Windows.Forms.Label label;
		private System.Windows.Forms.TextBox textBox;
	}
}

