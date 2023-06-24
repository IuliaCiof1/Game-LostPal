using UnityEngine;

namespace Blocks
{
    public class PlayerMoveN : MonoBehaviour
    {
        [SerializeField]
        private PlayerController player;

        //Whenever this object is enabled/activated it will send some new position coordonates to the PlayerController
        //It will also enable movement
        public void OnEnable() 
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            player.Move = true;
            player.TargetPosition = player.transform.position + new Vector3(0, 0.33f,0);
            gameObject.SetActive(false);
        }
    }
}
