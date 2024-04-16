using UnityEngine.InputSystem;
using UnityEngine;

public class ObjectsInteraction : MonoBehaviour
{
    private GameObject heldObject;
    private bool isHoldingObject = false;
    private GameObject head;
    private InputAction interact;
    private InputAction throwAction;
    private PlayerInputActions playerControls;

    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }
    private void Start()
    {
        head = GetComponentInChildren<Camera>().gameObject;
        SetInteractInput();
        SetThrowInput();
    }

    private void SetInteractInput()
    {
        interact = playerControls.Player.Interact;
        interact.Enable();
        interact.performed += Interact;
    }

    private void SetThrowInput()
    {
        throwAction = playerControls.Player.Throw;
        throwAction.Enable();
        throwAction.performed += ThrowObject;
    }

    void GrabObject(GameObject obj)
    {
        heldObject = obj;
        isHoldingObject = true;
        heldObject.GetComponent<Rigidbody>().isKinematic = true;
        heldObject.transform.SetParent(head.transform);
    }

    void ReleaseObject()
    {
        if (heldObject != null)
        {
            heldObject.GetComponent<Rigidbody>().isKinematic = false;
            heldObject.transform.SetParent(null);
            isHoldingObject = false;
            heldObject = null;
        }
    }

    void ThrowObject(InputAction.CallbackContext callbackContext)
    {
        if (heldObject != null)
        {
            Rigidbody rb = heldObject.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            heldObject.transform.SetParent(null);
            isHoldingObject = false;
            heldObject = null;
            rb.AddForce(head.transform.forward * 10f, ForceMode.Impulse);
        }
    }

    public void Interact(InputAction.CallbackContext callbackContext)
    {
        if (!isHoldingObject)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 2f);
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Grabbable"))
                {
                    GrabObject(collider.gameObject);
                    break;
                }
                else if (collider.CompareTag("Item"))
                {
                    Debug.Log("Add item");
                    InventoryManager.instance.AddItemToInventory(collider.GetComponent<Item>().inventoryItem);
                    Destroy(collider.gameObject);
                }
            }
        }
        else
        {
            ReleaseObject();
        }
    }
}
