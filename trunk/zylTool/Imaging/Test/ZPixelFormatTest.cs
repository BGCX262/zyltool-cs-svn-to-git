/*
 * ZPixelFormatTest.cs
 * 为处理 ZPixelFormat 的测试 提供方便。
 * @Author zyl910
 * 
 * [2011-10-07]
 * 定义。
 * 
 * 
 * 
 */
using System;
using System.Collections.Generic;
using System.Text;
// ## typedef ##	// C#不支持typedef，using仅对本模块内有效
using ZPixelFormat = System.UInt32;
//using Action_ZPixelFormat = System.Action<ZPixelFormat>;	// 居然不能识别ZPixelFormat。必须放在namespace中才行。
using zpf = zylTool.Imaging.ZPixelFormatTool;	// 为了减少代码量

namespace zylTool.Imaging.Test
{
	// ## typedef ##
	using Action_ZPixelFormat = System.Action<ZPixelFormat>;

	/// <summary>
	/// ZPixelFormatTest 是一个静态类，为处理 <see cref="ZPixelFormat"/> 的测试 提供方便。
	/// </summary>
	/// <remarks>
	/// 仅用于测试，不建议普通用户使用。
	/// </remarks>
	public static class ZPixelFormatTest
	{
		/// <summary>
		/// 枚举所有索引模式的。
		/// </summary>
		/// <param name="action">要对每个元素执行的 <see cref="Action"/>。</param>
		/// <remarks>
		/// 包括无效值。0至8位。
		/// </remarks>
		public static void ForEach_Indexed(Action_ZPixelFormat action)
		{
			ZPixelFormat pf;
			for (uint i = 0; i <= 8; ++i)
			{
				pf = zpf.zpfMake(zpf.ZPFT_PACKET8, 1, 0, i, zpf.ZPF_CHANNEL_INVALID, zpf.ZPF_CHANNEL_INVALID, zpf.ZPF_CHANNEL_INVALID);
				action(pf);
			}
		}
	}
}
