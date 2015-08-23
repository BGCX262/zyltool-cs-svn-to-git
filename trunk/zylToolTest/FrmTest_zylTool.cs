/*
 * FrmTest_zylTool.cs
 * 测试zylTool。
 * @Author zyl910
 * 
 * [2011-10-03]
 * 定义。
 * 
 * [2011-10-07]
 * 改名：FrmTest_zylTool。
 * 给界面加上菜单、面板和文本框。实现测试界面的框架。
 * OutLog
 * StartLog
 * 
 * 
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using zylTool;
using zylTool.Bits;
using zylTool.Imaging;
using zylTool.Imaging.Test;
using zylTool.Pointer;

namespace zylToolTest
{
	//struct Pair
	//{
	//    int a;
	//    int b;
	//}

	public partial class FrmTest_zylTool : Form
	{
		public FrmTest_zylTool()
		{
			InitializeComponent();
			//Pair[] arr = { { 1, 2 } };
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

		protected virtual void onTest()
		{
		}

		private void mnuFileExit_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void testFuncSame<T>(T v, FuncSame<T> func)
		{
			OutLog(string.Format("{0}", func(v)));
		}

		private void btnTest_Click(object sender, EventArgs e)
		{
			StartLog();
			OutLog("Test!");
			//// 测试联合体
			//unsafe
			//{
			//    BitFloat64 bf = new BitFloat64();
			//    OutLog(string.Format("{0}", sizeof(BitFloat64)));
			//    OutLog(string.Format("{0}", Marshal.SizeOf(bf)));
			//    //bf.sd = -1;
			//    bf.f[0] = -1;
			//    OutLog(string.Format("{0}", bf.sd[0]));
			//    OutLog(string.Format("{0}", bf.ud[0]));
			//    OutLog(string.Format("{0}", bf.uw[1]));
			//    OutLog(string.Format("{0}", bf.ub[3]));
			//    OutLog(string.Format("{0}", BitConverter.ToString(BitConverter.GetBytes(bf.uq))));
			//    OutLog(string.Format("{0}", bf.d));
			//}
			//// 测试委托的返回值
			//FuncSame<int> f = delegate(int arg)
			//{
			//    return arg + 1;
			//};
			//OutLog(string.Format("{0}", f(0)));
			//int i = 0;
			//testFuncSame(i, f);
			//testFuncSame(0, f);
			//testFuncSame(0, delegate(int arg)
			//{
			//    return arg - 1;
			//});
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

		private void btnTest1_Click(object sender, EventArgs e)
		{
			//
			#region PointerClamp
			unsafe
			{
				unchecked
				{
					StringBuilder sOut = new StringBuilder();
					//long* m_OffsetPointer = null;	// 指针偏移表的基地址
					int m_OffsetSize;	//	指针偏移表的尺寸（元素的个数）
					int m_OffsetBase;	// 指针偏移表中索引的基础偏移。定义：f(i) = OffsetPointer[OffsetBase + i]。
					long m_Stride;	// 指针的跨距
					long m_LimitStart;	// 限定区域左界
					long m_LimitSize;	// 限定区域的尺寸
					long m_Pos;	// 当前位置。有可能会在基于限定区域外，但因已经修正了指针偏移表，仍保证指针不会越界。

					// 调整参数
					m_OffsetSize = 5;
					m_OffsetBase = m_OffsetSize / 2;
					m_Stride = 1;
					m_LimitStart = 0;
					m_LimitSize = 6;

					// 初始化
					long* m_OffsetPointer = stackalloc long[m_OffsetSize];	// stackalloc只能用于定义变量时。不方便啊。
					IPointerOffset ipo = PointerTool.NewPointerOffset(PointerAddressingMode.Border);
					ipo.Init(m_OffsetPointer, m_OffsetSize, m_OffsetBase, m_Stride, m_LimitStart, m_LimitSize);

					// test
					bool bErr;
					sOut.AppendFormat("pos");
					for (int i = 0; i < m_OffsetSize; ++i)
					{
						sOut.AppendFormat("\tO{0}", (i - m_OffsetBase).ToString());
					}
					sOut.AppendLine();
					m_Pos = m_LimitStart - m_OffsetSize;
					ipo.Pos = m_Pos;
					for (; m_Pos < (m_LimitStart + m_LimitSize + m_OffsetSize); ++m_Pos)
					{
						//PointerOffsetClamp.ClampInit_Base(m_Pos, m_OffsetPointer, m_OffsetSize, m_OffsetBase, m_Stride, m_LimitStart, m_LimitSize);
						//PointerOffsetClamp.ClampInit_Fast(m_Pos, m_OffsetPointer, m_OffsetSize, m_OffsetBase, m_Stride, m_LimitStart, m_LimitSize);
						//ipo.Pos = m_Pos;
						// 输出
						sOut.AppendFormat("P{0}", m_Pos.ToString());
						bErr = false;
						for (int i = 0; i < m_OffsetSize; ++i)
						{
							long t = m_OffsetPointer[i];	// 得到修正值
							t = m_Pos + t;	// 进行修正
							if (t < m_LimitStart)
								bErr = true;
							if (t >= (m_LimitStart + m_LimitSize))
								bErr = true;
							sOut.AppendFormat("\t{0}", t.ToString());
						}
						if (bErr)
							sOut.Append("\tError");
						sOut.AppendLine();
						// next
						ipo.MoveNext();
					}
					m_Pos = m_LimitStart + m_LimitSize + m_OffsetSize;
					ipo.Pos = m_Pos;
					for (; m_Pos > (m_LimitStart - m_OffsetSize); m_Pos -= 2)
					{
						//ipo.Pos = m_Pos;
						// 输出
						sOut.AppendFormat("P{0}", m_Pos.ToString());
						bErr = false;
						for (int i = 0; i < m_OffsetSize; ++i)
						{
							long t = m_OffsetPointer[i];	// 得到修正值
							t = m_Pos + t;	// 进行修正
							if (t < m_LimitStart)
								bErr = true;
							if (t >= (m_LimitStart + m_LimitSize))
								bErr = true;
							sOut.AppendFormat("\t{0}", t.ToString());
						}
						if (bErr)
							sOut.Append("\tError");
						sOut.AppendLine();
						// next
						ipo.MoveScroll(-2);
					}

					sOut.AppendLine();
					txtLog.AppendText(sOut.ToString());
				}
			}
			#endregion	// PointerClamp

		}

		private void zpfBetterToolStripMenuItem_Click(object sender, EventArgs e)
		{
			StartLog("[ZPixelFormatTool.zpfBetter]");
			ZPixelFormatTest.ForEach_Indexed(delegate(UInt32 pf)
			{
				UInt32 pf2 = ZPixelFormatTool.zpfBetter(pf);
				OutLog(string.Format("{0:X8} ==>\t{1:X8}", pf, pf2));
			});
			OutLog("");
		}
	}
}