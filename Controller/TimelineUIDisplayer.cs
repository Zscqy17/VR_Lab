using OneFlyLib;
using System.Collections;
using UnityEngine;
using Universal;
using Universal.Audio;
using UnityEngine.SceneManagement;

/**********************************************
* 模块名: TimelineUIDisplayer.cs
* 功能描述：提供UI调用方法，封装了一些SDK中的方法
***********************************************/

public class TimelineUIDisplayer : MonoBehaviour
{
    static public TimelineUIDisplayer Instance;
    public UISceneHint uISceneHint;

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// 隐藏当前提示，当前提示包括结论UI、错误UI和结果UI，不包括左上角右上角的提示
    /// </summary>
    public void HideCurrentHint()
    {
        // var uiScene = FindObjectOfType<UISceneHint>();
        uISceneHint.HideCurrHint();
    }

    /// <summary>
    /// 显示实验结果
    /// </summary>
    public void ShowResult()
    {
        CourseRecorder.Instance.AddErrorsToResult();
        CourseControl.courseInfo.SetProgress(1 / 1);
        ManagerEvent.Call(Tips.ShowHint, UISceneHint.HintType.SUMMARY, "");
    }

    /// <summary>
    /// 显示实验结论
    /// </summary>
    /// <param name="text"></param>
    public void ShowConclusion(string text)
    {
        ManagerEvent.Call(Tips.ShowHint, UISceneHint.HintType.CONCLUSION, text);
    }

    /// <summary>
    /// 显示左上角屏幕操作提示UI
    /// </summary>
    /// <param name="str"></param>
    public void ShowOperationHint(string str)
    {
        OperationHint.Instance.ShowOperationHint(str);
    }

    /// <summary>
    /// 显示右上角屏幕UI
    /// 对应的语音语音需要手动在Timeline添加AudioTrack（AnimationEvent调用的函数只支持单个简单变量）
    /// </summary>
    /// <param name="text"></param>
    public void ShowAudioHint(string text)
    {
        AudioHint.Instance.ShowHint(null, text, null);
    }

    /// <summary>
    /// 隐藏右上角屏幕UI
    /// 需要手动调用来隐藏
    /// </summary>
    public void HideAudioHint()
    {
        AudioHint.Instance.StopShowCoroutine();
    }

    /// <summary>
    /// 显示错误信息
    /// </summary>
    /// <param name="text"></param>
    public void ShowError(string text)
    {
        ManagerEvent.Call(Tips.ShowHint, UISceneHint.HintType.ERROR, text);
    }

    public void ShowErrorAll(string text, float closeTime)
    {
        // AudioPlayer.Instance.PlayAudio(Universal.Audio.AudioType.effect, );      这里差一个参数，第二个参数为播放的“阿哦”错误音效，将音效添加到场景中的AudioPlayer下的Sound中，再传递下标
        ManagerEvent.Call(Tips.ShowHint, UISceneHint.HintType.ERROR, text);
        StopAllCoroutines();
        StartCoroutine(WaitForHidingHints(closeTime));
    }

    /// <summary>
    /// 将错误信息记录到后台，参数是对应的错误编号（错误信息记在config文件中）
    /// </summary>
    /// <param name="errorIndex"></param>
    public void AddErrorMessage(int errorIndex)
    {
        CourseControl.courseInfo.AddMessageInErrors(errorIndex);
    }

    /// <summary>
    /// 隐藏右上角的操作提示UI
    /// </summary>
    public void HideOperationHint()
    {
        OperationHint.Instance.CloseOperationHint();
    }

    /// <summary>
    /// 用于隐藏UI的计时协程
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    private IEnumerator WaitForHidingHints(float time)
    {
        yield return new WaitForSeconds(time);
        HideCurrentHint();
    }
}