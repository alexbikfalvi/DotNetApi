﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DotNetApi.Web.XmlRpc;
using DotNetApi.Web;

namespace DotNetApiTest
{
	public partial class FormMain : Form
	{
		private class Structure
		{
			[DisplayName("s")]
			public int Num { get; set; }
			public string Str { get; set; }
		};

		public FormMain()
		{
			InitializeComponent();
		}

		private XmlRpcAsyncRequest request = new XmlRpcAsyncRequest(CultureInfo.InvariantCulture);

		private void Test(object sender, EventArgs e)
		{
			/** Test the XML RPC request

			Structure s = new Structure();
			s.Num = 0;
			s.Str = "dffsd";

			byte[] xml = XmlRpcRequest.Create("name", new object[] {
				0,
				true,
				"This is a string",
				1.234,
				DateTime.Now,
				new int[] { 0, 5, 8 },
				new string[] { "dfsf", "dsf", "fdsf" },
				new object[] { 0, "f", DateTime.Now },
				s
			});

			this.textBox.AppendText(Encoding.UTF8.GetString(xml));
			this.textBox.AppendText("\r\n\r\n");
			StringBuilder hex = new StringBuilder(xml.Length * 3);
			foreach (byte b in xml)
				hex.AppendFormat("{0:x2} ", b);
			this.textBox.AppendText(hex.ToString());
			 * 
			 */
		}
	}
}
