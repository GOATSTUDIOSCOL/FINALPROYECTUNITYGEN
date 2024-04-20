using UnityEngine;
using Cinemachine;
using System.Collections.Generic;


public class OutlineInteraction : MonoBehaviour
{
    public float interactDistance = 5f;
    public float interactRadius = 1f;
    public CinemachineVirtualCamera playerCamera;

    private HashSet<Outline> highlightedObjects = new HashSet<Outline>();

    private void Start()
    {
        playerCamera = FindObjectOfType<CinemachineVirtualCamera>();
    }

    void Update()
    {
        if (playerCamera != null)
        {
            RaycastHit[] hits;
            Vector3 rayOrigin = playerCamera.Follow.position;
            hits = Physics.SphereCastAll(rayOrigin, interactRadius, playerCamera.transform.forward, interactDistance);
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.CompareTag("Grabbable") || hit.collider.CompareTag("Item"))
                {
                    Outline outlineComponent = hit.collider.GetComponent<Outline>();
                    if (outlineComponent != null)
                    {
                        outlineComponent.enabled = true;
                        highlightedObjects.Add(outlineComponent);
                    }
                }
            }
            DisableOutlineOnExitedObjects(hits);
        }
    }

    void DisableOutlineOnExitedObjects(RaycastHit[] hits)
    {

        HashSet<Outline> highlightedObjectsCopy = new HashSet<Outline>(highlightedObjects);
        foreach (Outline outline in highlightedObjectsCopy)
        {
            if (!ObjectIsStillHighlighted(outline, hits))
            {
                outline.enabled = false;
                highlightedObjects.Remove(outline);
            }
        }
    }

    bool ObjectIsStillHighlighted(Outline outline, RaycastHit[] hits)
    {
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.GetComponent<Outline>() == outline)
            {
                return true;
            }
        }
        return false;
    }

}
