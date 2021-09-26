using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControleInimigo2 : MonoBehaviour
{
    [Header("Valores gerais")]
    public int dano;
    public float velocidade;
    public float taxaDeAtaqueMelee;
    float largura;
    float altura;
    private float podeAtacar;
    private float sentidoMovimento;
    [Header("Componentes")]
    public LayerMask chaoLayer;
    public GameObject projetil;
    public Transform pontoDeDisparo;
    private Transform jogador;
    private Rigidbody2D rb;
    private Vector3 direcaoJogador;
    private Animator animator;
    private Vector3 raio = new Vector3(10f, 0f, 0f);
    private GameObject projetilInst;
    [Header("Características")]
    public bool disparar;
    public bool movimentacaoTerrestre;
    public bool movimentacaoAerea;
    public bool boss;
    public bool simplificarDisparo;
    public bool Cair;
    private Vector3 posicao;
    [Header("Variáveis de disparo")]
    public float velocidadeProjetil;
    public float taxaDeDisparo;
    public Vector2 direcaoProjetilFixo;
    private Vector3 direcaoProjetil;
    private bool atirou = true;
    private float primeiroDisparo;
    [Header("Variáveis de voador")]
    public float tempoParado;
    public float duracaoDash;
    public float forcaDash;
    public float intervaloEntreAtaques;
    private bool bloquearMovimentacao = false;
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
        if (boss)
        {
            movimentacaoTerrestre = true;
        }
    }
    void FixedUpdate()
    {
        if (jogador != null)
        {
            if (movimentacaoTerrestre)
            {
                if (boss)
                {
                    transform.Translate(raio.normalized * Time.deltaTime * velocidade * sentidoBoss);
                }
                else if (!boss && !bloquearMovimentacao)
                {                    
                    transform.Translate(raio.normalized * velocidade * sentidoMovimento * Time.deltaTime);
                }
            }
            if (movimentacaoAerea && !bloquearMovimentacao)
            {
                direcaoJogador = jogador.transform.position - transform.position;
                sentidoMovimento = (direcaoJogador.x / Mathf.Abs(direcaoJogador.x));
                rb.velocity = new Vector3(direcaoJogador.x, direcaoJogador.y, 0f) * velocidade;
                transform.localScale = new Vector3(sentidoMovimento * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(posicao, new Vector2(largura, altura + .2f));
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
            posicao = transform.position + new Vector3(largura * sentidoBoss, altura / 3, 0f);
            RaycastHit2D[] colisao = Physics2D.BoxCastAll(posicao, new Vector2(largura / 2, altura / 2), 0f, new Vector2(sentidoBoss, 0f), 0, chaoLayer);
            transform.localScale = new Vector3(sentidoBoss * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            if (colisao.Length > 0)
            {
                sentidoBoss = -sentidoBoss;
            }
        }
        else
        {
            sentidoMovimento = (direcaoJogador.x / Mathf.Abs(direcaoJogador.x));
            posicao = transform.position + new Vector3(largura * sentidoMovimento, 0f, 0f);
            transform.localScale = new Vector3(sentidoMovimento * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            RaycastHit2D[] colisao = Physics2D.BoxCastAll(posicao, new Vector2(largura, altura + .2f), 0f, new Vector2(sentidoMovimento, 0f), 0, chaoLayer);
            if (!Cair)
            {
                if (colisao.Length > 0)
                {
                    bloquearMovimentacao = false;
                }
                else
                {
                    bloquearMovimentacao = true;
                }
            }
        }
    }
    private void atirar()
    {
        primeiroDisparo = 0;
        direcaoProjetil = jogador.position - pontoDeDisparo.position;
        projetilInst = Instantiate(projetil, pontoDeDisparo);
        projetilInst.GetComponent<bala>().dano = dano;
        if (simplificarDisparo)
        {
            transform.localScale = new Vector3(direcaoProjetilFixo.x * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            projetilInst.GetComponent<bala>().veloc = new Vector3(direcaoProjetilFixo.x, direcaoProjetilFixo.y, 0f) * velocidadeProjetil;
        }
        else
        {
            sentidoMovimento = (direcaoProjetil.x / Mathf.Abs(direcaoProjetil.x));
            transform.localScale = new Vector3(sentidoMovimento * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            projetilInst.GetComponent<bala>().veloc = new Vector3(direcaoProjetil.x, direcaoProjetil.y, 0f) * velocidadeProjetil;
        }
    }
    private void VoadorPrepara()
    {
        bloquearMovimentacao = true;
        rb.velocity = Vector3.zero;
    }
    private void Dash()
    {
        Vector3 ultimaPosicaoJogador = direcaoJogador;
        rb.velocity = ultimaPosicaoJogador * forcaDash;
    }
    private void ParaDash()
    {
        bloquearMovimentacao = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            jogador = collision.transform;
            if (animator != null)
                animator.SetBool("ATACANDO", true);
            if (movimentacaoTerrestre)
            {
                StartCoroutine("MovHorz");
            }
            if (disparar)
            {
                primeiroDisparo = taxaDeDisparo - 1;
                StartCoroutine("Disparos");
            }
            if (movimentacaoAerea)
            {
                StartCoroutine("Voando");
            }
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
            if (obj.GetComponent<ControleSonic>() != null)
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
                        jogadorScript.atualizaBarraDeVida(dano, 1);
                        podeAtacar = Time.time;
                    }
                }
            } 
            else if (obj.GetComponent<CopiaJogador>() != null)
            {
                obj.GetComponent<CopiaJogador>().morrer();
            }
        } 
    }
    IEnumerator MovHorz()
    {
        while(jogador != null)
        {
            direcaoJogador = jogador.position - transform.position;
            andar();
            yield return new WaitForSeconds(.5f);
        }
    }
    IEnumerator Disparos()
    {
        while(jogador != null && atirou)
        {
            atirou = false;
            yield return new WaitForSeconds(taxaDeDisparo - primeiroDisparo);
            if (jogador != null)
                atirar();
            atirou = true;
        }
    }
    IEnumerator Voando()
    {
        while (jogador != null)
        {
            yield return new WaitForSeconds(intervaloEntreAtaques);
            VoadorPrepara();
            yield return new WaitForSeconds(tempoParado);
            Dash();
            yield return new WaitForSeconds(duracaoDash);
            ParaDash();
        }
    }
}
