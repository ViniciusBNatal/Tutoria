using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControleSonic : MonoBehaviour
{
    [Header("Componentes")]
    public LayerMask layerMascara;//quais layers vai ter verificação de colisão
    public Vector3 diferenca;
    public GameObject barraDeVida;
    public GameObject assaEsquerda;
    public GameObject assaDireita;
    public Image iconeVida;
    public Transform respawn;
    public Animator animator;
    private Rigidbody2D rb;
    private List<Image> Vidas = new List<Image>();
    [Header("Valores numéricos")]
    public float velocidade;
    public float forcaStomp;
    public int vidaMaxima;
    public bool habilidadePisao;
    public Vector2 forcapulo;
    public int pulosMax = 1;
    private const float TamanhoCaixaX = .8f;
    private const float TamanhoCaixaY = .2f;
    private int vidaAtual;
    private float posicaoAnterior;
    Vector3 inicio;
    private int pulos;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        inicio = gameObject.transform.position;
        vidaAtual = vidaMaxima;
        pulos = pulosMax;
        StartCoroutine("verificarChao");

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
        {
            if (pulos == pulosMax)
                pulos -= 1;
            animator.SetBool("CAINDO", true);
        }
        posicaoAnterior = transform.position.y;
        
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position - diferenca, new Vector2(TamanhoCaixaX, TamanhoCaixaY));
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
        if (Input.GetKeyDown(KeyCode.Space) && pulos > 0)
        {
            pulos--;
            Debug.Log(pulos);
            rb.AddForce(forcapulo, ForceMode2D.Impulse);
            animator.SetTrigger("PULAR");
            animator.SetBool("NOCHAO", false);
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
    IEnumerator verificarChao()
    {
        while (true)
        {
            Collider2D[] colisoes = Physics2D.OverlapBoxAll(transform.position - diferenca, new Vector2(TamanhoCaixaX, TamanhoCaixaY), 0f, layerMascara);
            if (colisoes.Length == 0)
                animator.SetBool("NOCHAO", false);
            else
            {
                animator.SetBool("NOCHAO", true);
                habilidadePisao = false;
                animator.SetBool("CAINDO", false);
                pulos = pulosMax;
            }
            yield return new WaitForSeconds(.1f);
        }
    }
}
