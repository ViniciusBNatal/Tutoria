using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jogador : MonoBehaviour
{
    private Rigidbody2D rb;
    private float posicaoAnterior;
    public float forcaPulo, velocidade;
    public float forcaStomp;
    float horz;
    float vecrt;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        horz = Input.GetAxis("Horizontal");
        vecrt = Input.GetAxis("Vertical");
        Stomp();

        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddForce(new Vector2(0, forcaPulo * velocidade * Time.deltaTime));
        }

        if (horz != 0)
        {
            rb.AddForce(new Vector2(horz * velocidade * Time.deltaTime, 0));
        }
        posicaoAnterior = transform.position.y;
    }
    private void Stomp()
    {
        if (Input.GetKeyDown(KeyCode.S) && transform.position.y < posicaoAnterior)
        {
            Debug.Log("stomp");
            rb.velocity = new Vector2(0f,0f);
            rb.velocity = new Vector2(0, forcaStomp * -1);
            //rb.AddForce(new Vector2(0, forcaStomp * Time.deltaTime * -1));
        }
    }
}
