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
 * Java Exception encapsulating a Javascript object that was thrown.
 * 
 * @author Stefan Haustein
 */
public class JsException : Exception {
  private JsObject error;
  internal int pc = -1;
  internal int lineNumber = -1;

  /**
   * Create a new Java exception for the given object.
   */
   public JsException(Object e) {
    if (e is Exception) {
      error = new JsError((Exception) e);
    } else if (e is JsError) {
      error = (JsError) e;
    } else {
      error = new JsError(JsError.ERROR_PROTOTYPE, "" + e);
    }
  }

  /**
   * Returns the encapsulated JavaScript object.
   */
  public JsObject getError() {
    return error;
  }

  /**
   * Returns the Javascript line number of the exception, or -1 if unknown.
   */
  public int getLineNumber() {
    return lineNumber;
  }

  /**
   * Returns a message describing the error condition.
   */
  public String getMessage() {
    return "Error at line " + lineNumber + " (pc:0x" + 
        MyInteger.toHexString(pc) + "): " + error;
  }
  public override string Message { get { return getMessage(); } }
  
}

}