using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace zylTool.Pointer
{
	/// <summary>
	/// 指针偏移-限制模式。该类采用了 <see cref="WrapAddressMode.Clamp"/> 策略，实现了指针偏移表管理。
	/// </summary>
	public sealed unsafe class PointerOffsetClamp : IPointerOffset
	{
		private long* m_OffsetPointer = null;	// 指针偏移表的基地址
		private int m_OffsetSize;	//	指针偏移表的尺寸（元素的个数）
		private int m_OffsetBase;	// 指针偏移表中索引的基础偏移。定义：f(i) = OffsetPointer[OffsetBase + i]。
		private long m_Stride;	// 指针的跨距
		private long m_LimitStart;	// 限定区域左界
		private long m_LimitSize;	// 限定区域的尺寸
		private long m_Pos;	// 当前位置。有可能会在基于限定区域外，但因已经修正了指针偏移表，仍保证指针不会越界。

		#region Constructor //构造函数
		/// <summary>
		/// 默认构造函数。为所有字段赋初值。
		/// </summary>
		public PointerOffsetClamp()
		{
			Clear();
		}

		/// <summary>
		/// 构造函数。根据参数进行初始化。
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
		public unsafe PointerOffsetClamp(long* OffsetPointer, int OffsetSize, int OffsetBase, long Stride, long LimitStart, long LimitSize)
		{
			Init(OffsetPointer, OffsetSize, OffsetBase, Stride, LimitStart, LimitSize);
		}

		#endregion	//Constructor //构造函数

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
		public unsafe bool Init(long* OffsetPointer, int OffsetSize, int OffsetBase, long Stride, long LimitStart, long LimitSize)
		{
			unchecked
			{
				// 检查参数
				if (null == OffsetPointer) return false;
				if (OffsetSize <= 0) return false;
				if (OffsetBase < 0) return false;
				if (OffsetBase >= OffsetSize) return false;
				if (Stride == 0) return false;
				if (LimitSize <= 0) return false;

				// 赋值
				m_OffsetPointer = OffsetPointer;
				m_OffsetSize = OffsetSize;
				m_OffsetBase = OffsetBase;
				m_Stride = Stride;
				m_LimitStart = LimitStart;
				m_LimitSize = LimitSize;

				// 初始化
				InitPos(LimitStart);
				return true;
			}
		}

		/// <summary>
		/// 初始化指针偏移表的内容
		/// </summary>
		/// <param name="Pos">当前位置</param>
		/// <remarks>该方法会按照保守策略将整个指针偏移表重新初始化，速度慢，但是保证正确。</remarks>
		public void InitPos(long Pos)
		{
			m_Pos = Pos;

			// 根据pos，初始化指针偏移表的内容
			// <MustOverride>
			ClampInit_Fast(m_Pos, m_OffsetPointer, m_OffsetSize, m_OffsetBase, m_Stride, m_LimitStart, m_LimitSize);
		}

		/// <summary>
		/// 清空
		/// </summary>
		public void Clear()
		{
			if (null == m_OffsetPointer) return;
			m_OffsetPointer = null;
			m_OffsetSize = 0;
			m_OffsetBase = 0;
			m_LimitStart = 0;
			m_LimitSize = 0;
			m_Pos = 0;
		}
		#endregion	//InitClear //初始化和清空

		#region Property //属性
		/// <summary>
		/// 指针偏移表的基地址。
		/// <para>请保证该内存块被锁定，可参考<seealso cref="fixed"/>、<seealso cref="stackalloc"/>、<seealso cref="System.Runtime.InteropServices.Marshal.AllocHGlobal"/>。</para>
		/// </summary>
		public unsafe long* OffsetPointer
		{
			get { return m_OffsetPointer; }
		}

		/// <summary>
		/// 指针偏移表的尺寸（元素的个数）。
		/// </summary>
		public int OffsetSize
		{
			get { return m_OffsetSize; }
		}

		/// <summary>
		/// 指针偏移表中索引的基础偏移。
		/// <para>定义：f(i) = OffsetPointer[OffsetBase + i]。</para>
		/// </summary>
		public int OffsetBase
		{
			get { return m_OffsetBase; }
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
		public long Stride
		{
			get { return m_Stride; }
		}

		/// <summary>
		/// 限定区域左界。
		/// </summary>
		public long LimitStart
		{
			get { return m_LimitStart; }
		}

		/// <summary>
		/// 限定区域的尺寸。
		/// <para>注：限定区域的范围是[LimitStart, LimitStart+LimitSize)区间（整数）。有时为了描述方便，会将其规范化为[0.0, 1.0)区间（实数）。</para>
		/// </summary>
		public long LimitSize
		{
			get { return m_LimitSize; }
		}

		/// <summary>
		/// 当前位置
		/// </summary>
		/// <remarks>修改本属性后，将会以最优化的方式调整指针偏移表。</remarks>
		public long Pos
		{
			get { return m_Pos; }
			set
			{
				// 检查
				Debug.Assert(null != m_OffsetPointer, "m_OffsetPointer is null!");
				if (value == m_Pos) return;

				// 修正指针偏移表
				//m_Pos = value;	// <MustOverride>
				MoveScroll(value - m_Pos);
			}
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
		public long getOffset(int idx)
		{
			Debug.Assert(null != m_OffsetPointer, "OffsetPointer is null!");
			return m_OffsetPointer[m_OffsetBase + idx];
		}

		/// <summary>
		/// 得到偏移后的指针
		/// </summary>
		/// <param name="pSrc">源指针</param>
		/// <param name="idx">索引值。可为负数，只要它在 [-OffsetBase, OffsetSize-OffsetBase) 区间。</param>
		/// <returns>偏移后的指针</returns>
		public unsafe byte* Ptr(byte* pSrc, int idx)
		{
			Debug.Assert(null != m_OffsetPointer, "OffsetPointer is null!");
			return pSrc + m_OffsetPointer[m_OffsetBase + idx];
		}

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
		public unsafe bool PtrEx(out byte* pOut, byte* pSrc, int idx)
		{
			Debug.Assert(null != m_OffsetPointer, "OffsetPointer is null!");
			pOut = pSrc + m_OffsetPointer[m_OffsetBase + idx];
			return true;
		}

		/// <summary>
		/// 执行前移。将会改变<seealso cref="Pos"/>和指针偏移表。
		/// </summary>
		/// <remarks>效果为：<c>Pos = Pos + 1</c>。</remarks>
		public void MoveNext()
		{
			MoveScroll(1);
		}

		/// <summary>
		/// 执行回移。将会改变<seealso cref="Pos"/>和指针偏移表。
		/// </summary>
		/// <remarks>效果为：<c>Pos = Pos - 1</c>。</remarks>
		public void MovePrev()
		{
			MoveScroll(-1);
		}

		/// <summary>
		/// 执行滚动。将会改变<seealso cref="Pos"/>和指针偏移表。
		/// </summary>
		/// <param name="ScrollValue">滚动量。</param>
		/// <remarks>效果为：<c>Pos = Pos + ScrollValue</c>。</remarks>
		public void MoveScroll(long ScrollValue)
		{
			if (0 == ScrollValue) return;
			//Pos = Pos + ScrollValue;
			// <MustOverride>
			MoveScroll_core(ScrollValue);
		}

		/// <summary>
		/// 执行滚动。将会改变<seealso cref="Pos"/>和指针偏移表。
		/// </summary>
		/// <param name="ScrollValue">滚动量。不能为0。</param>
		/// <remarks>效果为：<c>Pos = Pos + ScrollValue</c>。</remarks>
		private void MoveScroll_core(long ScrollValue)
		{
			Debug.Assert(0 != ScrollValue);
			//Pos = Pos + ScrollValue;
			// <MustOverride>
			long oldPos = m_Pos;
			m_Pos = m_Pos + ScrollValue;
			//ClampInit_Fast(m_Pos, m_OffsetPointer, m_OffsetSize, m_OffsetBase, m_Stride, m_LimitStart, m_LimitSize);
			//return;
			long i;
			long p1 = m_OffsetBase - (m_Pos - m_LimitStart);	// [未剪裁]标准区左界闭点
			long p2 = p1 + m_LimitSize;	// [未剪裁]标准区右界开点
			long q1 = Math.Max(0, Math.Min(p1, m_OffsetSize));	// [剪裁]标准区左界闭点
			long q2 = Math.Max(0, Math.Min(p2, m_OffsetSize));	// [剪裁]标准区右界开点
			long v = (p1 - m_OffsetBase) * m_Stride;
			for (i = 0; i < q1; ++i)
			{
				m_OffsetPointer[i] = v;	// 上溢区
			}
			long op1 = m_OffsetBase - (oldPos - m_LimitStart);
			long op2 = op1 + m_LimitSize;
			long oq1 = q1;
			long oq2 = q2;
			if (ScrollValue > 0)
			{
				oq2 = Math.Min(oq2, op1);
			}
			else
			{
				oq1 = Math.Max(oq1, op2);
			}
			for (i = oq1; i < oq2; ++i)
			{
				//Debug.WriteLine(string.Format("{0}, {1}, {2}", oq1, oq2, i));
				m_OffsetPointer[i] = (i - m_OffsetBase) * m_Stride;	// 标准区
			}
			v = (p2 - 1 - m_OffsetBase) * m_Stride;
			for (i = q2; i < m_OffsetSize; ++i)
			{
				m_OffsetPointer[i] = v;	// 上溢区
			}
		}

		#endregion	Method //方法

		#region	Algorithm // 算法
		/// <summary>
		/// 得到Clamp模式的指针偏移表.基本算法。
		/// <para>注意：该函数是纯算法类，不执行任何参数检查。请在调用前检查好参数。</para>
		/// </summary>
		/// <param name="m_Pos">当前位置。</param>
		/// <param name="m_OffsetPointer">指针偏移表的基地址。它不能为null。</param>
		/// <param name="m_OffsetSize">指针偏移表的尺寸（元素的个数）。它必须大于0。</param>
		/// <param name="m_OffsetBase">指针偏移表中索引的基础偏移。它必须在 [0,OffsetSize) 区间。</param>
		/// <param name="m_Stride">指针的跨距。它不能为0，但可以为负数。</param>
		/// <param name="m_LimitStart">限定区域左界。</param>
		/// <param name="m_LimitSize">限定区域的尺寸。它必须大于0。</param>
		public static unsafe void ClampInit_Base(long m_Pos,
			long* m_OffsetPointer, long m_OffsetSize, long m_OffsetBase, long m_Stride, 
			long m_LimitStart, long m_LimitSize)
		{
			// 断言检查。仅在DEBUG版有效
			Debug.Assert(null != m_OffsetPointer, "m_OffsetPointer is null!");
			Debug.Assert(m_OffsetSize > 0);
			Debug.Assert(m_OffsetBase >= 0);
			Debug.Assert(m_OffsetBase < m_OffsetSize);
			Debug.Assert(m_Stride != 0);
			Debug.Assert(m_LimitSize > 0);
			// 算法
			for (long i = 0; i < m_OffsetSize; ++i)
			{
				long ioff = (i - m_OffsetBase);	// 偏移量
				long ipos0 = m_Pos + ioff;	// 偏移后的位置
				long ipos = ipos0;	// 偏移后的位置再经Clamp规则修正
				if (ipos < m_LimitStart) ipos = m_LimitStart;
				if (ipos > (m_LimitStart + m_LimitSize - 1)) ipos = m_LimitStart + m_LimitSize - 1;
				m_OffsetPointer[i] = ipos - m_Pos;	// 储存偏移修正值
			}
		}

		/// <summary>
		/// 得到Clamp模式的指针偏移表.快速算法。
		/// <para>注意：该函数是纯算法类，不执行任何参数检查。请在调用前检查好参数。</para>
		/// </summary>
		/// <param name="m_Pos">当前位置。</param>
		/// <param name="m_OffsetPointer">指针偏移表的基地址。它不能为null。</param>
		/// <param name="m_OffsetSize">指针偏移表的尺寸（元素的个数）。它必须大于0。</param>
		/// <param name="m_OffsetBase">指针偏移表中索引的基础偏移。它必须在 [0,OffsetSize) 区间。</param>
		/// <param name="m_Stride">指针的跨距。它不能为0，但可以为负数。</param>
		/// <param name="m_LimitStart">限定区域左界。</param>
		/// <param name="m_LimitSize">限定区域的尺寸。它必须大于0。</param>
		public static unsafe void ClampInit_Fast(long m_Pos,
			long* m_OffsetPointer, long m_OffsetSize, long m_OffsetBase, long m_Stride,
			long m_LimitStart, long m_LimitSize)
		{
			// 断言检查。仅在DEBUG版有效
			Debug.Assert(null != m_OffsetPointer, "m_OffsetPointer is null!");
			Debug.Assert(m_OffsetSize > 0);
			Debug.Assert(m_OffsetBase >= 0);
			Debug.Assert(m_OffsetBase < m_OffsetSize);
			Debug.Assert(m_Stride != 0);
			Debug.Assert(m_LimitSize > 0);
			// 算法
			long i;
			long p1 = m_OffsetBase - (m_Pos - m_LimitStart);	// [未剪裁]标准区左界闭点
			long p2 = p1 + m_LimitSize;	// [未剪裁]标准区右界开点
			long q1 = Math.Max(0, Math.Min(p1, m_OffsetSize));	// [剪裁]标准区左界闭点
			long q2 = Math.Max(0, Math.Min(p2, m_OffsetSize));	// [剪裁]标准区右界开点
			long v = (p1 - m_OffsetBase) * m_Stride;
			for (i = 0; i < q1; ++i)
			{
				m_OffsetPointer[i] = v;	// 上溢区
			}
			for (i = q1; i < q2; ++i)
			{
				m_OffsetPointer[i] = (i - m_OffsetBase) * m_Stride;	// 标准区
			}
			v = (p2 - 1 - m_OffsetBase) * m_Stride;
			for (i = q2; i < m_OffsetSize; ++i)
			{
				m_OffsetPointer[i] = v;	// 上溢区
			}
		}

		#endregion	// Algorithm // 算法
	}
}

/*

算法推导
~~~~~~~~


一、Clamp初始化基本算法

基本算法为——
		public static unsafe void ClampInit_Base(long m_Pos,
			long* m_OffsetPointer, long m_OffsetSize, long m_OffsetBase, long m_Stride, 
			long m_LimitStart, long m_LimitSize)
		{
			for (long i = 0; i < m_OffsetSize; ++i)
			{
				long ioff = (i - m_OffsetBase);	// 偏移量
				long ipos0 = m_Pos + ioff;	// 偏移后的位置
				long ipos = ipos0;	// 偏移后的位置再经Clamp规则修正
				if (ipos < m_LimitStart) ipos = m_LimitStart;
				if (ipos > (m_LimitStart + m_LimitSize - 1)) ipos = m_LimitStart + m_LimitSize - 1;
				m_OffsetPointer[i] = ipos - m_Pos;	// 储存偏移修正值
			}
		}


二、ClampFast初始化快速算法

2.1 简单情况

先观察一下限定区域6、偏移表长4的偏移表数值——
Clamp6_4(OffsetSize=5, OffsetBase=2, Stride=1, LimitStart=0, LimitSize=6)
pos	-2	-1	0	1	2
-4	4				
-3	3				
-2	2	2	2	2	2
-1	1	1	1	1	2
0	0	0	0	1	2
1	-1	-1	0	1	2
2	-2	-1	0	1	2
3	-2	-1	0	1	2
4	-2	-1	0	1	1
5	-2	-1	0	0	0
6	-2	-1	-1	-1	-1
7	-2	-2	-2	-2	-2
8					-3
9					-4

从其中可以发现规律，表中数据大致可分为三类——上溢区、标准区、下溢区。可了简化区域计算，我们可以只关注标准区的范围。
标准区Start（前闭。未剪裁）：p1 = OffsetBase - pos;	// 不严格。下文会修正为：p1 = OffsetBase - (pos - LimitStart);
标准区end  （后开。未剪裁）：p2 = p1 + LimitSize;

然后各区（在偏移表中的）的地址范围（已划去OffsetBase）与偏移数值为——
上溢区：[0, p1)，值为 (p1-OffsetBase)*Stride
标准区：[p1, p2)，值为 (i-OffsetBase)*Stride
下溢区：[p2, OffsetSize)，值为 (p2-1-OffsetBase)*Stride

进行剪裁——
q1 = Math.MinMax(p1, 0, m_OffsetSize);
q2 = Math.MinMax(p2, 0, m_OffsetSize);

现在可以开始循环了——
上溢区：[0, q1)，for(i=0; i<q1; ++i);
标准区：[q1, q2)，for(i=q1; i<q2; ++i);
下溢区：[q2, OffsetSize)，for(i=q2; i<OffsetSize; ++i);


2.2 LimitStart为负数

观察一下限定区域6、限定区域左界-2、偏移表长4的偏移表数值——
clamp6bn2_5(OffsetSize=5, OffsetBase=2, Stride=1, LimitStart=-2, LimitSize=6)
pos	-2	-1	0	1	2
-6					4
-5					3
-4	2				2
-3	1			1	2
-2	0		0	1	2
-1	-1	-1	0	1	2
0	-2	-1	0	1	2
1	-2	-1	0	1	2
2	-2	-1	0	1	2
3	-2	-1	0	1	2
4	-2	-1	0	1	1
5	-2	-1	0		0
6	-2	-1			-1
7	-2				-2
8	-3				
9	-4				

可见只需调整p1就行了——
标准区Start（前闭。未剪裁）：p1 = OffsetBase - (pos - LimitStart);
标准区end  （后开。未剪裁）：p2 = p1 + LimitSize;


二、ClampFast滚动快速算法

观察前移一位时 标准区的变化——
[4,10)	// old
[3,9)	// cur

发现可以将 当前标准区右界 与 上次标准区的左界 进行比较，最小者作为标准区的新右界。
同理可推出回移时的处理。
具体算法见MoveScroll_core。

*/