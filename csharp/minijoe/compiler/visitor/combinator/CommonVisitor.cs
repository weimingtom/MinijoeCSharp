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
 * @author Andy Hayward
 */
public abstract class CommonVisitor : Visitor {
  public abstract ProgramNode visit(ProgramNode program);
  
  //
  // nodes
  //

  public abstract Statement visit(Statement statement);
  public abstract Expression visit(Expression expression);

  //
  // statements
  //

  public virtual Statement visit(FunctionDeclaration declaration) {
    return visit((Statement) declaration);
  }

  public virtual Statement visit(BlockStatement statement) {
    return visit((Statement) statement);
  }

  public virtual Statement visit(BreakStatement statement) {
    return visit((Statement) statement);
  }

  public virtual Statement visit(CaseStatement statement) {
    return visit((Statement) statement);
  }

  public virtual Statement visit(ContinueStatement statement) {
    return visit((Statement) statement);
  }

  public virtual Statement visit(DoStatement statement) {
    return visit((Statement) statement);
  }

  public virtual Statement visit(EmptyStatement statement) {
    return visit((Statement) statement);
  }

  public virtual Statement visit(ExpressionStatement statement) {
    return visit((Statement) statement);
  }

  public virtual Statement visit(ForStatement statement) {
    return visit((Statement) statement);
  }

  public virtual Statement visit(ForInStatement statement) {
    return visit((Statement) statement);
  }

  public virtual Statement visit(IfStatement statement) {
    return visit((Statement) statement);
  }

  public virtual Statement visit(LabelledStatement statement) {
    return visit((Statement) statement);
  }

  public virtual Statement visit(ReturnStatement statement) {
    return visit((Statement) statement);
  }

  public virtual Statement visit(SwitchStatement statement) {
    return visit((Statement) statement);
  }

  public virtual Statement visit(ThrowStatement statement) {
    return visit((Statement) statement);
  }

  public virtual Statement visit(TryStatement statement) {
    return visit((Statement) statement);
  }

  public virtual Statement visit(VariableStatement statement) {
    return visit((Statement) statement);
  }

  public virtual Statement visit(WhileStatement statement) {
    return visit((Statement) statement);
  }

  public virtual Statement visit(WithStatement statement) {
    return visit((Statement) statement);
  }

  //
  // expressions
  //

  public virtual Expression visit(AssignmentExpression expression) {
    return visit((Expression) expression);
  }

  public virtual Expression visit(AssignmentOperatorExpression expression) {
    return visit((Expression) expression);
  }

  public virtual Expression visit(BinaryOperatorExpression expression) {
    return visit((Expression) expression);
  }

  public virtual Expression visit(CallExpression expression) {
    return visit((Expression) expression);
  }

  public virtual Expression visit(ConditionalExpression expression) {
    return visit((Expression) expression);
  }

  public virtual Expression visit(DeleteExpression expression) {
    return visit((Expression) expression);
  }

  public virtual Expression visit(LogicalAndExpression expression) {
    return visit((Expression) expression);
  }

  public virtual Expression visit(LogicalOrExpression expression) {
    return visit((Expression) expression);
  }

  public virtual Expression visit(NewExpression expression) {
    return visit((Expression) expression);
  }

  public virtual Expression visit(IncrementExpression expression) {
    return visit((Expression) expression);
  }

  public virtual Expression visit(PropertyExpression expression) {
    return visit((Expression) expression);
  }

  public virtual Expression visit(UnaryOperatorExpression expression) {
    return visit((Expression) expression);
  }

  public virtual Expression visit(VariableExpression expression) {
    return visit((Expression) expression);
  }

  public virtual Expression visit(VariableDeclaration declaration) {
    return visit((Expression) declaration);
  }

  //
  // literals
  //

  public virtual Expression visit(Identifier literal) {
    return visit((Expression) literal);
  }

  public virtual Expression visit(ThisLiteral literal) {
    return visit((Expression) literal);
  }

  public virtual Expression visit(NullLiteral literal) {
    return visit((Expression) literal);
  }

  public virtual Expression visit(BooleanLiteral literal) {
    return visit((Expression) literal);
  }

  public virtual Expression visit(NumberLiteral literal) {
    return visit((Expression) literal);
  }

  public virtual Expression visit(StringLiteral literal) {
    return visit((Expression) literal);
  }

  public virtual Expression visit(ArrayLiteral literal) {
    return visit((Expression) literal);
  }

  public virtual Expression visit(FunctionLiteral literal) {
    return visit((Expression) literal);
  }

  public virtual Expression visit(ObjectLiteral literal) {
    return visit((Expression) literal);
  }

  public virtual Expression visit(ObjectLiteralProperty property) {
    return visit((Expression) property);
  }
}

}