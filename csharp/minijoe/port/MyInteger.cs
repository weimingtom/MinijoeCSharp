/*
 * Created by SharpDevelop.
 * User: a
 * Date: 2020/1/11
 * Time: 7:17
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace minijoe 
{
	/// <summary>
	/// Description of MyInteger.
	/// </summary>
	public class MyInteger
	{
		private MyInteger()
		{
			throw new Exception();
		}
		
		public static String toBinaryString(int t)
		{
			throw new Exception();
			return "";
		}
		
		public static string toHexString(int t)
		{
			return t.ToString("x");
		}
		
		public static string toString(int x, int toBase)
		{
			return Convert.ToString(x, toBase);
		}
		
		//---------------------------
		//not used
		
	    public static byte[] IntToBitConverter(int num)
	    {
	        byte[] bytes = BitConverter.GetBytes(num);
	        return bytes;
	    }
	    
	    public static int IntToBitConverter(byte[] bytes)
	    {
	        int temp = BitConverter.ToInt32(bytes, 0);
	        return temp;
	    }	
	}
}
