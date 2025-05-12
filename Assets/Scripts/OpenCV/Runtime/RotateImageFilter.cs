using UnityEngine;
using UnityEngine.UI;
using System;
using System.Runtime.InteropServices;
using UnityEngine.Serialization;

namespace OpenCV
{

public class RotateImageFilter : MonoBehaviour
{
    public float AngleDegrees = 90f;
    public float Scale = 1f;
    public Vector2 PivotNormalized = new Vector2(0.5f, 0.5f);
    public OpenCvManager CvManager;
    
    private float m_RotationAnglesIncrement;
    private Texture2D m_SourceTexture;
    private Texture2D m_ResultTexture;

    private void Start()
    {
        // Prepare Unity texture and assign
        m_ResultTexture = new Texture2D( CvManager.SourceTexture.width, CvManager.SourceTexture.height, CvManager.SourceTexture.format, false );
    }

    public void ApplyRotation()
    {
        // Get actual state of texture to stack filters
        m_SourceTexture = CvManager.DestinationTexture != null ? CvManager.DestinationTexture : CvManager.SourceTexture;
        int w = m_SourceTexture.width, h = m_SourceTexture.height;
        
        // Load source pixels into BGR buffer
        Color32[] pixels = m_SourceTexture.GetPixels32();
        byte[] bgrData = new byte[pixels.Length * CvConstants.ChannelsBgr];
        for (int i = 0; i < pixels.Length; i++)
        {
            int baseIdx = i * CvConstants.ChannelsBgr;
            bgrData[baseIdx + 0]  = pixels[i].b;
            bgrData[baseIdx + 1] = pixels[i].g;
            bgrData[baseIdx + 2]   = pixels[i].r;
        }

        // Create OpenCV image headers and pin buffers
        CvSize size = new CvSize { Width = w, Height = h };
        IntPtr srcH = OpenCvNativeImporter.cvCreateImageHeader(size, CvConstants.Depth8U, CvConstants.ChannelsBgr);
        GCHandle hSrc = GCHandle.Alloc(bgrData, GCHandleType.Pinned);
        OpenCvNativeImporter.cvSetData(srcH, hSrc.AddrOfPinnedObject(), w * CvConstants.ChannelsBgr);

        byte[] outData = new byte[bgrData.Length];
        IntPtr dstH = OpenCvNativeImporter.cvCreateImageHeader(size, CvConstants.Depth8U, CvConstants.ChannelsBgr);
        GCHandle hDst = GCHandle.Alloc(outData, GCHandleType.Pinned);
        OpenCvNativeImporter.cvSetData(dstH, hDst.AddrOfPinnedObject(), w * CvConstants.ChannelsBgr);

        // Compute manual affine matrix (2x3) for rotation+scale
        m_RotationAnglesIncrement += AngleDegrees;
        float theta = m_RotationAnglesIncrement * Mathf.Deg2Rad;
        float alpha = Scale * Mathf.Cos(theta);
        float beta  = Scale * Mathf.Sin(theta);
        float cx = PivotNormalized.x * w;
        float cy = PivotNormalized.y * h;
        float tx = (1 - alpha) * cx - beta * cy;
        float ty = beta * cx + (1 - alpha) * cy;

        float[] M = new float[6] { alpha,  beta,  tx,
                                   -beta,   alpha,  ty };
        GCHandle hMat = GCHandle.Alloc(M, GCHandleType.Pinned);
        IntPtr matH = OpenCvNativeImporter.cvCreateMatHeader(2, 3, CvConstants.Cv32Fc1);
        // step = #floats per row * sizeof(float)
        OpenCvNativeImporter.cvSetData(matH, hMat.AddrOfPinnedObject(), 3 * sizeof(float));

        // Apply affine warp
        OpenCvNativeImporter.cvWarpAffine(srcH, dstH, matH, CvConstants.CvLine8);

        // Copy back output to Unity Texture2D
        Color32[] outPixels = new Color32[pixels.Length];
        for (int i = 0; i < outPixels.Length; i++)
        {
            int idx = i * CvConstants.ChannelsBgr;
            byte b = outData[idx + 0];
            byte g = outData[idx + 1];
            byte r = outData[idx + 2];
            outPixels[i] = new Color32(r, g, b, CvConstants.AlphaOpaque);
        }
        m_ResultTexture.SetPixels32(outPixels);
        m_ResultTexture.Apply();
        CvManager.UpdateDestinationImage(m_ResultTexture);
        
        // Cleanup
        hSrc.Free();
        hDst.Free();
        hMat.Free();
        OpenCvNativeImporter.cvReleaseImageHeader(ref srcH);
        OpenCvNativeImporter.cvReleaseImageHeader(ref dstH);
        OpenCvNativeImporter.cvReleaseMat(ref matH);
    }
}
}