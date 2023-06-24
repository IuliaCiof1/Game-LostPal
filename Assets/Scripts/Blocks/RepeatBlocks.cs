using System.Collections;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;

namespace Blocks
{
    public class RepeatBlocks : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputNumber; //number of iterations
        [SerializeField] private float secondsToWait;
        private Transform SnapPoint;
        private bool fail;
        private Outline outline;
        public void OnEnable()
        {
            SnapPoint = transform.parent.Find("SnapPoint");
            PlayerController.OnPlayerFails += PlayerEventHandler;
            PlayerController.OnPlayerWins += PlayerEventHandler;
            fail = false;
            // outline =  transform.parent.GetComponent<Outline>();
            // outline.enabled = true;
            StartCoroutine(Repeat());
        }

        IEnumerator Repeat()
        {
            int i;
            for (i = 0; i < int.Parse(inputNumber.text); i++)
            {
                for (int j = 0; j < SnapPoint.childCount; j++)
                {
                    Transform block = SnapPoint.GetChild(j);

                    block.GetChild(0).gameObject.SetActive(true);
                    outline =  block.GetComponent<Outline>();
                    outline.enabled = true;
                    
                    yield return new WaitUntil(() => !block.GetChild(0).gameObject.activeSelf); //wait until the gameobject on the block is disabled. Needed for repeat blocks
                    yield return new WaitForSeconds(secondsToWait); //wait until animation ends
                    outline.enabled = false;
                }
            }

            // if (i == int.Parse(inputNumber.text))
            // {
                gameObject.SetActive(false);
            //}
            
        }
        
        public void OnDisable()
        {
            PlayerController.OnPlayerFails -= PlayerEventHandler;
            PlayerController.OnPlayerWins -= PlayerEventHandler;
        }
        
        // void PlayerFailsHandler()
        // {
        //     //outline.enabled = false;
        //
        //     //fail = true;
        //     StopAllCoroutines();
        //     //PlayerController.OnPlayerFails -= PlayerFailsHandler;
        //     gameObject.SetActive(false);
        // }
        //
        // void PlayerWinsHandler()
        // {
        //     //outline.enabled = false;
        //
        //     //fail = true;
        //     StopAllCoroutines();
        //     //PlayerController.OnPlayerWins -= PlayerWinsHandler;
        //     gameObject.SetActive(false);
        // }
        
        void PlayerEventHandler()
        {
            StopAllCoroutines();
            gameObject.SetActive(false);
        }
    }
    
}
