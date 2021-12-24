using UnityEngine;

/// <summary>
/// 卡牌交互的基类
/// </summary>
public class InteractEvent : MonoBehaviour
{
    /// <summary>
    /// 用于标识交互是否可用，当步骤不对或交互完成时置为true
    /// </summary>
    protected bool ifFinished = false;

    [Tooltip("交互的步骤下标")]
    protected int stateIndex = -1;

    protected EventController eventController;

    private void Awake()
    {
        eventController = FindObjectOfType<EventController>();
    }

    /// <summary>
    /// 检查当前步骤是否为交互步骤
    /// </summary>
    /// <returns></returns>
    protected bool CheckStateIndex()
    {
        if (stateIndex == StateController.Instance.stateIndex)
            return true;
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 交互脚本的重置，当进入新步骤后会立刻调用
    /// </summary>
    virtual public void ResetInteractEvent()
    {
        ifFinished = false;
    }


    /// <summary>
    /// 当播放完进入步骤的Timeline会调用该脚本，用于修改一些脚本绑定物体的属性
    /// </summary>
    virtual public void OnInteractionBegin()
    {

    }
}