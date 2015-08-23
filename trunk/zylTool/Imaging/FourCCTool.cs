/*
 * FourCCTool.cs
 * FourCC��
 * @Author zyl910
 * 
 * [2011-10-06]
 * ���塣
 * 
 * 
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace zylTool.Imaging
{
	/// <summary>
	/// FourCCTool ��һ����̬�࣬Ϊ���� FourCC �ṩ���㡣
	/// </summary>
	/// <remarks>
	/// FourCCȫ��Four-Character Codes����4���ַ���4 bytes����ɣ���һ�ֶ�����ʾ��Ƶ�ȶ�ý����������ʽ�����ֽڡ�ͨ����װΪ32λ�޷���������uint����
	/// </remarks>
	public static class FourCCTool
	{
		/// <summary>
		/// ��4���ַ�תΪFourCC��
		/// </summary>
		/// <param name="c0">�ַ�0��</param>
		/// <param name="c1">�ַ�1��</param>
		/// <param name="c2">�ַ�2��</param>
		/// <param name="c3">�ַ�3��</param>
		/// <returns>����FourCC��</returns>
		public static uint fromChar(char c0, char c1, char c2, char c3)
		{
			unchecked
			{
				uint rt = ((uint)c3) << 24
							| ((uint)c2) << 16
							| ((uint)c1) << 8
							| ((uint)c0);
				return rt;
			}
		}

		/// <summary>
		/// ���ַ���תΪFourCC��
		/// </summary>
		/// <param name="fourCC">FourCC�ַ�����</param>
		/// <returns>����FourCC��</returns>
		public static uint fromString(string strFourCC)
		{
			if ((null == strFourCC) || (strFourCC.Length != 4))
			{
				throw new ArgumentNullException("fourCC", "FourCC strings must be 4 characters long: " + strFourCC);
			}
			return fromChar(strFourCC[0], strFourCC[1], strFourCC[2], strFourCC[3]);
		}

		/// <summary>
		/// ��FourCCתΪ4���ַ���
		/// </summary>
		/// <param name="fourCC">FourCC��</param>
		/// <returns></returns>
		public static char[] convChars(uint fourCC)
		{
			unchecked
			{
				char[] chars = new char[4];
				chars[0] = (char)(fourCC & 0xFF);
				chars[1] = (char)((fourCC >> 8) & 0xFF);
				chars[2] = (char)((fourCC >> 16) & 0xFF);
				chars[3] = (char)((fourCC >> 24) & 0xFF);
				return chars;
			}
		}

		/// <summary>
		/// ��FourCCתΪ�ַ�����
		/// </summary>
		/// <param name="fourCC">FourCC��</param>
		/// <returns></returns>
		public static string convString(uint fourCC)
		{
			return new string(convChars(fourCC));
		}


	}

}
