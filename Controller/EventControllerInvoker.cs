using UnityEngine;

/**********************************************
* 模块名: EventControllerInvoker.cs
* 功能描述：Event触发后执行事件触发类
***********************************************/

public class EventControllerInvoker : Invoker
{
    public void PrintCurrentState()
    {
        if (StateController.Instance != null)
            Debug.Log("目前的步骤序号为" + StateController.Instance.stateIndex);
    }

    public void PrintExiting()
    {
        Debug.Log("exiting call from stateEventManager");
    }
}