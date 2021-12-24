using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 摇晃卡牌交互，在重置时间内达到摇晃次数触发事件。
/// </summary>
public class ShakeInteraction : InteractEvent {

    [Header("交互所在步骤")]
    public int targetStateIndex = -1;

    [Header("交互触发的事件")]
    public EventAsset eventAsset;

    [Header("摇晃的最小幅度")]
    public float swingDistance;

    RectTransform rectTransform;

    float currentX;

    bool posLeft = false;


    [Space(3)]
    [Header("目前摆动的次数")]
    int count;

    [Header("左右摆动的次数")]
    public int targetCount;

    [Header("重置初始的时间间隔")]
    public float resetTime;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public override void ResetInteractEvent()
    {
        Debug.Log("ResetInteractEvent");
        if (StateController.Instance.stateIndex == targetStateIndex)
        {
            Debug.Log("reset time" + resetTime);
            InvokeRepeating("ResetPosition", 0, resetTime);
            ifFinished = false;
            return;
        }
        Debug.Log("not the right state");
        ifFinished = true;
        CancelInvoke();
    }

    void Update()
    {
        //Debug.Log(IsInvoking());
        if (!ifFinished)
        {
            if (posLeft && rectTransform.position.x > currentX + swingDistance)
            {
                Debug.Log("swing to right");
                posLeft = false;
                count++;
                CheckCount();

            }
            else if (!posLeft && rectTransform.position.x < currentX - swingDistance)
            {
                Debug.Log("swing to left");
                posLeft = true;
                count++;
                CheckCount();

            }
        }
    }

    private void ResetPosition()
    {
        Debug.Log("reset position");
        currentX = rectTransform.position.x;
        count = 0;
    }

    private void CheckCount()
    {
        if (ifFinished)
            return;
        if (count >= targetCount * 2)
        {
            Debug.Log("shake event");
            EventController.Instance.EventTrigger(eventAsset);
            ifFinished = true;
        }
    }
}
