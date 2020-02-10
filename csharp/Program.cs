/*
 * Created by SharpDevelop.
 * User: a
 * Date: 2020/1/11
 * Time: 6:31
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using minijoe;

namespace minijoe
{
	class Program
	{
		public static void Main(string[] args)
		{
			//Console.WriteLine("Hello World!");
			
			// TODO: Implement Functionality Here
			MjShell.Main_(args);
			
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
	}
}