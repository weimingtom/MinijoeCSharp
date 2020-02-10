/*
 * Created by SharpDevelop.
 * User: a
 * Date: 2020/1/11
 * Time: 7:09
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace minijoe 
{
	/// <summary>
	/// Description of Calendar.
	/// </summary>
	public class Calendar
	{
		public const int YEAR = 0;
		public const int MONTH = 1;
		public const int DAY_OF_MONTH = 2;	
		public const int HOUR_OF_DAY = 3;
		public const int MINUTE = 4;
		public const int SECOND = 5;
		public const int MILLISECOND = 6;
		public const int DAY_OF_WEEK = 7;
		
		private static Calendar instance = new Calendar();
		
		public static Calendar getInstance() 
		{
			return instance;
		}
		
		private Calendar()
		{
			
		}
		
		public void setTime(DateTime time)
		{
			throw new Exception();
		}
		
		public DateTime getTime()
		{
			throw new Exception();
			//DateTime.Ticks == Java.Date.getTime()
			return new DateTime();
		}
		
		public void set(int type, int value)
		{
			throw new Exception();
		}
		
		public int get(int type)
		{
			throw new Exception();
			return 0;
		}
		
		public void setTimeZone(Timezone timezone)
		{
			throw new Exception();
		}	
	}
}
