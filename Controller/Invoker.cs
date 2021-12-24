using ProcessData;
using UnityEngine;
using UnityEngine.Events;

/**********************************************
* 模块名: EventControllerInvoker.cs
* 功能描述：用于调用UnityEvent的委托执行基类
***********************************************/

namespace ProcessData
{
    [System.Serializable]
    public struct EventFunction
    {
        [Header("Asset名")]
        public string AssetName;

        [Header("在timeline之前触发的函数")]
        public UnityEvent eneterFunction;

        [Header("在timeline之后触发的函数")]
        public UnityEvent exitFunction;
    }
}

public class Invoker : MonoBehaviour
{
    public EventFunction[] eventFunctions;

    virtual public void InvokeFunctions(string AssetName, TriggerTiming timing)
    {
        if (timing.Equals(TriggerTiming.BeforeTimeline))
        {
            var uEvent = GetEnterEvent(AssetName);
            if (uEvent != null)
            {
                uEvent.Invoke();
                Debug.LogWarning(uEvent.GetPersistentMethodName(0) + " is invoked!");
            }

        }
        else if (timing.Equals(TriggerTiming.AfterTimeline))
        {
            var uEvent = GetExitEvent(AssetName);
            if (uEvent != null)
                uEvent.Invoke();
        }
    }

    /// <summary>
    /// 获取Enter的委托
    /// </summary>
    /// <param name="AssetName"></param>
    /// <returns></returns>
    virtual protected UnityEvent GetEnterEvent(string AssetName)
    {
        foreach (var ef in eventFunctions)
        {
            if (ef.AssetName == AssetName)
                return ef.eneterFunction;
        }
        return null;
    }

    /// <summary>
    /// 获取Exit的委托
    /// </summary>
    /// <param name="AssetName"></param>
    /// <returns></returns>
    virtual protected UnityEvent GetExitEvent(string AssetName)
    {
        foreach (var ef in eventFunctions)
        {
            if (ef.AssetName == AssetName)
                return ef.exitFunction;
        }
        return null;
    }

    #region TEST CODE

    /// <summary>
    /// 测试用打印函数
    /// </summary>
    /// <param name="message"></param>
    public void PrintSomething(string message)
    {
        Debug.LogError(message);
    }

    #endregion TEST CODE
}