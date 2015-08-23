using System;
using System.Collections.Generic;
using System.Text;

namespace zylTool.Pointer
{
	/// <summary>
	/// 指针偏移接口。对实现了指针偏移表管理的类，需实现该接口。
	/// </summary>
	public interface IPointerOffset
	{
		#region InitClear //初始化和清空
		/// <summary>
		/// 初始化
		/// </summary>
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
		/// <returns>是否成功</returns>
		/// <remarks>初始化成功后，会使用 <paramref name="LimitStart"/> 调用 <seealso cref="InitPos"/> 方法。</remarks>
		unsafe bool Init(long* OffsetPointer, int OffsetSize, int OffsetBase, long Stride, long LimitStart, long LimitSize);

		/// <summary>
		/// 初始化指针偏移表的内容
		/// </summary>
		/// <param name="Pos">当前位置</param>
		/// <remarks>该方法会按照保守策略将整个指针偏移表重新初始化，速度慢，但是保证正确。</remarks>
		void InitPos(long Pos);

		/// <summary>
		/// 清空
		/// </summary>
		void Clear();

		#endregion	//InitClear //初始化和清空

		#region Property //属性
		/// <summary>
		/// 指针偏移表的基地址。
		/// <para>请保证该内存块被锁定，可参考<seealso cref="fixed"/>、<seealso cref="stackalloc"/>、<seealso cref="System.Runtime.InteropServices.Marshal.AllocHGlobal"/>。</para>
		/// </summary>
		unsafe long* OffsetPointer
		{
			get;
		}

		/// <summary>
		/// 指针偏移表的尺寸（元素的个数）。
		/// </summary>
		int OffsetSize
		{
			get;
		}

		/// <summary>
		/// 指针偏移表中索引的基础偏移。
		/// <para>定义：f(i) = OffsetPointer[OffsetBase + i]。</para>
		/// </summary>
		int OffsetBase
		{
			get;
		}

		/// <summary>
		/// 指针的跨距。
		/// </summary>
		/// <remarks>
		/// <para>例如——</para>
		/// <para>1.处理byte数组时，采用1。</para>
		/// <para>2.处理UTF16格式的char数组时，采用2。</para>
		/// <para>3.处理int数组时，采用4。</para>
		/// <para>4.处理long数组时，采用8。</para>
		/// <para>5.做图像处理时，采用像素字节数，或采用扫描行跨距。</para>
		/// </remarks>
		long Stride
		{
			get;
		}

		/// <summary>
		/// 限定区域左界。
		/// </summary>
		long LimitStart
		{
			get;
		}

		/// <summary>
		/// 限定区域的尺寸。
		/// <para>注：限定区域的范围是[LimitStart, LimitStart+LimitSize)区间（整数）。有时为了描述方便，会将其规范化为[0.0, 1.0)区间（实数）。</para>
		/// </summary>
		long LimitSize
		{
			get;
		}

		/// <summary>
		/// 当前位置
		/// </summary>
		/// <remarks>修改本属性后，将会以最优化的方式调整指针偏移表。</remarks>
		long Pos
		{
			get;
			set;
		}

		#endregion	Property //属性

		#region Method //方法
		/// <summary>
		/// 得到偏移值
		/// </summary>
		/// <param name="idx">索引值。可为负数，只要它在 [-OffsetBase, OffsetSize-OffsetBase) 区间。</param>
		/// <returns>偏移值</returns>
		/// <remarks>
		/// <para>定义：getOffset(idx) = OffsetPointer[OffsetBase + idx]。</para>
		/// <para>注意：在<seealso cref="WrapAddressMode.Border"/>模式下，若位于限定区域外，会返回<seealso cref="PointerTool.InvalidOffset"/>。</para>
		/// </remarks>
		long getOffset(int idx);

		/// <summary>
		/// 得到偏移后的指针
		/// </summary>
		/// <param name="pSrc">源指针</param>
		/// <param name="idx">索引值。可为负数，只要它在 [-OffsetBase, OffsetSize-OffsetBase) 区间。</param>
		/// <returns>偏移后的指针</returns>
		unsafe byte* Ptr(byte* pSrc, int idx);

		/// <summary>
		/// 得到偏移后的指针扩展版
		/// </summary>
		/// <param name="pOut">偏移后的指针</param>
		/// <param name="pSrc">源指针</param>
		/// <param name="idx">索引值。可为负数，只要它在 [-OffsetBase, OffsetSize-OffsetBase) 区间。</param>
		/// <returns>地址偏移量是否有效。若地址偏移量为<seealso cref="PointerTool.InvalidOffset"/>，返回false。否则返回true。</returns>
		/// <remarks>
		/// <para>在一般情况下（当地址偏移量不是<seealso cref="PointerTool.InvalidOffset"/>）时，算法为：<c>pOut = pSrc + OffsetPointer[OffsetBase + idx]</c>。</para>
		/// </remarks>
		unsafe bool PtrEx(out byte* pOut, byte* pSrc, int idx);

		/// <summary>
		/// 执行前移。将会改变<seealso cref="Pos"/>和指针偏移表。
		/// </summary>
		/// <remarks>效果为：<c>Pos = Pos + 1</c>。</remarks>
		void MoveNext();

		/// <summary>
		/// 执行回移。将会改变<seealso cref="Pos"/>和指针偏移表。
		/// </summary>
		/// <remarks>效果为：<c>Pos = Pos - 1</c>。</remarks>
		void MovePrev();

		/// <summary>
		/// 执行滚动。将会改变<seealso cref="Pos"/>和指针偏移表。
		/// </summary>
		/// <param name="ScrollValue">滚动量。</param>
		/// <remarks>效果为：<c>Pos = Pos + ScrollValue</c>。</remarks>
		void MoveScroll(long ScrollValue);

		#endregion	Method //方法
	}
}
