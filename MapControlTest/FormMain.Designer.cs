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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
			this.buttonProcess = new System.Windows.Forms.Button();
			this.textBox = new System.Windows.Forms.TextBox();
			this.buttonTest = new System.Windows.Forms.Button();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.buttonLongMessage = new System.Windows.Forms.Button();
			this.buttonShortMessage = new System.Windows.Forms.Button();
			this.mapControl = new DotNetApi.Windows.Controls.MapControl();
			this.checkBoxVisible = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// buttonProcess
			// 
			this.buttonProcess.Location = new System.Drawing.Point(93, 12);
			this.buttonProcess.Name = "buttonProcess";
			this.buttonProcess.Size = new System.Drawing.Size(75, 23);
			this.buttonProcess.TabIndex = 1;
			this.buttonProcess.Text = "Process";
			this.buttonProcess.UseVisualStyleBackColor = true;
			this.buttonProcess.Click += new System.EventHandler(this.OnProcess);
			// 
			// textBox
			// 
			this.textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.textBox.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.textBox.Location = new System.Drawing.Point(12, 457);
			this.textBox.Multiline = true;
			this.textBox.Name = "textBox";
			this.textBox.ReadOnly = true;
			this.textBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textBox.Size = new System.Drawing.Size(760, 93);
			this.textBox.TabIndex = 2;
			// 
			// buttonTest
			// 
			this.buttonTest.Location = new System.Drawing.Point(12, 12);
			this.buttonTest.Name = "buttonTest";
			this.buttonTest.Size = new System.Drawing.Size(75, 23);
			this.buttonTest.TabIndex = 0;
			this.buttonTest.Text = "Test";
			this.buttonTest.UseVisualStyleBackColor = true;
			this.buttonTest.Click += new System.EventHandler(this.OnTest);
			// 
			// openFileDialog
			// 
			this.openFileDialog.Filter = "ZIP files (*.zip)|*.zip";
			// 
			// buttonLongMessage
			// 
			this.buttonLongMessage.Location = new System.Drawing.Point(174, 12);
			this.buttonLongMessage.Name = "buttonLongMessage";
			this.buttonLongMessage.Size = new System.Drawing.Size(75, 23);
			this.buttonLongMessage.TabIndex = 4;
			this.buttonLongMessage.Text = "Long msg";
			this.buttonLongMessage.UseVisualStyleBackColor = true;
			this.buttonLongMessage.Click += new System.EventHandler(this.OnLongMessage);
			// 
			// buttonShortMessage
			// 
			this.buttonShortMessage.Location = new System.Drawing.Point(255, 12);
			this.buttonShortMessage.Name = "buttonShortMessage";
			this.buttonShortMessage.Size = new System.Drawing.Size(75, 23);
			this.buttonShortMessage.TabIndex = 5;
			this.buttonShortMessage.Text = "Short msg";
			this.buttonShortMessage.UseVisualStyleBackColor = true;
			this.buttonShortMessage.Click += new System.EventHandler(this.OnShortMessage);
			// 
			// mapControl
			// 
			this.mapControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.mapControl.Location = new System.Drawing.Point(12, 41);
			this.mapControl.Name = "mapControl";
			this.mapControl.Size = new System.Drawing.Size(760, 410);
			this.mapControl.TabIndex = 3;
			// 
			// checkBoxVisible
			// 
			this.checkBoxVisible.AutoSize = true;
			this.checkBoxVisible.Checked = true;
			this.checkBoxVisible.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBoxVisible.Location = new System.Drawing.Point(336, 16);
			this.checkBoxVisible.Name = "checkBoxVisible";
			this.checkBoxVisible.Size = new System.Drawing.Size(78, 17);
			this.checkBoxVisible.TabIndex = 6;
			this.checkBoxVisible.Text = "Visible msg";
			this.checkBoxVisible.UseVisualStyleBackColor = true;
			this.checkBoxVisible.CheckedChanged += new System.EventHandler(this.OnMessageVisibleChanged);
			// 
			// FormMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(784, 562);
			this.Controls.Add(this.checkBoxVisible);
			this.Controls.Add(this.buttonShortMessage);
			this.Controls.Add(this.buttonLongMessage);
			this.Controls.Add(this.mapControl);
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
		private DotNetApi.Windows.Controls.MapControl mapControl;
		private System.Windows.Forms.Button buttonLongMessage;
		private System.Windows.Forms.Button buttonShortMessage;
		private System.Windows.Forms.CheckBox checkBoxVisible;

	}
}

