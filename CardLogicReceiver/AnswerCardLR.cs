using UnityEngine;
using Universal.Card;
using UnityEngine.UI;

/**********************************************
* Copyright (C) 2019 讯飞幻境（北京）科技有限公司
* 模块名: ClassControlCard.cs
* 创建者：RyuRae
* 修改者列表：
* 创建日期：
* 功能描述：
***********************************************/
public class AnswerCardLR : CardOptionControl
{
    [System.Serializable]
    public struct Answer
    {
        public int stateIndex;
        public string rightAnswer;
        public EventAsset rightEvent;
        public EventAsset wrongEvent;
    }

    [Header("答案组")]
    public Answer[] answers;
    [Header("对应卡牌名称")]
    public string cardName = "";
    [Header("正确率文本组件")]
    public Text rightRateText;
    [Header("对应卡牌选择器")]
    public CardChoose cardChoose;
    private int rightCount;

    public void ResetRightRate()
    {
        rightCount = 0;
    }

    public void SetRightRateText()
    {
        float rightRate = (float)rightCount / answers.Length;
        rightRateText.text = string.Format("{0:p2}",rightRate); 
    }


    protected override void CardLeftEvent(string name)
    {
        if (!string.IsNullOrEmpty(name) && name.Equals(cardName))
        {
        }
    }


    protected override void CardRightEvent(string name)
    {
        if (!string.IsNullOrEmpty(name) && name.Equals(cardName))
        {
            if (cardChoose.currentOption == null)
            {
                return;
            }
            foreach (var current in answers)
            {
                if (current.stateIndex == StateController.Instance.stateIndex)
                {
                    if (cardChoose.currentOption.Equals(current.rightAnswer))
                    {
                        rightCount++;
                        EventController.Instance.EventTrigger(current.rightEvent);
                    }
                    else
                    {
                        EventController.Instance.EventTrigger(current.wrongEvent);
                    }
                    return;
                }
            }
        }
    }


}
