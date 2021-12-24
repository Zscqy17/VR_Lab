using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScale : MonoBehaviour
{
    //public float maxXScale, minXScale;
    public float maxYScale, minYScale;
    public float limit;

    //public void ModifyScale(float value)
    //{
    //    if (Mathf.Abs(value) >= limit)
    //    {
    //        float yIcrement = (value / 180f) * (maxYScale-minYScale);
    //        if (isLegal(yIcrement))
    //        {
    //            transform.localScale += new Vector3(0,yIcrement,0);
    //        }

    //    }
    //}


    public void ModifyScale(float value)
    {
       
        if (isLegal(value))
        {
            float yScale = minYScale + (value / 180f) * (maxYScale - minYScale);
            transform.localScale = new Vector3(transform.localScale.x, yScale, transform.localScale.z);
        }
    }


    private bool isLegal(float y)
    {
        return y > 0 && y < 180;
    }
}
