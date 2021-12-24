using System.Collections.Generic;
using UnityEngine;

/**********************************************
* 模块名: CourseRecorder.cs
* 功能描述：记录事件、步骤是否完成的记录管理类,不会随着场景切换重新加载而销毁
***********************************************/

public class CourseRecorder : MonoBehaviour
{
    private List<string> events_finished;/// <summary>
                                         /// 完成的事件集合
                                         /// </summary>

    private List<string> states_finished;/// <summary>
                                         /// 完成的步骤集合
                                         /// </summary>

    public List<int> errors;            ///记录的错误

    public List<int> exp_finished;      ///完成的实验

    public static CourseRecorder Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            GameObject.Destroy(this.gameObject);
            return;
        }
        Instance = this;
        GameObject.DontDestroyOnLoad(this);
        errors = new List<int>();
        exp_finished = new List<int>();
        events_finished = new List<string>();
        states_finished = new List<string>();
    }

    /// <summary>
    /// 添加完成的事件名到完成列表中
    /// </summary>
    /// <param name="event_asset_name">完成的事件asset名</param>
    public void AddFinishedEvent(string event_asset_name)
    {
        // Debug.Log("add " + event_asset_name + " to event finished list");
        events_finished.Add(event_asset_name);
    }

    /// <summary>
    /// 添加完成的步骤名到完成列表中
    /// </summary>
    /// <param name="state_asset_name">完成的步骤asset名</param>
    public void AddFinishedState(string state_asset_name)
    {
        //Debug.Log("add " + state_asset_name + " to state finished list");
        states_finished.Add(state_asset_name);
    }

    /// <summary>
    /// 添加错误
    /// </summary>
    /// <param name="errorIndex"></param>
    public void AddError(int errorIndex)
    {
        errors.Add(errorIndex);
    }

    /// <summary>
    /// 将所有错误记录加入到后台课程结果中，用于最后结算
    /// </summary>
    public void AddErrorsToResult()
    {
        foreach (var err in errors)
            TimelineUIDisplayer.Instance.AddErrorMessage(err);
        ClearErrors();
    }

    /// <summary>
    /// 检查事件是否已经完成过
    /// </summary>
    /// <param name="event_asset_name">事件对应的asset名</param>
    /// <returns></returns>
    public bool CheckEventFinish(string event_asset_name)
    {
        foreach (var name in events_finished)
        {
            if (name == event_asset_name)
                return true;
        }
        return false;
    }

    /// <summary>
    /// 检查步骤是否已经完成过
    /// </summary>
    /// <param name="state_asset_name">步骤对应的asset名</param>
    /// <returns></returns>
    public bool CheckStateFinish(string state_asset_name)
    {
        foreach (var name in states_finished)
        {
            if (name == state_asset_name)
                return true;
        }
        return false;
    }

    /// <summary>
    /// 清空事件完成列表
    /// </summary>
    public void ClearFinishedEventList()
    {
        events_finished.Clear();
    }

    /// <summary>
    /// 清空步骤完成列表
    /// </summary>
    public void ClearFinishedStateList()
    {
        states_finished.Clear();
    }

    /// <summary>
    /// 清空错误列表
    /// </summary>
    public void ClearErrors()
    {
        errors.Clear();
    }

    /// <summary>
    /// 清空完成的实验
    /// </summary>
    public void ClearFinishedExperiment()
    {
        exp_finished.Clear();
    }

    #region codes for testing

    /// <summary>
    /// 打印完成的事件列表
    /// </summary>
    public void PrintFinishedEvent()
    {
        string temp = "";
        foreach (var key in events_finished)
        {
            temp += key;
            temp += ' ';
        }
        //Debug.Log("Events that have finished: " + temp);
    }

    /// <summary>
    /// 打印完成的步骤列表
    /// </summary>
    public void PrintFinishedState()
    {
        string temp = "";
        foreach (var key in states_finished)
        {
            temp += key;
            temp += ' ';
        }
        //Debug.Log("States that have finished: " + temp);
    }

    public void PrintIfEventFinish(string event_asset_name)
    {
        Debug.Log(CheckStateFinish(event_asset_name));
    }

    public void PrintIfStateFinish(string state_asset_name)
    {
        Debug.Log(CheckStateFinish(state_asset_name));
    }

    #endregion codes for testing
}