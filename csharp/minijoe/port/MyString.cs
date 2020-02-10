/*
 * Created by SharpDevelop.
 * User: a
 * Date: 2020/2/4
 * Time: 11:32
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text;

namespace minijoe
{
	/// <summary>
	/// Description of MyString.
	/// </summary>
	public class MyString
	{
		private MyString()
		{
		}
		
		public static sbyte[] getBytes(string str)
		{
			byte[] bytes = Encoding.Default.GetBytes(str);
		  	sbyte[] bytes2 = new sbyte[bytes.Length];
		  	for (int i = 0; i < bytes.Length; ++i)
		  	{
		  		bytes2[i] = (sbyte)bytes[i];
		  	}
		  	return bytes2;
		}
	}
}
