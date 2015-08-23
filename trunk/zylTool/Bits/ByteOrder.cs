/*
 * ByteOrder.cs
 * 字节序处理。
 * @Author zyl910
 * 
 * [2011-10-09]
 * 定义。待扩展。
 * 
 * [2011-10-10]
 * T convToX<T>(bool XIsLittleEndian, T v)
 * T convToX<T>(T v, FuncSame<T> funcSwap)
 * 
 */
using System;
using System.Collections.Generic;
//using System.Text;
using System.Runtime.InteropServices;

namespace zylTool.Bits
{
	/// <summary>
	/// 字节序处理。
	/// </summary>
	public static class ByteOrder
	{
		#region by X	// 根据参数

		/// <summary>
		/// 利用 <seealso cref="IByteSwapable"/> 接口，将 本地字节序数据 转为指定字节序。
		/// </summary>
		/// <typeparam name="T">对象的类型。</typeparam>
		/// <param name="XIsLittleEndian">指定的字节序是不是小段模式。</param>
		/// <param name="v">原值。</param>
		/// <returns>返回指定字节序数据</returns>
		/// <remarks>
		/// 注：该函数还可将 指定字节序数据 转为 本地字节序。
		/// </remarks>
		public static T convToX<T>(bool XIsLittleEndian, T v) where T : IByteSwapable<T>
		{
			if (XIsLittleEndian == BitConverter.IsLittleEndian)
			{
				// 字节序相同，直接返回原值。
				return v;
			}
			else
			{
				// 字节序不同，返回字节序颠倒后的值。
				return v.swap();
			}
		}

		/// <summary>
		/// 利用 <seealso cref="FuncSame"/> 委托，将 本地字节序数据 转为指定字节序。
		/// </summary>
		/// <typeparam name="T">对象的类型。</typeparam>
		/// <param name="XIsLittleEndian">指定的字节序是不是小段模式。</param>
		/// <param name="v">原值。</param>
		/// <param name="funcSwap">交换字节顺序的函数。</param>
		/// <returns>指定字节序数据</returns>
		/// <remarks>
		/// 注：该函数还可将 指定字节序数据 转为 本地字节序。
		/// </remarks>
		public static T convToX<T>(bool XIsLittleEndian, T v, FuncSame<T> funcSwap)
		{
			if (XIsLittleEndian == BitConverter.IsLittleEndian)
			{
				return v;
			}
			else
			{
				return funcSwap(v);
			}
		}

		#endregion	// by X	// 根据X参数

		/// <summary>
		/// 将指定的数据转换为字节数组。基于 <seealso cref="System.Runtime.InteropServices.Marshal"/> 类。
		/// </summary>
		/// <param name="v">数据。</param>
		/// <returns>返回字节数组。</returns>
		/// <exception cref="System.ArgumentNullException"><c>Marshal.SizeOf(Object)</c></exception>
		public static byte[] getBytes(object v)
		{
			int cb = Marshal.SizeOf(v);
			if (cb > 0)
			{
				byte[] buf = new byte[cb];
				unsafe
				{
					fixed (void* p = &buf[0])
					{
						Marshal.StructureToPtr(v, new IntPtr(p), false);
					}
				}
				return buf;
			}
			return new byte[0];
		}

		// [Bug]IL 格式不正确。因为StructureToPtr需要object。
		//public static byte[] getBytes<T>(T v)
		//{
		//    int cb = Marshal.SizeOf(v);
		//    if (cb > 0)
		//    {
		//        byte[] buf = new byte[cb];
		//        unsafe
		//        {
		//            fixed (void* p = &buf[0])
		//            {
		//                Marshal.StructureToPtr(v, new IntPtr(p), false);
		//            }
		//        }
		//        return buf;
		//    }
		//    return new byte[0];
		//}

		/// <exception cref="System.ArgumentNullException"><c>Marshal.SizeOf(Object)</c></exception>
		public unsafe static object bytesTo(Type t, byte* pBuf, int bytesBuf)
		{
			int cb = Marshal.SizeOf(t);
			if ((cb > 0) && (cb <= bytesBuf))
			{
				return Marshal.PtrToStructure(new IntPtr((void*)pBuf), t);
			}
			return null;
		}

		/// <exception cref="System.ArgumentNullException"><c>Marshal.SizeOf(Object)</c></exception>
		public unsafe static int bytesTo<T>(ref T v, byte* pBuf, int bytesBuf)
		{
			Type t = v.GetType();
			int cb = Marshal.SizeOf(t);
			if ((cb > 0) && (cb <= bytesBuf))
			{
				object obj = Marshal.PtrToStructure(new IntPtr((void*)pBuf), t);
				if (null != obj)
				{
					v = (T)obj;
					return cb;
				}
			}
			return 0;
		}

	}
}
