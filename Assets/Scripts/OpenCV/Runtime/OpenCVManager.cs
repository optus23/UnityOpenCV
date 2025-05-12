using UnityEngine.UI;
using UnityEngine;

namespace OpenCV
{

public class OpenCvManager : MonoBehaviour
{
    [Header("--- TEXTURE ---")]
    public Texture2D SourceTexture;
    [HideInInspector]
    public Texture2D DestinationTexture;

    [Header("--- UNITY UI ---")]
    public RawImage CanvasImage;
    public RawImage BackgroundImage;
    
    public void UpdateDestinationImage(Texture2D texture2D)
    {
        CanvasImage.texture = DestinationTexture = texture2D;
    }
    
    public void ResetCanvasImage()
    {
        CanvasImage.texture = DestinationTexture = SourceTexture;
    }
    
}
}