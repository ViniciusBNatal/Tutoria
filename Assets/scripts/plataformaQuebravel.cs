using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plataformaQuebravel : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && collision.gameObject.GetComponent<ControleSonic>().habilidadePisao == true)
        {
            Destroy(this.gameObject);
        }
    }
}
