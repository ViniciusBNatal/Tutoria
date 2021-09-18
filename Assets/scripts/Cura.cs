using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cura : MonoBehaviour
{
    public int vidaRecuperada;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<ControleSonic>().atualizaBarraDeVida(vidaRecuperada);
            Destroy(this.gameObject);
        }
    }

}
