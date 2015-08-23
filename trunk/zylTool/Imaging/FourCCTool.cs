/*
 * FourCCTool.cs
 * FourCC。
 * @Author zyl910
 * 
 * [2011-10-06]
 * 定义。
 * 
 * 
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace zylTool.Imaging
{
	/// <summary>
	/// FourCCTool 是一个静态类，为处理 FourCC 提供方便。
	/// </summary>
	/// <remarks>
	/// FourCC全称Four-Character Codes，由4个字符（4 bytes）组成，是一种独立标示视频等多媒体数据流格式的四字节。通常封装为32位无符号整数（uint）。
	/// </remarks>
	public static class FourCCTool
	{
		/// <summary>
		/// 将4个字符转为FourCC。
		/// </summary>
		/// <param name="c0">字符0。</param>
		/// <param name="c1">字符1。</param>
		/// <param name="c2">字符2。</param>
		/// <param name="c3">字符3。</param>
		/// <returns>返回FourCC。</returns>
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
		/// 将字符串转为FourCC。
		/// </summary>
		/// <param name="fourCC">FourCC字符串。</param>
		/// <returns>返回FourCC。</returns>
		public static uint fromString(string strFourCC)
		{
			if ((null == strFourCC) || (strFourCC.Length != 4))
			{
				throw new ArgumentNullException("fourCC", "FourCC strings must be 4 characters long: " + strFourCC);
			}
			return fromChar(strFourCC[0], strFourCC[1], strFourCC[2], strFourCC[3]);
		}

		/// <summary>
		/// 将FourCC转为4个字符。
		/// </summary>
		/// <param name="fourCC">FourCC。</param>
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
		/// 将FourCC转为字符串。
		/// </summary>
		/// <param name="fourCC">FourCC。</param>
		/// <returns></returns>
		public static string convString(uint fourCC)
		{
			return new string(convChars(fourCC));
		}


	}

}
