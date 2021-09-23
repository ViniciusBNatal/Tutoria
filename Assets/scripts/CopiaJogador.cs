using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CopiaJogador : MonoBehaviour
{
    public LayerMask layerMascara;//quais layers vai ter verificação de colisão
    private Vector3 diferenca = new Vector3(0f, .51f, 0f);
    public float forcaStomp;
    public int vidaMaxima;
    public float velocidade;
    private Rigidbody2D rb;
    public Animator animator;
    private const float RAIO = 0.05f;
    private int vidaAtual;
    public bool habilidadePisao = false;
    private float posicaoAnterior;
    public Vector2 forcapulo;
    Vector3 inicio;
    private int pulos = 2;
    public float tempoPulo;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        inicio = gameObject.transform.position;
        vidaAtual = vidaMaxima;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (transform.position.y < posicaoAnterior)
            animator.SetBool("CAINDO", true);
        posicaoAnterior = transform.position.y;
        Collider2D[] colisoes = Physics2D.OverlapCircleAll(transform.position - diferenca, RAIO, layerMascara);
       if (colisoes.Length == 0)
           animator.SetBool("NOCHAO", false);
        else
        {
            animator.SetBool("NOCHAO", true);
            habilidadePisao = false;
            animator.SetBool("CAINDO", false);
            pulos = 2;
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position - diferenca, RAIO);
    }
    private void Stomp()
    {
        if (animator.GetBool("CAINDO"))
        {
            habilidadePisao = true;
            animator.SetTrigger("PISAO");
            rb.velocity = new Vector2(0f, 0f);
            rb.velocity = new Vector2(0, forcaStomp * -1);
        }
    }
    private void movimentoHorizontal()
    {
        float horz = 1;
        if (horz != 0)
        {
            animator.SetBool("CORRENDO", true);
            transform.Translate(velocidade * Time.deltaTime * horz, 0, 0);//anda o personagem direita/esquerda
            Debug.Log("a");
            if (horz < 0)
                transform.localScale = new Vector3(-1, 1, 1);//vira personagem para esquerda
            else
                transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            animator.SetBool("CORRENDO", false);
        }
    }
    private void Pular()
    {
        if (animator.GetBool("NOCHAO"))
        {
            rb.AddForce(forcapulo, ForceMode2D.Impulse);
            animator.SetTrigger("PULAR");
            animator.SetBool("NOCHAO", false);
            pulos--;
        }
    }
    public void morrer()
    {
        Destroy(this.gameObject);
    }
    IEnumerator Comando()
    {
        Pular();
        yield return new WaitForSeconds(tempoPulo);
        Stomp();
    }
}
