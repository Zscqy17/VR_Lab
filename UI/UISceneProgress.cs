using OneFlyLib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Universal.CheckStage;

/**********************************************
* Copyright (C) 2019 讯飞幻境（北京）科技有限公司
* 模块名: UISceneProgress.cs
* 创建者：RyuRae
* 修改者列表：
* 创建日期：
* 功能描述：
***********************************************/
public class UISceneProgress : UIScene {

    private Image mBack_Image;
    private Text mText_Progress;
    private StageControl stageControl;
	protected override void Start () {
        base.Start();
        mBack_Image = Global.FindChild<Image>(transform, "Back_Image");
        mText_Progress = Global.FindChild<Text>(transform, "Text_Progress");
        stageControl = FindObjectOfType<StageControl>();
    }

    void OnEnable()
    {
        ManagerEvent.Register(StageControl.GoToCurrStage, SetProgress);
    }

    //设置当前试验进度
    public void SetProgress(params object[] args)
    {
        if (args.Length > 0)
        {
            int currStage = (int)args[0];
            if (currStage <= 0) return;
            float progress = (currStage - 1) / (float)stageControl.StageCheckCount;
            if (mBack_Image == null)
                mBack_Image = Global.FindChild<Image>(transform, "Back_Image");
            mBack_Image.fillAmount = progress;
            if (mText_Progress == null)
                mText_Progress = Global.FindChild<Text>(transform, "Text_Progress");
            mText_Progress.text = progress.ToString("0%");
        }
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="progress"></param>
    public void SetProgress(float progress)
    {
        if (mBack_Image == null)
            mBack_Image = Global.FindChild<Image>(transform, "Back_Image");
        mBack_Image.fillAmount = progress;
        if (mText_Progress == null)
            mText_Progress = Global.FindChild<Text>(transform, "Text_Progress");
        mText_Progress.text = progress.ToString("0%");
    }

    void OnDisbale()
    {
        ManagerEvent.Unregister(StageControl.GoToCurrStage, SetProgress);
    }
}
