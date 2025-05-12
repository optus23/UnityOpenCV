using UnityEngine;

public class OpenLink : MonoBehaviour
{
    // Call this (e.g. from a button’s OnClick) to open the URL:
    public void OpenExternalLink(string url)
    {
        Application.OpenURL(url);
    }
}
