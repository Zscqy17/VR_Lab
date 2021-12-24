using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OneKeyTool
{
    [Serializable]
    public struct RenameAsset
    {
        [Tooltip("步骤id")] public string id;
        public RenameState renameState;
        public RenameEvent[] renameEvents;
    }

    [Serializable]
    public struct RenameState
    {
        public string state_name;
        public StateAsset stateAsset;
    }

    [Serializable]
    public struct RenameEvent
    {
        public string event_name;
        public EventAsset eventAsset;
    }

    public class OneKeyRename : MonoBehaviour
    {
        public RenameAsset[] renameAssets;

        public void Rename()
        {
#if UNITY_EDITOR
            foreach (RenameAsset renameAsset in renameAssets)
            {
                //重命名步骤名
                renameAsset.renameState.stateAsset.stateName =
                    renameAsset.id + "、" + renameAsset.renameState.state_name;
                //重命名步骤文件名
                renameAsset.renameState.stateAsset.name = renameAsset.renameState.stateAsset.stateName + "_State";
                AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(renameAsset.renameState.stateAsset),
                    renameAsset.renameState.stateAsset.name);

                //重命名事件名
                foreach (var renameEvent in renameAsset.renameEvents)
                {
                    //事件对应步骤名
                    renameEvent.eventAsset.stateName = renameAsset.renameState.stateAsset.stateName;
                    //事件名
                    renameEvent.eventAsset.eventName = renameEvent.event_name;
                    //文件名
                    renameEvent.eventAsset.name =
                        renameEvent.eventAsset.eventName + "_" + renameAsset.renameState.stateAsset.stateName +
                        "_Event";
                    AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(renameEvent.eventAsset),
                        renameEvent.eventAsset.name);
                }
            }

            AssetDatabase.Refresh();
            Debug.Log("重命名完成");
#endif
        }
    }

}