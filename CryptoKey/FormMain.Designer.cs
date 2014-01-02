namespace CryptoKey
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
			this.buttonGenerate = new System.Windows.Forms.Button();
			this.tableLayout = new System.Windows.Forms.TableLayoutPanel();
			this.labelIv = new System.Windows.Forms.Label();
			this.textBoxIv = new System.Windows.Forms.TextBox();
			this.labelKey = new System.Windows.Forms.Label();
			this.textBoxKey = new System.Windows.Forms.TextBox();
			this.tableLayout.SuspendLayout();
			this.SuspendLayout();
			// 
			// buttonGenerate
			// 
			this.buttonGenerate.Location = new System.Drawing.Point(12, 12);
			this.buttonGenerate.Name = "buttonGenerate";
			this.buttonGenerate.Size = new System.Drawing.Size(75, 23);
			this.buttonGenerate.TabIndex = 0;
			this.buttonGenerate.Text = "&Generate";
			this.buttonGenerate.UseVisualStyleBackColor = true;
			this.buttonGenerate.Click += new System.EventHandler(this.OnGenerate);
			// 
			// tableLayout
			// 
			this.tableLayout.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayout.ColumnCount = 1;
			this.tableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayout.Controls.Add(this.labelIv, 0, 0);
			this.tableLayout.Controls.Add(this.textBoxIv, 0, 1);
			this.tableLayout.Controls.Add(this.labelKey, 0, 2);
			this.tableLayout.Controls.Add(this.textBoxKey, 0, 3);
			this.tableLayout.Location = new System.Drawing.Point(12, 41);
			this.tableLayout.Name = "tableLayout";
			this.tableLayout.RowCount = 4;
			this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayout.Size = new System.Drawing.Size(560, 309);
			this.tableLayout.TabIndex = 1;
			// 
			// labelIv
			// 
			this.labelIv.AutoSize = true;
			this.labelIv.Location = new System.Drawing.Point(3, 0);
			this.labelIv.Name = "labelIv";
			this.labelIv.Size = new System.Drawing.Size(97, 13);
			this.labelIv.TabIndex = 0;
			this.labelIv.Text = "Initialization &vector:";
			// 
			// textBoxIv
			// 
			this.textBoxIv.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBoxIv.Location = new System.Drawing.Point(3, 16);
			this.textBoxIv.Multiline = true;
			this.textBoxIv.Name = "textBoxIv";
			this.textBoxIv.ReadOnly = true;
			this.textBoxIv.Size = new System.Drawing.Size(554, 135);
			this.textBoxIv.TabIndex = 1;
			// 
			// labelKey
			// 
			this.labelKey.AutoSize = true;
			this.labelKey.Location = new System.Drawing.Point(3, 154);
			this.labelKey.Name = "labelKey";
			this.labelKey.Size = new System.Drawing.Size(28, 13);
			this.labelKey.TabIndex = 2;
			this.labelKey.Text = "&Key:";
			// 
			// textBoxKey
			// 
			this.textBoxKey.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBoxKey.Location = new System.Drawing.Point(3, 170);
			this.textBoxKey.Multiline = true;
			this.textBoxKey.Name = "textBoxKey";
			this.textBoxKey.ReadOnly = true;
			this.textBoxKey.Size = new System.Drawing.Size(554, 136);
			this.textBoxKey.TabIndex = 3;
			// 
			// FormMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(584, 362);
			this.Controls.Add(this.tableLayout);
			this.Controls.Add(this.buttonGenerate);
			this.Name = "FormMain";
			this.Text = "Cryptographic Key Generator";
			this.tableLayout.ResumeLayout(false);
			this.tableLayout.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button buttonGenerate;
		private System.Windows.Forms.TableLayoutPanel tableLayout;
		private System.Windows.Forms.Label labelIv;
		private System.Windows.Forms.TextBox textBoxIv;
		private System.Windows.Forms.Label labelKey;
		private System.Windows.Forms.TextBox textBoxKey;
	}
}

