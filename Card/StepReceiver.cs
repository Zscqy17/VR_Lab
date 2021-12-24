using UnityEngine;
using Universal.Audio;
using Universal.Card;

public class StepReceiver : CardOptionControl
{
    [Header("请指定步骤卡牌")]
    public StageControlCardOption stageControlCard;

    private UISceneStep step;
    private DeserializeTool deserializeTool;

    public GameObject options;
    public AnswerCardLR answerLR;

    private void Start()
    {
        step = UIManager.Instance.GetUI<UISceneStep>(UIName.UISceneStep);
        deserializeTool = FindObjectOfType<DeserializeTool>();
    }


    protected override void CardRightEvent(string name)
    {
        if (!string.IsNullOrEmpty(name) && name.Equals("StageControl"))
        {
            if (step.gameObject.activeSelf)
            {
                int stateIndex = stageControlCard.GetStepNum();
                if (stateIndex == -1) return;
                OnChangeState();
                StateController.Instance.LoadState(stateIndex);
                deserializeTool.RestoreState(stateIndex);
                options.SetActive(false);
                stageControlCard.right.SetActive(false);
                stageControlCard.SetAllCardOptionParent(false);
                //stageControlCard.handlLastState = stageControlCard.handC.startOperation;
                //stageControlCard.handC.startOperation = false;
                answerLR.ResetRightRate();
            }
        }
        
    }

    private void OnChangeState()
    {
        KillCurrentPlayingTimelines();
        StopHints();
    }

    /// <summary>
    /// 删除所有目前播放的PlayableDirectors
    /// </summary>
    private void KillCurrentPlayingTimelines()
    {
        var timelinePlayers = FindObjectsOfType<UnityEngine.Playables.PlayableDirector>();
        foreach (var player in timelinePlayers)
            if (player.gameObject.name != "Timeline")
                Destroy(player.gameObject);
    }

    /// <summary>
    ///
    /// </summary>
    private void StopHints()
    {
        if (AudioPlayer.Instance != null)
            AudioPlayer.Instance.StopAudio(Universal.Audio.AudioType.speech);

        if (TimelineUIDisplayer.Instance != null)
        {
            TimelineUIDisplayer.Instance.HideCurrentHint();
            TimelineUIDisplayer.Instance.HideOperationHint();
            TimelineUIDisplayer.Instance.HideAudioHint();
        }
    }
}