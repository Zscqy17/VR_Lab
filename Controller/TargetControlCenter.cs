using ProcessData;
using UnityEngine;
using Universal.Card;

/**********************************************
* 模块名: TargetControlCenter.cs
* 功能描述：用于管理模型的移动改变以及模型的替换
***********************************************/

public class TargetControlCenter : MonoBehaviour
{
    public static TargetControlCenter Instance;

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// 模型替换，被StateController调用
    /// </summary>
    /// <param name="currentIndex">当前模型在数组中的位置下标</param>
    /// <param name="alterTargetObj">用于替换新模型，上面必须绑定ModelControl</param>
    public void SwitchTarget(int targetIndex, GameObject alterTargetObj)
    {
        if (alterTargetObj == null)
        {
            CourseControl.instance.objs[targetIndex].model = null;
            return;
        }

        if (targetIndex < CourseControl.instance.objs.Count && targetIndex >= 0)
        {
            CourseControl.instance.objs[targetIndex].model = alterTargetObj;
        }
    }

    /// <summary>
    /// 改变物体的移动方式，被StateControll和EventController调用
    /// </summary>
    /// <param name="moveArea">利用moveArea结构体进行通信</param>
    public void ManageModelMovement(MoveArea moveArea)
    {
        if (CourseControl.instance.objs[moveArea.index].model == null)
        {
            Debug.LogError("ControlCenter中下标为 " + moveArea.index.ToString() + "的model为null！");
            return;
        }
        var model_control = CourseControl.instance.objs[moveArea.index].model.GetComponent<ModelControl>();
        if (model_control == null)
        {
            Debug.LogError("未获取到ModelControl组件，请检查movearea的index");
            return;
        }

        model_control.updatePos = moveArea.updatePos;
        Debug.Log("Set " + CourseControl.instance.objs[moveArea.index].model.ToString() + "'s update pos to " + moveArea.updatePos);
        model_control.updateAngle = moveArea.updateAngle;

        if (moveArea.changeMoveSpace)
        {
            Debug.Log("change changeMoveSpace");
            model_control.moveSpace = moveArea.moveSpace;
        }
        if (moveArea.changeArea)
        {
            model_control.minX = moveArea.minX;
            model_control.minY = moveArea.minY;
            model_control.minZ = moveArea.minZ;
            model_control.maxX = moveArea.maxX;
            model_control.maxY = moveArea.maxY;
            model_control.maxZ = moveArea.maxZ;
        }
        if (moveArea.changeRatio)
        {
            model_control.ratio = moveArea.ratio;
        }
        model_control.stayIn = !moveArea.removable;//change to match SDK;
    }
}