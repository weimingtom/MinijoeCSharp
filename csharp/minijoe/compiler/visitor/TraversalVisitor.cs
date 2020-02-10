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
//import com.google.minijoe.compiler.ast.BinaryExpression;
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
//import com.google.minijoe.compiler.ast.UnaryExpression;
//import com.google.minijoe.compiler.ast.UnaryOperatorExpression;
//import com.google.minijoe.compiler.ast.VariableDeclaration;
//import com.google.minijoe.compiler.ast.VariableExpression;
//import com.google.minijoe.compiler.ast.VariableStatement;
//import com.google.minijoe.compiler.ast.WhileStatement;
//import com.google.minijoe.compiler.ast.WithStatement;

/**
 * @author Andy Hayward
 */
public class TraversalVisitor : Visitor {
  internal Visitor visitor;

  public TraversalVisitor() {
  }

  public TraversalVisitor(Visitor visitor) {
    this.visitor = visitor;
  }

  //
  // utilities
  //

  protected internal virtual Statement[] visitStatementArray(Statement[] statements) {
    if (statements != null) {
	  for (int i = 0; i < statements.Length; i++) {
        statements[i] = visitStatement(statements[i]);
      }
    }

    return statements;
  }

  protected internal virtual Statement visitStatement(Statement statement) {
    if (statement != null) {
      statement = statement.visitStatement(visitor);
    }

    return statement;
  }

  protected internal virtual Expression[] visitExpressionArray(Expression[] expressions) {
    if (expressions != null) {
	  for (int i = 0; i < expressions.Length; i++) {
        expressions[i] = visitExpression(expressions[i]);
      }
    }

    return expressions;
  }

  protected internal virtual Expression visitExpression(Expression expression) {
    if (expression != null) {
      expression = expression.visitExpression(visitor);
    }

    return expression;
  }

  protected internal virtual Expression visitBinaryExpression(BinaryExpression expression) {
    expression.leftExpression = visitExpression(expression.leftExpression);
    expression.rightExpression = visitExpression(expression.rightExpression);

    return expression;
  }

  protected internal virtual Expression visitUnaryExpression(UnaryExpression expression) {
    expression.subExpression = visitExpression(expression.subExpression);

    return expression;
  }


  protected internal virtual Identifier[] visitIdentifierArray(Identifier[] identifiers) {
    if (identifiers != null) {
	  for (int i = 0; i < identifiers.Length; i++) {
        identifiers[i] = visitIdentifier(identifiers[i]);
      }
    }

    return identifiers;
  }

  protected internal virtual Identifier visitIdentifier(Identifier identifier) {
    if (identifier != null) {
      identifier = (Identifier) identifier.visitExpression(visitor);
    }

    return identifier;
  }

  //
  // nodes
  //

  public virtual ProgramNode visit(ProgramNode program) {
    program.functions = visitStatementArray(program.functions);
    program.statements = visitStatementArray(program.statements);

    return program;
  }

  //
  // statements
  //

  public virtual Statement visit(FunctionDeclaration functionDeclaration) {
    functionDeclaration.literal = (FunctionLiteral) visitExpression(functionDeclaration.literal);

    return functionDeclaration;
  }

  public virtual Statement visit(BlockStatement blockStatement) {
    blockStatement.statements = visitStatementArray(blockStatement.statements);

    return blockStatement;
  }

  public virtual Statement visit(BreakStatement breakStatement) {
    breakStatement.identifier = visitIdentifier(breakStatement.identifier);

    return breakStatement;
  }

  public virtual Statement visit(CaseStatement caseStatement) {
    caseStatement.expression = visitExpression(caseStatement.expression);
    caseStatement.statements = visitStatementArray(caseStatement.statements);

    return caseStatement;
  }


  public virtual Statement visit(ContinueStatement continueStatement) {
    continueStatement.identifier = visitIdentifier(continueStatement.identifier);

    return continueStatement;
  }

  public virtual Statement visit(DoStatement doStatement) {
    doStatement.statement = visitStatement(doStatement.statement);
    doStatement.expression = visitExpression(doStatement.expression);

    return doStatement;
  }

  public virtual Statement visit(EmptyStatement emptyStatement) {
    return emptyStatement;
  }

  public virtual Statement visit(ExpressionStatement expressionStatement) {
    expressionStatement.expression = visitExpression(expressionStatement.expression);

    return expressionStatement;
  }

  public virtual Statement visit(ForStatement forStatement) {
    forStatement.initial = visitExpression(forStatement.initial);
    forStatement.condition = visitExpression(forStatement.condition);
    forStatement.increment = visitExpression(forStatement.increment);
    forStatement.statement = visitStatement(forStatement.statement);

    return forStatement;
  }

  public virtual Statement visit(ForInStatement forInStatement) {
    forInStatement.variable = visitExpression(forInStatement.variable);
    forInStatement.expression = visitExpression(forInStatement.expression);
    forInStatement.statement = visitStatement(forInStatement.statement);

    return forInStatement;
  }

  public virtual Statement visit(IfStatement ifStatement) {
    ifStatement.expression = visitExpression(ifStatement.expression);
    ifStatement.trueStatement = visitStatement(ifStatement.trueStatement);
    ifStatement.falseStatement = visitStatement(ifStatement.falseStatement);

    return ifStatement;
  }

  public virtual Statement visit(LabelledStatement labelledStatement) {
    labelledStatement.identifier = visitIdentifier(labelledStatement.identifier);
    labelledStatement.statement = visitStatement(labelledStatement.statement);

    return labelledStatement;
  }

  public virtual Statement visit(ReturnStatement returnStatement) {
    returnStatement.expression = visitExpression(returnStatement.expression);

    return returnStatement;
  }

  public virtual Statement visit(SwitchStatement switchStatement) {
    switchStatement.expression = visitExpression(switchStatement.expression);
    switchStatement.clauses = (CaseStatement[]) visitStatementArray(switchStatement.clauses);

    return switchStatement;
  }

  public virtual Statement visit(ThrowStatement throwStatement) {
    throwStatement.expression = visitExpression(throwStatement.expression);

    return throwStatement;
  }

  public virtual Statement visit(TryStatement tryStatement) {
    tryStatement.tryBlock = visitStatement(tryStatement.tryBlock);
    tryStatement.catchIdentifier = visitIdentifier(tryStatement.catchIdentifier);
    tryStatement.catchBlock = visitStatement(tryStatement.catchBlock);
    tryStatement.finallyBlock = visitStatement(tryStatement.finallyBlock);

    return tryStatement;
  }

  public virtual Statement visit(VariableStatement variableStatement) {
    variableStatement.declarations =
        (VariableDeclaration[]) visitExpressionArray(variableStatement.declarations);

    return variableStatement;
  }

  public virtual Statement visit(WhileStatement whileStatement) {
    whileStatement.expression = visitExpression(whileStatement.expression);
    whileStatement.statement = visitStatement(whileStatement.statement);

    return whileStatement;
  }

  public virtual Statement visit(WithStatement withStatement) {
    withStatement.expression = visitExpression(withStatement.expression);
    withStatement.statement = visitStatement(withStatement.statement);

    return withStatement;
  }

  //
  // Expressions
  //

  public virtual Expression visit(AssignmentExpression expression) {
    return visitBinaryExpression(expression);
  }

  public virtual Expression visit(AssignmentOperatorExpression expression) {
    return visitBinaryExpression(expression);
  }

  public virtual Expression visit(BinaryOperatorExpression expression) {
    return visitBinaryExpression(expression);
  }

  public virtual Expression visit(CallExpression callExpression) {
    callExpression.function = visitExpression(callExpression.function);
    callExpression.arguments = visitExpressionArray(callExpression.arguments);

    return callExpression;
  }

  public virtual Expression visit(ConditionalExpression conditionalExpression) {
    conditionalExpression.expression = visitExpression(conditionalExpression.expression);
    conditionalExpression.trueExpression = visitExpression(conditionalExpression.trueExpression);
    conditionalExpression.falseExpression = visitExpression(conditionalExpression.falseExpression);

    return conditionalExpression;
  }

  public virtual Expression visit(DeleteExpression expression) {
    return visitUnaryExpression(expression);
  }

  public virtual Expression visit(IncrementExpression expression) {
    return visitUnaryExpression(expression);
  }

  public virtual Expression visit(LogicalAndExpression expression) {
    return visitBinaryExpression(expression);
  }

  public virtual Expression visit(LogicalOrExpression expression) {
    return visitBinaryExpression(expression);
  }

  public virtual Expression visit(NewExpression newExpression) {
    newExpression.function = visitExpression(newExpression.function);
    newExpression.arguments = visitExpressionArray(newExpression.arguments);

    return newExpression;
  }

  public virtual Expression visit(PropertyExpression expression) {
    return visitBinaryExpression(expression);
  }

  public virtual Expression visit(UnaryOperatorExpression expression) {
    return visitUnaryExpression(expression);
  }

  public virtual Expression visit(VariableExpression variableExpression) {
    variableExpression.declarations =
        (VariableDeclaration[]) visitExpressionArray(variableExpression.declarations);

    return variableExpression;
  }

  public virtual Expression visit(VariableDeclaration variableDeclaration) {
    variableDeclaration.identifier = visitIdentifier(variableDeclaration.identifier);
    variableDeclaration.initializer = visitExpression(variableDeclaration.initializer);

    return variableDeclaration;
  }

  //
  // literals
  //

  public virtual Expression visit(Identifier identifier) {
    return identifier;
  }

  public virtual Expression visit(ThisLiteral thisLiteral) {
    return thisLiteral;
  }

  public virtual Expression visit(NullLiteral nullLiteral) {
    return nullLiteral;
  }

  public virtual Expression visit(BooleanLiteral booleanLiteral) {
    return booleanLiteral;
  }

  public virtual Expression visit(NumberLiteral numberLiteral) {
    return numberLiteral;
  }

  public virtual Expression visit(StringLiteral stringLiteral) {
    return stringLiteral;
  }

  public virtual Expression visit(ArrayLiteral arrayLiteral) {
    arrayLiteral.elements = visitExpressionArray(arrayLiteral.elements);

    return arrayLiteral;
  }

  public virtual Expression visit(FunctionLiteral functionLiteral) {
    functionLiteral.name = visitIdentifier(functionLiteral.name);
    functionLiteral.parameters = visitIdentifierArray(functionLiteral.parameters);
    functionLiteral.variables = visitIdentifierArray(functionLiteral.variables);
    functionLiteral.functions = visitStatementArray(functionLiteral.functions);
    functionLiteral.statements = visitStatementArray(functionLiteral.statements);

    return functionLiteral;
  }

  public virtual Expression visit(ObjectLiteral objectLiteral) {
    objectLiteral.properties =
        (ObjectLiteralProperty[]) visitExpressionArray(objectLiteral.properties);

    return objectLiteral;
  }

  public virtual Expression visit(ObjectLiteralProperty objectLiteralProperty) {
    objectLiteralProperty.name = visitExpression(objectLiteralProperty.name);
    objectLiteralProperty.value = visitExpression(objectLiteralProperty.value);

    return objectLiteralProperty;
  }
}

}