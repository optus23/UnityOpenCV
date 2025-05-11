namespace OpenCV
{

/// <summary>
/// Constants for OpenCV C API interop.
/// </summary>
public static class CvConstants
{
    // Image depth (bits per channel)
    // from IPL_DEPTH_8U
    public const int Depth8U = 8;

    // Channel counts
    public const int ChannelsBgr = 3;  // B, G, R
    public const int ChannelsGray = 1; // single-channel (grayscale)

    // Fully opaque alpha for Unity’s Color32
    public const byte AlphaOpaque = 255;
}

/// <summary>
/// Codes for cvCvtColor in the C API (from imgproc_c.h).
/// </summary>
public enum ColorConversion : int
{
    Bgr2Bgra = 0,   // CV_BGR2BGRA
    Bgra2Bgr = 1,   // CV_BGRA2BGR
    Bgr2Rgba = 2,   // CV_BGR2RGBA
    Rgba2Bgr = 3,   // CV_RGBA2BGR
    Bgr2Rgb = 4,    // CV_BGR2RGB
    Rgb2Bgr = 4,    // CV_RGB2BGR
    Bgra2Rgba = 5,  // CV_BGRA2RGBA
    Rgba2Bgra = 5,  // CV_RGBA2BGRA
    Bgr2Gray = 6,   // CV_BGR2GRAY
    Rgb2Gray = 7,   // CV_RGB2GRAY
    Gray2Bgr = 8,   // CV_GRAY2BGR and CV_GRAY2RGB
    Gray2Rgba = 9,  // CV_GRAY2BGRA and CV_GRAY2RGBA
    Bgra2Gray = 10, // CV_BGRA2GRAY
    Rgba2Gray = 11, // CV_RGBA2GRAY
}

/// <summary>
/// Types for cvSmooth in the C API (from the Cv-smooth-type enum).
/// </summary>
public enum SmoothType : int
{
    BlurNoScale = 0,
    Blur = 1,
    Gaussian = 2,  // CV_GAUSSIAN
    Median = 3,    // CV_MEDIAN
    Bilateral = 4, // CV_BILATERAL
}

}
