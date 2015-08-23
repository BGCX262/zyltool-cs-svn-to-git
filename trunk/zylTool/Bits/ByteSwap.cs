/*
 * ByteSwap.cs
 * [Code] 交换字节顺序。
 * @Author zyl910
 * 
 * [2011-10-07]
 * 定义。待扩展。
 * 
 * [2011-10-08]
 * IByteSwapable
 * swap<T>(T v)
 * swap<T>(T v, FuncSame<T> funcSwap)
 * 
 * [2011-10-09]
 * 优化了流程，完善了注释。
 * swap(UInt16 v)
 * swap(Int16 v)
 * 注释了尚未实现的swap(Int64 v)。
 * 
 * [2011-10-10]
 * T mov<T>(T v)
 * 
 * 
 */
using System;
using System.Collections.Generic;
//using System.Text;

namespace zylTool.Bits
{
	/// <summary>
	/// 支持交换字节顺序。一般是面对结构体，交换每个字段的字节顺序。
	/// </summary>
	/// <typeparam name="T">对象的类型。在写结构体实现本接口时，应传递本结构体自身的类型。</typeparam>
	public interface IByteSwapable<T>
	{
		/// <summary>
		/// 交换字节顺序，并返回新对象。
		/// </summary>
		/// <returns>返回交换了字节顺序后的新对象。</returns>
		T swap();
	}

	/// <summary>
	/// 交换字节顺序。
	/// </summary>
	public static class ByteSwap
	{
		/// <summary>
		/// 利用 <seealso cref="IByteSwapable"/> 接口来交换字节顺序。
		/// </summary>
		/// <typeparam name="T">对象的类型。</typeparam>
		/// <param name="v">原值。</param>
		/// <returns>返回交换字节顺序后的值。</returns>
		public static T swap<T>(T v) where T : IByteSwapable<T>
		{
			return v.swap();
		}

		/// <summary>
		/// 利用 <seealso cref="FuncSame"/> 委托来交换字节顺序。
		/// </summary>
		/// <typeparam name="T">对象的类型。</typeparam>
		/// <param name="v">原值。</param>
		/// <param name="funcSwap">交换字节顺序的函数。</param>
		/// <returns>返回交换字节顺序后的值。</returns>
		/// <remarks>
		/// 建议使用 FuncSameTool.doCall 方法。
		/// </remarks>
		public static T swap<T>(T v, FuncSame<T> funcSwap)
		{
			return funcSwap(v);
		}

		/// <summary>
		/// 交换 <seealso cref="Boolean"/> 的字节顺序。
		/// </summary>
		/// <param name="v">原值。</param>
		/// <returns>返回交换字节顺序后的值。</returns>
		/// <remarks>
		/// 虽然文档中没有明确说Boolean是多少字节。但在 <seealso cref="System.IO.BinaryReader"/> 中是1字节。
		/// </remarks>
		public static Boolean swap(Boolean v)
		{
			return v;
		}

		/// <summary>
		/// 交换 <seealso cref="Byte"/> 的字节顺序。
		/// </summary>
		/// <param name="v">原值。</param>
		/// <returns>返回交换字节顺序后的值。</returns>
		public static Byte swap(Byte v)
		{
			return v;	// 1Byte
		}

		/// <summary>
		/// 交换 <seealso cref="SByte"/> 的字节顺序。
		/// </summary>
		/// <param name="v">原值。</param>
		/// <returns>返回交换字节顺序后的值。</returns>
		public static SByte swap(SByte v)
		{
			return v;	// 1Byte
		}

		/// <summary>
		/// 交换 <seealso cref="UInt16"/> 的字节顺序。
		/// </summary>
		/// <param name="v">原值。</param>
		/// <returns>返回交换字节顺序后的值。</returns>
		public static UInt16 swap(UInt16 v)
		{
			unchecked
			{
				return (UInt16)(((v >> 8) & 0xFF) | ((v & 0xFF) << 8));
			}
		}

		/// <summary>
		/// 交换 <seealso cref="Int16"/> 的字节顺序。
		/// </summary>
		/// <param name="v">原值。</param>
		/// <returns>返回交换字节顺序后的值。</returns>
		public static Int16 swap(Int16 v)
		{
			unchecked
			{
				return (Int16)swap((UInt16)v);
			}
		}

		/*public static Int64 swap(Int64 v)
		{
			return v;	// <will>
		}

		public static Double swap(Double v)
		{
			unchecked
			{
				Int64 u = BitConverter.DoubleToInt64Bits(v);
				u = swap(u);
				return BitConverter.Int64BitsToDouble(u);
			}
		}*/

	}
}
