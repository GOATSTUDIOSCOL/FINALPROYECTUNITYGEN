using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class conexiones : MonoBehaviour
{
    public GameObject almacen;
    public string nombre;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == nombre)
        {
            almacen.GetComponent<Seleccionar>().cantConexiones += 1;
            GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == nombre)
        {
            almacen.GetComponent<Seleccionar>().cantConexiones -= 1;
            GetComponent<SpriteRenderer>().color = Color.gray;
        }
    }
}
