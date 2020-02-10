using System;

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

/**
 * Javascript Error class implementation. 
 * 
 * @author Stefan Haustein
 */
public class JsError : JsObject {
  
  internal static readonly JsObject ERROR_PROTOTYPE = 
      new JsObject(OBJECT_PROTOTYPE).addVar("name", "Error");
  
  internal static readonly JsObject EVAL_ERROR_PROTOTYPE = 
      new JsObject(ERROR_PROTOTYPE).addVar("name", "EvalError");

  internal static readonly JsObject RANGE_ERROR_PROTOTYPE = 
      new JsObject(ERROR_PROTOTYPE).addVar("name", "RangeError");

  internal static readonly JsObject REFERENCE_ERROR_PROTOTYPE = 
      new JsObject(ERROR_PROTOTYPE).addVar("name", "ReferenceError");

  internal static readonly JsObject SYNTAX_ERROR_PROTOTYPE = 
      new JsObject(ERROR_PROTOTYPE).addVar("name", "SyntaxError");
  
  internal static readonly JsObject TYPE_ERROR_PROTOTYPE = 
      new JsObject(ERROR_PROTOTYPE).addVar("name", "TypeError");

  internal static readonly JsObject URI_ERROR_PROTOTYPE = 
      new JsObject(ERROR_PROTOTYPE).addVar("name", "URIError");

  /** 
   * Generates a matching Javascript error for the given Java exception. 
   */
  public JsError(Exception e) : 
    this(getPrototype(e), e.ToString()) {
  }
  
  /** 
   * Constructs an Error instance with the given prototype and message.
   */
  public JsError(JsObject proto, string message) : 
    base(proto) {
    setObject("message", message);
  }

  /**
   * Returns the name and message of this error object.
   */
  public override String ToString(){
    return getString("name") + ": " + getString("message");
  }
  
  /**
   * Returns the Javascript prototype for the Javascript error best matching the given
   * Java exception class.
   */
  internal static JsObject getPrototype(Exception e) {
    if (e is NullReferenceException) {
      return REFERENCE_ERROR_PROTOTYPE;
    }
    if (e is IndexOutOfRangeException) {
      return RANGE_ERROR_PROTOTYPE;
    }
    if (e is InvalidCastException) {
      return TYPE_ERROR_PROTOTYPE;
    }
    return ERROR_PROTOTYPE;
  }
  
}

}
