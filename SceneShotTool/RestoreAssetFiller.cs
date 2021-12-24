using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RestoreAssetFiller : MonoBehaviour
{


    //public void RecordControlCenter()
    //{
    //    if (restoreAsset == null)
    //        return;

    //    restoreAsset.activeCard.Clear();
    //    restoreAsset.objs.Clear();
    //    foreach(var activeCardIndex in courseControl.activeCardSet)
    //    {
    //        restoreAsset.activeCard.Add(activeCardIndex);
    //    }
    //    foreach(var sceneObj in courseControl.objs)
    //    {
    //        var modelName = (sceneObj.model != null ? sceneObj.model.name : "");
    //        var uiName = (sceneObj.ui != null ? sceneObj.ui.name : "");
    //        var newSceneObjName = new SceneObjName(modelName, uiName);
    //        restoreAsset.objs.Add(newSceneObjName);
    //    }

    //    if (StateController.Instance != null)
    //        restoreAsset.stateIndex = StateController.Instance.stateIndex;

    //}

    public static void RecordControlCenter(RestoreAsset restoreAsset)
    {
        if (restoreAsset == null)
        {
            Debug.LogError("restoreAsset is null");
            return;
        }

        var courseControl = FindObjectOfType<CourseControl>();

        restoreAsset.activeCard.Clear();
        restoreAsset.objs.Clear();
        foreach (var activeCardIndex in courseControl.activeCardSet)
        {
            restoreAsset.activeCard.Add(activeCardIndex);
        }
        foreach (var sceneObj in courseControl.objs)
        {
            var modelName = (sceneObj.model != null ? sceneObj.model.name : "");
            var uiName = (sceneObj.ui != null ? sceneObj.ui.name : "");
            var newSceneObjName = new SceneObjName(modelName, uiName);
            restoreAsset.objs.Add(newSceneObjName);
        }

        if (StateController.Instance != null)
            restoreAsset.stateIndex = StateController.Instance.stateIndex;


    }
}
