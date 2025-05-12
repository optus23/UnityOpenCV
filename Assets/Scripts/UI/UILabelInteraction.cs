using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class UILabelInteraction : MonoBehaviour
{
    public float DisplayDuration = 2f;
    public TMP_Text Label;
    
    private Coroutine c_HideCoroutine;
    
    public void ShowLabelAndHide(string info, float time)
    {
        Label.enabled = true;
        Label.text = info;

        if (c_HideCoroutine != null)
        {
            StopCoroutine(c_HideCoroutine);
        }
        c_HideCoroutine = StartCoroutine(HideAfterDelay(Label, time));
    }

   
    private IEnumerator HideAfterDelay(TMP_Text label, float time)
    {
        yield return new WaitForSeconds(time);

        label.enabled = false;
    }
    
}
