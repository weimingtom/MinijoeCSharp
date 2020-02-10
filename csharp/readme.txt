breakpoint

-----------------------------------------
打印code码
Eval.cs

	  public static object eval(string input, JsObject context)
	  {
		ByteArrayOutputStream baos = new ByteArrayOutputStream();
		compile(input, baos);
-->		sbyte[] code = baos.toByteArray();
		
		
		
		if (true) {
			Console.WriteLine("code == ");
			for (int k = 0; k < code.Length; ++k) {
				Console.WriteLine("code[" + k + "] == " + code[k]);
			}
		}
		
		
-------------------------
生成code码的断点
ByteArrayOutputStream.cs

	private bool _isDebug = false;
	public void _setDebug(bool isDebug)
	{
		this._isDebug = isDebug;
	}
	public void write(int s)
	{
		if (_isDebug)
		{
---->			Console.WriteLine("Debug: write: " + (sbyte)(byte)(s & 0xff));
		}
		if (s == 77 || s == 16)
		{
			Console.WriteLine("=================");
		}
		
		