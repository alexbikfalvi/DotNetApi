namespace LocaleConverter
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
			this.buttonLoad = new System.Windows.Forms.Button();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.textBoxOutput = new System.Windows.Forms.TextBox();
			this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.labelLocales = new System.Windows.Forms.Label();
			this.textBoxLocales = new System.Windows.Forms.TextBox();
			this.buttonRead = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// buttonLoad
			// 
			this.buttonLoad.Location = new System.Drawing.Point(14, 14);
			this.buttonLoad.Name = "buttonLoad";
			this.buttonLoad.Size = new System.Drawing.Size(87, 27);
			this.buttonLoad.TabIndex = 0;
			this.buttonLoad.Text = "&Load";
			this.buttonLoad.UseVisualStyleBackColor = true;
			this.buttonLoad.Click += new System.EventHandler(this.OnLoad);
			// 
			// openFileDialog
			// 
			this.openFileDialog.Filter = "CLDR ZIP files (*.zip)|*.zip";
			this.openFileDialog.Title = "Open CLDR Zip File";
			// 
			// textBoxOutput
			// 
			this.textBoxOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxOutput.Location = new System.Drawing.Point(14, 47);
			this.textBoxOutput.Multiline = true;
			this.textBoxOutput.Name = "textBoxOutput";
			this.textBoxOutput.ReadOnly = true;
			this.textBoxOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textBoxOutput.Size = new System.Drawing.Size(653, 359);
			this.textBoxOutput.TabIndex = 1;
			// 
			// saveFileDialog
			// 
			this.saveFileDialog.Filter = "XML files (*.xml)|*.xml";
			this.saveFileDialog.Title = "Save Locale Resource File";
			// 
			// labelLocales
			// 
			this.labelLocales.AutoSize = true;
			this.labelLocales.Location = new System.Drawing.Point(108, 20);
			this.labelLocales.Name = "labelLocales";
			this.labelLocales.Size = new System.Drawing.Size(49, 15);
			this.labelLocales.TabIndex = 2;
			this.labelLocales.Text = "L&ocales:";
			// 
			// textBoxLocales
			// 
			this.textBoxLocales.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBoxLocales.Location = new System.Drawing.Point(170, 16);
			this.textBoxLocales.Name = "textBoxLocales";
			this.textBoxLocales.Size = new System.Drawing.Size(402, 23);
			this.textBoxLocales.TabIndex = 3;
			this.textBoxLocales.Text = "ca de en es fr pt ro";
			// 
			// buttonRead
			// 
			this.buttonRead.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonRead.Location = new System.Drawing.Point(580, 14);
			this.buttonRead.Name = "buttonRead";
			this.buttonRead.Size = new System.Drawing.Size(87, 27);
			this.buttonRead.TabIndex = 4;
			this.buttonRead.Text = "&Read";
			this.buttonRead.UseVisualStyleBackColor = true;
			this.buttonRead.Click += new System.EventHandler(this.OnRead);
			// 
			// FormMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(681, 418);
			this.Controls.Add(this.buttonRead);
			this.Controls.Add(this.textBoxLocales);
			this.Controls.Add(this.labelLocales);
			this.Controls.Add(this.textBoxOutput);
			this.Controls.Add(this.buttonLoad);
			this.Name = "FormMain";
			this.Text = "Locale Converter";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button buttonLoad;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.TextBox textBoxOutput;
		private System.Windows.Forms.SaveFileDialog saveFileDialog;
		private System.Windows.Forms.Label labelLocales;
		private System.Windows.Forms.TextBox textBoxLocales;
		private System.Windows.Forms.Button buttonRead;
	}
}

