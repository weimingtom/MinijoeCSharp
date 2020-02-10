using System;
using System.IO;

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
//import com.google.minijoe.compiler.ast.ArrayLiteral;
//import com.google.minijoe.compiler.ast.AssignmentExpression;
//import com.google.minijoe.compiler.ast.AssignmentOperatorExpression;
//import com.google.minijoe.compiler.ast.BinaryOperatorExpression;
//import com.google.minijoe.compiler.ast.BlockStatement;
//import com.google.minijoe.compiler.ast.BooleanLiteral;
//import com.google.minijoe.compiler.ast.BreakStatement;
//import com.google.minijoe.compiler.ast.CallExpression;
//import com.google.minijoe.compiler.ast.CaseStatement;
//import com.google.minijoe.compiler.ast.ConditionalExpression;
//import com.google.minijoe.compiler.ast.ContinueStatement;
//import com.google.minijoe.compiler.ast.DeleteExpression;
//import com.google.minijoe.compiler.ast.DoStatement;
//import com.google.minijoe.compiler.ast.EmptyStatement;
//import com.google.minijoe.compiler.ast.Expression;
//import com.google.minijoe.compiler.ast.ExpressionStatement;
//import com.google.minijoe.compiler.ast.ForInStatement;
//import com.google.minijoe.compiler.ast.ForStatement;
//import com.google.minijoe.compiler.ast.FunctionDeclaration;
//import com.google.minijoe.compiler.ast.FunctionLiteral;
//import com.google.minijoe.compiler.ast.Identifier;
//import com.google.minijoe.compiler.ast.IfStatement;
//import com.google.minijoe.compiler.ast.IncrementExpression;
//import com.google.minijoe.compiler.ast.LabelledStatement;
//import com.google.minijoe.compiler.ast.LogicalAndExpression;
//import com.google.minijoe.compiler.ast.LogicalOrExpression;
//import com.google.minijoe.compiler.ast.NewExpression;
//import com.google.minijoe.compiler.ast.NullLiteral;
//import com.google.minijoe.compiler.ast.NumberLiteral;
//import com.google.minijoe.compiler.ast.ObjectLiteral;
//import com.google.minijoe.compiler.ast.ObjectLiteralProperty;
//import com.google.minijoe.compiler.ast.ProgramNode;
//import com.google.minijoe.compiler.ast.PropertyExpression;
//import com.google.minijoe.compiler.ast.ReturnStatement;
//import com.google.minijoe.compiler.ast.Statement;
//import com.google.minijoe.compiler.ast.StringLiteral;
//import com.google.minijoe.compiler.ast.SwitchStatement;
//import com.google.minijoe.compiler.ast.ThisLiteral;
//import com.google.minijoe.compiler.ast.ThrowStatement;
//import com.google.minijoe.compiler.ast.TryStatement;
//import com.google.minijoe.compiler.ast.UnaryOperatorExpression;
//import com.google.minijoe.compiler.ast.VariableDeclaration;
//import com.google.minijoe.compiler.ast.VariableExpression;
//import com.google.minijoe.compiler.ast.VariableStatement;
//import com.google.minijoe.compiler.ast.WhileStatement;
//import com.google.minijoe.compiler.ast.WithStatement;
//
//import java.io.IOException;
//import java.io.Writer;

/**
 * @author Andy Hayward
 */
public class RoundtripVisitor : Visitor {
  internal Writer w;
  internal int indent = 0;

  public RoundtripVisitor(Writer w) {
	this.w = w;
  }

  //
  // utilities
  //

  private void increaseIndent() {
	indent += 2;
  }

  private void decreaseIndent() {
	indent -= 2;
  }

  private void visitStatementArray(Statement[] statements) {
	if (statements != null) {
	  for (int i = 0; i < statements.Length; i++) {
		visitStatement(statements[i]);
	  }
	}
  }

  private void visitStatement(Statement statement) {
	if (statement != null) {
	  statement.visitStatement(this);
	}
  }

  private void visitExpression(Expression expression) {
	if (expression != null) {
	  expression.visitExpression(this);
	}
  }

  private void write(String str) {
    try {
      w.write(str);
    } catch (IOException e) {
      throw new CompilerException(e);
    }
  }

  private void write(char c) {
    try {
      w.write(c);
    } catch (IOException e) {
      throw new CompilerException(e);
    }
  }

  private void writeIndent() {
    try {
      w.write('\n');
      for (int i = 0; i < indent; i++) {
        w.write(' ');
      }
    } catch (IOException e) {
      throw new CompilerException(e);
    }
  }

  //
  // nodes
  //

  public virtual ProgramNode visit(ProgramNode program) {
    visitStatementArray(program.statements);
    write("\n\n");
    return program;
  }

  //
  // statements
  //

  public virtual Statement visit(FunctionDeclaration declaration) {
    writeIndent();
    declaration.literal.visitExpression(this);
    write(';');
    return declaration;
  }

  public virtual Statement visit(BlockStatement statement) {
    writeIndent();
    write('{');
    increaseIndent();
    visitStatementArray(statement.statements);
    decreaseIndent();
    writeIndent();
    write('}');
    return statement;
  }

  public virtual Statement visit(BreakStatement statement) {
    writeIndent();
    if (statement.identifier != null) {
      write("break " + statement.identifier + ";");
    } else {
      write("break");
    }
    return statement;
  }

  public virtual Statement visit(CaseStatement statement) {
    writeIndent();
    if (statement.expression == null) {
      write("default:");
    } else {
      write("case ");
      statement.expression.visitExpression(this);
      write(": ");
    }
    visitStatementArray(statement.statements);
    return statement;
  }

  public virtual Statement visit(ContinueStatement statement) {
    writeIndent();
    if (statement.identifier != null) {
      write("continue " + statement.identifier + ";");
    } else {
      write("continue;");
    }
    return statement;
  }

  public virtual Statement visit(DoStatement statement) {
    writeIndent();
    write("do ");
    increaseIndent();
    statement.statement.visitStatement(this);
    decreaseIndent();
    writeIndent();
    write("while (");
    statement.expression.visitExpression(this);
    write(')');
    return statement;
  }

  public virtual Statement visit(EmptyStatement statement) {
    return statement;
  }

  public virtual Statement visit(ExpressionStatement statement) {
    writeIndent();
    statement.expression.visitExpression(this);
    write(';');
    return statement;
  }

  public virtual Statement visit(ForStatement statement) {
    writeIndent();
    write("for (");
    visitExpression(statement.initial);
    write("; ");
    visitExpression(statement.condition);
    write("; ");
    visitExpression(statement.increment);
    write(")");
    increaseIndent();
    statement.statement.visitStatement(this);
    decreaseIndent();
    return statement;
  }

  public virtual Statement visit(ForInStatement statement) {
    writeIndent();
    write("for (");
    statement.variable.visitExpression(this);
    write(" in ");
    statement.expression.visitExpression(this);
    write(")");
    statement.statement.visitStatement(this);
    return statement;
  }

  public virtual Statement visit(IfStatement statement) {
    writeIndent();
    write("if (");
    statement.expression.visitExpression(this);
    write(") ");
    statement.trueStatement.visitStatement(this);
    if (statement.falseStatement != null) {
      writeIndent();
      write("else ");
      statement.falseStatement.visitStatement(this);
    }
    return statement;
  }

  public virtual Statement visit(LabelledStatement statement) {
    writeIndent();
    statement.identifier.visitExpression(this);
    write(": ");
    statement.statement.visitStatement(this);
    return statement;
  }

  public virtual Statement visit(ReturnStatement statement) {
    writeIndent();
    if (statement.expression != null) {
      write("return ");
      statement.expression.visitExpression(this);
      write(";");
    } else {
      write("return;");
    }
    return statement;
  }

  public virtual Statement visit(SwitchStatement statement) {
    write("switch (");
    statement.expression.visitExpression(this);
    write(") {");
    visitStatementArray(statement.clauses);
    write("}");
    return statement;
  }

  public virtual Statement visit(ThrowStatement statement) {
    writeIndent();
    return null;
  }

  public virtual Statement visit(TryStatement statement) {
    writeIndent();
    write("try");
    statement.tryBlock.visitStatement(this);
    if (statement.catchBlock != null) {
      writeIndent();
      write("catch (");
      statement.catchIdentifier.visitExpression(this);
      write(")");
      statement.catchBlock.visitStatement(this);
    }
    if (statement.finallyBlock != null) {
      writeIndent();
      write("finally ");
      statement.finallyBlock.visitStatement(this);
    }
    return statement;
  }

  public virtual Statement visit(VariableStatement statement) {
	for (int i = 0; i < statement.declarations.Length; i++) {
      statement.declarations[i].visitExpression(this);
    }
    return statement;
  }

  public virtual Statement visit(WhileStatement statement) {
    writeIndent();
    write("while (");
    statement.expression.visitExpression(this);
    write(")");
    statement.statement.visitStatement(this);
    return statement;
  }

  public virtual Statement visit(WithStatement statement) {
    writeIndent();
    write("width (");
    statement.expression.visitExpression(this);
    write(")");
    statement.statement.visitStatement(this);
    return statement;
  }

  //
  // expressions
  //

  public virtual Expression visit(AssignmentExpression expression) {
    expression.leftExpression.visitExpression(this);
    write(" = ");
    expression.rightExpression.visitExpression(this);
    return expression;
  }

  public virtual Expression visit(AssignmentOperatorExpression expression) {
    expression.leftExpression.visitExpression(this);
    write(' ');
    write(expression.type.getValue());
    write(' ');
    expression.rightExpression.visitExpression(this);
    return expression;
  }

  public virtual Expression visit(BinaryOperatorExpression expression) {
    write('(');
    expression.leftExpression.visitExpression(this);
    write(' ');
    write(expression.operatorToken.getValue());
    write(' ');
    expression.rightExpression.visitExpression(this);
    write(')');

    return expression;
  }

  public virtual Expression visit(CallExpression expression) {
    expression.function.visitExpression(this);
    write('(');
	for (int i = 0; i < expression.arguments.Length; i++) {
      if (i > 0) {
        write(", ");
      }
      expression.arguments[i].visitExpression(this);
    }
    write(')');
    return null;
  }

  public virtual Expression visit(ConditionalExpression expression) {
    write("(");
    expression.expression.visitExpression(this);
    write(" ? ");
    expression.trueExpression.visitExpression(this);
    write(" : ");
    expression.falseExpression.visitExpression(this);
    write(")");
    return expression;
  }

  public virtual Expression visit(DeleteExpression expression) {
    write("delete ");
    expression.subExpression.visitExpression(this);
    return expression;
  }

  public virtual Expression visit(IncrementExpression expression) {
    if (expression.post) {
      expression.subExpression.visitExpression(this);
    }
    switch (expression.value) {
      case -1:
        write("--");
        break;
      case 1:
        write("++");
        break;
      default:
		throw new ArgumentException();
    }
    if (!expression.post) {
      expression.subExpression.visitExpression(this);
    }
    return expression;
  }

  public virtual Expression visit(LogicalAndExpression expression) {
    write('(');
    expression.leftExpression.visitExpression(this);
    write(" && ");
    expression.rightExpression.visitExpression(this);
    write(')');

    return expression;
  }

  public virtual Expression visit(LogicalOrExpression expression) {
    write('(');
    expression.leftExpression.visitExpression(this);
    write(" && ");
    expression.rightExpression.visitExpression(this);
    write(')');

    return expression;
  }

  public virtual Expression visit(NewExpression expression) {
    write("new ");
    expression.function.visitExpression(this);
    write("( ");
	for (int i = 0; i < expression.arguments.Length; i++) {
      if (i != 0) {
        write(", ");
      }
    }
    write(")");
    return expression;
  }

  public virtual Expression visit(PropertyExpression expression) {
    expression.leftExpression.visitExpression(this);
    write("[");
    expression.rightExpression.visitExpression(this);
    write("]");
    return expression;
  }

  public virtual Expression visit(UnaryOperatorExpression expression) {
    write('(');
    write(expression.operatorToken.getValue());
    write(' ');
    expression.subExpression.visitExpression(this);
    write(')');

    return expression;
  }

  public virtual Expression visit(VariableExpression expression) {
    write("var ");

	for (int i = 0; i < expression.declarations.Length; i++) {
      if (i != 0) {
        write(", ");
      }
      expression.declarations[i].visitExpression(this);
    }

    return expression;
  }

  public virtual Expression visit(VariableDeclaration declaration) {
    declaration.identifier.visitExpression(this);
    if (declaration.initializer != null) {
      write(" = ");
      declaration.initializer.visitExpression(this);
    }
    return declaration;
  }

  //
  // literals
  //

  public virtual Expression visit(Identifier identifier) {
    write(identifier.str);
    return identifier;
  }

  public virtual Expression visit(ThisLiteral literal) {
    write("this");
    return literal;
  }

  public virtual Expression visit(NullLiteral literal) {
    write("null");
    return literal;
  }

  public virtual Expression visit(BooleanLiteral literal) {
    write("" + literal.value);
    return literal;
  }

  public virtual Expression visit(NumberLiteral literal) {
    write("" + literal.value);
    return literal;
  }

  public virtual Expression visit(StringLiteral literal) {
    write("\"" + literal.str + "\"");
    return literal;
  }

  public virtual Expression visit(ArrayLiteral literal) {
    write("[");
	for (int i = 0; i < literal.elements.Length; i++) {
      if (i != 0) {
        write(", ");
      }
      if (literal.elements[i] != null) {
        literal.elements[i].visitExpression(this);
      }
    }
    return null;
  }

  public virtual Expression visit(FunctionLiteral literal) {
    write("function ");
    if (literal.name != null) {
      literal.name.visitExpression(this);
    }
    write('(');
	for (int i = 0; i < literal.parameters.Length; i++) {
      if (i > 0) {
        write(", ");
      }
      literal.parameters[i].visitExpression(this);
    }
    write(") {");
    increaseIndent();
    visitStatementArray(literal.statements);
    decreaseIndent();
    writeIndent();
    write("}");

    return literal;
  }

  public virtual Expression visit(ObjectLiteral literal) {
    write("{");
	for (int i = 0; i < literal.properties.Length; i++) {
      if (i > 0) {
        write(", ");
      }
      literal.properties[i].visitExpression(this);
    }
    write("}");
    return literal;
  }

  public virtual Expression visit(ObjectLiteralProperty property) {
    property.name.visitExpression(this);
    write(": ");
    property.value.visitExpression(this);
    return property;
  }
}

}