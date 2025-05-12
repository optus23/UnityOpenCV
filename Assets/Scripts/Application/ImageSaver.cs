using System;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Serialization;

public class ImageSaver : MonoBehaviour
{
    public UILabelInteraction UILabelInteraction;
    public RawImage RawImage;
    public float DisplayDuration = 2f;
    
    public void SaveImageToDisk()
    {
        Texture2D tex = RawImage.texture as Texture2D;
        if (tex == null)
            return;

        byte[] bytes = tex.EncodeToPNG();
        if (bytes == null || bytes.Length == 0)
        {
            Debug.LogError("Error when encoding texture.");
            return;
        }

        string exeFolder = System.IO.Path.GetDirectoryName(Application.dataPath);

        string filePath = System.IO.Path.Combine(exeFolder, GetFilenameTimeStamp());
        UILabelInteraction.ShowLabelAndHide("Image saved at: " + filePath, DisplayDuration);
        try
        {
            File.WriteAllBytes(filePath, bytes);
            Debug.Log( "Image saved at: " + filePath );
        }
        catch (IOException e)
        {
            Debug.LogError($"Unable to save image: {e.Message}");
        }
    }
    private string GetFilenameTimeStamp()
    {
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string fileNameWithTimestamp = $"image_{timestamp}.png";

        return fileNameWithTimestamp;
    }
    
    
    
    
}
