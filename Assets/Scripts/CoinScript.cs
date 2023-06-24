using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Tree"))
        {
            col.GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 0.4f);
        }
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Tree"))
        {
            col.GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, 0.4f);
        }
    }
}
