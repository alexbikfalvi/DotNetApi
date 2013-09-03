using System;
using System.IO;
using System.Windows.Forms;

namespace DotNetApi.Windows
{
	public static class Cursors
	{
		static Cursors()
		{
			Cursors.HandGrab = Cursors.FromBytes(Resources.CursorHandGrab);
		}

		public static Cursor HandGrab { get; private set; }

		private static Cursor FromBytes(byte[] data)
		{
			using (MemoryStream memoryStream = new MemoryStream(data))
			{
				return new Cursor(memoryStream);
			}
		}
	}
}
