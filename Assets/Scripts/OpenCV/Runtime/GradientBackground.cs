
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

namespace OpenCV
{

public class GradientBackground : MonoBehaviour
{
    [Header( "Corner Colors (RGB)" )]
    public Color topLeft = Color.red;
    public Color topRight = Color.green;
    public Color bottomLeft = Color.blue;
    public Color bottomRight = Color.yellow;
    public OpenCvManager CvManager;

    private Texture2D _tex;
    private byte[] _bgrData;
    private IntPtr _imgHeader;
    private GCHandle _pinHandle;
    private int _w, _h;

    void Start()
    {
        // Initialize resolution and buffers
        _w = Screen.width;
        _h = Screen.height;
        _tex = new Texture2D( _w, _h, TextureFormat.RGBA32, false, false );
        _bgrData = new byte[_w * _h * CvConstants.ChannelsBgr];

        // Create OpenCV image header around our buffer
        var size = new CvSize { Width = _w, Height = _h };

        _imgHeader = OpenCvNativeImporter.cvCreateImageHeader(
            size,
            CvConstants.Depth8U,
            CvConstants.ChannelsBgr
        );

        _pinHandle = GCHandle.Alloc( _bgrData, GCHandleType.Pinned );

        OpenCvNativeImporter.cvSetData(
            _imgHeader,
            _pinHandle.AddrOfPinnedObject(),
            _w * CvConstants.ChannelsBgr
        );

        // Fill static four-color bilinear gradient
        FillStaticGradient();

        // Copy BGR buffer to Unity texture once
        var pixels = new Color32[_w * _h];
        int C = CvConstants.ChannelsBgr;

        for ( int y = 0; y < _h; y++ )
        {
            for ( int x = 0; x < _w; x++ )
            {
                int idx = ( y * _w + x ) * C;
                byte b = _bgrData[idx + 0];
                byte g = _bgrData[idx + 1];
                byte r = _bgrData[idx + 2];
                pixels[y * _w + x] = new Color32( r, g, b, CvConstants.AlphaOpaque );
            }
        }

        _tex.SetPixels32( pixels );
        _tex.Apply();

        // Assign to material
        CvManager.BackgroundImage.enabled = true;
        CvManager.BackgroundImage.texture = _tex;
    }
    
    private void FillStaticGradient()
    {
        // Precompute corner bytes
        Vector3 cTL = new Vector3( topLeft.r, topLeft.g, topLeft.b );
        Vector3 cTR = new Vector3( topRight.r, topRight.g, topRight.b );
        Vector3 cBL = new Vector3( bottomLeft.r, bottomLeft.g, bottomLeft.b );
        Vector3 cBR = new Vector3( bottomRight.r, bottomRight.g, bottomRight.b );

        // Iterate over pixels
        for ( int y = 0; y < _h; y++ )
        {
            float ty = y / ( float )( _h - 1 );

            for ( int x = 0; x < _w; x++ )
            {
                float tx = x / ( float )( _w - 1 );

                // Bilinear interpolation
                Vector3 top = Vector3.Lerp( cTL, cTR, tx );
                Vector3 bottom = Vector3.Lerp( cBL, cBR, tx );
                Vector3 color = Vector3.Lerp( bottom, top, ty );

                byte b = ( byte )( color.z * 255f );
                byte g = ( byte )( color.y * 255f );
                byte r = ( byte )( color.x * 255f );

                int idx = ( y * _w + x ) * CvConstants.ChannelsBgr;
                _bgrData[idx + 0] = b;
                _bgrData[idx + 1] = g;
                _bgrData[idx + 2] = r;
            }
        }
    }

    void OnDestroy()
    {
        if ( _pinHandle.IsAllocated )
            _pinHandle.Free();

        if ( _imgHeader != IntPtr.Zero )
            OpenCvNativeImporter.cvReleaseImageHeader( ref _imgHeader );
    }
}

}
