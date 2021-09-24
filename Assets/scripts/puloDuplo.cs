using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class puloDuplo : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<ControleSonic>().pulosMax++;
            Destroy(this.gameObject);
        }
    }
}
