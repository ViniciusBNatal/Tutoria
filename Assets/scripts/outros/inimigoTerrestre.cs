using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class inimigoTerrestre : MonoBehaviour
{
    [Header("Pathfinding")]
    public Transform alvo;
    public float distanciaAtivacao = 50f;
    public float tempoNovoPonto = .5f;

    [Header("Physics")]
    public float velocidade = 200f;
    public float distanciaProximoPonto = 3f;
    public float forcaPulo = .3f;
    public float alturaMinimaParaPular = .1f;

    [Header("Custom Behaviour")]
    public bool seguirHabilitadio = true;
    public bool puloHabilitado = true;
    public bool orientarDirecaoHabilitado = true;

    private Path path;
    private int pontoPatrulhaAtual = 0;
    bool noChao = false;
    Seeker seeker;
    Rigidbody2D rb;

    public void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, tempoNovoPonto);
    }

    private void FixedUpdate()
    {
        if(alvoEmDistancia() && seguirHabilitadio)
        {
            segueCaminho();
        }
    }
    private void UpdatePath()
    {
        if (seguirHabilitadio && alvoEmDistancia() && seeker.IsDone())
        {
            seeker.StartPath(rb.position, alvo.position, OnPathComplete);

        }
    }
    private void segueCaminho()
    {
        if (path == null)
            return;
        //chegou ao final do caminho
        if (pontoPatrulhaAtual >= path.vectorPath.Count)
            return;

        //verifica se colidiu com algo
        Vector3 offsetInicial = transform.position - new Vector3(0f, GetComponent<Collider2D>().bounds.extents.y + alturaMinimaParaPular);
        noChao = Physics2D.Raycast(offsetInicial, -Vector3.up, 0.05f);

        //calculo direção
        Vector2 direcao = ((Vector2)path.vectorPath[pontoPatrulhaAtual] - rb.position).normalized;
        Vector2 forca = direcao * velocidade * Time.deltaTime;

        //pulo
        if (puloHabilitado && noChao)
        {
            if (direcao.y > alturaMinimaParaPular)
                rb.AddForce(Vector2.up * velocidade * forcaPulo);
        }

        //movimento horizontal
        rb.AddForce(forca);

        //proximo ponto de patrulha
        float distancia = Vector2.Distance(rb.position, path.vectorPath[pontoPatrulhaAtual]);
        if (distancia < distanciaProximoPonto)
            pontoPatrulhaAtual++;

        //direção do sprite
        if (orientarDirecaoHabilitado)
        {
            if (rb.velocity.x > 0.05f)
            {
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            } else if (rb.velocity.x < -0.05f)
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
    private bool alvoEmDistancia()
    {
        return Vector2.Distance(transform.position, alvo.transform.position) < distanciaAtivacao;
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            pontoPatrulhaAtual = 0;
        }
    }
}
