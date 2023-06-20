using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetHierarcy
{
    private string hierarchyString { get; set; }
    public string GetCodeEditorChildrenString(Transform parent, int depth)
    {
        string indent = new string(' ', depth*4);
        string name = parent.name;
        hierarchyString += indent + name[0] + name[1] + name[2] + '\n';
        
        if (parent.Find("SnapPoint") is not null)
        {
            parent = parent.Find("SnapPoint");
            for (int i = 0; i < parent.childCount; i++)
            {
                Transform child = parent.GetChild(i);
                GetCodeEditorChildrenString(child, depth + 1);
            }
        }
        Debug.Log(hierarchyString);
        return hierarchyString;
    }
}
