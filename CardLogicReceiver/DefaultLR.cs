using UnityEngine;
using Universal.Card;

/**********************************************
* Copyright (C) 2019 讯飞幻境（北京）科技有限公司
* 模块名: ClassControlCard.cs
* 创建者：RyuRae
* 修改者列表：
* 创建日期：
* 功能描述：
***********************************************/
public class DefaultLR : CardOptionControl
{
    [Header("对应卡牌名称")]
    public string cardName = "";
    [Header("所有右转事件")]
    public EventAsset[] rightEvents;
    [Header("所有左转事件")]
    public EventAsset[] leftEvents;

    protected override void CardLeftEvent(string name)
    {
        if (!string.IsNullOrEmpty(name) && name.Equals(cardName))
        {
            foreach(var current in leftEvents)
            {
                if (current.stateName.Equals(StateController.Instance.currentState))
                {
                    EventController.Instance.EventTrigger(current);
                }
            }
        }
    }


    protected override void CardRightEvent(string name)
    {
        if (!string.IsNullOrEmpty(name) && name.Equals(cardName))
        {
            foreach (var current in rightEvents)
            {
                if (current.stateName.Equals(StateController.Instance.currentState))
                {
                    EventController.Instance.EventTrigger(current);
                }
            }
        }
    }


}
