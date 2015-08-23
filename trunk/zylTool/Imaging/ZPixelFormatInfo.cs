/*
 * ZPixelFormatInfo.cs
 * 像素格式的信息。
 * @Author zyl910
 * 
 * [2011-10-03]
 * 定义。
 * 
 * [2011-10-06]
 * 修改ZPixelFormatType的定义，将其扩展为16种。
 * 
 */
using System;
//using System.Collections.Generic;
//using System.Text;

namespace zylTool.Imaging
{
	/// <summary>
	/// 像素格式类型
	/// </summary>
	public enum ZPixelFormatType
	{
		/// <summary>
		/// NO.
		/// <para>无效。</para>
		/// </summary>
		None = 0,
		/// <summary>
		/// P8. 8bit packet(channelCount!=0), or palette indexes(channelCount==0).
		/// <para>打包8位。Byte。</para>
		/// </summary>
		Packet8 = 1,
		/// <summary>
		/// P16. 16bit packet.
		/// <para>打包16位。UInt16。</para>
		/// </summary>
		Packet16 = 2,
		/// <summary>
		/// P32. 32bit packet.
		/// <para>打包32位。UInt32。</para>
		/// </summary>
		Packet32 = 3,
		/// <summary>
		/// P64. 64bit packet.
		/// <para>打包64位。UInt64。</para>
		/// </summary>
		Packet64 = 4,
		/// <summary>
		/// F16. float16 channel.
		/// <para>16位浮点。s10e5, binary16(IEEE 754-2008), FP16(DirectX)。</para>
		/// </summary>
		ChannelF16 = 5,
		/// <summary>
		/// F32. float32 channel.
		/// <para>32位浮点。Single。s23e8, binary32(IEEE 754-2008), FP32(DirectX)。</para>
		/// </summary>
		ChannelF32 = 6,
		/// <summary>
		/// F64. float64 channel.
		/// <para>64位浮点。Double。s52e11, binary64(IEEE 754-2008), FP64(DirectX)。</para>
		/// </summary>
		ChannelF64 = 7,
		/// <summary>
		/// U8. byte channel.
		/// <para>8位无符号整数。Byte。</para>
		/// </summary>
		ChannelU8 = 8,
		/// <summary>
		/// U16. uint16 channel.
		/// <para>16位无符号整数。UInt16。</para>
		/// </summary>
		ChannelU16 = 9,
		/// <summary>
		/// U32. uint32 channel.
		/// <para>32位无符号整数。UInt32。</para>
		/// </summary>
		ChannelU32 = 10,
		/// <summary>
		/// U64. uint64 channel.
		/// <para>64位无符号整数。UInt64。</para>
		/// </summary>
		ChannelU64 = 11,
		/// <summary>
		/// I8. sbyte channel.
		/// <para>8位带符号整数。SByte。</para>
		/// </summary>
		ChannelI8 = 12,
		/// <summary>
		/// I16. int16 channel.
		/// <para>16位带符号整数。Int16。</para>
		/// </summary>
		ChannelI16 = 13,
		/// <summary>
		/// I32. int32 channel.
		/// <para>32位带符号整数。Int32。</para>
		/// </summary>
		ChannelI32 = 14,
		/// <summary>
		/// I64. int64 channel.
		/// <para>64位带符号整数。Int64。</para>
		/// </summary>
		ChannelI64 = 15
	}

	/// <summary>
	/// 像素格式模式
	/// </summary>
	public enum ZPixelFormatMode
	{
		/// <summary>
		/// 无效模式。type==ZPFT_NONE。
		/// </summary>
		None = 0,
		/// <summary>
		/// 索引模式。(type==ZPFT_PACKET8) and (channelCount==0)。
		/// </summary>
		Indexed = 1,
		/// <summary>
		/// 打包模式。((type==ZPFT_PACKET8) and (channelCount!=0)) or (type in [ZPFT_PACKET16, ZPFT_PACKET64])。
		/// </summary>
		Packet = 2,
		/// <summary>
		/// 通道模式。type in [ZPFT_CHANNELF16, ZPFT_CHANNELI64]。
		/// </summary>
		Channel = 3
	}

	/// <summary>
	/// 像素格式Alpha模式
	/// </summary>
	public enum ZPixelFormatAlphaMode
	{
		/// <summary>
		/// 无效。
		/// </summary>
		None = 0,
		/// <summary>
		/// 数据。通道3存在数据，但不是Alpha。例如CMYK。
		/// </summary>
		DataC3 = 1,
		/// <summary>
		/// Alpha。通道3是Alpha通道。
		/// </summary>
		Alpha = 2,
		/// <summary>
		/// 预乘Alpha。通道3是Alpha通道，且前三个通道进行了预乘处理。
		/// </summary>
		PAlpha = 3
	}

	/// <summary>
	/// ZPixelFormatInfo是一个结构体，用于解析ZPixelFormat中的字段。
	/// </summary>
	public struct ZPixelFormatInfo
	{
	}

}
