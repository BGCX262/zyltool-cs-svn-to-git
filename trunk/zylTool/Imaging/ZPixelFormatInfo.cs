/*
 * ZPixelFormatInfo.cs
 * ���ظ�ʽ����Ϣ��
 * @Author zyl910
 * 
 * [2011-10-03]
 * ���塣
 * 
 * [2011-10-06]
 * �޸�ZPixelFormatType�Ķ��壬������չΪ16�֡�
 * 
 */
using System;
//using System.Collections.Generic;
//using System.Text;

namespace zylTool.Imaging
{
	/// <summary>
	/// ���ظ�ʽ����
	/// </summary>
	public enum ZPixelFormatType
	{
		/// <summary>
		/// NO.
		/// <para>��Ч��</para>
		/// </summary>
		None = 0,
		/// <summary>
		/// P8. 8bit packet(channelCount!=0), or palette indexes(channelCount==0).
		/// <para>���8λ��Byte��</para>
		/// </summary>
		Packet8 = 1,
		/// <summary>
		/// P16. 16bit packet.
		/// <para>���16λ��UInt16��</para>
		/// </summary>
		Packet16 = 2,
		/// <summary>
		/// P32. 32bit packet.
		/// <para>���32λ��UInt32��</para>
		/// </summary>
		Packet32 = 3,
		/// <summary>
		/// P64. 64bit packet.
		/// <para>���64λ��UInt64��</para>
		/// </summary>
		Packet64 = 4,
		/// <summary>
		/// F16. float16 channel.
		/// <para>16λ���㡣s10e5, binary16(IEEE 754-2008), FP16(DirectX)��</para>
		/// </summary>
		ChannelF16 = 5,
		/// <summary>
		/// F32. float32 channel.
		/// <para>32λ���㡣Single��s23e8, binary32(IEEE 754-2008), FP32(DirectX)��</para>
		/// </summary>
		ChannelF32 = 6,
		/// <summary>
		/// F64. float64 channel.
		/// <para>64λ���㡣Double��s52e11, binary64(IEEE 754-2008), FP64(DirectX)��</para>
		/// </summary>
		ChannelF64 = 7,
		/// <summary>
		/// U8. byte channel.
		/// <para>8λ�޷���������Byte��</para>
		/// </summary>
		ChannelU8 = 8,
		/// <summary>
		/// U16. uint16 channel.
		/// <para>16λ�޷���������UInt16��</para>
		/// </summary>
		ChannelU16 = 9,
		/// <summary>
		/// U32. uint32 channel.
		/// <para>32λ�޷���������UInt32��</para>
		/// </summary>
		ChannelU32 = 10,
		/// <summary>
		/// U64. uint64 channel.
		/// <para>64λ�޷���������UInt64��</para>
		/// </summary>
		ChannelU64 = 11,
		/// <summary>
		/// I8. sbyte channel.
		/// <para>8λ������������SByte��</para>
		/// </summary>
		ChannelI8 = 12,
		/// <summary>
		/// I16. int16 channel.
		/// <para>16λ������������Int16��</para>
		/// </summary>
		ChannelI16 = 13,
		/// <summary>
		/// I32. int32 channel.
		/// <para>32λ������������Int32��</para>
		/// </summary>
		ChannelI32 = 14,
		/// <summary>
		/// I64. int64 channel.
		/// <para>64λ������������Int64��</para>
		/// </summary>
		ChannelI64 = 15
	}

	/// <summary>
	/// ���ظ�ʽģʽ
	/// </summary>
	public enum ZPixelFormatMode
	{
		/// <summary>
		/// ��Чģʽ��type==ZPFT_NONE��
		/// </summary>
		None = 0,
		/// <summary>
		/// ����ģʽ��(type==ZPFT_PACKET8) and (channelCount==0)��
		/// </summary>
		Indexed = 1,
		/// <summary>
		/// ���ģʽ��((type==ZPFT_PACKET8) and (channelCount!=0)) or (type in [ZPFT_PACKET16, ZPFT_PACKET64])��
		/// </summary>
		Packet = 2,
		/// <summary>
		/// ͨ��ģʽ��type in [ZPFT_CHANNELF16, ZPFT_CHANNELI64]��
		/// </summary>
		Channel = 3
	}

	/// <summary>
	/// ���ظ�ʽAlphaģʽ
	/// </summary>
	public enum ZPixelFormatAlphaMode
	{
		/// <summary>
		/// ��Ч��
		/// </summary>
		None = 0,
		/// <summary>
		/// ���ݡ�ͨ��3�������ݣ�������Alpha������CMYK��
		/// </summary>
		DataC3 = 1,
		/// <summary>
		/// Alpha��ͨ��3��Alphaͨ����
		/// </summary>
		Alpha = 2,
		/// <summary>
		/// Ԥ��Alpha��ͨ��3��Alphaͨ������ǰ����ͨ��������Ԥ�˴���
		/// </summary>
		PAlpha = 3
	}

	/// <summary>
	/// ZPixelFormatInfo��һ���ṹ�壬���ڽ���ZPixelFormat�е��ֶΡ�
	/// </summary>
	public struct ZPixelFormatInfo
	{
	}

}
