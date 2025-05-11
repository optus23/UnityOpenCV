using System;
using System.Runtime.InteropServices;

namespace OpenCV
{

/// <summary>
/// Represents the size of an image in OpenCV.
/// </summary>
[StructLayout( LayoutKind.Sequential )]
public struct CvSize
{
    public int Width;
    public int Height;
}


/// <summary>
/// Provides P/Invoke declarations for the OpenCV library.
/// </summary>
internal static class OpenCvNativeImporter
{
    private const string OpenCvLibrary = "opencv_world4110";
    [DllImport( OpenCvLibrary, CallingConvention = CallingConvention.Cdecl )]
    public static extern IntPtr cvCreateImageHeader( CvSize size, int depth, int channels );

    [DllImport( OpenCvLibrary, CallingConvention = CallingConvention.Cdecl )]
    public static extern void cvSetData( IntPtr image, IntPtr data, int step );

    [DllImport( OpenCvLibrary, CallingConvention = CallingConvention.Cdecl )]
    public static extern void cvSmooth(
        IntPtr src,
        IntPtr dst,
        SmoothType smoothtype,
        int size1,
        int size2,
        double sigmaX,
        double sigmaY );
    
    [DllImport(OpenCvLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern void cvCvtColor(
        IntPtr src, IntPtr dst, ColorConversion code
    );
    
    [DllImport( OpenCvLibrary, CallingConvention = CallingConvention.Cdecl )]
    public static extern void cvReleaseImageHeader( ref IntPtr image );
}

}
