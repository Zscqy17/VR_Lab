using OneFlyLib;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Events;

namespace ProcessData
{
    /// <summary>
    /// 下屏选项修改的数据
    /// </summary>
    [System.Serializable]
    public struct ChangingOptions
    {
        [Tooltip("更换的sprite")]
        [XmlIgnore] public Sprite sprite;

        [Tooltip("选项物体的名称")]
        public string optionName;

        public ChangingOptions(Sprite _sprite, string _optionName)
        {
            sprite = _sprite;
            optionName = _optionName;
        }
    }

    /// <summary>
    /// 物体移动范围的数据
    /// </summary>
    [System.Serializable]
    public struct MoveArea
    {
        [Tooltip("是否更新物体位置")]
        public bool updatePos;

        [Tooltip("是否更新物体旋转角度")]
        public bool updateAngle;

        [Tooltip("模型在TargetControlCenter链表中的下标")]
        public int index;

        [Tooltip("是否改变移动范围")]
        public bool changeArea;

        public float minX, maxX;
        public float minY, maxY;
        public float minZ, maxZ;

        [Tooltip("是否改变移动轴向")]
        public bool changeMoveSpace;

        public MOVE_SPACE moveSpace;

        [Tooltip("是否改变移动比例")]
        public bool changeRatio;

        [Tooltip("物体随卡牌移动的比例")]
        public float ratio;

        [Tooltip("物体是否会随着卡牌取走而消失")]
        public bool removable;

        public MoveArea(Vector3 minVec, Vector3 maxVec)
        {
            updatePos = false;
            updateAngle = false;
            index = -1;
            changeArea = false;
            minX = minVec.x; minY = minVec.y; minZ = minVec.z;
            maxX = maxVec.x; maxY = maxVec.y; maxZ = maxVec.z;
            changeMoveSpace = false;
            moveSpace = MOVE_SPACE.NOMOVE;
            changeRatio = false;
            ratio = 0;
            removable = false;
        }
    }

    /// <summary>
    /// 语音提示的数据
    /// </summary>
    [System.Serializable]
    public struct AudioHintContent
    {
        [Tooltip("右上角音频提示显示的文字内容（为空则只播放语言不显示文字）")]
        public string audioHintText;

        [Tooltip("音频文件")]
        [XmlIgnore] public AudioClip audioHintClip;
    }

    /// <summary>
    /// 错误信息的数据
    /// </summary>
    [System.Serializable]
    public struct ErrorInfo
    {
        [Tooltip("错误在config文件中的序号")]
        public int errorIndex;

        [Tooltip("错误UI出现后停留的时间")]
        public float hideHintTime;

        [Tooltip("错误UI上显示的内容")]
        public string errorContent;
    }

    /// <summary>
    /// 目标模型切换的数据
    /// </summary>
    [System.Serializable]
    public struct TargetSwap
    {
        [Tooltip("模型在TargetControlCenter表中的下标")]
        public int targetIndex;

        [Tooltip("替换新的模型的可区分父物体名，非必填")]
        public string parentName;

        [Tooltip("用于替换的新模型的物体名")]
        public string newTargetName;

        public TargetSwap(int _targetIndex, string _parentName, string _newTargetName)
        {
            targetIndex = _targetIndex;
            parentName = _parentName;
            newTargetName = _newTargetName;
        }
    }

    /// <summary>
    /// 显示指定下一步骤的数据
    /// </summary>
    [System.Serializable]
    public struct NextStateInfo
    {
        [Tooltip("手动指定下一步骤（默认读取下一步骤）")]
        public bool loadAnotherState;

        [Tooltip("下一步骤的编号（StateController中StateAsset的下标）")]
        public int index;
    }

    /// <summary>
    /// 触发时机
    /// </summary>
    public enum TriggerTiming
    {
        BeforeTimeline,    //Timeline之前
        InTimelineFrame,
        AfterTimeline       //Timeline之后
    }

    /// <summary>
    ///
    /// </summary>
    public enum StateTiming
    {
        OnEnterState,
        OnExitState
    }

    [System.Serializable]
    public struct EventObjectString
    {
        public string name;
        public TriggerTiming triggerTiming;

        public EventObjectString(string _name,TriggerTiming _timimg)
        {
            name = _name;
            triggerTiming = _timimg;
        }
    }

    [System.Serializable]
    public struct Movement
    {
        [Tooltip("移动固定的距离")]
        public float triggerDistance;

        [Tooltip("对应的步骤下标")]
        public int stateIndex;

        [Tooltip("目标位置")]
        public GameObject targetGameObject;

        [Tooltip("触发的Event效果")]
        public EventAsset eventAsset;

        [Tooltip("当目标物体激活时才触发")]
        public bool triggerWhileTargetActive;

        [Tooltip("触发后不播放音效")]
        public bool mute;

        [Tooltip("自动配置移动范围(适用于单个步骤单个移动)")]
        public bool autoRange;
    }
    
    [System.Serializable]
    public struct BasicInteraction
    {
        [Tooltip("对应的步骤下标")]
        public int stateIndex;

        [Tooltip("触发的Event效果")]
        public EventAsset eventAsset;
    }

    /// <summary>
    /// StateAsset和对应步骤的UnityEvent
    /// </summary>
    [System.Serializable]
    public struct StateSequence
    {
        [Tooltip("StateAsset")]
        public StateAsset stateAsset;

        [Tooltip("进入步骤触发的函数")]
        public UnityEvent enterFunction;

        [Tooltip("离开步骤触发的步骤")]
        public UnityEvent exitFunction;
    }

    [System.Serializable]
    public struct CardOptionEvent
    {
        [Header("卡牌的名字")]
        public string cardName;

        [Header("左转触发的事件")]
        public EventAsset[] leftEvents;

        [Header("右转触发的事件")]
        public EventAsset[] rightEvents;
    }
}

public static class ExternalMehods
{
    public static Transform FindChildRecursively(this Transform trans, string goName)
    {
        Transform child = trans.Find(goName);
        if (child != null)
            return child;

        Transform go = null;
        for (int i = 0; i < trans.childCount; i++)
        {
            child = trans.GetChild(i);
            go = FindChildRecursively(child, goName);
            if (go != null)
                return go;
        }
        return null;
    }
}