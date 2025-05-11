using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
namespace OpenCV
{

public class GaussianBlurFilter : MonoBehaviour
{
    public RawImage CanvasImage;
    public Texture2D SourceTexture;
    public int KernelSize = 5;
    public double SigmaX = 1.0;
    public double SigmaY = 0.0;
 
    private Texture2D m_ResultTexture;


    private void Start()
    {
        // Create output texture
        m_ResultTexture = new Texture2D( SourceTexture.width, SourceTexture.height, SourceTexture.format, false );
    }

    public void ApplyGaussianBlur()
    {
        int w = SourceTexture.width, h = SourceTexture.height;
        
        // Read and convert to BGR array
        Color32[] pixels = SourceTexture.GetPixels32();
        byte[] bgrData = new byte[pixels.Length * CvConstants.ChannelsBgr];

        for ( int i = 0; i < pixels.Length; i++ )
        {
            bgrData[i * CvConstants.ChannelsBgr + 0] = pixels[i].b; // BGR + Blue Channel
            bgrData[i * CvConstants.ChannelsBgr + 1] = pixels[i].g; // BGR + Green Channel
            bgrData[i * CvConstants.ChannelsBgr + 2] = pixels[i].r; // BGR + Red Channel
        }
        CvSize size = new CvSize { Width = w, Height = h };

        // Create headers & pin buffers
        IntPtr srcHeader = OpenCvNativeImporter.cvCreateImageHeader( size, CvConstants.Depth8U, CvConstants.ChannelsBgr );
        GCHandle handleSrc = GCHandle.Alloc( bgrData, GCHandleType.Pinned );
        OpenCvNativeImporter.cvSetData( srcHeader, handleSrc.AddrOfPinnedObject(), w * CvConstants.ChannelsBgr );

        // Gaussian Filter
        byte[] resultData = new byte[bgrData.Length];
        IntPtr dstHeader = OpenCvNativeImporter.cvCreateImageHeader( size, CvConstants.Depth8U, CvConstants.ChannelsBgr );
        GCHandle handleDst = GCHandle.Alloc( resultData, GCHandleType.Pinned );
        OpenCvNativeImporter.cvSetData( dstHeader, handleDst.AddrOfPinnedObject(), w * CvConstants.ChannelsBgr );

        // Call Smooth OpenCV DLL method
        OpenCvNativeImporter.cvSmooth( srcHeader, dstHeader, SmoothType.Gaussian, KernelSize, KernelSize, SigmaX, SigmaY );

        var outPixels = new Color32[pixels.Length];

        for ( int i = 0; i < pixels.Length; i++ )
        {
            outPixels[i] = new Color32(
                resultData[i * CvConstants.ChannelsBgr + 2], // BGR + Red Channel
                resultData[i * CvConstants.ChannelsBgr + 1], // BGR + Green Channel
                resultData[i * CvConstants.ChannelsBgr + 0], // BGR + Blue Channel
                CvConstants.AlphaOpaque
            );
        }

        m_ResultTexture.SetPixels32( outPixels );
        m_ResultTexture.Apply();

        CanvasImage.texture = m_ResultTexture;

        // Cleanup
        handleSrc.Free();
        handleDst.Free();
        OpenCvNativeImporter.cvReleaseImageHeader( ref srcHeader );
        OpenCvNativeImporter.cvReleaseImageHeader( ref dstHeader );
    }
}

}