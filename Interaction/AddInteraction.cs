using ProcessData;
using UnityEngine;
using Universal.Audio;

/**********************************************
* 模块名: AddInteraction.cs
* 功能描述：放入交互事件的触发
***********************************************/

public class AddInteraction : InteractEvent
{
    [SerializeField]
    [Tooltip("交互信息")]
    private BasicInteraction[] basicInteractions;

    private BasicInteraction currentBasicInteraction;

    [SerializeField]
    [Tooltip("当物体激活时播放音效")]
    private bool playAudioOnEnable;

    private void OnEnable()
    {
        if (StateController.Instance == null || !CheckStateIndex())
        {
            return;
        }

        if (playAudioOnEnable)
            AudioPlayer.Instance.PlayAudio(Universal.Audio.AudioType.effect, 2);             //这里需要改成模型添加的音效，添加到场景的AudioPlayer中

        if (!ifFinished)
        {
            if (eventController != null && StateController.Instance.interactEvents.Contains(this))
            {
                eventController.EventTrigger(currentBasicInteraction.eventAsset);
            }
            ifFinished = true;
        }
    }

    override public void ResetInteractEvent()
    {
        foreach (BasicInteraction bi in basicInteractions)
        {
            if (bi.stateIndex == StateController.Instance.stateIndex)
            {
                currentBasicInteraction = bi;
                stateIndex = bi.stateIndex;
                ifFinished = false;
                return;
            }
        }

        ifFinished = true;
        stateIndex = -1;
    }
}