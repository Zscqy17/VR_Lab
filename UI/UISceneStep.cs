using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**********************************************
* Copyright (C) 2019 讯飞幻境（北京）科技有限公司
* 模块名: UISceneStep.cs
* 创建者：RyuRae
* 修改者列表：
* 创建日期：
* 功能描述：
***********************************************/

public class UISceneStep : UIScene
{
    public StageControlCardOption stageControlCardOption;

    public List<ItemStep> items = new List<ItemStep>();

    protected override void Start()
    {
        base.Start();
        LoadItems(0);
    }

    /// <summary>
    /// 根据装载指定实验的跳步选项
    /// </summary>
    /// <param name="operationIndex"></param>
    public void LoadItems(int operationIndex)
    {
        items.Clear();
        Debug.Log("LoadItems" + operationIndex.ToString());
        for (int i = 0; i < transform.childCount; ++i)
        {
            var itemComponent = transform.GetChild(i).GetComponent<ItemStep>();

            if (itemComponent.operationIndex == operationIndex)
            {
                items.Add(itemComponent);
            }
        }
    }

    public void SetTween(Vector3 vec)
    {
        if (!gameObject.activeSelf) return;
        StopCoroutine(WaitFor(0.1f));
        Vector3 pos = transform.InverseTransformPoint(vec);
        items.ForEach(p => p.SetTween(pos));
    }

    private Transform tempTransform = null;

    public int GetCurrSelected(Vector3 pos)
    {
        var flag = false;
        int stateIndex = -1;
        if (!gameObject.activeSelf) return -1;
        for (int i = 0; i < items.Count; i++)
        {
            if (Vector3.Distance(pos, items[i].transform.position) < 50)
            {
                items[i].selected.SetActive(true);
                //string temp = items[i].name.Split('_')[1];
                stateIndex = items[i].GetComponent<ItemStep>().stateIndex;

                if (!tempTransform || !tempTransform.Equals(items[i].transform))
                    stageControlCardOption.right.SetActive(true);
                tempTransform = items[i].transform;
                flag = true;
            }
            else
                items[i].selected.SetActive(false);
        }
        if(!flag)
        {
            tempTransform = null;
            stageControlCardOption.right.SetActive(false);
        }
        return stateIndex;
    }

    public void SetTweenBack(Vector3 vec)
    {
        if (!gameObject.activeSelf) return;
        Vector3 pos = transform.InverseTransformPoint(vec);
        items.ForEach(p => p.SetTweenBack(pos));
        if (gameObject.activeInHierarchy)
            StartCoroutine(WaitFor(0.1f));
    }

    private IEnumerator WaitFor(float time)
    {
        yield return new WaitForSeconds(time);
        SetVisible(false);
    }

    public override void SetVisible(bool visible)
    {
        //base.SetVisible(visible);
        if (!visible)
            items.ForEach(p => p.Reset());
        gameObject.SetActive(visible);
    }
}