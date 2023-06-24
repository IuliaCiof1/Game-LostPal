using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeInImages : MonoBehaviour,IPointerClickHandler
{
    // Start is called before the first frame update
    [SerializeField] private float fadeInDuration;
    private int index;
    private Image[] images;
    void Start()
    {
        images = GetComponentsInChildren<Image>();
        index = 0;
        StartCoroutine(FadeIn());
    }

    private bool check;
    public IEnumerator FadeIn()
    {
        // foreach (Image image in GetComponentsInChildren<Image>())
        // {
            for (float i = 0; i <= 1; i += Time.deltaTime)
            {
                images[index].color = new Color(1, 1, 1, i);
                Debug.Log(Time.deltaTime);
                yield return null;
                check = false;
            }

            //yield return new WaitForSeconds(fadeInDuration);
       // }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (index < images.Length - 1)
        {
            check = true;
            images[index].color = new Color(1, 1, 1, 1);
            index++;
            StopAllCoroutines();
            StartCoroutine(FadeIn());
        }
        else
        {
            StopAllCoroutines();
            SceneManager.LoadScene("Level 1");
        }
    }
}
