using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plataformaQuebravel : MonoBehaviour
{
    public GameObject plataforma;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetComponent<CopiaJogador>() == null && collision.gameObject.GetComponent<ControleSonic>().habilidadePisao == true)
            {
                ControleSonic boneco = collision.gameObject.GetComponent<ControleSonic>();
                boneco.habilidadePisao = true;
                boneco.animator.SetBool("NOCHAO", false);
                boneco.animator.SetBool("CAINDO", true);
                Destroy(plataforma);
            } 
            else if (collision.gameObject.GetComponent<ControleSonic>() == null && collision.gameObject.GetComponent<CopiaJogador>().habilidadePisao == true)
            {
                CopiaJogador boneco = collision.gameObject.GetComponent<CopiaJogador>();
                boneco.habilidadePisao = true;
                boneco.animator.SetBool("NOCHAO", false);
                boneco.animator.SetBool("CAINDO", true);
                Destroy(plataforma);
            }
        }
    }
}
