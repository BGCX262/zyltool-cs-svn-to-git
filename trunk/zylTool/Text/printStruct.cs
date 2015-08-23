/*
 * printStruct.cs
 * ���ṹ���е���Ϣת��Ϊ�ɴ�ӡ���ַ�����
 * @Author zyl910
 * 
 * [2011-10-04]
 * ���塣
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
	/// ���ṹ���е���Ϣת��Ϊ�ɴ�ӡ���ַ���
	/// </summary>
	public static class printStruct
	{
		/// <summary>
		/// ��ȡΪ�˻�������Ļ����ַ���
		/// </summary>
		/// <value>������\r\n�����ַ��������ڷ� Unix ƽ̨�� �� ������\n�����ַ��������� Unix ƽ̨��</value>
		private readonly static string NewLine = Environment.NewLine;

		/// <summary>
		/// ��һ���������õ��ַ���
		/// </summary>
		private readonly static string IndentNext = "\t";

		/// <summary>
		/// Image�����Ϣ
		/// </summary>
		/// <param name="v">�������Ķ���</param>
		/// <param name="indentStr">����</param>
		/// <returns>�ɴ�ӡ���ַ�����</returns>
		public static string printImage(Image v, string indentStr)
		{
			string sNextIndent = indentStr + IndentNext;	// ��һ������
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
