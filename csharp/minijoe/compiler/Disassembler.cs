using System;
using System.Text;
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

//import java.io.DataInputStream;
//import java.io.IOException;

/**
 * @author Andy Hayward
 */
public class Disassembler {
  internal const string HEX_DIGITS = "0123456789ABCDEF";

  internal static readonly String[] OPCODES = {
    "NOP",
    "ADD",
    "AND",
    "APPEND",
    "ASR",
    "ENUM",
    "IN",
    "DIV",
    "DUP",
    "EQEQ",
    "CTX_GET",
    "GET",
    "CTX",
    "DEL",
    "GT",
    "THROW",
    "INC",
    "DEC",
    "LT",
    "MOD",
    "MUL",
    "NEG",
    "NEW_ARRAY",
    "NEW_OBJ",
    "NEW",
    "NOT",
    "OR",
    "DROP",
    "PUSH_TRUE",
    "PUSH_FALSE",
    "RET",
    "CTX_SET",
    "SET_KC",
    "SET",
    "SHL",
    "SHR",
    "SUB",
    "SWAP",
    "PUSH_THIS",
    "PUSH_NULL",
    "UNDEF",
    "DDUP",
    "ROT",
    "EQEQEQ",
    "XOR",
    "INV",
    "WITH_START",
    "WITH_END",
    "ABOVE",
    "INSTANCEOF",
    "TYPEOF",
    "PUSH_GLOBAL"
  };

  internal const int XCODE_START = 0xEA;

  internal static readonly String[] XCODES = {
    "PUSH_FN",
    "PUSH_NUM",
    "GO", "IF",
    "CALL",
    "LINE",
    "LCL_GET",
    "LCL_SET",
    "NEXT",
    "PUSH_INT",
    "PUSH_STR"
  };


  internal DataInputStream dis;
  internal String[] globalStringTable;
  internal String indent = "";
  internal String[] stringLiterals;
  internal double[] numberLiterals;
  internal String[] localVariableNames;

  public Disassembler(DataInputStream dis) {
    this.dis = dis;
  }

  internal Disassembler(DataInputStream dis, String[] globalStringTable, String indent) {
    this.dis = dis;
    this.globalStringTable = globalStringTable;
    this.indent = indent;
  }

  public virtual void dump() {
    StringBuilder buf = new StringBuilder(7);
	for (int i = 0; i < 7; i++) {
	  buf.Append((char) dis.read());
	}
	String magic = buf.ToString();
	Console.WriteLine("Header: \"" + magic + "\" Version " + dis.read());

	if (!"MiniJoe".Equals(magic)) {
      throw new IOException("Magic does not match \"MiniJoe\"!");
    }

    dumpTables();

	Console.WriteLine("EOF: " + (dis.read() == -1));
  }

  internal virtual void dumpTables() {
    //loop:
      while (true) {
        int type = dis.read();
		Console.Write(indent);
		Console.Write("Block type ");
        printHex(type);
		Console.Write(": ");
        int count;
        switch (type) {
          case 0x00:
			Console.WriteLine("Comment");
			Console.Write(indent);
			Console.WriteLine(dis.readUTF());

            break;

          case 0x10:
            count = dis.readUnsignedShort();
			Console.WriteLine("Global String Table (" + count + " entries)");
			globalStringTable = new string[count];
            for (int i = 0; i < count; i++) {
              globalStringTable[i] = dis.readUTF();
			  Console.WriteLine(indent + "  " + i + ": \"" + globalStringTable[i] + "\"");
            }
            break;

          case 0x20:
            count = dis.readUnsignedShort();
			Console.WriteLine("Number Literals (" + count + " entries)");
            numberLiterals = new double[count];
            for (int i = 0; i < count; i++) {
              numberLiterals[i] = dis.readDouble();
			  Console.WriteLine(indent + "  " + i + ": " + numberLiterals[i]);
            }
            break;

          case 0x30:
            count = dis.readUnsignedShort();
			Console.WriteLine("String Literals (" + count + " entries)");
            stringLiterals = new String[count];
            for (int i = 0; i < count; i++) {
              int index = dis.readUnsignedShort();
			  Console.WriteLine(indent + "  " + i + " -> " + index + ": \"" + globalStringTable[index] + "\"");
              stringLiterals[i] = globalStringTable[index];
            }
            break;

          case 0x40:
            count = dis.readUnsignedShort();
			Console.WriteLine("Regex Literals (" + count + " entries)");
            stringLiterals = new String[count];
            for (int i = 0; i < count; i++) {
              int index = dis.readUnsignedShort();
			  Console.WriteLine(indent + "  " + i + " -> " + index + ": \"" + globalStringTable[index] + "\"");
            }
            break;

          case 0x50:
            count = dis.readUnsignedShort();
			Console.WriteLine("Function Literals (" + count + " entries)");
            for (int i = 0; i < count; i++) {
			  Console.WriteLine(indent + "  function literal " + i + ": ");
              new Disassembler(dis, globalStringTable, indent + "    ").dumpTables();
            }
            break;

          case 0x60:
            count = dis.readUnsignedShort();
			Console.WriteLine("Local Variable Names (" + count + " entries)");
            localVariableNames = new String[count];
            for (int i = 0; i < count; i++) {
              int index = dis.readUnsignedShort();
			  Console.WriteLine(indent + "  " + i + " -> " + index + ": \"" + globalStringTable[index] + "\"");
              localVariableNames[i] = globalStringTable[index];
            }
            break;

          case 0x080:
            int locals = dis.readUnsignedShort();
            int parameters = dis.readUnsignedShort();
            int flags = dis.read();
            int size = dis.readUnsignedShort();
			Console.WriteLine("Code (locals:" + locals + " param:" + parameters + " flags:" + MyInteger.toBinaryString(flags) + " size: " + size + ")");
			sbyte[] code = new sbyte [size];
            dis.readFully(code);
            disassemble(code);
            break;

          case 0xE0:
            count = dis.readUnsignedShort();
			Console.WriteLine("Line Numbers (" + count + " entries)");
            for (int i = 0; i < count; i++) {
              int programCounter = dis.readUnsignedShort();
              int lineNumber = dis.readUnsignedShort();
			  Console.Write(indent + "  ");
              printHex(programCounter >> 8);
              printHex(programCounter);
			  Console.WriteLine(" line = " + lineNumber);
            }
            break;
            
          case 0x0ff:
			Console.WriteLine("End Marker");
			goto loopBreak;

          default:
		    Console.WriteLine("Unknown block type -- aborting");
            throw new IOException("Unknown block type: " + type);
        }
      }
loopBreak:;
  }

  internal virtual void printHex(int i) {
	Console.Write(HEX_DIGITS[(i >> 4) & 15]);
	Console.Write(HEX_DIGITS[i & 15]);
  }

  private void disassemble(sbyte[] code) {
    int i = 0;

	while (i < code.Length) {
      int opcode;

      String name = null;

	  Console.Write(indent + "  ");

      printHex(i >> 8);
      printHex(i & 255);
	  Console.Write(' ');
      
      printHex(opcode = code[i++]);
	  Console.Write(' ');
      if (opcode >= 0) {
		Console.Write("     ");
		if (opcode < OPCODES.Length) {
          name = OPCODES[opcode];
        }
		Console.Write(name == null ? "???" : name);
      } else {
		int index = ((opcode & 0x0ff) - XCODE_START) >> 1;
	    if (index < XCODES.Length) {
          name = XCODES[index];
        }
        printHex(code[i]);
        int imm;
        if ((opcode & 1) == 0) {
		  Console.Write("  ");
          imm = code[i++];
        } else {
          imm = (code[i] << 8) | (code[i + 1] & 0xff);
          i++;
          printHex(code[i++]);
        }
		Console.Write(' ');
		Console.Write(name == null ? "???" : name);
		Console.Write(' ');
		Console.Write("" + imm);

        switch (opcode & 0xfe) {
          case 0xEC:
		    Console.Write(" -> " + numberLiterals[imm]);
            break;
          case 0xFE:
		    Console.Write(" -> \"" + stringLiterals[imm] + "\"");
            break;
        }
      }
	  Console.WriteLine();
    }
  }
}

}