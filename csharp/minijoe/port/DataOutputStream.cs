/*
 * Created by SharpDevelop.
 * User: a
 * Date: 2020/1/11
 * Time: 6:55
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text;

namespace minijoe 
{
	/// <summary>
	/// Description of DataOutputStream.
	/// </summary>
	public class DataOutputStream
	{
		private ByteArrayOutputStream ostr;
		
		public DataOutputStream(OutputStream ostr)
		{
			this.ostr = (ByteArrayOutputStream)ostr;
		}
		
		public void _setDebug(bool enable)
		{
			this.ostr._setDebug(enable);
		}
		
		public void write(sbyte s)
		{
			ostr.write((int)s);
		}
		
		public void write(char s)
		{
			ostr.write((int)s);
		}
		
		public void write(sbyte[] s)
		{
			for (int i = 0; i < s.Length; ++i)
			{
				ostr.write((int)s[i]);
			}
		}
		
		public void write(int t)
		{
			ostr.write((byte)(((uint)t) & 0xff));
		}
		
		public void writeShort(int t)
		{
			byte[] s = BitConverter.GetBytes((short)t);
			for (int i = 0; i < s.Length; ++i)
			{
				ostr.write((int)s[i]);
			}
		}
		
		public void flush()
		{
			ostr.flush();
		}
		
		public void writeUTF(String str)
		{
			byte[] s = Encoding.GetEncoding("UTF-8").GetBytes(str);
			writeShort(s.Length);
			for (int i = 0; i < s.Length; ++i)
			{
				ostr.write((int)s[i]);
			}
		}
		
		public void writeDouble(double v)
		{
			byte[] s = BitConverter.GetBytes(v);
			for (int i = 0; i < s.Length; ++i)
			{
				ostr.write((int)s[i]);
			}
		}
	}
}
