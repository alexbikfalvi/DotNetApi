using System;
using System.Drawing;
using System.Windows.Forms;
using MapApi.IO;

namespace MapControlTest
{
	public partial class FormMain : Form
	{
		public FormMain()
		{
			InitializeComponent();
		}

		private void OnProcess(object sender, EventArgs e)
		{
			//JObject obj = JObject.Parse(Resources.World);
			//this.ParseToken(obj, 0);
		}

		//private void ParseToken(JToken parent, int level)
		//{
		//	this.textBox.AppendText(string.Format("{0} L{1} : {2} ", new string(' ', level), level, parent.Type));
		//	switch (parent.Type)
		//	{
		//		case JTokenType.Property:
		//			JProperty property = parent as JProperty;
		//			this.textBox.AppendText(string.Format("\'{0}\' ", property.Name));
		//			break;
		//		case JTokenType.Float:
		//		case JTokenType.String:
		//			JValue value = parent as JValue;
		//			this.textBox.AppendText(string.Format("\'{0}\' ", value.Value));
		//			break;

		//	}
		//	this.textBox.AppendText(Environment.NewLine);
		//	foreach (JToken token in parent.Children())
		//	{
		//		this.ParseToken(token, level + 1);
		//	}
		//}

		private void OnTest(object sender, EventArgs e)
		{
			if (this.openFileDialog.ShowDialog(this) == DialogResult.OK)
			{
				using (ShapeFile shapeFile = new ShapeFile(this.openFileDialog.FileName))
				{
				}
			}
		}
	}
}
