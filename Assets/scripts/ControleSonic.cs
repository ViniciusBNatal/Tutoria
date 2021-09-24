using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControleSonic : MonoBehaviour
{
    public LayerMask layerMascara;//quais layers vai ter verificação de colisão
    public Vector3 diferenca;
    public float forcaStomp;
    public GameObject barraDeVida;
    public Image iconeVida;
    public int vidaMaxima;
    public Transform respawn;
    public float velocidade;
    private List<Image> Vidas = new List<Image>();
    private Rigidbody2D rb;
    public Animator animator;
    private const float RAIO = 0.05f;
    private int vidaAtual;
    public bool habilidadePisao;
    private float posicaoAnterior;
    public Vector2 forcapulo;
    Vector3 inicio;
    private int pulos;
    private int pulosMax = 1;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        inicio = gameObject.transform.position;
        vidaAtual = vidaMaxima;
        pulos = pulosMax;

        for (int i = 0; i < vidaMaxima; i++)
        {
            Image temp = Instantiate<Image>(iconeVida, barraDeVida.transform) ;
            float largura = temp.rectTransform.rect.width;
            temp.transform.localPosition = new Vector3(largura * (Vidas.Count + 1), 0, 0);
            Vidas.Add(temp);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Stomp();
        movimentoHorizontal();
        Pular();
    }
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
            pulos = pulosMax;
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position - diferenca, RAIO);
    }
    private void Stomp()
    {
        if (Input.GetKeyDown(KeyCode.S) && animator.GetBool("NOCHAO") == false)
        {
            habilidadePisao = true;
            animator.SetTrigger("PISAO");
            rb.velocity = new Vector2(0f, 0f);
            rb.velocity = new Vector2(0, forcaStomp * -1);
        }
    }
    private void movimentoHorizontal()
    {
        float horz = Input.GetAxis("Horizontal");
        if (horz != 0)
        {
            animator.SetBool("CORRENDO", true);
            transform.Translate(velocidade * Time.deltaTime * horz, 0, 0);//anda o personagem direita/esquerda
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
        if (Input.GetKeyDown(KeyCode.Space) && pulos >= 1)
        {
            rb.AddForce(forcapulo, ForceMode2D.Impulse);
            animator.SetTrigger("PULAR");
            animator.SetBool("NOCHAO", false);
            pulos--;
        }
    }
    public void atualizaBarraDeVida(int valor)
    {
        if (valor > 0)//ganhou vida
        {
            if (valor + vidaAtual > vidaMaxima)
                valor = vidaMaxima - vidaAtual;
            for (int i = 0; i < valor; i++)
            {
                vidaAtual++;
                Image vida = Instantiate(iconeVida, barraDeVida.transform);
                float largura = vida.rectTransform.rect.width;
                vida.transform.localPosition = new Vector3(largura * (Vidas.Count + 1), 0, 0);
                Vidas.Add(vida);
            }
        } 
        else if (valor < 0)//perdeu vida
        {
            if (valor > vidaAtual)
                valor = vidaAtual;
            for (int i = 0; i > valor; i--)
            {
                vidaAtual--;
                Destroy(Vidas[Vidas.Count - 1]);
                Vidas.RemoveAt(Vidas.Count - 1);
            }
            if (vidaAtual <= 0)//morreu
            {
                morrer();
            }
        }
    }
    public void morrer()
    {
        if (respawn != null)
        {
            transform.position = respawn.position;
        }
        else
        {
            transform.position = inicio;
        }
        atualizaBarraDeVida(vidaMaxima);
    }
}
