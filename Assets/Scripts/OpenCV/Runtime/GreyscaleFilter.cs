using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace OpenCV
{

public class GreyscaleFilter : MonoBehaviour
{
    public OpenCvManager CvManager;
    
    private Texture2D m_SourceTexture;
    private Texture2D m_ResultTexture;
    
    void Start()
    {
        // Create output texture
        m_ResultTexture = new Texture2D( CvManager.SourceTexture.width, CvManager.SourceTexture.height, CvManager.SourceTexture.format, false );
    }

    public void ApplyGreyscale()
    {
        // Get actual state of texture to stack filters
        m_SourceTexture = CvManager.DestinationTexture != null ? CvManager.DestinationTexture : CvManager.SourceTexture;
        int w = m_SourceTexture.width, h = m_SourceTexture.height;

        // Read and convert to BGR array
        Color32[] pixels = m_SourceTexture.GetPixels32();
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

        // Grey Scale Filter
        byte[] grayData = new byte[w * h];
        IntPtr dstHeader = OpenCvNativeImporter.cvCreateImageHeader( size, CvConstants.Depth8U, CvConstants.ChannelsGray );
        GCHandle handleDst = GCHandle.Alloc( grayData, GCHandleType.Pinned );
        OpenCvNativeImporter.cvSetData( dstHeader, handleDst.AddrOfPinnedObject(), w * CvConstants.ChannelsGray );
        
        // Call CvtColor OpenCV DLL method
        OpenCvNativeImporter.cvCvtColor( srcHeader, dstHeader, ColorConversion.Bgr2Gray );

        Color32[] outPixels = new Color32[pixels.Length];

        for ( int i = 0; i < outPixels.Length; i++ )
        {
            byte v = grayData[i];
            outPixels[i] = new Color32( v, v, v, CvConstants.AlphaOpaque );
        }

        m_ResultTexture.SetPixels32( outPixels );
        m_ResultTexture.Apply();
        CvManager.UpdateDestinationImage(m_ResultTexture);

        // Cleanup
        handleSrc.Free();
        handleDst.Free();
        OpenCvNativeImporter.cvReleaseImageHeader( ref srcHeader );
        OpenCvNativeImporter.cvReleaseImageHeader( ref dstHeader );
    }
}

}