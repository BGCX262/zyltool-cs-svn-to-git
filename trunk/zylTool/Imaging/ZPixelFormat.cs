/*
 * ZPixelFormat.cs
 * ���ظ�ʽ����Ϣ��
 * @Author zyl910
 * 
 * [2011-10-03]
 * ���塣
 * 
 * [2011-10-04]
 * ������zpfMode��
 * 
 * [2011-10-05]
 * zpfChannelAll
 * zpfChannelBits����δʵ���㷨��
 * 
 * [2011-10-06]
 * zpfIsAuto
 * zpfIsFourCC
 * zpfIsNoSupport
 * zpfIsError
 * �޸�ZPixelFormatType�Ķ��壬������չΪ16�֡�
 * zpfCheck
 * zpfChannelN
 * 
 * [2011-10-07]
 * Ϊ����ǿת��λ�������unchecked��
 * zpfBetter��
 * �Ľ�typedef����
 * 
 * 
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
// ## typedef ##	// C#��֧��typedef��using���Ա�ģ������Ч
using ZPixelFormat = System.UInt32;

namespace zylTool.Imaging
{
	//public typedef UInt32 ZPixelFormat;

	/// <summary>
	/// ZPixelFormatTool ��һ����̬�࣬Ϊ����ZPixelFormat�ṩ���㡣
	/// </summary>
	/// <remarks>
	/// ZPixelFormat - ���ظ�ʽ��
	/// <para><c>public typedef UInt32 ZPixelFormat;</c></para>
	/// <para>�����ڱ������ظ���ͨ���ĳɷ���Ϣ����������ɫ�ռ����Ϣ��</para>
	/// <para>bits 0-5 = channel0	// ��0��ͨ������Ϣ��B����Ϊ����ɫģʽʱ��type==ZPFT_PACKET8, channelCount==0����channel0�����������ص���ռλ������Ϊ1��2��4��8��8���Ż�ΪZPFT_CHANNELI8����</para>
	/// <para>bits 6-11 = channel1	// ��1��ͨ������Ϣ��G</para>
	/// <para>bits 12-17 = channel2	// ��2��ͨ������Ϣ��R</para>
	/// <para>bits 18-23 = channel3	// ��3��ͨ������Ϣ��A</para>
	/// <para>bits 24-27 = type	// ���ظ�ʽ���͡���ZPixelFormatType��</para>
	/// <para>bits 28-29 = channelCount	// ռ�õ�ͨ������������������ͨ�������ڼ���ͨ��ģʽ�µ������ֽ�������ֵΪͨ����-1��ȡֵ��ΧΪ0~3����ʾ1~4��ͨ������DXGI_FORMAT_R16G16_UINTΪ(2-1)=1��</para>
	/// <para>bits 30-31 = alphaMode	// Alphaģʽ�����ZPixelFormatAlphaMode��</para>
	/// <para>bits 30 = dataC3	// ��3��ͨ���������ݡ����� (alpha==0, channelCount==4)ʱ��Ч��</para>
	/// <para>bits 30 = palpha	// Pre-multiplied alpha�����ظ�ʽ��������˵� alpha ֵ��</para>
	/// <para>bits 31 = alpha	// ͨ��3��Alphaͨ�����������˱�־ʱ�����Ը�ͨ����</para>
	/// <para></para>
	/// <para>��������</para>
	/// <para>1.������ģʽ��Ϊ�ڴ洦����ƣ�ʹ�ñ����ֽ���Byte Order�����ⲿ�洢ʱ��ע��ת���ֽ���</para>
	/// <para>2.��typeΪZPFT_PACKET64ʱ����ͨ���Ĳ�����Ӧ��[0~64]�ķ�Χ��������ֻ��6λ�����Ϊ63�����ǽ�63����Ϊ��Ч��ֵ�������ᵼ���޷�ʹ��λ��63����λ��63�ܺ������ɺ��ԡ�</para>
	/// </remarks>
	public static class ZPixelFormatTool
	{
		//public static ZPixelFormat px;	// ����usingָ�����͵ı���

		#region ZPixelFormat
		/// <summary>
		/// ���֧�ֵ�ͨ��������
		/// </summary>
		public const int ZPF_CHANNEL_MAXCOUNT = 4;

		/// <summary>
		/// ����ͨ�� ��λ����
		/// </summary>
		public const int ZPF_CHANNEL_BITS = 6;

		/// <summary>
		/// ͨ����λ����
		/// </summary>
		public const int ZPF_CHANNEL_AllBITS = 24;

		/// <summary>
		/// ͨ�������롣
		/// </summary>
		public const UInt32 ZPF_CHANNEL_AllMASK = 0xFFFFFF;

		/// <summary>
		/// channel0 ��λ�ơ�
		/// </summary>
		public const int ZPF_CHANNEL0_SHIFT = 0;
		/// <summary>
		/// channel0 �����롣
		/// </summary>
		public const UInt32 ZPF_CHANNEL0_MASK = 0x3F;

		/// <summary>
		/// channel1 ��λ�ơ�
		/// </summary>
		public const int ZPF_CHANNEL1_SHIFT = 6;
		/// <summary>
		/// channel1 �����롣
		/// </summary>
		public const UInt32 ZPF_CHANNEL1_MASK = 0xFC0;

		/// <summary>
		/// channel2 ��λ�ơ�
		/// </summary>
		public const int ZPF_CHANNEL2_SHIFT = 12;
		/// <summary>
		/// channel2 �����롣
		/// </summary>
		public const UInt32 ZPF_CHANNEL2_MASK = 0x3F000;

		/// <summary>
		/// channel3 ��λ�ơ�
		/// </summary>
		public const int ZPF_CHANNEL3_SHIFT = 18;
		/// <summary>
		/// channel3 �����롣
		/// </summary>
		public const UInt32 ZPF_CHANNEL3_MASK = 0xFC0000;

		/// <summary>
		/// ͨ����Ч��
		/// </summary>
		public const UInt32 ZPF_CHANNEL_INVALID = ZPF_CHANNEL0_MASK;

		/// <summary>
		/// type ��λ�ơ�
		/// </summary>
		public const int ZPF_TYPE_SHIFT = 24;
		/// <summary>
		/// type ��λ����
		/// </summary>
		public const int ZPF_TYPE_BITS = 4;
		/// <summary>
		/// type �����롣
		/// </summary>
		public const UInt32 ZPF_TYPE_MASK = 0x0F000000;

		/// <summary>
		/// channelCount ��λ�ơ�
		/// </summary>
		public const int ZPF_CHANNEL_COUNT_SHIFT = 28;
		/// <summary>
		/// channelCount ��λ����
		/// </summary>
		public const int ZPF_CHANNEL_COUNT_BITS = 2;
		/// <summary>
		/// channelCount �����롣
		/// </summary>
		public const UInt32 ZPF_CHANNEL_COUNT_MASK = 0x30000000;

		/// <summary>
		/// alphaMode ��λ�ơ�
		/// </summary>
		public const int ZPF_ALPHA_MODE_SHIFT = 30;
		/// <summary>
		/// alphaMode ��λ����
		/// </summary>
		public const int ZPF_ALPHA_MODE_BITS = 2;
		/// <summary>
		/// alphaMode �����롣
		/// </summary>
		public const UInt32 ZPF_ALPHA_MODE_MASK = 0xC0000000;

		/// <summary>
		/// alphaͨ����Ч��
		/// </summary>
		/// <seealso cref="ZPixelFormatAlphaMode.None"/>
		public const UInt32 ZPF_ALPHA_MODE_NONE = 0x00000000;
		/// <summary>
		/// ����ͨ����ͨ��3�������ݣ�������Alpha������CMYK��
		/// </summary>
		/// <seealso cref="ZPixelFormatAlphaMode.DataC3"/>
		public const UInt32 ZPF_ALPHA_MODE_DATAC3 = 0x40000000;
		/// <summary>
		/// Alpha��ͨ��3��Alphaͨ����
		/// </summary>
		/// <seealso cref="ZPixelFormatAlphaMode.Alpha"/>
		public const UInt32 ZPF_ALPHA_MODE_ALPHA = 0x80000000;
		/// <summary>
		/// Ԥ��Alpha��ͨ��3��Alphaͨ������ǰ����ͨ��������Ԥ�˴���
		/// </summary>
		/// <seealso cref="ZPixelFormatAlphaMode.PAlpha"/>
		public const UInt32 ZPF_ALPHA_MODE_PALPHA = 0xC0000000;

		#endregion	// ZPixelFormat

		#region ZPixelFormatType
		/// <summary>
		/// NO.
		/// <para>[ZPixelFormatType] ��Ч��</para>
		/// </summary>
		public const UInt32 ZPFT_NONE = 0;
		/// <summary>
		/// P8. 8bit packet(channelCount!=0), or palette indexes(channelCount==0).
		/// <para>[ZPixelFormatType] ���8λ��Byte��</para>
		/// </summary>
		public const UInt32 ZPFT_PACKET8 = 1;
		/// <summary>
		/// P16. 16bit packet.
		/// <para>[ZPixelFormatType] ���16λ��UInt16��</para>
		/// </summary>
		public const UInt32 ZPFT_PACKET16 = 2;
		/// <summary>
		/// P32. 32bit packet.
		/// <para>[ZPixelFormatType] ���32λ��UInt32��</para>
		/// </summary>
		public const UInt32 ZPFT_PACKET32 = 3;
		/// <summary>
		/// P64. 64bit packet.
		/// <para>[ZPixelFormatType] ���64λ��UInt64��</para>
		/// </summary>
		public const UInt32 ZPFT_PACKET64 = 4;
		/// <summary>
		/// F16. float16 channel.
		/// <para>[ZPixelFormatType] 16λ���㡣s10e5, binary16(IEEE 754-2008), FP16(DirectX)��</para>
		/// </summary>
		public const UInt32 ZPFT_CHANNELF16 = 5;
		/// <summary>
		/// F32. float32 channel.
		/// <para>[ZPixelFormatType] 32λ���㡣Single��s23e8, binary32(IEEE 754-2008), FP32(DirectX)��</para>
		/// </summary>
		public const UInt32 ZPFT_CHANNELF32 = 6;
		/// <summary>
		/// F64. float64 channel.
		/// <para>[ZPixelFormatType] 64λ���㡣Double��s52e11, binary64(IEEE 754-2008), FP64(DirectX)��</para>
		/// </summary>
		public const UInt32 ZPFT_CHANNELF64 = 7;
		/// <summary>
		/// U8. byte channel.
		/// <para>[ZPixelFormatType] 8λ�޷���������Byte��</para>
		/// </summary>
		public const UInt32 ZPFT_CHANNELU8 = 8;
		/// <summary>
		/// U16. uint16 channel.
		/// <para>[ZPixelFormatType] 16λ�޷���������UInt16��</para>
		/// </summary>
		public const UInt32 ZPFT_CHANNELU16 = 9;
		/// <summary>
		/// U32. uint32 channel.
		/// <para>[ZPixelFormatType] 32λ�޷���������UInt32��</para>
		/// </summary>
		public const UInt32 ZPFT_CHANNELU32 = 10;
		/// <summary>
		/// U64. uint64 channel.
		/// <para>[ZPixelFormatType] 64λ�޷���������UInt64��</para>
		/// </summary>
		public const UInt32 ZPFT_CHANNELU64 = 11;
		/// <summary>
		/// I8. sbyte channel.
		/// <para>[ZPixelFormatType] 8λ������������SByte��</para>
		/// </summary>
		public const UInt32 ZPFT_CHANNELI8 = 12;
		/// <summary>
		/// I16. int16 channel.
		/// <para>[ZPixelFormatType] 16λ������������Int16��</para>
		/// </summary>
		public const UInt32 ZPFT_CHANNELI16 = 13;
		/// <summary>
		/// I32. int32 channel.
		/// <para>[ZPixelFormatType] 32λ������������Int32��</para>
		/// </summary>
		public const UInt32 ZPFT_CHANNELI32 = 14;
		/// <summary>
		/// I64. int64 channel.
		/// <para>[ZPixelFormatType] 64λ������������Int64��</para>
		/// </summary>
		public const UInt32 ZPFT_CHANNELI64 = 15;

		#endregion	// ZPixelFormatType

		#region ZPixelFormatMode
		/// <summary>
		/// ��Чģʽ��type==ZPFT_NONE��
		/// </summary>
		public const UInt32 ZPFM_NONE = 0;
		/// <summary>
		/// ����ģʽ��(type==ZPFT_PACKET8) and (channelCount==0)��
		/// </summary>
		public const UInt32 ZPFM_INDEXED = 1;
		/// <summary>
		/// ���ģʽ��((type==ZPFT_PACKET8) and (channelCount!=0)) or (type in [ZPFT_PACKET16, ZPFT_PACKET64])��
		/// </summary>
		public const UInt32 ZPFM_PACKET = 2;
		/// <summary>
		/// ͨ��ģʽ��type in [ZPFT_CHANNELF16, ZPFT_CHANNELI64]��
		/// </summary>
		public const UInt32 ZPFM_CHANNEL = 3;

		#endregion	// ZPixelFormatMode

		#region ZPixelFormatAlphaMode
		/// <summary>
		/// ��Ч��
		/// </summary>
		public const UInt32 ZPFAM_NONE = 0;
		/// <summary>
		/// ���ݡ�ͨ��3�������ݣ�������Alpha������CMYK��
		/// </summary>
		public const UInt32 ZPFAM_DATAC3 = 1;
		/// <summary>
		/// Alpha��ͨ��3��Alphaͨ����
		/// </summary>
		public const UInt32 ZPFAM_ALPHA = 2;
		/// <summary>
		/// Ԥ��Alpha��ͨ��3��Alphaͨ������ǰ����ͨ��������Ԥ�˴���
		/// </summary>
		public const UInt32 ZPFAM_PALPHA = 3;

		#endregion	// ZPixelFormatAlphaMode

		#region const
		/// <summary>
		/// �Զ���
		/// </summary>
		public static readonly ZPixelFormat Auto = zpfMake(ZPFT_NONE, 1, 0, 0, 0, 0, 0);
		/// <summary>
		/// ��FourCC��ʽ��
		/// </summary>
		public static readonly ZPixelFormat FourCC = zpfMake(ZPFT_NONE, 2, 0, 0, 0, 0, 0);
		/// <summary>
		/// ��֧�֡�
		/// </summary>
		public static readonly ZPixelFormat NoSupport = zpfMake(ZPFT_NONE, 3, 0, 0, 0, 0, 0);
		/// <summary>
		/// ����
		/// </summary>
		public static readonly ZPixelFormat Error = zpfMake(ZPFT_NONE, 4, 0, 0, 0, 0, 0);

		/// <summary>
		/// �ǲ����Զ���
		/// </summary>
		/// <param name="pf">ZPixelFormat������</param>
		/// <returns>���Զ��ͷ���true�����򷵻�false��</returns>
		public static bool zpfIsAuto(ZPixelFormat pf)
		{
			if (ZPFT_NONE != zpfType(pf)) return false;
			return 1 == zpfChannelCount(pf);
		}

		/// <summary>
		/// �ǲ���FourCC��
		/// </summary>
		/// <param name="pf">ZPixelFormat������</param>
		/// <returns>��FourCC�ͷ���true�����򷵻�false��</returns>
		public static bool zpfIsFourCC(ZPixelFormat pf)
		{
			if (ZPFT_NONE != zpfType(pf)) return false;
			return 2 == zpfChannelCount(pf);
		}

		/// <summary>
		/// �ǲ��ǲ�֧�֡�
		/// </summary>
		/// <param name="pf">ZPixelFormat������</param>
		/// <returns>�ǲ�֧�־ͷ���true�����򷵻�false��</returns>
		public static bool zpfIsNoSupport(ZPixelFormat pf)
		{
			if (ZPFT_NONE != zpfType(pf)) return false;
			return 3 == zpfChannelCount(pf);
		}

		/// <summary>
		/// �ǲ��Ǵ���
		/// </summary>
		/// <param name="pf">ZPixelFormat������</param>
		/// <returns>�Ǵ���ͷ���true�����򷵻�false��</returns>
		public static bool zpfIsError(ZPixelFormat pf)
		{
			if (ZPFT_NONE != zpfType(pf)) return false;
			return 4 == zpfChannelCount(pf);
		}

		#endregion	// const

		/// <summary>
		/// ����ZPixelFormat��
		/// </summary>
		/// <param name="type">���͡�ֵΪ <see cref="ZPFT_NONE"/> - <see cref="ZPFT_CHANNELI64"/>��</param>
		/// <param name="channelCount">ռ�õ�ͨ������������������ͨ�������ڼ���ͨ��ģʽ�µ������ֽ�������ֵΪ 1 - 4��</param>
		/// <param name="alphaMode">Alphaģʽ��ֵΪ <see cref="ZPFAM_NONE"/> - <see cref="ZPFAM_PALPHA"/>��</param>
		/// <param name="channel0">ͨ��0�Ĳ�����ZPFM_INDEXEDģʽ��������λ��1/2/4��ZPFM_PACKETģʽ���Ǹ�ͨ������ʼλ�ơ�ZPFM_CHANNELģʽ���Ǹ�ͨ����������ֵ�������ֽ�����Ҳ����λ������</param>
		/// <param name="channel1">ͨ��1�Ĳ�����ZPFM_PACKETģʽ���Ǹ�ͨ������ʼλ�ơ�ZPFM_CHANNELģʽ���Ǹ�ͨ����������ֵ��</param>
		/// <param name="channel2">ͨ��2�Ĳ�����ZPFM_PACKETģʽ���Ǹ�ͨ������ʼλ�ơ�ZPFM_CHANNELģʽ���Ǹ�ͨ����������ֵ��</param>
		/// <param name="channel3">ͨ��3�Ĳ�����ZPFM_PACKETģʽ���Ǹ�ͨ������ʼλ�ơ�ZPFM_CHANNELģʽ���Ǹ�ͨ����������ֵ��</param>
		/// <returns>����ZPixelFormat��ʽ�ı�����</returns>
		/// <remarks>�ú�������Debugʱ��������Ƿ����������������ʱ��飬������������Ч�ԡ�</remarks>
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
		/// ��������ɫģʽ�µ�ZPixelFormat��
		/// </summary>
		/// <param name="bitsPixel">���ص�λ����ֻ����1/2/4/8��</param>
		/// <returns>����ZPixelFormat��ʽ�ı�����<seealso cref="ZPixelFormatTool.NoSupport"/>��ʾʧ�ܡ�</returns>
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
					//Ϊ�����ܣ�������ʹ���쳣�����Ǹ��ݷ���ֵ�жϡ�
					break;
			}
			return NoSupport;
		}

		/// <summary>
		/// ��ZPixelFormat��������ֶΡ�
		/// </summary>
		/// <param name="pf">ZPixelFormat������</param>
		/// <param name="type">���͡�</param>
		/// <param name="channelCount">ռ�õ�ͨ��������</param>
		/// <param name="alphaMode">Alphaģʽ��</param>
		/// <param name="channel0">ͨ��0�Ĳ�����</param>
		/// <param name="channel1">ͨ��1�Ĳ�����</param>
		/// <param name="channel2">ͨ��2�Ĳ�����</param>
		/// <param name="channel3">ͨ��3�Ĳ�����</param>
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
		/// ��ZPixelFormat��� type �ֶΡ�
		/// </summary>
		/// <param name="pf">ZPixelFormat������</param>
		/// <returns>��������ݡ�</returns>
		public static UInt32 zpfType(ZPixelFormat pf)
		{
			unchecked
			{
				return (pf & ZPF_TYPE_MASK) >> ZPF_TYPE_SHIFT;
			}
		}

		/// <summary>
		/// ��ZPixelFormat��� channelCount �ֶΡ�
		/// </summary>
		/// <param name="pf">ZPixelFormat������</param>
		/// <returns>��������ݡ�</returns>
		public static UInt32 zpfChannelCount(ZPixelFormat pf)
		{
			unchecked
			{
				return 1 + ((pf & ZPF_CHANNEL_COUNT_MASK) >> ZPF_CHANNEL_COUNT_SHIFT);
			}
		}

		/// <summary>
		/// ��ZPixelFormat��� alphaMode �ֶΡ�
		/// </summary>
		/// <param name="pf">ZPixelFormat������</param>
		/// <returns>��������ݡ�</returns>
		public static UInt32 zpfAlphaMode(ZPixelFormat pf)
		{
			unchecked
			{
				return (pf & ZPF_ALPHA_MODE_MASK) >> ZPF_ALPHA_MODE_SHIFT;
			}
		}

		/// <summary>
		/// ��ZPixelFormat��� channel0 �ֶΡ�
		/// </summary>
		/// <param name="pf">ZPixelFormat������</param>
		/// <returns>��������ݡ�</returns>
		public static UInt32 zpfChannel0(ZPixelFormat pf)
		{
			unchecked
			{
				return (pf & ZPF_CHANNEL0_MASK) >> ZPF_CHANNEL0_SHIFT;
			}
		}

		/// <summary>
		/// ��ZPixelFormat��� channel1 �ֶΡ�
		/// </summary>
		/// <param name="pf">ZPixelFormat������</param>
		/// <returns>��������ݡ�</returns>
		public static UInt32 zpfChannel1(ZPixelFormat pf)
		{
			unchecked
			{
				return (pf & ZPF_CHANNEL1_MASK) >> ZPF_CHANNEL1_SHIFT;
			}
		}

		/// <summary>
		/// ��ZPixelFormat��� channel2 �ֶΡ�
		/// </summary>
		/// <param name="pf">ZPixelFormat������</param>
		/// <returns>��������ݡ�</returns>
		public static UInt32 zpfChannel2(ZPixelFormat pf)
		{
			unchecked
			{
				return (pf & ZPF_CHANNEL2_MASK) >> ZPF_CHANNEL2_SHIFT;
			}
		}

		/// <summary>
		/// ��ZPixelFormat��� channel3 �ֶΡ�
		/// </summary>
		/// <param name="pf">ZPixelFormat������</param>
		/// <returns>��������ݡ�</returns>
		public static UInt32 zpfChannel3(ZPixelFormat pf)
		{
			unchecked
			{
				return (pf & ZPF_CHANNEL3_MASK) >> ZPF_CHANNEL3_SHIFT;
			}
		}

		/// <summary>
		/// ��ZPixelFormat�����i��ͨ�����ֶΡ�
		/// </summary>
		/// <param name="pf">ZPixelFormat������</param>
		/// <param name="i">��i��ͨ��</param>
		/// <returns>��������ݡ�</returns>
		public static UInt32 zpfChannelN(ZPixelFormat pf, int i)
		{
			unchecked
			{
				return (pf >> (ZPF_CHANNEL0_SHIFT * i)) & ZPF_CHANNEL0_MASK;
			}
		}

		/// <summary>
		/// ��������ظ�ʽ����ռλ����
		/// </summary>
		/// <param name="pf">ZPixelFormat������</param>
		/// <returns>������ռλ����</returns>
		public static int zpfPixelBits(ZPixelFormat pf)
		{
			unchecked
			{
				int channelCount = (int)zpfChannelCount(pf);
				UInt32 type = zpfType(pf);
				switch (type)
				{
					case ZPFT_PACKET8:
						if (1 == channelCount)	// ����ɫģʽ
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
		/// ��������ظ�ʽ����ռ�ֽ�����
		/// </summary>
		/// <param name="pf">ZPixelFormat������</param>
		/// <returns>������ռ�ֽ�����ע������ģʽ�¿��ܻ᷵��0��</returns>
		public static int zpfPixelBytes(ZPixelFormat pf)
		{
			return zpfPixelBits(pf) / 8;
		}

		/// <summary>
		/// ȡ��ZPixelFormat��ģʽ��
		/// </summary>
		/// <param name="pf">ZPixelFormat������</param>
		/// <returns>����ZPixelFormat��ģʽ��ֵΪ <see cref="ZPFM_NONE"/> - <see cref="ZPFM_CHANNEL"/>��</returns>
		public static UInt32 zpfMode(ZPixelFormat pf)
		{
			UInt32 type = zpfType(pf);
			switch (type)
			{
				case ZPFT_PACKET8:
					if (1 == zpfChannelCount(pf))	// ����ɫģʽ
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
		/// ͬʱȡ��4��ͨ������ֵ��ֱ����ʾԭʼֵ������������
		/// </summary>
		/// <param name="arrChannel">һ�������������ڽ���4��ͨ������ֵ��</param>
		/// <param name="pf">ZPixelFormat������</param>
		/// <remarks>
		/// ZPFM_INDEXEDģʽ�£���0��Ԫ��������λ��1/2/4��
		/// <para>ZPFM_PACKETģʽ�£��Ǹ�ͨ������ʼλ�ơ�</para>
		/// <para>ZPFM_CHANNELģʽ�£��Ǹ�ͨ����������ֵ��</para>
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
		/// ����ÿ��ͨ����λ������ZPFM_PACKETģʽ���ر����á�
		/// </summary>
		/// <param name="arrChannel">һ�������������ڽ���4��ͨ����λ����</param>
		/// <param name="pf">ZPixelFormat������</param>
		public static void zpfChannelBits(ref UInt32[] arrChannel, ZPixelFormat pf)
		{
			UInt32 bitsPacket = 0;	// ���ģʽ����λ��
			UInt32 bits = 0;
			UInt32 type = zpfType(pf);
			switch (type)
			{
				case ZPFT_PACKET8:
					if (1 == zpfChannelCount(pf))	// ����ģʽ
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
			UInt32 nInvalid;	// ��Ч��ֵ��
			if (bitsPacket > 0)
			{
				// == ���ģʽ ==
				UInt32[] arrShift = new UInt32[ZPF_CHANNEL_MAXCOUNT];
				zpfChannelAll(ref arrShift, pf);
				nInvalid = bitsPacket;	// ��Ч��ֵ�����ݵ�ǰλ�����¼��㡣
				if (nInvalid > ZPF_CHANNEL_INVALID) nInvalid = ZPF_CHANNEL_INVALID;
				for (int i = 0; i < ZPF_CHANNEL_MAXCOUNT; ++i)
				{
					UInt32 nShift = arrShift[i];	// ��ǰͨ����λ��
					UInt32 nNextShift = nShift;	// ����λͨ����λ�ơ���ǰͨ����λ�� = nNextShift - nShift
					if (nShift < nInvalid)	// ��ǰͨ����Ч
					{
						nNextShift = bitsPacket;
						// ���ǰ��ġ�1.��ø���λͨ����λ�ƣ�2.�ж��Ƿ��ù���λ����ֵ��
						for (int j = 0; j < i; ++j)
						{
							UInt32 nj = arrShift[j];
							if (nShift == nj)
							{
								nNextShift = nShift;	// ��λ����ֵ�ѱ�ռ�ã����Ա�ͨ����λ��Ϊ0��
								continue;
							}
							// ���� ����λͨ����λ��
							if ((nj < nInvalid) && (nj < nNextShift))
							{
								nNextShift = nj;
							}
						}
						// �ټ������
						if (nNextShift > nShift)
						{
							for (int j = i + 1; j < ZPF_CHANNEL_MAXCOUNT; ++j)
							{
								UInt32 nj = arrShift[j];
								// ���� ����λͨ����λ��
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
			// == ͨ��ģʽ ==
			nInvalid = zpfChannelCount(pf);	// ��Ч��ֵ��Ϊͨ����Ŀ��
			for (int i = 0; i < ZPF_CHANNEL_MAXCOUNT; ++i)
			{
				UInt32 n = 0;
				if (zpfChannelN(pf,i) < nInvalid)	// ��Чͨ��
				{
					n = bits;
				}
				arrChannel[i] = n;
			}
		}

		/// <summary>
		/// ���ZPixelFormat�Ƿ���Ч��
		/// </summary>
		/// <param name="pf">ZPixelFormat������</param>
		/// <returns>����Ч���ͷ���true�����򷵻�false��</returns>
		public static bool zpfCheck(ZPixelFormat pf)
		{
			unchecked
			{
				UInt32 mode = zpfMode(pf);
				UInt32 nInvalid;	// ��Ч��ֵ��
				if (ZPFM_INDEXED == mode)
				{
					UInt32 bits = zpfChannel0(pf);
					switch (bits)
					{
						case 1:
						case 2:
						case 4:
						case 8:	// ��Ȼ�������Ż���������
							return true;
					}
				}
				else if (ZPFM_PACKET == mode)
				{
					nInvalid = (uint)zpfPixelBits(pf);	// ��Ч��ֵ������λ����
					if (nInvalid > ZPF_CHANNEL_INVALID) nInvalid = ZPF_CHANNEL_INVALID;
					for (int i = 0; i < ZPF_CHANNEL_MAXCOUNT; ++i)
					{
						UInt32 n = zpfChannelN(pf, i);
						if ((n >= 0) && (n < nInvalid))
							return true;	// ���ٴ���һ����Ч��ͨ��
					}
				}
				else if (ZPFM_CHANNEL == mode)
				{
					nInvalid = zpfChannelCount(pf);	// ��Ч��ֵ������ͨ��������
					for (int i = 0; i < ZPF_CHANNEL_MAXCOUNT; ++i)
					{
						UInt32 n = zpfChannelN(pf, i);
						if ((n >= 0) && (n < nInvalid))
							return true;	// ���ٴ���һ����Ч��ͨ��
					}
				}
				return false;
			}
		}

		/// <summary>
		/// �Ż�ZPixelFormat�����Խ�����ģʽ�ʹ��ģʽת��Ϊͨ��ģʽ��
		/// </summary>
		/// <param name="pf">ZPixelFormat������</param>
		/// <returns>�����Ż���Ľ������������Ż����ͷ���ԭֵ��</returns>
		public static ZPixelFormat zpfBetter(ZPixelFormat pf)
		{
			unchecked
			{
				UInt32 type = zpfType(pf);
				if ((ZPFT_PACKET8 <= type) && (type <= ZPFT_PACKET64))	// ����ģʽ�ʹ��ģʽ
				{
					int channelCount = (int)zpfChannelCount(pf);
					if ((ZPFT_PACKET8 == type) && (1 == channelCount))	// ����ɫģʽ
					{
						if (8 == zpfChannel0(pf))	// 8λ����תΪZPFT_CHANNELU8��
						{
							return zpfMakeIndexed(8);
						}
					}
					else
					{
						// ��ȡ��������
						UInt32[] arrShift = new UInt32[ZPF_CHANNEL_MAXCOUNT];	// λ��
						zpfChannelAll(ref arrShift, pf);
						UInt32[] arrBits = new UInt32[ZPF_CHANNEL_MAXCOUNT];	// λ��
						zpfChannelBits(ref arrBits, pf);
						uint pixelBits = (uint)zpfPixelBits(pf);
						uint nInvalid = pixelBits;	// ��Ч��ֵ������λ����
						if (nInvalid > ZPF_CHANNEL_INVALID) nInvalid = ZPF_CHANNEL_INVALID;

						// ���
						int cntChannel = 0;	// ���õ���ͨ������
						UInt32 maxShift = 0;	// ������Чλ��
						bool[] isAlign = new bool[4] { true, true, true, true };	// �ܱ� 8/16/32/64 ����
						UInt32 channelBits;	// ����ͨ����λ����8/16/32/64
						for (int i = 0; i < ZPF_CHANNEL_MAXCOUNT; ++i)
						{
							if (arrShift[i] < nInvalid)	// ��Ч��λ��
							{
								++cntChannel;
								if (maxShift < arrShift[i]) maxShift = arrShift[i];
								channelBits = 8;	// ������8/16/32/64
								for (int j = 0; j < 4; ++j)
								{
									isAlign[j] &= (0 == (arrShift[i] % channelBits));	//λ���Ƿ��ܱ� channelBits ����
									isAlign[j] &= (channelBits == arrBits[i]);	//λ���Ƿ��� channelBits ���
									// next
									channelBits *= 2;	// 8/16/32/64
								}
							}
						}

						// �����Ż�
						if (cntChannel > 0)
						{
							UInt32[] arr = new UInt32[ZPF_CHANNEL_MAXCOUNT];	// �µ�ͨ��ֵ��ͨ��ģʽ����������
							UInt32 alphaMode = zpfAlphaMode(pf);	// �̳�ԭ����Alphaģʽ
							UInt32 typeNew = ZPFT_PACKET64;	// �µ�����
							channelBits = 64;	// �ȳ���64λ���ٳ��Ե�λ
							for (int i = 3; i >= 0; --i)
							{
								if (isAlign[i] && (channelBits <= pixelBits))
								{
									// ���ͨ������
									UInt32 cntNew;	// �µ�ͨ������
									cntNew = pixelBits / channelBits;
									if (cntNew > ZPF_CHANNEL_MAXCOUNT) continue;	// ͨ���������ܳ���ZPF_CHANNEL_MAXCOUNT
									if ((maxShift / channelBits) >= cntNew) continue;	// ��������ͨ����
									// �����µ�����
									for (int j = 0; j < ZPF_CHANNEL_MAXCOUNT; ++j)
									{
										UInt32 n = arrShift[j];
										if (n < nInvalid)	// ��Ч��λ��
										{
											// תΪ����
											n = n / channelBits;
											//if (Environment.
											if (!BitConverter.IsLittleEndian)
											{
												// ��˷�ʽ
												n = (cntNew - 1) - n;
											}
										}
										else
										{
											// ��Ч��λ��
											n = ZPF_CHANNEL_INVALID;
										}
										arr[j] = n;
									}
									// ����
									//UInt32 cntNew = 1 + (maxShift / channelBits);	// �µ�ͨ������
									return zpfMake(typeNew, cntNew, alphaMode, arr[0], arr[1], arr[2], arr[3]);
								}
								// Next
								typeNew -= 1;	// ���� ZPFT_PACKET8~ZPFT_PACKET64 �������ԡ�
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
