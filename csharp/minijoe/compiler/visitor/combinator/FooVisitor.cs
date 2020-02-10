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

//import com.google.minijoe.compiler.CompilerException;
//import com.google.minijoe.compiler.ast.Expression;
//import com.google.minijoe.compiler.ast.FunctionLiteral;
//import com.google.minijoe.compiler.ast.ProgramNode;
//import com.google.minijoe.compiler.visitor.TraversalVisitor;
//import com.google.minijoe.compiler.visitor.Visitor;

/**
 * @author Andy Hayward
 */
public class FooVisitor : IdentityVisitor {
  internal class FooTraversal : PostfixVisitor {
 	public FooTraversal(Visitor visitor) : 
	  base(visitor) {
    }

	public override Expression visit(FunctionLiteral literal) {
      return literal.visitExpression(new FooVisitor());
    }
  }

  public override ProgramNode visit(ProgramNode program) {
	Console.WriteLine(this + " visit #1 to  " + program);

    new TraversalVisitor(new FooTraversal(this)).visit(program);

	Console.WriteLine(this + " visit #2 to  " + program);

    return program;
  }

  public override Expression visit(FunctionLiteral literal) {
	Console.WriteLine(this + " visit #1 to " + literal);

    new TraversalVisitor(new FooTraversal(this)).visit(literal);

	Console.WriteLine(this + " visit #2 to " + literal);

    return literal;
  }
}

}