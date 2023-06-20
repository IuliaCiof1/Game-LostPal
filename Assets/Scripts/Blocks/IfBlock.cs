using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace Blocks
{
    public class IfBlock : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown conditionDropdown;
        [SerializeField] private Rigidbody2D player;
        private Transform snapPoint;

        private float secondsToWait = 0.4f;

        //private bool fail;

        [SerializeField] private Tilemap tilemap;
        private Outline outline;
        
        private void OnEnable()
        {
            
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
            snapPoint = transform.parent.Find("SnapPoint");
            //PlayerController.OnPlayerFails += PlayerFailsHandler;
            PlayerController.OnPlayerFails += PlayerEventHandler;
            PlayerController.OnPlayerWins += PlayerEventHandler;
            
            //fail = false;
            IfStatement();
        }

        public void IfStatement()
        {
            float offset = 0.33f;
            float positionX = player.transform.position.x;
            float positionY = player.transform.position.y;
            
            //get positions for nearby tiles
            Vector2[] tilePositions =
            {
                //North                                                  //East
                new Vector2(positionX, (positionY + offset)), new Vector2((positionX + offset), positionY), 
                //South                                                 //West
                new Vector2(positionX, (positionY - offset)), new Vector2((positionX - offset), positionY)
            };
            
            if ((conditionDropdown.options[conditionDropdown.value].text.Equals("obstacle North")
            && tilemap.GetTile(tilemap.WorldToCell(tilePositions[0])) is not null)
                
               || (conditionDropdown.options[conditionDropdown.value].text.Equals("obstacle East")
            && tilemap.GetTile(tilemap.WorldToCell(tilePositions[1])) is not null)
               
               || (conditionDropdown.options[conditionDropdown.value].text.Equals("obstacle South")
           && tilemap.GetTile(tilemap.WorldToCell(tilePositions[2])) is not null)
               
               || (conditionDropdown.options[conditionDropdown.value].text.Equals("obstacle West")
            && tilemap.GetTile(tilemap.WorldToCell(tilePositions[3])) is not null))
            {
                StartCoroutine(GetEveryBlock());
            }
            
            gameObject.SetActive(false);
        }

        IEnumerator GetEveryBlock()
        {
            foreach(Transform block in snapPoint.transform)
            {
                        
                //block = snapPoint.GetChild(j);

                block.GetChild(0).gameObject.SetActive(true);
                outline =  block.GetComponent<Outline>();
                outline.enabled = true;
                        
                yield return new WaitUntil(() => !block.GetChild(0).gameObject.activeSelf); //wait until the gameobject on the block is disabled. Needed for repeat blocks
                yield return new WaitForSeconds(secondsToWait); //wait until animation ends

                outline.enabled = false;
            }
        }
        
        // void PlayerFailsHandler()
        // {
        //     outline.enabled = false;
        //     fail = true;
        //     StopAllCoroutines();
        //     PlayerController.OnPlayerFails -= PlayerFailsHandler;
        //
        //     gameObject.SetActive(false);
        // }
        
        void PlayerEventHandler()
        {
            StopAllCoroutines();
            gameObject.SetActive(false);
        }
        
        public void OnDisable()
        {
            //PlayerController.OnPlayerFails -= PlayerFailsHandler;
            PlayerController.OnPlayerFails -= PlayerEventHandler;
            PlayerController.OnPlayerWins -= PlayerEventHandler;
        }
    }
}
