using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProcessData;
using UnityEngine.Events;

namespace ProcessData
{
    [System.Serializable]
    public struct StateEvent
    {
        [Header("进入步骤所执行的函数")]
        public UnityEvent eneterEvent;
        [Header("离开步骤所执行的函数")]
        public UnityEvent exitEvent;
    }
}
public class StateEventManager : MonoBehaviour {

    [Header("步骤事件")]
    public StateEvent[] Events;

    public void PrintCurrentState()
    {
        if(StateController.Instance!=null)
           Debug.Log("目前的步骤序号为" + StateController.Instance.stateIndex);
    }

    public void PrintExiting()
    {
        Debug.Log("exiting call from stateEventManager");
    }
}
