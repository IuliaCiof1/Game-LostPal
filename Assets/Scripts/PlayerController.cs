using System;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRb;
    [SerializeField] private float speed = 7f;
    [SerializeField] private Animator animator;

    public bool Move { get; set; }
    public Vector2 TargetPosition { get; set; }

    private bool moveN, moveS, moveEW;

    //[SerializeField] private GameObject finishPanel;
    //public bool IsWin { get; set; }
    
    public delegate void PlayerWins();
    public static event PlayerWins OnPlayerWins, OnPlayerFails;

    private AudioSource audioSource;

    private Vector2 startPosition, newPosition;

    private BoxCollider2D colider;
    //private TilemapCollider2D[] objectTileColliders;
    
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;

        audioSource = GetComponent<AudioSource>();
        colider = GetComponent<BoxCollider2D>();
        // objectTileColliders = GameObject.FindObjectsOfType<TilemapCollider2D>();
        //Collider2D[] objectColliders = GameObject.FindObjectsOfType<Collider2D>();
    }

    private void Update()
    {
        // if(IsWin)
        //     OnPlayerWins?.Invoke();
    }

    // public Tilemap tilemap;
    //
    // public TileBase tilebase;
    // Update is called once per frame
    public void FixedUpdate()
    {
        // float offset = 0.66f;
        // float positionX = transform.position.x;
        // float positionY = transform.position.y;
        //
        // Vector3[] tilePositions =
        // {
        //     new Vector3(positionX, (positionY + offset), 0), new Vector3((positionX + offset), positionY,0 ), 
        //     new Vector3(positionX, (positionY + offset), 0), new Vector3((positionX + offset), positionY, 0)
        //
        // };
        //
        // //Debug.Log(tilePositions);
        // //
        // for (int i = 0; i < tilePositions.Length; i++)
        //  {
        //      //Debug.Log("controller "+tilePositions[i]);
        //      //tilemap.SetTile(tilemap.WorldToCell(tilePositions[i]), tilebase);
        //      //Debug.Log(tilemap.WorldToCell(tilePositions[i]));
        //      Debug.Log(tilemap.GetTile(tilemap.WorldToCell(tilePositions[i])));
        // }

        //Movement animation
         animator.SetBool("moveEW",moveEW);
         animator.SetBool("moveN",moveN);
         animator.SetBool("moveS",moveS);
        
         if (Move)
         {
             audioSource.enabled = true;//PlayOneShot(audioClip);
             // if (CheckIfBlockAhead())
             // {
             //     
             // }
             
             //If player moves to the right
             if (TargetPosition.y == transform.position.y && TargetPosition.x > transform.position.x)
             {
                 transform.localScale = new Vector2(-Math.Abs(transform.localScale.x),transform.localScale.y);
                 moveEW = true;
                 moveN = false;
                 moveS = false;
             }
        
             //If player moves to the left
             else if (TargetPosition.y == transform.position.y && TargetPosition.x < transform.position.x)
             {
                 transform.localScale = new Vector2(Math.Abs(transform.localScale.x),transform.localScale.y);
                 moveEW = true;
                 moveN = false;
                 moveS = false;
             }
             
             //If player moves up
             else if (TargetPosition.x == transform.position.x && TargetPosition.y > transform.position.y)
             {
                 moveEW = false;
                 moveN = true;
                 moveS = false;
             }
             
             //If player moves to the down
             else if (TargetPosition.x == transform.position.x && TargetPosition.y < transform.position.y)
             {
                 moveEW = false;
                 moveN = false;
                 moveS = true;
             }
             

             newPosition = Vector2.MoveTowards(playerRb.position, TargetPosition, speed * Time.deltaTime); 
             playerRb.MovePosition(newPosition);
             
             if (playerRb.position == TargetPosition)
             { 
                 Move = false;
                 moveEW = false;
                 moveN = false;
                 moveS = false;
                 colider.enabled = true;
             }
         }
         else
         {
             audioSource.enabled = false;
         }
         
        
    }

    // public bool CheckIfBlockAhead()
    // {
    //     for (int i = 0; i < objectTileColliders.Length; i++)
    //     {
    //         if (Vector2.Distance(transform.position, objectTileColliders[i].transform.position) <= 0.66f)
    //         {
    //             return true;
    //         }
    //     }
    // }
    

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Finish"))
        {
            //IsWin = true;
            OnPlayerWins?.Invoke();
        }
        else if (col.CompareTag("Tree"))
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

    public void RestartPosition()
    {
        TargetPosition = startPosition;
        transform.position = startPosition;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
       //TargetPosition = transform.position;
        RestartPosition();
        OnPlayerFails?.Invoke();
    }
}
