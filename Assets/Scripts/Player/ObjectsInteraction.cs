using UnityEngine.InputSystem;
using UnityEngine;
using Cinemachine;
using Unity.Netcode;
using Unity.Netcode.Components;
using Unity.VisualScripting;

public class ObjectsInteraction : NetworkBehaviour
{
    private GameObject heldObject;
    private bool isHoldingObject = false;
    private InputAction interact;
    private InputAction throwAction;
    private PlayerInputActions playerControls;
    public float pickUpRange = 5f;
    private CinemachineVirtualCamera playerCamera;
    public float interactDistance = 5f;
    private float interactRadius = 1f;
    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            enabled = false;
            return;
        }
    }
    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }
    private void Start()
    {
        SetInteractInput();
        SetThrowInput();
        playerCamera = FindObjectOfType<CinemachineVirtualCamera>();

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
        if (!isHoldingObject)
        {
            isHoldingObject = true;
            heldObject = obj;
            PickupObjectServerRpc(obj.GetComponent<NetworkObject>().NetworkObjectId);
        }
    }

    void ReleaseObject()
    {
        if (heldObject != null)
        {
            isHoldingObject = false;
            DropObjectServerRpc(heldObject.GetComponent<NetworkObject>().NetworkObjectId);
            heldObject = null;
        }
    }

    void ThrowObject(InputAction.CallbackContext callbackContext)
    {
        if (heldObject != null)
        {
            isHoldingObject = false;
            ThrowObjectServerRpc(heldObject.GetComponent<NetworkObject>().NetworkObjectId);
            heldObject = null;
        }
    }

    /*   public void Interact(InputAction.CallbackContext callbackContext)
       {
           if (!isHoldingObject)
           {
               RaycastHit[] hits;
               Vector3 rayOrigin = playerCamera.Follow.position;
               hits = Physics.SphereCastAll(rayOrigin, interactRadius, playerCamera.transform.forward, interactDistance);
               foreach (RaycastHit hit in hits)
               {
                   if (hit.collider.CompareTag("Grabbable"))
                   {
                       GrabObject(hit.collider.gameObject);
                   }
                   else if (hit.collider.CompareTag("Item"))
                   {
                       InventoryManager.instance.AddItemToInventory(hit.collider.GetComponent<Item>().inventoryItem);
                       RpcTest.instance.DespawnObjectRpc(hit.collider.GetComponent<NetworkObject>().NetworkObjectId);
                   }
                   else if (hit.collider.CompareTag("BoxPuzzle") && InventoryManager.instance.hasCard)
                   {
                       hit.collider.GetComponent<BoxActivation>().OpenBoxRpc();
                   }
                   else if (hit.collider.CompareTag("Door"))
                   {
                       if (!hit.collider.GetComponent<Door>().hasPuzzle)
                       {
                           hit.collider.GetComponent<Door>().OpenDoorRpc();                    }

                   }
                   else if (hit.collider.CompareTag("PuzzleBrian"))
                   {
                       hit.collider.GetComponentInChildren<Seleccionar>().isOnPuzzle = true;
                       hit.collider.GetComponentInChildren<Seleccionar>().playerMovement = GetComponent<PlayerMovement>();
                       hit.collider.GetComponentInChildren<Seleccionar>().cameraController = GetComponent<PlayerMovement>().playerCamera.GetComponent<CameraController>();
                       GetComponent<PlayerMovement>().enabled = false;
                       GetComponent<PlayerMovement>().playerCamera.GetComponent<CameraController>().enabled = false;
                   }
               }
           }
           else
           {
               ReleaseObject();
           }
       }
    */
    public void Interact(InputAction.CallbackContext callbackContext)
    {
        if (isHoldingObject)
        {
            ReleaseObject();
            return;
        }

        RaycastHit[] hits;
        Vector3 rayOrigin = playerCamera.Follow.position;
        hits = Physics.SphereCastAll(rayOrigin, interactRadius, playerCamera.transform.forward, interactDistance);

        foreach (RaycastHit hit in hits)
        {
            switch (hit.collider.tag)
            {
                case "Grabbable":
                    GrabObject(hit.collider.gameObject);
                    break;

                case "Item":
                    InventoryItem item = hit.collider.GetComponent<Item>().inventoryItem;
                    InventoryManager.instance.AddItemToInventory(item);
                    RpcTest.instance.DespawnObjectRpc(hit.collider.GetComponent<NetworkObject>().NetworkObjectId);
                    break;

                case "BoxPuzzle":
                    if (InventoryManager.instance.hasCard)
                    {
                        hit.collider.GetComponent<BoxActivation>().OpenBoxRpc();
                    } 
                    else if (!InventoryManager.instance.hasCard)
                    {
                        hit.collider.GetComponent<BoxActivation>().NoCardRpc();
                    }
                    break;

                case "Door":
                    Door door = hit.collider.GetComponent<Door>();
                    if (door != null && !door.hasPuzzle)
                    {
                        door.OpenDoorRpc();
                    }
                    break;

                case "PuzzleSlider":
                    hit.collider.GetComponent<TileReference>().tile.SetActive(true);
                    hit.collider.enabled = false;
                    break;
                case "Notes":
                    hit.collider.GetComponent<Notes>().SetNoteState();
                    break;
            }
        }
    }


    [ServerRpc]
    public void PickupObjectServerRpc(ulong objToPickupID)
    {
        NetworkManager.SpawnManager.SpawnedObjects.TryGetValue(objToPickupID, out var objectToPickup);
        if (objectToPickup == null) return;

        if (objectToPickup.TryGetComponent(out NetworkObject networkObject) && networkObject.TrySetParent(transform))
        {
            var pickUpObjectRigidbody = objectToPickup.GetComponent<Rigidbody>();
            pickUpObjectRigidbody.isKinematic = true;
            pickUpObjectRigidbody.interpolation = RigidbodyInterpolation.None;
            objectToPickup.GetComponent<NetworkTransform>().InLocalSpace = true;
            objectToPickup.transform.position = new Vector3(objectToPickup.transform.position.x, 1.6f, objectToPickup.transform.position.z);
        }
    }

    [ServerRpc]
    public void DropObjectServerRpc(ulong objToPickupID)
    {
        NetworkManager.SpawnManager.SpawnedObjects.TryGetValue(objToPickupID, out var objectToPickup);
        objectToPickup.transform.parent = null;
        var pickedUpObjectRigidbody = objectToPickup.GetComponent<Rigidbody>();
        pickedUpObjectRigidbody.isKinematic = false;
        pickedUpObjectRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        objectToPickup.GetComponent<NetworkTransform>().InLocalSpace = false;
    }

    [ServerRpc]
    public void ThrowObjectServerRpc(ulong objToPickupID)
    {
        NetworkManager.SpawnManager.SpawnedObjects.TryGetValue(objToPickupID, out var objectToPickup);
        objectToPickup.transform.parent = null;
        var pickedUpObjectRigidbody = objectToPickup.GetComponent<Rigidbody>();
        pickedUpObjectRigidbody.isKinematic = false;
        pickedUpObjectRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        pickedUpObjectRigidbody.AddForce(transform.forward * 10f, ForceMode.Impulse);
        objectToPickup.GetComponent<NetworkTransform>().InLocalSpace = false;
    }
}
