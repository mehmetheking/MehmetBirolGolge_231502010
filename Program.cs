using System;
using System.Windows.Forms;
using MehmetBirolGolge_231502010.Forms;

namespace MehmetBirolGolge_231502010
{
	internal static class Program
	{
		/// <summary>
		/// Uygulamanın ana girdi noktası.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			// LoginForm ile başla
			Application.Run(new LoginForm());
		}
	}
}