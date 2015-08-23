/*
 * bitFloat64.cs
 * 64λ��������λ�ṹ��
 * @Author zyl910
 * 
 * [2011-10-08]
 * ���塣
 * 
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace zylTool.Bits
{
	/// <summary>
	/// BitFloat64 ��һ�������壬���ڷ���64λ��������λ�ṹ��
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	public unsafe struct BitFloat64
	{
		[FieldOffset(0)]
		public Double d;

		[FieldOffset(0)]
		public Int64 sq;

		[FieldOffset(0)]
		public UInt64 uq;

		/// <summary>
		/// fixed Single f[2];
		/// </summary>
		[FieldOffset(0)]
		public fixed Single f[2];

		/// <summary>
		/// fixed Int32 sd[2];
		/// </summary>
		[FieldOffset(0)]
		public fixed Int32 sd[2];

		/// <summary>
		/// fixed UInt32 ud[2];
		/// </summary>
		[FieldOffset(0)]
		public fixed UInt32 ud[2];

		/// <summary>
		/// fixed Int16 iw[4];
		/// </summary>
		[FieldOffset(0)]
		public fixed Int16 iw[4];

		/// <summary>
		/// fixed UInt16 uw[4];
		/// </summary>
		[FieldOffset(0)]
		public fixed UInt16 uw[4];

		/// <summary>
		/// fixed SByte ib[8];
		/// </summary>
		[FieldOffset(0)]
		public fixed SByte ib[8];

		/// <summary>
		/// fixed Byte ub[8];
		/// </summary>
		[FieldOffset(0)]
		public fixed Byte ub[8];


	}

}
