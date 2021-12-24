using DevelopEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

/**********************************************
* 模块名: TimelineManager.cs
* 功能描述：用于Timeline播放及回调事件的执行
***********************************************/

public class TimelineManager : MonoSingleton<TimelineManager>
{
    [HideInInspector]
    public PlayableDirector playableDirector;

    private PlayableDirector tempPlaybleDirector;

    private void Awake()
    {
        if (!playableDirector)
        {
            playableDirector = GetComponent<PlayableDirector>();
        }
        tempPlaybleDirector = playableDirector;
    }

    /// <summary>
    /// 播放timeline并执行回调action()
    /// </summary>
    /// <param name="asset"></param>
    /// <param name="action"></param>
    public void PlayTimeline(TimelineAsset asset, Action action = null)
    {
        if (asset)
        {
            var playableDirectorGameObject = new GameObject(asset.name);
            var new_playableDirector = playableDirectorGameObject.AddComponent<PlayableDirector>();//播放Timeline时会临时添加一个PlaybleDirector
            new_playableDirector.extrapolationMode = DirectorWrapMode.None; //初始化
            new_playableDirector.playOnAwake = false;
            StartCoroutine(WaitTimelinePlay(asset, new_playableDirector, action));
        }
        else
            action();
    }

    #region HELPER FUNCTION

    /// <summary>
    /// Event调用播放timeline的协程
    /// </summary>
    /// <param name="asset"></param>
    /// <param name="new_playableDirector">临时创建的新playable director</param>
    /// <param name="action">回调执行的委托</param>
    /// <returns></returns>
    private IEnumerator WaitTimelinePlay(TimelineAsset asset, PlayableDirector new_playableDirector, Action action)
    {
        ResetTimelineBinding(asset, new_playableDirector);
        new_playableDirector.Play(asset);
        yield return new WaitForSeconds((float)new_playableDirector.playableAsset.duration);

        if (new_playableDirector != null)
        {
            if (new_playableDirector.gameObject.activeInHierarchy)//创建的新PlayableDirector物体在场景中激活才会执行回调
                action();
            Destroy(new_playableDirector.gameObject); //播放完毕后销毁创建的新物体
        }
    }

    /// <summary>
    /// 获取原PlayableDirector上tracks和object的bindings并复制到新的Director上
    /// </summary>
    /// <param name="timelineAsset"></param>
    /// <param name="new_playableDirector">临时创建的新playable director</param>
    private void ResetTimelineBinding(TimelineAsset timelineAsset, PlayableDirector new_playableDirector)
    {
        tempPlaybleDirector.playableAsset = timelineAsset;
        new_playableDirector.playableAsset = timelineAsset;

        List<PlayableBinding> newBindingList = new List<PlayableBinding>();
        List<PlayableBinding> oldBindingList = new List<PlayableBinding>();

        foreach (PlayableBinding pb in tempPlaybleDirector.playableAsset.outputs)
        {
            //  Debug.Log(pb);
            oldBindingList.Add(pb);
        }

        foreach (PlayableBinding pb in new_playableDirector.playableAsset.outputs)
        {
            // Debug.Log(pb);
            newBindingList.Add(pb);
        }

        new_playableDirector.playableAsset = timelineAsset;

        for (int i = 0; i < oldBindingList.Count; i++)
        {
            new_playableDirector.SetGenericBinding(newBindingList[i].sourceObject, tempPlaybleDirector.GetGenericBinding(oldBindingList[i].sourceObject));
        }

        tempPlaybleDirector.playableAsset = null;
    }

    #endregion HELPER FUNCTION
}