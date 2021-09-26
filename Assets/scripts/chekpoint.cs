using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chekpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<ControleSonic>().pontosRespawn[1] = this.transform;
        }
    }
}
