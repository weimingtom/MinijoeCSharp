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

//import com.google.minijoe.compiler.ast.Expression;
//import com.google.minijoe.compiler.ast.ProgramNode;
//import com.google.minijoe.compiler.ast.Statement;
//import com.google.minijoe.compiler.visitor.TraversalVisitor;
//import com.google.minijoe.compiler.visitor.Visitor;

/**
 * @author Andy Hayward
 */
public class DebugVisitor : SequenceVisitor {
internal class IncrementVisitor : CommonVisitor {
    PrintVisitor visitor;

    public IncrementVisitor(PrintVisitor visitor) {
      this.visitor = visitor;
    }

	public override ProgramNode visit(ProgramNode program) {
      visitor.incrementDepth();

      return program;
    }

	public override Statement visit(Statement statement) {
      visitor.incrementDepth();

      return statement;
    }

	public override Expression visit(Expression expression) {
      visitor.incrementDepth();

      return expression;
    }
  }

  class DecrementVisitor : CommonVisitor {
    PrintVisitor visitor;

    public DecrementVisitor(PrintVisitor visitor) {
      this.visitor = visitor;
    }

	public override ProgramNode visit(ProgramNode program) {
      visitor.decrementDepth();

      return program;
    }

	public override Statement visit(Statement statement) {
      visitor.decrementDepth();

      return statement;
    }

	public override Expression visit(Expression expression) {
      visitor.decrementDepth();

      return expression;
    }
  }

  public DebugVisitor() {
	PrintVisitor visitor = new PrintVisitor();

    visitors = new Visitor[] {
        visitor,
        new IncrementVisitor(visitor),
        new TraversalVisitor(this),
        new DecrementVisitor(visitor)
    };
  }
}

}