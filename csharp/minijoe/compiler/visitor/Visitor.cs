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

/**
 * @author Andy Hayward
 */
public interface Visitor {
  //
  // nodes
  //

  ProgramNode visit(ProgramNode program);

  //
  // statements
  //

  Statement visit(FunctionDeclaration statement);
  Statement visit(BlockStatement statement);
  Statement visit(BreakStatement statement);
  Statement visit(CaseStatement statement);
  Statement visit(ContinueStatement statement);
  Statement visit(DoStatement statement);
  Statement visit(EmptyStatement statement);
  Statement visit(ExpressionStatement statement);
  Statement visit(ForStatement statement);
  Statement visit(ForInStatement statement);
  Statement visit(IfStatement statement);
  Statement visit(LabelledStatement statement);
  Statement visit(ReturnStatement statement);
  Statement visit(SwitchStatement statement);
  Statement visit(ThrowStatement statement);
  Statement visit(TryStatement statement);
  Statement visit(VariableStatement statement);
  Statement visit(WhileStatement statement);
  Statement visit(WithStatement statement);

  //
  // expressions
  //

  Expression visit(AssignmentExpression expression);
  Expression visit(AssignmentOperatorExpression expression);
  Expression visit(BinaryOperatorExpression expression);
  Expression visit(CallExpression expression);
  Expression visit(ConditionalExpression expression);
  Expression visit(DeleteExpression expression);
  Expression visit(IncrementExpression expression);
  Expression visit(LogicalAndExpression expression);
  Expression visit(LogicalOrExpression expression);
  Expression visit(NewExpression expression);
  Expression visit(PropertyExpression expression);
  Expression visit(UnaryOperatorExpression expression);
  Expression visit(VariableExpression expression);
  Expression visit(VariableDeclaration declaration);

  //
  // literals
  //

  Expression visit(Identifier identifier);
  Expression visit(ThisLiteral literal);
  Expression visit(NullLiteral literal);
  Expression visit(BooleanLiteral literal);
  Expression visit(NumberLiteral literal);
  Expression visit(StringLiteral literal);
  Expression visit(ArrayLiteral literal);
  Expression visit(FunctionLiteral literal);
  Expression visit(ObjectLiteral literal);
  Expression visit(ObjectLiteralProperty property);
}

}