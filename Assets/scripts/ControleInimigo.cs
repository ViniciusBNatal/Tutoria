using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControleInimigo : MonoBehaviour
{
    public bool disparar;
    public bool movimentar;
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
    private Vector3 posicao;
    private float podeAtacar;
    private float podeDisparar;
    private bool detectado;
    // Start is called before the first frame update
    void Start()
    {
        largura = GetComponent<SpriteRenderer>().bounds.size.x;
        altura = GetComponent<SpriteRenderer>().bounds.size.y;
        animator = GetComponent<Animator>();

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
        Gizmos.DrawWireCube(posicao, new Vector2(largura, altura +1));
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        GameObject objeto = collision.gameObject;
        algoEncostou(objeto);
        
    }
    public void andar()
    {
        if (jogador != null)
        {
            posicao = transform.position + new Vector3(largura * sentido, 0f, 0f);
            transform.Translate(raio.normalized * Time.deltaTime * velocidade * sentido);
            transform.localScale = new Vector3(1f * sentido, 1f, 1f);
            RaycastHit2D[] colisao = Physics2D.BoxCastAll(posicao, new Vector2(largura, altura + 1), 0f, new Vector2(sentido, 0), 0f, chaoLayer);
            if (colisao.Length <= 0)
                sentido = -sentido;
        }
    }
    public void atirar()
    {
        if (Time.time > podeDisparar + taxaDeDisparo && jogador != null)
        {
            Vector3 direcao = jogador.transform.position - pontoDeDisparo.position;
            GameObject bala = Instantiate(projetil, pontoDeDisparo);
            bala.GetComponent<Rigidbody2D>().velocity = new Vector3(direcao.x, direcao.y, 0f) * velocidadeProjetil;
            podeDisparar = Time.time;
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
                Destroy(this.gameObject);
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
