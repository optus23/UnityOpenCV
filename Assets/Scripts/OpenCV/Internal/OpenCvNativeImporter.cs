using System;
using System.Runtime.InteropServices;

namespace OpenCV
{

[StructLayout( LayoutKind.Sequential )]
public struct CvSize
{
    public int Width;
    public int Height;
}

[StructLayout(LayoutKind.Sequential)]
public struct CvPoint2D32F
{
    public float x, y;
}

[StructLayout(LayoutKind.Sequential)] 
public struct CvPoint { public int X, Y;
    public CvPoint( int x, int y )
    {
        X=x;
        Y=y;
    } 
}

[StructLayout( LayoutKind.Sequential )]
public struct CvScalar
{
    public double Val0,Val1,Val2,Val3; 
    public CvScalar(double v0,double v1,double v2,double v3)
    {
        Val0=v0;
        Val1=v1;
        Val2=v2;
        Val3=v3;
    }
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
    public static extern void cvSmooth( IntPtr src, IntPtr dst, SmoothType smoothtype, int size1, int size2, double sigmaX, double sigmaY );
    
    [DllImport(OpenCvLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern void cvCvtColor( IntPtr src, IntPtr dst, ColorConversion code );
    
    [DllImport( OpenCvLibrary, CallingConvention = CallingConvention.Cdecl )]
    public static extern void cvReleaseImageHeader( ref IntPtr image );
    
    [DllImport(OpenCvLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr cvGetRotationMatrix2D( CvPoint2D32F center, double angle, double scale );

    [DllImport(OpenCvLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern void cvWarpAffine( IntPtr src, IntPtr dst, IntPtr mapMatrix, int flags );
   
    [DllImport(OpenCvLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr cvCreateMatHeader(int rows, int cols, int type);
    
    [DllImport(OpenCvLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern void cvReleaseMat(ref IntPtr mat);

    [DllImport(OpenCvLibrary, CallingConvention = CallingConvention.Cdecl)]
    public static extern void cvRectangle( IntPtr img, CvPoint pt1, CvPoint pt2, CvScalar color, int thickness, int lineType, int shift );
}

}
