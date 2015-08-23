using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using zylTool;
using zylTool.Bits;

namespace ByteOrderTest
{
	public partial class FrmByteOrderTest : Form
	{
		public FrmByteOrderTest()
		{
			InitializeComponent();
		}

		/// <summary>
		/// 准备输出日志。
		/// </summary>
		/// <param name="s">输出日志。</param>
		internal void StartLog(string s)
		{
			if (txtLog.Lines.Length > 2000)
			{
				txtLog.Clear();
			}
			if (!string.IsNullOrEmpty(s))
			{
				txtLog.AppendText(s + Environment.NewLine);
			}
		}

		/// <summary>
		/// 准备输出日志。
		/// </summary>
		internal void StartLog()
		{
			StartLog(string.Empty);
		}

		/// <summary>
		/// 输出日志。
		/// </summary>
		/// <param name="s">输出日志。</param>
		internal void OutLog(string s)
		{
			txtLog.AppendText(s + Environment.NewLine);
		}

		private void testFuncSame<T>(T v, FuncSame<T> func)
		{
			OutLog(string.Format("{0}", func(v)));
		}

		private void btnTest1_Click(object sender, EventArgs e)
		{
			// 测试swap
			unchecked
			{
				Float16 f;
				f._raw = (short)0x89AB;
				//f = f.swap();
				//f = ByteSwap.swap(f);
				//f = ByteSwap.swap(f, Float16.swap);
				//f = ByteSwap.swap(f, ByteSwap.swap);
				OutLog(string.Format("{0:X}: {1}", f._raw, BitConverter.ToString(BitConverter.GetBytes(f._raw))));
				f = FuncSameTool.doCall(f, FuncSameTool.mov);
				OutLog(string.Format("{0:X}: {1}", f._raw, BitConverter.ToString(BitConverter.GetBytes(f._raw))));
				f = FuncSameTool.doCall(f, ByteSwap.swap);
				OutLog(string.Format("{0:X}: {1}", f._raw, BitConverter.ToString(BitConverter.GetBytes(f._raw))));
			}
			//// 测试byteorder
			//{
			//    int i = 0x01234567;
			//    byte[] buf = ByteOrder.getBytes(i);
			//    OutLog(string.Format("{0}", BitConverter.ToString(buf)));
			//    unsafe
			//    {
			//        fixed (byte* pBuf = &buf[0])
			//        {
			//            ByteOrder.bytesTo(ref i, pBuf, buf.Length);
			//        }
			//    }
			//    OutLog(string.Format("{0:X}", i));
			//}
		}

	}
}