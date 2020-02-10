using System;
using System.IO;

namespace minijoe 
{
	/*
	 * Created by SharpDevelop.
	 * User: a
	 * Date: 2020/1/11
	 * Time: 6:54
	 * 
	 * To change this template use Tools | Options | Coding | Edit Standard Headers.
	 */
	/// <summary>
	/// Description of ByteArrayOutputStream.
	/// </summary>
	public class ByteArrayOutputStream : OutputStream
	{
		private MemoryStream mem;
		
		public ByteArrayOutputStream(int size)
		{
			mem = new MemoryStream(size);
		}
		
		public ByteArrayOutputStream()
		{
			mem = new MemoryStream();
		}
		
		private bool _isDebug = false;
		public void _setDebug(bool isDebug)
		{
			this._isDebug = isDebug;
		}
		public void write(int s)
		{
			if (false)
			{
				if (_isDebug)
				{
					Console.WriteLine("Debug: write: " + (sbyte)(byte)(s & 0xff));
				}
				if (s == 77 || s == 16)
				{
					Console.WriteLine("=================");
				}
			}
			mem.WriteByte((byte)(s & 0xff));
		}
		
		public sbyte[] toByteArray()
		{
			byte[] arr = mem.ToArray();
			sbyte[] result = new sbyte[arr.Length];
			for (int i = 0; i < arr.Length; ++i)
			{
				result[i] = (sbyte)arr[i];
			}
			return result;
		}
		
		public int size()
		{
			return (int)mem.Length;
		}
		
		public void flush()
		{
			mem.Flush();
		}
	}
}
