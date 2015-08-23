/*
 * ColorSpace.cs
 * 色彩空间的定义及辅助函数。
 * @Author zyl910
 * 
 * [2011-10-05]
 * 定义。
 * 
 * [2011-10-06]
 * 将ColorSpace枚举改为继承自uint，为了与FourCC兼容。
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
	/// 色彩空间
	/// </summary>
	/// <remarks>
	/// ColorSpace分为两类，一类是正常的色彩空间，用于描述色彩；另一类是非色彩数据模式，用于存放图形学的其他数据。
	/// <para>注意：</para>
	/// <para>1.大于等于零的是色彩数据模式，小于0的是非色彩数据模式。</para>
	/// <para>2.仅在色彩数据模式之间进行色彩空间转换。</para>
	/// <para>3.源与目标的任一一方是非色彩数据模式时，不进行色彩空间转换，仅复制数据（既相当于色彩空间都是RGB）。</para>
	/// <para>4.Indexed和VectorIndexed表示索引模式。在进行数据复制时，不缩放数值，仅对数值进行截断处理。浮点通道转为整数时，仅将其看做8位整数。</para>
	/// <para>5.Indexed为索引色模式，仅能作为源空间。用作目标空间或未提供调色板时，当做Gray来处理。</para>
	/// <para>6.Gray作为源空间时，会尝试给目标空间的前3个均填充。用作目标空间时，只取源空间的首个通道的数值。</para>
	/// <para>7.所有色彩空间都可以有Alpha通道（已经占用了4个的除外，如CMYK），所以都省略了“A”字母。若有Alpha通道，它必定是通道3。</para>
	/// <para>数值规范化：</para>
	/// <para>1.对于无符号整数，表示[0.0, 1.0]。</para>
	/// <para>2.对于带符号整数，[0,最大值]表示[0.0, 1.0]，[最小值, 0]表示[-1.0, 0.0]。</para>
	/// <para>色彩转换的格式支持的优先顺序：</para>
	/// <para>1.ZPFT_CHANNELF32。</para>
	/// <para>2.ZPFT_CHANNELI8。</para>
	/// <para>3.ZPFT_CHANNELI16。</para>
	/// <para>4.带符号的ZPFT_CHANNELI8。</para>
	/// <para>4.带符号的ZPFT_CHANNELI16。</para>
	/// <para>4.其他。</para>
	/// </remarks>
	public enum ColorSpace: uint	// 基于32位无符号数
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
	/// ColorSpaceTool 是一个静态类，为处理 <see cref="ColorSpace"/> 提供方便。
	/// </summary>
	public static class ColorSpaceTool
	{
		/// <summary>
		/// 该ColorSpace是否是色彩数据模式？
		/// </summary>
		/// <param name="cs"><seealso cref="ColorSpace"/>变量。</param>
		/// <returns>当该ColorSpace是色彩数据模式时，返回true。否则是非色彩数据模式，返回false。</returns>
		public static bool isColor(ColorSpace cs)
		{
			return (cs < ColorSpace.Vector);
		}

		/// <summary>
		/// 该ColorSpace是否是色彩数据模式？
		/// </summary>
		/// <param name="cs">以整数方式存储的<seealso cref="ColorSpace"/>变量。</param>
		/// <returns>当该ColorSpace是色彩数据模式时，返回true。否则是非色彩数据模式，返回false。</returns>
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
		/// 颜色空间名称的数组。
		/// </summary>
		/// <remarks>
		/// 请不要修改该数组。
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
		/// 查找 <seealso cref="ColorSpace"/> 的名称信息。
		/// </summary>
		/// <param name="cs">色彩空间 <seealso cref="ColorSpace"/>变量。</param>
		/// <returns>返回名称信息。</returns>
		public static ColorSpaceName findName(ColorSpace cs)
		{
			return KeyValuePairs.find(Names, cs);
		}

		#endregion	// Names

	}

	/// <summary>
	/// ColorSpaceName 是一个不变类（immutable pattern，不变模式），用于得到 <see cref="ColorSpace"/> 的名称等信息。
	/// </summary>
	public sealed class ColorSpaceName
	{
		/// <summary>
		/// 色彩空间。
		/// </summary>
		public readonly ColorSpace colorSpace;
		/// <summary>
		/// 色彩空间的名称。例如“CIEXYZ”。
		/// </summary>
		public readonly string name;
		/// <summary>
		/// 色彩空间的短名称，仅是通道的缩写。例如“XYZ”。
		/// </summary>
		public readonly string nameShort;
		/// <summary>
		/// 各个色彩通道的名称。
		/// </summary>
		public readonly string[] nameChannels;
		/// <summary>
		/// 描述信息。
		/// </summary>
		public readonly string description;

		/// <summary>
		/// 从指定的参数初始化 ColorSpaceName 类的新实例。 
		/// </summary>
		/// <param name="colorSpace">色彩空间。</param>
		/// <param name="name">色彩空间的名称。例如“CIEXYZ”。</param>
		/// <param name="nameShort">色彩空间的短名称，仅是通道的缩写。例如“XYZ”。</param>
		/// <param name="nameC0">通道0的名称。</param>
		/// <param name="nameC1">通道1的名称。</param>
		/// <param name="nameC2">通道2的名称。</param>
		/// <param name="nameC3">通道3的名称。</param>
		/// <param name="description">描述信息。</param>
		public ColorSpaceName(ColorSpace colorSpace, string name, string nameShort, string nameC0, string nameC1, string nameC2, string nameC3, string description)
		{
			this.colorSpace = colorSpace;
			this.name = name;
			this.nameShort = nameShort;
			this.description = description;
			this.nameChannels = new string[4] { nameC0, nameC1, nameC2, nameC3 };
		}

		/// <summary>
		/// 从指定的参数初始化 ColorSpaceName 类的新实例。忽略描述。
		/// </summary>
		/// <param name="colorSpace">色彩空间。</param>
		/// <param name="name">色彩空间的名称。例如“CIEXYZ”。</param>
		/// <param name="nameShort">色彩空间的短名称，仅是通道的缩写。例如“XYZ”。</param>
		/// <param name="nameC0">通道0的名称。</param>
		/// <param name="nameC1">通道1的名称。</param>
		/// <param name="nameC2">通道2的名称。</param>
		/// <param name="nameC3">通道3的名称。</param>
		public ColorSpaceName(ColorSpace colorSpace, string name, string nameShort, string nameC0, string nameC1, string nameC2, string nameC3)
			:this(colorSpace, name, nameShort, nameC0, nameC1, nameC2, nameC3, string.Empty)
		{
		}

		/// <summary>
		/// 从指定的ColorSpaceName初始化 ColorSpaceName 类的新实例。 
		/// </summary>
		/// <param name="v"></param>
		public ColorSpaceName(ColorSpaceName v)
			: this(v.colorSpace, v.name, v.nameShort, v.nameC0, v.nameC1, v.nameC2, v.nameC3, v.description)
		{
		}

		/// <summary>
		/// 通道0的名称。
		/// </summary>
		public string nameC0
		{
			get { return nameChannels[0]; }
		}

		/// <summary>
		/// 通道1的名称。
		/// </summary>
		public string nameC1
		{
			get { return nameChannels[1]; }
		}

		/// <summary>
		/// 通道2的名称。
		/// </summary>
		public string nameC2
		{
			get { return nameChannels[2]; }
		}

		/// <summary>
		/// 通道3的名称。
		/// </summary>
		public string nameC3
		{
			get { return nameChannels[3]; }
		}

	}

}
