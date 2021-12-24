using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateByAngle : MonoBehaviour
{
    [Header("是否顺时针旋转")]
    public bool isClockwise;
    [Header("卡牌旋转角度低于此值则不触发旋转")]
    public float limit;
    [Header("平面镜旋转范围")]
    public float minAngle, maxAngle;
    private float span;

    private void Start()
    {
        UpdateSpan();
    }

    /// <summary>
    /// 计算角度活动范围大小
    /// </summary>
    public void UpdateSpan()
    {
        span = Mathf.Abs(maxAngle - minAngle);
    }

    /// <summary>
    /// 将卡牌旋转角度转换为平面镜旋转角度
    /// </summary>
    /// <param name="rawAngle"></param>
    /// <returns></returns>
    private float remapAngle(float rawAngle)
    {
        return (rawAngle * span) / 180f;
    }

    /// <summary>
    /// 判断旋转后的平面镜是否会超出角度限制
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    //private bool IsLegalAngle(float angle)
    //{
    //    Vector3 newDir;
    //    if (isClockwise)
    //    {
    //         newDir= Quaternion.AngleAxis(angle, Vector3.forward) * transform.up;

    //    }
    //    else
    //    {
    //        newDir = Quaternion.AngleAxis(-angle, Vector3.forward) * transform.up;
    //    }
    //    float difference = Vector3.SignedAngle(transform.parent.up, newDir, Vector3.forward);
    //    Debug.Log(gameObject.name + "的angleDifference是" + difference);
    //    if(minAngle<= difference && difference<= maxAngle)
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}

    private bool IsLegalAngle(float angle)
    {
        return angle > 0 && angle < 180;

    }

    /// <summary>
    /// 按角度旋转平面镜
    /// </summary>
    /// <param name="rawAngle"></param>
    //public void Rotate(float rawAngle)
    //{
    //    if (Mathf.Abs(rawAngle) >= limit)
    //    {
    //        float angle = remapAngle(rawAngle);
    //        if (IsLegalAngle(angle))
    //        {
    //            if (isClockwise)
    //            {
    //                transform.Rotate(Vector3.forward * angle, Space.World);
    //            }
    //            else
    //            {
    //                transform.Rotate(Vector3.forward * -angle, Space.World);
    //            }
    //        }

    //    }
    //}

    public void Rotate(float rawAngle)
    {

      
        if (IsLegalAngle(rawAngle))
        {
            float angle = remapAngle(rawAngle);
            //if (isClockwise)
            //{
            //    transform.localEulerAngles = new Vector3(angle, transform.localEulerAngles.y, transform.localEulerAngles.z);
            //}
            //else
            //{
            //    transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, -angle);

            //}
            transform.localEulerAngles = new Vector3(angle, transform.localEulerAngles.y, transform.localEulerAngles.z);
        }


    }

}
