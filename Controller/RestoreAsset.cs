using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SceneObjName
{
    [Tooltip("绑定的模型名称")]
    public string objName;
    [Tooltip("绑定的下屏卡牌UI名称")]
    public string UIName;

    public SceneObjName(string _objName, string _uiName)
    {
        objName = _objName;
        UIName = _uiName;
    }
}

[CreateAssetMenu(menuName = "RestoreAsset", fileName = "restoreAsset", order = 4)]
public class RestoreAsset : ScriptableObject
{
    [Header("跳转的步骤编号")]
    public int stateIndex;

    [Header("重置的UICanvas预制体")]
    public GameObject UICanvasPrefab;

    [Header("重置的3DObjectCamera预制体")]
    public GameObject ObjectCameraPrefab;
    
    [Header("存储ControlCenter中绑定信息的List")]
    public List<SceneObjName> objs=new List<SceneObjName>();

    [Header("存储当前激活卡牌信息的List")]
    public List<int> activeCard=new List<int>();

    public float progress;
}
