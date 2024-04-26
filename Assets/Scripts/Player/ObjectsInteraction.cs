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
    public float interactRadius = 1f;
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
        isHoldingObject = true;
        heldObject = obj;
        PickupObjectServerRpc(obj.GetComponent<NetworkObject>().NetworkObjectId);
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

    public void Interact(InputAction.CallbackContext callbackContext)
    {
        if (!isHoldingObject)
        {
            RaycastHit hit;
            Vector3 rayOrigin = playerCamera.Follow.position;
            if (Physics.SphereCast(rayOrigin, interactRadius, playerCamera.transform.forward, out hit, interactDistance))
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
                    if(!hit.collider.GetComponent<Door>().hasPuzzle)
                    {
                        hit.collider.GetComponent<Door>().OpenDoorRpc();
                        Debug.Log("Se detecto este collider " + hit.collider.gameObject.name);
                    }
                    
                }
                else if (hit.collider.CompareTag("PuzzleBrian"))
                {
                    Debug.Log("puzzle brian");
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

    [ServerRpc]
    public void PickupObjectServerRpc(ulong objToPickupID)
    {
        NetworkManager.SpawnManager.SpawnedObjects.TryGetValue(objToPickupID, out var objectToPickup);
        if (objectToPickup == null || objectToPickup.transform.parent != null) return;

        if (objectToPickup.TryGetComponent(out NetworkObject networkObject) && networkObject.TrySetParent(transform))
        {
            var pickUpObjectRigidbody = objectToPickup.GetComponent<Rigidbody>();
            pickUpObjectRigidbody.isKinematic = true;
            pickUpObjectRigidbody.interpolation = RigidbodyInterpolation.None;
            objectToPickup.GetComponent<NetworkTransform>().InLocalSpace = true;
            objectToPickup.transform.position = new Vector3(objectToPickup.transform.position.x, objectToPickup.transform.position.y + 2, objectToPickup.transform.position.z);
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
