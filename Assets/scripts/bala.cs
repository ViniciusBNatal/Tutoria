﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bala : MonoBehaviour
{
    public int dano = 1;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<ControleSonic>().atualizaBarraDeVida(dano);
        }
        Destroy(this.gameObject);
    }
}