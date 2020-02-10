using System;
using System.Collections;
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
//import com.google.minijoe.compiler.Config;
//import com.google.minijoe.compiler.Token;
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
//import com.google.minijoe.compiler.ast.Node;
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
//import com.google.minijoe.sys.JsFunction;
//
//import java.io.ByteArrayOutputStream;
//import java.io.DataOutputStream;
//import java.io.IOException;
//import java.util.Hashtable;
//import java.util.Vector;

/**
 * Code generation visitor. This needs two passes, the first pass is used to
 * determine the offsets of jump locations, the second pass writes the actual
 * byte code.
 *
 * @author Andy Hayward
 * @author Stefan Haustein
 */
public class CodeGenerationVisitor : Visitor {
  public static readonly sbyte BLOCK_COMMENT = (sbyte) 0x00;
  public static readonly sbyte BLOCK_GLOBAL_STRING_TABLE = (sbyte) 0x10;
  public static readonly sbyte BLOCK_NUMBER_LITERALS = (sbyte) 0x20;
  public static readonly sbyte BLOCK_STRING_LITERALS = (sbyte) 0x30;
  public static readonly sbyte BLOCK_REGEX_LITERALS = (sbyte) 0x40;
  public static readonly sbyte BLOCK_FUNCTION_LITERALS = (sbyte) 0x50;
  public static readonly sbyte BLOCK_LOCAL_VARIABLE_NAMES = (sbyte) 0x60;
  public static readonly sbyte BLOCK_CODE = unchecked((sbyte) 0x80);
  public static readonly sbyte BLOCK_LINENUMBER = unchecked((sbyte) 0xE0);
  public static readonly sbyte BLOCK_DEBUG = unchecked((sbyte) 0xF0);
  public static readonly sbyte BLOCK_END = unchecked((sbyte) 0xFF);

  // pop the previous value of the assignment target. should be optimized away
  // in most cases.

  private DataOutputStream dos;
  private ByteArrayOutputStream codeStream = new ByteArrayOutputStream(0);

  private Hashtable globalStringMap = new Hashtable();
  private ArrayList globalStringTable = new ArrayList();

  private ArrayList functionLiterals = new ArrayList();
  private ArrayList numberLiterals = new ArrayList();
  private ArrayList stringLiterals = new ArrayList();

  private Hashtable localVariableTable = new Hashtable();

  private Hashtable jumpLabels = new Hashtable();
  private ArrayList unresolvedJumps = new ArrayList();
  private ArrayList labelSet = new ArrayList();

  private ArrayList lineNumberVector = new ArrayList();

  // TODO consider getting rid of this

  private Expression pendingAssignment;

  private Statement currentBreakStatement;
  private Statement currentContinueStatement;
  private Statement currentTryStatement;
  private String currentTryLabel;
  
  private bool enableLocalsOptimization = false;

  internal CodeGenerationVisitor parent;

  private class LineNumber {
	internal LineNumber(int programCounter, int lineNumber) {
      this.programCounter = programCounter;
      this.lineNumber = lineNumber;
    }

	internal int programCounter;
	internal int lineNumber;
  }

  public CodeGenerationVisitor(DataOutputStream stream) {
    this.dos = stream;
    this.globalStringMap = new Hashtable();
	this.globalStringTable = new ArrayList();
  }

  public CodeGenerationVisitor( CodeGenerationVisitor parent, FunctionLiteral function,
      DataOutputStream dos) {
    this.parent = parent;
    this.globalStringMap = parent.globalStringMap;
    this.globalStringTable = parent.globalStringTable;
    this.dos = dos;
    this.enableLocalsOptimization = function.enableLocalsOptimization;

	for (int i = 0; i < function.variables.Length; i++) {
      Identifier variable = function.variables[i];
      addToGlobalStringTable(variable.str);
	  localVariableTable[variable] = variable;
    }

	for (int i = 0; i < function.variables.Length; i++) {
      addToGlobalStringTable(function.variables[i].str);
    }

	for (int i = 0; i < function.functions.Length; i++) {
      if (function.functions[i] != null) {
        function.functions[i].visitStatement(this);
      }
    }

	for (int i = 0; i < function.statements.Length; i++) {
      if (function.statements[i] != null) {
        function.statements[i].visitStatement(this);
      }
    }

	sbyte[] byteCode = codeStream.toByteArray();

    // TODO remove this magic numbers.
    int flags = Config.FASTLOCALS && function.enableLocalsOptimization ? 0x01 : 0x00;

    if (function.name != null) {
      writeCommentBlock("function " + function.name.str);
    }

    writeStringLiteralBlock();
    writeNumberLiteralBlock();
    writeFunctionLiteralBlock();
    writeLocalVariableNameBlock(function.variables);
	writeCodeBlock(function.variables.Length, function.parameters.Length, flags, byteCode);
    writeLineNumberBlock();
    writeEndMarker();
  }

  //
  // utility methods
  //

  private void writeMagic() {
    try {
      dos.write('M');
      dos.write('i');
      dos.write('n');
      dos.write('i');
      dos.write('J');
      dos.write('o');
      dos.write('e');
      dos.write(0x00); // version
    } catch (IOException e) {
      throw new CompilerException(e);
    }
  }

  private void writeCommentBlock(string comment) {
    try {
      if (comment != null) {
        dos.write(BLOCK_COMMENT);
        dos.writeUTF(comment);
      }
    } catch (IOException e) {
      throw new CompilerException(e);
    }
  }

  private void writeGlobalStringTableBlock() {
    try {
	  if (globalStringTable.Count > 0) {
        dos.write(BLOCK_GLOBAL_STRING_TABLE);
		dos.writeShort(globalStringTable.Count);
		for (int i = 0; i < globalStringTable.Count; i++) {
		  dos.writeUTF((String) globalStringTable[i]);
        }
      }
    } catch (IOException e) {
      throw new CompilerException(e);
    }
  }

  private void writeNumberLiteralBlock() {
    try {
	  if (numberLiterals.Count > 0) {
        dos.write(BLOCK_NUMBER_LITERALS);
		dos.writeShort(numberLiterals.Count);
		for (int i = 0; i < numberLiterals.Count; i++) {
		  dos.writeDouble((double)((Double) numberLiterals[i]));
        }
      }
    } catch (IOException e) {
      throw new CompilerException(e);
    }
  }

  private void writeStringLiteralBlock() {
    try {
	  if (stringLiterals.Count > 0) {
        dos.write(BLOCK_STRING_LITERALS);
		dos.writeShort(stringLiterals.Count);
		for (int i = 0; i < stringLiterals.Count; i++) {
		  dos.writeShort((int)(short)((Int32) globalStringMap[stringLiterals
		       [i]]));
        }
      }
    } catch (IOException e) {
      throw new CompilerException(e);
    }
  }

  private void writeLocalVariableNameBlock(Identifier[] variables) {
    try {
      if (variables != null) {
        dos.write(BLOCK_LOCAL_VARIABLE_NAMES);
		dos.writeShort(variables.Length);
		for (int i = 0; i < variables.Length; i++) {
		  dos.writeShort((int)(short)((Int32) globalStringMap[variables[i].str]));
        }
      }
    } catch (IOException e) {
      throw new CompilerException(e);
    }
  }

  private void writeFunctionLiteralBlock() {
    try {
	  if (functionLiterals.Count > 0) {
        dos.write(BLOCK_FUNCTION_LITERALS);
		dos.writeShort(functionLiterals.Count);
		for (int i = 0; i < functionLiterals.Count; i++) {
		  dos.write((sbyte[]) functionLiterals[i]);
        }
      }
    } catch (IOException e) {
      throw new CompilerException(e);
    }
  }

  private void writeCodeBlock(int localVariableCount, int paramenterCount,
    int flags, sbyte[] code) {
    try {
      dos.write(BLOCK_CODE);
      dos.writeShort(localVariableCount);
      dos.writeShort(paramenterCount);
      dos.write(flags);
	  dos.writeShort(code.Length);

	  for (int i = 0; i < unresolvedJumps.Count; i += 2) {
		String label = (String) unresolvedJumps[i];
		int address = (int)((Int32) unresolvedJumps[i + 1]);
		Int32 target = (Int32) jumpLabels[label];

        if (target == null) {
          throw new CompilerException("Unresolved Jump Label: " + label);
        }

		int delta = (int)target - address - 2;

		code[address + 0] = (sbyte)(delta >> 8);
		code[address + 1] = (sbyte)(delta & 255);
      }

      dos.write(code);
    } catch (IOException e) {
      throw new CompilerException(e);
    }
  }

  private void writeLineNumberBlock() {
    try {
      dos.write(BLOCK_LINENUMBER);
	  int lineNumberCount = lineNumberVector.Count;
	  dos.writeShort(lineNumberVector.Count);
      for (int i = 0; i < lineNumberCount; i++) {
		LineNumber lineNumber = (LineNumber) lineNumberVector[i];
        dos.writeShort(lineNumber.programCounter & 0xffff);
        dos.writeShort(lineNumber.lineNumber & 0xffff);
      }
    } catch (IOException e) {
      throw new CompilerException(e);
    }
  }

  private void writeEndMarker() {
    try {
      dos.write(BLOCK_END);
    } catch (IOException e) {
      throw new CompilerException(e);
    }
  }

  private void writeOp(int op) {
    codeStream.write(op);
  }

  private void writeOpGet(Identifier identifier) {
    int index = identifier.index;

    if (Config.FASTLOCALS && enableLocalsOptimization &&  index >= 0) {
      writeXop(JsFunction.XOP_LCL_GET, index);
    } else {
      writeXop(JsFunction.XOP_PUSH_STR, getStringLiteralIndex(identifier.str));
      writeOp(JsFunction.OP_CTX_GET);
    }
  }

  private void writeOpSet(Identifier identifier) {
    int index = identifier.index;

    if (Config.FASTLOCALS && enableLocalsOptimization &&  index >= 0) {
      writeXop(JsFunction.XOP_LCL_SET, index);
    } else {
      writeXop(JsFunction.XOP_PUSH_STR, getStringLiteralIndex(identifier.str));
      writeOp(JsFunction.OP_CTX_SET);
    }
  }

  internal virtual void visitWithNewLabelSet(Node node) {
	ArrayList saveLabelSet = labelSet;
	labelSet = new ArrayList();
	if (node is Statement) {
      ((Statement) node).visitStatement(this);
	} else if (node is Expression) {
      ((Expression) node).visitExpression(this);
    }
    labelSet = saveLabelSet;
  }

  /**
   * Write a variable-length operation with an immediate parameter to the given
   * output stream.
   */
  internal virtual void writeXop(int opcode, int param) {
    if (opcode == JsFunction.XOP_ADD) {
      switch (param) {
        case 1:
          writeOp(JsFunction.OP_INC);
          return;
        case -1:
          writeOp(JsFunction.OP_DEC);
          return;
      }
    }

    if ((param & 0x0ff80) == 0 || (param & 0x0ff80) == 0xff80) {
      codeStream.write(opcode << 1);
      codeStream.write(param);
    } else {
      codeStream.write((opcode << 1) | 1);
      codeStream.write(param >> 8);
      codeStream.write(param & 255);
    }
  }

  internal virtual void writeJump(int op, Object baseObj, String type) {
    int pos = codeStream.size() + 1;

	if (baseObj is String) {
      type = type + "-" + baseObj;
	} else if (baseObj is Node) {
	  type = type + "=" + baseObj.GetHashCode();
    } else if (baseObj == null) {
	  throw new Exception("Invalid position for " + type);
    } else {
	  throw new Exception("Illegal Jump base object");
    }

	Int32 target = (Int32) jumpLabels[type];
	if (jumpLabels[type] == null) {
      writeXop(op, 32767);
	  unresolvedJumps.Add(type);
	  unresolvedJumps.Add(new int?(pos));
    } else {
      // minus one for pc after decoding 8 bit imm
	  int delta = (int)target - pos - 1;

      if (delta > -127 && delta < 128) {
        codeStream.write(op << 1);
        codeStream.write(delta);
      } else {
        // minus one more for pc after decoding 16 bit imm
        codeStream.write((op << 1) | 1);
        // minus one more for pc after decoding 16 bit imm
        delta -= 1;
        codeStream.write(delta >> 8);
        codeStream.write(delta & 255);
      }
    }
  }

  internal virtual void setLabel(Node node, String label) {
	Int32 pos = (Int32)(codeStream.size());
	jumpLabels[label + "=" + node.GetHashCode()] = pos;
	for (int i = 0; i < labelSet.Count; i++) {
	  jumpLabels[label + "-" + labelSet[i]] = pos;
	}
  }

  private void writeBinaryOperator(Token type) {
    if (type == Token.OPERATOR_ASSIGNMENT) {
      // should be handled as special case
      writeOp(JsFunction.OP_DROP);
    } else if (type == Token.OPERATOR_BITWISEAND
        || type == Token.OPERATOR_BITWISEANDASSIGNMENT) {
      writeOp(JsFunction.OP_AND);
    } else if (type == Token.OPERATOR_BITWISEOR
        || type == Token.OPERATOR_BITWISEORASSIGNMENT) {
      writeOp(JsFunction.OP_OR);
    } else if (type == Token.OPERATOR_BITWISEXOR
        || type == Token.OPERATOR_BITWISEXORASSIGNMENT) {
      writeOp(JsFunction.OP_XOR);
    } else if (type == Token.OPERATOR_COMMA) {
      // should be handled as special case in caller to avoid swap
      writeOp(JsFunction.OP_SWAP);
      writeOp(JsFunction.OP_DROP);
    } else if (type == Token.OPERATOR_DIVIDE
        || type == Token.OPERATOR_DIVIDEASSIGNMENT) {
      writeOp(JsFunction.OP_DIV);
    } else if (type == Token.OPERATOR_EQUALEQUAL) {
      writeOp(JsFunction.OP_EQEQ);
    } else if (type == Token.OPERATOR_EQUALEQUALEQUAL) {
      writeOp(JsFunction.OP_EQEQEQ);
    } else if (type == Token.OPERATOR_GREATERTHAN) {
      writeOp(JsFunction.OP_GT);
    } else if (type == Token.OPERATOR_GREATERTHANOREQUAL) {
      writeOp(JsFunction.OP_LT);
      writeOp(JsFunction.OP_NOT);
    } else if (type == Token.OPERATOR_LESSTHAN) {
      writeOp(JsFunction.OP_LT);
    } else if (type == Token.OPERATOR_LESSTHANOREQUAL) {
      writeOp(JsFunction.OP_GT);
      writeOp(JsFunction.OP_NOT);
    } else if (type == Token.OPERATOR_MINUS
        || type == Token.OPERATOR_MINUSASSIGNMENT) {
      writeOp(JsFunction.OP_SUB);
    } else if (type == Token.OPERATOR_MODULO
        || type == Token.OPERATOR_MODULOASSIGNMENT) {
      writeOp(JsFunction.OP_MOD);
    } else if (type == Token.OPERATOR_MULTIPLY
        || type == Token.OPERATOR_MULTIPLYASSIGNMENT) {
      writeOp(JsFunction.OP_MUL);
    } else if (type == Token.OPERATOR_NOTEQUAL) {
      writeOp(JsFunction.OP_EQEQ);
      writeOp(JsFunction.OP_NOT);
    } else if (type == Token.OPERATOR_NOTEQUALEQUAL) {
      writeOp(JsFunction.OP_EQEQEQ);
      writeOp(JsFunction.OP_NOT);
    } else if (type == Token.OPERATOR_PLUS
        || type == Token.OPERATOR_PLUSASSIGNMENT) {
      writeOp(JsFunction.OP_ADD);
    } else if (type == Token.OPERATOR_SHIFTLEFT
        || type == Token.OPERATOR_SHIFTLEFTASSIGNMENT) {
      writeOp(JsFunction.OP_SHL);
    } else if (type == Token.OPERATOR_SHIFTRIGHT
        || type == Token.OPERATOR_SHIFTRIGHTASSIGNMENT) {
      writeOp(JsFunction.OP_SHR);
    } else if (type == Token.OPERATOR_SHIFTRIGHTUNSIGNED
        || type == Token.OPERATOR_SHIFTRIGHTUNSIGNEDASSIGNMENT) {
      writeOp(JsFunction.OP_ASR);
    } else if (type == Token.KEYWORD_IN) {
      writeOp(JsFunction.OP_IN);
    } else if (type == Token.KEYWORD_INSTANCEOF) {
      writeOp(JsFunction.OP_INSTANCEOF);
    } else {
	  throw new ArgumentException("Not binary: " + type.ToString());
    }
  }

  private void writeUnaryOperator(Token type) {
    if (type == Token.OPERATOR_PLUS) {
      writeXop(JsFunction.XOP_ADD, 0);
    } else if (type == Token.OPERATOR_MINUS) {
      writeOp(JsFunction.OP_NEG);
    } else if (type == Token.OPERATOR_BITWISENOT) {
      writeOp(JsFunction.OP_INV);
    } else if (type == Token.OPERATOR_LOGICALNOT) {
      writeOp(JsFunction.OP_NOT);
    } else if (type == Token.KEYWORD_VOID) {
      writeOp(JsFunction.OP_DROP);
      writeOp(JsFunction.OP_PUSH_UNDEF);
    } else if (type == Token.KEYWORD_TYPEOF) {
      writeOp(JsFunction.OP_TYPEOF);
    } else {
	  throw new ArgumentException("Not unary: " + type.ToString());
    }
  }

  /** value must be on stack, is kept on stack */
  private void writeVarDef(String name, bool initialize) {
    if (initialize) {
      writeXop(JsFunction.XOP_PUSH_STR, getStringLiteralIndex(name));
      writeOp(JsFunction.OP_CTX_SET);
    }
  }

  private void addToGlobalStringTable(String s) {
	if (globalStringMap[s] == null) {
	  globalStringMap[s] = new int?(globalStringTable.Count);
	  globalStringTable.Add(s);
	}
  }

  private int getStringLiteralIndex(string @string) {
	int i = stringLiterals.IndexOf(@string);
    if (i == -1) {
	  i = stringLiterals.Count;
	  addToGlobalStringTable(@string);
	  stringLiterals.Add(@string);
    }
    return i;
  }

  //
  // nodes
  //

  public virtual ProgramNode visit(ProgramNode program) {
	for (int i = 0; i < program.functions.Length; i++) {
      program.functions[i].visitStatement(this);
    }

	for (int i = 0; i < program.statements.Length; i++) {
      program.statements[i].visitStatement(this);
    }

    writeMagic();
    writeGlobalStringTableBlock();
    writeStringLiteralBlock();
    writeNumberLiteralBlock();
    writeFunctionLiteralBlock();
    writeCodeBlock(0, 0, 0x00, codeStream.toByteArray());
    writeLineNumberBlock();
    writeEndMarker();

    return program;
  }

  //
  // statements
  //

  private void addLineNumber(Statement statement) {
    if (Config.LINENUMBER) {
		lineNumberVector.Add(new LineNumber(codeStream.size(), statement.getLineNumber()));
    }
  }

  public virtual Statement visit(FunctionDeclaration statement) {
    addLineNumber(statement);

    statement.literal.visitExpression(this);
    writeOp(JsFunction.OP_DROP);
    return statement;
  }

  public virtual Statement visit(BlockStatement statement) {
	for (int i = 0; i < statement.statements.Length; i++) {
      statement.statements[i].visitStatement(this);
    }
    return statement;
  }

  public virtual Statement visit(BreakStatement statement) {
    addLineNumber(statement);

    writeJump(JsFunction.XOP_GO, statement.identifier == null
        ? (Object) currentBreakStatement : statement.identifier.str, "break");
    return statement;
  }

  public virtual Statement visit(CaseStatement statement) {
	throw new Exception("should not be visited");
  }

  public virtual Statement visit(ContinueStatement statement) {
    addLineNumber(statement);

    writeJump(JsFunction.XOP_GO,
        statement.identifier == null
        ? (Object) currentBreakStatement
            : statement.identifier.str,
    "continue");
    return statement;
  }

  public virtual Statement visit(DoStatement statement) {
    addLineNumber(statement);

    Statement saveBreakStatement = currentBreakStatement;
    Statement saveContinueStatement = currentContinueStatement;
    currentBreakStatement = statement;
    currentContinueStatement = statement;

    setLabel(statement, "do");

    visitWithNewLabelSet(statement.statement);

    setLabel(statement, "continue");
    visitWithNewLabelSet(statement.expression);
    writeOp(JsFunction.OP_NOT);
    writeJump(JsFunction.XOP_IF, statement, "do");
    setLabel(statement, "break");

    currentBreakStatement = saveBreakStatement;
    currentContinueStatement = saveContinueStatement;
    return statement;
  }

  public virtual Statement visit(EmptyStatement statement) {
    return statement;
  }

  public virtual Statement visit(ExpressionStatement statement) {
    addLineNumber(statement);

    statement.expression.visitExpression(this);
    writeOp(JsFunction.OP_DROP);
    return statement;
  }

  public virtual Statement visit(ForInStatement statement) {
    addLineNumber(statement);

    Statement saveBreakStatement = currentBreakStatement;
    Statement saveContinueStatement = currentContinueStatement;

    currentBreakStatement = statement;
    currentContinueStatement = statement;

    statement.expression.visitExpression(this);
    writeOp(JsFunction.OP_ENUM);
    setLabel(statement, "continue");
    writeJump(JsFunction.XOP_NEXT, statement, "break");

	if (statement.variable is Identifier) {
      writeXop(JsFunction.XOP_PUSH_STR,
               getStringLiteralIndex(((Identifier) statement.variable).str));
      writeOp(JsFunction.OP_CTX_SET);
	} else if (statement.variable is VariableDeclaration) {
      writeVarDef(((VariableDeclaration) statement.variable).identifier.str, true);
    } else {
	  throw new ArgumentException();
    }
    writeOp(JsFunction.OP_DROP);

    statement.statement.visitStatement(this);
    writeJump(JsFunction.XOP_GO, statement, "continue");
    setLabel(statement, "break");
    writeOp(JsFunction.OP_DROP);

    currentBreakStatement = saveBreakStatement;
    currentContinueStatement = saveContinueStatement;

    return statement;
  }

  public virtual Statement visit(ForStatement statement) {
    addLineNumber(statement);

    if (statement.initial != null) {
      statement.initial.visitExpression(this);
	  if (!(statement.initial is VariableExpression)) {
        writeOp(JsFunction.OP_DROP);
      }
    }

    Statement saveBreakStatement = currentBreakStatement;
    Statement saveContinueStatement = currentContinueStatement;

    currentBreakStatement = statement;
    currentContinueStatement = statement;

    setLabel(statement, "start");

    if (statement.condition != null) {
      visitWithNewLabelSet(statement.condition);
      writeJump(JsFunction.XOP_IF, statement, "break");
    }

    if (statement.statement != null) {
      visitWithNewLabelSet(statement.statement);
    }

    setLabel(statement, "continue");

    if (statement.increment != null) {
      visitWithNewLabelSet(statement.increment);
      writeOp(JsFunction.OP_DROP);
    }

    writeJump(JsFunction.XOP_GO, statement, "start");

    setLabel(statement, "break");

    currentBreakStatement = saveBreakStatement;
    currentContinueStatement = saveContinueStatement;

    return statement;
  }

  public virtual Statement visit(IfStatement statement) {
    addLineNumber(statement);

    statement.expression.visitExpression(this);
    if (statement.falseStatement == null) {
      writeJump(JsFunction.XOP_IF, statement, "endif");
      statement.trueStatement.visitStatement(this);
    } else {
      writeJump(JsFunction.XOP_IF, statement, "else");
      statement.trueStatement.visitStatement(this);
      writeJump(JsFunction.XOP_GO, statement, "endif");
      setLabel(statement, "else");
      statement.falseStatement.visitStatement(this);
    }
    setLabel(statement, "endif");
    return statement;
  }

  public virtual Statement visit(ReturnStatement statement) {
    addLineNumber(statement);

    if (statement.expression == null) {
      writeOp(JsFunction.OP_PUSH_UNDEF);
    } else {
      statement.expression.visitExpression(this);
    }
    writeOp(JsFunction.OP_RET);
    return statement;
  }

  public virtual Statement visit(SwitchStatement statement) {
    addLineNumber(statement);

    statement.expression.visitExpression(this);

    Statement saveBreakStatemet = currentBreakStatement;
    currentBreakStatement = statement;

    String defaultLabel = "break";

	for (int i = 0; i < statement.clauses.Length; i++) {
      CaseStatement cs = statement.clauses[i];
      if (cs.expression == null) {
        defaultLabel = "case" + i;
      } else {
        writeOp(JsFunction.OP_DUP);
        cs.expression.visitExpression(this);
        writeOp(JsFunction.OP_EQEQEQ);
        writeOp(JsFunction.OP_NOT);
        writeJump(JsFunction.XOP_IF, statement, "case" + i);
      }
    }

    writeOp(JsFunction.OP_DROP);
    writeJump(JsFunction.XOP_GO, statement, defaultLabel);

	for (int i = 0; i < statement.clauses.Length; i++) {
      setLabel(statement, "case" + i);
      Statement[] statements = statement.clauses[i].statements;
	  for (int j = 0; j < statements.Length; j++) {
        statements[j].visitStatement(this);
      }
    }
    setLabel(statement, "break");

    currentBreakStatement = saveBreakStatemet;
    return statement;
  }

  public virtual Statement visit(ThrowStatement statement) {
    addLineNumber(statement);

    statement.expression.visitExpression(this);
    if (currentTryStatement == null) {
      writeOp(JsFunction.OP_THROW);
    } else {
      writeJump(JsFunction.XOP_GO, currentTryStatement, "catch");
    }
    return statement;
  }

  public virtual Statement visit(TryStatement statement) {
    addLineNumber(statement);

    Statement saveTryStatement = currentTryStatement;
    String saveTryLabel = currentTryLabel;

    currentTryStatement = statement;
    currentTryLabel = statement.catchBlock != null ? "catch" : "finally";

    statement.tryBlock.visitStatement(this);

    writeJump(JsFunction.XOP_GO, statement, "end");

    if (statement.catchBlock != null) {
      setLabel(statement, "catch");
      if (statement.finallyBlock == null) {
        currentTryLabel = saveTryLabel;
        currentTryStatement = saveTryStatement;
      } else {
        currentTryLabel = "finally";
      }

      // add var and init from stack
      writeVarDef(statement.catchIdentifier.str, true);
      writeOp(JsFunction.OP_DROP);
      statement.catchBlock.visitStatement(this);

      writeJump(JsFunction.XOP_GO, statement, "end");
    }

    // reset everything
    currentTryStatement = saveTryStatement;
    currentTryLabel = saveTryLabel;

    if (statement.finallyBlock != null) {
      // finally block for the case that an exception was thrown --
      // it is kept on the stack and rethrown at the end
      setLabel(statement, "finally");
      statement.finallyBlock.visitStatement(this);

      if (currentTryStatement == null) {
        writeOp(JsFunction.OP_THROW);
      } else {
        writeJump(JsFunction.XOP_GO, currentTryStatement, "catch");
      }
    }

    // finally block if no exception was thrown
    setLabel(statement, "end");

    if (statement.finallyBlock != null) {
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
    addLineNumber(statement);

    Statement saveBreakStatement = currentBreakStatement;
    Statement saveContinueStatement = currentContinueStatement;

    currentBreakStatement = statement;
    currentContinueStatement = statement;

    setLabel(statement, "continue");
    visitWithNewLabelSet(statement.expression);
    writeJump(JsFunction.XOP_IF, statement, "break");

    visitWithNewLabelSet(statement.statement);

    writeJump(JsFunction.XOP_GO, statement, "continue");

    setLabel(statement, "break");

    currentBreakStatement = saveBreakStatement;
    currentContinueStatement = saveContinueStatement;
    return statement;
  }

  public virtual Statement visit(WithStatement statement) {
    addLineNumber(statement);

    if (currentTryStatement == null) {
      statement.expression.visitExpression(this);
      writeOp(JsFunction.OP_WITH_START);
      statement.statement.visitStatement(this);
      writeOp(JsFunction.OP_WITH_END);
    } else {
      // if an exception is thrown inside the with statement,
      // it is necessary to restore the context
      Statement saveTryStatement = currentTryStatement;
      String saveTryLabel = currentTryLabel;
      currentTryLabel = "finally";
      currentTryStatement = statement;
      statement.expression.visitExpression(this);
      writeOp(JsFunction.OP_WITH_END);
      statement.statement.visitStatement(this);
      writeOp(JsFunction.OP_WITH_END);
      writeJump(JsFunction.XOP_GO, statement, "end");

      currentTryStatement = saveTryStatement;
      currentTryLabel = saveTryLabel;

      setLabel(statement, "finally");
      writeOp(JsFunction.OP_WITH_END);
      writeOp(JsFunction.OP_THROW);
      setLabel(statement, "end");
    }
    return statement;
  }

  //
  // expression
  //

  public virtual Expression visit(Identifier identifier) {
    Expression pa = pendingAssignment;
    pendingAssignment = null;

	Identifier localVariable = (Identifier) localVariableTable[identifier];

    if (localVariable != null) {
      identifier = localVariable;
    }

    if (pa == null) {
      writeOpGet(identifier);
	} else if (pa is AssignmentExpression) {
      ((AssignmentExpression) pa).rightExpression.visitExpression(this);
      writeOpSet(identifier);
	} else if (pa is AssignmentOperatorExpression) {
      writeOpGet(identifier);
      ((AssignmentOperatorExpression) pa).rightExpression.visitExpression(this);
      writeBinaryOperator(((AssignmentOperatorExpression) pa).type);
      writeOpSet(identifier);
	} else if (pa is IncrementExpression) {
      IncrementExpression ie = (IncrementExpression) pa;
      writeOpGet(identifier);
      writeXop(JsFunction.XOP_ADD, ((IncrementExpression) pa).value);
      writeOpSet(identifier);
      if (ie.post) {
        writeXop(JsFunction.XOP_ADD, -((IncrementExpression) pa).value);
      }
	} else if (pa is DeleteExpression) {
      writeOp(JsFunction.OP_CTX);
      writeXop(JsFunction.XOP_PUSH_STR, getStringLiteralIndex(identifier.str));
      writeOp(JsFunction.OP_DEL);
    } else {
	  throw new System.ArgumentException();
    }

    return identifier;
  }

  public virtual Expression visit(BinaryOperatorExpression expression) {
    expression.leftExpression.visitExpression(this);
    expression.rightExpression.visitExpression(this);
    writeBinaryOperator(expression.operatorToken);
    return expression;
  }

  public virtual Expression visit(UnaryOperatorExpression expression) {
    expression.subExpression.visitExpression(this);
    writeUnaryOperator(expression.operatorToken);
    return expression;
  }

  public virtual Expression visit(AssignmentExpression expression) {

    Expression savePendingAssignment = pendingAssignment;
    pendingAssignment = expression;
    expression.leftExpression.visitExpression(this);
    if (pendingAssignment != null) {
	  throw new Exception("Pending assignment was not resolved");
    }
    pendingAssignment = savePendingAssignment;
    return expression;
  }

  public virtual Expression visit(AssignmentOperatorExpression expression) {
    Expression savePendingAssignment = pendingAssignment;
    pendingAssignment = expression;
    expression.leftExpression.visitExpression(this);
    if (pendingAssignment != null) {
	  throw new Exception("Pending assignment was not resolved");
    }
    pendingAssignment = savePendingAssignment;
    return expression;
  }

  public virtual Expression visit(CallExpression expression) {

	if (expression.function is PropertyExpression) {
      PropertyExpression pe = (PropertyExpression) expression.function;
      pe.leftExpression.visitExpression(this);
      writeOp(JsFunction.OP_DUP);
      pe.rightExpression.visitExpression(this);
      writeOp(JsFunction.OP_GET);
    } else {
      writeOp(JsFunction.OP_PUSH_GLOBAL);
      expression.function.visitExpression(this);
    }
    // push arguments
	for (int i = 0; i < expression.arguments.Length; i++) {
      expression.arguments[i].visitExpression(this);
    }

    if (currentTryStatement == null) {
	  writeXop(JsFunction.XOP_CALL, expression.arguments.Length);
    } else {
	  writeXop(JsFunction.XOP_TRY_CALL, expression.arguments.Length);
      writeJump(JsFunction.XOP_IF, currentTryStatement, currentTryLabel);
    }
    return expression;
  }

  public virtual Expression visit(ConditionalExpression expression) {
    expression.expression.visitExpression(this);
    writeJump(JsFunction.XOP_IF, expression, "else");
    expression.trueExpression.visitExpression(this);
    writeJump(JsFunction.XOP_GO, expression, "endif");
    setLabel(expression, "else");
    expression.falseExpression.visitExpression(this);
    setLabel(expression, "endif");
    return expression;
  }

  public virtual Expression visit(DeleteExpression expression) {
    Expression savePendingAssignment = pendingAssignment;
    pendingAssignment = expression;
    expression.subExpression.visitExpression(this);
    if (pendingAssignment != null) {
	  throw new Exception("Pending assignment was not resolved");
    }
    pendingAssignment = savePendingAssignment;
    return expression;
  }

  public virtual Expression visit(LogicalAndExpression expression) {
    expression.leftExpression.visitExpression(this);
    writeOp(JsFunction.OP_DUP);
    // jump (= skip) if false since false && any = false
    writeJump(JsFunction.XOP_IF, expression, "end");
    writeOp(JsFunction.OP_DROP);
    expression.rightExpression.visitExpression(this);
    setLabel(expression, "end");
    return expression;
  }

  public virtual Expression visit(LogicalOrExpression expression) {
    expression.leftExpression.visitExpression(this);
    writeOp(JsFunction.OP_DUP);
    // jump (= skip) if true since true && any =
    writeOp(JsFunction.OP_NOT);
    writeJump(JsFunction.XOP_IF, expression, "end");
    writeOp(JsFunction.OP_DROP);
    expression.rightExpression.visitExpression(this);
    setLabel(expression, "end");
    return expression;
  }

  public virtual Expression visit(NewExpression expression) {
    expression.function.visitExpression(this);
    writeOp(JsFunction.OP_NEW);
    if (expression.arguments != null) {
	  for (int i = 0; i < expression.arguments.Length; i++) {
        expression.arguments[i].visitExpression(this);
      }
	  writeXop(JsFunction.XOP_CALL, expression.arguments.Length);
    } else {
      writeXop(JsFunction.XOP_CALL, 0);
    }
    writeOp(JsFunction.OP_DROP);
    return expression;
  }

  public virtual Expression visit(IncrementExpression expression) {
    Expression savePendingAssignment = pendingAssignment;
    pendingAssignment = expression;
    expression.subExpression.visitExpression(this);
    if (pendingAssignment != null) {
	  throw new Exception("Pending assignment was not resolved");
    }
    pendingAssignment = savePendingAssignment;
    return expression;
  }

  public virtual Expression visit(PropertyExpression expression) {
    Expression pa = pendingAssignment;
    pendingAssignment = null;

    if (pa == null) {
      expression.leftExpression.visitExpression(this);
      expression.rightExpression.visitExpression(this);
      writeOp(JsFunction.OP_GET);
	} else if (pa is AssignmentExpression) {
      // push value
      ((AssignmentExpression) pa).rightExpression.visitExpression(this);
      // push object
      expression.leftExpression.visitExpression(this);
      // push property
      expression.rightExpression.visitExpression(this);
      writeOp(JsFunction.OP_SET);
	} else if (pa is AssignmentOperatorExpression) {
      // this case is a bit tricky...
      AssignmentOperatorExpression aoe = (AssignmentOperatorExpression) pa;
      expression.leftExpression.visitExpression(this);
      expression.rightExpression.visitExpression(this);
      // duplicate object and member
      writeOp(JsFunction.OP_DDUP);
      writeOp(JsFunction.OP_GET);
      // push value
      aoe.rightExpression.visitExpression(this);
      // exec assignment op
      writeBinaryOperator(aoe.type);
      // move result value below object and property
      writeOp(JsFunction.OP_ROT);
      writeOp(JsFunction.OP_SET);
	} else if (pa is IncrementExpression) {
      IncrementExpression ie = (IncrementExpression) pa;
      expression.leftExpression.visitExpression(this);
      expression.rightExpression.visitExpression(this);
      // duplicate object and member
      writeOp(JsFunction.OP_DDUP);
      writeOp(JsFunction.OP_GET);
      // increment / decrement
      writeXop(JsFunction.XOP_ADD, ie.value);
      // move result value below object and property
      writeOp(JsFunction.OP_ROT);
      writeOp(JsFunction.OP_SET);
      if (ie.post) {
        writeXop(JsFunction.XOP_ADD, -ie.value);
      }
	} else if (pa is DeleteExpression) {
      expression.leftExpression.visitExpression(this);
      expression.rightExpression.visitExpression(this);
      writeOp(JsFunction.OP_DEL);
    }
    return expression;
  }


  /**
   * Used in for statements only. Does not leave anything on the stack
   * -- in contrast to all other expressions. Handled properly in
   * visit(ForStatement).
   */
  public virtual Expression visit(VariableExpression expression) {
	for (int i = 0; i < expression.declarations.Length; i++) {
      expression.declarations[i].visitExpression(this);
    }
    return expression;
  }

  public virtual Expression visit(VariableDeclaration declaration) {
    if (declaration.initializer != null) {
      declaration.initializer.visitExpression(this);
      writeVarDef(declaration.identifier.str, true);
      writeOp(JsFunction.OP_DROP);
    } else {
      writeVarDef(declaration.identifier.str, false);
    }
    return declaration;
  }

  //
  // Identifiers and literals
  //

  public virtual Expression visit(ThisLiteral literal) {
    writeOp(JsFunction.OP_PUSH_THIS);
    return literal;
  }

  public virtual Expression visit(NullLiteral literal) {
    writeOp(JsFunction.OP_PUSH_NULL);
    return literal;
  }

  public virtual Expression visit(BooleanLiteral literal) {
    writeOp(literal.value ? JsFunction.OP_PUSH_TRUE
        : JsFunction.OP_PUSH_FALSE);
    return literal;
  }

  public virtual Expression visit(NumberLiteral literal) {
    double v = literal.value;
	if (32767 >= v && v >= -32767 && v == Math.Floor(v)) {
      writeXop(JsFunction.XOP_PUSH_INT, (int) v);
    } else {
	  double? d = new double?(v);
	  int i = numberLiterals.IndexOf(d);
      if (i == -1) {
		i = numberLiterals.Count;
		numberLiterals.Add(d);
      }
      writeXop(JsFunction.XOP_PUSH_NUM, i);
    }

    return literal;
  }

  public virtual Expression visit(StringLiteral literal) {
    writeXop(JsFunction.XOP_PUSH_STR, getStringLiteralIndex(literal.str));
    return literal;
  }

  public virtual Expression visit(ArrayLiteral literal) {
    writeOp(JsFunction.OP_NEW_ARR);
	for (int i = 0; i < literal.elements.Length; i++) {
      if (literal.elements[i] == null) {
        writeOp(JsFunction.OP_PUSH_UNDEF);
      } else {
        literal.elements[i].visitExpression(this);
      }
      writeOp(JsFunction.OP_APPEND);
    }
    return literal;
  }

  public virtual Expression visit(FunctionLiteral literal) {
    ByteArrayOutputStream baos = new ByteArrayOutputStream();

    new CodeGenerationVisitor(this, literal, new DataOutputStream(baos));

	functionLiterals.Add(baos.toByteArray());

	writeXop(JsFunction.XOP_PUSH_FN, functionLiterals.Count - 1);

    if (literal.name != null) {
      writeVarDef(literal.name.str, true);
    }
    return literal;
  }

  public virtual Expression visit(ObjectLiteral literal) {
    writeOp(JsFunction.OP_NEW_OBJ);
	for (int i = 0; i < literal.properties.Length; i++) {
      literal.properties[i].visitExpression(this);
    }
    return literal;
  }

  public virtual Expression visit(ObjectLiteralProperty property) {
    property.name.visitExpression(this);
    property.value.visitExpression(this);
    writeOp(JsFunction.OP_SET_KC);

    return property;
  }

  public virtual Statement visit(LabelledStatement statement) {
	labelSet.Add(statement.identifier.str);
    statement.statement.visitStatement(this);
    return statement;
  }
}

}