using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TBContinued : MonoBehaviour
{
    [SerializeField] private TMP_Text tbcText;

    [SerializeField] private int timeToWait;
    private void OnEnable()
    {
        Dialog.OnDialogFinish += HandleOnDIalogFinish;
    }

    private void OnDisable()
    {
        Dialog.OnDialogFinish -= HandleOnDIalogFinish;
    }

    void HandleOnDIalogFinish()
    {
        Debug.Log("aaaaaaaaaaa");
        //tbcText.SetActive(true);
        tbcText.gameObject.SetActive(true);
        //StartCoroutine(LoadMainMenu());
    }

    IEnumerator LoadMainMenu()
    {
       yield return new WaitForSeconds(timeToWait);
        //SceneManager.LoadScene(0);
    }
}
