using System;
using System.IO;
using System.Text;

namespace minijoe {

//import java.io.IOException;

//import com.google.minijoe.compiler.CompilerException;
//import com.google.minijoe.compiler.Eval;
//import com.google.minijoe.sys.JsObject;

public class MjShell {
	public static void Main_(string[] argv) {
		  JsObject global = Eval.createGlobal();
		  Eval.eval("print('hello')", global);
		  while (true) {
			  bool isExit = false;
			  Console.Write("> ");
			  StringBuilder sb = new StringBuilder();
			  while (true) {
			  	  int ch = Console.Read();
				  if (ch == -1) {
					  isExit = true;
					  break;
			      } else if (ch == '\n') {
					  break;
				  } else {
					  sb.Append((char)ch);
				  }
			  }
			  if (isExit) {
				  break;
			  }
			  String expr = sb.ToString();
			  Console.WriteLine("" + Eval.eval(expr, global));
		  }
	  }
}

}
