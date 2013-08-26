using System;
using System.Drawing;
using System.Windows.Forms;
using DotNetApi;
using MapApi;

using System.Xml.Linq;

namespace MapControlTest
{
	public partial class FormMain : Form
	{
		public FormMain()
		{
			InitializeComponent();
		}

		private void OnTest(object sender, EventArgs e)
		{
			try
			{
				this.textBox.AppendText("Creating map...{0}".FormatWith(Environment.NewLine));

				Map map = Map.Read(Maps.Ne110mAdmin0Countries);

				this.textBox.AppendText("Map created.{0}".FormatWith(Environment.NewLine));

				// Set the map.
				this.mapControl.Map = map;
			}
			catch (Exception exception)
			{
				this.textBox.AppendText("Exception. {0}{1}".FormatWith(exception.Message, Environment.NewLine));
			}
		}

		private void OnProcess(object sender, EventArgs e)
		{
		}

		private void OnLongMessage(object sender, EventArgs e)
		{
			this.mapControl.Message = "TeX is a computer program for typesetting documents, created by Donald Knuth.";
		}

		private void OnShortMessage(object sender, EventArgs e)
		{
			this.mapControl.Message = "Blah";
		}
	}
}
