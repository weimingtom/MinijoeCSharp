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
//import com.google.minijoe.compiler.visitor.Visitor;

/**
 * The identity visitor.
 *
 * @author Andy Hayward
 */
public class IdentityVisitor : Visitor {
  //
  // nodes
  //

  public virtual ProgramNode visit(ProgramNode program) {
    return program;
  }

  //
  // statements
  //

  public virtual Statement visit(FunctionDeclaration declaration) {
    return declaration;
  }

  public virtual Statement visit(BlockStatement statement) {
    return statement;
  }

  public virtual Statement visit(BreakStatement statement) {
    return statement;
  }

  public virtual Statement visit(CaseStatement statement) {
    return statement;
  }

  public virtual Statement visit(ContinueStatement statement) {
    return statement;
  }

  public virtual Statement visit(DoStatement statement) {
    return statement;
  }

  public virtual Statement visit(EmptyStatement statement) {
    return statement;
  }

  public virtual Statement visit(ExpressionStatement statement) {
    return statement;
  }

  public virtual Statement visit(ForInStatement statement) {
    return statement;
  }

  public virtual Statement visit(ForStatement statement) {
    return statement;
  }

  public virtual Statement visit(IfStatement statement) {
    return statement;
  }

  public virtual Statement visit(LabelledStatement statement) {
    return statement;
  }

  public virtual Statement visit(ReturnStatement statement) {
    return statement;
  }

  public virtual Statement visit(SwitchStatement statement) {
    return statement;
  }

  public virtual Statement visit(ThrowStatement statement) {
    return statement;
  }

  public virtual Statement visit(TryStatement statement) {
    return statement;
  }

  public virtual Statement visit(VariableStatement statement) {
    return statement;
  }

  public virtual Statement visit(WhileStatement statement) {
    return statement;
  }

  public virtual Statement visit(WithStatement statement) {
    return statement;
  }

  //
  // expressions
  //

  public virtual Expression visit(AssignmentExpression expression) {
    return expression;
  }

  public virtual Expression visit(AssignmentOperatorExpression expression) {
    return expression;
  }

  public virtual Expression visit(BinaryOperatorExpression expression) {
    return expression;
  }

  public virtual Expression visit(CallExpression expression) {
    return expression;
  }

  public virtual Expression visit(ConditionalExpression expression) {
    return expression;
  }

  public virtual Expression visit(DeleteExpression expression) {
    return expression;
  }

  public virtual Expression visit(LogicalAndExpression expression) {
    return expression;
  }

  public virtual Expression visit(LogicalOrExpression expression) {
    return expression;
  }

  public virtual Expression visit(NewExpression expression) {
    return expression;
  }

  public virtual Expression visit(IncrementExpression expression) {
    return expression;
  }

  public virtual Expression visit(PropertyExpression expression) {
    return expression;
  }

  public virtual Expression visit(UnaryOperatorExpression expression) {
    return expression;
  }

  public virtual Expression visit(VariableDeclaration expression) {
    return expression;
  }

  public virtual Expression visit(VariableExpression expression) {
    return expression;
  }

  //
  // literals
  //

  public virtual Expression visit(Identifier identifier) {
    return identifier;
  }

  public virtual Expression visit(ThisLiteral literal) {
    return literal;
  }

  public virtual Expression visit(NullLiteral literal) {
    return literal;
  }

  public virtual Expression visit(BooleanLiteral literal) {
    return literal;
  }

  public virtual Expression visit(NumberLiteral literal) {
    return literal;
  }

  public virtual Expression visit(StringLiteral literal) {
    return literal;
  }

  public virtual Expression visit(ArrayLiteral literal) {
    return literal;
  }

  public virtual Expression visit(FunctionLiteral literal) {
    return literal;
  }

  public virtual Expression visit(ObjectLiteral literal) {
    return literal;
  }

  public virtual Expression visit(ObjectLiteralProperty property) {
    return property;
  }
}

}