/*
 * ByteOrder.cs
 * �ֽ�����
 * @Author zyl910
 * 
 * [2011-10-09]
 * ���塣����չ��
 * 
 * [2011-10-10]
 * T convToX<T>(bool XIsLittleEndian, T v)
 * T convToX<T>(T v, FuncSame<T> funcSwap)
 * 
 */
using System;
using System.Collections.Generic;
//using System.Text;
using System.Runtime.InteropServices;

namespace zylTool.Bits
{
	/// <summary>
	/// �ֽ�����
	/// </summary>
	public static class ByteOrder
	{
		#region by X	// ���ݲ���

		/// <summary>
		/// ���� <seealso cref="IByteSwapable"/> �ӿڣ��� �����ֽ������� תΪָ���ֽ���
		/// </summary>
		/// <typeparam name="T">��������͡�</typeparam>
		/// <param name="XIsLittleEndian">ָ�����ֽ����ǲ���С��ģʽ��</param>
		/// <param name="v">ԭֵ��</param>
		/// <returns>����ָ���ֽ�������</returns>
		/// <remarks>
		/// ע���ú������ɽ� ָ���ֽ������� תΪ �����ֽ���
		/// </remarks>
		public static T convToX<T>(bool XIsLittleEndian, T v) where T : IByteSwapable<T>
		{
			if (XIsLittleEndian == BitConverter.IsLittleEndian)
			{
				// �ֽ�����ͬ��ֱ�ӷ���ԭֵ��
				return v;
			}
			else
			{
				// �ֽ���ͬ�������ֽ���ߵ����ֵ��
				return v.swap();
			}
		}

		/// <summary>
		/// ���� <seealso cref="FuncSame"/> ί�У��� �����ֽ������� תΪָ���ֽ���
		/// </summary>
		/// <typeparam name="T">��������͡�</typeparam>
		/// <param name="XIsLittleEndian">ָ�����ֽ����ǲ���С��ģʽ��</param>
		/// <param name="v">ԭֵ��</param>
		/// <param name="funcSwap">�����ֽ�˳��ĺ�����</param>
		/// <returns>ָ���ֽ�������</returns>
		/// <remarks>
		/// ע���ú������ɽ� ָ���ֽ������� תΪ �����ֽ���
		/// </remarks>
		public static T convToX<T>(bool XIsLittleEndian, T v, FuncSame<T> funcSwap)
		{
			if (XIsLittleEndian == BitConverter.IsLittleEndian)
			{
				return v;
			}
			else
			{
				return funcSwap(v);
			}
		}

		#endregion	// by X	// ����X����

		/// <summary>
		/// ��ָ��������ת��Ϊ�ֽ����顣���� <seealso cref="System.Runtime.InteropServices.Marshal"/> �ࡣ
		/// </summary>
		/// <param name="v">���ݡ�</param>
		/// <returns>�����ֽ����顣</returns>
		/// <exception cref="System.ArgumentNullException"><c>Marshal.SizeOf(Object)</c></exception>
		public static byte[] getBytes(object v)
		{
			int cb = Marshal.SizeOf(v);
			if (cb > 0)
			{
				byte[] buf = new byte[cb];
				unsafe
				{
					fixed (void* p = &buf[0])
					{
						Marshal.StructureToPtr(v, new IntPtr(p), false);
					}
				}
				return buf;
			}
			return new byte[0];
		}

		// [Bug]IL ��ʽ����ȷ����ΪStructureToPtr��Ҫobject��
		//public static byte[] getBytes<T>(T v)
		//{
		//    int cb = Marshal.SizeOf(v);
		//    if (cb > 0)
		//    {
		//        byte[] buf = new byte[cb];
		//        unsafe
		//        {
		//            fixed (void* p = &buf[0])
		//            {
		//                Marshal.StructureToPtr(v, new IntPtr(p), false);
		//            }
		//        }
		//        return buf;
		//    }
		//    return new byte[0];
		//}

		/// <exception cref="System.ArgumentNullException"><c>Marshal.SizeOf(Object)</c></exception>
		public unsafe static object bytesTo(Type t, byte* pBuf, int bytesBuf)
		{
			int cb = Marshal.SizeOf(t);
			if ((cb > 0) && (cb <= bytesBuf))
			{
				return Marshal.PtrToStructure(new IntPtr((void*)pBuf), t);
			}
			return null;
		}

		/// <exception cref="System.ArgumentNullException"><c>Marshal.SizeOf(Object)</c></exception>
		public unsafe static int bytesTo<T>(ref T v, byte* pBuf, int bytesBuf)
		{
			Type t = v.GetType();
			int cb = Marshal.SizeOf(t);
			if ((cb > 0) && (cb <= bytesBuf))
			{
				object obj = Marshal.PtrToStructure(new IntPtr((void*)pBuf), t);
				if (null != obj)
				{
					v = (T)obj;
					return cb;
				}
			}
			return 0;
		}

	}
}
