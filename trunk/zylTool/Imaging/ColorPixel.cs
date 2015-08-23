/*
 *ColorPixel.cs
 * 色彩空间和像素格式。
 * @Author zyl910
 * 
 * [2011-10-06]
 * 定义。
 * 
 * [2011-10-07]
 * 改进typedef区。
 * 
 */
using System;
using System.Collections.Generic;
//using System.Text;
using System.Runtime.InteropServices;
// ## typedef ##	// C#不支持typedef，using仅对本模块内有效
using ZPixelFormat = System.UInt32;

namespace zylTool.Imaging
{
	/// <summary>
	/// ColorPixel是一个结构体，用于存储 色彩空间和像素格式 信息。
	/// </summary>
	/// <remarks>
	/// 在一般情况下，<see cref="colorSpace"/>有效，<see cref="fourCC"/>无效。
	/// <para>当像素格式为<seealso cref="ZPixelFormatTool.FourCC"/>时，<see cref="colorSpace"/>无效，<see cref="fourCC"/>有效。</para>
	/// </remarks>
	[StructLayout(LayoutKind.Explicit)]
	public struct ColorPixel
	{
		/// <summary>
		/// 色彩空间。
		/// </summary>
		/// <remarks>
		/// 当像素格式不是<seealso cref="ZPixelFormatTool.FourCC"/>时，该字段有效。其他情况下无效。
		/// </remarks>
		[FieldOffset(0)]
		public ColorSpace colorSpace;

		/// <summary>
		/// FourCC。
		/// </summary>
		/// <remarks>
		/// 当像素格式为<seealso cref="ZPixelFormatTool.FourCC"/>时，该字段有效。其他情况下无效。
		/// </remarks>
		[FieldOffset(0)]
		public uint fourCC;

		/// <summary>
		/// 像素格式。
		/// </summary>
		/// <remarks>
		/// 在一般情况下，<see cref="colorSpace"/>有效，<see cref="fourCC"/>无效。
		/// <para>当像素格式为<seealso cref="ZPixelFormatTool.FourCC"/>时，<see cref="colorSpace"/>无效，<see cref="fourCC"/>有效。</para>
		/// </remarks>
		[FieldOffset(4)]
		public ZPixelFormat pixelFormat;

		/// <summary>
		/// 用指定的 色彩空间和像素格式 初始化 ColorPixel 结构。 
		/// </summary>
		/// <param name="colorSpace"></param>
		/// <param name="pixelFormat"></param>
		public ColorPixel(ColorSpace colorSpace, ZPixelFormat pixelFormat)
		{
			this.fourCC = 0;	// 为了通过编译
			this.colorSpace = colorSpace;
			this.pixelFormat = pixelFormat;
		}

		/// <summary>
		/// 用指定的 FourCC数字 初始化 ColorPixel 结构。 
		/// </summary>
		/// <param name="fourCC">FourCC数字。</param>
		public ColorPixel(uint fourCC)
		{
			this.colorSpace = ColorSpace.Default;	// 为了通过编译
			this.fourCC = fourCC;
			this.pixelFormat = ZPixelFormatTool.FourCC;
		}

		/// <summary>
		/// 用指定的 FourCC字符串 初始化 ColorPixel 结构。 
		/// </summary>
		/// <param name="fourCC">FourCC字符串。</param>
		public ColorPixel(string fourCC)
		{
			this.colorSpace = ColorSpace.Default;	// 为了通过编译
			this.fourCC = FourCCTool.fromString(fourCC);
			this.pixelFormat = ZPixelFormatTool.FourCC;
		}

		/// <summary>
		/// 是不是FourCC。
		/// </summary>
		/// <returns>是FourCC就返回true，否则返回false。</returns>
		public bool isFourCC()
		{
			return ZPixelFormatTool.zpfIsFourCC(pixelFormat);
		}

	}
}
