using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControleInimigo : MonoBehaviour
{
    public bool disparar;
    public bool movimentar;
    public bool boss;
    public bool simplificarDisparo;
    public int dano = 1;
    public float velocidadeProjetil;
    public float taxaDeDisparo;
    public float velocidade;
    public float taxaDeAtaqueMelee;
    public LayerMask chaoLayer;
    private Transform jogador;
    public GameObject projetil;
    public Transform pontoDeDisparo;
    private Animator animator;
    private Vector3 raio = new Vector3(10f, 0f, 0f);
    float largura;
    float altura;
    float sentido = 1;
    public Vector2 direcaoProjetil;
    private Vector3 posicao;
    private float podeAtacar;
    private float podeDisparar;
    private bool detectado;
    float sentidoBoss = 1;
    int vida;
    public float forcaRepulsaoJogador;
    public int vidaMax;
    // Start is called before the first frame update
    void Start()
    {
        largura = GetComponent<SpriteRenderer>().bounds.size.x;
        altura = GetComponent<SpriteRenderer>().bounds.size.y;
        animator = GetComponent<Animator>();
        vida = vidaMax;
    }

    // Update is called once per frame
    private void Update()
    {
        if (disparar)
            atirar();
    }
    void FixedUpdate()
    {
        if (movimentar)
        andar();
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(posicao, new Vector2(largura / 4, altura - 1));
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        GameObject objeto = collision.gameObject;
        algoEncostou(objeto);
        
    }
    public void andar()
    {
        if (boss)
        {
            posicao = transform.position + new Vector3(largura * sentidoBoss, 0f, 0f);
            RaycastHit2D[] colisao = Physics2D.BoxCastAll(posicao, new Vector2(largura / 4, altura - 1), 0f, new Vector2(sentidoBoss, 0f), 0, chaoLayer);
            transform.Translate(raio.normalized * Time.deltaTime * velocidade * sentidoBoss);
            transform.localScale = new Vector3(sentidoBoss * transform.localScale.x, transform.localScale.y, transform.localScale.z);
            if (colisao.Length > 0)
            {
                sentidoBoss = -sentidoBoss;
            }
        }
        else
        {
            if (jogador != null)
            {
                Vector3 direcao = jogador.transform.position - transform.position;
                float sentido = (direcao.x / Mathf.Abs(direcao.x));
                posicao = transform.position + new Vector3(largura * sentido, 0f, 0f);
                RaycastHit2D[] colisao = Physics2D.BoxCastAll(posicao, new Vector2(largura, altura + 1), 0f, new Vector2(sentido, 0f), 0, chaoLayer);
                if (colisao.Length > 0)
                {
                    transform.Translate(raio.normalized * Time.deltaTime * velocidade * sentido);
                    transform.localScale = new Vector3(sentido * transform.localScale.x, transform.localScale.y, transform.localScale.z);
                }
            }
        }
    }
    public void atirar()
    {
        if (Time.time > podeDisparar + taxaDeDisparo && jogador != null)
        {
            GameObject bala = Instantiate(projetil, pontoDeDisparo);
            Vector3 direcao = jogador.transform.position - pontoDeDisparo.position;
            if (simplificarDisparo)
            {
                float sentido = (direcaoProjetil.x / Mathf.Abs(direcaoProjetil.x));
                transform.localScale = new Vector3(-sentido * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                bala.GetComponent<Rigidbody2D>().velocity = new Vector2(direcaoProjetil.x, direcaoProjetil.y) * velocidadeProjetil;
                podeDisparar = Time.time;
            }
            else
            {
                float sentido = (direcao.x / Mathf.Abs(direcao.x));
                transform.localScale = new Vector3(sentido * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                bala.GetComponent<Rigidbody2D>().velocity = new Vector3(direcao.x, direcao.y, 0f) * velocidadeProjetil;
                podeDisparar = Time.time;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            jogador = collision.transform;
            if (animator != null)
                animator.SetBool("ATACANDO", true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            jogador = null;
            if (animator != null)
                animator.SetBool("ATACANDO", false);
        }
    }
    public void algoEncostou(GameObject obj)
    {
        if (obj.tag == "Player")
        {
            if (obj.GetComponent<ControleSonic>().habilidadePisao == true)
            {
                if (boss)
                {
                    vida--;
                    obj.GetComponent<ControleSonic>().habilidadePisao = false;
                    obj.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
                    obj.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, forcaRepulsaoJogador);
                    if (vida <= 0)
                        Destroy(this.gameObject);
                }
                else
                {
                    obj.GetComponent<ControleSonic>().habilidadePisao = false;
                    Destroy(this.gameObject);
                }
            }
            else
            {
                if (Time.time > taxaDeAtaqueMelee + podeAtacar)
                {
                    obj.GetComponent<ControleSonic>().atualizaBarraDeVida(dano);
                    podeAtacar = Time.time;
                }
            }
        }
    } 
}
