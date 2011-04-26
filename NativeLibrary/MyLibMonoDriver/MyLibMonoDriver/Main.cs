using System;
using System.Runtime.InteropServices;

namespace MyLibMonoDriver
{
	class MainClass
	{
		[DllImport("MyLib")]
		public static extern int HelloWorld ();

		[DllImport("MyLib")]
		public static extern void camTexture (int nTexId, int width, int height, IntPtr dataPtr);


		public static void Main (string[] args)
		{
			Console.WriteLine ("Starting Main...");
			Console.WriteLine ("Number from Mono: " + HelloWorld ());
			
			IntPtr x;
			camTexture (0, 640, 480, x);
			Console.WriteLine ("After console.");
		}
	}
}

