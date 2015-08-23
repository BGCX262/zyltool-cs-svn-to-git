/*
 * bitFloat32.cs
 * 16λ��������s10e5, binary16(IEEE 754-2008), FP16(DirectX)��
 * @Author zyl910
 * 
 * [2011-10-08]
 * ���塣��ʵ���㷨��
 * 
 * [2011-10-09]
 * Ϊ�˼���ƣ���swapStatic����swap��
 * ��Ϊ����ByteSwap.swap��ʵ��IByteSwapable�ӿڡ�
 * 
 */
using System;
using System.Collections.Generic;
using System.Text;
//using System.Runtime.InteropServices;

namespace zylTool.Bits
{
	/// <summary>
	/// 16λ��������s10e5, binary16(IEEE 754-2008), FP16(DirectX)��
	/// </summary>
	public struct Float16 : IByteSwapable<Float16>
	{
		/// <summary>
		/// ԭʼ���ݡ���������ʽ�洢16λ��������
		/// </summary>
		public Int16 _raw;

		/// <summary>
		/// ��ָ���� 32λ������ ��ʼ�� Float16 �ṹ�� 
		/// </summary>
		/// <param name="v"></param>
		Float16(Single v)
		{
			_raw = FP16ToSingle(v);
		}

		public static implicit operator Single(Float16 v)
		{
			return FP16FromSingle(v._raw);
		}

		public static implicit operator Float16(Single v)
		{
			Float16 rt;
			rt._raw = FP16ToSingle(v);
			return rt;
		}

		/// <summary>
		/// ��������תΪ����������ʽ�洢16λ����������
		/// </summary>
		/// <param name="v">��������</param>
		/// <returns>���ء���������ʽ�洢16λ����������</returns>
		public static Single FP16FromSingle(Int16 v)
		{
			// <will>
			return 0;
		}

		/// <summary>
		/// ������������ʽ�洢16λ��������תΪ��������
		/// </summary>
		/// <param name="v">��������ʽ�洢16λ��������</param>
		/// <returns>���ظ�������</returns>
		public static Int16 FP16ToSingle(Single v)
		{
			// <will>
			return 0;
		}

		#region IByteSwapable<Float16> ��Ա

		/// <summary>
		/// �����ֽ�˳�򣬲������¶���
		/// </summary>
		/// <returns>���ؽ������ֽ�˳�����¶���</returns>
		public Float16 swap()
		{
			//throw new Exception("The method or operation is not implemented.");
			unchecked
			{
				Float16 rt;
				//rt._raw = (Int16)(
				//    ((_raw >> 8) & 0xFF)
				//    | ((_raw & 0xFF) << 8)
				//    );
				rt._raw = ByteSwap.swap(_raw);
				return rt;
			}
		}

		/// <summary>
		/// ���� <seealso cref="Float16"/> ���ֽ�˳��
		/// </summary>
		/// <param name="v">ԭֵ��</param>
		/// <returns>�����ֽ�˳��ת���ֵ��</returns>
		public static Float16 swap(Float16 v)
		{
			return v.swap();
		}

		#endregion
	}
}
