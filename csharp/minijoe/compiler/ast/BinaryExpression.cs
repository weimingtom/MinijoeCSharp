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
 * @author Andy Hayward
 */
public abstract class BinaryExpression : Expression {
  public Expression leftExpression;
  public Expression rightExpression;

  public BinaryExpression(Expression left, Expression right) {
    if (left == null || right == null) {
	  throw new NullReferenceException();
    }

    this.leftExpression = left;
    this.rightExpression = right;
  }
}

}