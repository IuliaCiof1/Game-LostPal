using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeInText : MonoBehaviour
{
    private TMP_Text text;

    void OnEnable()
    {
        text = GetComponent<TMP_Text>();
        StartCoroutine(FadeIn());
    }

    private bool check;
        public IEnumerator FadeIn()
        {
            // foreach (Image image in GetComponentsInChildren<Image>())
            // {
                for (float i = 0; i <= 1; i += Time.deltaTime/3)
                {
                    text.color = new Color(1, 1, 1, i);
                    
                    yield return null;
                    check = false;
                }

                for (float i = 1; i >=0; i -= Time.deltaTime/3)
                {
                    text.color = new Color(1, 1, 1, i);
                    
                    yield return null;
                    check = false;
                }
                
                yield return new WaitForSeconds(1);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                //yield return new WaitForSeconds(fadeInDuration);
           // }
        }
}
