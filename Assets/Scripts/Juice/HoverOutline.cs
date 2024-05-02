using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverOutline : MonoBehaviour
{

    Outline outline;
    private bool isHighlighted = false;
    public Camera cam;
    private void Start()
    {
        outline = GetComponent<Outline>();
        cam = FindObjectOfType<Camera>();
    }

    private void OnMouseOver()
    {
        if (!isHighlighted)
        {
            isHighlighted = true;
            outline.OutlineWidth = 10;
        }
    }

    private void OnMouseExit()
    {
        if (isHighlighted)
        {
            isHighlighted = false;
            outline.OutlineWidth = 0;
        }
    }
}