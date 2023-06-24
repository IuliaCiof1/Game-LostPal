using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WallErrorMessage : MonoBehaviour
{
    private Image image;
    private TMP_Text text;
    public void OnEnable()
    {
        PlayerController.OnPlayerFails += OnPlayerFailsHandler;
    }

    public void OnDisable()
    {
        PlayerController.OnPlayerFails -= OnPlayerFailsHandler;
    }

    public void Start()
    {
        image = GetComponent<Image>();
        text = transform.GetChild(0).GetComponent<TMP_Text>();
    }

    public void OnPlayerFailsHandler()
    {
        StopAllCoroutines();
        image.enabled = true;
        text.gameObject.SetActive(true);
        StartCoroutine(FadeOut());
    }

    public IEnumerator FadeOut()
    {
        for (float i = 1; i >= 0; i -= (Time.deltaTime/5))
        {
            image.color = new Color(1, 1, 1, i);
            text.color = new Color(0.8f, 0.09f, 0.09f, i);
            yield return null;
        }
        
        image.enabled = false;
        text.gameObject.SetActive(false);
        image.color = new Color(1, 1, 1, 1);
        text.color = new Color(0.8f, 0.09f, 0.09f, 1);
    }
}
