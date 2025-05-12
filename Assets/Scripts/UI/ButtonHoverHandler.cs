using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverHandler : MonoBehaviour, IPointerEnterHandler
{
    public UILabelInteraction LabelInteraction;
    public string Message;
    public float DisplayDuration;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        LabelInteraction.ShowLabelAndHide( Message, DisplayDuration );
        Debug.Log("Pointer entered!");
    }
    
}
