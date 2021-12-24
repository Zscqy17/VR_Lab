using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerResult : MonoBehaviour
{

    [SerializeField][Header("选择题的总数目")]
    int questionCount;

    float errorCount;//错误的题目数,float用于类型转换

    [SerializeField][Header("用于显示正确率的Text组件")]
    Text resultText;//用于显示正确率的Text组件

    void Start()
    {
        errorCount = 0;
    }

    /// <summary>
    /// 改变Text显示内容，每次记录错误都会调用，最后弹出UI正确率UI前也应该调用
    /// </summary>
    public void SetResult()
    {
        if (resultText)
        {
            resultText.text = ((questionCount - errorCount) / questionCount * 100).ToString("##0.00") + "%";
        }
        else
            Debug.LogError("请检查是否已指定AnswerResult脚本用于显示正确率的Text组件");
    }//*** 至少调用一次该方法 ***

    /// <summary>
    /// 选择错误时调用该方法记录错误，正确时无需调用
    /// </summary>
    public void TriggerWrongAnswer()
    {
        Debug.Log("TriggerWrongAnswer");
        errorCount+=1;
        SetResult();//每次调用都会重置Text内容
    }
}
