using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seleccionar : MonoBehaviour
{
    public GameObject objetoDetectado;
    public int cantConexiones;
    public bool isOnPuzzle = false;
    int casNumberY = 5;
    int casNumberX = 5;
    public PlayerMovement playerMovement;
    public CameraController cameraController;
    public Door door;
    public Collider parentCollider;
    PlaySFX sfx;
    private void Start()
    {
        sfx = GetComponent<PlaySFX>();
    }
    private void Update()
    {
        if (isOnPuzzle)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (casNumberY < 6)
                {
                    transform.position += new Vector3(0, 0.34f, 0);
                    casNumberY += 3;
                    sfx.Play(1);
                }

            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                if (casNumberY > 3)
                {
                    transform.position += new Vector3(0, -0.34f, 0);
                    casNumberY -= 3;
                    sfx.Play(1);
                }

            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (casNumberX > 3)
                {
                    transform.position += new Vector3(0.34F, 0, 0);
                    casNumberX -= 3;
                    sfx.Play(1);
                }

            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (casNumberX < 6)
                {
                    transform.position += new Vector3(-0.34F, 0, 0);
                    casNumberX += 3;
                    sfx.Play(1);
                }
            }

            if (Input.GetKeyDown(KeyCode.E) & objetoDetectado != null)
            {
                objetoDetectado.transform.Rotate(0, 0, -90);
                sfx.Play(2);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isOnPuzzle= false;
                playerMovement.enabled = true;
                cameraController.enabled = true;
            }

            if (cantConexiones == 7)
            {
                isOnPuzzle = false;
                playerMovement.enabled = true;
                cameraController.enabled = true;
                sfx.Play(0);
                door.OpenDoorRpc();
                parentCollider.enabled = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("conexion"))
        {
            objetoDetectado = collision.gameObject;
        }
        else
        {
            objetoDetectado = null;
        }
    }



}


