using UnityEngine;
using UnityEngine.UI;
using Universal.Audio;

/// <summary>
/// 选择卡牌下屏UI上用来激活选项，需要为选项及卡牌UI绑定Trigger的Collider
/// </summary>
public class ChooseTrigger : MonoBehaviour
{
    [HideInInspector]
    public int optionIndex = -1; //存储卡牌目前选择的选项编号

    [SerializeField]
    [Header("选项被选中时高亮图片")]
    private Sprite[] highLightSprite;

    [SerializeField]
    [Header("选项普通图片")]
    private Sprite[] normalSprite;

    [Header("激活的“确认”选项物体")]
    public GameObject OptionGameObject;

    [Header("选项被选中高亮时播放的音效的下标")]
    public int enterSoundIndex;

    [Header("是否使用图片自适应")]
    public bool setNativeSize;

    [HideInInspector]
    public Collider optionCollider;

    /// <summary>
    /// 选择卡牌UI进入选项后调用的函数,置选项高亮并播放音效
    /// </summary>
    public void OptionEnter(Collider optionCollider)
    {
        AudioPlayer.Instance.PlayAudio(Universal.Audio.AudioType.effect, enterSoundIndex);//选项高亮时播放音效

        OptionGameObject.SetActive(true);

        var index = optionCollider.gameObject.tag[0] - 'A' + 1;

        optionIndex = index;
        OptionHightlight(optionCollider, index - 1);
    }

    /// <summary>
    /// 选择卡牌离开选项后调用的函数，置选项回原形
    /// </summary>
    /// <param name="colliderTemp"></param>
    public void OptionExit()
    {
        OptionGameObject.SetActive(false);

        if (optionCollider)
        {
            var index = optionCollider.gameObject.tag[0] - 'A' + 1;

            var imgComponent = optionCollider.GetComponentInParent<Image>();
            imgComponent.sprite = normalSprite[index - 1];
            if (setNativeSize)
                imgComponent.SetNativeSize();
            optionCollider = null;
        }
        else
            Debug.Log("no option collider");
    }

    public void OptionHightlight(Collider colliderTemp, int index)
    {
        optionCollider = colliderTemp;
        var imageComponent = colliderTemp.transform.parent.gameObject.GetComponent<Image>();
        if (imageComponent)
        {
            imageComponent.sprite = highLightSprite[index];
            imageComponent.SetNativeSize();
        }
    }

    private void OnDisable()
    {
        OptionExit();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (TimelineManager.Instance.playableDirector.state == UnityEngine.Playables.PlayState.Playing)
            return;
        OptionEnter(collider);
    }

    private void OnTriggerExit(Collider collider)
    {
        OptionExit();
    }
}