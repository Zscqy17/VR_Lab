using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Audio;
using UnityEngine.UI;
using ProcessData;


namespace ProcessData
{
    [System.Serializable]
    public struct LineInfo
    {
        public string optionName;
        public string otherName;
        public EventAsset correctEvent;
        public EventAsset incorrectEvent;
    }
}
/// <summary>
/// 连线交互
/// </summary>
public class DrawLineInteraction : MonoBehaviour {

    public int drawLineStateIndex;

    public LineInfo[] lineInfos;

    [Header("选项的tag标签名")]
    public string optionTagName;

    [Header("选择首项后对于项的tag标签名")]
    public string otherTagName;


    [HideInInspector]
    public bool selecting; //是否已经选择首项

    [HideInInspector]
    public int optionIndex = -1; //存储卡牌目前选择的选项编号


    [SerializeField]
    [Header("选项被选中时高亮颜色")]
    Color highLightColor;

    [Header("激活的“确认”选项物体")]
    public GameObject OptionGameObject;

    [Header("选项被选中高亮时播放的音效的下标")]
    public int enterSoundIndex;

    Collider lastCollider; //存储最后碰到选项的collider
    string optionSelectedName; //

    public Image[] drawOptionImage;


    /// <summary>
    /// 选择卡牌UI进入选项后调用的函数,置选项高亮并播放音效
    /// </summary>
    public void OptionEnter(Collider optionCollider)
    {
        AudioPlayer.Instance.PlayAudio(Universal.Audio.AudioType.effect, enterSoundIndex);//选项高亮时播放音效

        OptionGameObject.SetActive(true);

        var imageComponent = optionCollider.gameObject.transform.parent.GetComponent<Image>();
        if (imageComponent)
            imageComponent.color = highLightColor;

        switch (optionCollider.gameObject.transform.parent.name)
        {
            case "Option_0.5": { optionIndex = 1; optionSelectedName = optionCollider.gameObject.transform.parent.name; } break;
            case "Option_2.4": { optionIndex = 2; optionSelectedName = optionCollider.gameObject.transform.parent.name; } break;
            case "Option_9.1": { optionIndex = 3; optionSelectedName = optionCollider.gameObject.transform.parent.name; } break;
            default: return;
        }
    }

    /// <summary>
    /// 选择卡牌离开选项后调用的函数，置选项回原形
    /// </summary>
    /// <param name="colliderTemp"></param>
    public void OptionExit()
    {
        if (!selecting)
        {
            OptionGameObject.SetActive(false);
            CancelHightLight();
            optionIndex = -1;
        }
    }

    void OnDisable()
    {
        /*
        if (drawLineStateIndex != StateController.Instance.stateIndex || TimelineManager.Instance.playableDirector.state == UnityEngine.Playables.PlayState.Playing)
            return;
            */
        if (!selecting)
        {
                OptionExit();
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        /*
        if (drawLineStateIndex != StateController.Instance.stateIndex||TimelineManager.Instance.playableDirector.state==UnityEngine.Playables.PlayState.Playing)
            return;
            */
        if (collider.tag == optionTagName && !selecting)
        {
            OptionEnter(collider);
        }
        else if (collider.tag == otherTagName && selecting)
        {
            CheckLine(collider.gameObject.transform.parent.name);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        /*
        if (drawLineStateIndex != StateController.Instance.stateIndex||TimelineManager.Instance.playableDirector.state == UnityEngine.Playables.PlayState.Playing)
            return;
            */
        if (collider.tag == optionTagName)
        {
            if (!selecting)
            {
                OptionExit();
            }
        }
    }

    private void CheckLine(string colliderName)
    {
        foreach (LineInfo li in lineInfos)
        {
            if (colliderName == li.otherName)
            {
                CancelHightLight();
                if (optionSelectedName == li.optionName)
                {
                    selecting = false;
                    EventController.Instance.EventTrigger(li.correctEvent);
                }
                else
                {
                    selecting = false;
                    EventController.Instance.EventTrigger(li.incorrectEvent);
                }
            }
        }
    }

    public void CancelHightLight()
    {
        foreach (Image im in drawOptionImage)
        {
            if(im.gameObject.transform.GetChild(1).gameObject.activeInHierarchy)
            im.color = Color.white;
        }
    }


    public void SelectFirstChoice()
    {
        AudioPlayer.Instance.PlayAudio(Universal.Audio.AudioType.effect, 1);
        selecting = true;
    }
}
