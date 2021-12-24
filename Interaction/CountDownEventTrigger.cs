using UnityEngine;

/// <summary>
/// 倒计时间后触发事件，可挂载在一个空物体上，激活空物体后倒计时开始，到时间后会触发事件并隐藏这个空物体
/// 可重新激活物体重复触发事件
/// </summary>

public class CountDownEventTrigger : MonoBehaviour
{
    [Header("倒计时时间")]
    public float countDownTime;

    [Header("触发的事件")]
    public EventAsset eventAssset;

    private float beginTime; //倒计时开始的时间

    private bool flag; //事件是否已被触发

    private void OnEnable()
    {
        flag = false;
        beginTime = Time.time;   //物体被激活时获取当前时间
    }

    private void Update()
    {
        if (Time.time > beginTime + countDownTime && !flag)
        {
            Debug.Log("count down event");
            EventController.Instance.EventTrigger(eventAssset);
            flag = true;
            gameObject.SetActive(false);  //触发后自动隐藏物体
        }
    }
}