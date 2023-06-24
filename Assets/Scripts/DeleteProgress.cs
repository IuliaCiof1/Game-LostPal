using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteProgress : MonoBehaviour
{
   [SerializeField] private GameObject lvls;
   public void _DeleteProgress()
   {
      PlayerPrefs.DeleteKey("levelAt");
      for (int i = 1; i < lvls.transform.childCount; i++)
      {
         Destroy(lvls.transform.GetChild(i).gameObject);
      }
      lvls.GetComponent<LevelSelection>().CreateLvls();
   }
}
