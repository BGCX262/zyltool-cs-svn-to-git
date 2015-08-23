/*
 * FuncSame.cs
 * FuncSame委托及相关处理。
 * @Author zyl910
 * 
 * [2011-10-10]
 * FuncSame委托。源自delegate.cs。
 * 定义 FuncSameTool静态类。
 * 
 * 
 */
using System;
using System.Collections.Generic;
//using System.Text;

namespace zylTool
{
	/// <summary>
	/// 封装一个函数，该方法中参数与返回值的类型相同。
	/// </summary>
	/// <typeparam name="T">对象的类型。</typeparam>
	/// <param name="arg">参数。</param>
	/// <returns>返回值。</returns>
	/// <remarks>
	/// 用途——
	/// <para>1.转换函数。将参数进行处理后再返回。</para>
	/// </remarks>
	public delegate T FuncSame<T>(T arg);

	/// <summary>
	/// FuncSameTool是一个静态类，为处理 <seealso cref="FuncSame"/>委托 提供方便。
	/// </summary>
	public static class FuncSameTool
	{
		/// <summary>
		/// 调用函数，利用 <seealso cref="FuncSame"/> 委托来转换数据。
		/// </summary>
		/// <typeparam name="T">对象的类型。</typeparam>
		/// <param name="v">原值。</param>
		/// <param name="func">处理函数。</param>
		/// <returns>返回处理后的值。</returns>
		public static T doCall<T>(T v, FuncSame<T> func)
		{
			return func(v);
		}

		
		/// <summary>
		/// 赋值函数（仅返回原值，不做任何处理）。可作为 <seealso cref="FuncSame"/> 委托。
		/// </summary>
		/// <typeparam name="T">对象的类型。</typeparam>
		/// <param name="v">原值。</param>
		/// <returns>返回原值。</returns>
		public static T mov<T>(T v)
		{
			return v;
		}

	}
}
