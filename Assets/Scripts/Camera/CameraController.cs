using Unity.Netcode;
using UnityEngine;
using Cinemachine;
using UnityEngine.UIElements;

public class CameraController : NetworkBehaviour
{
    public float mouseSensitivity = 100f;
    private float xRotation = 0f;
    public CinemachineVirtualCamera cvc;


    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            cvc.Priority = -1;
            gameObject.SetActive(false);
        }
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
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
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

