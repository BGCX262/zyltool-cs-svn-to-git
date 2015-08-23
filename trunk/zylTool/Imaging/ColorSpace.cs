/*
 * ColorSpace.cs
 * ɫ�ʿռ�Ķ��弰����������
 * @Author zyl910
 * 
 * [2011-10-05]
 * ���塣
 * 
 * [2011-10-06]
 * ��ColorSpaceö�ٸ�Ϊ�̳���uint��Ϊ����FourCC���ݡ�
 * ColorSpaceTool.isColor(uint)
 * class ColorSpaceName
 * ColorSpaceTool.Names
 * ColorSpaceTool.newKVPName
 * ColorSpaceTool.findName
 * 
 * 
 */
using System;
using System.Collections.Generic;
//using System.Text;

namespace zylTool.Imaging
{
	/// <summary>
	/// ɫ�ʿռ�
	/// </summary>
	/// <remarks>
	/// ColorSpace��Ϊ���࣬һ����������ɫ�ʿռ䣬��������ɫ�ʣ���һ���Ƿ�ɫ������ģʽ�����ڴ��ͼ��ѧ���������ݡ�
	/// <para>ע�⣺</para>
	/// <para>1.���ڵ��������ɫ������ģʽ��С��0���Ƿ�ɫ������ģʽ��</para>
	/// <para>2.����ɫ������ģʽ֮�����ɫ�ʿռ�ת����</para>
	/// <para>3.Դ��Ŀ�����һһ���Ƿ�ɫ������ģʽʱ��������ɫ�ʿռ�ת�������������ݣ����൱��ɫ�ʿռ䶼��RGB����</para>
	/// <para>4.Indexed��VectorIndexed��ʾ����ģʽ���ڽ������ݸ���ʱ����������ֵ��������ֵ���нضϴ�������ͨ��תΪ����ʱ�������俴��8λ������</para>
	/// <para>5.IndexedΪ����ɫģʽ��������ΪԴ�ռ䡣����Ŀ��ռ��δ�ṩ��ɫ��ʱ������Gray������</para>
	/// <para>6.Gray��ΪԴ�ռ�ʱ���᳢�Ը�Ŀ��ռ��ǰ3������䡣����Ŀ��ռ�ʱ��ֻȡԴ�ռ���׸�ͨ������ֵ��</para>
	/// <para>7.����ɫ�ʿռ䶼������Alphaͨ�����Ѿ�ռ����4���ĳ��⣬��CMYK�������Զ�ʡ���ˡ�A����ĸ������Alphaͨ�������ض���ͨ��3��</para>
	/// <para>��ֵ�淶����</para>
	/// <para>1.�����޷�����������ʾ[0.0, 1.0]��</para>
	/// <para>2.���ڴ�����������[0,���ֵ]��ʾ[0.0, 1.0]��[��Сֵ, 0]��ʾ[-1.0, 0.0]��</para>
	/// <para>ɫ��ת���ĸ�ʽ֧�ֵ�����˳��</para>
	/// <para>1.ZPFT_CHANNELF32��</para>
	/// <para>2.ZPFT_CHANNELI8��</para>
	/// <para>3.ZPFT_CHANNELI16��</para>
	/// <para>4.�����ŵ�ZPFT_CHANNELI8��</para>
	/// <para>4.�����ŵ�ZPFT_CHANNELI16��</para>
	/// <para>4.������</para>
	/// </remarks>
	public enum ColorSpace: uint	// ����32λ�޷�����
	{
		RGB = 0,
		Indexed = 1,
		Gray = 2,
		YCbCr = 3,
		CIEXYZ,
		CIELab,
		HSV,
		CMYK,

		Vector = 0x80000000u,
		VectorIndexed,	// 0x80000001u
		XYZW,
		UVWA,
		DepthStencil,

		Default = RGB
	}

	/// <summary>
	/// ColorSpaceTool ��һ����̬�࣬Ϊ���� <see cref="ColorSpace"/> �ṩ���㡣
	/// </summary>
	public static class ColorSpaceTool
	{
		/// <summary>
		/// ��ColorSpace�Ƿ���ɫ������ģʽ��
		/// </summary>
		/// <param name="cs"><seealso cref="ColorSpace"/>������</param>
		/// <returns>����ColorSpace��ɫ������ģʽʱ������true�������Ƿ�ɫ������ģʽ������false��</returns>
		public static bool isColor(ColorSpace cs)
		{
			return (cs < ColorSpace.Vector);
		}

		/// <summary>
		/// ��ColorSpace�Ƿ���ɫ������ģʽ��
		/// </summary>
		/// <param name="cs">��������ʽ�洢��<seealso cref="ColorSpace"/>������</param>
		/// <returns>����ColorSpace��ɫ������ģʽʱ������true�������Ƿ�ɫ������ģʽ������false��</returns>
		public static bool isColor(uint cs)
		{
			return (cs < 0x80000000u);
		}

		#region Names
		private static KeyValuePair<ColorSpace, ColorSpaceName> newKVPName(ColorSpace colorSpace, string name, string nameShort, string nameC0, string nameC1, string nameC2, string nameC3, string description)
		{
			return KeyValuePairs.newKVP(colorSpace, new ColorSpaceName(colorSpace, name, nameShort, nameC0, nameC1, nameC2, nameC3, description));
		}

		/// <summary>
		/// ��ɫ�ռ����Ƶ����顣
		/// </summary>
		/// <remarks>
		/// �벻Ҫ�޸ĸ����顣
		/// </remarks>
		public static readonly KeyValuePair<ColorSpace, ColorSpaceName>[] Names = {
			newKVPName(ColorSpace.RGB, "RGB", "RGB", "R", "G", "B", "A", string.Empty),
			newKVPName(ColorSpace.Indexed, "Indexed", "Indexed", "I", "X1", "X2", "A", string.Empty),
			newKVPName(ColorSpace.Gray, "Gray", "Gray", "G", "X1", "X2", "A", string.Empty),
			newKVPName(ColorSpace.YCbCr, "YCbCr", "YCbCr", "Y", "Cb", "Cr", "A", string.Empty),
			newKVPName(ColorSpace.CIEXYZ, "CIEXYZ", "XYZ", "X", "Y", "Z", "A", string.Empty),
			newKVPName(ColorSpace.CIELab, "CIELab", "Lab", "L", "a", "b", "A", string.Empty),
			newKVPName(ColorSpace.HSV, "HSV", "HSV", "H", "S", "V", "A", string.Empty),
			newKVPName(ColorSpace.CMYK, "CMYK", "CMYK", "C", "M", "Y", "K", string.Empty),

			newKVPName(ColorSpace.Vector, "Vector", "Vector", "V0", "V1", "V2", "V3", string.Empty),
			newKVPName(ColorSpace.VectorIndexed, "VectorIndexed", "VectorIndexed", "I0", "I1", "I2", "I3", string.Empty),
			newKVPName(ColorSpace.XYZW, "XYZW", "XYZW", "X", "Y", "Z", "W", string.Empty),
			newKVPName(ColorSpace.UVWA, "UVWA", "UVWA", "U", "V", "W", "A", string.Empty),
			newKVPName(ColorSpace.DepthStencil, "DepthStencil", "DepthStencil", "Depth", "Stencil", "X2", "A", string.Empty)
		};

		/// <summary>
		/// ���� <seealso cref="ColorSpace"/> ��������Ϣ��
		/// </summary>
		/// <param name="cs">ɫ�ʿռ� <seealso cref="ColorSpace"/>������</param>
		/// <returns>����������Ϣ��</returns>
		public static ColorSpaceName findName(ColorSpace cs)
		{
			return KeyValuePairs.find(Names, cs);
		}

		#endregion	// Names

	}

	/// <summary>
	/// ColorSpaceName ��һ�������ࣨimmutable pattern������ģʽ�������ڵõ� <see cref="ColorSpace"/> �����Ƶ���Ϣ��
	/// </summary>
	public sealed class ColorSpaceName
	{
		/// <summary>
		/// ɫ�ʿռ䡣
		/// </summary>
		public readonly ColorSpace colorSpace;
		/// <summary>
		/// ɫ�ʿռ�����ơ����硰CIEXYZ����
		/// </summary>
		public readonly string name;
		/// <summary>
		/// ɫ�ʿռ�Ķ����ƣ�����ͨ������д�����硰XYZ����
		/// </summary>
		public readonly string nameShort;
		/// <summary>
		/// ����ɫ��ͨ�������ơ�
		/// </summary>
		public readonly string[] nameChannels;
		/// <summary>
		/// ������Ϣ��
		/// </summary>
		public readonly string description;

		/// <summary>
		/// ��ָ���Ĳ�����ʼ�� ColorSpaceName �����ʵ���� 
		/// </summary>
		/// <param name="colorSpace">ɫ�ʿռ䡣</param>
		/// <param name="name">ɫ�ʿռ�����ơ����硰CIEXYZ����</param>
		/// <param name="nameShort">ɫ�ʿռ�Ķ����ƣ�����ͨ������д�����硰XYZ����</param>
		/// <param name="nameC0">ͨ��0�����ơ�</param>
		/// <param name="nameC1">ͨ��1�����ơ�</param>
		/// <param name="nameC2">ͨ��2�����ơ�</param>
		/// <param name="nameC3">ͨ��3�����ơ�</param>
		/// <param name="description">������Ϣ��</param>
		public ColorSpaceName(ColorSpace colorSpace, string name, string nameShort, string nameC0, string nameC1, string nameC2, string nameC3, string description)
		{
			this.colorSpace = colorSpace;
			this.name = name;
			this.nameShort = nameShort;
			this.description = description;
			this.nameChannels = new string[4] { nameC0, nameC1, nameC2, nameC3 };
		}

		/// <summary>
		/// ��ָ���Ĳ�����ʼ�� ColorSpaceName �����ʵ��������������
		/// </summary>
		/// <param name="colorSpace">ɫ�ʿռ䡣</param>
		/// <param name="name">ɫ�ʿռ�����ơ����硰CIEXYZ����</param>
		/// <param name="nameShort">ɫ�ʿռ�Ķ����ƣ�����ͨ������д�����硰XYZ����</param>
		/// <param name="nameC0">ͨ��0�����ơ�</param>
		/// <param name="nameC1">ͨ��1�����ơ�</param>
		/// <param name="nameC2">ͨ��2�����ơ�</param>
		/// <param name="nameC3">ͨ��3�����ơ�</param>
		public ColorSpaceName(ColorSpace colorSpace, string name, string nameShort, string nameC0, string nameC1, string nameC2, string nameC3)
			:this(colorSpace, name, nameShort, nameC0, nameC1, nameC2, nameC3, string.Empty)
		{
		}

		/// <summary>
		/// ��ָ����ColorSpaceName��ʼ�� ColorSpaceName �����ʵ���� 
		/// </summary>
		/// <param name="v"></param>
		public ColorSpaceName(ColorSpaceName v)
			: this(v.colorSpace, v.name, v.nameShort, v.nameC0, v.nameC1, v.nameC2, v.nameC3, v.description)
		{
		}

		/// <summary>
		/// ͨ��0�����ơ�
		/// </summary>
		public string nameC0
		{
			get { return nameChannels[0]; }
		}

		/// <summary>
		/// ͨ��1�����ơ�
		/// </summary>
		public string nameC1
		{
			get { return nameChannels[1]; }
		}

		/// <summary>
		/// ͨ��2�����ơ�
		/// </summary>
		public string nameC2
		{
			get { return nameChannels[2]; }
		}

		/// <summary>
		/// ͨ��3�����ơ�
		/// </summary>
		public string nameC3
		{
			get { return nameChannels[3]; }
		}

	}

}
