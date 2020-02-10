package com.google.minijoe.samples.shell;

import java.io.IOException;

import com.google.minijoe.compiler.CompilerException;
import com.google.minijoe.compiler.Eval;
import com.google.minijoe.sys.JsObject;

public class MjShell {
	  public static void main(String[] argv) throws IOException, CompilerException {
		  JsObject global = Eval.createGlobal();
		  Eval.eval("print('hello')", global);
		  while (true) {
			  boolean isExit = false;
			  System.out.print("> ");
			  StringBuffer sb = new StringBuffer();
			  while (true) {
				  int ch = System.in.read();
				  if (ch == -1) {
					  isExit = true;
					  break;
			      } else if (ch == '\n') {
					  break;
				  } else {
					  sb.append((char)ch);
				  }
			  }
			  if (isExit) {
				  break;
			  }
			  String expr = sb.toString();
			  System.out.println("" + Eval.eval(expr, global));
		  }
	  }
}
