using System;
using Xwt;

namespace ScreenMate
{
	class Program
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
