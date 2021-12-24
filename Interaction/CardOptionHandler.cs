using UnityEngine;
using Universal.Card;

public class CardOptionHandler : CardOptionControl
{
    [Header("对应的下屏UI")]
    public CardControl cardUI;

    [Header("左转触发的事件")]
    public EventAsset[] leftEvents;

    [Header("右转触发的事件")]
    public EventAsset[] rightEvents;

    [Header("自动设置卡牌选项激活/隐藏状态")]
    public bool autoActivate = false;

    // 自动隐藏会在选项触发后自动隐藏对应选项(只是一边并不是两边都同时隐藏)并在有旋转交互的步骤激活左(右)选项

    protected override void CardLeftEvent(string name)
    {
        if (!cardUI)
        {
            Debug.LogError(gameObject.name + "未指定对应的下屏UI！");
            return;
        }
        if (!string.IsNullOrEmpty(name) && name.Equals(cardUI.gameObject.name))
        {
            foreach (EventAsset currentEvent in leftEvents)
            {
                if (currentEvent.stateName.Equals(StateController.Instance.currentState))
                {
                    EventController.Instance.EventTrigger(currentEvent);
                    if (autoActivate)
                    {
                        var leftOption = cardUI.transform.GetChild(1);
                        if (leftOption != null)
                            leftOption.gameObject.SetActive(false);
                    }
                    return;
                }
            }
        }
    }

    protected override void CardRightEvent(string name)
    {
        if (!cardUI)
        {
            Debug.LogError(gameObject.name + "未指定对应的下屏UI！");
            return;
        }

        if (!string.IsNullOrEmpty(name) && name.Equals(cardUI.gameObject.name))
        {
            foreach (EventAsset currentEvent in rightEvents)
            {
                if (currentEvent.stateName.Equals(StateController.Instance.currentState))
                {
                    EventController.Instance.EventTrigger(currentEvent);
                    if (autoActivate)
                    {
                        var rightOption = cardUI.transform.GetChild(2);
                        if (rightOption != null)
                            rightOption.gameObject.SetActive(false);
                    }
                    return;
                }
            }
        }
    }

    /// <summary>
    /// 进入步骤播放完进入步骤动画后检查该步骤是否存在对应的旋转交互，如果有则激活对应左右选项
    /// </summary>
    public void AutoSetOptions()
    {
        if (!autoActivate)
            return;

        foreach (EventAsset currentEvent in leftEvents)
        {
            if (currentEvent.stateName.Equals(StateController.Instance.currentState))
            {
                cardUI.transform.GetChild(1)?.gameObject.SetActive(true);
                break;
            }
        }
        foreach (EventAsset currentEvent in rightEvents)
        {
            if (currentEvent.stateName.Equals(StateController.Instance.currentState))
            {
                cardUI.transform.GetChild(2)?.gameObject.SetActive(true);
                break;
            }
        }
    }
}