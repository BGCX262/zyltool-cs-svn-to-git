/*
 * bitFloat32.cs
 * 16位浮点数。s10e5, binary16(IEEE 754-2008), FP16(DirectX)。
 * @Author zyl910
 * 
 * [2011-10-08]
 * 定义。待实现算法。
 * 
 * [2011-10-09]
 * 为了简化设计，将swapStatic更名swap。
 * 改为调用ByteSwap.swap来实现IByteSwapable接口。
 * 
 */
using System;
using System.Collections.Generic;
using System.Text;
//using System.Runtime.InteropServices;

namespace zylTool.Bits
{
	/// <summary>
	/// 16位浮点数。s10e5, binary16(IEEE 754-2008), FP16(DirectX)。
	/// </summary>
	public struct Float16 : IByteSwapable<Float16>
	{
		/// <summary>
		/// 原始数据。以整数方式存储16位浮点数。
		/// </summary>
		public Int16 _raw;

		/// <summary>
		/// 用指定的 32位浮点数 初始化 Float16 结构。 
		/// </summary>
		/// <param name="v"></param>
		Float16(Single v)
		{
			_raw = FP16ToSingle(v);
		}

		public static implicit operator Single(Float16 v)
		{
			return FP16FromSingle(v._raw);
		}

		public static implicit operator Float16(Single v)
		{
			Float16 rt;
			rt._raw = FP16ToSingle(v);
			return rt;
		}

		/// <summary>
		/// 将浮点数转为“以整数方式存储16位浮点数”。
		/// </summary>
		/// <param name="v">浮点数。</param>
		/// <returns>返回“以整数方式存储16位浮点数”。</returns>
		public static Single FP16FromSingle(Int16 v)
		{
			// <will>
			return 0;
		}

		/// <summary>
		/// 将“以整数方式存储16位浮点数”转为浮点数。
		/// </summary>
		/// <param name="v">以整数方式存储16位浮点数。</param>
		/// <returns>返回浮点数。</returns>
		public static Int16 FP16ToSingle(Single v)
		{
			// <will>
			return 0;
		}

		#region IByteSwapable<Float16> 成员

		/// <summary>
		/// 交换字节顺序，并返回新对象。
		/// </summary>
		/// <returns>返回交换了字节顺序后的新对象。</returns>
		public Float16 swap()
		{
			//throw new Exception("The method or operation is not implemented.");
			unchecked
			{
				Float16 rt;
				//rt._raw = (Int16)(
				//    ((_raw >> 8) & 0xFF)
				//    | ((_raw & 0xFF) << 8)
				//    );
				rt._raw = ByteSwap.swap(_raw);
				return rt;
			}
		}

		/// <summary>
		/// 交换 <seealso cref="Float16"/> 的字节顺序。
		/// </summary>
		/// <param name="v">原值。</param>
		/// <returns>返回字节顺序翻转后的值。</returns>
		public static Float16 swap(Float16 v)
		{
			return v.swap();
		}

		#endregion
	}
}
