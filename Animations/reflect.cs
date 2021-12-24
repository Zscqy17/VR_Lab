using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class reflect : MonoBehaviour
{

    [Header("最大距离")] public float maxDistance = 50;

    [Header("最多反射次数")] public int maxReflectTimes = 3;

    public bool renderLine;

    //public GameObject pointLight1,pointLight2;
    /// <summary>
    /// 渲染射线
    /// </summary>
    private LineRenderer _lineRender;

    /// <summary>
    /// 射到的点的集合，用来渲染射线
    /// </summary>
    private List<Vector3> _renderPoints;

    private void Awake()
    {
        _lineRender = GetComponent<LineRenderer>();
    }

    public void ToggleRenderLine(bool state)
    {
        renderLine = state;
    }


    private void Update()
    {
        if (renderLine)
        {
            _lineRender.enabled = true;
            _renderPoints = new List<Vector3>();
            _renderPoints.Add(transform.position); //LineRenderer以自己为起点

            _renderPoints.AddRange(GetRenderPoints(transform.position, transform.forward,
                maxDistance, maxReflectTimes));//获取反射点

            _lineRender.positionCount = _renderPoints.Count;
            _lineRender.SetPositions(_renderPoints.ToArray());

            //pointLight1.SetActive(true);
            //pointLight2.SetActive(true);
            //pointLight1.transform.position = _renderPoints[1] + Vector3.right * -0.05f;
            //pointLight2.transform.position = _renderPoints[2] + Vector3.right * 0.1f;

        }
        else
        {
            _lineRender.enabled = false;
            //pointLight1.SetActive(false);
            //pointLight2.SetActive(false);
        }
      
    }

    /// <summary>
    /// 获得反射点
    /// </summary>
    /// <param name="start">起始位置</param>
    /// <param name="dir">方向</param>
    /// <param name="dis">最大距离</param>
    /// <param name="times">反射次数</param>
    private List<Vector3> GetRenderPoints(Vector3 start, Vector3 dir, float dis, int times)
    {
        var hitPosList = new List<Vector3>();
        while (dis > 0 && times > 0)
        {
            RaycastHit hit;
            if (!Physics.Raycast(start, dir, out hit, dis))
                break;
            hitPosList.Add(hit.point);
            var reflectDir = Vector3.Reflect(dir, hit.normal);
            dis -= (hit.point - start).magnitude;
            times--;
            start = hit.point;
            dir = reflectDir;
        }
        return hitPosList;


    }
}
