using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    public GameObject objAcoplado;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            objAcoplado.GetComponent<CopiaJogador>().StartCoroutine("Comando");
        }
    }
}
