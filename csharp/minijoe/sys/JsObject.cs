using System;
using System.Collections;
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

//import java.util.Date;
//import java.util.Enumeration;
//import java.util.Hashtable;

/** 
 * Root class for all objects that are accessible via the JavaScript 
 * interpreter. 
 * 
 * Constants named ID_XXX are the identification codes for corresponding
 * native members, used in set() and evalNative().
 * 
 * This class also implements the container classes for numbers, booleans
 * and strings since the object constructor must be able to construct all
 * those classes and the string methods need to be transferable to other 
 * classes
 * 
 * @author Stefan Haustein
 */
public class JsObject  {

  /**
   * special function id for empty constructors, 
   * ignored in JsObject.evalNative()
   */
  public const int ID_NOOP = -1;

  // Object methods
  
  protected internal const int ID_INIT_OBJECT = 1;
  protected internal const int ID_TO_STRING = 2;
  protected internal const int ID_VALUE_OF = 3;
  protected internal const int ID_TO_LOCALE_STRING = 4;
  protected internal const int ID_HAS_OWN_PROPERTY = 5;
  protected internal const int ID_IS_PROTOTYPE_OF = 6;
  protected internal const int ID_PROPERTY_IS_ENUMERABLE = 7;
  
  // global functions  
  
  internal const int ID_PARSE_INT = 10;
  internal const int ID_PARSE_FLOAT = 11;
  internal const int ID_IS_NAN = 12;
  internal const int ID_IS_FINITE = 13;
  internal const int ID_DECODE_URI = 14;
  internal const int ID_DECODE_URI_COMPONENT = 15;
  internal const int ID_ENCODE_URI = 16;
  internal const int ID_ENCODE_URI_COMPONENT = 17;
  internal const int ID_PRINT = 18;

  // math functions
  
  internal const int ID_ABS = 19;
  internal const int ID_ACOS = 20;
  internal const int ID_ASIN = 21;
  internal const int ID_ATAN = 22;
  internal const int ID_ATAN2 = 23;
  internal const int ID_CEIL = 24;
  internal const int ID_COS = 25;
  internal const int ID_EXP = 26;
  internal const int ID_FLOOR = 27;
  internal const int ID_LOG = 28;
  internal const int ID_MAX = 29;
  internal const int ID_MIN = 30;
  internal const int ID_POW = 31;
  internal const int ID_RANDOM = 32;
  internal const int ID_ROUND = 33;
  internal const int ID_SIN = 34;
  internal const int ID_SQRT = 35;
  internal const int ID_TAN = 36;
  
  // boolean / number
  
  internal const int ID_INIT_BOOLEAN = 37;
  internal const int ID_INIT_STRING = 38;
  
  // string methods
  
  internal const int ID_FROM_CHAR_CODE = 39;
  
  internal const int ID_CHAR_AT = 40;
  internal const int ID_CHAR_CODE_AT = 41;
  internal const int ID_CONCAT = 42;
  internal const int ID_INDEX_OF = 43;
  internal const int ID_LAST_INDEX_OF = 44;
  internal const int ID_LOCALE_COMPARE = 45;
  internal const int ID_MATCH = 46;
  internal const int ID_REPLACE = 47;
  internal const int ID_SEARCH = 48;
  internal const int ID_SLICE = 49;
  internal const int ID_SPLIT = 50;
  internal const int ID_SUBSTRING = 51;
  internal const int ID_TO_LOWER_CASE = 52;
  internal const int ID_TO_LOCALE_LOWER_CASE = 53;
  internal const int ID_TO_UPPER_CASE = 54;
  internal const int ID_TO_LOCALE_UPPER_CASE = 55;
  internal const int ID_LENGTH = 56;
  internal const int ID_LENGTH_SET = 57;

  internal const int ID_INIT_NUMBER = 60;
  internal const int ID_TO_FIXED = 61;
  internal const int ID_TO_EXPONENTIAL = 62;
  internal const int ID_TO_PRECISION = 63;

  internal const int ID_PARSE = 64;
  internal const int ID_UTC = 65;
  
  // constructors
  
  internal const int ID_INIT_ARRAY = 66;
  internal const int ID_INIT_ERROR = 67;
  internal const int ID_INIT_FUNCTION = 68;
  internal const int ID_INIT_DATE = 69;

  // math constants
  
  internal const int ID_E = 70;
  internal const int ID_E_SET = 71;
  internal const int ID_LN10 = 72;
  internal const int ID_LN10_SET = 73;
  internal const int ID_LN2 = 74;
  internal const int ID_LN2_SET = 75;
  internal const int ID_LOG2E = 76;
  internal const int ID_LOG2E_SET = 77;
  internal const int ID_LOG10E = 78;
  internal const int ID_LOG10E_SET = 79;
  internal const int ID_PI = 80;
  internal const int ID_PI_SET = 81;
  internal const int ID_SQRT1_2 = 82;
  internal const int ID_SQRT1_2_SET = 83;
  internal const int ID_SQRT2 = 84;
  internal const int ID_SQRT2_SET = 85;
  
  public const int TYPE_UNDEFINED = 0;
  public const int TYPE_NULL = 1;
  public const int TYPE_OBJECT = 2;
  public const int TYPE_BOOLEAN = 3;
  public const int TYPE_NUMBER = 4;
  public const int TYPE_STRING = 5;
  public const int TYPE_FUNCTION = 6;
  
  /** Javascript type names as returned by the typeof operator. Note
   * that TYPE_NULL and TYPE_OBJECT are mapped to object both. */
  internal static readonly string[] TYPE_NAMES = new string[] {"undefined", "object", "object", 
    "boolean", "number", "string", "function"
  };
  
  /** Placeholder for Javascript undefined (Java null) values in hashtables */
  private static readonly Object UNDEFINED_PLACEHOLDER = new Object();
  
  /** Prototype of all Javascript objects */
  public static readonly JsObject OBJECT_PROTOTYPE = 
      new JsObject(null)
      .addVar("toString", new JsFunction(ID_TO_STRING, 0))
      .addVar("valueOf", new JsFunction(ID_VALUE_OF, 0))
      .addVar("toLocaleString", new JsFunction(ID_TO_LOCALE_STRING, 0))
      .addVar("hasOwnProperty", new JsFunction(ID_HAS_OWN_PROPERTY, 1))
      .addVar("isPrototypeOf", new JsFunction(ID_IS_PROTOTYPE_OF, 1))
      .addVar("propertyIsEnumerable", 
          new JsFunction(ID_PROPERTY_IS_ENUMERABLE, 1))
      ;
  
  public static readonly JsObject BOOLEAN_PROTOTYPE = 
      new JsObject(OBJECT_PROTOTYPE);

  public static readonly JsObject NUMBER_PROTOTYPE = 
      new JsObject(OBJECT_PROTOTYPE)
      .addVar("toFixed", new JsFunction(ID_TO_FIXED, 1))
      .addVar("toExponential", new JsFunction(ID_TO_EXPONENTIAL, 1))
      .addVar("toPrecision", new JsFunction(ID_TO_PRECISION, 1))
      ;

  public static readonly JsObject STRING_PROTOTYPE = 
      new JsObject(OBJECT_PROTOTYPE)
      .addVar("charAt", new JsFunction(ID_CHAR_AT, 1))
      .addVar("charCodeAt", new JsFunction(ID_CHAR_CODE_AT, 1))
      .addVar("concat", new JsFunction(ID_CONCAT, 1))
      .addVar("indexOf", new JsFunction(ID_INDEX_OF, 2))
      .addVar("lastIndexOf", new JsFunction(ID_LAST_INDEX_OF, 2))
      .addVar("localeCompare", new JsFunction(ID_LOCALE_COMPARE, 1))
      .addVar("replace", new JsFunction(ID_REPLACE, 2))
      .addVar("search", new JsFunction(ID_SEARCH, 1))
      .addVar("slice", new JsFunction(ID_SLICE, 2))
      .addVar("split", new JsFunction(ID_SPLIT, 2))
      .addVar("substring", new JsFunction(ID_SUBSTRING, 2))
      .addVar("toLowerCase", new JsFunction(ID_TO_LOWER_CASE, 0))
      .addVar("toLocaleLowerCase", new JsFunction(ID_TO_LOCALE_LOWER_CASE, 0))
      .addVar("toUpperCase", new JsFunction(ID_TO_UPPER_CASE, 0))
      .addVar("toLocaleUpperCase", new JsFunction(ID_TO_LOCALE_UPPER_CASE, 0))
      .addVar("length", new JsFunction(ID_LENGTH, -1))
      ;
  
  public static readonly Object[] NO_PARAM = new Object[0];

  /** Prototype chain */
  protected internal JsObject __proto__;
  
  /** Hashtable holding the reverse mapping for native methods. */
  private Hashtable natives = new Hashtable(10);
  /** Hashtable holding the properties and values of this object. */
  private Hashtable data;
  /** Parent object in scope chain */
  protected internal JsObject scopeChain;

  /** Primitive object value, used for Number and String. */
  internal Object value;

  /** 
   * Constructs a new Javascript object with the given prototype. 
   */
  public JsObject(JsObject __proto__){
    this.__proto__ = __proto__;
  }

  /** 
   * Return the raw value of a property, taking the prototype chain into account, but not the
   * scope chain or native getters or setters.
   */
  public virtual object getRawInPrototypeChain(String key) {
    Object result;
    
    if (data != null) {
		  result = data[key];
      if (result != null) {
        return result == UNDEFINED_PLACEHOLDER ? null : result;
      }
    }
    
    if (__proto__ != null) {
      result = __proto__.getRawInPrototypeChain(key);
      if (result != null){
        return result;
      }
    }
    return null;
  }

  /**
   * Returns the given property, taking native getters into account.
   * 
   * @param prop Name of the property
   * @return stored value or null
   */
  public virtual Object getObject(String prop) {
    Object v = getRawInPrototypeChain(prop);
    if (v is JsFunction) {
      JsFunction nat = (JsFunction) v;
      if (nat.getParameterCount() == -1){
        JsArray stack = new JsArray();
        evalNative(nat.index, stack, 0, 0);
        return stack.getObject(0);
      }
    } else if (v == null && scopeChain != null) {
      v = scopeChain.getObject(prop);
    }
    
    return v;
  }
  
  /**
   * Returns the numeric value for the given key. If the actual value is not
   * numeric, it is converted automatically, using the ECMA 262 conversion 
   * rules.
   */
  public double getNumber(String key) {
    return JsSystem.toNumber(getObject(key));
  }

  /**
   * Returns the string value for the given key. If the actual value is not a 
   * string, it is converted automatically, using the ECMA 262 conversion 
   * rules.
   */
  public String getString(String key) {
    return JsSystem.toString(getObject(key));
  }
  
  /** 
   * Returns the value at array index i, casted to an integer (32 bit). 
   */
	  public int getInt(String key) {
    return (int) getNumber(key);
  }
  
  /** 
   * Set method called from the byte code interpreter, 
   * avoiding temporary stack creation. This method is
   * overwritten in JsArray. 
   */
  public virtual void vmSetOperation(JsArray stack, int keyIndex, int valueIndex) {
    String key = stack.getString(keyIndex);

// TODO re-enable optimization 
//    Object old = getRaw(key);
//
//    if (old instanceof JsFunction){
//      JsFunction nat = (JsFunction) old;
//      if (nat.getParameterCount() == -1){
//        evalNative(nat.index + 1, stack, valueIndex, 0);
//        return;
//      }
//    }

    setObject(key, stack.getObject(valueIndex));
  }

  /** 
   * Get method called from the bytecode interpreter,  avoiding temporary stack 
   * creation. This method is overwritten in JsArray and JsArguments.
   */
  public virtual void vmGetOperation(JsArray stack, int keyIndex, int valueIndex) {
    String key = stack.getString(keyIndex);

 // TODO re-enable optimization 
//    Object old = getRaw(key);
//
//    if (old instanceof JsFunction){
//      JsFunction f = (JsFunction) old;
//      if (f.getParameterCount() == -1){
//        evalNative(f.index, stack, valueIndex, 0);
//        return;
//      }
//    }
    stack.setObject(valueIndex, getObject(key));
  }

  /**
   * Set the given property to the given value without taking the scope or
   * prototype chain into account.
   * 
   * @param prop property name
   * @param value value to set
   * @return this (for chained calls)
   */
  public virtual JsObject addVar(String prop, Object v) {
    if (data == null){
      data = new Hashtable();
    }
	data[prop] = (v == null ? UNDEFINED_PLACEHOLDER : v);
	if (v is JsFunction && ((JsFunction) v).index != ID_NOOP) {
      String key = getNativeKey(((JsFunction) v).factoryTypeId, ((JsFunction) v).index);
      if(key != null) {
	    if (natives.ContainsKey(key)) {
		  Console.WriteLine("Duplicate native function ID '" + 
            ((JsFunction) v).index + "' detected for method '" + prop + "'.");
        } else {
			  natives[key] = prop;
        }
      }
    }
    return this;
  }

  /** 
   * Convenience method for <tt>addVar(prop, new JsFunction(int nativeCallId, int parCount)</tt>
   */
  public virtual JsObject addNative(string prop, int nativePropertyId, int parCount) {
    return addVar(prop, new JsFunction(nativePropertyId, parCount));
  }

  /**
   * Get the function's name for a particular ID.
   */
  public virtual String getFunctionName(int factoryTypeId, int index) {
    return getFunctionNameImpl(getNativeKey(factoryTypeId, index));
  }

  private String getFunctionNameImpl(String key) {
    String prop = (String) natives[key];
    if (prop == null && __proto__ != null) {
      prop = __proto__.getFunctionNameImpl(key);
    }
    return prop;
  }

  private String getNativeKey(int factoryTypeId, int index) {
    return (factoryTypeId <= JsSystem.FACTORY_ID_OBJECT) ?
      JsSystem.FACTORY_ID_OBJECT + ":" + index : factoryTypeId + ":" + index;
  }

  /** 
   * Set the given property to a numeric value.
   */
  public virtual void setNumber(String key, double n) {
    setObject(key, (Double)(n));
  }
  
  /**
   * Sets the given property to the given value, taking the prototype chain,
   * scope chain, and setters into account.
   * 
   * @param prop property name
   * @param value value to set
   * @return this (for chained calls)
   */
  public virtual void setObject(String key, Object v) {

    Object old = getRawInPrototypeChain(key);
	if (old is JsFunction 
          && ((JsFunction) old).getParameterCount() == -1) {
        JsFunction nat = (JsFunction) old;
        JsArray stack = new JsArray();
        stack.setObject(0, v);
        evalNative(nat.index + 1, stack, 0, 0);
        return;
    } else if (old == null && scopeChain != null) {
      scopeChain.setObject(key, v);
    } else {
      if (data == null) {
        data = new Hashtable();
      }
	  data[key] = (v == null ? UNDEFINED_PLACEHOLDER : v);
    }
  }

  /** 
   * Returns a key enumeration for this object only, not including the 
   * prototype or scope chain.
   */
  public virtual IEnumerator keys() {
    if(data == null) {
      data = new Hashtable();
    }
	return data.Keys.GetEnumerator();
  }

  /**
   * Returns elements enumeration for this object only, not including the
   * prototype or scope chain.
   */
  public virtual IEnumerator elements() {
    if(data == null) {
      data = new Hashtable();
    }
	return data.Values.GetEnumerator();
  }

  /**
   * Returns a string representation of this.
   */
  public override String ToString() {
    return value == null ? "[object Object]" : value.ToString();
  }

  /**
   * Delete the given property. Returns true if it was actually deleted.
   */
	  public virtual bool delete(string key) {
    if (data == null) {
      return true;
    }
    
    //TODO check whether this covers dontdelete sufficiently
  
		Object old = data[key];
		bool isFunc = old is JsFunction;
    if (isFunc && ((JsFunction) old).getParameterCount() == -1){
      return false;
    }

	data.Remove(key);
    if(isFunc) {
      natives.Remove(getNativeKey(((JsFunction) old).factoryTypeId, ((JsFunction) old).index));
    }
    return true;
  }

  /**
   * Clears all properties.
   */
  public virtual void clear() {
    if(data != null) {
      data.Clear();
      data = null;
    }
  }

  /** 
   * Execute java member implementation. Parameters for functions start at 
   * stack[sp+2]. Function and getter results are returned at stack[sp+0].
   * The assignement value for a setter is stored at stack[sp+0]. 
   */
  public virtual void evalNative(int index, JsArray stack, int sp, int parCount) {
    Object obj;
    switch(index) {
      // object methods
      
      case ID_NOOP:
        break;
        
      case ID_INIT_OBJECT:
        obj = stack.getObject(sp + 2);
        if (isConstruction(stack, sp)){
		  if (obj is Boolean || obj is Double || 
			  obj is String) {
            value = obj;
		  } else if (obj is JsObject) {
            stack.setObject(sp - 1,  obj);
          }
          // otherwise, don't do anything -- regular constructor call
        } else {
          if (obj == null || obj == JsSystem.JS_NULL) {
            stack.setObject(sp, new JsObject(OBJECT_PROTOTYPE));
          } else {
            stack.setObject(sp, JsSystem.toJsObject(obj));
          }
        }
        break;
        
      case ID_TO_STRING:
      case ID_TO_LOCALE_STRING:
        stack.setObject(sp, JsSystem.toString(stack.getObject(sp)));
        break;

      case ID_HAS_OWN_PROPERTY:
        stack.setBoolean(sp, data != null && 
		    data[stack.getString(sp + 2)] != null);
        break;
        
      case ID_IS_PROTOTYPE_OF:
        obj = stack.getObject(sp + 2);
        stack.setBoolean(sp, false);
		while (obj is JsObject){
          if (obj == this) {
            stack.setBoolean(sp, true);
            break;
          }
        }
        break;
        
      case ID_PROPERTY_IS_ENUMERABLE:
        obj = getRawInPrototypeChain(stack.getString(sp + 2));
		stack.setBoolean(sp, obj != null && !(obj is JsFunction));
        break;
        
      case ID_VALUE_OF:
        stack.setObject(sp, value == null ? this : value);
        break;

        // Number methods
        
      case ID_INIT_NUMBER:
        if (isConstruction(stack, sp)) {
		  value = (Double)(stack.getNumber(sp + 2));
        } else {
          stack.setNumber(sp, stack.getNumber(sp + 2));
        }
        break;

        // Boolean methods

      case ID_INIT_BOOLEAN:
        if (isConstruction(stack, sp)) {
		  value = stack.getBoolean(sp + 2) ? true : false;
        } else {
          stack.setObject(sp, stack.getBoolean(sp + 2) 
			  ? true : false);
        }
        break;

      case ID_INIT_STRING:
        if (isConstruction(stack, sp)) {
          value = stack.getString(sp +2);
        } else {
          stack.setObject(sp, parCount == 0 ? "" : stack.getString(sp + 2));
        }
        break;

      // initializers that can be used as functions need to be covered in Object
        
      case ID_INIT_ARRAY:
        JsArray array = (isConstruction(stack, sp)
          ? (JsArray) this : new JsArray());
        if (parCount == 1 && stack.isNumber(sp + 2)) {
          array.setSize(stack.getInt(sp + 2));
        } else {
          for (int i2 = 0; i2 < parCount; i2++) {
            stack.copy(sp + i2 + 2, array, i2);
          }
        }
        stack.setObject(sp, array);
        break;
        
      case ID_INIT_ERROR:
        if (isConstruction(stack, sp)) {
          setObject("message", stack.getString(sp + 2));
        } else {
          stack.setObject(sp, new JsError(stack.getJsObject(sp), 
              stack.getString(sp + 2)));
        }
        break;
        
        
      case ID_INIT_FUNCTION:
        // Note: this will exchange the "this" object at sp-1 if it is used as constructor
		bool construction = isConstruction(stack, sp);
        
		StringBuilder buf = new StringBuilder("(function(");
        for(int i3 = 0; i3 < parCount-1; i3++) {
          if(i3 != 0) {
		    buf.Append(',');
          }
		  buf.Append(stack.getString(sp + 2 + i3));
        }
		buf.Append("){");
        if(parCount != 0) {
		  buf.Append(stack.getString(sp + 2 + parCount - 1));
        }
		buf.Append("});");
        
		Console.WriteLine("eval: " + buf);
        
        JsObject global = (JsObject) stack.getObject(0);
        JsFunction eval = (JsFunction) global.getObject("eval");
        stack.setObject(sp, global); // global
        stack.setObject(sp + 1, eval);
		stack.setObject(sp + 2, buf.ToString());
        eval.eval(stack, sp, 1);
        
        if(construction) {
          stack.copy(sp, stack, sp-1);
        }
        break;
      
        
      case ID_INIT_DATE:
        // reset to defaults
        if (isConstruction(stack, sp)){
          JsDate d_2 = (JsDate) this;
          if (parCount == 1) {
			d_2.time.setTime(new DateTime((long) stack.getNumber(sp + 2)));
          } else if (parCount > 1){
			d_2.time.setTime(new DateTime());
            int year = stack.getInt(sp + 2);
            if (year >= 0 && year <= 99) { 
              year += 1900;
            }
            d_2.setDate(false, year, 
                           stack.getNumber(sp + 3), 
                           stack.getNumber(sp + 4));
            
            d_2.setTime(false, stack.getNumber(sp + 5), 
                  stack.getNumber(sp + 6), 
                  stack.getNumber(sp + 7),
                  stack.getNumber(sp + 8));
          }
        } else {
          stack.setObject(sp, 
              new JsDate(JsDate.DATE_PROTOTYPE).toString(true, true, true));
        }
        break;
   
        
      // global properties
        
      case ID_PRINT:
	    Console.WriteLine(stack.getString(sp + 2));
        break;
        
      case ID_PARSE_INT:
	    String s = stack.getString(sp + 2).Trim().ToLower();
        try {
          if (stack.isNull(sp + 3)) {
		    stack.setInt(sp, s.StartsWith("0x") 
				? Convert.ToInt32(s.Substring(2), 16) 
				    : Convert.ToInt32(s));
          } else {
				stack.setInt(sp, Convert.ToInt32(s, stack.getInt(sp + 3)));
          }
	    } catch (FormatException) {
          stack.setInt(sp, 0);
        }
        break;
        
      case ID_PARSE_FLOAT:
        try {
		  stack.setNumber(sp, Convert.ToDouble(stack.getString(sp + 2)));
		} catch (FormatException) {
		  stack.setNumber(sp, double.NaN);
        }
        break;
        
      case ID_IS_NAN:
	    stack.setBoolean(sp, double.IsNaN(stack.getNumber(sp + 2)));
        break;

      case ID_IS_FINITE:
		double d_ = stack.getNumber(sp + 2);
	    stack.setBoolean(sp, !Double.IsInfinity(d_) && !Double.IsNaN(d_));
        break;

      case ID_DECODE_URI:
        obj = stack.getObject(sp + 2);
		if (obj is sbyte[]) {
		  stack.setObject(sp, JsSystem.decodeURI((sbyte[]) obj));
        } else {
		  stack.setObject(sp, JsSystem.decodeURI(obj.ToString()));
        }
        break;

      case ID_ENCODE_URI:
        obj = stack.getObject(sp + 2);
		if (obj is sbyte[]) {
		  stack.setObject(sp, JsSystem.encodeURI((sbyte[]) obj));
        } else {
		  stack.setObject(sp, JsSystem.encodeURI(obj.ToString()));
        }
        break;

      //TODO Implement
      case ID_DECODE_URI_COMPONENT:
      case ID_ENCODE_URI_COMPONENT:
	    throw new Exception("NYI");
        
      // math properties
        
      case ID_ABS:
	    stack.setNumber(sp, Math.Abs(stack.getNumber(sp + 2)));
        break;

      case ID_ACOS:
      case ID_ASIN:
      case ID_ATAN:
      case ID_ATAN2:
	    throw new Exception("NYI");
          
      case ID_CEIL:
		stack.setNumber(sp, Math.Ceiling(stack.getNumber(sp + 2)));
        break;
        
      case ID_COS:
		stack.setNumber(sp, Math.Cos(stack.getNumber(sp + 2)));
        break;
        
      case ID_EXP:
        stack.setNumber(sp, JsSystem.exp(stack.getNumber(sp + 2)));
        break;
        
      case ID_FLOOR:
	    stack.setNumber(sp, Math.Floor(stack.getNumber(sp + 2)));
        break;
        
      case ID_LOG:
        stack.setNumber(sp, JsSystem.ln(stack.getNumber(sp + 2)));
        break;
        
      case ID_MAX:
		double d2 = Double.NegativeInfinity;
        for (int i3 = 0; i3 < parCount; i3++){
		  d2 = Math.Max(d2, stack.getNumber(sp + 2 + i3));
        }
        stack.setNumber(sp, d2);
        break;

      case ID_MIN:
	    double d3 = Double.PositiveInfinity;
        for (int i4 = 0; i4 < parCount; i4++){
		  d3 = Math.Min(d3, stack.getNumber(sp + 2 + i4));
        }
        stack.setNumber(sp, d3);
        break;

      case ID_POW:
        stack.setNumber(sp, JsSystem.pow(stack.getNumber(sp + 2), 
            stack.getNumber(sp + 3)));
        break;
        
      case ID_RANDOM:
	    stack.setNumber(sp, JsSystem.random.NextDouble());
        break;

      case ID_ROUND:
	    stack.setNumber(sp, Math.Floor(stack.getNumber(sp + 2) + 0.5));
        break;

      case ID_SIN:
		stack.setNumber(sp, Math.Sin(stack.getNumber(sp + 2)));
        break;

      case ID_SQRT:
		stack.setNumber(sp, Math.Sqrt(stack.getNumber(sp + 2)));
        break;

      case ID_TAN:
		stack.setNumber(sp, Math.Tan(stack.getNumber(sp + 2)));
        break;

      // string methods
        
      case ID_FROM_CHAR_CODE:
        char[] chars = new char[parCount];
        for (int i5 = 0; i5 < parCount; i5++) {
          chars[i5] = (char) stack.getInt(sp + 2 + i5);
        }
        stack.setObject(sp, new String(chars));
        break;
        
      // string.prototype methods
        
      case ID_CHAR_AT:
        s = stack.getString(sp);
        int i6 = stack.getInt(sp + 2);
		stack.setObject(sp, i6 < 0 || i6 >= s.Length 
		    ? "" : s.Substring(i6, 1));
        break;
        
      case ID_CHAR_CODE_AT:
        s = stack.getString(sp);
        int i7 = stack.getInt(sp + 2);
		stack.setNumber(sp, i7 < 0 || i7 >= s.Length 
			? Double.NaN : s[i7]);
        break;
        
      case ID_CONCAT:
		buf = new StringBuilder(stack.getString(sp));
        for (int i8 = 0; i8 < parCount; i8++) {
		  buf.Append(stack.getString(sp + i8 + 2));
        }
		stack.setObject(sp, buf.ToString());
        break;
        
      case ID_INDEX_OF:
	    stack.setNumber(sp, stack.getString(sp).IndexOf(stack.getString(sp + 2), 
            stack.getInt(sp + 3)));
        break;
        
      case ID_LAST_INDEX_OF:
        s = stack.getString(sp);
        String find = stack.getString(sp + 2);
        double d4 = stack.getNumber(sp + 3);
		int max = (Double.IsNaN(d4)) ? s.Length : (int) d4;
        
        int best = -1;
        while (true) {
		  int found = s.IndexOf(find, best + 1);
          if (found == -1 || found > max){
            break;
          }
          best = found;
        }
        
        stack.setNumber(sp, best);
        break;
        
      case ID_LOCALE_COMPARE:
        stack.setNumber(sp, 
		    stack.getString(sp).CompareTo(stack.getString(sp + 2)));
        break;
        
      case ID_REPLACE:
        s = stack.getString(sp);
        find = stack.getString(sp + 2);
        String replace = stack.getString(sp + 3);
		if (!find.Equals("")) {
		  StringBuilder sb = new StringBuilder(s);
		  int length = find.Length;

          // Parse nodes into vector
		  while ((index = sb.ToString().IndexOf(find)) >= 0) {
		    sb.Remove(index, index + length);
			sb.Insert(index, replace);
          }
		  stack.setObject(sp, sb.ToString());
          sb = null;
        }
        break;
      case ID_MATCH:
      case ID_SEARCH:
	    throw new Exception("Regexp NYI");
        
      case ID_SLICE:
        s = stack.getString(sp);
		int len = s.Length;
        int start = stack.getInt(sp + 2);
        int end = stack.isNull(sp + 3) ? len : stack.getInt(sp + 3);
        if (start < 0) {
		  start = Math.Max(len + start, 0);
        }
        if (end < 0) {
		  end = Math.Max(len + start, 0);
        }
        if (start > len){
          start = len;
        }
        if (end > len) {
          end = len;
        }
        if (end < start) {
          end = start;
        }
		stack.setObject(sp, s.Substring(start, end - start));
        break;
        
      case ID_SPLIT:
        s = stack.getString(sp);
        String sep = stack.getString(sp + 2);
        double limit = stack.getNumber(sp + 3);
		if (Double.IsNaN(limit) || limit < 1) {
		  limit = Double.MaxValue;
        }
        
        JsArray a = new JsArray();
		if (sep.Length == 0) {
		  if (s.Length < limit) {
		    limit = s.Length;
          }
          for (int i9 = 0; i9 < limit; i9++) {
		    a.setObject(i9, s.Substring(i9, 1));
          }
        }
        else {
          int cut0 = 0;
		  while (cut0 < s.Length && a.size() < limit) {
		    int cut = s.IndexOf(sep, cut0);
            if(cut == -1) { 
			  cut = s.Length;
            }
			a.setObject(a.size(), s.Substring(cut0, cut - cut0));
			cut0 = cut + sep.Length;
          }
        }
        stack.setObject(sp, a);
        break;
        
      case ID_SUBSTRING:
        s = stack.getString(sp);
		len = s.Length;
        start = stack.getInt(sp + 2);
        end = stack.isNull(sp + 3) ? len : stack.getInt(sp + 3);
        if (start > end){
          int tmp = end;
          end = start;
          start = tmp;
        }
		start = Math.Min(Math.Max(0, start), len);
		end = Math.Min(Math.Max(0, end), len);
		stack.setObject(sp, s.Substring(start, end - start));
        break;

      case ID_TO_LOWER_CASE: //TODO: which locale to use as defautlt? us?
      case ID_TO_LOCALE_LOWER_CASE:
	    stack.setObject(sp, stack.getString(sp + 2).ToLower());
        break;

      case ID_TO_UPPER_CASE: //TODO: which locale to use as defautlt? us?
      case ID_TO_LOCALE_UPPER_CASE:
	    stack.setObject(sp, stack.getString(sp + 2).ToUpper());
        break;

      case ID_LENGTH:
	    stack.setInt(sp, ToString().Length);
        break;
        
      case ID_LENGTH_SET:
        // cannot be changed!
        break;

      case ID_TO_EXPONENTIAL:
      case ID_TO_FIXED:
      case ID_TO_PRECISION:
        stack.setObject(sp, JsSystem.formatNumber(index, 
            stack.getNumber(sp + 2), stack.getNumber(sp + 3)));
        break;
        
      case ID_UTC:
        JsDate date = new JsDate(JsDate.DATE_PROTOTYPE);
		date.time.setTime(new DateTime());
        int year2 = stack.getInt(sp + 2);
        if (year2 >= 0 && year2 <= 99) { 
          year2 += 1900;
        }
        date.setDate(true, year2, 
                       stack.getNumber(sp + 3), 
                       stack.getNumber(sp + 4));
        
        date.setTime(true, stack.getNumber(sp + 5), 
              stack.getNumber(sp + 6), 
              stack.getNumber(sp + 7),
              stack.getNumber(sp + 8));

		stack.setNumber(sp, date.time.getTime().Ticks);
        break;
        
      case ID_PARSE:
        double[] vals = {Double.NaN, Double.NaN, Double.NaN, 
            Double.NaN, Double.NaN, Double.NaN, Double.NaN};
        
        s = stack.getString(sp + 2);
        int curr = -1;
        int pos = 0;
		for (int i10 = 0; i10 < s.Length; i10++) {
		  char c = s[i10];
          if (c >= '0' && c <= '9'){
            if (curr == -1){
              curr = c - 48; 
            } else {
              curr = curr * 10 + (c - 48);
            }
          } else if (curr != -1){
				if (pos < vals.Length) {
              vals[pos++] = curr;
            }
            curr = -1;
          }
        }
		if (curr != -1 && pos < vals.Length) {
          vals[pos++] = curr;
        }
        
		bool utc = s.EndsWith("GMT") || s.EndsWith("UTC");
        date = new JsDate(JsDate.DATE_PROTOTYPE);
		date.time.setTime(new DateTime());
        date.setDate(utc, vals[0], vals[1], vals[2]);
        date.setTime(utc, vals[3], vals[4], vals[5], vals[6]);
		stack.setNumber(sp, date.time.getTime().Ticks);
        break;
        
      // Math constants
        
      case ID_E:
        stack.setNumber(sp, Math.E);
        break;
      case ID_LN10:
        stack.setNumber(sp, 2.302585092994046);
        break;
      case ID_LN2:
        stack.setNumber(sp, JsSystem.LN2);
        break;
      case ID_LOG2E:
        stack.setNumber(sp, 1.4426950408889634);
        break;
      case ID_LOG10E:
        stack.setNumber(sp, 0.4342944819032518);
        break;
      case ID_PI:
        stack.setNumber(sp, Math.PI);
        break;
      case ID_SQRT1_2:
	    stack.setNumber(sp, Math.Sqrt(0.5));
        break;
      case ID_SQRT2:
		stack.setNumber(sp, Math.Sqrt(2.0));
        break;
        
      case ID_E_SET:
      case ID_LN10_SET:
      case ID_LN2_SET:
      case ID_LOG2E_SET:
      case ID_LOG10E_SET:
      case ID_PI_SET:
      case ID_SQRT1_2_SET:
      case ID_SQRT2_SET:
        // dont do anything: cannot overwrite those values!
        break;  
               
      default:
	    throw new ArgumentException("Unknown native id: " + index 
            + " this: " + this);
    }
  }

  /** 
   * Determines whether the given call is an actual constructor call 
   * (with new)
   * TODO check whether there may be false positives...
   */
  protected internal virtual bool isConstruction(JsArray stack, int sp) {
    return sp > 0 && stack.getObject(sp - 1) == stack.getObject(sp);
  }
}

}