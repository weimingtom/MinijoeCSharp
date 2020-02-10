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

//import com.google.minijoe.compiler.ast.Expression;
//import com.google.minijoe.compiler.ast.FunctionLiteral;
//import com.google.minijoe.compiler.ast.Identifier;
//import com.google.minijoe.compiler.ast.NumberLiteral;
//import com.google.minijoe.compiler.ast.Statement;
//import com.google.minijoe.compiler.ast.StringLiteral;
//import com.google.minijoe.compiler.ast.VariableDeclaration;

//import java.util.Vector;

/**
 * @author Andy Hayward
 */
public class CompilerUtil {
  /**
   * Prevent instantiation.
   */
  private CompilerUtil() {  
  }
  
  public static Statement[] vectorToStatementArray(ArrayList vector) {
    Statement[] statementArray = new Statement[vector.Count];

	vector.CopyTo(statementArray);

    return statementArray;
  }

  public static Expression[] vectorToExpressionArray(ArrayList vector) {
    Expression[] expressionArray = new Expression[vector.Count];

	vector.CopyTo(expressionArray);

    return expressionArray;
  }

  public static VariableDeclaration[] vectorToDeclarationArray(ArrayList vector) {
    VariableDeclaration[] declarationArray = new VariableDeclaration[vector.Count];

	vector.CopyTo(declarationArray);

    return declarationArray;
  }

  public static Identifier[] vectorToIdentifierArray(ArrayList vector) {
    Identifier[] identifierArray = new Identifier[vector.Count];

	vector.CopyTo(identifierArray);

    return identifierArray;
  }

  public static FunctionLiteral[] vectorToFunctionLiteralArray(ArrayList vector) {
	FunctionLiteral[] literalArray = new FunctionLiteral[vector.Count];

	vector.CopyTo(literalArray);

    return literalArray;
  }

  public static NumberLiteral[] vectorToNumberLiteralArray(ArrayList vector) {
	NumberLiteral[] literalArray = new NumberLiteral[vector.Count];

	vector.CopyTo(literalArray);

    return literalArray;
  }

  public static StringLiteral[] vectorToStringLiteralArray(ArrayList vector) {
	StringLiteral[] literalArray = new StringLiteral[vector.Count];

	vector.CopyTo(literalArray);

    return literalArray;
  }
}

}