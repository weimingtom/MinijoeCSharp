using System;
using System.Text;

// Copyright 2010 Google Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace minijoe {

//import java.io.UnsupportedEncodingException;
//import java.util.Calendar;

/**
 * A set of simple static utility methods for CLDC.
 *  
 * @author Peter Baldwin
 * @author Stefan Haustein
 */
public class Util {

  private static readonly String[] HTTP_SPEC_DAYS = {
    "Sun", "Mon", "Tue", " Wed", "Thu", "Fri", "Sat"};
  private static readonly String[] HTTP_SPEC_MONTHS = {
    "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"};
  private const string HTTP_TIME_ZONE = "GMT";
  private const string HEX_DIGITS = "0123456789ABCDEF";

  // If we have a numeric time zone then there should be
  // 5 characters present ie [+-]HH:MM
  private const int NUMERIC_TIME_ZONE_LENGTH = 6;

  // Time section should be at least this long ie THHMMSS
  private const int MIN_TIME_STRING_LENGTH = 7;

  /**
   * Returns a string array created by splitting the given string at the given separator.
   */
  public static String[] split(String target, char separator) {
    int separatorInstances = 0;
		int targetLength = target.Length;
		for (int index = target.IndexOf(separator, 0); 
    index != -1 && index < targetLength;
		index = target.IndexOf(separator, index)) {
      separatorInstances++;
      // Skip over separators
      if (index >= 0) {
        index++;
      }
    }
    String[] results = new String[separatorInstances + 1];
    int beginIndex = 0;
    for (int i = 0; i < separatorInstances; i++) {
	  int endIndex = target.IndexOf(separator, beginIndex);
	  results[i] = target.Substring(beginIndex, endIndex - beginIndex);
      beginIndex = endIndex + 1;
    }
    // Last piece (or full string if there were no separators).
	results[separatorInstances] = target.Substring(beginIndex);
    return results;
  }

  /**
   * Resolves a base URL and a relative URL to an absolute URL, e.g.
   * "http://acme.com/prod/index.html" and "foo.html" to
   * "http://acme.com/prod/foo.html". If the relative URL parameter contains
   * an absolute URL, it is returned unchanged.
   *
   * @param baseUrl the absolute base URL used for resolving the URL. If null,
   *    the relative URL is returned
   * @param relative URL, must not be null
   * @return the resulting absolute URL
   */
  public static String getAbsoluteUrl(String baseUrl, String relative) {

    if (baseUrl == null || baseUrl.Length == 0) {
      return relative;
    }

    // If there is a colon that is not separating a drive letter (c:) and that
    // is not or beyond the longest known protocol (6th char in "https://"),
    // we assume that the second parameter is an absolute address and return it
    // unchanged.
    int colPos = relative.IndexOf(':');
    if (colPos > 1 && colPos < 7) {
      return relative;
    }

    if (relative.StartsWith("//")) {
      int cut = baseUrl.IndexOf("//");
	  return (cut == -1 ? baseUrl : baseUrl.Substring(0, cut)) + relative;
    }

    // cut off query and label
		int cutH = baseUrl.IndexOf('#');
		int cutQ = baseUrl.IndexOf('?');
    if (cutH != -1 || cutQ != -1) {
      int cut;
      if (cutH != -1 && cutQ != -1) {
	    cut = Math.Min(cutH, cutQ);
      } else if (cutH != -1) {
        cut = cutH;
      } else {
        cut = cutQ;
      }
	  baseUrl = baseUrl.Substring(0, cut);
    }

	colPos = baseUrl.IndexOf(':');
	if (relative.StartsWith("/")) {
	  int cut = baseUrl.IndexOf('/', colPos + 3);
	  return (cut == -1 ? baseUrl : baseUrl.Substring(0, cut)) + relative;
    }

	int cut2 = baseUrl.LastIndexOf('/');

    if (cut2 > colPos + 2) {
	  return baseUrl.Substring(0, cut2 + 1) + relative;
    }

    return baseUrl + '/' + relative;
  }

  /**
   * Find the index of the given object in the given array.
   */
  public static int indexOf(Object[] array, Object s) {
    for (int i = 0; i < array.Length; i++) {
      if (s == null) {
        if (array[i] == null) {
          return i;
        }
      } else {
	    if (s.Equals(array[i])) {
          return i;
        }
      }
    }
    return -1;
  }

  /**
   * Throws a runtime exception if i1 != i2.
   */
  public static void assertEquals(int i1, int i2) {
    if(i1 != i2) {
      throw new Exception("assert " + i1 + " == " + i2);
    }
  }

  /**
   * Returns true if a line break is valid between the characters c and d.
   */
  public static bool canBreak(char c, char d) {
    if (c <= ' ' || c == '(') {
      return true;
    } 
    if (d > ' ') {
	  return "-.,/+)!?;".IndexOf(c) != -1;
    }
    return false;
  }


  /**
   * Encodes a String into UTF-8 for use in an URL query string or html POST
   * data.
   */
  public static String encodeURL(String s) {
    try {
	  byte[] utf8 = Encoding.GetEncoding("UTF-8").GetBytes(s);

	  int len = utf8.Length;
	  StringBuilder sbuf = new StringBuilder(len);

      for (int i = 0; i < len; i++) {
        int ch = utf8[i];

        if (('A' <= ch && ch <= 'Z') || ('a' <= ch && ch <= 'z') || ('0' <= ch && ch <= '9') || (
            ch == '-' || ch == '_' || ch == '.' || ch == '!' || ch == '~' || ch == '*' || 
            ch == '\'' || ch == '(' || ch == ')')) {
		  sbuf.Append((char) ch);
        } else if (ch == ' ') {
		  sbuf.Append('+');
        } else {
		  sbuf.Append('%');
		  sbuf.Append(HEX_DIGITS[(ch >> 4) & 0xF]);
		  sbuf.Append(HEX_DIGITS[ch & 0xF]);
        }
      }
	  return sbuf.ToString();
	} catch (ArgumentException) {
	  throw new Exception();
    }
  }

  /**
   * Throws a runtime exception if b is false.
   */
  public static void assertTrue(bool b) {
    if(!b) {
	  throw new Exception("assertTrue: " + b);
    }
  }

  /**
   * Parses a time string in the format defined in RFC 1036 or the 
   * Netscape HTTP cookie specification (Weekday, DD-Mon-YYYY HH:MM:SS GMT)
   * 
   * @param s the time string
   * @return milliseconds since  1970-01-01 00:00:00 GMT. 
   */
  public static long parseHttpDateTime(String s) {
    int cut = s.IndexOf(',');
	s = s.Substring(cut + 1).Trim();

    String[] parts = split(s, ' ');
    String[] dateParts;
    int timeIndex;

	if (parts[0].Length > 2) {
      dateParts = split(parts[0], '-');
      timeIndex = 1;
    } else {
      dateParts = parts;
      timeIndex = 3;
    }

	if (dateParts.Length < 3 || timeIndex + 1 >= parts.Length) {
      throw new FormatException();
    }

    int day = Convert.ToInt32(dateParts[0]);
    String monthName = dateParts[1];
	if (monthName.Length < 3) {
      throw new FormatException();
    }
	int year = Convert.ToInt32(dateParts[2]);
	if (dateParts[2].Length == 2) {
      year += 2000; 
    }

	monthName = Char.ToUpper(monthName[0]) + 
	monthName.Substring(1).ToLower();

    int month = indexOf(HTTP_SPEC_MONTHS, monthName);
    if (month == -1) {
      throw new FormatException();
    }

    Calendar calendar = Calendar.getInstance();
    calendar.set(Calendar.YEAR, year);
    calendar.set(Calendar.MONTH, month);
    calendar.set(Calendar.DAY_OF_MONTH, day);

    String time = parts[timeIndex];
	String timezone = parts[timeIndex + 1].ToUpper();
	if ("GMT".Equals(timezone)) {
      timezone = "Z";
    }
    parseTime('T' + time + timezone, calendar);

	long result = calendar.getTime().Ticks;
    return result;
  }   

  /**
   * Parses a time string in the format [Tt]HH:MM:SS(.[0-9]+)?
   * followed by an optional timezone which is [Zz] or [-+]HH:MM
   * 
   * @param time the time string that will be parsed
   * @param calendar the calendar object that will be updated with the parsed time
   * @return
   */
  private static long parseTime(String time, Calendar calendar) {
		if (time.Length < MIN_TIME_STRING_LENGTH) {
      throw new FormatException(); 
    }

    long timeZoneOffset = 0;

	char first = time[0];
    // There should be a t separating the date and time and
    // it should be long enought to fit "THHMMSS".
	if (time.Length < MIN_TIME_STRING_LENGTH || 
        (first != 'T' && first != 't')) {
      throw new FormatException();
    }
    // everything after the 'T' or 't'
		time = time.Substring(1);

		int value = Convert.ToInt32(time.Substring(0, 2));
    if (value > 23) {
      throw new FormatException();
    }
    calendar.set(Calendar.HOUR_OF_DAY, value);
	if (time[2] == ':') {
	  time = time.Substring(3);
    } else {
	  time = time.Substring(2);
    }

	value = Convert.ToInt32(time.Substring(0, 2));
    if (value > 59) {
      throw new FormatException();
    }
    calendar.set(Calendar.MINUTE, value);
	if (time[2] == ':') {
	  time = time.Substring(3);
    } else {
	  time = time.Substring(2);
    }

	value = Convert.ToInt32(time.Substring(0, 2));
    if (value > 59) {
      throw new FormatException();
    }
    calendar.set(Calendar.SECOND, value);
	time = time.Substring(2);

    calendar.set(Calendar.MILLISECOND, 0);

	if (time.Length > 0) {
      try {
        // If the next character is a '.' we have a miliseconds specified in
        // the string.
        int currentStringPos = 0;
		if (time[currentStringPos] == '.') {
          ++currentStringPos;
          // Next character must be a digit
		  if (!Char.IsDigit(time[currentStringPos])) {
            throw new FormatException();         
          }
          int multiplier = 100;
          int milliseconds =
		    MyChar.digit(time[currentStringPos], 10) * multiplier;

          // There may be 0 or more more digits to process
          // Note if there are more than 3 digits in total we will be ignoring
          // the fractions of a millisecond sepecified.
          ++currentStringPos;
		  while (currentStringPos < time.Length && 
		    Char.IsDigit(time[currentStringPos])) {
            multiplier /= 10;
			milliseconds += MyChar.digit(time[currentStringPos], 10) * multiplier;
            currentStringPos++;
          }
          calendar.set(Calendar.MILLISECOND, milliseconds);
        }
        // If we still have characters to process this should
        // be timezone information. Which is either a 'Z' or
        // +HH:MM or -HH:MM
		if (currentStringPos < time.Length) {
		  String timezone = time.Substring(currentStringPos);

          // If its a string of 1 character which is a Z we are
          // using Universal time, so no change nessesary.
		  if ((timezone[0] == 'Z' || timezone[0] == 'z') && 
		    timezone.Length == 1) {
            // Nothing to do here.

            // Check to see if it might be of the form [+-]HH:MM
		  } else if (timezone.Length == NUMERIC_TIME_ZONE_LENGTH && (timezone[0] == '+' || 
			  timezone[0] == '-') && timezone[3] == ':') {
			int hours = Convert.ToInt32(timezone.Substring(1, 2));
		    int minutes = Convert.ToInt32(timezone.Substring(4, 2));
            timeZoneOffset = ((hours * 60) + minutes) * 60000;
				if (timezone[0] == '+') {
              timeZoneOffset = -timeZoneOffset;  
            }
          } else {
            // We have spare characters that aren't a valid timezone,
            // so its an invalid date.
            throw new FormatException();
          }
        }
		  } catch (IndexOutOfRangeException) {
        throw new FormatException();
      }
    }
    return timeZoneOffset;
  }
}

}