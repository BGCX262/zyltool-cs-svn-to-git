/*
 * ZPixelFormat.cs
 * 像素格式的信息。
 * @Author zyl910
 * 
 * [2011-10-03]
 * 定义。
 * 
 * [2011-10-04]
 * 定义至zpfMode。
 * 
 * [2011-10-05]
 * zpfChannelAll
 * zpfChannelBits。尚未实现算法。
 * 
 * [2011-10-06]
 * zpfIsAuto
 * zpfIsFourCC
 * zpfIsNoSupport
 * zpfIsError
 * 修改ZPixelFormatType的定义，将其扩展为16种。
 * zpfCheck
 * zpfChannelN
 * 
 * [2011-10-07]
 * 为所有强转、位运算加上unchecked。
 * zpfBetter。
 * 改进typedef区。
 * 
 * 
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
// ## typedef ##	// C#不支持typedef，using仅对本模块内有效
using ZPixelFormat = System.UInt32;

namespace zylTool.Imaging
{
	//public typedef UInt32 ZPixelFormat;

	/// <summary>
	/// ZPixelFormatTool 是一个静态类，为处理ZPixelFormat提供方便。
	/// </summary>
	/// <remarks>
	/// ZPixelFormat - 像素格式。
	/// <para><c>public typedef UInt32 ZPixelFormat;</c></para>
	/// <para>仅用于表述像素各个通道的成分信息。不考虑颜色空间等信息。</para>
	/// <para>bits 0-5 = channel0	// 第0个通道的信息。B。当为索引色模式时（type==ZPFT_PACKET8, channelCount==0），channel0就是整个像素的所占位数，可为1、2、4、8（8可优化为ZPFT_CHANNELI8）。</para>
	/// <para>bits 6-11 = channel1	// 第1个通道的信息。G</para>
	/// <para>bits 12-17 = channel2	// 第2个通道的信息。R</para>
	/// <para>bits 18-23 = channel3	// 第3个通道的信息。A</para>
	/// <para>bits 24-27 = type	// 像素格式类型。见ZPixelFormatType。</para>
	/// <para>bits 28-29 = channelCount	// 占用的通道数量（包括保留的通道，用于计算通道模式下的像素字节数）。值为通道数-1，取值范围为0~3，表示1~4个通道。如DXGI_FORMAT_R16G16_UINT为(2-1)=1。</para>
	/// <para>bits 30-31 = alphaMode	// Alpha模式。详见ZPixelFormatAlphaMode。</para>
	/// <para>bits 30 = dataC3	// 第3个通道存在数据。仅当 (alpha==0, channelCount==4)时有效。</para>
	/// <para>bits 30 = palpha	// Pre-multiplied alpha。像素格式包含自左乘的 alpha 值。</para>
	/// <para>bits 31 = alpha	// 通道3是Alpha通道。当不带此标志时，忽略该通道。</para>
	/// <para></para>
	/// <para>特例——</para>
	/// <para>1.本像素模式是为内存处理设计，使用本机字节序（Byte Order）。外部存储时请注意转化字节序。</para>
	/// <para>2.当type为ZPFT_PACKET64时，各通道的参数理应在[0~64]的范围。但现在只有6位，最大为63。于是将63定义为无效阈值。这样会导致无法使用位移63，但位移63很罕见，可忽略。</para>
	/// </remarks>
	public static class ZPixelFormatTool
	{
		//public static ZPixelFormat px;	// 测试using指定类型的别名

		#region ZPixelFormat
		/// <summary>
		/// 最大支持的通道数量。
		/// </summary>
		public const int ZPF_CHANNEL_MAXCOUNT = 4;

		/// <summary>
		/// 各个通道 的位长。
		/// </summary>
		public const int ZPF_CHANNEL_BITS = 6;

		/// <summary>
		/// 通道总位长。
		/// </summary>
		public const int ZPF_CHANNEL_AllBITS = 24;

		/// <summary>
		/// 通道总掩码。
		/// </summary>
		public const UInt32 ZPF_CHANNEL_AllMASK = 0xFFFFFF;

		/// <summary>
		/// channel0 的位移。
		/// </summary>
		public const int ZPF_CHANNEL0_SHIFT = 0;
		/// <summary>
		/// channel0 的掩码。
		/// </summary>
		public const UInt32 ZPF_CHANNEL0_MASK = 0x3F;

		/// <summary>
		/// channel1 的位移。
		/// </summary>
		public const int ZPF_CHANNEL1_SHIFT = 6;
		/// <summary>
		/// channel1 的掩码。
		/// </summary>
		public const UInt32 ZPF_CHANNEL1_MASK = 0xFC0;

		/// <summary>
		/// channel2 的位移。
		/// </summary>
		public const int ZPF_CHANNEL2_SHIFT = 12;
		/// <summary>
		/// channel2 的掩码。
		/// </summary>
		public const UInt32 ZPF_CHANNEL2_MASK = 0x3F000;

		/// <summary>
		/// channel3 的位移。
		/// </summary>
		public const int ZPF_CHANNEL3_SHIFT = 18;
		/// <summary>
		/// channel3 的掩码。
		/// </summary>
		public const UInt32 ZPF_CHANNEL3_MASK = 0xFC0000;

		/// <summary>
		/// 通道无效。
		/// </summary>
		public const UInt32 ZPF_CHANNEL_INVALID = ZPF_CHANNEL0_MASK;

		/// <summary>
		/// type 的位移。
		/// </summary>
		public const int ZPF_TYPE_SHIFT = 24;
		/// <summary>
		/// type 的位长。
		/// </summary>
		public const int ZPF_TYPE_BITS = 4;
		/// <summary>
		/// type 的掩码。
		/// </summary>
		public const UInt32 ZPF_TYPE_MASK = 0x0F000000;

		/// <summary>
		/// channelCount 的位移。
		/// </summary>
		public const int ZPF_CHANNEL_COUNT_SHIFT = 28;
		/// <summary>
		/// channelCount 的位长。
		/// </summary>
		public const int ZPF_CHANNEL_COUNT_BITS = 2;
		/// <summary>
		/// channelCount 的掩码。
		/// </summary>
		public const UInt32 ZPF_CHANNEL_COUNT_MASK = 0x30000000;

		/// <summary>
		/// alphaMode 的位移。
		/// </summary>
		public const int ZPF_ALPHA_MODE_SHIFT = 30;
		/// <summary>
		/// alphaMode 的位长。
		/// </summary>
		public const int ZPF_ALPHA_MODE_BITS = 2;
		/// <summary>
		/// alphaMode 的掩码。
		/// </summary>
		public const UInt32 ZPF_ALPHA_MODE_MASK = 0xC0000000;

		/// <summary>
		/// alpha通道无效。
		/// </summary>
		/// <seealso cref="ZPixelFormatAlphaMode.None"/>
		public const UInt32 ZPF_ALPHA_MODE_NONE = 0x00000000;
		/// <summary>
		/// 数据通道。通道3存在数据，但不是Alpha。例如CMYK。
		/// </summary>
		/// <seealso cref="ZPixelFormatAlphaMode.DataC3"/>
		public const UInt32 ZPF_ALPHA_MODE_DATAC3 = 0x40000000;
		/// <summary>
		/// Alpha。通道3是Alpha通道。
		/// </summary>
		/// <seealso cref="ZPixelFormatAlphaMode.Alpha"/>
		public const UInt32 ZPF_ALPHA_MODE_ALPHA = 0x80000000;
		/// <summary>
		/// 预乘Alpha。通道3是Alpha通道，且前三个通道进行了预乘处理。
		/// </summary>
		/// <seealso cref="ZPixelFormatAlphaMode.PAlpha"/>
		public const UInt32 ZPF_ALPHA_MODE_PALPHA = 0xC0000000;

		#endregion	// ZPixelFormat

		#region ZPixelFormatType
		/// <summary>
		/// NO.
		/// <para>[ZPixelFormatType] 无效。</para>
		/// </summary>
		public const UInt32 ZPFT_NONE = 0;
		/// <summary>
		/// P8. 8bit packet(channelCount!=0), or palette indexes(channelCount==0).
		/// <para>[ZPixelFormatType] 打包8位。Byte。</para>
		/// </summary>
		public const UInt32 ZPFT_PACKET8 = 1;
		/// <summary>
		/// P16. 16bit packet.
		/// <para>[ZPixelFormatType] 打包16位。UInt16。</para>
		/// </summary>
		public const UInt32 ZPFT_PACKET16 = 2;
		/// <summary>
		/// P32. 32bit packet.
		/// <para>[ZPixelFormatType] 打包32位。UInt32。</para>
		/// </summary>
		public const UInt32 ZPFT_PACKET32 = 3;
		/// <summary>
		/// P64. 64bit packet.
		/// <para>[ZPixelFormatType] 打包64位。UInt64。</para>
		/// </summary>
		public const UInt32 ZPFT_PACKET64 = 4;
		/// <summary>
		/// F16. float16 channel.
		/// <para>[ZPixelFormatType] 16位浮点。s10e5, binary16(IEEE 754-2008), FP16(DirectX)。</para>
		/// </summary>
		public const UInt32 ZPFT_CHANNELF16 = 5;
		/// <summary>
		/// F32. float32 channel.
		/// <para>[ZPixelFormatType] 32位浮点。Single。s23e8, binary32(IEEE 754-2008), FP32(DirectX)。</para>
		/// </summary>
		public const UInt32 ZPFT_CHANNELF32 = 6;
		/// <summary>
		/// F64. float64 channel.
		/// <para>[ZPixelFormatType] 64位浮点。Double。s52e11, binary64(IEEE 754-2008), FP64(DirectX)。</para>
		/// </summary>
		public const UInt32 ZPFT_CHANNELF64 = 7;
		/// <summary>
		/// U8. byte channel.
		/// <para>[ZPixelFormatType] 8位无符号整数。Byte。</para>
		/// </summary>
		public const UInt32 ZPFT_CHANNELU8 = 8;
		/// <summary>
		/// U16. uint16 channel.
		/// <para>[ZPixelFormatType] 16位无符号整数。UInt16。</para>
		/// </summary>
		public const UInt32 ZPFT_CHANNELU16 = 9;
		/// <summary>
		/// U32. uint32 channel.
		/// <para>[ZPixelFormatType] 32位无符号整数。UInt32。</para>
		/// </summary>
		public const UInt32 ZPFT_CHANNELU32 = 10;
		/// <summary>
		/// U64. uint64 channel.
		/// <para>[ZPixelFormatType] 64位无符号整数。UInt64。</para>
		/// </summary>
		public const UInt32 ZPFT_CHANNELU64 = 11;
		/// <summary>
		/// I8. sbyte channel.
		/// <para>[ZPixelFormatType] 8位带符号整数。SByte。</para>
		/// </summary>
		public const UInt32 ZPFT_CHANNELI8 = 12;
		/// <summary>
		/// I16. int16 channel.
		/// <para>[ZPixelFormatType] 16位带符号整数。Int16。</para>
		/// </summary>
		public const UInt32 ZPFT_CHANNELI16 = 13;
		/// <summary>
		/// I32. int32 channel.
		/// <para>[ZPixelFormatType] 32位带符号整数。Int32。</para>
		/// </summary>
		public const UInt32 ZPFT_CHANNELI32 = 14;
		/// <summary>
		/// I64. int64 channel.
		/// <para>[ZPixelFormatType] 64位带符号整数。Int64。</para>
		/// </summary>
		public const UInt32 ZPFT_CHANNELI64 = 15;

		#endregion	// ZPixelFormatType

		#region ZPixelFormatMode
		/// <summary>
		/// 无效模式。type==ZPFT_NONE。
		/// </summary>
		public const UInt32 ZPFM_NONE = 0;
		/// <summary>
		/// 索引模式。(type==ZPFT_PACKET8) and (channelCount==0)。
		/// </summary>
		public const UInt32 ZPFM_INDEXED = 1;
		/// <summary>
		/// 打包模式。((type==ZPFT_PACKET8) and (channelCount!=0)) or (type in [ZPFT_PACKET16, ZPFT_PACKET64])。
		/// </summary>
		public const UInt32 ZPFM_PACKET = 2;
		/// <summary>
		/// 通道模式。type in [ZPFT_CHANNELF16, ZPFT_CHANNELI64]。
		/// </summary>
		public const UInt32 ZPFM_CHANNEL = 3;

		#endregion	// ZPixelFormatMode

		#region ZPixelFormatAlphaMode
		/// <summary>
		/// 无效。
		/// </summary>
		public const UInt32 ZPFAM_NONE = 0;
		/// <summary>
		/// 数据。通道3存在数据，但不是Alpha。例如CMYK。
		/// </summary>
		public const UInt32 ZPFAM_DATAC3 = 1;
		/// <summary>
		/// Alpha。通道3是Alpha通道。
		/// </summary>
		public const UInt32 ZPFAM_ALPHA = 2;
		/// <summary>
		/// 预乘Alpha。通道3是Alpha通道，且前三个通道进行了预乘处理。
		/// </summary>
		public const UInt32 ZPFAM_PALPHA = 3;

		#endregion	// ZPixelFormatAlphaMode

		#region const
		/// <summary>
		/// 自动。
		/// </summary>
		public static readonly ZPixelFormat Auto = zpfMake(ZPFT_NONE, 1, 0, 0, 0, 0, 0);
		/// <summary>
		/// 是FourCC格式。
		/// </summary>
		public static readonly ZPixelFormat FourCC = zpfMake(ZPFT_NONE, 2, 0, 0, 0, 0, 0);
		/// <summary>
		/// 不支持。
		/// </summary>
		public static readonly ZPixelFormat NoSupport = zpfMake(ZPFT_NONE, 3, 0, 0, 0, 0, 0);
		/// <summary>
		/// 错误。
		/// </summary>
		public static readonly ZPixelFormat Error = zpfMake(ZPFT_NONE, 4, 0, 0, 0, 0, 0);

		/// <summary>
		/// 是不是自动。
		/// </summary>
		/// <param name="pf">ZPixelFormat变量。</param>
		/// <returns>是自动就返回true，否则返回false。</returns>
		public static bool zpfIsAuto(ZPixelFormat pf)
		{
			if (ZPFT_NONE != zpfType(pf)) return false;
			return 1 == zpfChannelCount(pf);
		}

		/// <summary>
		/// 是不是FourCC。
		/// </summary>
		/// <param name="pf">ZPixelFormat变量。</param>
		/// <returns>是FourCC就返回true，否则返回false。</returns>
		public static bool zpfIsFourCC(ZPixelFormat pf)
		{
			if (ZPFT_NONE != zpfType(pf)) return false;
			return 2 == zpfChannelCount(pf);
		}

		/// <summary>
		/// 是不是不支持。
		/// </summary>
		/// <param name="pf">ZPixelFormat变量。</param>
		/// <returns>是不支持就返回true，否则返回false。</returns>
		public static bool zpfIsNoSupport(ZPixelFormat pf)
		{
			if (ZPFT_NONE != zpfType(pf)) return false;
			return 3 == zpfChannelCount(pf);
		}

		/// <summary>
		/// 是不是错误。
		/// </summary>
		/// <param name="pf">ZPixelFormat变量。</param>
		/// <returns>是错误就返回true，否则返回false。</returns>
		public static bool zpfIsError(ZPixelFormat pf)
		{
			if (ZPFT_NONE != zpfType(pf)) return false;
			return 4 == zpfChannelCount(pf);
		}

		#endregion	// const

		/// <summary>
		/// 生成ZPixelFormat。
		/// </summary>
		/// <param name="type">类型。值为 <see cref="ZPFT_NONE"/> - <see cref="ZPFT_CHANNELI64"/>。</param>
		/// <param name="channelCount">占用的通道数量（包括保留的通道，用于计算通道模式下的像素字节数）。值为 1 - 4。</param>
		/// <param name="alphaMode">Alpha模式。值为 <see cref="ZPFAM_NONE"/> - <see cref="ZPFAM_PALPHA"/>。</param>
		/// <param name="channel0">通道0的参数。ZPFM_INDEXED模式下是像素位数1/2/4。ZPFM_PACKET模式下是该通道的起始位移。ZPFM_CHANNEL模式下是该通道的索引数值（不是字节数，也不是位数）。</param>
		/// <param name="channel1">通道1的参数。ZPFM_PACKET模式下是该通道的起始位移。ZPFM_CHANNEL模式下是该通道的索引数值。</param>
		/// <param name="channel2">通道2的参数。ZPFM_PACKET模式下是该通道的起始位移。ZPFM_CHANNEL模式下是该通道的索引数值。</param>
		/// <param name="channel3">通道3的参数。ZPFM_PACKET模式下是该通道的起始位移。ZPFM_CHANNEL模式下是该通道的索引数值。</param>
		/// <returns>返回ZPixelFormat格式的变量。</returns>
		/// <remarks>该函数仅在Debug时检查数字是否溢出。不进行运行时检查，不检查语义的有效性。</remarks>
		public static ZPixelFormat zpfMake(UInt32 type, UInt32 channelCount, UInt32 alphaMode, UInt32 channel0, UInt32 channel1, UInt32 channel2, UInt32 channel3)
		{
			Debug.Assert((type >= 0) && (type <= 15));
			Debug.Assert((channelCount >= 1) && (channelCount <= 4));
			Debug.Assert((alphaMode >= 0) && (alphaMode <= 3));
			Debug.Assert((channel0 >= 0) && (channel0 <= ZPF_CHANNEL0_MASK));
			Debug.Assert((channel1 >= 0) && (channel1 <= ZPF_CHANNEL0_MASK));
			Debug.Assert((channel2 >= 0) && (channel2 <= ZPF_CHANNEL0_MASK));
			Debug.Assert((channel3 >= 0) && (channel3 <= ZPF_CHANNEL0_MASK));
			unchecked
			{
				ZPixelFormat rt = ((channel0 << ZPF_CHANNEL0_SHIFT) & ZPF_CHANNEL0_MASK)
					| ((channel1 << ZPF_CHANNEL1_SHIFT) & ZPF_CHANNEL1_MASK)
					| ((channel2 << ZPF_CHANNEL2_SHIFT) & ZPF_CHANNEL2_MASK)
					| ((channel3 << ZPF_CHANNEL3_SHIFT) & ZPF_CHANNEL3_MASK)
					| ((type << ZPF_TYPE_SHIFT) & ZPF_TYPE_MASK)
					| (((channelCount - 1) << ZPF_CHANNEL_COUNT_SHIFT) & ZPF_CHANNEL_COUNT_MASK)
					| ((alphaMode << ZPF_ALPHA_MODE_SHIFT) & ZPF_ALPHA_MODE_MASK)
					;
				return rt;
			}
		}

		/// <summary>
		/// 生成索引色模式下的ZPixelFormat。
		/// </summary>
		/// <param name="bitsPixel">像素的位数。只能是1/2/4/8。</param>
		/// <returns>返回ZPixelFormat格式的变量。<seealso cref="ZPixelFormatTool.NoSupport"/>表示失败。</returns>
		public static ZPixelFormat zpfMakeIndexed(UInt32 bitsPixel)
		{
			switch (bitsPixel)
			{
				case 1:
				case 2:
				case 4:
					return zpfMake(ZPFT_PACKET8, 1, 0, bitsPixel, ZPF_CHANNEL_INVALID, ZPF_CHANNEL_INVALID, ZPF_CHANNEL_INVALID);
				case 8:
					return zpfMake(ZPFT_CHANNELI8, 1, 0, 0, ZPF_CHANNEL_INVALID, ZPF_CHANNEL_INVALID, ZPF_CHANNEL_INVALID);
				default:
					//throw new ArgumentException("bitsPixel must be 1/2/4/8.", "bitsPixel");
					//为了性能，决定不使用异常，而是根据返回值判断。
					break;
			}
			return NoSupport;
		}

		/// <summary>
		/// 从ZPixelFormat解出各个字段。
		/// </summary>
		/// <param name="pf">ZPixelFormat变量。</param>
		/// <param name="type">类型。</param>
		/// <param name="channelCount">占用的通道数量。</param>
		/// <param name="alphaMode">Alpha模式。</param>
		/// <param name="channel0">通道0的参数。</param>
		/// <param name="channel1">通道1的参数。</param>
		/// <param name="channel2">通道2的参数。</param>
		/// <param name="channel3">通道3的参数。</param>
		public static void zpfUnmake(ZPixelFormat pf, out UInt32 type, out UInt32 channelCount, out UInt32 alphaMode, out UInt32 channel0, out UInt32 channel1, out UInt32 channel2, out UInt32 channel3)
		{
			unchecked
			{
				type = (pf & ZPF_TYPE_MASK) >> ZPF_TYPE_SHIFT;
				channelCount = 1 + ((pf & ZPF_CHANNEL_COUNT_MASK) >> ZPF_CHANNEL_COUNT_SHIFT);
				alphaMode = (pf & ZPF_ALPHA_MODE_MASK) >> ZPF_ALPHA_MODE_SHIFT;
				channel0 = (pf & ZPF_CHANNEL0_MASK) >> ZPF_CHANNEL0_SHIFT;
				channel1 = (pf & ZPF_CHANNEL1_MASK) >> ZPF_CHANNEL1_SHIFT;
				channel2 = (pf & ZPF_CHANNEL2_MASK) >> ZPF_CHANNEL2_SHIFT;
				channel3 = (pf & ZPF_CHANNEL3_MASK) >> ZPF_CHANNEL3_SHIFT;
			}
		}

		/// <summary>
		/// 从ZPixelFormat解出 type 字段。
		/// </summary>
		/// <param name="pf">ZPixelFormat变量。</param>
		/// <returns>解出的数据。</returns>
		public static UInt32 zpfType(ZPixelFormat pf)
		{
			unchecked
			{
				return (pf & ZPF_TYPE_MASK) >> ZPF_TYPE_SHIFT;
			}
		}

		/// <summary>
		/// 从ZPixelFormat解出 channelCount 字段。
		/// </summary>
		/// <param name="pf">ZPixelFormat变量。</param>
		/// <returns>解出的数据。</returns>
		public static UInt32 zpfChannelCount(ZPixelFormat pf)
		{
			unchecked
			{
				return 1 + ((pf & ZPF_CHANNEL_COUNT_MASK) >> ZPF_CHANNEL_COUNT_SHIFT);
			}
		}

		/// <summary>
		/// 从ZPixelFormat解出 alphaMode 字段。
		/// </summary>
		/// <param name="pf">ZPixelFormat变量。</param>
		/// <returns>解出的数据。</returns>
		public static UInt32 zpfAlphaMode(ZPixelFormat pf)
		{
			unchecked
			{
				return (pf & ZPF_ALPHA_MODE_MASK) >> ZPF_ALPHA_MODE_SHIFT;
			}
		}

		/// <summary>
		/// 从ZPixelFormat解出 channel0 字段。
		/// </summary>
		/// <param name="pf">ZPixelFormat变量。</param>
		/// <returns>解出的数据。</returns>
		public static UInt32 zpfChannel0(ZPixelFormat pf)
		{
			unchecked
			{
				return (pf & ZPF_CHANNEL0_MASK) >> ZPF_CHANNEL0_SHIFT;
			}
		}

		/// <summary>
		/// 从ZPixelFormat解出 channel1 字段。
		/// </summary>
		/// <param name="pf">ZPixelFormat变量。</param>
		/// <returns>解出的数据。</returns>
		public static UInt32 zpfChannel1(ZPixelFormat pf)
		{
			unchecked
			{
				return (pf & ZPF_CHANNEL1_MASK) >> ZPF_CHANNEL1_SHIFT;
			}
		}

		/// <summary>
		/// 从ZPixelFormat解出 channel2 字段。
		/// </summary>
		/// <param name="pf">ZPixelFormat变量。</param>
		/// <returns>解出的数据。</returns>
		public static UInt32 zpfChannel2(ZPixelFormat pf)
		{
			unchecked
			{
				return (pf & ZPF_CHANNEL2_MASK) >> ZPF_CHANNEL2_SHIFT;
			}
		}

		/// <summary>
		/// 从ZPixelFormat解出 channel3 字段。
		/// </summary>
		/// <param name="pf">ZPixelFormat变量。</param>
		/// <returns>解出的数据。</returns>
		public static UInt32 zpfChannel3(ZPixelFormat pf)
		{
			unchecked
			{
				return (pf & ZPF_CHANNEL3_MASK) >> ZPF_CHANNEL3_SHIFT;
			}
		}

		/// <summary>
		/// 从ZPixelFormat解出第i个通道的字段。
		/// </summary>
		/// <param name="pf">ZPixelFormat变量。</param>
		/// <param name="i">第i个通道</param>
		/// <returns>解出的数据。</returns>
		public static UInt32 zpfChannelN(ZPixelFormat pf, int i)
		{
			unchecked
			{
				return (pf >> (ZPF_CHANNEL0_SHIFT * i)) & ZPF_CHANNEL0_MASK;
			}
		}

		/// <summary>
		/// 计算该像素格式的所占位数。
		/// </summary>
		/// <param name="pf">ZPixelFormat变量。</param>
		/// <returns>返回所占位数。</returns>
		public static int zpfPixelBits(ZPixelFormat pf)
		{
			unchecked
			{
				int channelCount = (int)zpfChannelCount(pf);
				UInt32 type = zpfType(pf);
				switch (type)
				{
					case ZPFT_PACKET8:
						if (1 == channelCount)	// 索引色模式
						{
							return (int)zpfChannel0(pf);
						}
						return 8;
					case ZPFT_PACKET16:
						return 16;
					case ZPFT_PACKET32:
						return 32;
					case ZPFT_PACKET64:
						return 64;
					case ZPFT_CHANNELU8:
					case ZPFT_CHANNELI8:
						return 8 * channelCount;
					case ZPFT_CHANNELF16:
					case ZPFT_CHANNELU16:
					case ZPFT_CHANNELI16:
						return 16 * channelCount;
					case ZPFT_CHANNELF32:
					case ZPFT_CHANNELU32:
					case ZPFT_CHANNELI32:
						return 32 * channelCount;
					case ZPFT_CHANNELF64:
					case ZPFT_CHANNELU64:
					case ZPFT_CHANNELI64:
						return 64 * channelCount;
				}
				return 0;
			}
		}

		/// <summary>
		/// 计算该像素格式的所占字节数。
		/// </summary>
		/// <param name="pf">ZPixelFormat变量。</param>
		/// <returns>返回所占字节数。注意索引模式下可能会返回0。</returns>
		public static int zpfPixelBytes(ZPixelFormat pf)
		{
			return zpfPixelBits(pf) / 8;
		}

		/// <summary>
		/// 取得ZPixelFormat的模式。
		/// </summary>
		/// <param name="pf">ZPixelFormat变量。</param>
		/// <returns>返回ZPixelFormat的模式。值为 <see cref="ZPFM_NONE"/> - <see cref="ZPFM_CHANNEL"/>。</returns>
		public static UInt32 zpfMode(ZPixelFormat pf)
		{
			UInt32 type = zpfType(pf);
			switch (type)
			{
				case ZPFT_PACKET8:
					if (1 == zpfChannelCount(pf))	// 索引色模式
					{
						return ZPFM_INDEXED;
					}
					return ZPFM_PACKET;
				case ZPFT_PACKET16:
				case ZPFT_PACKET32:
				case ZPFT_PACKET64:
					return ZPFM_PACKET;
				case ZPFT_CHANNELF16:
				case ZPFT_CHANNELF32:
				case ZPFT_CHANNELF64:
				case ZPFT_CHANNELU8:
				case ZPFT_CHANNELU16:
				case ZPFT_CHANNELU32:
				case ZPFT_CHANNELU64:
				case ZPFT_CHANNELI8:
				case ZPFT_CHANNELI16:
				case ZPFT_CHANNELI32:
				case ZPFT_CHANNELI64:
					return ZPFM_CHANNEL;
			}
			return ZPFM_NONE;
		}

		/// <summary>
		/// 同时取得4个通道的数值。直接显示原始值，不做修正。
		/// </summary>
		/// <param name="arrChannel">一个缓冲区，用于接收4个通道的数值。</param>
		/// <param name="pf">ZPixelFormat变量。</param>
		/// <remarks>
		/// ZPFM_INDEXED模式下，第0个元素是像素位数1/2/4。
		/// <para>ZPFM_PACKET模式下，是该通道的起始位移。</para>
		/// <para>ZPFM_CHANNEL模式下，是该通道的索引数值。</para>
		/// </remarks>
		public static void zpfChannelAll(ref UInt32[] arrChannel, ZPixelFormat pf)
		{
			unchecked
			{
				arrChannel[0] = (pf & ZPF_CHANNEL0_MASK) >> ZPF_CHANNEL0_SHIFT;
				arrChannel[1] = (pf & ZPF_CHANNEL1_MASK) >> ZPF_CHANNEL1_SHIFT;
				arrChannel[2] = (pf & ZPF_CHANNEL2_MASK) >> ZPF_CHANNEL2_SHIFT;
				arrChannel[3] = (pf & ZPF_CHANNEL3_MASK) >> ZPF_CHANNEL3_SHIFT;
			}
		}

		/// <summary>
		/// 计算每个通道的位数。在ZPFM_PACKET模式下特别有用。
		/// </summary>
		/// <param name="arrChannel">一个缓冲区，用于接收4个通道的位数。</param>
		/// <param name="pf">ZPixelFormat变量。</param>
		public static void zpfChannelBits(ref UInt32[] arrChannel, ZPixelFormat pf)
		{
			UInt32 bitsPacket = 0;	// 打包模式的总位数
			UInt32 bits = 0;
			UInt32 type = zpfType(pf);
			switch (type)
			{
				case ZPFT_PACKET8:
					if (1 == zpfChannelCount(pf))	// 索引模式
					{
						arrChannel[0] = zpfChannel0(pf);
						arrChannel[1] = 0;
						arrChannel[2] = 0;
						arrChannel[3] = 0;
						return;
					}
					bitsPacket = 8;
					break;
				case ZPFT_PACKET16:
					bitsPacket = 16;
					break;
				case ZPFT_PACKET32:
					bitsPacket = 32;
					break;
				case ZPFT_PACKET64:
					bitsPacket = 64;
					break;
				case ZPFT_CHANNELU8:
				case ZPFT_CHANNELI8:
					bits = 8;
					break;
				case ZPFT_CHANNELF16:
				case ZPFT_CHANNELU16:
				case ZPFT_CHANNELI16:
					bits = 16;
					break;
				case ZPFT_CHANNELF32:
				case ZPFT_CHANNELU32:
				case ZPFT_CHANNELI32:
					bits = 32;
					break;
				case ZPFT_CHANNELF64:
				case ZPFT_CHANNELU64:
				case ZPFT_CHANNELI64:
					bits = 64;
					break;
			}
			// == main ==
			UInt32 nInvalid;	// 无效阈值。
			if (bitsPacket > 0)
			{
				// == 打包模式 ==
				UInt32[] arrShift = new UInt32[ZPF_CHANNEL_MAXCOUNT];
				zpfChannelAll(ref arrShift, pf);
				nInvalid = bitsPacket;	// 无效阈值。根据当前位数重新计算。
				if (nInvalid > ZPF_CHANNEL_INVALID) nInvalid = ZPF_CHANNEL_INVALID;
				for (int i = 0; i < ZPF_CHANNEL_MAXCOUNT; ++i)
				{
					UInt32 nShift = arrShift[i];	// 当前通道的位移
					UInt32 nNextShift = nShift;	// 更高位通道的位移。当前通道的位长 = nNextShift - nShift
					if (nShift < nInvalid)	// 当前通道有效
					{
						nNextShift = bitsPacket;
						// 检查前面的。1.获得更高位通道的位移；2.判断是否用过该位移数值。
						for (int j = 0; j < i; ++j)
						{
							UInt32 nj = arrShift[j];
							if (nShift == nj)
							{
								nNextShift = nShift;	// 该位移数值已被占用，所以本通道的位长为0。
								continue;
							}
							// 更新 更高位通道的位移
							if ((nj < nInvalid) && (nj < nNextShift))
							{
								nNextShift = nj;
							}
						}
						// 再检查后面的
						if (nNextShift > nShift)
						{
							for (int j = i + 1; j < ZPF_CHANNEL_MAXCOUNT; ++j)
							{
								UInt32 nj = arrShift[j];
								// 更新 更高位通道的位移
								if ((nj < nInvalid) && (nj < nNextShift))
								{
									nNextShift = nj;
								}
							}
						}
					}
					arrChannel[0] = nNextShift - nShift;
				}
				return;
			}
			// == 通道模式 ==
			nInvalid = zpfChannelCount(pf);	// 无效阈值。为通道数目。
			for (int i = 0; i < ZPF_CHANNEL_MAXCOUNT; ++i)
			{
				UInt32 n = 0;
				if (zpfChannelN(pf,i) < nInvalid)	// 有效通道
				{
					n = bits;
				}
				arrChannel[i] = n;
			}
		}

		/// <summary>
		/// 检查ZPixelFormat是否有效。
		/// </summary>
		/// <param name="pf">ZPixelFormat变量。</param>
		/// <returns>若有效，就返回true，否则返回false。</returns>
		public static bool zpfCheck(ZPixelFormat pf)
		{
			unchecked
			{
				UInt32 mode = zpfMode(pf);
				UInt32 nInvalid;	// 无效阈值。
				if (ZPFM_INDEXED == mode)
				{
					UInt32 bits = zpfChannel0(pf);
					switch (bits)
					{
						case 1:
						case 2:
						case 4:
						case 8:	// 虽然不是最优化，但允许。
							return true;
					}
				}
				else if (ZPFM_PACKET == mode)
				{
					nInvalid = (uint)zpfPixelBits(pf);	// 无效阈值。根据位数。
					if (nInvalid > ZPF_CHANNEL_INVALID) nInvalid = ZPF_CHANNEL_INVALID;
					for (int i = 0; i < ZPF_CHANNEL_MAXCOUNT; ++i)
					{
						UInt32 n = zpfChannelN(pf, i);
						if ((n >= 0) && (n < nInvalid))
							return true;	// 至少存在一个有效的通道
					}
				}
				else if (ZPFM_CHANNEL == mode)
				{
					nInvalid = zpfChannelCount(pf);	// 无效阈值。根据通道数量。
					for (int i = 0; i < ZPF_CHANNEL_MAXCOUNT; ++i)
					{
						UInt32 n = zpfChannelN(pf, i);
						if ((n >= 0) && (n < nInvalid))
							return true;	// 至少存在一个有效的通道
					}
				}
				return false;
			}
		}

		/// <summary>
		/// 优化ZPixelFormat。尝试将索引模式和打包模式转化为通道模式。
		/// </summary>
		/// <param name="pf">ZPixelFormat变量。</param>
		/// <returns>返回优化后的结果。如果不能优化，就返回原值。</returns>
		public static ZPixelFormat zpfBetter(ZPixelFormat pf)
		{
			unchecked
			{
				UInt32 type = zpfType(pf);
				if ((ZPFT_PACKET8 <= type) && (type <= ZPFT_PACKET64))	// 索引模式和打包模式
				{
					int channelCount = (int)zpfChannelCount(pf);
					if ((ZPFT_PACKET8 == type) && (1 == channelCount))	// 索引色模式
					{
						if (8 == zpfChannel0(pf))	// 8位，可转为ZPFT_CHANNELU8。
						{
							return zpfMakeIndexed(8);
						}
					}
					else
					{
						// 获取基本数据
						UInt32[] arrShift = new UInt32[ZPF_CHANNEL_MAXCOUNT];	// 位移
						zpfChannelAll(ref arrShift, pf);
						UInt32[] arrBits = new UInt32[ZPF_CHANNEL_MAXCOUNT];	// 位长
						zpfChannelBits(ref arrBits, pf);
						uint pixelBits = (uint)zpfPixelBits(pf);
						uint nInvalid = pixelBits;	// 无效阈值。根据位数。
						if (nInvalid > ZPF_CHANNEL_INVALID) nInvalid = ZPF_CHANNEL_INVALID;

						// 检查
						int cntChannel = 0;	// 所用到的通道数量
						UInt32 maxShift = 0;	// 最大的有效位移
						bool[] isAlign = new bool[4] { true, true, true, true };	// 能被 8/16/32/64 整除
						UInt32 channelBits;	// 单个通道的位数。8/16/32/64
						for (int i = 0; i < ZPF_CHANNEL_MAXCOUNT; ++i)
						{
							if (arrShift[i] < nInvalid)	// 有效的位移
							{
								++cntChannel;
								if (maxShift < arrShift[i]) maxShift = arrShift[i];
								channelBits = 8;	// 除数。8/16/32/64
								for (int j = 0; j < 4; ++j)
								{
									isAlign[j] &= (0 == (arrShift[i] % channelBits));	//位移是否能被 channelBits 整除
									isAlign[j] &= (channelBits == arrBits[i]);	//位长是否与 channelBits 相等
									// next
									channelBits *= 2;	// 8/16/32/64
								}
							}
						}

						// 尝试优化
						if (cntChannel > 0)
						{
							UInt32[] arr = new UInt32[ZPF_CHANNEL_MAXCOUNT];	// 新的通道值（通道模式下是索引）
							UInt32 alphaMode = zpfAlphaMode(pf);	// 继承原来的Alpha模式
							UInt32 typeNew = ZPFT_PACKET64;	// 新的类型
							channelBits = 64;	// 先尝试64位，再尝试低位
							for (int i = 3; i >= 0; --i)
							{
								if (isAlign[i] && (channelBits <= pixelBits))
								{
									// 检查通道数量
									UInt32 cntNew;	// 新的通道数量
									cntNew = pixelBits / channelBits;
									if (cntNew > ZPF_CHANNEL_MAXCOUNT) continue;	// 通道数量不能超过ZPF_CHANNEL_MAXCOUNT
									if ((maxShift / channelBits) >= cntNew) continue;	// 索引大于通道数
									// 计算新的索引
									for (int j = 0; j < ZPF_CHANNEL_MAXCOUNT; ++j)
									{
										UInt32 n = arrShift[j];
										if (n < nInvalid)	// 有效地位移
										{
											// 转为索引
											n = n / channelBits;
											//if (Environment.
											if (!BitConverter.IsLittleEndian)
											{
												// 大端方式
												n = (cntNew - 1) - n;
											}
										}
										else
										{
											// 无效的位移
											n = ZPF_CHANNEL_INVALID;
										}
										arr[j] = n;
									}
									// 生成
									//UInt32 cntNew = 1 + (maxShift / channelBits);	// 新的通道数量
									return zpfMake(typeNew, cntNew, alphaMode, arr[0], arr[1], arr[2], arr[3]);
								}
								// Next
								typeNew -= 1;	// 利用 ZPFT_PACKET8~ZPFT_PACKET64 的连续性。
								channelBits /= 2;
							}
						}
					}
				}
				return pf;
			}
		}

	}
}
