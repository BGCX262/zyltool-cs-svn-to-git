using System;
using System.Collections.Generic;
using System.Text;

namespace zylTool.Pointer
{
	/// <summary>
	/// ָ��ƫ�ƽӿڡ���ʵ����ָ��ƫ�Ʊ������࣬��ʵ�ָýӿڡ�
	/// </summary>
	public interface IPointerOffset
	{
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
		unsafe bool Init(long* OffsetPointer, int OffsetSize, int OffsetBase, long Stride, long LimitStart, long LimitSize);

		/// <summary>
		/// ��ʼ��ָ��ƫ�Ʊ������
		/// </summary>
		/// <param name="Pos">��ǰλ��</param>
		/// <remarks>�÷����ᰴ�ձ��ز��Խ�����ָ��ƫ�Ʊ����³�ʼ�����ٶ��������Ǳ�֤��ȷ��</remarks>
		void InitPos(long Pos);

		/// <summary>
		/// ���
		/// </summary>
		void Clear();

		#endregion	//InitClear //��ʼ�������

		#region Property //����
		/// <summary>
		/// ָ��ƫ�Ʊ�Ļ���ַ��
		/// <para>�뱣֤���ڴ�鱻�������ɲο�<seealso cref="fixed"/>��<seealso cref="stackalloc"/>��<seealso cref="System.Runtime.InteropServices.Marshal.AllocHGlobal"/>��</para>
		/// </summary>
		unsafe long* OffsetPointer
		{
			get;
		}

		/// <summary>
		/// ָ��ƫ�Ʊ�ĳߴ磨Ԫ�صĸ�������
		/// </summary>
		int OffsetSize
		{
			get;
		}

		/// <summary>
		/// ָ��ƫ�Ʊ��������Ļ���ƫ�ơ�
		/// <para>���壺f(i) = OffsetPointer[OffsetBase + i]��</para>
		/// </summary>
		int OffsetBase
		{
			get;
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
		long Stride
		{
			get;
		}

		/// <summary>
		/// �޶�������硣
		/// </summary>
		long LimitStart
		{
			get;
		}

		/// <summary>
		/// �޶�����ĳߴ硣
		/// <para>ע���޶�����ķ�Χ��[LimitStart, LimitStart+LimitSize)���䣨����������ʱΪ���������㣬�Ὣ��淶��Ϊ[0.0, 1.0)���䣨ʵ������</para>
		/// </summary>
		long LimitSize
		{
			get;
		}

		/// <summary>
		/// ��ǰλ��
		/// </summary>
		/// <remarks>�޸ı����Ժ󣬽��������Ż��ķ�ʽ����ָ��ƫ�Ʊ�</remarks>
		long Pos
		{
			get;
			set;
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
		long getOffset(int idx);

		/// <summary>
		/// �õ�ƫ�ƺ��ָ��
		/// </summary>
		/// <param name="pSrc">Դָ��</param>
		/// <param name="idx">����ֵ����Ϊ������ֻҪ���� [-OffsetBase, OffsetSize-OffsetBase) ���䡣</param>
		/// <returns>ƫ�ƺ��ָ��</returns>
		unsafe byte* Ptr(byte* pSrc, int idx);

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
		unsafe bool PtrEx(out byte* pOut, byte* pSrc, int idx);

		/// <summary>
		/// ִ��ǰ�ơ�����ı�<seealso cref="Pos"/>��ָ��ƫ�Ʊ�
		/// </summary>
		/// <remarks>Ч��Ϊ��<c>Pos = Pos + 1</c>��</remarks>
		void MoveNext();

		/// <summary>
		/// ִ�л��ơ�����ı�<seealso cref="Pos"/>��ָ��ƫ�Ʊ�
		/// </summary>
		/// <remarks>Ч��Ϊ��<c>Pos = Pos - 1</c>��</remarks>
		void MovePrev();

		/// <summary>
		/// ִ�й���������ı�<seealso cref="Pos"/>��ָ��ƫ�Ʊ�
		/// </summary>
		/// <param name="ScrollValue">��������</param>
		/// <remarks>Ч��Ϊ��<c>Pos = Pos + ScrollValue</c>��</remarks>
		void MoveScroll(long ScrollValue);

		#endregion	Method //����
	}
}
