using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plataformaQuebravel : MonoBehaviour
{
    public GameObject plataforma;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && collision.gameObject.GetComponent<ControleSonic>().habilidadePisao == true)
        {
            Debug.Log("a");
            collision.gameObject.GetComponent<ControleSonic>().habilidadePisao = true;
            collision.gameObject.GetComponent<ControleSonic>().animator.SetBool("NOCHAO", false);
            collision.gameObject.GetComponent<ControleSonic>().animator.SetBool("CAINDO", true);
            Destroy(plataforma);
        }
    }
}
