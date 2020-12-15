using System.Runtime.InteropServices;

namespace ScriptGraph.Util
{
	[StructLayout(LayoutKind.Explicit)]
	public struct FloatInt
	{
		[FieldOffset(0)]
		public float f;

		[FieldOffset(0)]
		public int i;
	}
}