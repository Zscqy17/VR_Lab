using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 更新倒计时UI文字的代码
/// </summary>
///
public class CoundDownUI : MonoBehaviour
{
    [Header("倒计时开始时间")]
    public int countTime;

    //组件倒计时UI下的Text组件
    private Text text_component;

    AudioSource audioSource;

    [Header("是否播放倒计时音频")]
    public bool playaudio;

    public AudioClip countdown_clip;

    private void Awake()
    {
        text_component = GetComponentInChildren<Text>();
        audioSource = GetComponentInChildren<AudioSource>();
    }

    private void Start()
    {
        text_component.text = countTime.ToString();
    }

    private void OnEnable()
    {
        StartCoroutine(UpdateText());
    }

    /// <summary>
    /// 使用携程更新，每秒一次
    /// </summary>
    /// <returns></returns>
    private IEnumerator UpdateText()
    {
        for (var i = countTime; i >= 0; --i)
        {
            if(playaudio)
                audioSource.PlayOneShot(countdown_clip);
            text_component.text = i.ToString();
            yield return new WaitForSeconds(1.0f);
        }
        yield return null;
    }
}