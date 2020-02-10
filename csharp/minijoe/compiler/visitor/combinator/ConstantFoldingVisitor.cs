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
//import com.google.minijoe.compiler.ast.BinaryOperatorExpression;
//import com.google.minijoe.compiler.ast.Expression;
//import com.google.minijoe.compiler.ast.NumberLiteral;
//import com.google.minijoe.compiler.ast.UnaryOperatorExpression;

/**
 * @author Andy Hayward
 */
public class ConstantFoldingVisitor : IdentityVisitor {
  public override Expression visit(BinaryOperatorExpression boe) {
    boe.leftExpression = boe.leftExpression.visitExpression(this);
    boe.rightExpression = boe.rightExpression.visitExpression(this);

	if (boe.leftExpression is NumberLiteral 
		&& boe.rightExpression is NumberLiteral) {
      double left = ((NumberLiteral) boe.leftExpression).value;
      double right = ((NumberLiteral) boe.rightExpression).value;

      Token op = boe.operatorToken;

      if (op == Token.OPERATOR_PLUS) {
        return new NumberLiteral(left + right);
      } else if (op == Token.OPERATOR_MINUS) {
        return new NumberLiteral(left - right);
      } else if (op == Token.OPERATOR_MULTIPLY) {
        return new NumberLiteral(left * right);
      } else if (op == Token.OPERATOR_DIVIDE) {
        return new NumberLiteral(left / right);
      } else if (op == Token.OPERATOR_MODULO) {
        return new NumberLiteral(left % right);
      }
    }
    return boe;
  }

  public override Expression visit(UnaryOperatorExpression uoe) {

    uoe.subExpression = uoe.subExpression.visitExpression(this);

	if (uoe.subExpression is NumberLiteral) {
      double value = ((NumberLiteral) uoe.subExpression).value;
      Token op = uoe.operatorToken;

      if (op == Token.OPERATOR_MINUS) {
        return new NumberLiteral (-value);
      } else if (op == Token.OPERATOR_PLUS) {
        return new NumberLiteral(value);
      }
    }
    return uoe;
  }
}

}