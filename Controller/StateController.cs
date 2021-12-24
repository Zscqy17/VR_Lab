using ProcessData;
using System.Collections.Generic;
using UnityEngine;

/**********************************************
* 模块名: StateController.cs
* 功能描述：步骤管理，读取StateAsset文件的参数，在每个步骤不同阶段执行不同动作的类
***********************************************/

public class StateController : Controller
{
    static public StateController Instance;

    [Header("当前状态名称")]
    public string currentState;

    [Header("当前配置文件")]
    public StateAsset currentStateAsset;

    [Header("所有配置文件")]
    public StateAsset[] stateAssets;

    [Header("步骤编号")]
    public int stateIndex = 0;

    [Header("完成情况")]
    public bool[] effects;

    [HideInInspector]
    public NextStateInfo nextStateInfo_state;

    [HideInInspector]
    public List<InteractEvent> interactEvents = new List<InteractEvent>();

    [HideInInspector]
    private List<CardOptionHandler> optionHandlers = new List<CardOptionHandler>();

    public bool AutoHideHint
    {
        get;
        set;
    }

    private void Awake()
    {
        Instance = this;
    }

    protected override void Start()
    {
        Init();
        LoadState(stateIndex);                      //激活时读取序号为stateIndex的配置文件
        CourseControl.instance.InitCourse();        //初始化课程
    }

    /// <summary>
    /// 当进入新的State后立刻执行的方法
    /// </summary>
    public void OnEnteringState(StateAsset stateAsset)
    {
        foreach (InteractEvent interactEvent in interactEvents)     //重置所有交互脚本
            interactEvent.ResetInteractEvent();
        nextStateInfo_state.loadAnotherState = false;
        ActivateGameObject(stateAsset.gameObjects_Active, TriggerTiming.BeforeTimeline);
        HideGameObject(stateAsset.gameObjects_Hide, TriggerTiming.BeforeTimeline);
        SwapTarget(stateAsset.targetswap);
        functionInvoker.InvokeFunctions(stateAsset.name, TriggerTiming.BeforeTimeline);
        TimelineManager.Instance.PlayTimeline(stateAsset.enterStateTimeline, () => OnTimelineEnd(stateAsset));
    }

    /// <summary>
    /// 播放完Timeline后进行执行，包括激活禁用卡牌、物体等
    /// </summary>
    public void OnTimelineEnd(StateAsset stateAsset)
    {
        Debug.Log("the current stage index is " + stateIndex);
        foreach (var carOptionHandler in optionHandlers)
            carOptionHandler.AutoSetOptions();
        foreach (var interactComponent in interactEvents)
            interactComponent.OnInteractionBegin();
        ShowAudioHint(stateAsset.audioHintContent);
        ShowOperationHint(stateAsset.operationHintText);
        functionInvoker.InvokeFunctions(stateAsset.name, TriggerTiming.AfterTimeline);
        ActivateGameObject(stateAsset.gameObjects_Active, TriggerTiming.AfterTimeline);
        HideGameObject(stateAsset.gameObjects_Hide, TriggerTiming.AfterTimeline);
        SetTargetsList(stateAsset.activeTargets_Enter, true);
        SetTargetsList(stateAsset.disableTargets_Enter, false);
        SetObjectMovementByIndex(stateAsset.moveArea);
        ChangeImageSprite(stateAsset.optionSprite);
        if (effects.Length == 0)
            UpdateStateEffects();
    }

    /// <summary>
    /// 步骤结束时执行，与Initialize相对，包括激活禁用卡牌、物体等，当满足该步骤条件后直接执行
    /// </summary>
    public void OnExitingState(StateAsset stateAsset)
    {
        SetTargetsList(stateAsset.activeTargets_Exit, true);
        SetTargetsList(stateAsset.disableTargets_Exit, false);
        if (stateAsset.progress > 0 && stateAsset.progress <= 1)
            SetCourseProgress(stateAsset.progress);//设置进度
        CourseRecorder.Instance.AddFinishedState(stateAsset.name);
    }

    #region Editor按钮调用方法

    /// <summary>
    /// 将步骤目标重置
    /// </summary>
    public void RestoreEffect()
    {
        for (int i = 0; i < effects.Length; i++)
        {
            effects[i] = false;
        }
    }

    #endregion Editor按钮调用方法

    #region StateController's Helper Functions

    /// <summary>
    /// StateController初始化
    /// </summary>
    override protected void Init()
    {
        base.Init();
        interactEvents.Clear();
        optionHandlers.Clear();

        foreach (InteractEvent uiObject in Resources.FindObjectsOfTypeAll<InteractEvent>())//获取所有交互脚本实例
        {
            if (uiObject.hideFlags == HideFlags.NotEditable || uiObject.hideFlags == HideFlags.HideAndDontSave)
            {
                continue;
            }
            else
                interactEvents.Add(uiObject);
        }

        var handlers = FindObjectOfType<CardOptionHandler>();
        foreach (var handler in optionHandlers)
            optionHandlers.Add(handler);

        currentState = "";
        currentStateAsset = null;
        effects = new bool[0];
    }

    /// <summary>
    /// 当步骤所有目标完成调用该方法读取下一步骤
    /// </summary>
    private void StateFinishLoad()
    {
        OnExitingState(currentStateAsset);
        if (stateIndex >= stateAssets.Length - 1 && !nextStateInfo_state.loadAnotherState)
        {
            return;             //最后一步，结束
        }
        int nextIndex = 0;
        if (nextStateInfo_state.loadAnotherState)
        {
            if (nextStateInfo_state.index >= stateAssets.Length || nextStateInfo_state.index < 0)
                Debug.LogError("显式指定的下一步骤的下标超出范围");
            else
                nextIndex = nextStateInfo_state.index;
        }
        else
            nextIndex = ++stateIndex;
        LoadState(nextIndex);
    }

    /// <summary>
    /// 更新StateController步骤目标状态
    /// </summary>
    public void UpdateStateEffects()
    {
        if (stateAssets != null)
        {
            if (effects != null)
            {
                if (!CheckEffectsFinish())   // 检查是否所有步骤是否已完成
                    return;
                else
                    TimelineManager.Instance.PlayTimeline(currentStateAsset.exitStateTimeline, () => { StateFinishLoad(); });
            }
        }
    }

    /// <summary>
    /// 进入指定下标步骤
    /// </summary>
    /// <param name="index"></param>
    public void LoadState(int index)
    {
        stateIndex = index;
        currentStateAsset = stateAssets[index];
        LoadStateAsset(currentStateAsset);
        OnEnteringState(currentStateAsset);
    }

    /// <summary>
    /// 检查当前步骤目标是否完成
    /// </summary>
    /// <param name="vs"></param>
    /// <returns></returns>
    public bool CheckEffectsFinish()
    {
        foreach (var effect in effects)
        {
            if (!effect)
                return false;
        }
        return true;
    }

    /// <summary>
    /// 读取配置文件信息
    /// </summary>
    /// <param name="asset"></param>
    public void LoadStateAsset(StateAsset asset)
    {
        if (asset.stateName != "")
        {
            currentState = asset.stateName;
        }
        if (asset.effects != null)
        {
            effects = new bool[asset.effects.Length];
            for (int i = 0; i < asset.effects.Length; i++)
            {
                effects[i] = asset.effects[i];
                effects[i] = false;
            }
        }
        AutoHideHint = asset.autoHideHint;
    }

    #endregion StateController's Helper Functions
}