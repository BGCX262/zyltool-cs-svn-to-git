zylTool
@Author zyl910

zyl910的常用类库，基于 .Net 2.0 精简框架。

注意——
1.尽量保证与 .Net 2.0 精简框架 的兼容性。短期内不考虑增加新的条件编译常量。



条件编译
~~~~~~~~

　　条件编译常数有——
（无）


namespace
~~~~~~~~~

　　命名空间一览——
zylTool：根空间。可用来临时存放暂时不好分类的元素。
zylTool.Bits：位运算与字节序列处理。
zylTool.Imaging：图像处理。
zylTool.Imaging.Test：[Test] 图像处理的一些测试。
zylTool.Pointer：指针操作相关。
zylTool.Text：文本与字符串操作。

　　注——
[Code]：低级操作的代码，比较难用，且没有错误检查容易出错。不建议普通用户使用，而应该使用它的高级封装版。仅对类库开发者者有用。
[Test]：测试性代码。不建议普通用户使用。仅对测试者有用。


更新日志
~~~~~~~~

[2011-10-03]
正式定义工程
主要开发：像素格式相关。


[2011-10-07]
开始写Readme。
整合了zylTool.Pointer命名空间。
“原位处理”英语。最后决定采用“Self”。
<Save> 19:30 Copy Dell。
XNA对double的支持。发现X360下不支持BitConverter.DoubleToInt64Bits。
XNA对double的支持。检查X360下的引用项目中是否能用BitConverter.DoubleToInt64Bits。通过。
ILSpy分析.Net自身是怎么实现字节序处理的。

[2011-10-08]
固定长度的数组。发现必须用fixed。[MarshalAs]仅对序列化时有效，数组还是以对象的方式存在，联合体会破坏其结构。
zylTool.Bits.BitFloat32 结构。
zylTool.Bits.BitFloat64 结构。
zylTool.Bits.Float16 结构。
zylTool.FuncSame 委托。
IByteSwapable
swap<T>(T v)
swap<T>(T v, FuncSame<T> funcSwap)

[2011-10-09]
将VS2005设置为支持UTF8。暂时不将所有源码均转为UTF-8。
zylTool.Bits.Float16
zylTool.Bits.ByteSwap
zylTool.Bits.ByteOrder

[2011-10-10]
ByteOrder. 编写字节序的相关代码, 下一步准备靠泛型来优化架构。
zylTool.Bits.ByteSwap
zylTool.Bits.ByteOrder
zylTool.FuncSame: 移至FuncSame.cs

[2011-10-11]
程序集的版本设置.
增加samples\20111011 ByteOrder.
准备字节序处理的新架构.




will--
ByteSwap静态类
.像素格式处理
.指针偏移表
.字节序处理
.色彩空间转换
.像素纯数据复制
.色彩像素复制
.Alpha复制
.括号键值文本解析
.图像处理框架
.图像文件读写框架

