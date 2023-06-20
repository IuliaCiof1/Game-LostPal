using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButtonManager : MonoBehaviour
{
    public int SceneIndex { get; set; }

    public void GoToSceneIndex()
    {
        SceneManager.LoadScene(SceneIndex);
    }
}
