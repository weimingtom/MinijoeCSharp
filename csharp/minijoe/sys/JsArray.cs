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

//import java.util.Enumeration;
//import java.util.Vector;

/**
 * Array implementation. The array grows automatically for write access. 
 * 
 * This class has convenience methods to set and get primitive values and
 * Javascript objects. Types are converted accordingly.
 * 
 * @author Stefan Haustein
 */
public class JsArray : JsObject {

  private const int INITIAL_SPACE = 16;
  
  private const int ID_JOIN = 405;
  private const int ID_POP = 406;
  private const int ID_PUSH = 407;
  private const int ID_REVERSE = 408;
  private const int ID_SHIFT = 409;
  private const int ID_SORT = 411;
  private const int ID_SPLICE = 412;
  private const int ID_UNSHIFT = 413;

  public static readonly JsObject PROTOTYPE = new JsObject(OBJECT_PROTOTYPE)
      .addVar("length", new JsFunction(ID_LENGTH, -1))
      .addVar("concat", new JsFunction(ID_CONCAT, 1))
      .addVar("join", new JsFunction(ID_JOIN, 0))
      .addVar("pop", new JsFunction(ID_POP, 0))
      .addVar("push", new JsFunction(ID_PUSH, 1))
      .addVar("reverse", new JsFunction(ID_REVERSE, 0))
      .addVar("shift", new JsFunction(ID_SHIFT, 0))
      .addVar("slice", new JsFunction(ID_SLICE, 2))
      .addVar("sort", new JsFunction(ID_SORT, 1))
      .addVar("splice", new JsFunction(ID_SPLICE, 2))
      .addVar("unshift", new JsFunction(ID_UNSHIFT, 1))
      ;

  /**
   * Marker object, used to indicate that the actual value is contained in
   * the longs array.
   */
  private static readonly object NUMBER_MARKER = new object();

  /**
   * Objects contained in this array.
   */
  private Object[] objects = new Object[INITIAL_SPACE];

  /**
   * numeric values contained in this array, represented as 64 bit fixed point
   * value (16 bit fraction).
   */
  private double[] numbers = new double[INITIAL_SPACE];

  /**
   * Active size of this array.
   */
  private int size_;

  /**
   * Creates a new empty array.
   */
  public JsArray() : 
    base(PROTOTYPE) {
  }

  /**
   * Returns the boolean value at array index i. If the actual value is not
   * boolean, it is converted automatically according to the ECMA conversion
   * rules. This method handles Boolean.TRUE, Boolean.FALSE and numbers. All
   * other values are delegated to JsSystem.toBoolean()
   */
  public bool getBoolean(int i) {
    if (i >= size_) {
      return false;
    }
    Object o = objects[i];

    if (o == NUMBER_MARKER) {
      double d = numbers[i];
      return d != 0 && !Double.IsNaN(d);
    }
	if ((bool)o == true) {
      return true;
    }
	if ((bool)o == false) {
      return false;
    }

    return JsSystem.toBoolean(o);
  }

  public virtual JsObject getJsObject(int i) {
    return JsSystem.toJsObject(getObject(i));
  }
  
  
  /**
   * Returns the numerical value at array index i. If the actual value is not
   * numeric, it is converted automatically. 
   */
  public double getNumber(int i) {
    if (i >= size_){
      return 0;
    }
    Object o = objects[i];
    return o == NUMBER_MARKER ? numbers[i] : JsSystem.toNumber(o);
  }

  /** 
   * Returns the active size of this array.
   */
  public virtual int size() {
    return size_;
  }

  /** 
   * Returns the value at array index i, casted to an integer. This routine
   * corresponds to toInt32 in the ECMAScript v3 specification.
   */
   public int getInt(int i) {
    double d = getNumber(i);
    
	if (Double.IsInfinity(d) || Double.IsNaN(d)) {
      return 0;
    } else {
      return (int) (long) d;
    }
  }

  /**
   * Returns the Object at the given index. Numeric values are returned as an
   * instance of Double. Used to transfer values between hashtables and arrayz
   */
  public Object getObject(int i) {
    if(i >= size_) return null;
    Object o = objects[i];
    return o == NUMBER_MARKER ? (Double)(numbers[i]) : o;
  }

  public virtual string getString(int i){
    Object o = getObject(i);
	return (o is string) 
        ? ((String) o)
            : JsSystem.toString(o);
  }
  
  /**
   * Shortcut for setFP(i, v &lt;&lt; 16);
   */
  public void setInt(int i, int v) {
    setNumber(i, v);
  }

  /**
   * Set the fix point value at array index i to v
   */
  public void setNumber(int i, double v) {
    if (i >= size_) {
      setObject(i, null);
    }
    objects[i] = NUMBER_MARKER;
    numbers[i] = v;
  }

  /**
   * Set the object at the given index. For Long objects, the LONG_MARKER is set
   * in the object array and the longValue is stored in longs. In contrast to 
   * Java arrays, the array grows automatically.
   */
   public void setObject(int i, Object v) {
    // System.out.println("size: "+size+" arr.len: "+objects.length+" i: "+i);
    if (i >= size_) {
      size_ = i + 1;
	  if (i >= objects.Length) {
        double[] newNums = new double[i * 3 / 2];
		Array.Copy(numbers, 0, newNums, 0, numbers.Length);
        numbers = newNums;
		Object[] newObjects = new Object[numbers.Length];
		Array.Copy(objects, 0, newObjects, 0, objects.Length);
        objects = newObjects;
      }
    }
	if (v is Double) {
	  numbers[i] = (double)((Double) v);
      objects[i] = NUMBER_MARKER;
    } else {
      objects[i] = v;
    }
  }

  /**
   * Swap the values at indices i1 and i2. i1 and i2 both must be smaller than
   * size.
   */
  public virtual void swap(int i1, int i2) {
    double f = numbers[i1];
    Object o = objects[i1];
    numbers[i1] = numbers[i2];
    objects[i1] = objects[i2];
    numbers[i2] = f;
    objects[i2] = o;
  }

  /**
   * Copy len values from index from to index to. The ranges may overlap, but
   * Max(from, to)+len must be smaller than size.
   * 
   * @param from source index
   * @param to target index
   * @param len number of elements to copy
   */
  public virtual void copy(int from, JsArray target, int to, int len) {
    if (target.size_ < to + len) {
      target.setObject(to + len - 1, null);
    }
    
	int maxIdx = Math.Min(size_, from + len);
	int l = Math.Max(0, maxIdx - from);
    
	Array.Copy(numbers, from, target.numbers, to, l);
	Array.Copy(objects, from, target.objects, to, l);
    
    for(int i = to + l; i < maxIdx; i++){
      target.setObject(i, null);
    }
  }

  /**
   * Copy a single value at the index "from" to index "to" in the array 
   * "target". 
   */
  public virtual void copy(int from, JsArray target, int to) {
    if (from >= size_) {
      target.setObject(to, null);
      return;
    }    
    if (to >= target.size_) {
      target.setObject(to, null);
    }
    target.numbers[to] = numbers[from];
    target.objects[to] = objects[from];
  }

  /** 
   * Returns a string representation of this array. This method does not check for
   * circular references, so if the array contains itself it will result in an 
   * Exception.
   */
   public override string ToString() {
     StringBuilder buf = new StringBuilder();

    if(size_ > 0){
      buf.Append(JsSystem.toString(getObject(0)));
      for (int i = 1; i < size_; i++) {
        buf.Append(",");
        buf.Append(JsSystem.toString(getObject(i)));
      }
    }

    return buf.ToString();
  }

  /**
   * Determines whether the value can be converted to a number in a
   * meaningful way (used to decide whether the plus operator operates
   * on toNumber or toString). 
   */
  public virtual bool isNumber(int i) {
    if (i >= size_) {
      return true;
    }
    Object o = objects[i];
    return o == NUMBER_MARKER || (bool)o == true || (bool)o == false || 
      (o is JsDate);
  }

  /**
   * Determines whether the value at index i is a valid array index. 
   */  
  public virtual bool isArrayIndex(int i) {
    if (i >= size_) {
      return false;
    }
    Object o = objects[i];
    if(o == NUMBER_MARKER) {
      double d = numbers[i];
      return d >= 0 && (d == (int) d);
    }
    if (o is String) {
      String s = (String) o;
	  if(s.Length == 0) return false;
      for (int j = 0; j < s.Length; j++){
	    char c = s[j];
        if(c < '0' || c > '9') {
          return false;
        }
      }
      return true;
    }
    return false;
  }

  /**
   * Override the set method of JsObject to handle the SET byte code
   * (property/array write access).
   */
  public override void vmSetOperation(JsArray stack, int keyIndex, int valueIndex) {
    if (stack.isArrayIndex(keyIndex)) {
      stack.copy(valueIndex, this, stack.getInt(keyIndex));
    } else {
      base.vmSetOperation(stack, keyIndex, valueIndex);
    }
  }

  /** 
   * Returns an enumeration of property names for this object.
   */ 
  public override IEnumerator keys(){
    ArrayList v = new ArrayList();
	for (IEnumerator e = base.keys(); e.MoveNext();){
	  v.Add(e.Current);
    }

    for(int i = 0; i < size_; i++){
      if(objects[i] != null) {
			v.Add((Double)(i));
      }
    }
    
	return v.GetEnumerator();
  }
  
  /**
   * Handles the GET_MEMBER instruction (property/array read access).
   * Overrides the corresponding method in JsObject.
   */
  public override void vmGetOperation(JsArray stack, int keyIndex, int valueIndex) {
    if (stack.isArrayIndex(keyIndex)) {
      copy(stack.getInt(keyIndex), stack, valueIndex);
    } else {
		  base.vmGetOperation(stack, keyIndex, valueIndex);
    }
  }

  /**
   * Determines whether the array content at the given index is null or 
   * undefined.
   *
   * @param i Array index
   * @return true if the array content at the given index is null or undefined
   */
  public virtual bool isNull(int i) {
    return i >= size_ || objects[i] == null || objects [i] == JsSystem.JS_NULL;
  }

  /**
   * Java implementations of JS array members and methods.
   */
  public override void evalNative(int index, JsArray stack, 
      int sp, int parCount) {
    switch (index) {
      
      case ID_CONCAT:
        JsArray array = new JsArray();
        copy(0, array, 0, size_);
        stack.copy(sp+2, array, size_, parCount);
        stack.setObject(sp, array);
        break;
      
      case ID_JOIN:
	    StringBuilder buf = new StringBuilder();
        String sep = stack.isNull(sp+2) ? "," : stack.getString(sp+2);
        for(int i = 0; i < size_; i++){
          if(i > 0){
            buf.Append(sep);
          }
          if(!isNull(i)) {
            buf.Append(getString(i));
          }
        }
        stack.setObject(sp, buf.ToString());
        break;
        
      case ID_LENGTH_SET:
        setSize(stack.getInt(sp));
        break;
      case ID_LENGTH:
        stack.setInt(sp, size_);
        break;
        
      case ID_POP:
        if(size_ == 0){
          stack.setObject(sp, null);
        }
        else {
          copy(size_-1, stack, sp);
          setSize(size_ - 1);
        }
        break;
      
      case ID_PUSH:
        stack.copy(sp+2, this, size_, parCount);
        stack.setNumber(sp, size_);
        break;
        
      case ID_REVERSE:
        for(int i = 0; i < size_/2; i ++) {
          swap(i, size_-1-i);
        }
        break;
        
      case ID_SHIFT:
        if(size_ == 0){
          stack.setObject(sp, null);
        }
        else {
          copy(0, stack, sp);
          copy(1, this, 0, size_-1);
          setSize(size_ - 1);
        }
        break;
        
      case ID_SLICE:
        int start = stack.getInt(sp+2);
        int end = stack.isNull(sp+3) ? size_ : stack.getInt(sp + 3);
        if (start < 0) {
          start = Math.Max(size_ + start, 0);
        }
        if(end < 0) {
          end = Math.Max(size_ + start, 0);
        }
        if(start > size_){
          start = size_;
        }
        if(end > size_) {
          end = size_;
        }
        if(end < start) {
          end = start;
        }
        array = new JsArray();
        copy(start, array, 0, end-start);
        stack.setObject(sp, array);
        break;
        
      case ID_SORT:
        Object compare = stack.getObject(sp + 2);
        if (compare is JsFunction) {
          sort(0, size_, (JsFunction) compare, stack.getJsObject(sp), stack, sp);
        }
        else {
          sort(0, size_);          
        }
        stack.setObject(sp, this);
        break;
        
      case ID_SPLICE:
        array = new JsArray();
        start = stack.getInt(sp + 2);
        int delCount = stack.getInt(sp + 3);
        if(start < 0){
          start = size_ - start;
        }
        int itemCount = Math.Max(0, parCount - 2);
        copy(0, array, 0, start);
        stack.copy(sp + 4, array, start, itemCount);
        copy(start + delCount, array, start + itemCount, size_ - (start + delCount));
        stack.setObject(sp, array);
        break;
        
      case ID_UNSHIFT:
        copy(0, this, parCount, size_);
        stack.copy(sp + 2, this, 0, parCount);
        stack.setInt(sp, parCount);
        break;
        
      default:
        base.evalNative(index, stack, sp, parCount);
		break;
    }
  }

  /**
   * Sort the array contents between the given indices using string comparison
   * 
   * @param left lower interval border (inclusive)
   * @param right higher interval border (exclusive)
   */
  private void sort(int left, int right) {

    if(right > left + 1){
       int pivotIndex = left + JsSystem.random.Next(right-left);
       String pivotValue = getString(pivotIndex);
       swap(pivotIndex, right-1);
       int storeIndex = left;
       
       for(int i = left; i < right - 1; i++) {
         if(getString(i).CompareTo(pivotValue) <= 0) {
           swap(i, storeIndex++);
         }
       }
       swap(storeIndex, right-1);
       
       sort(left, storeIndex);
       sort(storeIndex, right);
    }
  }

  /**
   * Sort the array contents between the given indices using a Javascript
   * comparator function,
   * 
   * @param left lower interval border (inclusive)
   * @param right higher interval border (exclusive)
   */
  private void sort(int left, int right, JsFunction compare, JsObject ctx, 
      JsArray stack, int sp) {

    if(right > left + 1){
       int pivotIndex = left + JsSystem.random.Next(right-left);
       swap(pivotIndex, right-1);
       int storeIndex = left;
       
       for(int i = left; i < right - 1; i++) {
         stack.setObject(sp, ctx);
         stack.setObject(sp + 1, compare);
         copy(i, stack, sp + 2);
         copy(right-1, stack, sp + 3);
         compare.eval(stack, sp, 2);
         
         if(stack.getNumber(sp) <= 0) {
           swap(i, storeIndex++);
         }
       }
       swap(storeIndex, right-1);
       
       sort(left, storeIndex, compare, ctx, stack, sp);
       sort(storeIndex, right, compare, ctx, stack, sp);
    }
  }
  
  /**
   * Sets the size of the array to the given new size.
   * 
   * @param newLen the new array size
   */
  public void setSize(int newLen) {
    if (objects.Length < newLen) {
      setObject(newLen - 1, null);
    } else {
      size_ = newLen;
    }
  }

  /** 
   * Sets a boolean value for the given index.
   * 
   * @param i index
   * @param b value
   */
	  public virtual void setBoolean(int i, bool b){
		setObject(i, b ? true : false);
  }

  /**
   * Returns the element type for the given index (One of TYPE_XXX constants)
   * defined in JsObject.
   * 
   * @param i index
   * @return type 
   */
  public virtual int getType(int i) {
    if (i > size_) return TYPE_UNDEFINED;
    Object o = objects[i];
    if (o == NUMBER_MARKER) {
      return TYPE_NUMBER;
    }
    if ((bool)o == true || (bool)o == false) {
      return TYPE_BOOLEAN;
    }
    if (o == null) {
      return TYPE_UNDEFINED;
    }
    if (o == JsSystem.JS_NULL) {
      return TYPE_NULL;
    }
    if (o is String) {
      return TYPE_STRING;
    }
		if (o is JsFunction) {
      return TYPE_FUNCTION;
    }
    return TYPE_OBJECT;
  }
}

}