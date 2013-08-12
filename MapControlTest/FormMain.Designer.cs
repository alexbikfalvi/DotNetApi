namespace MapControlTest
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
			this.buttonProcess = new System.Windows.Forms.Button();
			this.textBox = new System.Windows.Forms.TextBox();
			this.buttonTest = new System.Windows.Forms.Button();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.SuspendLayout();
			// 
			// buttonProcess
			// 
			this.buttonProcess.Location = new System.Drawing.Point(12, 12);
			this.buttonProcess.Name = "buttonProcess";
			this.buttonProcess.Size = new System.Drawing.Size(75, 23);
			this.buttonProcess.TabIndex = 0;
			this.buttonProcess.Text = "Process";
			this.buttonProcess.UseVisualStyleBackColor = true;
			this.buttonProcess.Click += new System.EventHandler(this.OnProcess);
			// 
			// textBox
			// 
			this.textBox.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textBox.Location = new System.Drawing.Point(12, 41);
			this.textBox.Multiline = true;
			this.textBox.Name = "textBox";
			this.textBox.ReadOnly = true;
			this.textBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textBox.Size = new System.Drawing.Size(760, 509);
			this.textBox.TabIndex = 1;
			// 
			// buttonTest
			// 
			this.buttonTest.Location = new System.Drawing.Point(93, 12);
			this.buttonTest.Name = "buttonTest";
			this.buttonTest.Size = new System.Drawing.Size(75, 23);
			this.buttonTest.TabIndex = 2;
			this.buttonTest.Text = "Test";
			this.buttonTest.UseVisualStyleBackColor = true;
			this.buttonTest.Click += new System.EventHandler(this.OnTest);
			// 
			// openFileDialog
			// 
			this.openFileDialog.Filter = "ZIP files (*.zip)|*.zip";
			// 
			// FormMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(784, 562);
			this.Controls.Add(this.buttonTest);
			this.Controls.Add(this.textBox);
			this.Controls.Add(this.buttonProcess);
			this.Name = "FormMain";
			this.Text = "MapControl Test";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button buttonProcess;
		private System.Windows.Forms.TextBox textBox;
		private System.Windows.Forms.Button buttonTest;
		private System.Windows.Forms.OpenFileDialog openFileDialog;

	}
}

