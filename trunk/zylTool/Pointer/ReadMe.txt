zylTool.Pointer

zylTool.Pointer名称空间专门用来存放指针操作的相关类。

注意——
1.所有指针操作的偏移量均是64位（long）。因为指针减法的结果是long，且实际测试过指针运算时int、long的速度一样快。
2.为了性能，很多函数不会做参数检查、越界检查、空指针检查等检查。请小心使用，注意初始化，避免传递无效值。

设计如下——
PointerTool：为指针操作提供常数和静态方法。枚举定义也写在该文件
IPointerOffset：指针偏移接口
PointerOffsetClamp：指针偏移-限制模式


public enum PointerAddressMode
{
	Unknown = 0,
	Border,
	Clamp,
	Wrap,
	Mirror,
	MirrorOnce
}



函数示意——
---[/]---：边框（Border）。越界后返回边框特殊值。
___[/]^^^：限制（Clamp）。将越界值限制在边界。
///[/]///：环绕（Wrap）。重复出现。
\/\[/]\/\：镜像（Mirror）。即这样的重复模式：原、反向、原、反向……。
^^\[/]^^^：一次镜像（MirrorOnce）。仅对左界做一次镜像，然后在做限制处理。
符号解释——
[/]：原函数（例如递增函数）。
-：边框特殊值。
_：原函数左界的值。
^：原函数右界的值。
/：原函数的重复（例如递增函数）。
\：原函数的镜像（例如递减函数）。


数值解释。假设原函数是[1234]，那么——
~~~~.~~~~[1234]~~~~.~~~~：边框（Border）。越界后返回边框特殊值。
1111.1111[1234]4444.4444：限制（Clamp）。将越界值限制在边界。
1234.1234[1234]1234.1234：环绕（Wrap）。重复出现。
1234.4321[1234]4321.1234：镜像（Mirror）。即这样的重复模式：原、反向、原、反向……。
4444.4321[1234]4444.4444：一次镜像（MirrorOnce）。仅对左界做一次镜像，然后在做限制处理。


资料备份
~~~~~~~~



D3DTEXTUREADDRESS.D3DTADDRESS_WRAP：环绕。重复出现，假设u的值是0-3，那么就重复出现3次
D3DTEXTUREADDRESS.D3DTADDRESS_MIRROR：镜像。两两之间视为镜像
D3DTEXTUREADDRESS.D3DTADDRESS_CLAMP：限制。把纹理坐标截取到[0.0, 1.0]范围内，也就是说，这种模式只应用纹理一次，然后就重复使用纹理边缘处像素的颜色。
D3DTEXTUREADDRESS.D3DTADDRESS_BORDER：边框。在[0.0,1.0]外设置边框颜色
D3DTEXTUREADDRESS.D3DTADDRESS_MIRRORONCE：一次镜像。纹理在[-1.0, 1.0]范围内作镜像，在该范围外作限制。
D3DSAMP_BORDERCOLOR：边框颜色


http://www.gesoftfactory.com/developer/Textures.htm
DX9 纹理

http://blog.csdn.net/xfate/article/details/5825370
D3D学习笔记:纹理


d:\Program Files (x86)\Microsoft DirectX SDK (June 2010)\Include\d3d9types.h
typedef enum _D3DTEXTUREADDRESS {
    D3DTADDRESS_WRAP            = 1,
    D3DTADDRESS_MIRROR          = 2,
    D3DTADDRESS_CLAMP           = 3,
    D3DTADDRESS_BORDER          = 4,
    D3DTADDRESS_MIRRORONCE      = 5,
    D3DTADDRESS_FORCE_DWORD     = 0x7fffffff, /* force 32-bit size enum */
} D3DTEXTUREADDRESS;

typedef enum _D3DSAMPLERSTATETYPE
{
    D3DSAMP_ADDRESSU       = 1,  /* D3DTEXTUREADDRESS for U coordinate */
    D3DSAMP_ADDRESSV       = 2,  /* D3DTEXTUREADDRESS for V coordinate */
    D3DSAMP_ADDRESSW       = 3,  /* D3DTEXTUREADDRESS for W coordinate */
    D3DSAMP_BORDERCOLOR    = 4,  /* D3DCOLOR */
    D3DSAMP_MAGFILTER      = 5,  /* D3DTEXTUREFILTER filter to use for magnification */
    D3DSAMP_MINFILTER      = 6,  /* D3DTEXTUREFILTER filter to use for minification */
    D3DSAMP_MIPFILTER      = 7,  /* D3DTEXTUREFILTER filter to use between mipmaps during minification */
    D3DSAMP_MIPMAPLODBIAS  = 8,  /* float Mipmap LOD bias */
    D3DSAMP_MAXMIPLEVEL    = 9,  /* DWORD 0..(n-1) LOD index of largest map to use (0 == largest) */
    D3DSAMP_MAXANISOTROPY  = 10, /* DWORD maximum anisotropy */
    D3DSAMP_SRGBTEXTURE    = 11, /* Default = 0 (which means Gamma 1.0,
                                   no correction required.) else correct for
                                   Gamma = 2.2 */
    D3DSAMP_ELEMENTINDEX   = 12, /* When multi-element texture is assigned to sampler, this
                                    indicates which element index to use.  Default = 0.  */
    D3DSAMP_DMAPOFFSET     = 13, /* Offset in vertices in the pre-sampled displacement map.
                                    Only valid for D3DDMAPSAMPLER sampler  */
    D3DSAMP_FORCE_DWORD   = 0x7fffffff, /* force 32-bit size enum */
} D3DSAMPLERSTATETYPE;


//
// Values for D3DSAMP_***FILTER texture stage states
//
typedef enum _D3DTEXTUREFILTERTYPE
{
    D3DTEXF_NONE            = 0,    // filtering disabled (valid for mip filter only)
    D3DTEXF_POINT           = 1,    // nearest
    D3DTEXF_LINEAR          = 2,    // linear interpolation
    D3DTEXF_ANISOTROPIC     = 3,    // anisotropic
    D3DTEXF_PYRAMIDALQUAD   = 6,    // 4-sample tent
    D3DTEXF_GAUSSIANQUAD    = 7,    // 4-sample gaussian
/* D3D9Ex only -- */
#if !defined(D3D_DISABLE_9EX)

    D3DTEXF_CONVOLUTIONMONO = 8,    // Convolution filter for monochrome textures

#endif // !D3D_DISABLE_9EX
/* -- D3D9Ex only */
    D3DTEXF_FORCE_DWORD     = 0x7fffffff,   // force 32-bit size enum
} D3DTEXTUREFILTERTYPE;


d:\Program Files (x86)\Microsoft DirectX SDK (June 2010)\Include\D3D11.h
typedef 
enum D3D11_FILTER
    {	D3D11_FILTER_MIN_MAG_MIP_POINT	= 0,
	D3D11_FILTER_MIN_MAG_POINT_MIP_LINEAR	= 0x1,
	D3D11_FILTER_MIN_POINT_MAG_LINEAR_MIP_POINT	= 0x4,
	D3D11_FILTER_MIN_POINT_MAG_MIP_LINEAR	= 0x5,
	D3D11_FILTER_MIN_LINEAR_MAG_MIP_POINT	= 0x10,
	D3D11_FILTER_MIN_LINEAR_MAG_POINT_MIP_LINEAR	= 0x11,
	D3D11_FILTER_MIN_MAG_LINEAR_MIP_POINT	= 0x14,
	D3D11_FILTER_MIN_MAG_MIP_LINEAR	= 0x15,
	D3D11_FILTER_ANISOTROPIC	= 0x55,
	D3D11_FILTER_COMPARISON_MIN_MAG_MIP_POINT	= 0x80,
	D3D11_FILTER_COMPARISON_MIN_MAG_POINT_MIP_LINEAR	= 0x81,
	D3D11_FILTER_COMPARISON_MIN_POINT_MAG_LINEAR_MIP_POINT	= 0x84,
	D3D11_FILTER_COMPARISON_MIN_POINT_MAG_MIP_LINEAR	= 0x85,
	D3D11_FILTER_COMPARISON_MIN_LINEAR_MAG_MIP_POINT	= 0x90,
	D3D11_FILTER_COMPARISON_MIN_LINEAR_MAG_POINT_MIP_LINEAR	= 0x91,
	D3D11_FILTER_COMPARISON_MIN_MAG_LINEAR_MIP_POINT	= 0x94,
	D3D11_FILTER_COMPARISON_MIN_MAG_MIP_LINEAR	= 0x95,
	D3D11_FILTER_COMPARISON_ANISOTROPIC	= 0xd5
    } 	D3D11_FILTER;

typedef 
enum D3D11_FILTER_TYPE
    {	D3D11_FILTER_TYPE_POINT	= 0,
	D3D11_FILTER_TYPE_LINEAR	= 1
    } 	D3D11_FILTER_TYPE;

#define	D3D11_FILTER_TYPE_MASK	( 0x3 )

#define	D3D11_MIN_FILTER_SHIFT	( 4 )

#define	D3D11_MAG_FILTER_SHIFT	( 2 )

#define	D3D11_MIP_FILTER_SHIFT	( 0 )

#define	D3D11_COMPARISON_FILTERING_BIT	( 0x80 )

#define	D3D11_ANISOTROPIC_FILTERING_BIT	( 0x40 )

#define D3D11_ENCODE_BASIC_FILTER( min, mag, mip, bComparison )                                           \
                                   ( ( D3D11_FILTER ) (                                                   \
                                   ( ( bComparison ) ? D3D11_COMPARISON_FILTERING_BIT : 0 ) |             \
                                   ( ( ( min ) & D3D11_FILTER_TYPE_MASK ) << D3D11_MIN_FILTER_SHIFT ) |   \
                                   ( ( ( mag ) & D3D11_FILTER_TYPE_MASK ) << D3D11_MAG_FILTER_SHIFT ) |   \
                                   ( ( ( mip ) & D3D11_FILTER_TYPE_MASK ) << D3D11_MIP_FILTER_SHIFT ) ) )   
#define D3D11_ENCODE_ANISOTROPIC_FILTER( bComparison )                                                \
                                         ( ( D3D11_FILTER ) (                                         \
                                         D3D11_ANISOTROPIC_FILTERING_BIT |                            \
                                         D3D11_ENCODE_BASIC_FILTER( D3D11_FILTER_TYPE_LINEAR,         \
                                                                    D3D11_FILTER_TYPE_LINEAR,         \
                                                                    D3D11_FILTER_TYPE_LINEAR,         \
                                                                    bComparison ) ) )                   
#define D3D11_DECODE_MIN_FILTER( d3d11Filter )                                                              \
                                 ( ( D3D11_FILTER_TYPE )                                                    \
                                 ( ( ( d3d11Filter ) >> D3D11_MIN_FILTER_SHIFT ) & D3D11_FILTER_TYPE_MASK ) ) 
#define D3D11_DECODE_MAG_FILTER( d3d11Filter )                                                              \
                                 ( ( D3D11_FILTER_TYPE )                                                    \
                                 ( ( ( d3d11Filter ) >> D3D11_MAG_FILTER_SHIFT ) & D3D11_FILTER_TYPE_MASK ) ) 
#define D3D11_DECODE_MIP_FILTER( d3d11Filter )                                                              \
                                 ( ( D3D11_FILTER_TYPE )                                                    \
                                 ( ( ( d3d11Filter ) >> D3D11_MIP_FILTER_SHIFT ) & D3D11_FILTER_TYPE_MASK ) ) 
#define D3D11_DECODE_IS_COMPARISON_FILTER( d3d11Filter )                                                    \
                                 ( ( d3d11Filter ) & D3D11_COMPARISON_FILTERING_BIT )                         
#define D3D11_DECODE_IS_ANISOTROPIC_FILTER( d3d11Filter )                                               \
                            ( ( ( d3d11Filter ) & D3D11_ANISOTROPIC_FILTERING_BIT ) &&                  \
                            ( D3D11_FILTER_TYPE_LINEAR == D3D11_DECODE_MIN_FILTER( d3d11Filter ) ) &&   \
                            ( D3D11_FILTER_TYPE_LINEAR == D3D11_DECODE_MAG_FILTER( d3d11Filter ) ) &&   \
                            ( D3D11_FILTER_TYPE_LINEAR == D3D11_DECODE_MIP_FILTER( d3d11Filter ) ) )      

typedef enum D3D11_TEXTURE_ADDRESS_MODE
    {	D3D11_TEXTURE_ADDRESS_WRAP	= 1,
	D3D11_TEXTURE_ADDRESS_MIRROR	= 2,
	D3D11_TEXTURE_ADDRESS_CLAMP	= 3,
	D3D11_TEXTURE_ADDRESS_BORDER	= 4,
	D3D11_TEXTURE_ADDRESS_MIRROR_ONCE	= 5
    } 	D3D11_TEXTURE_ADDRESS_MODE;

typedef struct D3D11_SAMPLER_DESC
    {
    D3D11_FILTER Filter;
    D3D11_TEXTURE_ADDRESS_MODE AddressU;
    D3D11_TEXTURE_ADDRESS_MODE AddressV;
    D3D11_TEXTURE_ADDRESS_MODE AddressW;
    FLOAT MipLODBias;
    UINT MaxAnisotropy;
    D3D11_COMPARISON_FUNC ComparisonFunc;
    FLOAT BorderColor[ 4 ];
    FLOAT MinLOD;
    FLOAT MaxLOD;
    } 	D3D11_SAMPLER_DESC;



d:\VS2005\VC\PlatformSDK\Include\GdiPlusEnums.h
enum QualityMode
{
    QualityModeInvalid   = -1,
    QualityModeDefault   = 0,
    QualityModeLow       = 1, // Best performance
    QualityModeHigh      = 2  // Best rendering quality
};
enum InterpolationMode
{
    InterpolationModeInvalid          = QualityModeInvalid,
    InterpolationModeDefault          = QualityModeDefault,
    InterpolationModeLowQuality       = QualityModeLow,
    InterpolationModeHighQuality      = QualityModeHigh,
    InterpolationModeBilinear,
    InterpolationModeBicubic,
    InterpolationModeNearestNeighbor,
    InterpolationModeHighQualityBilinear,
    InterpolationModeHighQualityBicubic
};


d:\VS2005\VC\PlatformSDK\Include\GdiPlusColor.h
enum ColorMode
{
    ColorModeARGB32 = 0,
    ColorModeARGB64 = 1
};
    enum
    {
        AlphaShift  = 24,
        RedShift    = 16,
        GreenShift  = 8,
        BlueShift   = 0
    };

    static ARGB MakeARGB(IN BYTE a,
                         IN BYTE r,
                         IN BYTE g,
                         IN BYTE b)
    {
        return (((ARGB) (b) <<  BlueShift) |
                ((ARGB) (g) << GreenShift) |
                ((ARGB) (r) <<   RedShift) |
                ((ARGB) (a) << AlphaShift));
    }

Imaging.h
typedef DWORD ARGB;


d:\VS2005\VC\PlatformSDK\Include\GdiPlusPixelFormats.h
// In-memory pixel data formats:
// bits 0-7 = format index
// bits 8-15 = pixel size (in bits)
// bits 16-23 = flags
// bits 24-31 = reserved

typedef INT PixelFormat;

#define    PixelFormatIndexed      0x00010000 // Indexes into a palette
#define    PixelFormatGDI          0x00020000 // Is a GDI-supported format
#define    PixelFormatAlpha        0x00040000 // Has an alpha component
#define    PixelFormatPAlpha       0x00080000 // Pre-multiplied alpha
#define    PixelFormatExtended     0x00100000 // Extended color 16 bits/channel
#define    PixelFormatCanonical    0x00200000 

#define    PixelFormatUndefined       0
#define    PixelFormatDontCare        0

#define    PixelFormat1bppIndexed     (1 | ( 1 << 8) | PixelFormatIndexed | PixelFormatGDI)
#define    PixelFormat4bppIndexed     (2 | ( 4 << 8) | PixelFormatIndexed | PixelFormatGDI)
#define    PixelFormat8bppIndexed     (3 | ( 8 << 8) | PixelFormatIndexed | PixelFormatGDI)
#define    PixelFormat16bppGrayScale  (4 | (16 << 8) | PixelFormatExtended)
#define    PixelFormat16bppRGB555     (5 | (16 << 8) | PixelFormatGDI)
#define    PixelFormat16bppRGB565     (6 | (16 << 8) | PixelFormatGDI)
#define    PixelFormat16bppARGB1555   (7 | (16 << 8) | PixelFormatAlpha | PixelFormatGDI)
#define    PixelFormat24bppRGB        (8 | (24 << 8) | PixelFormatGDI)
#define    PixelFormat32bppRGB        (9 | (32 << 8) | PixelFormatGDI)
#define    PixelFormat32bppARGB       (10 | (32 << 8) | PixelFormatAlpha | PixelFormatGDI | PixelFormatCanonical)
#define    PixelFormat32bppPARGB      (11 | (32 << 8) | PixelFormatAlpha | PixelFormatPAlpha | PixelFormatGDI)
#define    PixelFormat48bppRGB        (12 | (48 << 8) | PixelFormatExtended)
#define    PixelFormat64bppARGB       (13 | (64 << 8) | PixelFormatAlpha  | PixelFormatCanonical | PixelFormatExtended)
#define    PixelFormat64bppPARGB      (14 | (64 << 8) | PixelFormatAlpha  | PixelFormatPAlpha | PixelFormatExtended)
#define    PixelFormatMax             15
