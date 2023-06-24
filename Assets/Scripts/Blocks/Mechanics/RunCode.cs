using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Blocks.Mechanics
{
    public class RunCode : MonoBehaviour
    {
        [SerializeField] private Transform playerBlock, CodeEditor;

        [SerializeField] private float secondsToWait;
        
        private Transform SnapPoint;
    
        [SerializeField] private GameObject finishPanel;
        [SerializeField] private PlayerController player;

        private Button runBtnComponent;

        [SerializeField] private AudioClip errorSound;
        //private Transform block;
        private Outline outline;

        private string hierarchyString;
        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            runBtnComponent = GetComponent<Button>();
            SnapPoint = playerBlock.Find("SnapPoint");
        }

        public void Run()
        {
            //get the blocks inside the "Player" block that's inside the CodeEditor
            //only those blocks will be executed
            if (playerBlock.IsChildOf(CodeEditor))
            {
                // SnapPoint = playerBlock.Find("SnapPoint");

                StartCoroutine(RunCoroutine());
            }
        
        }

        //Caroutine is needed since for loops move faster than the frames and animation
        public IEnumerator RunCoroutine()
        {
                
                //Get every block inside the Player block and execute it
                for (int i = 0; i < SnapPoint.childCount; i++)
                {
                    runBtnComponent.enabled = false;
                    Transform block = SnapPoint.GetChild(i);

                    block.GetChild(0).gameObject.SetActive(true);
                    
                    //outline the block that is being executed
                    outline =  block.GetComponent<Outline>();
                    outline.enabled = true;
                    
                    yield return new WaitUntil(() => !block.GetChild(0).gameObject.activeSelf); //wait until the gameobject on the block is disabled. Needed for repeat blocks
                    yield return new WaitForSeconds(secondsToWait); //wait until animation ends

                    outline.enabled = false;
                }

                player.RestartPosition();
                runBtnComponent.enabled = true;
        }

        public void OnEnable()
        {
            PlayerController.OnPlayerWins += PlayerWinHandle;
            PlayerController.OnPlayerFails += PlayerFailsHandler;
        }

        public void OnDisable()
        {
            PlayerController.OnPlayerWins -= PlayerWinHandle;
            PlayerController.OnPlayerFails -= PlayerFailsHandler;
        }

        void PlayerWinHandle()
        {
            StopAllCoroutines();
            PlayerPrefs.SetInt("levelAt",SceneManager.GetActiveScene().buildIndex+1); //save progress
            runBtnComponent.enabled = false;
            finishPanel.SetActive(true);
            
            // GetHierarcy a = new GetHierarcy();
            //
            // //Debug.Log(a.GetCodeEditorChildrenString(playerBlock, 0));
            //
            // //Unity Analytics
            // AnalyticsService.Instance.CustomData("BlocksCombination", new Dictionary<string, object>
            //     {
            //         {"Level", SceneManager.GetActiveScene()},
            //         {"Combination", a.GetCodeEditorChildrenString(playerBlock, 0)},
            //         {"Time", Math.Round(Time.deltaTime,2)}
            //     });

            PlayerController.OnPlayerWins -= PlayerWinHandle;
            PlayerController.OnPlayerFails -= PlayerFailsHandler;
        }


        void PlayerFailsHandler()
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.PlayOneShot(errorSound);
            outline.enabled = false;
            StopAllCoroutines();
            runBtnComponent.enabled = true;
            //PlayerController.OnPlayerFails -= PlayerFailsHandler;
        }
    }
}
