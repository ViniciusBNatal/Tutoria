using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bala : MonoBehaviour
{
    public int dano;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "plataforma" && collision.gameObject.tag != "Inimigo")
        {
            if (collision.gameObject.tag == "Player")
            {
                collision.gameObject.GetComponent<ControleSonic>().atualizaBarraDeVida(dano);
            }
            Destroy(this.gameObject);
        }
    }
}
