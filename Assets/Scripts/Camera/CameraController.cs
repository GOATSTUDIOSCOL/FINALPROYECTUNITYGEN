using Unity.Netcode;
using UnityEngine;
using Cinemachine;
using UnityEngine.UIElements;
using System.Collections;
using System.Collections.Generic;

public class CameraController : NetworkBehaviour
{
    public float mouseSensitivity = 100f;
    private float xRotation = 0f;
    public CinemachineVirtualCamera cvc;    
    public Dictionary<ulong, GameObject> cameras; 

    private void Awake()
    {
        cameras = new Dictionary<ulong, GameObject>();
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            cvc.Priority = -1;
            gameObject.SetActive(false);
        }

        UIManager.Instance.SpawnOrDespawnPlayer(cameras);
    }

    public override void OnNetworkDespawn()
    {
        Debug.Log("Disconnected client ID: " + NetworkObjectId + " size of camera controllers: " + cameras.Count);
        cameras.Remove(NetworkObjectId);
        Debug.Log("Disconnected client ID: " + NetworkObjectId + " size of camera controllers: " + cameras.Count);

        UIManager.Instance.SpawnOrDespawnPlayer(cameras);
    }

    public Dictionary<ulong, GameObject> AvailableCameras() {
        return cameras;
    }

    public void ActivateCamera(GameObject camera, int camPriority) {

    }   

    void FixedUpdate()
    {
        RotatePlayer();
        RotateCamera();
    }


    private void RotateCamera()
    {
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    private void RotatePlayer()
    {
        if (Input.GetAxis("Mouse X") != 0)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            Quaternion deltaRotation = Quaternion.Euler(0f, mouseX, 0f);
            Quaternion currentRotation = transform.parent.GetComponent<Rigidbody>().rotation;
            Quaternion newRotation = deltaRotation * currentRotation;
            transform.parent.GetComponent<Rigidbody>().MoveRotation(newRotation);
        }
        else
        {
            transform.parent.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
    }
}

