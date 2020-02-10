using System.Collections;

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
//import com.google.minijoe.compiler.Token;
//import com.google.minijoe.compiler.CompilerUtil;
//import com.google.minijoe.compiler.ast.AssignmentExpression;
//import com.google.minijoe.compiler.ast.BinaryOperatorExpression;
//import com.google.minijoe.compiler.ast.BlockStatement;
//import com.google.minijoe.compiler.ast.EmptyStatement;
//import com.google.minijoe.compiler.ast.Expression;
//import com.google.minijoe.compiler.ast.ExpressionStatement;
//import com.google.minijoe.compiler.ast.FunctionDeclaration;
//import com.google.minijoe.compiler.ast.FunctionLiteral;
//import com.google.minijoe.compiler.ast.Identifier;
//import com.google.minijoe.compiler.ast.ProgramNode;
//import com.google.minijoe.compiler.ast.Statement;
//import com.google.minijoe.compiler.ast.VariableDeclaration;
//import com.google.minijoe.compiler.ast.VariableExpression;
//import com.google.minijoe.compiler.ast.VariableStatement;
//import com.google.minijoe.compiler.ast.WithStatement;
//
//import java.util.Vector;

/**
 * Process function and variable declarations.
 * 
 * @author Andy Hayward
 */
public class DeclarationVisitor : TraversalVisitor {
  private ArrayList functionVector;
  private ArrayList variableVector;
  private bool hasWithStatement = false;
  private bool hasArgumentsVariable = false;
  private bool hasFunctionLiteral = false;

  public DeclarationVisitor() : 
    base() {
    visitor = this;
  }

  private void addVariable(Identifier identifier) {
	if (variableVector.IndexOf(identifier) == -1) {
	  identifier.index = variableVector.Count;
	  variableVector.Add(identifier);
    }
  }

  public override ProgramNode visit(ProgramNode program) {
	ArrayList oldFunctionVector = functionVector;

	functionVector = new ArrayList();

	program = base.visit(program);

    program.functions = CompilerUtil.vectorToStatementArray(functionVector);

    functionVector = oldFunctionVector;

    return program;
  }

  public override Expression visit(FunctionLiteral literal) {
	ArrayList oldFunctionVector = functionVector;
	ArrayList oldVariableVector = variableVector;
	bool oldHasWithStatement = hasWithStatement;
	bool oldHasArgumentsVariable = hasArgumentsVariable;

	functionVector = new ArrayList();
	variableVector = new ArrayList();
    hasWithStatement = false;
    hasArgumentsVariable = false;
    hasFunctionLiteral = false;

    Identifier[] parameters = literal.parameters;
	for (int i = 0; i < parameters.Length; i++) {
      addVariable(parameters[i]);
    }

	literal = (FunctionLiteral) base.visit(literal);

    literal.functions = CompilerUtil.vectorToStatementArray(functionVector);
    literal.variables = CompilerUtil.vectorToIdentifierArray(variableVector);

    // if this function literal:
    // * contains a function literal
    // * contains a 'with' statement
    // * contains a reference to 'arguments'
    //
    // then we need to disable the "access locals by index" optimisation for
    // this function literal.

    literal.enableLocalsOptimization =
        !(hasWithStatement | hasArgumentsVariable | hasFunctionLiteral);

    functionVector = oldFunctionVector;
    variableVector = oldVariableVector;
    hasWithStatement = oldHasWithStatement;
    hasArgumentsVariable = oldHasArgumentsVariable;
    hasFunctionLiteral = true;

    return literal;
  }

  public override Statement visit(FunctionDeclaration functionDeclaration) {
	functionDeclaration = (FunctionDeclaration) base.visit(functionDeclaration);

	functionVector.Add(new ExpressionStatement(functionDeclaration.literal));

    return new EmptyStatement();
  }

  public override Statement visit(WithStatement withStatement) {
	withStatement = (WithStatement) base.visit(withStatement);

    hasWithStatement = true;

    return withStatement;
  }

  public override Statement visit(VariableStatement variableStatement) {
	ArrayList statements = new ArrayList(0);

	for (int i = 0; i < variableStatement.declarations.Length; i++) {
      Expression expression = visitExpression(variableStatement.declarations[i]);

      if (expression != null) {
        Statement statement = new ExpressionStatement(expression);
        statement.setLineNumber(variableStatement.getLineNumber());
		statements.Add(statement);
      }
    }

	if (statements.Count == 0) {
      return new EmptyStatement();
    } else if (statements.Count == 1) {
	  return (ExpressionStatement) statements[0];
    } else {
      return new BlockStatement(CompilerUtil.vectorToStatementArray(statements));
    }
  }

  public override Expression visit(VariableExpression variableExpression) {
    Expression result = null;

	for (int i = 0; i < variableExpression.declarations.Length; i++) {
      Expression expression = visitExpression(variableExpression.declarations[i]);

      if (expression != null) {
        if (result == null) {
          result = expression;
        } else {
          result = new BinaryOperatorExpression(result, expression, Token.OPERATOR_COMMA);
        }
      }
    }

    return result;
  }

  public override Expression visit(VariableDeclaration declaration) {
    Identifier identifier = visitIdentifier(declaration.identifier);
    Expression initializer = visitExpression(declaration.initializer);
    Expression result = null;

    if (variableVector != null) {
      addVariable(identifier);
    }

    if (initializer != null) {
      result = new AssignmentExpression(identifier, initializer);
    } else {
      result = identifier;
    }

    return result;
  }

  public override Expression visit(Identifier identifier) {
	identifier = (Identifier) base.visit(identifier);

	if (identifier.str.Equals("arguments")) {
	  hasArgumentsVariable = true;
	}

	return identifier;
  }
}

}