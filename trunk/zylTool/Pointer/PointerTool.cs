using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace zylTool.Pointer
{
	/// <summary>
	/// ����Ѱַģʽ����ָ���Ƴ��޶������绺�����ߴ磩ʱ�������ֲ���ģʽ����ָ��ʹ��ص��޶����򣬻򷵻�����ֵ��
	/// <remarks>
	/// <para>ע1���޶�����ķ�Χ��[LimitStart, LimitStart+LimitSize)���䣨����������ʱΪ���������㣬�Ὣ��淶��Ϊ[0.0, 1.0)���䣨ʵ������</para>
	/// <para>ע2��ȡС��������<c>getDecimal(x) = x - floor(x)</c></para>
	/// <para>ע3��ȡ����������<c>modfloor(x, y) = x - floor(x/y)*y</c></para>
	/// <para></para>
	/// <para>ʾ�⡪��</para>
	/// <para>---[/]---���߿�Border����Խ��󷵻ر߿�����ֵ��</para>
	/// <para>___[/]^^^�����ƣ�Clamp������Խ��ֵ�����ڱ߽硣</para>
	/// <para>///[/]///�����ƣ�Wrap�����ظ����֡�</para>
	/// <para>\/\[/]\/\������Mirror�������������ظ�ģʽ��ԭ������ԭ�����򡭡���</para>
	/// <para>^^\[/]^^^��һ�ξ���MirrorOnce�������������һ�ξ���Ȼ���������ƴ���</para>
	/// <para></para>
	/// <para>���Ž��͡���</para>
	/// <para>[/]��ԭ���������������������</para>
	/// <para>-���߿�����ֵ��</para>
	/// <para>_��ԭ��������ֵ��</para>
	/// <para>^��ԭ�����ҽ��ֵ��</para>
	/// <para>/��ԭ�������ظ������������������</para>
	/// <para>\��ԭ�����ľ�������ݼ���������</para>
	/// </remarks>
	/// </summary>
	public enum WrapAddressMode
	{
		/// <summary>
		/// δ֪��
		/// </summary>
		Unknown = 0,
		/// <summary>
		/// �߿���[0.0,1.0)��Χ��ʱ����������ֵ��ʧ�ܡ�Ĭ��ָ��ֵ �ȣ���
		/// <para>�������壺<c>fBorder(x) = (x in [0.0,1.0))?x:defaultValue</c>��</para>
		/// <para>��ַƫ������Խ�����Ч��</para>
		/// </summary>
		Border,
		/// <summary>
		/// ���ơ����������Ƶ�[0.0, 1.0)��Χ�ڡ�
		/// <para>�������壺<c>fClamp(x) = min(0.0, maxOpen(x, 1.0) )</c>��</para>
		/// <para>��ַƫ����������Ч��</para>
		/// </summary>
		Clamp,
		/// <summary>
		/// ���ơ��������Ƴ��޶������ҽ�ʱ���ͻ��Ƶ��޶�������硣�������Ƴ��޶��������ʱ���ͻ��Ƶ��޶������ҽ硣����ѭ����
		/// <para>�������壺<c>fWrap(x) = getDecimal(x) = x - floor(x)</c>��</para>
		/// <para>��ַƫ����������Ч��</para>
		/// </summary>
		Wrap,
		/// <summary>
		/// ����
		/// <para>�������壺<c>fMirror(x) = abs(modfloor(x+1, 2) - 1)</c>��</para>
		/// <para>��ַƫ����������Ч��</para>
		/// </summary>
		/// <remarks>һʱ�벻������������������ɲο�DirectX��˵��������������Ѱַģʽ��D3DTEXTUREADDRESSö�����͵�D3DTADDRESS_MIRROR��Ա��ʾ������ʹMicrosoft Direct3D����������������߽��ȶ�������о���Ȼ�����ظ�ʹ�á����磬����Ӧ�ó��򴴽���һ������ͼԪ������������ָ��Ϊ(0.0,0.0), (0.0,3.0), (3.0,3.0)��(3.0,0.0)��������Ѱַģʽ����ΪD3DTADDRESS_MIRROR��ʹ������u��v�����ظ����Σ�ÿһ�к�ÿһ�е������������к��е�����ľ���</remarks>
		Mirror,
		/// <summary>
		/// һ�ξ�����(-1.0, 0)��Χ��������Ȼ�󽫷�Χ�����Ƶ�(-1.0, 1.0)���䡣
		/// <para>�������壺<c>fMirrorOnce(x) = fClamp(abs(x)) = min(0.0, maxOpen(abs(x), 1.0) )</c>��</para>
		/// <para>��ַƫ����������Ч��</para>
		/// </summary>
		MirrorOnce
	}

	/// <summary>
	/// ָ������ĸ��������ࡣΪָ������ṩ�����;�̬������
	/// </summary>
	public static unsafe class PointerTool
	{
		/// <summary>
		/// ��Ч��ƫ������
		/// <para>ͨ���� <seealso cref="WrapAddressMode.Border"/> ���ʹ�á�</para>
		/// </summary>
		public const long InvalidOffset = long.MinValue;

		/// <summary>
		/// Ĭ��ָ��ֵ��һ��Ϊ��ָ�롣
		/// </summary>
		public readonly static byte* DefaultPointer = null;

		#region PointerOffset	// ָ��ƫ�Ʊ�
		/// <summary>
		/// ����ָ��ƫ�Ʊ�������
		/// </summary>
		/// <param name="AddressMode">ָ��Ѱַģʽ</param>
		/// <returns>ָ��ƫ�Ʊ�������ʧ��ʱ����null��</returns>
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
		/// ����ָ��ƫ�Ʊ�������
		/// </summary>
		/// <param name="AddressMode">ָ��Ѱַģʽ</param>
		/// <param name="DefaultPointer">Ĭ��ָ�룬������<seealso cref="WrapAddressMode.Border"/>ģʽ��Ĭ��Ϊnull��</param>
		/// <returns>ָ��ƫ�Ʊ�������ʧ��ʱ����null��</returns>
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
		/// ����ָ��ƫ�Ʊ�������
		/// </summary>
		/// <param name="AddressMode">ָ��Ѱַģʽ</param>
		/// <param name="OffsetPointer">ָ��ƫ�Ʊ�Ļ���ַ��������Ϊnull��
		/// 	<para>�뱣֤���ڴ�鱻�������ɲο�<seealso cref="fixed"/>��<seealso cref="stackalloc"/>��<seealso cref="System.Runtime.InteropServices.Marshal.AllocHGlobal"/>��</para>
		/// </param>
		/// <param name="OffsetSize">ָ��ƫ�Ʊ�ĳߴ磨Ԫ�صĸ����������������0��</param>
		/// <param name="OffsetBase">ָ��ƫ�Ʊ��������Ļ���ƫ�ơ��������� [0,OffsetSize) ���䡣
		///		<para>���壺f(i) = OffsetPointer[OffsetBase + i]��</para>
		/// </param>
		/// <param name="Stride">ָ��Ŀ�ࡣ������Ϊ0��������Ϊ������</param>
		/// <param name="LimitStart">�޶�������硣</param>
		/// <param name="LimitSize">�޶�����ĳߴ硣���������0��
		///		<para>ע���޶�����ķ�Χ��[LimitStart, LimitStart+LimitSize)���䣨����������ʱΪ���������㣬�Ὣ��淶��Ϊ[0.0, 1.0)���䣨ʵ������</para>
		/// </param>
		/// <returns>ָ��ƫ�Ʊ�������ʧ��ʱ����null��</returns>
		public static unsafe IPointerOffset NewPointerOffset(WrapAddressMode AddressMode,
			long* OffsetPointer, int OffsetSize, int OffsetBase, long Stride,
			long LimitStart, long LimitSize)
		{
			return NewPointerOffset(AddressMode, OffsetPointer, OffsetSize, OffsetBase, Stride, LimitStart, LimitSize, DefaultPointer);
		}

		/// <summary>
		/// ����ָ��ƫ�Ʊ�������
		/// </summary>
		/// <param name="AddressMode">ָ��Ѱַģʽ</param>
		/// <param name="OffsetPointer">ָ��ƫ�Ʊ�Ļ���ַ��������Ϊnull��
		/// 	<para>�뱣֤���ڴ�鱻�������ɲο�<seealso cref="fixed"/>��<seealso cref="stackalloc"/>��<seealso cref="System.Runtime.InteropServices.Marshal.AllocHGlobal"/>��</para>
		/// </param>
		/// <param name="OffsetSize">ָ��ƫ�Ʊ�ĳߴ磨Ԫ�صĸ����������������0��</param>
		/// <param name="OffsetBase">ָ��ƫ�Ʊ��������Ļ���ƫ�ơ��������� [0,OffsetSize) ���䡣
		///		<para>���壺f(i) = OffsetPointer[OffsetBase + i]��</para>
		/// </param>
		/// <param name="Stride">ָ��Ŀ�ࡣ������Ϊ0��������Ϊ������</param>
		/// <param name="LimitStart">�޶�������硣</param>
		/// <param name="LimitSize">�޶�����ĳߴ硣���������0��
		///		<para>ע���޶�����ķ�Χ��[LimitStart, LimitStart+LimitSize)���䣨����������ʱΪ���������㣬�Ὣ��淶��Ϊ[0.0, 1.0)���䣨ʵ������</para>
		/// </param>
		/// <param name="DefaultPointer">Ĭ��ָ�룬������<seealso cref="WrapAddressMode.Border"/>ģʽ��Ĭ��Ϊnull��</param>
		/// <returns>ָ��ƫ�Ʊ�������ʧ��ʱ����null��</returns>
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
		/// �õ�һάƫ���������ָ��
		/// </summary>
		/// <param name="pSrc">Դָ��</param>
		/// <param name="idx1">[1]����ֵ</param>
		/// <param name="OffsetPointer1">[1]ָ��ƫ�Ʊ�Ļ���ַ</param>
		/// <param name="OffsetBase1">[1]�����Ļ���ƫ��</param>
		/// <param name="DefaultPointer1">[1]Ĭ��ָ��</param>
		/// <returns>�������ָ��</returns>
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
		/// �õ�һάƫ���������ָ����ٰ�
		/// </summary>
		/// <param name="pSrc">Դָ��</param>
		/// <param name="idxAddBase1">[1]���ϻ�ַ�������ֵ</param>
		/// <param name="OffsetPointer1">[1]ָ��ƫ�Ʊ�Ļ���ַ</param>
		/// <returns>�������ָ��</returns>
		/// <remarks>
		/// �㷨Ϊ��<c>pSrc + OffsetPointer1[idxAddBase1]</c>��
		/// ע�⣺���ڵ�ַƫ��������Чʱ���ſ�ʹ�ô˷������ɲο�<seealso cref="WrapAddressMode"/>��
		/// </remarks>
		public static unsafe byte* Ptr1DFast(byte* pSrc, int idxAddBase1, long* OffsetPointer1)
		{
			Debug.Assert(null != OffsetPointer1, "OffsetPointer1 is null!");
			return pSrc + OffsetPointer1[idxAddBase1];
		}

		/// <summary>
		/// �õ���άƫ���������ָ��
		/// </summary>
		/// <param name="pSrc">Դָ��</param>
		/// <param name="idx1">[1]����ֵ</param>
		/// <param name="OffsetPointer1">[1]ָ��ƫ�Ʊ�Ļ���ַ</param>
		/// <param name="OffsetBase1">[1]�����Ļ���ƫ��</param>
		/// <param name="DefaultPointer1">[1]Ĭ��ָ��</param>
		/// <param name="idx2">[2]����ֵ</param>
		/// <param name="OffsetPointer2">[2]ָ��ƫ�Ʊ�Ļ���ַ</param>
		/// <param name="OffsetBase2">[2]�����Ļ���ƫ��</param>
		/// <param name="DefaultPointer2">[2]Ĭ��ָ��</param>
		/// <returns>�������ָ��</returns>
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
		/// �õ���άƫ���������ָ����ٰ�
		/// </summary>
		/// <param name="pSrc">Դָ��</param>
		/// <param name="idxAddBase1">[1]���ϻ�ַ�������ֵ</param>
		/// <param name="OffsetPointer1">[1]ָ��ƫ�Ʊ�Ļ���ַ</param>
		/// <param name="idxAddBase2">[2]���ϻ�ַ�������ֵ</param>
		/// <param name="OffsetPointer2">[2]ָ��ƫ�Ʊ�Ļ���ַ</param>
		/// <returns>�������ָ��</returns>
		/// <remarks>
		/// �㷨Ϊ��<c>pSrc + OffsetPointer1[idxAddBase1] + OffsetPointer2[idxAddBase2]</c>��
		/// ע�⣺���ڵ�ַƫ��������Чʱ���ſ�ʹ�ô˷������ɲο�<seealso cref="WrapAddressMode"/>��
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
		/// �õ�һάƫ���������ָ��
		/// </summary>
		/// <param name="pSrc">Դָ��</param>
		/// <param name="idx1">[1]����ֵ</param>
		/// <param name="PointerOffset1">[1]ָ��ƫ�Ʊ�������</param>
		/// <returns>�������ָ��</returns>
		public static unsafe byte* ItfPtr1D(byte* pSrc, int idx1, IPointerOffset PointerOffset1)
		{
			Debug.Assert(null != PointerOffset1, "PointerOffset1 is null!");
			return PointerOffset1.Ptr(pSrc, idx1);
		}

		/// <summary>
		/// �õ���άƫ���������ָ��
		/// </summary>
		/// <param name="pSrc">Դָ��</param>
		/// <param name="idx1">[1]����ֵ</param>
		/// <param name="PointerOffset1">[1]ָ��ƫ�Ʊ�������</param>
		/// <param name="idx2">[2]����ֵ</param>
		/// <param name="PointerOffset2">[2]ָ��ƫ�Ʊ�������</param>
		/// <returns>�������ָ��</returns>
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
		/// �õ���άƫ���������ָ��
		/// </summary>
		/// <param name="pSrc">Դָ��</param>
		/// <param name="idx1">[1]����ֵ</param>
		/// <param name="PointerOffset1">[1]ָ��ƫ�Ʊ�������</param>
		/// <param name="idx2">[2]����ֵ</param>
		/// <param name="PointerOffset2">[2]ָ��ƫ�Ʊ�������</param>
		/// <param name="idx3">[3]����ֵ</param>
		/// <param name="PointerOffset3">[3]ָ��ƫ�Ʊ�������</param>
		/// <returns>�������ָ��</returns>
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

		#endregion	// PointerOffset	// ָ��ƫ�Ʊ�
	}
}
