using System.Collections;
using System.Collections.Generic;
using Blocks.Mechanics;
//using Newtonsoft.Json.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    void Start()
    {
        CreateLvls();
    }

    public void CreateLvls()
    {
        int levelAt = PlayerPrefs.GetInt("levelAt", 1); //0 is the scene index of the MainMenu scene
        
        for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            GameObject btn = Instantiate(prefab);
            btn.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text = (i-1).ToString();
            btn.transform.SetParent(transform);
            btn.transform.GetComponent<SceneButtonManager>().SceneIndex = i;

            if (i > levelAt)
            {
                btn.GetComponent<Image>().color = Color.HSVToRGB(0.40f/3.6f, 0.51f, 0.82f); //hue from editor is different from hue in code
                btn.transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>().color = Color.HSVToRGB(0.20f/3.6f, 1, 0.82f);
                btn.GetComponent<Button>().interactable = false;
            }

        }
    }
}
