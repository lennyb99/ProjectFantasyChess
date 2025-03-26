using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject informationPanel;

    public void OnPointerEnter(PointerEventData eventData)
    {
        informationPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        informationPanel.SetActive(false);
    }
}
