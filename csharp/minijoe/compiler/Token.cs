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

/**
 * @author Andy Hayward
 */
public class Token {
  //
  // Types
  //

  public const int TYPE_INVALID = 0;

  public const int TYPE_NEWLINE = 1;
  public const int TYPE_MULTILINECOMMENT = 2;
  public const int TYPE_SINGLELINECOMMENT = 3;
  public const int TYPE_WHITESPACE = 4;

  public const int TYPE_KEYWORD = 5;
  public const int TYPE_OPERATOR = 6;

  public const int TYPE_IDENTIFIER = 7;
  public const int TYPE_STRING = 8;
  public const int TYPE_REGEX = 9;
  public const int TYPE_OCTAL = 10;
  public const int TYPE_DECIMAL = 11;
  public const int TYPE_HEXADECIMAL = 12;
  public const int TYPE_FLOAT = 13;

  public const int TYPE_EOF = 14;
  public const int TYPE_UNKNOWN = 15;

  //
  // Whitespace tokens
  //

  public static readonly Token NEWLINE = new Token(TYPE_NEWLINE);
  public static readonly Token MULTILINECOMMENT = new Token(TYPE_MULTILINECOMMENT);
  public static readonly Token SINGLELINECOMMENT = new Token(TYPE_SINGLELINECOMMENT);
  public static readonly Token WHITESPACE = new Token(TYPE_WHITESPACE);

  //
  // Keyword tokens
  //

  public static readonly Token KEYWORD_BREAK = new Token(TYPE_KEYWORD, "break");
  public static readonly Token KEYWORD_CASE = new Token(TYPE_KEYWORD, "case");
  public static readonly Token KEYWORD_CATCH = new Token(TYPE_KEYWORD, "catch");
  public static readonly Token KEYWORD_CONTINUE = new Token(TYPE_KEYWORD, "continue");
  public static readonly Token KEYWORD_DEFAULT = new Token(TYPE_KEYWORD, "default");
  public static readonly Token KEYWORD_DELETE = new Token(TYPE_KEYWORD, "delete");
  public static readonly Token KEYWORD_DO = new Token(TYPE_KEYWORD, "do");
  public static readonly Token KEYWORD_ELSE = new Token(TYPE_KEYWORD, "else");
  public static readonly Token KEYWORD_FALSE = new Token(TYPE_KEYWORD, "false");
  public static readonly Token KEYWORD_FINALLY = new Token(TYPE_KEYWORD, "finally");
  public static readonly Token KEYWORD_FOR = new Token(TYPE_KEYWORD, "for");
  public static readonly Token KEYWORD_FUNCTION = new Token(TYPE_KEYWORD, "function");
  public static readonly Token KEYWORD_IF = new Token(TYPE_KEYWORD, "if");
  public static readonly Token KEYWORD_IN = new Token(TYPE_KEYWORD, "in");
  public static readonly Token KEYWORD_INSTANCEOF = new Token(TYPE_KEYWORD, "instanceof");
  public static readonly Token KEYWORD_NEW = new Token(TYPE_KEYWORD, "new");
  public static readonly Token KEYWORD_NULL = new Token(TYPE_KEYWORD, "null");
  public static readonly Token KEYWORD_RETURN = new Token(TYPE_KEYWORD, "return");
  public static readonly Token KEYWORD_SWITCH = new Token(TYPE_KEYWORD, "switch");
  public static readonly Token KEYWORD_THIS = new Token(TYPE_KEYWORD, "this");
  public static readonly Token KEYWORD_THROW = new Token(TYPE_KEYWORD, "throw");
  public static readonly Token KEYWORD_TRUE = new Token(TYPE_KEYWORD, "true");
  public static readonly Token KEYWORD_TRY = new Token(TYPE_KEYWORD, "try");
  public static readonly Token KEYWORD_TYPEOF = new Token(TYPE_KEYWORD, "typeof");
  public static readonly Token KEYWORD_VAR = new Token(TYPE_KEYWORD, "var");
  public static readonly Token KEYWORD_VOID = new Token(TYPE_KEYWORD, "void");
  public static readonly Token KEYWORD_WHILE = new Token(TYPE_KEYWORD, "while");
  public static readonly Token KEYWORD_WITH = new Token(TYPE_KEYWORD, "with");

  //
  // Reserved keyword tokens
  //

  public static readonly Token KEYWORD_ABSTRACT = new Token(TYPE_KEYWORD, "abstract");
  public static readonly Token KEYWORD_BOOLEAN = new Token(TYPE_KEYWORD, "boolean");
  public static readonly Token KEYWORD_BYTE = new Token(TYPE_KEYWORD, "byte");
  public static readonly Token KEYWORD_CHAR = new Token(TYPE_KEYWORD, "char");
  public static readonly Token KEYWORD_CLASS = new Token(TYPE_KEYWORD, "class");
  public static readonly Token KEYWORD_CONST = new Token(TYPE_KEYWORD, "const");
  public static readonly Token KEYWORD_DEBUGGER = new Token(TYPE_KEYWORD, "debugger");
  public static readonly Token KEYWORD_DOUBLE = new Token(TYPE_KEYWORD, "double");
  public static readonly Token KEYWORD_ENUM = new Token(TYPE_KEYWORD, "enum");
  public static readonly Token KEYWORD_EXPORT = new Token(TYPE_KEYWORD, "export");
  public static readonly Token KEYWORD_EXTENDS = new Token(TYPE_KEYWORD, "extends");
  public static readonly Token KEYWORD_FINAL = new Token(TYPE_KEYWORD, "final");
  public static readonly Token KEYWORD_FLOAT = new Token(TYPE_KEYWORD, "float");
  public static readonly Token KEYWORD_GOTO = new Token(TYPE_KEYWORD, "goto");
  public static readonly Token KEYWORD_IMPLEMENTS = new Token(TYPE_KEYWORD, "implements");
  public static readonly Token KEYWORD_IMPORT = new Token(TYPE_KEYWORD, "import");
  public static readonly Token KEYWORD_INT = new Token(TYPE_KEYWORD, "int");
  public static readonly Token KEYWORD_INTERFACE = new Token(TYPE_KEYWORD, "interface");
  public static readonly Token KEYWORD_LONG = new Token(TYPE_KEYWORD, "long");
  public static readonly Token KEYWORD_NATIVE = new Token(TYPE_KEYWORD, "native");
  public static readonly Token KEYWORD_PACKAGE = new Token(TYPE_KEYWORD, "package");
  public static readonly Token KEYWORD_PRIVATE = new Token(TYPE_KEYWORD, "private");
  public static readonly Token KEYWORD_PROTECTED = new Token(TYPE_KEYWORD, "protected");
  public static readonly Token KEYWORD_PUBLIC = new Token(TYPE_KEYWORD, "public");
  public static readonly Token KEYWORD_SHORT = new Token(TYPE_KEYWORD, "short");
  public static readonly Token KEYWORD_STATIC = new Token(TYPE_KEYWORD, "static");
  public static readonly Token KEYWORD_SUPER = new Token(TYPE_KEYWORD, "super");
  public static readonly Token KEYWORD_SYNCHRONIZED = new Token(TYPE_KEYWORD, "synchronized");
  public static readonly Token KEYWORD_THROWS = new Token(TYPE_KEYWORD, "throws");
  public static readonly Token KEYWORD_TRANSIENT = new Token(TYPE_KEYWORD, "transient");
  public static readonly Token KEYWORD_VOLATILE = new Token(TYPE_KEYWORD, "volatile");

  //
  // Operator tokens
  //

  public static readonly Token OPERATOR_ASSIGNMENT = new Token(TYPE_OPERATOR, "=");
  public static readonly Token OPERATOR_BITWISEAND = new Token(TYPE_OPERATOR, "&");
  public static readonly Token OPERATOR_BITWISEANDASSIGNMENT = new Token(TYPE_OPERATOR, "&=");
  public static readonly Token OPERATOR_BITWISENOT = new Token(TYPE_OPERATOR, "~");
  public static readonly Token OPERATOR_BITWISEOR = new Token(TYPE_OPERATOR, "|");
  public static readonly Token OPERATOR_BITWISEORASSIGNMENT = new Token(TYPE_OPERATOR, "|=");
  public static readonly Token OPERATOR_BITWISEXOR = new Token(TYPE_OPERATOR, "^");
  public static readonly Token OPERATOR_BITWISEXORASSIGNMENT = new Token(TYPE_OPERATOR, "^=");
  public static readonly Token OPERATOR_CLOSEBRACE = new Token(TYPE_OPERATOR, "}");
  public static readonly Token OPERATOR_CLOSEPAREN = new Token(TYPE_OPERATOR, ")");
  public static readonly Token OPERATOR_CLOSESQUARE = new Token(TYPE_OPERATOR, "]");
  public static readonly Token OPERATOR_COLON = new Token(TYPE_OPERATOR, ":");
  public static readonly Token OPERATOR_COMMA = new Token(TYPE_OPERATOR, ",");
  public static readonly Token OPERATOR_CONDITIONAL = new Token(TYPE_OPERATOR, "?");
  public static readonly Token OPERATOR_DIVIDE = new Token(TYPE_OPERATOR, "/");
  public static readonly Token OPERATOR_DIVIDEASSIGNMENT = new Token(TYPE_OPERATOR, "/=");
  public static readonly Token OPERATOR_DOT = new Token(TYPE_OPERATOR, ".");
  public static readonly Token OPERATOR_EQUALEQUAL = new Token(TYPE_OPERATOR, "==");
  public static readonly Token OPERATOR_EQUALEQUALEQUAL = new Token(TYPE_OPERATOR, "===");
  public static readonly Token OPERATOR_GREATERTHAN = new Token(TYPE_OPERATOR, ">");
  public static readonly Token OPERATOR_GREATERTHANOREQUAL = new Token(TYPE_OPERATOR, ">=");
  public static readonly Token OPERATOR_LESSTHAN = new Token(TYPE_OPERATOR, "<");
  public static readonly Token OPERATOR_LESSTHANOREQUAL = new Token(TYPE_OPERATOR, "<=");
  public static readonly Token OPERATOR_LOGICALAND = new Token(TYPE_OPERATOR, "&&");
  public static readonly Token OPERATOR_LOGICALNOT = new Token(TYPE_OPERATOR, "!");
  public static readonly Token OPERATOR_LOGICALOR = new Token(TYPE_OPERATOR, "||");
  public static readonly Token OPERATOR_MINUS = new Token(TYPE_OPERATOR, "-");
  public static readonly Token OPERATOR_MINUSASSIGNMENT = new Token(TYPE_OPERATOR, "-=");
  public static readonly Token OPERATOR_MINUSMINUS = new Token(TYPE_OPERATOR, "--");
  public static readonly Token OPERATOR_MODULO = new Token(TYPE_OPERATOR, "%");
  public static readonly Token OPERATOR_MODULOASSIGNMENT = new Token(TYPE_OPERATOR, "%=");
  public static readonly Token OPERATOR_MULTIPLY = new Token(TYPE_OPERATOR, "*");
  public static readonly Token OPERATOR_MULTIPLYASSIGNMENT = new Token(TYPE_OPERATOR, "*=");
  public static readonly Token OPERATOR_NOTEQUAL = new Token(TYPE_OPERATOR, "!=");
  public static readonly Token OPERATOR_NOTEQUALEQUAL = new Token(TYPE_OPERATOR, "!==");
  public static readonly Token OPERATOR_OPENBRACE = new Token(TYPE_OPERATOR, "{");
  public static readonly Token OPERATOR_OPENPAREN = new Token(TYPE_OPERATOR, "(");
  public static readonly Token OPERATOR_OPENSQUARE = new Token(TYPE_OPERATOR, "[");
  public static readonly Token OPERATOR_PLUS = new Token(TYPE_OPERATOR, "+");
  public static readonly Token OPERATOR_PLUSASSIGNMENT = new Token(TYPE_OPERATOR, "+=");
  public static readonly Token OPERATOR_PLUSPLUS = new Token(TYPE_OPERATOR, "++");
  public static readonly Token OPERATOR_SEMICOLON = new Token(TYPE_OPERATOR, ";");
  public static readonly Token OPERATOR_SHIFTLEFT = new Token(TYPE_OPERATOR, "<<");
  public static readonly Token OPERATOR_SHIFTLEFTASSIGNMENT = new Token(TYPE_OPERATOR, "<<=");
  public static readonly Token OPERATOR_SHIFTRIGHT = new Token(TYPE_OPERATOR, ">>");
  public static readonly Token OPERATOR_SHIFTRIGHTASSIGNMENT = new Token(TYPE_OPERATOR, ">>=");
  public static readonly Token OPERATOR_SHIFTRIGHTUNSIGNED = new Token(TYPE_OPERATOR, ">>>");
  public static readonly Token OPERATOR_SHIFTRIGHTUNSIGNEDASSIGNMENT = new Token(TYPE_OPERATOR, ">>>=");

  //
  // Other tokens
  //

  public static readonly Token EOF = new Token(TYPE_EOF);

  //
  // Internal state
  //

  private int type;
  private String value;

  /**
   * Public constructor.
   */
  public Token(int type) : 
    this(type, null) {
  }

  /**
   * Public constructor.
   */
  public Token(int type, String value) {
    this.type = type;
    this.value = value;
  }

  /**
   * Implementation of equals().
   */
  public override bool Equals(object obj) {
	if (this.GetType() == obj.GetType()) {
      Token token = (Token) obj;

      switch (type) {
        case TYPE_FLOAT:
        case TYPE_OCTAL:
        case TYPE_DECIMAL:
        case TYPE_HEXADECIMAL:
        case TYPE_REGEX:
        case TYPE_STRING:
        case TYPE_IDENTIFIER:
          // these token types are equal iff their types and values are equal
		  return (this.type == token.type && this.value.Equals(token.value));

        default:
          // otherwise tokens are equal only iff they're the the same token
          return (this == token);
      }
    }

    return false;
  }

  /**
   * Returns the type of this token.
   */
  public int getType() {
    return type;
  }

  /**
   * Returns the value of this token as a string.
   */
  public String getValue() {
    return value;
  }

  /**
   * Implementation of hashcode.
   */
  public override int GetHashCode() {
	return type ^ value.GetHashCode();
  }

  /**
   * Return true is this token represents whitespace.
   */
  public bool isWhitespace() {
    return type == TYPE_NEWLINE
        || type == TYPE_MULTILINECOMMENT
        || type == TYPE_SINGLELINECOMMENT
        || type == TYPE_WHITESPACE;
  }

  /**
   * Return true is this token represents whitespace except newlines.
   */
  public bool isWhitespaceNotNewline() {
    return type == TYPE_MULTILINECOMMENT
        || type == TYPE_SINGLELINECOMMENT
        || type == TYPE_WHITESPACE;
  }

  /**
   * Return true is this token represents the EOF.
   */
  public bool isEOF() {
    return type == TYPE_EOF;
  }

  /**
   * Return true is this token represents an identifier.
   */
  public bool isIdentifier() {
    return type == TYPE_IDENTIFIER;
  }

  /**
   * Return true is this token represents a newline.
   */
  public bool isNewLine() {
    return type == TYPE_NEWLINE;
  }

  /**
   * Return true is this token represents a numeric literal.
   */
  public bool isNumericLiteral() {
    return type == TYPE_FLOAT
        || type == TYPE_OCTAL
        || type == TYPE_DECIMAL
        || type == TYPE_HEXADECIMAL;
  }

  /**
   * Return true is this token represents a regex literal.
   */
  public bool isRegexLiteral() {
    return type == TYPE_REGEX;
  }

  /**
   * Return true is this token represents a string literal.
   */
  public bool isStringLiteral() {
    return type == TYPE_STRING;
  }

  /**
   * Returns a string representation of this token.
   */
  public sealed override String ToString() {
    switch (type) {
      case TYPE_NEWLINE:
        return "NEWLINE";

      case TYPE_MULTILINECOMMENT:
        return "MULTILINECOMMENT:";

      case TYPE_SINGLELINECOMMENT:
        return "SINGLELINECOMMENT:";

      case TYPE_WHITESPACE:
        return "WHITESPACE:";

      case TYPE_KEYWORD:
        return "KEYWORD: " + value;

      case TYPE_OPERATOR:
        return "OPERATOR: " + value;

      case TYPE_OCTAL:
        return "OCTAL: " + value;

      case TYPE_FLOAT:
        return "FLOAT: " + value;

      case TYPE_DECIMAL:
        return "DECIMAL: " + value;

      case TYPE_HEXADECIMAL:
        return "HEXADECIMAL: " + value;

      case TYPE_REGEX:
        return "REGEX: " + value;

      case TYPE_STRING:
        return "STRING: " + value;

      case TYPE_IDENTIFIER:
        return "IDENTIFIER: " + value;

      case TYPE_EOF:
        return "EOF";

      case TYPE_UNKNOWN:
        return "UNKNOWN: " + value;

      default:
	    throw new ArgumentException();
    }
  }
}

}