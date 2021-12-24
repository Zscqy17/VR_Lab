using ProcessData;
using UnityEngine;

/// <summary>
/// 移除卡牌的交互脚本
/// </summary>
public class RemoveInteraction : InteractEvent
{
    [SerializeField]
    [Tooltip("交互信息")]
    private BasicInteraction[] basicInteractions;

    private BasicInteraction currentBasicInteraction;

    /// <summary>
    /// 模型移除时执行事件
    /// </summary>
    private void OnDisable()
    {
        if (ifFinished || StateController.Instance == null || TimelineManager.Instance.playableDirector == null)
        {
            return;
        }

        if (CheckStateIndex())
        {
            ifFinished = true;
            eventController.EventTrigger(currentBasicInteraction.eventAsset);
        }
    }

    override public void ResetInteractEvent()
    {
        foreach (BasicInteraction bi in basicInteractions)
        {
            if (bi.stateIndex == StateController.Instance.stateIndex)
            {
                ifFinished = false;
                currentBasicInteraction = bi;
                stateIndex = bi.stateIndex;
                return;
            }
        }

        ifFinished = true;
        stateIndex = -1;
    }
}