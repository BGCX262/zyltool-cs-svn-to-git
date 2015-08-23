using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace zylTool.Pointer
{
	/// <summary>
	/// ָ��ƫ��-����ģʽ����������� <see cref="WrapAddressMode.Clamp"/> ���ԣ�ʵ����ָ��ƫ�Ʊ����
	/// </summary>
	public sealed unsafe class PointerOffsetClamp : IPointerOffset
	{
		private long* m_OffsetPointer = null;	// ָ��ƫ�Ʊ�Ļ���ַ
		private int m_OffsetSize;	//	ָ��ƫ�Ʊ�ĳߴ磨Ԫ�صĸ�����
		private int m_OffsetBase;	// ָ��ƫ�Ʊ��������Ļ���ƫ�ơ����壺f(i) = OffsetPointer[OffsetBase + i]��
		private long m_Stride;	// ָ��Ŀ��
		private long m_LimitStart;	// �޶��������
		private long m_LimitSize;	// �޶�����ĳߴ�
		private long m_Pos;	// ��ǰλ�á��п��ܻ��ڻ����޶������⣬�����Ѿ�������ָ��ƫ�Ʊ��Ա�ָ֤�벻��Խ�硣

		#region Constructor //���캯��
		/// <summary>
		/// Ĭ�Ϲ��캯����Ϊ�����ֶθ���ֵ��
		/// </summary>
		public PointerOffsetClamp()
		{
			Clear();
		}

		/// <summary>
		/// ���캯�������ݲ������г�ʼ����
		/// </summary>
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
		/// <returns>�Ƿ�ɹ�</returns>
		/// <remarks>��ʼ���ɹ��󣬻�ʹ�� <paramref name="LimitStart"/> ���� <seealso cref="InitPos"/> ������</remarks>
		public unsafe PointerOffsetClamp(long* OffsetPointer, int OffsetSize, int OffsetBase, long Stride, long LimitStart, long LimitSize)
		{
			Init(OffsetPointer, OffsetSize, OffsetBase, Stride, LimitStart, LimitSize);
		}

		#endregion	//Constructor //���캯��

		#region InitClear //��ʼ�������
		/// <summary>
		/// ��ʼ��
		/// </summary>
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
		/// <returns>�Ƿ�ɹ�</returns>
		/// <remarks>��ʼ���ɹ��󣬻�ʹ�� <paramref name="LimitStart"/> ���� <seealso cref="InitPos"/> ������</remarks>
		public unsafe bool Init(long* OffsetPointer, int OffsetSize, int OffsetBase, long Stride, long LimitStart, long LimitSize)
		{
			unchecked
			{
				// ������
				if (null == OffsetPointer) return false;
				if (OffsetSize <= 0) return false;
				if (OffsetBase < 0) return false;
				if (OffsetBase >= OffsetSize) return false;
				if (Stride == 0) return false;
				if (LimitSize <= 0) return false;

				// ��ֵ
				m_OffsetPointer = OffsetPointer;
				m_OffsetSize = OffsetSize;
				m_OffsetBase = OffsetBase;
				m_Stride = Stride;
				m_LimitStart = LimitStart;
				m_LimitSize = LimitSize;

				// ��ʼ��
				InitPos(LimitStart);
				return true;
			}
		}

		/// <summary>
		/// ��ʼ��ָ��ƫ�Ʊ������
		/// </summary>
		/// <param name="Pos">��ǰλ��</param>
		/// <remarks>�÷����ᰴ�ձ��ز��Խ�����ָ��ƫ�Ʊ����³�ʼ�����ٶ��������Ǳ�֤��ȷ��</remarks>
		public void InitPos(long Pos)
		{
			m_Pos = Pos;

			// ����pos����ʼ��ָ��ƫ�Ʊ������
			// <MustOverride>
			ClampInit_Fast(m_Pos, m_OffsetPointer, m_OffsetSize, m_OffsetBase, m_Stride, m_LimitStart, m_LimitSize);
		}

		/// <summary>
		/// ���
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
		#endregion	//InitClear //��ʼ�������

		#region Property //����
		/// <summary>
		/// ָ��ƫ�Ʊ�Ļ���ַ��
		/// <para>�뱣֤���ڴ�鱻�������ɲο�<seealso cref="fixed"/>��<seealso cref="stackalloc"/>��<seealso cref="System.Runtime.InteropServices.Marshal.AllocHGlobal"/>��</para>
		/// </summary>
		public unsafe long* OffsetPointer
		{
			get { return m_OffsetPointer; }
		}

		/// <summary>
		/// ָ��ƫ�Ʊ�ĳߴ磨Ԫ�صĸ�������
		/// </summary>
		public int OffsetSize
		{
			get { return m_OffsetSize; }
		}

		/// <summary>
		/// ָ��ƫ�Ʊ��������Ļ���ƫ�ơ�
		/// <para>���壺f(i) = OffsetPointer[OffsetBase + i]��</para>
		/// </summary>
		public int OffsetBase
		{
			get { return m_OffsetBase; }
		}

		/// <summary>
		/// ָ��Ŀ�ࡣ
		/// </summary>
		/// <remarks>
		/// <para>���硪��</para>
		/// <para>1.����byte����ʱ������1��</para>
		/// <para>2.����UTF16��ʽ��char����ʱ������2��</para>
		/// <para>3.����int����ʱ������4��</para>
		/// <para>4.����long����ʱ������8��</para>
		/// <para>5.��ͼ����ʱ�����������ֽ����������ɨ���п�ࡣ</para>
		/// </remarks>
		public long Stride
		{
			get { return m_Stride; }
		}

		/// <summary>
		/// �޶�������硣
		/// </summary>
		public long LimitStart
		{
			get { return m_LimitStart; }
		}

		/// <summary>
		/// �޶�����ĳߴ硣
		/// <para>ע���޶�����ķ�Χ��[LimitStart, LimitStart+LimitSize)���䣨����������ʱΪ���������㣬�Ὣ��淶��Ϊ[0.0, 1.0)���䣨ʵ������</para>
		/// </summary>
		public long LimitSize
		{
			get { return m_LimitSize; }
		}

		/// <summary>
		/// ��ǰλ��
		/// </summary>
		/// <remarks>�޸ı����Ժ󣬽��������Ż��ķ�ʽ����ָ��ƫ�Ʊ�</remarks>
		public long Pos
		{
			get { return m_Pos; }
			set
			{
				// ���
				Debug.Assert(null != m_OffsetPointer, "m_OffsetPointer is null!");
				if (value == m_Pos) return;

				// ����ָ��ƫ�Ʊ�
				//m_Pos = value;	// <MustOverride>
				MoveScroll(value - m_Pos);
			}
		}

		#endregion	Property //����

		#region Method //����
		/// <summary>
		/// �õ�ƫ��ֵ
		/// </summary>
		/// <param name="idx">����ֵ����Ϊ������ֻҪ���� [-OffsetBase, OffsetSize-OffsetBase) ���䡣</param>
		/// <returns>ƫ��ֵ</returns>
		/// <remarks>
		/// <para>���壺getOffset(idx) = OffsetPointer[OffsetBase + idx]��</para>
		/// <para>ע�⣺��<seealso cref="WrapAddressMode.Border"/>ģʽ�£���λ���޶������⣬�᷵��<seealso cref="PointerTool.InvalidOffset"/>��</para>
		/// </remarks>
		public long getOffset(int idx)
		{
			Debug.Assert(null != m_OffsetPointer, "OffsetPointer is null!");
			return m_OffsetPointer[m_OffsetBase + idx];
		}

		/// <summary>
		/// �õ�ƫ�ƺ��ָ��
		/// </summary>
		/// <param name="pSrc">Դָ��</param>
		/// <param name="idx">����ֵ����Ϊ������ֻҪ���� [-OffsetBase, OffsetSize-OffsetBase) ���䡣</param>
		/// <returns>ƫ�ƺ��ָ��</returns>
		public unsafe byte* Ptr(byte* pSrc, int idx)
		{
			Debug.Assert(null != m_OffsetPointer, "OffsetPointer is null!");
			return pSrc + m_OffsetPointer[m_OffsetBase + idx];
		}

		/// <summary>
		/// �õ�ƫ�ƺ��ָ����չ��
		/// </summary>
		/// <param name="pOut">ƫ�ƺ��ָ��</param>
		/// <param name="pSrc">Դָ��</param>
		/// <param name="idx">����ֵ����Ϊ������ֻҪ���� [-OffsetBase, OffsetSize-OffsetBase) ���䡣</param>
		/// <returns>��ַƫ�����Ƿ���Ч������ַƫ����Ϊ<seealso cref="PointerTool.InvalidOffset"/>������false�����򷵻�true��</returns>
		/// <remarks>
		/// <para>��һ������£�����ַƫ��������<seealso cref="PointerTool.InvalidOffset"/>��ʱ���㷨Ϊ��<c>pOut = pSrc + OffsetPointer[OffsetBase + idx]</c>��</para>
		/// </remarks>
		public unsafe bool PtrEx(out byte* pOut, byte* pSrc, int idx)
		{
			Debug.Assert(null != m_OffsetPointer, "OffsetPointer is null!");
			pOut = pSrc + m_OffsetPointer[m_OffsetBase + idx];
			return true;
		}

		/// <summary>
		/// ִ��ǰ�ơ�����ı�<seealso cref="Pos"/>��ָ��ƫ�Ʊ�
		/// </summary>
		/// <remarks>Ч��Ϊ��<c>Pos = Pos + 1</c>��</remarks>
		public void MoveNext()
		{
			MoveScroll(1);
		}

		/// <summary>
		/// ִ�л��ơ�����ı�<seealso cref="Pos"/>��ָ��ƫ�Ʊ�
		/// </summary>
		/// <remarks>Ч��Ϊ��<c>Pos = Pos - 1</c>��</remarks>
		public void MovePrev()
		{
			MoveScroll(-1);
		}

		/// <summary>
		/// ִ�й���������ı�<seealso cref="Pos"/>��ָ��ƫ�Ʊ�
		/// </summary>
		/// <param name="ScrollValue">��������</param>
		/// <remarks>Ч��Ϊ��<c>Pos = Pos + ScrollValue</c>��</remarks>
		public void MoveScroll(long ScrollValue)
		{
			if (0 == ScrollValue) return;
			//Pos = Pos + ScrollValue;
			// <MustOverride>
			MoveScroll_core(ScrollValue);
		}

		/// <summary>
		/// ִ�й���������ı�<seealso cref="Pos"/>��ָ��ƫ�Ʊ�
		/// </summary>
		/// <param name="ScrollValue">������������Ϊ0��</param>
		/// <remarks>Ч��Ϊ��<c>Pos = Pos + ScrollValue</c>��</remarks>
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
			long p1 = m_OffsetBase - (m_Pos - m_LimitStart);	// [δ����]��׼�����յ�
			long p2 = p1 + m_LimitSize;	// [δ����]��׼���ҽ翪��
			long q1 = Math.Max(0, Math.Min(p1, m_OffsetSize));	// [����]��׼�����յ�
			long q2 = Math.Max(0, Math.Min(p2, m_OffsetSize));	// [����]��׼���ҽ翪��
			long v = (p1 - m_OffsetBase) * m_Stride;
			for (i = 0; i < q1; ++i)
			{
				m_OffsetPointer[i] = v;	// ������
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
				m_OffsetPointer[i] = (i - m_OffsetBase) * m_Stride;	// ��׼��
			}
			v = (p2 - 1 - m_OffsetBase) * m_Stride;
			for (i = q2; i < m_OffsetSize; ++i)
			{
				m_OffsetPointer[i] = v;	// ������
			}
		}

		#endregion	Method //����

		#region	Algorithm // �㷨
		/// <summary>
		/// �õ�Clampģʽ��ָ��ƫ�Ʊ�.�����㷨��
		/// <para>ע�⣺�ú����Ǵ��㷨�࣬��ִ���κβ�����顣���ڵ���ǰ���ò�����</para>
		/// </summary>
		/// <param name="m_Pos">��ǰλ�á�</param>
		/// <param name="m_OffsetPointer">ָ��ƫ�Ʊ�Ļ���ַ��������Ϊnull��</param>
		/// <param name="m_OffsetSize">ָ��ƫ�Ʊ�ĳߴ磨Ԫ�صĸ����������������0��</param>
		/// <param name="m_OffsetBase">ָ��ƫ�Ʊ��������Ļ���ƫ�ơ��������� [0,OffsetSize) ���䡣</param>
		/// <param name="m_Stride">ָ��Ŀ�ࡣ������Ϊ0��������Ϊ������</param>
		/// <param name="m_LimitStart">�޶�������硣</param>
		/// <param name="m_LimitSize">�޶�����ĳߴ硣���������0��</param>
		public static unsafe void ClampInit_Base(long m_Pos,
			long* m_OffsetPointer, long m_OffsetSize, long m_OffsetBase, long m_Stride, 
			long m_LimitStart, long m_LimitSize)
		{
			// ���Լ�顣����DEBUG����Ч
			Debug.Assert(null != m_OffsetPointer, "m_OffsetPointer is null!");
			Debug.Assert(m_OffsetSize > 0);
			Debug.Assert(m_OffsetBase >= 0);
			Debug.Assert(m_OffsetBase < m_OffsetSize);
			Debug.Assert(m_Stride != 0);
			Debug.Assert(m_LimitSize > 0);
			// �㷨
			for (long i = 0; i < m_OffsetSize; ++i)
			{
				long ioff = (i - m_OffsetBase);	// ƫ����
				long ipos0 = m_Pos + ioff;	// ƫ�ƺ��λ��
				long ipos = ipos0;	// ƫ�ƺ��λ���پ�Clamp��������
				if (ipos < m_LimitStart) ipos = m_LimitStart;
				if (ipos > (m_LimitStart + m_LimitSize - 1)) ipos = m_LimitStart + m_LimitSize - 1;
				m_OffsetPointer[i] = ipos - m_Pos;	// ����ƫ������ֵ
			}
		}

		/// <summary>
		/// �õ�Clampģʽ��ָ��ƫ�Ʊ�.�����㷨��
		/// <para>ע�⣺�ú����Ǵ��㷨�࣬��ִ���κβ�����顣���ڵ���ǰ���ò�����</para>
		/// </summary>
		/// <param name="m_Pos">��ǰλ�á�</param>
		/// <param name="m_OffsetPointer">ָ��ƫ�Ʊ�Ļ���ַ��������Ϊnull��</param>
		/// <param name="m_OffsetSize">ָ��ƫ�Ʊ�ĳߴ磨Ԫ�صĸ����������������0��</param>
		/// <param name="m_OffsetBase">ָ��ƫ�Ʊ��������Ļ���ƫ�ơ��������� [0,OffsetSize) ���䡣</param>
		/// <param name="m_Stride">ָ��Ŀ�ࡣ������Ϊ0��������Ϊ������</param>
		/// <param name="m_LimitStart">�޶�������硣</param>
		/// <param name="m_LimitSize">�޶�����ĳߴ硣���������0��</param>
		public static unsafe void ClampInit_Fast(long m_Pos,
			long* m_OffsetPointer, long m_OffsetSize, long m_OffsetBase, long m_Stride,
			long m_LimitStart, long m_LimitSize)
		{
			// ���Լ�顣����DEBUG����Ч
			Debug.Assert(null != m_OffsetPointer, "m_OffsetPointer is null!");
			Debug.Assert(m_OffsetSize > 0);
			Debug.Assert(m_OffsetBase >= 0);
			Debug.Assert(m_OffsetBase < m_OffsetSize);
			Debug.Assert(m_Stride != 0);
			Debug.Assert(m_LimitSize > 0);
			// �㷨
			long i;
			long p1 = m_OffsetBase - (m_Pos - m_LimitStart);	// [δ����]��׼�����յ�
			long p2 = p1 + m_LimitSize;	// [δ����]��׼���ҽ翪��
			long q1 = Math.Max(0, Math.Min(p1, m_OffsetSize));	// [����]��׼�����յ�
			long q2 = Math.Max(0, Math.Min(p2, m_OffsetSize));	// [����]��׼���ҽ翪��
			long v = (p1 - m_OffsetBase) * m_Stride;
			for (i = 0; i < q1; ++i)
			{
				m_OffsetPointer[i] = v;	// ������
			}
			for (i = q1; i < q2; ++i)
			{
				m_OffsetPointer[i] = (i - m_OffsetBase) * m_Stride;	// ��׼��
			}
			v = (p2 - 1 - m_OffsetBase) * m_Stride;
			for (i = q2; i < m_OffsetSize; ++i)
			{
				m_OffsetPointer[i] = v;	// ������
			}
		}

		#endregion	// Algorithm // �㷨
	}
}

/*

�㷨�Ƶ�
~~~~~~~~


һ��Clamp��ʼ�������㷨

�����㷨Ϊ����
		public static unsafe void ClampInit_Base(long m_Pos,
			long* m_OffsetPointer, long m_OffsetSize, long m_OffsetBase, long m_Stride, 
			long m_LimitStart, long m_LimitSize)
		{
			for (long i = 0; i < m_OffsetSize; ++i)
			{
				long ioff = (i - m_OffsetBase);	// ƫ����
				long ipos0 = m_Pos + ioff;	// ƫ�ƺ��λ��
				long ipos = ipos0;	// ƫ�ƺ��λ���پ�Clamp��������
				if (ipos < m_LimitStart) ipos = m_LimitStart;
				if (ipos > (m_LimitStart + m_LimitSize - 1)) ipos = m_LimitStart + m_LimitSize - 1;
				m_OffsetPointer[i] = ipos - m_Pos;	// ����ƫ������ֵ
			}
		}


����ClampFast��ʼ�������㷨

2.1 �����

�ȹ۲�һ���޶�����6��ƫ�Ʊ�4��ƫ�Ʊ���ֵ����
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

�����п��Է��ֹ��ɣ��������ݴ��¿ɷ�Ϊ���ࡪ������������׼���������������˼�������㣬���ǿ���ֻ��ע��׼���ķ�Χ��
��׼��Start��ǰ�ա�δ���ã���p1 = OffsetBase - pos;	// ���ϸ����Ļ�����Ϊ��p1 = OffsetBase - (pos - LimitStart);
��׼��end  ���󿪡�δ���ã���p2 = p1 + LimitSize;

Ȼ���������ƫ�Ʊ��еģ��ĵ�ַ��Χ���ѻ�ȥOffsetBase����ƫ����ֵΪ����
��������[0, p1)��ֵΪ (p1-OffsetBase)*Stride
��׼����[p1, p2)��ֵΪ (i-OffsetBase)*Stride
��������[p2, OffsetSize)��ֵΪ (p2-1-OffsetBase)*Stride

���м��á���
q1 = Math.MinMax(p1, 0, m_OffsetSize);
q2 = Math.MinMax(p2, 0, m_OffsetSize);

���ڿ��Կ�ʼѭ���ˡ���
��������[0, q1)��for(i=0; i<q1; ++i);
��׼����[q1, q2)��for(i=q1; i<q2; ++i);
��������[q2, OffsetSize)��for(i=q2; i<OffsetSize; ++i);


2.2 LimitStartΪ����

�۲�һ���޶�����6���޶��������-2��ƫ�Ʊ�4��ƫ�Ʊ���ֵ����
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

�ɼ�ֻ�����p1�����ˡ���
��׼��Start��ǰ�ա�δ���ã���p1 = OffsetBase - (pos - LimitStart);
��׼��end  ���󿪡�δ���ã���p2 = p1 + LimitSize;


����ClampFast���������㷨

�۲�ǰ��һλʱ ��׼���ı仯����
[4,10)	// old
[3,9)	// cur

���ֿ��Խ� ��ǰ��׼���ҽ� �� �ϴα�׼������� ���бȽϣ���С����Ϊ��׼�������ҽ硣
ͬ����Ƴ�����ʱ�Ĵ���
�����㷨��MoveScroll_core��

*/