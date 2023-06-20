using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpW : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D player;
        
    public void OnEnable() 
    {
           
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        player.GetComponent<BoxCollider2D>().enabled = false;
        player.GetComponent<PlayerController>().Move = true;
        player.GetComponent<PlayerController>().TargetPosition = player.transform.position + new Vector3(-0.33f*2,0,0);
            
        gameObject.SetActive(false);
    }
}
