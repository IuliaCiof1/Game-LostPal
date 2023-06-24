using UnityEngine;
using UnityEngine.UI;

namespace Blocks
{
    public class playerMoveE : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody2D player;

        //Whenever this object is enabled/activated it will send some new position coordonates to the PlayerController
        //It will also enable movement
        private Outline outline;
        public void OnEnable() 
        {
            // outline =  transform.parent.GetComponent<Outline>();
            // outline.enabled = true;
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
            player.GetComponent<PlayerController>().Move = true;
            player.GetComponent<PlayerController>().TargetPosition = player.transform.position + new Vector3(0.33f, 0,0);

            //outline.enabled = false;
            gameObject.SetActive(false);
        }
    
    }
}
