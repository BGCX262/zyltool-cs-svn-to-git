/*
 *ColorPixel.cs
 * ɫ�ʿռ�����ظ�ʽ��
 * @Author zyl910
 * 
 * [2011-10-06]
 * ���塣
 * 
 * [2011-10-07]
 * �Ľ�typedef����
 * 
 */
using System;
using System.Collections.Generic;
//using System.Text;
using System.Runtime.InteropServices;
// ## typedef ##	// C#��֧��typedef��using���Ա�ģ������Ч
using ZPixelFormat = System.UInt32;

namespace zylTool.Imaging
{
	/// <summary>
	/// ColorPixel��һ���ṹ�壬���ڴ洢 ɫ�ʿռ�����ظ�ʽ ��Ϣ��
	/// </summary>
	/// <remarks>
	/// ��һ������£�<see cref="colorSpace"/>��Ч��<see cref="fourCC"/>��Ч��
	/// <para>�����ظ�ʽΪ<seealso cref="ZPixelFormatTool.FourCC"/>ʱ��<see cref="colorSpace"/>��Ч��<see cref="fourCC"/>��Ч��</para>
	/// </remarks>
	[StructLayout(LayoutKind.Explicit)]
	public struct ColorPixel
	{
		/// <summary>
		/// ɫ�ʿռ䡣
		/// </summary>
		/// <remarks>
		/// �����ظ�ʽ����<seealso cref="ZPixelFormatTool.FourCC"/>ʱ�����ֶ���Ч�������������Ч��
		/// </remarks>
		[FieldOffset(0)]
		public ColorSpace colorSpace;

		/// <summary>
		/// FourCC��
		/// </summary>
		/// <remarks>
		/// �����ظ�ʽΪ<seealso cref="ZPixelFormatTool.FourCC"/>ʱ�����ֶ���Ч�������������Ч��
		/// </remarks>
		[FieldOffset(0)]
		public uint fourCC;

		/// <summary>
		/// ���ظ�ʽ��
		/// </summary>
		/// <remarks>
		/// ��һ������£�<see cref="colorSpace"/>��Ч��<see cref="fourCC"/>��Ч��
		/// <para>�����ظ�ʽΪ<seealso cref="ZPixelFormatTool.FourCC"/>ʱ��<see cref="colorSpace"/>��Ч��<see cref="fourCC"/>��Ч��</para>
		/// </remarks>
		[FieldOffset(4)]
		public ZPixelFormat pixelFormat;

		/// <summary>
		/// ��ָ���� ɫ�ʿռ�����ظ�ʽ ��ʼ�� ColorPixel �ṹ�� 
		/// </summary>
		/// <param name="colorSpace"></param>
		/// <param name="pixelFormat"></param>
		public ColorPixel(ColorSpace colorSpace, ZPixelFormat pixelFormat)
		{
			this.fourCC = 0;	// Ϊ��ͨ������
			this.colorSpace = colorSpace;
			this.pixelFormat = pixelFormat;
		}

		/// <summary>
		/// ��ָ���� FourCC���� ��ʼ�� ColorPixel �ṹ�� 
		/// </summary>
		/// <param name="fourCC">FourCC���֡�</param>
		public ColorPixel(uint fourCC)
		{
			this.colorSpace = ColorSpace.Default;	// Ϊ��ͨ������
			this.fourCC = fourCC;
			this.pixelFormat = ZPixelFormatTool.FourCC;
		}

		/// <summary>
		/// ��ָ���� FourCC�ַ��� ��ʼ�� ColorPixel �ṹ�� 
		/// </summary>
		/// <param name="fourCC">FourCC�ַ�����</param>
		public ColorPixel(string fourCC)
		{
			this.colorSpace = ColorSpace.Default;	// Ϊ��ͨ������
			this.fourCC = FourCCTool.fromString(fourCC);
			this.pixelFormat = ZPixelFormatTool.FourCC;
		}

		/// <summary>
		/// �ǲ���FourCC��
		/// </summary>
		/// <returns>��FourCC�ͷ���true�����򷵻�false��</returns>
		public bool isFourCC()
		{
			return ZPixelFormatTool.zpfIsFourCC(pixelFormat);
		}

	}
}
