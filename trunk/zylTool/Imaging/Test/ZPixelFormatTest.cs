/*
 * ZPixelFormatTest.cs
 * Ϊ���� ZPixelFormat �Ĳ��� �ṩ���㡣
 * @Author zyl910
 * 
 * [2011-10-07]
 * ���塣
 * 
 * 
 * 
 */
using System;
using System.Collections.Generic;
using System.Text;
// ## typedef ##	// C#��֧��typedef��using���Ա�ģ������Ч
using ZPixelFormat = System.UInt32;
//using Action_ZPixelFormat = System.Action<ZPixelFormat>;	// ��Ȼ����ʶ��ZPixelFormat���������namespace�в��С�
using zpf = zylTool.Imaging.ZPixelFormatTool;	// Ϊ�˼��ٴ�����

namespace zylTool.Imaging.Test
{
	// ## typedef ##
	using Action_ZPixelFormat = System.Action<ZPixelFormat>;

	/// <summary>
	/// ZPixelFormatTest ��һ����̬�࣬Ϊ���� <see cref="ZPixelFormat"/> �Ĳ��� �ṩ���㡣
	/// </summary>
	/// <remarks>
	/// �����ڲ��ԣ���������ͨ�û�ʹ�á�
	/// </remarks>
	public static class ZPixelFormatTest
	{
		/// <summary>
		/// ö����������ģʽ�ġ�
		/// </summary>
		/// <param name="action">Ҫ��ÿ��Ԫ��ִ�е� <see cref="Action"/>��</param>
		/// <remarks>
		/// ������Чֵ��0��8λ��
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
