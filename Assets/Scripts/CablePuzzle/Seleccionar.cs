using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seleccionar : MonoBehaviour
{
    public GameObject objetoDetectado;
    public int cantConexiones;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (transform.position.y != 3.4f)
            {
                transform.position += new Vector3(0, 3.4f, 0);
            }
            
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (transform.position.y != -3.4f)
            {
                transform.position += new Vector3(0, -3.4f, 0);
            }
            
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            if(transform.position.x != -3.4F)
            {
                transform.position += new Vector3(-3.4F, 0, 0);
            }
           
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if(transform.position.x != 3.4f)
            {
                transform.position += new Vector3(3.4F, 0, 0);
            }
           
        }

        if (Input.GetKeyDown(KeyCode.E) & objetoDetectado != null)
        {
            objetoDetectado.transform.Rotate(0,0,-90);
        }

        if(cantConexiones == 7)
        {
            Debug.Log("puzzle resuelto");
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


