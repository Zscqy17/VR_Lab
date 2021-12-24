using OneFlyLib;
using ProcessData;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Universal.Audio;

/**********************************************
* 模块名: Controller.cs
* 功能描述：EventController和StateController的基类，提供各种共用的方法和字段
***********************************************/

public class Controller : MonoBehaviour
{
    //场景里的UICanvas物体
    protected Transform canvas;

    //场景里的3DObjectCamera物体
    protected Transform objectCamera;

    //用于触发委托的类
    protected Invoker functionInvoker;

    protected UISceneProgress uiSceneProgress;

    virtual protected void Start()
    {
        Init();
    }

    /// <summary>
    /// Controller初始化
    /// </summary>
    virtual protected void Init()
    {
        objectCamera = GameObject.Find("3DObjectsCamera").transform;
        canvas = GameObject.Find("UICanvas").transform;
        uiSceneProgress = FindObjectOfType<UISceneProgress>();
        functionInvoker = GetComponent<Invoker>();
    }

    #region 场景内GameObject检索

    /// <summary>
    /// 通过物体名检索物体
    /// </summary>
    /// <param name="eventObjectNames"></param>
    /// <param name="afterTimeline"></param>
    /// <returns></returns>
    private GameObject[] GetGameObjectsByName(EventObjectString[] eventObjectNames, TriggerTiming activeType)
    {
        List<GameObject> objects = new List<GameObject>();

        for (int i = 0; i < eventObjectNames.Length; i++)
        {
            if (eventObjectNames[i].triggerTiming == activeType)
            {
                Debug.Log(eventObjectNames[i].name);
                if (canvas.FindChildRecursively(eventObjectNames[i].name))
                {
                    objects.Add(canvas.FindChildRecursively(eventObjectNames[i].name).gameObject);
                    continue;
                }
                else if (objectCamera.FindChildRecursively(eventObjectNames[i].name))
                {
                    objects.Add(objectCamera.FindChildRecursively(eventObjectNames[i].name).gameObject);
                    continue;
                }
                else
                    Debug.LogError("could not find object");
            }
            else
            {
                continue;
            }
        }
        return objects.ToArray();
    }

    /// <summary>
    /// 通过物体名检索物体
    /// </summary>
    /// <param name="names"></param>
    /// <returns></returns>
    private GameObject[] GetGameObjectsByName(string[] names)
    {
        List<GameObject> objects = new List<GameObject>();

        for (int i = 0; i < names.Length; i++)
        {
            if (canvas.FindChildRecursively(names[i]))
            {
                objects.Add(objectCamera.FindChildRecursively(names[i]).gameObject);
                continue;
            }
            else if (objectCamera.FindChildRecursively(names[i]))
            {
                objects.Add(objectCamera.FindChildRecursively(names[i]).gameObject);
                continue;
            }
            else
                continue;
        }

        return objects.ToArray();
    }

    /// <summary>
    /// 通过物体名检索物体
    /// </summary>
    /// <param name="names"></param>
    /// <returns></returns>
    private bool GetGameObjectByName(string name, out GameObject obj)
    {
        if (canvas.FindChildRecursively(name))
        {
            obj = canvas.FindChildRecursively(name).gameObject;
            return true;
        }
        else if (objectCamera.FindChildRecursively(name))
        {
            obj = objectCamera.FindChildRecursively(name).gameObject;
            return true;
        }
        else
        {
            obj = null;
            return false;
        }
    }

    /// <summary>
    /// 通过物体名检索物体
    /// </summary>
    /// <param name="names"></param>
    /// <returns></returns>
    private GameObject GetGameObjectByName(string name)
    {
        if (canvas.FindChildRecursively(name))
        {
            return canvas.FindChildRecursively(name).gameObject;
        }
        else if (objectCamera.FindChildRecursively(name))
        {
            return objectCamera.FindChildRecursively(name).gameObject;
        }
        return null;
    }

    /// <summary>
    /// transform类型递归查找子物体
    /// </summary>
    /// <returns>返回需要查找的子物体.</returns>
    /// <param name="parent">查找起点.</param>
    /// <param name="targetName">需要查找的子物体名字.</param>
    public static Transform FindFunc(Transform parent, string targetName)
    {
        Transform target = parent.Find(targetName);
        //如果找到了直接返回
        if (target != null)
            return target;
        //如果没有没有找到，说明没有在该子层级，则先遍历该层级所有transform，然后通过递归继续查找----再次调用该方法
        for (int i = 0; i < parent.childCount; i++)
        {
            //通过再次调用该方法递归下一层级子物体
            target = FindFunc(parent.GetChild(i), targetName);

            if (target != null)
                return target;
        }
        return target;
    }


    #endregion 场景内GameObject检索

    #region 对Asset里变量的操作的方法

    /// <summary>
    /// 设置卡牌激活/禁用
    /// </summary>
    /// <param name="list"></param>
    /// <param name="cardState"></param>
    virtual protected void SetTargetsList(int[] list, bool cardState)
    {
        if (list.Length > 0)
        {
            for (int i = 0; i < list.Length; i++)
                ManagerEvent.Call(Tips.SetCardsAvailable, cardState, list[i]);
        }
    }

    /// <summary>
    /// 显示右上方蓝框的语音提示信息
    /// </summary>
    /// <param name="audioClip"></param>
    /// <param name="content"></param>
    virtual protected void ShowAudioHint(AudioHintContent audioContent)
    {
        if (audioContent.audioHintText != "" && audioContent.audioHintClip)
        {
            AudioHint.Instance.ShowHint(audioContent.audioHintClip, audioContent.audioHintText);
        }
        else if (audioContent.audioHintClip)
        {
            AudioPlayer.Instance.StopSpeech();
            AudioHint.Instance.StopShowCoroutine();
            AudioPlayer.Instance.PlaySpeech(audioContent.audioHintClip);
        }
    }

    /// <summary>
    /// 显示左上方的黄框的语音提示信息
    /// </summary>
    /// <param name="content"></param>
    virtual protected void ShowOperationHint(string content)
    {
        if (content != "")
            TimelineUIDisplayer.Instance.ShowOperationHint(content);
    }

    /// <summary>
    /// 改变Image组件的Sprite
    /// </summary>
    /// <param name="changeOptions"></param>
    virtual protected void ChangeImageSprite(ChangingOptions[] changeOptions)
    {
        for (int i = 0; i < changeOptions.Length; i++)
        {
            Image image;
            SpriteRenderer spriteRenderer;
            var obj = GetGameObjectByName(changeOptions[i].optionName);
            if (obj != null)
            {
                if (obj.GetComponent<Image>())
                {
                    image = obj.GetComponent<Image>();
                    image.sprite = changeOptions[i].sprite;
                    //return;
                    continue;
                }
                else if (obj.GetComponent<SpriteRenderer>())
                {
                    spriteRenderer = obj.GetComponent<SpriteRenderer>();
                    spriteRenderer.sprite = changeOptions[i].sprite;
                    //return;
                    continue;
                }
            }
            else
                Debug.LogError("could not find the target object of change sprite");
        }
    }

    /// <summary>
    /// 改变物体的运动方式
    /// </summary>
    /// <param name="moveAreas"></param>
    virtual protected void SetObjectMovementByIndex(MoveArea[] moveAreas)
    {
        for (int i = 0; i < moveAreas.Length; i++)
        {
            TargetControlCenter.Instance.ManageModelMovement(moveAreas[i]);
        }
    }

    /// <summary>
    /// 播放effect类型音效
    /// </summary>
    /// <param name="clip"></param>
    virtual protected void PlayAudioEffect(AudioClip clip)
    {
        if (clip != null)
            AudioPlayer.Instance.PlayEffect(clip);
        else
            return;
    }

    /// <summary>
    /// 更换卡牌对应的模型
    /// </summary>
    /// <param name="targets"></param>
    virtual protected void SwapTarget(TargetSwap[] targets)
    {
        for (int i = 0; i < targets.Length; i++)
        {
            //无父物体名
            if (targets[i].parentName.Equals(string.Empty))
            {
                if (targets[i].newTargetName == "")
                {
                    TargetControlCenter.Instance.SwitchTarget(targets[i].targetIndex, null);
                    continue;
                }
                var obj = GetGameObjectByName(targets[i].newTargetName);
                if (obj != null)
                    TargetControlCenter.Instance.SwitchTarget(targets[i].targetIndex, obj);
            }
            else
            {
                var parent = GetGameObjectByName(targets[i].parentName);
                if (targets[i].newTargetName == "")
                {
                    TargetControlCenter.Instance.SwitchTarget(targets[i].targetIndex, parent);
                    continue;
                }
                Debug.Log("替换模型： " + "父物体：" + parent + "子物体： " + targets[i].newTargetName);
                //var obj = parent.transform.Find(targets[i].newTargetName).gameObject;
                var obj = FindFunc(parent.transform, targets[i].newTargetName).gameObject;
                if (obj != null)
                    TargetControlCenter.Instance.SwitchTarget(targets[i].targetIndex, obj);
            }
        }
    }


    /// <summary>
    /// 显示错误并记录
    /// </summary>
    /// <param name="eventAsset"></param>
    virtual protected void ShowError(EventAsset eventAsset)
    {
        if (eventAsset.errorInfo.errorIndex > 0)
        {
            TimelineUIDisplayer.Instance.AddErrorMessage(eventAsset.errorInfo.errorIndex);
        }
        if (eventAsset.errorInfo.hideHintTime != 0)
        {
            TimelineUIDisplayer.Instance.ShowErrorAll(eventAsset.errorInfo.errorContent, eventAsset.errorInfo.hideHintTime);
        }
    }

    /// <summary>
    /// 显式指定设置下一步步骤
    /// </summary>
    /// <param name="info"></param>
    public void SetNextState(NextStateInfo info)
    {
        StateController.Instance.nextStateInfo_state = info;
    }

    /// <summary>
    /// 隐藏语音和操作提示
    /// </summary>
    virtual protected void HideHints(EventAsset eventAsset)
    {
        if (eventAsset.hideOperationHint)
            Universal.OperationHint.Instance.CloseOperationHint();
        if (eventAsset.hideAudioHint)
        {
            AudioPlayer.Instance.StopSpeech();
            AudioHint.Instance.StopShowCoroutine();
        }

        if (StateController.Instance.effects.Length > 0)
        {
            int index = 0;
            for (; index < StateController.Instance.effects.Length; index++)
            {
                if (!StateController.Instance.effects[index])
                    break;
            }
            //自动隐藏提示,步骤最后一个事件触发后，立刻隐藏语音和操作提示
            if (index == StateController.Instance.effects.Length - 1 && StateController.Instance.AutoHideHint)
            {
                AudioPlayer.Instance.StopSpeech();
                AudioHint.Instance.StopShowCoroutine();
                Universal.OperationHint.Instance.CloseOperationHint();
            }
        }
    }

    /// <summary>
    /// 激活物体
    /// </summary>
    /// <param name="eventObjectNames"></param>
    /// <param name="afterTimeline"></param>
    virtual protected void ActivateGameObject(EventObjectString[] eventObjectNames, TriggerTiming activeType)
    {
        var objs = GetGameObjectsByName(eventObjectNames, activeType);

        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i])
            {
                objs[i].SetActive(true);
            }
        }
    }

    /// <summary>
    /// 隐藏物体
    /// </summary>
    /// <param name="eventObjectNames"></param>
    /// <param name="afterTimeline"></param>
    virtual protected void HideGameObject(EventObjectString[] eventObjectNames, TriggerTiming activeType)
    {
        var objs = new GameObject[eventObjectNames.Length];
        objs = GetGameObjectsByName(eventObjectNames, activeType);

        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i])
            {
                objs[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// 设置课程进度显示
    /// </summary>
    /// <param name="progress"></param>
    virtual public void SetCourseProgress(float progress)
    {
        if (progress < 0 || !uiSceneProgress)
            return;
        uiSceneProgress.SetProgress(Mathf.Clamp(progress, 0, 1));
    }

    #endregion 对Asset里变量的操作的方法
}