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
        m_ResultTexture = new Texture2D( SourceTexture.width, SourceTexture.height, SourceTexture.format, false );
    }

    public void ApplyGaussianBlur()
    {
        int w = SourceTexture.width;
        int h = SourceTexture.height;

        Color32[] pixels = SourceTexture.GetPixels32();
        byte[] bgrData = new byte[pixels.Length * 3];

        for ( int i = 0; i < pixels.Length; i++ )
        {
            bgrData[i * 3 + 0] = pixels[i].b;
            bgrData[i * 3 + 1] = pixels[i].g;
            bgrData[i * 3 + 2] = pixels[i].r;
        }
        

        CvSize size = new CvSize { Width = w, Height = h };

        IntPtr srcHeader = OpenCvNativeImporter.cvCreateImageHeader( size, 8, 3 );
        GCHandle handleSrc = GCHandle.Alloc( bgrData, GCHandleType.Pinned );
        OpenCvNativeImporter.cvSetData( srcHeader, handleSrc.AddrOfPinnedObject(), w * 3 );

        byte[] resultData = new byte[bgrData.Length];
        IntPtr dstHeader = OpenCvNativeImporter.cvCreateImageHeader( size, 8, 3 );
        GCHandle handleDst = GCHandle.Alloc( resultData, GCHandleType.Pinned );
        OpenCvNativeImporter.cvSetData( dstHeader, handleDst.AddrOfPinnedObject(), w * 3 );

        OpenCvNativeImporter.cvSmooth( srcHeader, dstHeader, 2, KernelSize, KernelSize, SigmaX, SigmaY );

        var outPixels = new Color32[pixels.Length];

        for ( int i = 0; i < pixels.Length; i++ )
        {
            outPixels[i] = new Color32(
                resultData[i * 3 + 2],
                resultData[i * 3 + 1],
                resultData[i * 3 + 0],
                255
            );
        }

        m_ResultTexture.SetPixels32( outPixels );
        m_ResultTexture.Apply();

        CanvasImage.texture = m_ResultTexture;

        handleSrc.Free();
        handleDst.Free();
        OpenCvNativeImporter.cvReleaseImageHeader( ref srcHeader );
        OpenCvNativeImporter.cvReleaseImageHeader( ref dstHeader );
    }
}

}