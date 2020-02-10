using System;
using System.Text;

// Copyright 2008 Google Inc.
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

//import java.util.Random;


/**
 * Class containing static MiniJoe helper methods. An instance of this class
 * is used as factory for the built-in JS types.
 * 
 * Note: Java null is mapped to undefined in javascript, and there is a special
 * Javascript null value "JS_NULL".
 * 
 * @author Stefan Haustein
 */
public class JsSystem : JsObjectFactory {

  public static readonly Object JS_NULL = new Object();

  internal const int FACTORY_ID_OBJECT = 0;
  internal const int FACTORY_ID_ARRAY = 1;
  internal const int FACTORY_ID_DATE = 2;
  internal const int FACTORY_ID_BOOLEAN = 3;
  internal const int FACTORY_ID_STRING = 4;
  internal const int FACTORY_ID_NUMBER = 5;
  internal const int FACTORY_ID_ERROR = 6;
  internal const int FACTORY_ID_EVAL_ERROR = 7;
  internal const int FACTORY_ID_RANGE_ERROR = 8;
  internal const int FACTORY_ID_REFERENCE_ERROR = 9;
  internal const int FACTORY_ID_SYNTAX_ERROR = 10;
  internal const int FACTORY_ID_TYPE_ERROR = 11;
  internal const int FACTORY_ID_URI_ERROR = 12;
  internal const int FACTORY_ID_FUNCTION = 13;

  internal static readonly Double NAN = (Double)(Double.NaN);
  internal static readonly Double INFINITY = (Double)(Double.PositiveInfinity);
  internal const double LN2 = 0.6931471805599453;

  internal static Random random = new Random();
  internal static JsSystem instance = new JsSystem();

  /** 
   * Returns the singleton instance of the factory 
   */
  public static JsSystem getInstance() {
    return instance;
  }

  /**
   * Returns a new global environment.
   */
  public static JsObject createGlobal(){
    JsObject global = new JsObject(null);
    
    global.addVar("Date", new JsFunction(instance, JsSystem.FACTORY_ID_DATE, 
        JsDate.DATE_PROTOTYPE, JsObject.ID_INIT_DATE, 7)
      .addVar("parse", new JsFunction(JsDate.ID_PARSE, 1))
      .addVar("UTC", new JsFunction(JsDate.ID_UTC, 7)))
    .addVar("Array", new JsFunction(instance, JsSystem.FACTORY_ID_ARRAY, 
        JsArray.PROTOTYPE, JsObject.ID_INIT_ARRAY, 1))
    .addVar("Object", new JsFunction(instance, JsSystem.FACTORY_ID_OBJECT, 
        JsObject.OBJECT_PROTOTYPE,  JsObject.ID_INIT_OBJECT, 1))
    .addVar("Boolean", new JsFunction(instance, JsSystem.FACTORY_ID_BOOLEAN,
        JsObject.BOOLEAN_PROTOTYPE, JsObject.ID_INIT_BOOLEAN, 1))
    .addVar("Number", new JsFunction(instance, JsSystem.FACTORY_ID_NUMBER,
        JsObject.NUMBER_PROTOTYPE, JsObject.ID_INIT_NUMBER, 1)
	      .addVar("MAX_VALUE", (Double)(Double.MaxValue))
	      .addVar("MIN_VALUE", (Double)(Double.Epsilon))
          .addVar("NaN", NAN)
          .addVar("NEGATIVE_INFINITY", (Double)(Double.NegativeInfinity))
          .addVar("POSITIVE_INFINITY", INFINITY))
    .addVar("String", new JsFunction(instance, JsSystem.FACTORY_ID_STRING,
        JsObject.STRING_PROTOTYPE, JsObject.ID_INIT_STRING, 1)
      .addVar("fromCharCode", new JsFunction(JsObject.ID_FROM_CHAR_CODE, 1)))
    .addVar("Function", new JsFunction(instance, JsSystem.FACTORY_ID_FUNCTION,
        JsFunction.FUNCTION_PROTOTYPE, JsObject.ID_INIT_FUNCTION, 1)
      .addVar("fromCharCode", new JsFunction(JsObject.ID_FROM_CHAR_CODE, 1)))
    .addVar("NaN", NAN)
    .addVar("Infinity", INFINITY)
    .addVar("undefined", null)
    .addVar("parseInt", new JsFunction(JsObject.ID_PARSE_INT, 2))
    .addVar("parseFloat", new JsFunction(JsObject.ID_PARSE_FLOAT, 1))
    .addVar("isNaN", new JsFunction(JsObject.ID_IS_NAN, 1))
    .addVar("isFinite", new JsFunction(JsObject.ID_IS_FINITE, 1)) 
    .addVar("decodeURI", new JsFunction(JsObject.ID_DECODE_URI, 1))
    .addVar("print", new JsFunction(JsObject.ID_PRINT, 1))
    .addVar("decodeURIComponent", new JsFunction(
        JsObject.ID_DECODE_URI_COMPONENT, 1))
    .addVar("encodeURI", new JsFunction(JsObject.ID_ENCODE_URI, 1))
    .addVar("encodeURIComponent", new JsFunction(
        JsObject.ID_ENCODE_URI_COMPONENT, 1))
    .addVar("Error", new JsFunction(instance, FACTORY_ID_ERROR, 
        JsError.ERROR_PROTOTYPE, JsError.ID_INIT_ERROR, 1))
    .addVar("EvalError", new JsFunction(instance, FACTORY_ID_EVAL_ERROR, 
        JsError.EVAL_ERROR_PROTOTYPE, JsError.ID_INIT_ERROR, 1))
    .addVar("RangeError", new JsFunction(instance, FACTORY_ID_RANGE_ERROR, 
        JsError.RANGE_ERROR_PROTOTYPE, JsError.ID_INIT_ERROR, 1))
    .addVar("ReferenceError", new JsFunction(instance, 
        FACTORY_ID_REFERENCE_ERROR, JsError.REFERENCE_ERROR_PROTOTYPE, 
        JsError.ID_INIT_ERROR, 1))
    .addVar("SyntaxError", new JsFunction(instance, FACTORY_ID_SYNTAX_ERROR, 
        JsError.SYNTAX_ERROR_PROTOTYPE, JsError.ID_INIT_ERROR, 1))
    .addVar("TypeError", new JsFunction(instance, FACTORY_ID_TYPE_ERROR, 
        JsError.TYPE_ERROR_PROTOTYPE, JsError.ID_INIT_ERROR, 1))
    ;
    
    //TODO Math object should be pre-constructed an reused.
    global.addVar("Math", new JsObject(JsObject.OBJECT_PROTOTYPE)
      .addNative("E", JsObject.ID_E, -1)
      .addNative("LN10", JsObject.ID_LN10, -1)
      .addNative("LN2", JsObject.ID_LN2, -1)
      .addNative("LOG2E", JsObject.ID_LOG2E, -1)
      .addNative("LOG10E", JsObject.ID_LOG10E, -1)
      .addNative("PI", JsObject.ID_PI, -1)
      .addNative("SQRT1_2", JsObject.ID_SQRT1_2, -1)
      .addNative("SQRT2", JsObject.ID_SQRT2, -1)
      .addNative("abs", JsObject.ID_ABS, 1)
      .addNative("acos", JsObject.ID_ACOS, 1)
      .addNative("asin", JsObject.ID_ASIN, 1)
      .addNative("atan", JsObject.ID_ATAN, 1)
      .addNative("atan2", JsObject.ID_ATAN2, 2)
      .addNative("ceil", JsObject.ID_CEIL, 1)
      .addNative("cos", JsObject.ID_COS, 1)
      .addNative("exp", JsObject.ID_EXP, 1)
      .addNative("floor", JsObject.ID_FLOOR, 1)
      .addNative("log", JsObject.ID_LOG, 1)
      .addNative("max", JsObject.ID_MAX, 2)
      .addNative("min", JsObject.ID_MIN, 2)
      .addNative("pow", JsObject.ID_POW, 2)
      .addNative("random", JsObject.ID_RANDOM, 0)
      .addNative("round", JsObject.ID_ROUND, 1)
      .addNative("sin", JsObject.ID_SIN, 1)
      .addNative("sqrt", JsObject.ID_SQRT, 1)
      .addNative("tan", JsObject.ID_TAN, 1));
    
    return global;
  }
  
  
  /**
   * Returns a string representation of the given object. This function works 
   * like the toString method in the ECMA 262 specification, except that it
   * never throws an exception. Instead, "null" is returned for null and 
   * "undefined" for undefined.
   * 
   * @param o the object to convert to a string
   * @return the string representation of the object.
   */
  public static String toString(Object o){

    if (o == null) {
      return "undefined";
    }
    if (o == JsSystem.JS_NULL) {
      return "null";
    }
	if (o is Double) {
      Double d = (Double) o;
	  if ((double)d == (double)(long) d) {
	    return Convert.ToString((double)(long) d);
      } else {
		return Convert.ToString((double)d);
      }
    }
    
    //TODO if(o instanceof JsObject) {...}
    
	return o.ToString();
  }
  
  /** 
   * This function works like toNumber in the ECMA 262 documentation,
   * except that it does not throw an exception for null or undefined.
   * Instead, null is converted to 0 and undefined is converted to NaN.
   */  
  public static double toNumber(Object o){
    if (o is Double) {
	  return (double)((Double) o);
    }
    if (o == null) {
      return 0;
    }
	if (o is string) {
      try {
	    return Convert.ToDouble((string) o);
      } catch (FormatException) {
        return Double.NaN;
      }
    }
	if (o is Boolean) {
	  return (bool)((Boolean) o) ? 1 : 0;
    }
	if (o is JsDate) {
	  return ((JsDate) o).time.getTime().Ticks;
    }
    return Double.NaN;
  }
  
  
  /** 
   * This function works like toBoolean in the ECMA 262 documentation,
   * except that it does not throw an exception for null or undefined.
   */  
  public static bool toBoolean(Object o) {
    if (o is Boolean) {
	  return (bool)((Boolean) o);
    }
	if (o is Double) {
	  double d = (double)((Double) o);
	  return !Double.IsNaN(d) && d != 0;
    }
   
    return !(o == null || "".Equals(o) || o == JsSystem.JS_NULL);
  }
  
  /**
   * Performs "boxing" for primitive values (string, boolean, number). Never
   * throws an exception. Instead, a new empty JsObject is returned for null
   * and undefined.
   */
  public static JsObject toJsObject(Object v){
    if (v is JsObject) {
      return (JsObject) v;
    }
	if (v is string) {
      JsObject o = new JsObject(JsObject.STRING_PROTOTYPE);
      o.value = v;
      return o;
    }
	if (v is Boolean) {
      JsObject o = new JsObject(JsObject.BOOLEAN_PROTOTYPE);
      o.value = v;
      return o;
    }
	if (v is Double) {
      JsObject o = new JsObject(JsObject.NUMBER_PROTOTYPE);
      o.value = v;
      return o;
    }
    //TODO
    return new JsObject(JsObject.OBJECT_PROTOTYPE);
  }
  
  /**
   * Creates a new instance of a JS object (Date, Object, Array), depending on 
   * the factory id.
   * 
   * @param type one of FACTORY_ID_OBJECT, FACTORY_ID_ARRAY, FACTORY_ID_DATE
   * @return the newly created instance
   */
  public virtual JsObject newInstance(int type) {
    switch(type){
      case JsSystem.FACTORY_ID_OBJECT: 
        return new JsObject( JsObject.OBJECT_PROTOTYPE);
      case JsSystem.FACTORY_ID_ARRAY: 
        return new JsArray();
      case JsSystem.FACTORY_ID_DATE: 
        return new JsDate(JsDate.DATE_PROTOTYPE);
      case JsSystem.FACTORY_ID_BOOLEAN:
        return new JsObject(JsObject.BOOLEAN_PROTOTYPE);
      case JsSystem.FACTORY_ID_NUMBER:
        return new JsObject(JsObject.NUMBER_PROTOTYPE);
      case JsSystem.FACTORY_ID_STRING:
        return new JsObject(JsObject.STRING_PROTOTYPE);
      case JsSystem.FACTORY_ID_ERROR:
        return new JsError(JsError.ERROR_PROTOTYPE, null);
      case JsSystem.FACTORY_ID_EVAL_ERROR:
        return new JsError(JsError.EVAL_ERROR_PROTOTYPE, null);
      case JsSystem.FACTORY_ID_RANGE_ERROR:
        return new JsError(JsError.RANGE_ERROR_PROTOTYPE, null);
      case JsSystem.FACTORY_ID_REFERENCE_ERROR:
        return new JsError(JsError.REFERENCE_ERROR_PROTOTYPE, null);
      case JsSystem.FACTORY_ID_SYNTAX_ERROR:
        return new JsError(JsError.SYNTAX_ERROR_PROTOTYPE, null);
      case JsSystem.FACTORY_ID_TYPE_ERROR:
        return new JsError(JsError.TYPE_ERROR_PROTOTYPE, null);
      case JsSystem.FACTORY_ID_URI_ERROR:
        return new JsError(JsError.URI_ERROR_PROTOTYPE, null);
      case JsSystem.FACTORY_ID_FUNCTION:
        // this will be overwritten by the eval result in case ID_INIT_FUNCITON in JsObject
        return new JsFunction(-1, -1);
    }
	throw new ArgumentException();
  }

  //TODO: Check whether NaN/Inf is covered correctly in ln, exp, pow
  
  /**
   * source: http://en.wikipedia.org/wiki/Exponential_function
   */
  public static double exp(double x){
    long n = (long) Math.Floor(x / LN2);
    double u = x - n * LN2;
    double m = 1;
    for (int i = 15; i >= 1; i--){
      m = 1 + (u / i) * m;
    }
    if (n != 0) {
	  long bits = MyDouble.doubleToLongBits(m);
      bits += (n << 52);
	  m = MyDouble.longBitsToDouble(bits);
    }
    return m;
    
  }
  
  /** Number of bits for the ln implementation (see ln) */
  private const int M = 62; // m*ln(2)
  
  /** 
   * Calculates logarithm to the basis e for x. Algorithm from Wikipedia.
   */
  public static double ln(double x){
    return Math.PI / (2.0 * avg(1.0, 4.0 / (x * (1L<<M)))) - LN2 * M;
  }
  
  /** 
   * Performs multiplications for integer exponents. In all other cases,
   * exp(y * ln(x)) is calculated.
   */
  public static double pow(double x, double y){
    long n = (long) y;
    if (y > 0 && y == n) {
      double result = 1;
      while (n > 0) {
        if ((n & 1) != 0) {
          result *= x;
          n--;
        }
        x *= x;
        n >>= 1;
      }
      return result;
    }
    return exp(y * JsSystem.ln(x));
  }
  
  /**
   * Converts a double to a string. This method is based on extracting the
   * decimal mantisse and exponent from toString(), this should probably
   * be replaced by a more elegant method... 
   */
  public static String formatNumber(int op, double d, double digitsRaw){
    String s = Convert.ToString(d);
    
	if (Double.IsInfinity(d) || Double.IsNaN(d) 
        || (op == JsObject.ID_TO_PRECISION && Double.IsNaN(digitsRaw))) {
      return s;
    }
    
    int digits = (int) digitsRaw;
    
	StringBuilder buf = new StringBuilder();
	bool neg = false;
	bool negExp = false;
    
    long value = 0;
    int exp = 0;
    long subExp = 0;
    int part = 0;  
    
	for (int i = 0; i < s.Length; i++) {
	  char c = s[i];
      switch(c){
      case '-': 
        if (part == 2){
          negExp = true;
        } else {
          neg = true; 
        }
        break;
      case 'e':
      case 'E':
        part = 2;
        break;
        
      case '.':
        part = 1;
        break;
      
      default:
        if (part == 2) {
          exp = exp * 10 + (c - 48);
        } else {
		  buf.Append(c);
          if (part == 1){
            subExp ++;
          }
        }
		break;
      }
    }
    
	while (buf.Length > 1 && buf[0] == '0') {
	  buf.Remove(0, 1);
    }

    if (negExp){
      exp = -exp;
    }
	exp -= (int)subExp;

	Console.WriteLine(neg ? "-" : "" + buf + "E" + exp);
    
    if (op == JsObject.ID_TO_PRECISION) {
      if (exp < -6 || exp >= digits) {
	    digits = Math.Max(0, digits - 1);
        op = JsObject.ID_TO_EXPONENTIAL;
      } else {
        op = JsObject.ID_TO_FIXED;
		digits = digits + Math.Min(exp, 0);
      }
    }
    
    if (digits < 0){
      digits = 0;
    }
    
    if (op == JsObject.ID_TO_EXPONENTIAL) {
      while (buf.Length > digits) {
		exp += buf.Length - digits;
		buf.Length = digits + 1;
	    long l = Convert.ToInt64(buf.ToString());
        l = (l + 5) / 10;
		buf.Length = 0;
		buf.Append(l);
      }
	  if (buf.Length > 1){
	    buf.Insert(1, ".");
      }
	  exp += buf.Length - 1;
      return (neg ? "" : "-") + value + "E" + exp;
    } else {
      int delta = digits + exp; // 2 digits, exp -5 -> 3 to cut off
	  Console.WriteLine("delta: " + delta);
      if (delta < 0) {
        delta = -delta; // delta = digits to cut off
		if (buf.Length - delta + 1 <= 0) {
		  buf.Length = 0;
        } else {
		  buf.Length = buf.Length - delta + 1;
		  long l = Convert.ToInt64(buf.ToString());
          l = (l + 5) / 10;
		  buf.Length = 0;
		  buf.Append(l);
        }
		while (buf.Length < digits + 1){
		  buf.Insert(0, '0');
        }
      } else while (delta > 0){
	    buf.Append('0');
        delta--;
      }
      
	  buf.Insert(buf.Length - digits, '.');
      
	  return buf.ToString();
    }
  }
  
  public static double avg(double a, double b) {
   for (int i = 0; i < 10; i++) {
      double t = (a + b) / 2.0;
	  b = Math.Sqrt(a * b);
      a = t;
    }
    return (a + b) / 2;
  }

  public static string encodeURI(sbyte[] bytes) {
    StringBuilder result = new StringBuilder(bytes.Length);
	for (int i = 0; i < bytes.Length; i++) {
      switch ((char)bytes[i]) {
        // Based on article: http://en.wikipedia.org/wiki/Percent-encoding
        case ' ':  result.Append('+');   break;
        case '!':  result.Append("%21"); break;
        case '"':  result.Append("%22"); break;
        case '#':  result.Append("%23"); break;
        case '$':  result.Append("%24"); break;
        case '%':  result.Append("%25"); break;
        case '&':  result.Append("%26"); break;
        case '\'': result.Append("%27"); break;
        case '(':  result.Append("%28"); break;
        case ')':  result.Append("%29"); break;
        case '*':  result.Append("%2A"); break;
        case '+':  result.Append("%2B"); break;
        case ',':  result.Append("%2C"); break;
        case '/':  result.Append("%2F"); break;
        case ':':  result.Append("%3A"); break;
        case ';':  result.Append("%3B"); break;
        case '<':  result.Append("%3C"); break;
        case '=':  result.Append("%3D"); break;
        case '>':  result.Append("%3E"); break;
        case '?':  result.Append("%3F"); break;
        case '@':  result.Append("%40"); break;
        case '[':  result.Append("%5B"); break;
        case '\\': result.Append("%5C"); break;
        case ']':  result.Append("%5D"); break;
        case '^':  result.Append("%5E"); break;
        default: result.Append((char) bytes[i]); break;
      }
    }
	return result.ToString();
  }

  public static String encodeURI(String str) {
  	return encodeURI(MyString.getBytes(str));
  }

  public static string decodeURI(sbyte[] bytes) {
    StringBuilder result = new StringBuilder(bytes.Length);
	for (int i = 0; i < bytes.Length; i++) {
	  switch ((char)bytes[i]) {
        case '+':
		  result.Append(' ');
          break;
        case '%':
		  if(i + 2 < bytes.Length) {
		    int next1 = MyChar.digit((char) bytes[i + 1], 16);
			int next2 = MyChar.digit((char) bytes[i + 2], 16);
            if(next1 > -1 && next2 > -1) {
              int b = (next1 << 4) + next2;
				  result.Append((char) b);
              i += 2;
              break;
            }
          }
		  goto default;
        default:
		  result.Append((char) bytes[i]);
		  break;
      }
    }
	return result.ToString();
  }

  public static String decodeURI(String str) {
    return decodeURI(MyString.getBytes(str));
  }
}

}