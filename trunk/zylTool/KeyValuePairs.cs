/*
 * KeyValuePairs.cs
 * 为处理 <seealso cref="System.Collections.Generic.KeyValuePair"/>数组 提供方便。。
 * @Author zyl910
 * 
 * [2011-10-05]
 * 定义至BitFlagsStrB。
 * 
 * [2011-10-06]
 * newKVP。
 * 重构：成员名称的首字母均改为小写。仅有常量和只读字段的首字母为大写，当做常量的只读字段也是大写。
 * 
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace zylTool
{
	/// <summary>
	/// KeyValuePairs 是一个静态类，为处理 <seealso cref="System.Collections.Generic.KeyValuePair"/>数组 提供方便。
	/// </summary>
	/// <remarks>
	/// 因为SortedDictionary等泛型集合类不允许在构造函数中填写内容，所以只能使用<seealso cref="System.Collections.Generic.KeyValuePair"/>数组 来定义常数。
	/// <para>如下——</para>
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
		/// 创建新的键值对。
		/// </summary>
		/// <typeparam name="TKey">键的类型。</typeparam>
		/// <typeparam name="TValue">值的类型。</typeparam>
		/// <param name="key">每个键/值对中定义的对象。</param>
		/// <param name="value">与 key 相关联的定义。</param>
		/// <returns>返回新的键值对。</returns>
		/// <remarks>
		/// 相当于直接使用KeyValuePair的构造函数，但省去了类型参数的填写。
		/// </remarks>
		public static KeyValuePair<TKey, TValue> newKVP<TKey, TValue>(TKey key, TValue value)
		{
			return new KeyValuePair<TKey, TValue>(key, value);
		}

		/// <summary>
		/// 在整个 <seealso cref="KeyValuePair"/>数组 中搜索指定键并返回从零开始的索引。 
		/// </summary>
		/// <typeparam name="TKey">键的类型。</typeparam>
		/// <typeparam name="TValue">值的类型。</typeparam>
		/// <param name="arr"><seealso cref="KeyValuePair"/>数组。</param>
		/// <param name="find">欲查找的键。</param>
		/// <returns>如果找到，则为整个 <seealso cref="KeyValuePair"/>数组 中 key 的从零开始的索引；否则为 -1。</returns>
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
		/// 在整个 <seealso cref="KeyValuePair"/>数组 中搜索指定值并返回从零开始的索引。 
		/// </summary>
		/// <typeparam name="TKey">键的类型。</typeparam>
		/// <typeparam name="TValue">值的类型。</typeparam>
		/// <param name="arr"><seealso cref="KeyValuePair"/>数组。</param>
		/// <param name="find">欲查找的值。</param>
		/// <returns>如果找到，则为整个 <seealso cref="KeyValuePair"/>数组 中 value 的从零开始的索引；否则为 -1。</returns>
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
		/// 在整个 <seealso cref="KeyValuePair"/>数组 中搜索指定键并返回其值。
		/// </summary>
		/// <typeparam name="TKey">键的类型。</typeparam>
		/// <typeparam name="TValue">值的类型。</typeparam>
		/// <param name="arr"><seealso cref="KeyValuePair"/>数组。</param>
		/// <param name="find">欲查找的键。</param>
		/// <returns>如果找到，就返回其值。否则返回 <typeparamref name="TValue"/> 的默认值。</returns>
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
		/// 在整个 <seealso cref="KeyValuePair"/>数组 中搜索指定键并返回其值，能指定默认值。
		/// </summary>
		/// <typeparam name="TKey">键的类型。</typeparam>
		/// <typeparam name="TValue">值的类型。</typeparam>
		/// <param name="arr"><seealso cref="KeyValuePair"/>数组。</param>
		/// <param name="find">欲查找的键。</param>
		/// <param name="defaultValue">指定的默认值。</param>
		/// <returns>如果找到，就返回其值。否则返回 <paramref name="defaultValue"/>。</returns>
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
		/// 尝试获取与指定的键相关联的值。
		/// </summary>
		/// <typeparam name="TKey">键的类型。</typeparam>
		/// <typeparam name="TValue">值的类型。</typeparam>
		/// <param name="arr"><seealso cref="KeyValuePair"/>数组。</param>
		/// <param name="find">欲查找的键。</param>
		/// <param name="value">当此方法返回时，如果找到指定键，则返回与该键相关联的值；否则，将返回 value 参数的类型的默认值。</param>
		/// <returns>如果 <seealso cref="KeyValuePair"/>数组 包含具有指定键的元素，则为 true；否则为 false。</returns>
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
		/// 解析位标识。
		/// </summary>
		/// <typeparam name="TValue">值的类型。</typeparam>
		/// <param name="arr"><seealso cref="KeyValuePair"/>数组。</param>
		/// <param name="flags">位标识</param>
		/// <param name="sIndent">缩进字符串。将会出现在每一行文本的左边。</param>
		/// <returns>返回位标识的解析结果，是一个包含多行文本的字符串。</returns>
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
		/// 解析位标识。StringBuilder版。
		/// </summary>
		/// <typeparam name="TValue">值的类型。</typeparam>
		/// <param name="sb">输出的StringBuilder。</param>
		/// <param name="arr"><seealso cref="KeyValuePair"/>数组。</param>
		/// <param name="flags">位标识</param>
		/// <param name="sIndent">缩进字符串。将会出现在每一行文本的左边。</param>
		/// <returns>返回向StringBuilder之中添加的行数。</returns>
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
