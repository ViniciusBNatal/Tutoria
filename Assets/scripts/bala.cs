using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bala : MonoBehaviour
{
    public int dano;
    public Vector3 veloc;
    private Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine("velocidade");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<ControleSonic>().atualizaBarraDeVida(dano, 1);
            Destroy(this.gameObject);
        } 
        else if(collision.gameObject.tag != "plataforma" && collision.gameObject.tag != "Inimigo")
        {
            Destroy(this.gameObject);
        }
    }
    IEnumerator velocidade()
    {
        rb.velocity = veloc;
        yield return new WaitForSeconds(.5f);
    }
}
