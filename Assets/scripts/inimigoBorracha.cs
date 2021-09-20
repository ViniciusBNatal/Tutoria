using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inimigoBorracha : MonoBehaviour
{
    public Vector3 diferenca;
    public LayerMask layerMascara;
    public float RAIO;
    private bool podePular;
    public float forcaPulo;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
       
    }
    private void pular()
    {
         rb.velocity = new Vector2(0f, forcaPulo);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<ControleSonic>().morrer();
        }
        else
        {
            Collider2D[] colisoes = Physics2D.OverlapCircleAll(transform.position - diferenca, RAIO, layerMascara);
            if (colisoes.Length > 0)
                pular();
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position - diferenca, RAIO);
    }
}
