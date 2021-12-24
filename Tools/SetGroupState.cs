using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

[Serializable]
public struct ObjectGroup
{
    [SerializeField]
    public string name;
    [SerializeField]
    public GameObject[] gameObjects;
}




public class SetGroupState : MonoBehaviour {
    [Header("配置需要测试的函数（包括多个函数组合）")]
    public UnityEvent[] testFunctionlines;


    [SerializeField]
    [Header("配置需要显示/隐藏的Gameobjects组")]
    public ObjectGroup[] objectGroups;


    public void SetActive(int index,bool state)
    {
        if (objectGroups.Length <= 0) { return; }
        for (int i = 0; i < objectGroups.Length; i++)
        {
            if (index == i&& objectGroups[i].gameObjects.Length>0)
            {
                foreach (GameObject obj in objectGroups[i].gameObjects)
                {
                    obj.SetActive(state);
                }
            }

        }
        
    }

    public void InvokeFunction(int index)
    {
        if (testFunctionlines.Length <= 0) { return; }
        for (int i = 0; i < testFunctionlines.Length; i++)
        {
            if (index == i)
            {
                if(testFunctionlines[i]!=null)
                { testFunctionlines[i].Invoke(); }
               
            }
        }

    }
}
