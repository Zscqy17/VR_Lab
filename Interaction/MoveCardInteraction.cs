using OneFlyLib;
using ProcessData;
using System.Collections.Generic;
using UnityEngine;
using Universal.Audio;

/**********************************************
* 模块名: MoveCardInteraction.cs
* 功能描述：移动交互事件的触发
***********************************************/

public class MoveCardInteraction : InteractEvent
{
    [SerializeField]
    private Movement[] moveInteractionGroups;

    [Header("是否打印距离")]
    [SerializeField]
    private bool printDistance;

    // 存储当前步骤所有移动交互的列表
    private List<Movement> currentMovement = new List<Movement>();

    /// <summary>
    /// 在Update中检查距离是否达到阈值内，达到后触发Event事件
    /// </summary>
    private void Update()
    {
        if (currentMovement.Count > 0)
            CheckDistance();
    }

    /// <summary>
    /// 进入步骤后从array里寻找当前步骤的移动交互，并添加到list中
    /// </summary>
    public override void ResetInteractEvent()
    {
        currentMovement.Clear();

        foreach (Movement movement in moveInteractionGroups)
        {
            if (movement.stateIndex == StateController.Instance.stateIndex)
            {
                if (movement.targetGameObject == null)
                    continue;
                currentMovement.Add(movement);
                stateIndex = movement.stateIndex;
            }
        }
    }

    /// <summary>
    /// 检查移动交互触发距离
    /// </summary>
    private void CheckDistance()
    {
        int index = 0;
        foreach (var movement in currentMovement)
        {
            // 计算物体和目标点之间世界坐标下距离
            var distance = Vector3.Distance(transform.position, movement.targetGameObject.transform.position) * 100;

            // 打印距离
            if (printDistance)
                Debug.Log("distance from " + gameObject.name + " to " + movement.targetGameObject.name +
                    " is " + distance.ToString());

            // 距离小于阈值后还要检查是否设置了目标物体激活才触发交互
            if (distance < movement.triggerDistance &&
                (!movement.triggerWhileTargetActive || movement.targetGameObject.activeInHierarchy))
            {
                MoveEventTrigger(index);
                return;
            }

            index++;
        }
    }

    /// <summary>
    /// 触发指定的移动事件, 会将完成的移动交互从List里删除
    /// </summary>
    /// <param name="index">移动事件在list中的下标</param>
    private void MoveEventTrigger(int index)
    {
        var MoveInteraction = currentMovement[index];

        currentMovement.RemoveAt(index);
        EventController.Instance.EventTrigger(MoveInteraction.eventAsset);
        SetTransform(MoveInteraction.targetGameObject.transform);
        if (!MoveInteraction.mute)
            PlayAudio();
    }

    /// <summary>
    /// 将物体的物体的Transform对齐目标的Transform
    /// </summary>
    private void SetTransform(Transform targetTransform)
    {
        gameObject.transform.position = targetTransform.position;
        gameObject.transform.rotation = targetTransform.rotation;
    }

    /// <summary>
    /// 播放移动交互音频
    /// </summary>
    private void PlayAudio()
    {
        AudioPlayer.Instance.PlayAudio(Universal.Audio.AudioType.effect, 1);
    }

    /// <summary>
    /// 进入步骤播放完开始Timeline后调用
    /// </summary>
    public override void OnInteractionBegin()
    {
        if (currentMovement.Count > 0)
            AutoSetRange();
    }

    /// <summary>
    /// 自动设置绑定物体的ModelControl的移动范围和是否随着卡牌移动
    /// 会在播放完进入步骤开场动画后调用，如果设置了Movement为autorange则会自动计算移动的范围
    /// 并设置updatePos为true
    /// </summary>
    /// <param name="topLeft"></param>
    /// <param name="bottomRight"></param>
    private void AutoSetRange()
    {
        var modelControl = gameObject.GetComponent<Universal.Card.ModelControl>();

        if (!modelControl)
        {
            Debug.LogError("MoveCardInteraction 上未绑定ModelControl物体");
            return;
        }

        Vector3 min = Vector3.positiveInfinity;
        Vector3 max = Vector3.negativeInfinity;
        var updatePosFlag = false;

        switch (modelControl.moveSpace)
        {
            case MOVE_SPACE.X:
                foreach (var item in currentMovement)
                {
                    if (item.autoRange)
                    {
                        min.x = Mathf.Min(min.x, item.targetGameObject.transform.position.x, transform.position.x);
                        max.x = Mathf.Max(max.x, item.targetGameObject.transform.position.x, transform.position.x);
                        updatePosFlag = true;
                    }
                }
                // 若数值仍为infinity则保持原来ModelControl数值
                modelControl.minX = (modelControl.minX == float.NegativeInfinity ? modelControl.minX : min.x);
                modelControl.maxX = (modelControl.maxX == float.PositiveInfinity ? modelControl.maxX : max.x);
                break;

            case MOVE_SPACE.Y:
                foreach (var item in currentMovement)
                {
                    if (item.autoRange)
                    {
                        min.y = Mathf.Min(min.y, item.targetGameObject.transform.position.y, transform.position.y);
                        max.y = Mathf.Max(max.y, item.targetGameObject.transform.position.y, transform.position.y);
                        updatePosFlag = true;
                    }
                }
                modelControl.minY = (modelControl.minY == float.NegativeInfinity ? modelControl.minY : min.y);
                modelControl.maxY = (modelControl.maxY == float.PositiveInfinity ? modelControl.maxY : max.y);
                break;

            case MOVE_SPACE.Z:
                foreach (var item in currentMovement)
                {
                    if (item.autoRange)
                    {
                        min.z = Mathf.Min(min.z, item.targetGameObject.transform.position.z, transform.position.z);
                        max.z = Mathf.Max(max.z, item.targetGameObject.transform.position.z, transform.position.z);
                        updatePosFlag = true;
                    }
                }
                modelControl.minZ = (modelControl.minZ == float.NegativeInfinity ? modelControl.minZ : min.z);
                modelControl.maxZ = (modelControl.maxZ == float.PositiveInfinity ? modelControl.maxZ : max.z);
                break;

            case MOVE_SPACE.XY:
                foreach (var item in currentMovement)
                {
                    if (item.autoRange)
                    {
                        min.x = Mathf.Min(min.x, item.targetGameObject.transform.position.x, transform.position.x);
                        max.x = Mathf.Max(max.x, item.targetGameObject.transform.position.x, transform.position.x);
                        min.y = Mathf.Min(min.y, item.targetGameObject.transform.position.y, transform.position.y);
                        max.y = Mathf.Max(max.y, item.targetGameObject.transform.position.y, transform.position.y);
                        updatePosFlag = true;
                    }
                }
                modelControl.minX = (modelControl.minX == float.PositiveInfinity ? modelControl.minX : min.x);
                modelControl.maxX = (modelControl.maxX == float.NegativeInfinity ? modelControl.maxX : max.x);
                modelControl.minY = (modelControl.minY == float.PositiveInfinity ? modelControl.minY : min.y);
                modelControl.maxY = (modelControl.maxY == float.NegativeInfinity ? modelControl.maxY : max.y);
                break;

            case MOVE_SPACE.XZ:
                foreach (var item in currentMovement)
                {
                    if (item.autoRange)
                    {
                        min.x = Mathf.Min(min.x, item.targetGameObject.transform.position.x, transform.position.x);
                        max.x = Mathf.Max(max.x, item.targetGameObject.transform.position.x, transform.position.x);
                        min.z = Mathf.Min(min.z, item.targetGameObject.transform.position.z, transform.position.z);
                        max.z = Mathf.Max(max.z, item.targetGameObject.transform.position.z, transform.position.z);
                        updatePosFlag = true;
                    }
                }
                modelControl.minX = (modelControl.minX == float.PositiveInfinity ? modelControl.minX : min.x);
                modelControl.maxX = (modelControl.maxX == float.NegativeInfinity ? modelControl.maxX : max.x);
                modelControl.minZ = (modelControl.minZ == float.PositiveInfinity ? modelControl.minZ : min.z);
                modelControl.maxZ = (modelControl.maxZ == float.NegativeInfinity ? modelControl.maxZ : max.z);
                break;

            case MOVE_SPACE.YZ:
                foreach (var item in currentMovement)
                {
                    if (item.autoRange)
                    {
                        min.y = Mathf.Min(min.y, item.targetGameObject.transform.position.y, transform.position.y);
                        max.y = Mathf.Max(max.y, item.targetGameObject.transform.position.y, transform.position.y);
                        min.z = Mathf.Min(min.z, item.targetGameObject.transform.position.z, transform.position.z);
                        max.z = Mathf.Max(max.z, item.targetGameObject.transform.position.z, transform.position.z);
                        updatePosFlag = true;
                    }
                }
                modelControl.minY = (modelControl.minY == float.PositiveInfinity ? modelControl.minY : min.y);
                modelControl.maxY = (modelControl.maxY == float.NegativeInfinity ? modelControl.maxY : max.y);
                modelControl.minZ = (modelControl.minZ == float.PositiveInfinity ? modelControl.minZ : min.z);
                modelControl.maxZ = (modelControl.maxZ == float.NegativeInfinity ? modelControl.maxZ : max.z);
                break;

            default:
                break;
        }
        if (updatePosFlag)
            modelControl.updatePos = true;
    }
}