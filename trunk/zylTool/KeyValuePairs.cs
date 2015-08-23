/*
 * KeyValuePairs.cs
 * Ϊ���� <seealso cref="System.Collections.Generic.KeyValuePair"/>���� �ṩ���㡣��
 * @Author zyl910
 * 
 * [2011-10-05]
 * ������BitFlagsStrB��
 * 
 * [2011-10-06]
 * newKVP��
 * �ع�����Ա���Ƶ�����ĸ����ΪСд�����г�����ֻ���ֶε�����ĸΪ��д������������ֻ���ֶ�Ҳ�Ǵ�д��
 * 
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace zylTool
{
	/// <summary>
	/// KeyValuePairs ��һ����̬�࣬Ϊ���� <seealso cref="System.Collections.Generic.KeyValuePair"/>���� �ṩ���㡣
	/// </summary>
	/// <remarks>
	/// ��ΪSortedDictionary�ȷ��ͼ����಻�����ڹ��캯������д���ݣ�����ֻ��ʹ��<seealso cref="System.Collections.Generic.KeyValuePair"/>���� �����峣����
	/// <para>���¡���</para>
	/// <code>
	/// private readonly static KeyValuePair&lt;int, string&gt;[] kvpArray = {
	/// 	new KeyValuePair&lt;int, string&gt;(1, "One")
	/// 	,new KeyValuePair&lt;int, string&gt;(2, "Two")
	/// };
	/// </code>
	/// </remarks>
	public static class KeyValuePairs
	{
		/// <summary>
		/// �����µļ�ֵ�ԡ�
		/// </summary>
		/// <typeparam name="TKey">�������͡�</typeparam>
		/// <typeparam name="TValue">ֵ�����͡�</typeparam>
		/// <param name="key">ÿ����/ֵ���ж���Ķ���</param>
		/// <param name="value">�� key ������Ķ��塣</param>
		/// <returns>�����µļ�ֵ�ԡ�</returns>
		/// <remarks>
		/// �൱��ֱ��ʹ��KeyValuePair�Ĺ��캯������ʡȥ�����Ͳ�������д��
		/// </remarks>
		public static KeyValuePair<TKey, TValue> newKVP<TKey, TValue>(TKey key, TValue value)
		{
			return new KeyValuePair<TKey, TValue>(key, value);
		}

		/// <summary>
		/// ������ <seealso cref="KeyValuePair"/>���� ������ָ���������ش��㿪ʼ�������� 
		/// </summary>
		/// <typeparam name="TKey">�������͡�</typeparam>
		/// <typeparam name="TValue">ֵ�����͡�</typeparam>
		/// <param name="arr"><seealso cref="KeyValuePair"/>���顣</param>
		/// <param name="find">�����ҵļ���</param>
		/// <returns>����ҵ�����Ϊ���� <seealso cref="KeyValuePair"/>���� �� key �Ĵ��㿪ʼ������������Ϊ -1��</returns>
		public static int indexOfKey<TKey, TValue>(KeyValuePair<TKey, TValue>[] arr, TKey find)
			where TKey : IComparable
		{
			if (null == arr) throw new ArgumentNullException("arr");
			for (int i = 0; i < arr.Length; ++i)
			{
				if (0 == find.CompareTo(arr[i].Key))
					return i;
			}
			return -1;
		}

		/// <summary>
		/// ������ <seealso cref="KeyValuePair"/>���� ������ָ��ֵ�����ش��㿪ʼ�������� 
		/// </summary>
		/// <typeparam name="TKey">�������͡�</typeparam>
		/// <typeparam name="TValue">ֵ�����͡�</typeparam>
		/// <param name="arr"><seealso cref="KeyValuePair"/>���顣</param>
		/// <param name="find">�����ҵ�ֵ��</param>
		/// <returns>����ҵ�����Ϊ���� <seealso cref="KeyValuePair"/>���� �� value �Ĵ��㿪ʼ������������Ϊ -1��</returns>
		public static int indexOfValue<TKey, TValue>(KeyValuePair<TKey, TValue>[] arr, TValue find)
			where TValue : IComparable
		{
			if (null == arr)	throw new ArgumentNullException("arr");
			for (int i = 0; i < arr.Length; ++i)
			{
				if (0 == find.CompareTo(arr[i].Value))
					return i;
			}
			return -1;
		}

		/// <summary>
		/// ������ <seealso cref="KeyValuePair"/>���� ������ָ������������ֵ��
		/// </summary>
		/// <typeparam name="TKey">�������͡�</typeparam>
		/// <typeparam name="TValue">ֵ�����͡�</typeparam>
		/// <param name="arr"><seealso cref="KeyValuePair"/>���顣</param>
		/// <param name="find">�����ҵļ���</param>
		/// <returns>����ҵ����ͷ�����ֵ�����򷵻� <typeparamref name="TValue"/> ��Ĭ��ֵ��</returns>
		public static TValue find<TKey, TValue>(KeyValuePair<TKey, TValue>[] arr, TKey find)
			where TKey : IComparable
		{
			if (null == arr) throw new ArgumentNullException("arr");
			foreach (KeyValuePair<TKey, TValue> kv in arr)
				if (0 == kv.Key.CompareTo(find))
					return kv.Value;
			return default(TValue);
		}

		/// <summary>
		/// ������ <seealso cref="KeyValuePair"/>���� ������ָ������������ֵ����ָ��Ĭ��ֵ��
		/// </summary>
		/// <typeparam name="TKey">�������͡�</typeparam>
		/// <typeparam name="TValue">ֵ�����͡�</typeparam>
		/// <param name="arr"><seealso cref="KeyValuePair"/>���顣</param>
		/// <param name="find">�����ҵļ���</param>
		/// <param name="defaultValue">ָ����Ĭ��ֵ��</param>
		/// <returns>����ҵ����ͷ�����ֵ�����򷵻� <paramref name="defaultValue"/>��</returns>
		public static TValue find<TKey, TValue>(KeyValuePair<TKey, TValue>[] arr, TKey find, TValue defaultValue)
			where TKey : IComparable
		{
			if (null == arr) throw new ArgumentNullException("arr");
			foreach (KeyValuePair<TKey, TValue> kv in arr)
				if (0 == kv.Key.CompareTo(find))
					return kv.Value;
			return defaultValue;
		}

		/// <summary>
		/// ���Ի�ȡ��ָ���ļ��������ֵ��
		/// </summary>
		/// <typeparam name="TKey">�������͡�</typeparam>
		/// <typeparam name="TValue">ֵ�����͡�</typeparam>
		/// <param name="arr"><seealso cref="KeyValuePair"/>���顣</param>
		/// <param name="find">�����ҵļ���</param>
		/// <param name="value">���˷�������ʱ������ҵ�ָ�������򷵻���ü��������ֵ�����򣬽����� value ���������͵�Ĭ��ֵ��</param>
		/// <returns>��� <seealso cref="KeyValuePair"/>���� ��������ָ������Ԫ�أ���Ϊ true������Ϊ false��</returns>
		public static bool tryGetValue<TKey, TValue>(KeyValuePair<TKey, TValue>[] arr, TKey find, out TValue value)
			where TKey : IComparable
		{
			if (null == arr) throw new ArgumentNullException("arr");
			foreach (KeyValuePair<TKey, TValue> kv in arr)
			{
				if (0 == kv.Key.CompareTo(find))
				{
					value = kv.Value;
					return true;
				}
			}
			value = default(TValue);
			return false;
		}

		/// <summary>
		/// ����λ��ʶ��
		/// </summary>
		/// <typeparam name="TValue">ֵ�����͡�</typeparam>
		/// <param name="arr"><seealso cref="KeyValuePair"/>���顣</param>
		/// <param name="flags">λ��ʶ</param>
		/// <param name="sIndent">�����ַ��������������ÿһ���ı�����ߡ�</param>
		/// <returns>����λ��ʶ�Ľ����������һ�����������ı����ַ�����</returns>
		public static string bitFlagsStr<TValue>(KeyValuePair<uint, TValue>[] arr, uint flags, string sIndent)
		{
			if (null == arr) throw new ArgumentNullException("arr");
			string sAll = "";
			foreach (KeyValuePair<uint, TValue> kv in arr)
				if (0 != (kv.Key & flags))
					sAll += sIndent + kv.Value.ToString() + Environment.NewLine;
			return sAll;
		}

		/// <summary>
		/// ����λ��ʶ��StringBuilder�档
		/// </summary>
		/// <typeparam name="TValue">ֵ�����͡�</typeparam>
		/// <param name="sb">�����StringBuilder��</param>
		/// <param name="arr"><seealso cref="KeyValuePair"/>���顣</param>
		/// <param name="flags">λ��ʶ</param>
		/// <param name="sIndent">�����ַ��������������ÿһ���ı�����ߡ�</param>
		/// <returns>������StringBuilder֮����ӵ�������</returns>
		public static int bitFlagsStrB<TValue>(StringBuilder sb, KeyValuePair<uint, TValue>[] arr, uint flags, string sIndent)
		{
			if (null == sb) throw new ArgumentNullException("sb");
			if (null == arr) throw new ArgumentNullException("arr");
			int cnt = 0;
			foreach (KeyValuePair<uint, TValue> kv in arr)
			{
				if (0 != (kv.Key & flags))
				{
					sb.AppendLine(sIndent + kv.Value.ToString());
					++cnt;
				}
			}
			return cnt;
		}

	}
}
