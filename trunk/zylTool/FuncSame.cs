/*
 * FuncSame.cs
 * FuncSameί�м���ش���
 * @Author zyl910
 * 
 * [2011-10-10]
 * FuncSameί�С�Դ��delegate.cs��
 * ���� FuncSameTool��̬�ࡣ
 * 
 * 
 */
using System;
using System.Collections.Generic;
//using System.Text;

namespace zylTool
{
	/// <summary>
	/// ��װһ���������÷����в����뷵��ֵ��������ͬ��
	/// </summary>
	/// <typeparam name="T">��������͡�</typeparam>
	/// <param name="arg">������</param>
	/// <returns>����ֵ��</returns>
	/// <remarks>
	/// ��;����
	/// <para>1.ת�����������������д�����ٷ��ء�</para>
	/// </remarks>
	public delegate T FuncSame<T>(T arg);

	/// <summary>
	/// FuncSameTool��һ����̬�࣬Ϊ���� <seealso cref="FuncSame"/>ί�� �ṩ���㡣
	/// </summary>
	public static class FuncSameTool
	{
		/// <summary>
		/// ���ú��������� <seealso cref="FuncSame"/> ί����ת�����ݡ�
		/// </summary>
		/// <typeparam name="T">��������͡�</typeparam>
		/// <param name="v">ԭֵ��</param>
		/// <param name="func">��������</param>
		/// <returns>���ش�����ֵ��</returns>
		public static T doCall<T>(T v, FuncSame<T> func)
		{
			return func(v);
		}

		
		/// <summary>
		/// ��ֵ������������ԭֵ�������κδ���������Ϊ <seealso cref="FuncSame"/> ί�С�
		/// </summary>
		/// <typeparam name="T">��������͡�</typeparam>
		/// <param name="v">ԭֵ��</param>
		/// <returns>����ԭֵ��</returns>
		public static T mov<T>(T v)
		{
			return v;
		}

	}
}
