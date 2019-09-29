using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutsideScreenDetection : MonoBehaviour
{
    private GameObject m_Pointer;
    void Start()
    {
        m_Pointer = GameObject.Find("OutOfViewportPointer");
    }

    (float rotation, Vector3 position) GetPointerPositionAndRotation()
    {
        var pointerRectTransform = m_Pointer.GetComponent<RectTransform>();
        var positionOnScreen = Camera.main.WorldToScreenPoint(transform.position);
        var rotation = 0;
        var position = Vector3.zero;
        var maxHeight = Screen.height/2f - pointerRectTransform.sizeDelta.y;
        var maxWidth = Screen.width/2f - pointerRectTransform.sizeDelta.x;
        if (positionOnScreen.x > Screen.width)
        {
            position.x = maxWidth;
            if (positionOnScreen.y > Screen.height)
            {
                position.y = maxHeight;
                rotation = 45;
                
            }
            else if (positionOnScreen.y < 0)
            {
                position.y = pointerRectTransform.sizeDelta.y;
                rotation = 315;
            }
            else
            {
                position.y = positionOnScreen.y;
                rotation = 0;
            }
        }
        else if (positionOnScreen.x < 0)
        {
            position.x = pointerRectTransform.sizeDelta.x;
            if (positionOnScreen.y > Screen.height)
            {
                position.y = maxHeight;
                rotation = 135;
            }
            else if (positionOnScreen.y < 0)
            {
                position.y = pointerRectTransform.sizeDelta.y;
                rotation = 225;
            }
            else
            {
                position.y = positionOnScreen.y;
                rotation = 180;
            }
        }
        else
        {
            position.x = positionOnScreen.x;
            if (positionOnScreen.y > Screen.height)
            {
                position.y = maxHeight;
                rotation = 90;
            }
            else if (positionOnScreen.y < 0)
            {
                position.y = pointerRectTransform.sizeDelta.y;
                rotation = 270;
            }
        }

        return (rotation, position);

    }

    void OnBecameInvisible()
    {
        var (rotation, position) = GetPointerPositionAndRotation();
        var rect = m_Pointer.GetComponent<RectTransform>();
        rect.anchoredPosition = position;
        rect.Rotate(Vector3.forward, rotation);
        m_Pointer.GetComponent<CanvasGroup>().alpha = 1;
    }

    void OnBecameVisible()
    {
        var pointer = GameObject.Find("OutOfViewportPointer");
        pointer.GetComponent<CanvasGroup>().alpha = 0;
    }
}
