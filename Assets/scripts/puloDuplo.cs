using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class puloDuplo : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            ControleSonic script = collision.GetComponent<ControleSonic>();
            script.pulosMax++;
            script.assaDireita.SetActive(true);
            script.assaEsquerda.SetActive(true);
            Destroy(this.gameObject);
        }
    }
}
