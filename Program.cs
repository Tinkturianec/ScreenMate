using System;
using Xwt;

namespace ScreenMate
{
	static class Program
	{
		[STAThread]
		public static void Main (string[] args)
		{
			Application.Initialize(ToolkitType.Gtk);
			new ScreenMate().Start();
			Application.Run();
		}
	}
}
