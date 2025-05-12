using UnityEngine;
using UnityEngine.UI;
using System;
using System.Runtime.InteropServices;

namespace OpenCV
{

public class OutlineImageFilter : MonoBehaviour
{
    
    [Header( "Outline Parameters" )]
    public Color OutlineColor = Color.red;
    public int Thickness = 5;
    public OpenCvManager CvManager;
    public void ApllyOutlineImage()
    {
        // Source texture from RawImage
        Texture2D srcTex = (Texture2D)CvManager.CanvasImage.texture;
        int w = srcTex.width;
        int h = srcTex.height;

        // Prepare output texture
        Texture2D dstTex = new Texture2D(w, h, TextureFormat.RGBA32, false);

        // Read pixels and pack BGR
        var pixels = srcTex.GetPixels32();
        byte[] bgrData = new byte[pixels.Length * 3];
        for (int i = 0; i < pixels.Length; i++)
        {
            int idx = i * 3;
            bgrData[idx + 0] = pixels[i].b;
            bgrData[idx + 1] = pixels[i].g;
            bgrData[idx + 2] = pixels[i].r;
        }

        // Create OpenCV header
        CvSize size = new CvSize { Width = w, Height = h };
        IntPtr imgH = OpenCvNativeImporter.cvCreateImageHeader(size, 8, CvConstants.ChannelsBgr);
        GCHandle handle = GCHandle.Alloc(bgrData, GCHandleType.Pinned);
        OpenCvNativeImporter.cvSetData(imgH, handle.AddrOfPinnedObject(), w * CvConstants.ChannelsBgr);

        // Draw rectangle border
        // Top-left at (0,0), bottom-right at (w-1,h-1)
        CvPoint p1 = new CvPoint(0, 0);
        CvPoint p2 = new CvPoint(w - 1, h - 1);
        // OpenCV uses BGR for CvScalar: val0=blue, val1=green, val2=red
        CvScalar color = new CvScalar(
            OutlineColor.b * 255.0,
            OutlineColor.g * 255.0,
            OutlineColor.r * 255.0,
            0
        );
        OpenCvNativeImporter.cvRectangle(imgH, p1, p2, color, Thickness, CvConstants.CvLine8, 0);

        // Copy back to RGBA texture
        Color32[] outPixels = new Color32[pixels.Length];
        for (int i = 0; i < pixels.Length; i++)
        {
            int idx = i * 3;
            byte b = bgrData[idx + 0];
            byte g = bgrData[idx + 1];
            byte r = bgrData[idx + 2];
            outPixels[i] = new Color32(r, g, b, CvConstants.AlphaOpaque);
        }
        dstTex.SetPixels32(outPixels);
        dstTex.Apply();

        // Assign result to RawImage
        CvManager.UpdateDestinationImage(dstTex);

        // Cleanup
        handle.Free();
        OpenCvNativeImporter.cvReleaseImageHeader(ref imgH);
    }
}
}