using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDetector : MonoBehaviour
{
    public Transform tile;
    void OnMouseDown()
    {
        tile.transform.Rotate(0, 0, -90);
    }
}
