using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class torreta : MonoBehaviour
{
    public float alcance;
    public float taxaDeDisparo;
    public float forca;
    float proximoTempoDeDisparo = 0;
    Vector2 direcao;
    public GameObject bala;
    public Transform pontoDeDisparo;
    public Transform alvo;
    private Animator animator;
    private CircleCollider2D areaDeteccao;
    private SpriteRenderer spriteJogador;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        areaDeteccao = GetComponent<CircleCollider2D>();
        spriteJogador = alvo.gameObject.GetComponent<SpriteRenderer>();
        areaDeteccao.radius = alcance;
    }
    // Update is called once per frame
    void Update()
    {
        Vector2 alvopos = alvo.position;
        direcao = alvopos - (Vector2)transform.position;

        if (animator.GetBool("ATACANDO") == true)
        {
            if (direcao.x < 0)
                transform.localScale = new Vector3(1f, 1f, 1f);
            else if (direcao.x > 0)
                transform.localScale = new Vector3(-1f, 1f, 1f);

            if (Time.time > proximoTempoDeDisparo)
            {
                proximoTempoDeDisparo = Time.time + 1 / taxaDeDisparo;
                atira();
            }
            if (Mathf.Abs(direcao.x) - spriteJogador.size.x/2 > alcance || Mathf.Abs(direcao.y) - spriteJogador.size.y / 2 > alcance)// calculo que retira a diferenca do pivot para as bordas do sprite do jogador
            {
                animator.SetBool("ATACANDO", false);
            }
        }
    }
    private void atira()
    {
        GameObject balains = Instantiate(bala, pontoDeDisparo.position, Quaternion.identity);
        balains.GetComponent<Rigidbody2D>().AddForce(direcao * forca);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            animator.SetBool("ATACANDO", true);
        }
    }
}