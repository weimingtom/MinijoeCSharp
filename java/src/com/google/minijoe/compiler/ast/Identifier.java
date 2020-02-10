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

package com.google.minijoe.compiler.ast;

import com.google.minijoe.compiler.CompilerException;
import com.google.minijoe.compiler.visitor.Visitor;

/**
 * @author Andy Hayward
 */
public class Identifier extends Expression {
  public String str;
  public int index = -1;

  public Identifier(String str) {
    this.str = str;
  }

  public boolean equals(Object obj) {
    if (obj == null) {
      return false;
    }

    if (this.getClass() != obj.getClass()) {
      return false;
    }

    Identifier other = (Identifier) obj;

    return this.str.equals(other.str);
  }

  public int hashCode() {
    return str.hashCode();
  }

  public String toString() {
    return str;
  }

  public Expression visitExpression(Visitor visitor) throws CompilerException {
    return visitor.visit(this);
  }
}
