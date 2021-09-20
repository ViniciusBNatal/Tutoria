using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControleInimigo : MonoBehaviour
{
    [Header("Valores gerais")]
    public int dano;
    public float velocidade;
    public float taxaDeAtaqueMelee;
    float largura;
    float altura;
    private float podeAtacar;
    [Header("Componentes")]
    public LayerMask chaoLayer;
    public GameObject projetil;
    public Transform pontoDeDisparo;
    private Transform jogador;
    private Rigidbody2D rb;
    private Vector3 direcaoJogador;
    private Animator animator;
    private Vector3 raio = new Vector3(10f, 0f, 0f);
    [Header("Características")]
    public bool disparar;
    public bool movimentacaoTerrestre;
    public bool movimentacaoAerea;
    public bool boss;
    public bool simplificarDisparo;
    private Vector3 posicao;
    [Header("Variáveis de disparo")]
    public float velocidadeProjetil;
    public float taxaDeDisparo;
    public Vector2 direcaoProjetil;
    private float podeDisparar;
    [Header("Variáveis de voador")]
    public float tempoDeEspera;
    public float duracaoDash;
    public float forcaDash;
    private float pararDash;
    private bool bloquearMovimentacao, esperando, duranteDash;
    private float pacing;
    [Header("BOSS")]
    public float forcaRepulsaoJogador;
    public int vidaMax;
    float sentidoBoss = 1;
    int vida;
    private Vector3 direcao;
    // Start is called before the first frame update
    void Start()
    {
        largura = GetComponent<SpriteRenderer>().bounds.size.x;
        altura = GetComponent<SpriteRenderer>().bounds.size.y;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        vida = vidaMax;
    }

    // Update is called once per frame
    private void Update()
    {
        if (disparar)
        {
            atirar();
        }
    }
    void FixedUpdate()
    {
        direcaoJogador = jogador.transform.position - transform.position;
        if (movimentacaoTerrestre)
        {
            andar();
        } 
        if (movimentacaoAerea)
        {
            voar();
        }
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
    private void andar()
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
                float sentido = (direcaoJogador.x / Mathf.Abs(direcaoJogador.x));
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
    private void voar()
    {
        if (jogador != null)
        {
            direcaoJogador = jogador.transform.position - transform.position;
            if (!bloquearMovimentacao)
                rb.velocity = new Vector3(direcaoJogador.x, direcaoJogador.y, 0f) * velocidade;
            ataqueVoador();
        }
    }
    private void atirar()
    {
        if (Time.time > podeDisparar + taxaDeDisparo && jogador != null)
        {
            Vector3 direcaoProjetil = jogador.transform.position - pontoDeDisparo.position;
            GameObject bala = Instantiate(projetil, pontoDeDisparo);
            bala.GetComponent<bala>().dano = dano;
            if (simplificarDisparo)
            {
                pontoDeDisparo.localPosition = new Vector3(this.direcaoProjetil.x, this.direcaoProjetil.y, 0f);
                bala.GetComponent<Rigidbody2D>().velocity = new Vector2(this.direcaoProjetil.x, this.direcaoProjetil.y) * velocidadeProjetil;
                podeDisparar = Time.time;
            }
            else
            {
                float sentido = (direcaoProjetil.x / Mathf.Abs(direcaoProjetil.x));
                transform.localScale = new Vector3(sentido * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                bala.GetComponent<Rigidbody2D>().velocity = new Vector3(direcaoProjetil.x, direcaoProjetil.y, 0f) * velocidadeProjetil;
                podeDisparar = Time.time;
            }
        }
    }
    private void ataqueVoador()
    {
        if (Time.time > podeAtacar + taxaDeAtaqueMelee && bloquearMovimentacao == false)
        {
            bloquearMovimentacao = true;
            rb.velocity = Vector3.zero;
            esperando = true;
            pacing = Time.time;
        }
        if (Time.time > pacing + tempoDeEspera && bloquearMovimentacao == true && duranteDash == false)
        {
            Vector3 ultimaPosicaoJogador = direcaoJogador;
            rb.velocity = ultimaPosicaoJogador * forcaDash;
            esperando = false;
            duranteDash = true;
            pararDash = Time.time;
        }
        if (Time.time > pararDash + duracaoDash && esperando == false && bloquearMovimentacao == true)
        {
            bloquearMovimentacao = false;
            duranteDash = false;
            podeAtacar = Time.time;
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
    private void algoEncostou(GameObject obj)
    {
        if (obj.tag == "Player")
        {
            ControleSonic jogadorScript = obj.GetComponent<ControleSonic>();
            Rigidbody2D rbJogador = obj.GetComponent<Rigidbody2D>();
            if (obj.GetComponent<ControleSonic>().habilidadePisao == true)
            {
                if (boss)
                {
                    vida--;
                    jogadorScript.habilidadePisao = false;
                    rbJogador.velocity = new Vector2(0f, 0f);
                    rbJogador.velocity = new Vector2(0f, forcaRepulsaoJogador);
                    if (vida <= 0)
                        Destroy(this.gameObject);
                }
                else
                {
                    jogadorScript.habilidadePisao = false;
                    Destroy(this.gameObject);
                }
            }
            else
            {
                if (Time.time > taxaDeAtaqueMelee + podeAtacar)
                {
                    jogadorScript.atualizaBarraDeVida(dano);
                    podeAtacar = Time.time;
                }
            }
        }
    } 
}
