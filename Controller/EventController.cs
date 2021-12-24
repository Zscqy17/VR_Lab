using ProcessData;
using UnityEngine;

/**********************************************
* 模块名: EventController.cs
* 功能描述：通过读取EventAsset中的参数执行操作，用于交互触发的效果
***********************************************/

public class EventController : Controller
{
    [HideInInspector]
    static public EventController Instance;

    [HideInInspector]
    public string objectID;

    private void Awake()
    {
        Instance = this;
    }

    protected override void Start()
    {
        Init();
    }

    /// <summary>
    /// 触发事件的接口
    /// </summary>
    /// <param name="eventAsset"></param>
    public void EventTrigger(EventAsset eventAsset)
    {
        //if (StateController.Instance.currentState.Equals(eventAsset.stateName))
        //{
            EventTriggerAtOnce(eventAsset);//触发事件后立刻执行的动作
            TimelineManager.Instance.PlayTimeline(eventAsset.timelineAsset, () => { EventTriggerCallBack(eventAsset); });//触发事件后播放Timeline后进行回调
        // }
       
    }

    /// <summary>
    /// 触发事件播放timeline后再执行的方法
    /// </summary>
    /// <param name="eventAsset"></param>
    private void EventTriggerCallBack(EventAsset eventAsset)
    {
        functionInvoker.InvokeFunctions(eventAsset.name, TriggerTiming.AfterTimeline);
        ActivateGameObject(eventAsset.gameObejcts_active, TriggerTiming.AfterTimeline);
        HideGameObject(eventAsset.gameObjects_hide, TriggerTiming.AfterTimeline);
        SetTargetsList(eventAsset.activeTargets, true);
        SetTargetsList(eventAsset.disableTargets, false);
        CourseRecorder.Instance.AddFinishedEvent(eventAsset.name);
        UpdateStateControllerEffect(eventAsset.canLoop);
    }

    /// <summary>
    /// 触发事件后播放timeline前立刻执行的方法
    /// </summary>
    /// <param name="eventAsset"></param>
    private void EventTriggerAtOnce(EventAsset eventAsset)
    {
        HideHints(eventAsset);
        ShowError(eventAsset);
        ShowAudioHint(eventAsset.audioHintContent);
        PlayAudioEffect(eventAsset.audioClip);
        functionInvoker.InvokeFunctions(eventAsset.name, TriggerTiming.BeforeTimeline);
        SetObjectMovementByIndex(eventAsset.moveArea);
        ChangeImageSprite(eventAsset.optionSprites);
        ActivateGameObject(eventAsset.gameObejcts_active, TriggerTiming.BeforeTimeline);
        HideGameObject(eventAsset.gameObjects_hide, TriggerTiming.BeforeTimeline);
        SetNextState(eventAsset.nextStateInfo);
    }

    /// <summary>
    /// 更新StateController的状态，当事件触发结束timeline播放回调时才会被调用
    /// </summary>
    public void UpdateStateControllerEffect(bool canLoop)
    {
        if (canLoop)        //若事件为CanLoop则不会更新步骤状态
            return;
        for (int i = 0; i < StateController.Instance.effects.Length; i++)
        {
            if (StateController.Instance.effects[i] != true)
            {
                Debug.Log("updated at state " + StateController.Instance.stateIndex);
                StateController.Instance.effects[i] = true;
                StateController.Instance.UpdateStateEffects();
                break;
            }
        }
    }
}