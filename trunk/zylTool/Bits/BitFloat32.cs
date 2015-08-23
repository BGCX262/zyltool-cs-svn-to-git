/*
 * bitFloat32.cs
 * 32位浮点数的位结构。
 * @Author zyl910
 * 
 * [2011-10-08]
 * 定义。
 * 
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace zylTool.Bits
{
	/// <summary>
	/// BitFloat32是一个联合体，用于分析32位浮点数的位结构。
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	public unsafe struct BitFloat32
	{
		[FieldOffset(0)]
		public Single f;

		[FieldOffset(0)]
		public Int32 sd;

		[FieldOffset(0)]
		public UInt32 ud;

		/// <summary>
		/// fixed Int16 iw[2];
		/// </summary>
		[FieldOffset(0)]
		public fixed Int16 iw[2];

		/// <summary>
		/// fixed UInt16 uw[2];
		/// </summary>
		[FieldOffset(0)]
		public fixed UInt16 uw[2];

		/// <summary>
		/// fixed SByte ib[4];
		/// </summary>
		[FieldOffset(0)]
		public fixed SByte ib[4];

		/// <summary>
		/// fixed Byte ub[4];
		/// </summary>
		[FieldOffset(0)]
		public fixed Byte ub[4];


	}
}
