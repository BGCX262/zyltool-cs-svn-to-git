/*
 * printStruct.cs
 * 将结构体中的信息转化为可打印的字符串。
 * @Author zyl910
 * 
 * [2011-10-04]
 * 定义。
 * 
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace zylTool.Text
{
	/// <summary>
	/// 将结构体中的信息转化为可打印的字符串
	/// </summary>
	public static class printStruct
	{
		/// <summary>
		/// 获取为此环境定义的换行字符串
		/// </summary>
		/// <value>包含“\r\n”的字符串，用于非 Unix 平台， 或 包含“\n”的字符串，用于 Unix 平台。</value>
		private readonly static string NewLine = Environment.NewLine;

		/// <summary>
		/// 下一级缩进所用的字符串
		/// </summary>
		private readonly static string IndentNext = "\t";

		/// <summary>
		/// Image类的信息
		/// </summary>
		/// <param name="v">欲分析的对象</param>
		/// <param name="indentStr">缩进</param>
		/// <returns>可打印的字符串。</returns>
		public static string printImage(Image v, string indentStr)
		{
			string sNextIndent = indentStr + IndentNext;	// 下一级缩进
			string sAll = string.Format(indentStr + "[Image]" + NewLine);
			if (null == v)
			{
				sAll += string.Format(indentStr + "null" + NewLine);
			}
			else
			{
			}
			return sAll;
		}

	}
}
