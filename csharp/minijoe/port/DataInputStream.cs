/*
 * Created by SharpDevelop.
 * User: a
 * Date: 2020/1/11
 * Time: 6:54
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Text;

namespace minijoe 
{
	/// <summary>
	/// Description of DataInputStream.
	/// </summary>
	public class DataInputStream
	{
		private MemoryStream istr;
		
		public DataInputStream(ByteArrayInputStream instr)
		{
			byte[] buf = new Byte[instr.arr.Length];
			for (int i = 0; i < instr.arr.Length; ++i)
			{
				buf[i] = (byte)instr.arr[i];
			}
			istr = new MemoryStream(buf);
		}
		
		public int readUnsignedShort()
		{
			byte[] buf = new byte[2];
			istr.Read(buf, 0, buf.Length);
			return (int)BitConverter.ToUInt16(buf, 0);
		}
		
		public int read()
		{
			return istr.ReadByte();
		}
		
		public String readUTF()
		{
			int len = readShort();
			byte[] buf = new byte[len];
			istr.Read(buf, 0, buf.Length);
			
			return Encoding.GetEncoding("UTF-8").GetString(buf);
		}
		
		public double readDouble()
		{
			byte[] buf = new byte[8];
			istr.Read(buf, 0, buf.Length);
			return (double)BitConverter.ToDouble(buf, 0);		
		}
		
		public void readFully(sbyte[] str)
		{
			for (int i = 0; i < str.Length; ++i)
			{
				str[i] = (sbyte)istr.ReadByte();
			}
		}
		
		public void readFully(sbyte[] str, int offset, int len)
		{
			for (int i = offset; i < offset + len; ++i)
			{
				str[i] = (sbyte)istr.ReadByte();
			}
		}
		
		public int readShort()
		{
			byte[] buf = new byte[2];
			istr.Read(buf, 0, buf.Length);
			return (int)BitConverter.ToInt16(buf, 0);
		}
	}
}
