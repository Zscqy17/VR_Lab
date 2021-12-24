using UnityEngine;
using Universal.Card;

public class SwingInteraction : InteractEvent
{
    [Header("第一次加上语音计时触发时间(进入步骤首次)")]
    public float countTime;

    [Header("触发一次错误后加上错误提示触发时间")]
    public float countTime_post;

    [Header("交互所在步骤下标")]
    public int targetStateIndex = -1;

    [Header("在时间内完成摇晃触发的Event")]
    public EventAsset eventAsset_Finish;

    [Header("到时间未摇晃触发的Event")]
    public EventAsset eventAsset_Unfinished;

    [Header("摇摆的中心点")]
    public float center_X = 0;

    [Header("左右摇晃离中心点的距离")]
    public float swingDistance = 0;

    [Header("摇摆触发事件的次数")]
    public int swingCount;

    private int count;//目前已摆动次数

    private bool ifLeft;  //物体位置，true为在基准点左边

    private Vector3 originalPos; //物体原位置

    private ModelControl modelControl;

    private void Awake()
    {
        eventController = FindObjectOfType<EventController>();
        modelControl = GetComponent<ModelControl>();
    }

    private void Update()
    {
        if (ifFinished)
            return;

        if (ifLeft && transform.localPosition.x > center_X + swingDistance)
        {
            Debug.Log("swing to right");
            ifLeft = false;
            count++;
            CheckCount();
        }
        else if (!ifLeft && transform.localPosition.x < center_X - swingDistance)
        {
            Debug.Log("swing to left");
            ifLeft = true;
            count++;
            CheckCount();
        }
    }

    private void Reset()
    {
        if (ifFinished)
        {
            CancelInvoke();
            return;
        }

        if (!ifFinished)
        {
            EventController.Instance.EventTrigger(eventAsset_Unfinished);
            CancelInvoke();
            Invoke("Reset", countTime_post);
        }

        if (modelControl)
            modelControl.updatePos = false;

        transform.localPosition = originalPos;
        count = 0;
    }

    public override void ResetInteractEvent()
    {
        if (targetStateIndex == StateController.Instance.stateIndex)
        {
            ifFinished = false;
            Invoke("Reset", countTime);
            originalPos = transform.localPosition;
        }
        else
        {
            ifFinished = true;
            CancelInvoke();
        }
    }

    private void CheckCount()
    {
        if (ifFinished)
            return;
        if (count >= swingCount * 2)
        {
            ifFinished = true;
            eventController.EventTrigger(eventAsset_Finish);
            CancelInvoke();
        }
        else
            return;
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}