/*
 * ByteSwap.cs
 * [Code] �����ֽ�˳��
 * @Author zyl910
 * 
 * [2011-10-07]
 * ���塣����չ��
 * 
 * [2011-10-08]
 * IByteSwapable
 * swap<T>(T v)
 * swap<T>(T v, FuncSame<T> funcSwap)
 * 
 * [2011-10-09]
 * �Ż������̣�������ע�͡�
 * swap(UInt16 v)
 * swap(Int16 v)
 * ע������δʵ�ֵ�swap(Int64 v)��
 * 
 * [2011-10-10]
 * T mov<T>(T v)
 * 
 * 
 */
using System;
using System.Collections.Generic;
//using System.Text;

namespace zylTool.Bits
{
	/// <summary>
	/// ֧�ֽ����ֽ�˳��һ������Խṹ�壬����ÿ���ֶε��ֽ�˳��
	/// </summary>
	/// <typeparam name="T">��������͡���д�ṹ��ʵ�ֱ��ӿ�ʱ��Ӧ���ݱ��ṹ����������͡�</typeparam>
	public interface IByteSwapable<T>
	{
		/// <summary>
		/// �����ֽ�˳�򣬲������¶���
		/// </summary>
		/// <returns>���ؽ������ֽ�˳�����¶���</returns>
		T swap();
	}

	/// <summary>
	/// �����ֽ�˳��
	/// </summary>
	public static class ByteSwap
	{
		/// <summary>
		/// ���� <seealso cref="IByteSwapable"/> �ӿ��������ֽ�˳��
		/// </summary>
		/// <typeparam name="T">��������͡�</typeparam>
		/// <param name="v">ԭֵ��</param>
		/// <returns>���ؽ����ֽ�˳����ֵ��</returns>
		public static T swap<T>(T v) where T : IByteSwapable<T>
		{
			return v.swap();
		}

		/// <summary>
		/// ���� <seealso cref="FuncSame"/> ί���������ֽ�˳��
		/// </summary>
		/// <typeparam name="T">��������͡�</typeparam>
		/// <param name="v">ԭֵ��</param>
		/// <param name="funcSwap">�����ֽ�˳��ĺ�����</param>
		/// <returns>���ؽ����ֽ�˳����ֵ��</returns>
		/// <remarks>
		/// ����ʹ�� FuncSameTool.doCall ������
		/// </remarks>
		public static T swap<T>(T v, FuncSame<T> funcSwap)
		{
			return funcSwap(v);
		}

		/// <summary>
		/// ���� <seealso cref="Boolean"/> ���ֽ�˳��
		/// </summary>
		/// <param name="v">ԭֵ��</param>
		/// <returns>���ؽ����ֽ�˳����ֵ��</returns>
		/// <remarks>
		/// ��Ȼ�ĵ���û����ȷ˵Boolean�Ƕ����ֽڡ����� <seealso cref="System.IO.BinaryReader"/> ����1�ֽڡ�
		/// </remarks>
		public static Boolean swap(Boolean v)
		{
			return v;
		}

		/// <summary>
		/// ���� <seealso cref="Byte"/> ���ֽ�˳��
		/// </summary>
		/// <param name="v">ԭֵ��</param>
		/// <returns>���ؽ����ֽ�˳����ֵ��</returns>
		public static Byte swap(Byte v)
		{
			return v;	// 1Byte
		}

		/// <summary>
		/// ���� <seealso cref="SByte"/> ���ֽ�˳��
		/// </summary>
		/// <param name="v">ԭֵ��</param>
		/// <returns>���ؽ����ֽ�˳����ֵ��</returns>
		public static SByte swap(SByte v)
		{
			return v;	// 1Byte
		}

		/// <summary>
		/// ���� <seealso cref="UInt16"/> ���ֽ�˳��
		/// </summary>
		/// <param name="v">ԭֵ��</param>
		/// <returns>���ؽ����ֽ�˳����ֵ��</returns>
		public static UInt16 swap(UInt16 v)
		{
			unchecked
			{
				return (UInt16)(((v >> 8) & 0xFF) | ((v & 0xFF) << 8));
			}
		}

		/// <summary>
		/// ���� <seealso cref="Int16"/> ���ֽ�˳��
		/// </summary>
		/// <param name="v">ԭֵ��</param>
		/// <returns>���ؽ����ֽ�˳����ֵ��</returns>
		public static Int16 swap(Int16 v)
		{
			unchecked
			{
				return (Int16)swap((UInt16)v);
			}
		}

		/*public static Int64 swap(Int64 v)
		{
			return v;	// <will>
		}

		public static Double swap(Double v)
		{
			unchecked
			{
				Int64 u = BitConverter.DoubleToInt64Bits(v);
				u = swap(u);
				return BitConverter.Int64BitsToDouble(u);
			}
		}*/

	}
}
