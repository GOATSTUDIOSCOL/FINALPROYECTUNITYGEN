using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using Unity.Netcode;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : NetworkBehaviour
{
    public static PlayerMovement Instance { get; private set;}
    #region MovementVariables
    [Header("Movement Settings")]
    public float speed = 5f;
    public Transform playerCamera;
    public Transform head;
    public AudioClip walkSound;
    #endregion

    #region Components
    private PlayerInputActions playerControls;
    private AudioSource playerAudio;
    private Animator animator;
    private Rigidbody rb;
    #endregion

    #region inputActions
    private InputAction move;
    private InputAction jump;
    private InputAction pause;
    #endregion

    #region States
    private bool isGrounded;
    private bool isPaused;
    #endregion

    public Dictionary<ulong, Transform> heads; 
    // private NetworkVariable<Dictionary<ulong, Transform>> current = new NetworkVariable<Dictionary<ulong, Transform>>();
    

    private GameObject pausePanel;

    public Transform hand;

    private void Awake()
    {
        Instance = this;
        playerControls = new PlayerInputActions(); 
        heads = new Dictionary<ulong, Transform>();
        UIManager.Instance.PlayerDead();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        FindObjectOfType<CinemachineVirtualCamera>().Follow = head.transform;
        playerAudio = GetComponent<AudioSource>();
        FixUI();
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            Debug.Log("Networkspawn PlayerMovement not mine" +  NetworkObjectId);
            enabled = false; 
            Debug.Log("Connected client ID: " + NetworkObjectId + " size of camera controllers: " + heads.Count);
        } else {
            Debug.Log("Networkspawn PlayerMovement mine" +  NetworkObjectId);
        }
        heads[NetworkObjectId] = head;
        UIManager.Instance.SpawnOrDespawnPlayer(heads, NetworkObjectId);
    }

    public override void OnNetworkDespawn()
    {
        Debug.Log("Disconnected client ID: " + NetworkObjectId + " size of camera controllers: " + heads.Count);
        if (heads.ContainsKey(NetworkObjectId))
            heads.Remove(NetworkObjectId);
        Debug.Log("Disconnected client ID: " + NetworkObjectId + " size of camera controllers: " + heads.Count);

        UIManager.Instance.SpawnOrDespawnPlayer(heads, NetworkObjectId);
    }

    public void SwitchHead(ulong headObjectId, ulong netid) {
        Debug.Log("CHANGING CAMERA");
        Debug.Log("SwitchHead " +  NetworkObjectId + " " + headObjectId);
        Debug.Log("SwitchHead " +  netid + " " + headObjectId);
        
        if (heads.ContainsKey(headObjectId)) {
            // SwitchHeadServerRpc(NetworkObjectId, headObjectId);
            Transform newHead = heads[headObjectId];
            FindObjectOfType<CinemachineVirtualCamera>().Follow = newHead;
        }
    }  

    [ServerRpc(RequireOwnership = false)]
    public void SwitchHeadServerRpc(ulong currentObjectId, ulong nextObjectId) {
        Debug.Log("RPC SwitchHead " +  currentObjectId + " " + nextObjectId);
        
        if (currentObjectId == nextObjectId) return;

        
        
        NetworkManager.SpawnManager.SpawnedObjects.TryGetValue(currentObjectId, out var currentObj);
        NetworkManager.SpawnManager.SpawnedObjects.TryGetValue(nextObjectId, out var nextObj);
        if (currentObj  == null || nextObj == null ) return;

        // if (objectToPickup.TryGetComponent(out NetworkObject networkObject) && networkObject.TrySetParent(transform))
        // {
        //     var pickUpObjectRigidbody = objectToPickup.GetComponent<Rigidbody>();
        //     pickUpObjectRigidbody.isKinematic = true;
        //     pickUpObjectRigidbody.interpolation = RigidbodyInterpolation.None;
        //     objectToPickup.GetComponent<NetworkTransform>().InLocalSpace = true;
        //     objectToPickup.transform.position = new Vector3(objectToPickup.transform.position.x, objectToPickup.transform.position.y + 1.5f, objectToPickup.transform.position.z);
        // }

        // if (heads.ContainsKey(headObjectId)) {

        //     Transform newHead = heads[headObjectId];
        //     FindObjectOfType<CinemachineVirtualCamera>().Follow = newHead;
        // }
    }  


    

    private void OnEnable()
    {
        SetMoveInput();
    }

    private void OnDisable()
    {
        move.Disable();
    }

    

    private void FixedUpdate()
    {
        Move();
    }

    public void FixUI()
    {
        GameObject.FindGameObjectWithTag("LobbyUI").SetActive(false);
        GameObject.FindGameObjectWithTag("InventoryUI").GetComponent<Canvas>().enabled = true;
        GameObject.FindGameObjectWithTag("InventoryUI").GetComponentInChildren<LoadingScreen>().enabled = true;
        GameManager.instance.GetComponent<AudioSource>().Play();
        GameManager.instance.gameStarted = true;
        GameManager.instance.HideCursor();
    }

    public void Move()
    {
        Vector2 input = move.ReadValue<Vector2>();
        animator.SetFloat("moveX", input.x);
        animator.SetFloat("moveY", input.y);
        if (input.magnitude > 0f)
        {
            float targetAngle = Mathf.Atan2(input.x, input.y) * Mathf.Rad2Deg;
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            moveDir = transform.TransformDirection(moveDir); // Transform the direction relative to player's rotation
            rb.MovePosition(rb.position + moveDir.normalized * speed * 2f * Time.deltaTime);
            if (!playerAudio.isPlaying)
                playerAudio.PlayOneShot(walkSound);
        }
        else
        {
            playerAudio.clip = null;
            playerAudio.Stop();
        }
    }

    private void SetMoveInput()
    {
        move = playerControls.Player.Move;
        move.Enable();
    }


}
