using ProcessData;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Timeline;

/**********************************************
* 模块名: StateAsset.cs
* 功能描述：步骤信息配置文件类
***********************************************/

[CreateAssetMenu(menuName = "StateAsset", fileName = "stateAsset", order = 2)]

public class StateAsset : ScriptableObject
{
    [Header("状态名称")]
    public string stateName;

    [SerializeField]
    [Header("进入步骤后改变的模型移动方式")]
    public MoveArea[] moveArea;

    [SerializeField]
    [Header("进入步骤改变的UI选项及图片")]
    public ChangingOptions[] optionSprite;

    public string[] sprite_Paths;

    [SerializeField]
    [Header("进入步骤显示的右上角音频提示文字及音频")]
    public AudioHintContent audioHintContent;

    public string audioHintClip_ID_Path;

    [Header("进入步骤后显示的左上角操作提示")]
    public string operationHintText;

    [SerializeField]
    [Header("进入步骤替换新的模型")]
    public TargetSwap[] targetswap;

    [Header("步骤激活的物体名称")]
    public EventObjectString[] gameObjects_Active;

    [Header("步骤隐藏的物体名称")]
    public EventObjectString[] gameObjects_Hide;

    [Header("进入新步骤激活的卡牌编号")]
    public int[] activeTargets_Enter;

    [Header("进入新步骤禁用的卡牌编号")]
    public int[] disableTargets_Enter;

    [Header("结束步骤激活的卡牌编号")]
    public int[] activeTargets_Exit;

    [Header("结束步骤禁用的卡牌编号")]
    public int[] disableTargets_Exit;

    [Header("进入步骤后播放的Timeline（可无，播放完后才会进行进入新步骤初始化）")]
    [XmlIgnore] public TimelineAsset enterStateTimeline;

    public string enterStateTimeline_Path;

    [Header("结束步骤后播放的Timeline（可无，播放完后才会进入下一步骤）")]
    [XmlIgnore] public TimelineAsset exitStateTimeline;

    public string exitStateTimeline_Path;

    [Header("最后一个事件触发自动隐藏语音和操作提示")]
    public bool autoHideHint;

    [Space(10)]
    [Header("步骤目标完成情况")]
    public bool[] effects;

    [Header("课程进度，离开步骤后自动设置，若为负数则不会更新进度")]
    public float progress = -1;
}