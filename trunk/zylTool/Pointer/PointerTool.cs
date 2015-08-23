using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace zylTool.Pointer
{
	/// <summary>
	/// 环绕寻址模式。若指针移出限定区域（如缓冲区尺寸）时，以哪种策略模式修正指针使其回到限定区域，或返回特殊值。
	/// <remarks>
	/// <para>注1：限定区域的范围是[LimitStart, LimitStart+LimitSize)区间（整数）。有时为了描述方便，会将其规范化为[0.0, 1.0)区间（实数）。</para>
	/// <para>注2：取小数函数：<c>getDecimal(x) = x - floor(x)</c></para>
	/// <para>注3：取余数函数：<c>modfloor(x, y) = x - floor(x/y)*y</c></para>
	/// <para></para>
	/// <para>示意——</para>
	/// <para>---[/]---：边框（Border）。越界后返回边框特殊值。</para>
	/// <para>___[/]^^^：限制（Clamp）。将越界值限制在边界。</para>
	/// <para>///[/]///：环绕（Wrap）。重复出现。</para>
	/// <para>\/\[/]\/\：镜像（Mirror）。即这样的重复模式：原、反向、原、反向……。</para>
	/// <para>^^\[/]^^^：一次镜像（MirrorOnce）。仅对左界做一次镜像，然后在做限制处理。</para>
	/// <para></para>
	/// <para>符号解释——</para>
	/// <para>[/]：原函数（例如递增函数）。</para>
	/// <para>-：边框特殊值。</para>
	/// <para>_：原函数左界的值。</para>
	/// <para>^：原函数右界的值。</para>
	/// <para>/：原函数的重复（例如递增函数）。</para>
	/// <para>\：原函数的镜像（例如递减函数）。</para>
	/// </remarks>
	/// </summary>
	public enum WrapAddressMode
	{
		/// <summary>
		/// 未知。
		/// </summary>
		Unknown = 0,
		/// <summary>
		/// 边框。在[0.0,1.0)范围外时，返回特殊值（失败、默认指针值 等）。
		/// <para>函数定义：<c>fBorder(x) = (x in [0.0,1.0))?x:defaultValue</c>。</para>
		/// <para>地址偏移量：越界后无效！</para>
		/// </summary>
		Border,
		/// <summary>
		/// 限制。将坐标限制到[0.0, 1.0)范围内。
		/// <para>函数定义：<c>fClamp(x) = min(0.0, maxOpen(x, 1.0) )</c>。</para>
		/// <para>地址偏移量：恒有效。</para>
		/// </summary>
		Clamp,
		/// <summary>
		/// 环绕。当向右移出限定区域右界时，就环绕到限定区域左界。当向左移出限定区域左界时，就环绕到限定区域右界。无限循环。
		/// <para>函数定义：<c>fWrap(x) = getDecimal(x) = x - floor(x)</c>。</para>
		/// <para>地址偏移量：恒有效。</para>
		/// </summary>
		Wrap,
		/// <summary>
		/// 镜像。
		/// <para>函数定义：<c>fMirror(x) = abs(modfloor(x+1, 2) - 1)</c>。</para>
		/// <para>地址偏移量：恒有效。</para>
		/// </summary>
		/// <remarks>一时想不出简洁清晰的描述，可参考DirectX的说法——镜像纹理寻址模式由D3DTEXTUREADDRESS枚举类型的D3DTADDRESS_MIRROR成员表示，它会使Microsoft Direct3D在纹理坐标的整数边界先对纹理进行镜像然后再重复使用。例如，设想应用程序创建了一个方的图元并把纹理坐标指定为(0.0,0.0), (0.0,3.0), (3.0,3.0)和(3.0,0.0)，把纹理寻址模式设置为D3DTADDRESS_MIRROR会使纹理在u和v方向都重复三次，每一行和每一列的纹理都是相邻行和列的纹理的镜像。</remarks>
		Mirror,
		/// <summary>
		/// 一次镜像。在(-1.0, 0)范围内作镜像，然后将范围外限制到(-1.0, 1.0)区间。
		/// <para>函数定义：<c>fMirrorOnce(x) = fClamp(abs(x)) = min(0.0, maxOpen(abs(x), 1.0) )</c>。</para>
		/// <para>地址偏移量：恒有效。</para>
		/// </summary>
		MirrorOnce
	}

	/// <summary>
	/// 指针操作的辅助工具类。为指针操作提供常数和静态方法。
	/// </summary>
	public static unsafe class PointerTool
	{
		/// <summary>
		/// 无效的偏移量。
		/// <para>通常与 <seealso cref="WrapAddressMode.Border"/> 配合使用。</para>
		/// </summary>
		public const long InvalidOffset = long.MinValue;

		/// <summary>
		/// 默认指针值。一般为空指针。
		/// </summary>
		public readonly static byte* DefaultPointer = null;

		#region PointerOffset	// 指针偏移表
		/// <summary>
		/// 创建指针偏移表管理对象
		/// </summary>
		/// <param name="AddressMode">指针寻址模式</param>
		/// <returns>指针偏移表管理对象。失败时返回null。</returns>
		public static IPointerOffset NewPointerOffset(WrapAddressMode AddressMode)
		{
			switch (AddressMode)
			{
				case WrapAddressMode.Clamp:
					return new PointerOffsetClamp();
				case WrapAddressMode.Border:
					return new PointerOffsetBorder();
			}
			return null;
		}

		/// <summary>
		/// 创建指针偏移表管理对象
		/// </summary>
		/// <param name="AddressMode">指针寻址模式</param>
		/// <param name="DefaultPointer">默认指针，仅用于<seealso cref="WrapAddressMode.Border"/>模式。默认为null。</param>
		/// <returns>指针偏移表管理对象。失败时返回null。</returns>
		public static IPointerOffset NewPointerOffset(WrapAddressMode AddressMode, byte* DefaultPointer)
		{
			switch (AddressMode)
			{
				case WrapAddressMode.Clamp:
					return new PointerOffsetClamp();
				case WrapAddressMode.Border:
					return new PointerOffsetBorder(DefaultPointer);
			}
			return null;
		}

		/// <summary>
		/// 创建指针偏移表管理对象
		/// </summary>
		/// <param name="AddressMode">指针寻址模式</param>
		/// <param name="OffsetPointer">指针偏移表的基地址。它不能为null。
		/// 	<para>请保证该内存块被锁定，可参考<seealso cref="fixed"/>、<seealso cref="stackalloc"/>、<seealso cref="System.Runtime.InteropServices.Marshal.AllocHGlobal"/>。</para>
		/// </param>
		/// <param name="OffsetSize">指针偏移表的尺寸（元素的个数）。它必须大于0。</param>
		/// <param name="OffsetBase">指针偏移表中索引的基础偏移。它必须在 [0,OffsetSize) 区间。
		///		<para>定义：f(i) = OffsetPointer[OffsetBase + i]。</para>
		/// </param>
		/// <param name="Stride">指针的跨距。它不能为0，但可以为负数。</param>
		/// <param name="LimitStart">限定区域左界。</param>
		/// <param name="LimitSize">限定区域的尺寸。它必须大于0。
		///		<para>注：限定区域的范围是[LimitStart, LimitStart+LimitSize)区间（整数）。有时为了描述方便，会将其规范化为[0.0, 1.0)区间（实数）。</para>
		/// </param>
		/// <returns>指针偏移表管理对象。失败时返回null。</returns>
		public static unsafe IPointerOffset NewPointerOffset(WrapAddressMode AddressMode,
			long* OffsetPointer, int OffsetSize, int OffsetBase, long Stride,
			long LimitStart, long LimitSize)
		{
			return NewPointerOffset(AddressMode, OffsetPointer, OffsetSize, OffsetBase, Stride, LimitStart, LimitSize, DefaultPointer);
		}

		/// <summary>
		/// 创建指针偏移表管理对象
		/// </summary>
		/// <param name="AddressMode">指针寻址模式</param>
		/// <param name="OffsetPointer">指针偏移表的基地址。它不能为null。
		/// 	<para>请保证该内存块被锁定，可参考<seealso cref="fixed"/>、<seealso cref="stackalloc"/>、<seealso cref="System.Runtime.InteropServices.Marshal.AllocHGlobal"/>。</para>
		/// </param>
		/// <param name="OffsetSize">指针偏移表的尺寸（元素的个数）。它必须大于0。</param>
		/// <param name="OffsetBase">指针偏移表中索引的基础偏移。它必须在 [0,OffsetSize) 区间。
		///		<para>定义：f(i) = OffsetPointer[OffsetBase + i]。</para>
		/// </param>
		/// <param name="Stride">指针的跨距。它不能为0，但可以为负数。</param>
		/// <param name="LimitStart">限定区域左界。</param>
		/// <param name="LimitSize">限定区域的尺寸。它必须大于0。
		///		<para>注：限定区域的范围是[LimitStart, LimitStart+LimitSize)区间（整数）。有时为了描述方便，会将其规范化为[0.0, 1.0)区间（实数）。</para>
		/// </param>
		/// <param name="DefaultPointer">默认指针，仅用于<seealso cref="WrapAddressMode.Border"/>模式。默认为null。</param>
		/// <returns>指针偏移表管理对象。失败时返回null。</returns>
		public static unsafe IPointerOffset NewPointerOffset(WrapAddressMode AddressMode,
			long* OffsetPointer, int OffsetSize, int OffsetBase, long Stride,
			long LimitStart, long LimitSize, byte* DefaultPointer)
		{
			switch (AddressMode)
			{
				case WrapAddressMode.Clamp:
					return new PointerOffsetClamp(OffsetPointer, OffsetSize, OffsetBase, Stride, LimitStart, LimitSize);
				case WrapAddressMode.Border:
					return new PointerOffsetBorder(OffsetPointer, OffsetSize, OffsetBase, Stride, LimitStart, LimitSize, DefaultPointer);
			}
			return null;
		}

		/// <summary>
		/// 得到一维偏移修正后的指针
		/// </summary>
		/// <param name="pSrc">源指针</param>
		/// <param name="idx1">[1]索引值</param>
		/// <param name="OffsetPointer1">[1]指针偏移表的基地址</param>
		/// <param name="OffsetBase1">[1]索引的基础偏移</param>
		/// <param name="DefaultPointer1">[1]默认指针</param>
		/// <returns>修正后的指针</returns>
		public static unsafe byte* Ptr1D(byte* pSrc, int idx1, long* OffsetPointer1, int OffsetBase1, byte* DefaultPointer1)
		{
			Debug.Assert(null != OffsetPointer1, "OffsetPointer1 is null!");
			long off = OffsetPointer1[OffsetBase1 + idx1];
			if (InvalidOffset == off)
			{
				return DefaultPointer1;
			}
			else
			{
				return pSrc + off;
			}
		}

		/// <summary>
		/// 得到一维偏移修正后的指针快速版
		/// </summary>
		/// <param name="pSrc">源指针</param>
		/// <param name="idxAddBase1">[1]加上基址后的索引值</param>
		/// <param name="OffsetPointer1">[1]指针偏移表的基地址</param>
		/// <returns>修正后的指针</returns>
		/// <remarks>
		/// 算法为：<c>pSrc + OffsetPointer1[idxAddBase1]</c>。
		/// 注意：仅在地址偏移量恒有效时，才可使用此方法。可参考<seealso cref="WrapAddressMode"/>。
		/// </remarks>
		public static unsafe byte* Ptr1DFast(byte* pSrc, int idxAddBase1, long* OffsetPointer1)
		{
			Debug.Assert(null != OffsetPointer1, "OffsetPointer1 is null!");
			return pSrc + OffsetPointer1[idxAddBase1];
		}

		/// <summary>
		/// 得到二维偏移修正后的指针
		/// </summary>
		/// <param name="pSrc">源指针</param>
		/// <param name="idx1">[1]索引值</param>
		/// <param name="OffsetPointer1">[1]指针偏移表的基地址</param>
		/// <param name="OffsetBase1">[1]索引的基础偏移</param>
		/// <param name="DefaultPointer1">[1]默认指针</param>
		/// <param name="idx2">[2]索引值</param>
		/// <param name="OffsetPointer2">[2]指针偏移表的基地址</param>
		/// <param name="OffsetBase2">[2]索引的基础偏移</param>
		/// <param name="DefaultPointer2">[2]默认指针</param>
		/// <returns>修正后的指针</returns>
		public static unsafe byte* Ptr2D(byte* pSrc,
			int idx1, long* OffsetPointer1, int OffsetBase1, byte* DefaultPointer1,
			int idx2, long* OffsetPointer2, int OffsetBase2, byte* DefaultPointer2)
		{
			Debug.Assert(null != OffsetPointer1, "OffsetPointer1 is null!");
			Debug.Assert(null != OffsetPointer2, "OffsetPointer2 is null!");
			byte* p = pSrc;
			long off = OffsetPointer1[OffsetBase1 + idx1];
			if (InvalidOffset == off)
			{
				return DefaultPointer1;
			}
			else
			{
				p = p + off;
				off = OffsetPointer2[OffsetBase2 + idx2];
				if (InvalidOffset == off)
				{
					return DefaultPointer2;
				}
				else
				{
					p = p + off;
				}
			}
			return p;
		}

		/// <summary>
		/// 得到二维偏移修正后的指针快速版
		/// </summary>
		/// <param name="pSrc">源指针</param>
		/// <param name="idxAddBase1">[1]加上基址后的索引值</param>
		/// <param name="OffsetPointer1">[1]指针偏移表的基地址</param>
		/// <param name="idxAddBase2">[2]加上基址后的索引值</param>
		/// <param name="OffsetPointer2">[2]指针偏移表的基地址</param>
		/// <returns>修正后的指针</returns>
		/// <remarks>
		/// 算法为：<c>pSrc + OffsetPointer1[idxAddBase1] + OffsetPointer2[idxAddBase2]</c>。
		/// 注意：仅在地址偏移量恒有效时，才可使用此方法。可参考<seealso cref="WrapAddressMode"/>。
		/// </remarks>
		public static unsafe byte* Ptr2DFast(byte* pSrc,
			int idxAddBase1, long* OffsetPointer1,
			int idxAddBase2, long* OffsetPointer2)
		{
			Debug.Assert(null != OffsetPointer1, "OffsetPointer1 is null!");
			Debug.Assert(null != OffsetPointer2, "OffsetPointer2 is null!");
			return pSrc + OffsetPointer1[idxAddBase1] + OffsetPointer2[idxAddBase2];
		}

		/// <summary>
		/// 得到一维偏移修正后的指针
		/// </summary>
		/// <param name="pSrc">源指针</param>
		/// <param name="idx1">[1]索引值</param>
		/// <param name="PointerOffset1">[1]指针偏移表管理对象</param>
		/// <returns>修正后的指针</returns>
		public static unsafe byte* ItfPtr1D(byte* pSrc, int idx1, IPointerOffset PointerOffset1)
		{
			Debug.Assert(null != PointerOffset1, "PointerOffset1 is null!");
			return PointerOffset1.Ptr(pSrc, idx1);
		}

		/// <summary>
		/// 得到二维偏移修正后的指针
		/// </summary>
		/// <param name="pSrc">源指针</param>
		/// <param name="idx1">[1]索引值</param>
		/// <param name="PointerOffset1">[1]指针偏移表管理对象</param>
		/// <param name="idx2">[2]索引值</param>
		/// <param name="PointerOffset2">[2]指针偏移表管理对象</param>
		/// <returns>修正后的指针</returns>
		public static unsafe byte* ItfPtr2D(byte* pSrc,
			int idx1, IPointerOffset PointerOffset1,
			int idx2, IPointerOffset PointerOffset2)
		{
			Debug.Assert(null != PointerOffset1, "PointerOffset1 is null!");
			Debug.Assert(null != PointerOffset2, "PointerOffset2 is null!");
			byte* p = pSrc;
			if (PointerOffset1.PtrEx(out p, p, idx1))
			{
				if (PointerOffset2.PtrEx(out p, p, idx2))
				{
					//
				}
			}
			return p;
		}

		/// <summary>
		/// 得到三维偏移修正后的指针
		/// </summary>
		/// <param name="pSrc">源指针</param>
		/// <param name="idx1">[1]索引值</param>
		/// <param name="PointerOffset1">[1]指针偏移表管理对象</param>
		/// <param name="idx2">[2]索引值</param>
		/// <param name="PointerOffset2">[2]指针偏移表管理对象</param>
		/// <param name="idx3">[3]索引值</param>
		/// <param name="PointerOffset3">[3]指针偏移表管理对象</param>
		/// <returns>修正后的指针</returns>
		public static unsafe byte* ItfPtr3D(byte* pSrc,
			int idx1, IPointerOffset PointerOffset1,
			int idx2, IPointerOffset PointerOffset2,
			int idx3, IPointerOffset PointerOffset3)
		{
			Debug.Assert(null != PointerOffset1, "PointerOffset1 is null!");
			Debug.Assert(null != PointerOffset2, "PointerOffset2 is null!");
			Debug.Assert(null != PointerOffset3, "PointerOffset3 is null!");
			byte* p = pSrc;
			if (PointerOffset1.PtrEx(out p, p, idx1))
			{
				if (PointerOffset2.PtrEx(out p, p, idx2))
				{
					if (PointerOffset3.PtrEx(out p, p, idx3))
					{
						//
					}
				}
			}
			return p;
		}

		#endregion	// PointerOffset	// 指针偏移表
	}
}
