using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OneKeyTool
{
    public class OneKeyLoadAssets : MonoBehaviour
    {
        public StateController StateController;

        [Header("是否是分支实验")] public bool isBranch;
        [HideInInspector] public int index;

        [Header("OneKey系列中含States和Events的脚本物体")]
        //public OneKeyBase[] oneKeyBases;
        public OneKeyActive active;

        public OneKeyCardSet cardSet;
        public OneKeyReplace replace;

        #region 读取出来的信息
        
        //读到的State
        public List<StateAsset> states = new List<StateAsset>();
        //读到的Event
        public List<EventAsset> events = new List<EventAsset>();

        //存储每个实验对应Asset路径
        //private Dictionary<int, List<string>> pathsList = new Dictionary<int, List<string>>();

        #endregion

        /// <summary>
        /// 加载读取到的State
        /// </summary>
        public void LoadStateAssets()
        {
#if UNITY_EDITOR
            ClearStates();
            string[] paths = AssetDatabase.FindAssets("t:StateAsset", new []{"Assets/Resources/Courses"});
            
            foreach (var path in paths)
            {
                states.Add(AssetDatabase.LoadAssetAtPath<StateAsset>(AssetDatabase.GUIDToAssetPath(path)));
            }
            Debug.LogFormat("加载State完成，State个数{0}",states.Count);
#endif
        }

        /// <summary>
        /// 按路径加载读取到的State
        /// </summary>
        public void LoadStateAssets(string[] fold_paths)
        {
#if UNITY_EDITOR
            ClearStates();
            string[] paths = AssetDatabase.FindAssets("t:StateAsset", fold_paths);

            foreach (var path in paths)
            {
                states.Add(AssetDatabase.LoadAssetAtPath<StateAsset>(AssetDatabase.GUIDToAssetPath(path)));
            }
            Debug.LogFormat("加载State完成，State个数{0}", states.Count);
#endif
        }

        /// <summary>
        /// 加载读取到的Event
        /// </summary>
        public void LoadEventAssets()
        {
#if UNITY_EDITOR
            ClearEvents();
            string[] paths = AssetDatabase.FindAssets("t:EventAsset", new[] { "Assets/Resources/Courses" });

            foreach (var path in paths)
            {
                events.Add(AssetDatabase.LoadAssetAtPath<EventAsset>(AssetDatabase.GUIDToAssetPath(path)));
            }
            Debug.LogFormat("加载Event完成，Event个数{0}", events.Count);
#endif
        }

        /// <summary>
        /// 加载读取到的Event
        /// </summary>
        public void LoadEventAssets(string[] fold_paths)
        {
#if UNITY_EDITOR
            ClearEvents();
            string[] paths = AssetDatabase.FindAssets("t:EventAsset", fold_paths);

            foreach (var path in paths)
            {
                events.Add(AssetDatabase.LoadAssetAtPath<EventAsset>(AssetDatabase.GUIDToAssetPath(path)));
            }
            Debug.LogFormat("加载Event完成，Event个数{0}", events.Count);
#endif
        }

        public void LoadBranchState()
        {
#if UNITY_EDITOR
            string[] foldes = AssetDatabase.GetSubFolders("Assets/Resources/Courses");
            var fold_paths = new List<string>();
            foreach (var fold in foldes)
            {
                var fold_id = Convert.ToInt32(fold.Split('/', '.')[3]);
                //是指定的分支实验
                if (fold_id == index)
                {
                    fold_paths.Add(fold);
                }
            }

            if (fold_paths.Count > 0)
            {
                LoadStateAssets(fold_paths.ToArray());
            }
            else
            {
                Debug.LogError("找不到指定分支实验！");
            }
#endif
        }

        public void LoadBranchEvent()
        {
#if UNITY_EDITOR
            string[] foldes = AssetDatabase.GetSubFolders("Assets/Resources/Courses");
            var fold_paths = new List<string>();
            foreach (var fold in foldes)
            {
                var fold_id = Convert.ToInt32(fold.Split('/', '.')[3]);
                //是指定的分支实验
                if (fold_id == index)
                {
                    fold_paths.Add(fold);
                }
            }

            if (fold_paths.Count > 0)
            {
                LoadEventAssets(fold_paths.ToArray());
            }
            else
            {
                Debug.LogWarning("找不到指定分支实验！或该分支实验无Event");
            }
#endif

        }

        /// <summary>
        /// 将加载出来的State放到StateController里
        /// </summary>
        public void AddStateToController()
        {
            if (states.Count > 0)
            {
                if (StateController)
                {
                    this.StateController.stateAssets = states.ToArray();
                    Debug.Log("添加到StateController完成");
                    Debug.Log("State填充完成，位置：StateController");
                }
                else
                {
                    Debug.LogError("请给StateController赋值！");
                }
            }
            else
            {
                Debug.LogError("State列表为空！请先 按加载按钮 或 确认Assets/Resources/Courses路径下有StateAsset资源！");
            }
        }

        /// <summary>
        /// 将加载出来的State放到OneKey工具里
        /// </summary>
        public void AddStateToOneKey()
        {
            if (states.Count > 0)
            {
                if (replace)
                {
                    replace.states = states.ToArray();
                }

                if (active)
                {
                    active.states = states.ToArray();
                }

                if (cardSet)
                {
                    cardSet.states = states.ToArray();
                }
                Debug.Log("State填充完成，位置：OneKey");
            }
            else
            {
                Debug.LogError("State列表为空！请先 按加载按钮 或 确认Assets/Resources/Courses路径下有StateAsset资源！");
            }
        }

        /// <summary>
        /// 将加载出来的Event放到OneKey工具里
        /// </summary>
        public void AddEventToOneKey()
        {
            if (events.Count > 0)
            {
                if (replace)
                {
                    replace.eventAssets = events.ToArray();
                }

                if (active)
                {
                    active.eventAssets = events.ToArray();
                }

                if (cardSet)
                {
                    cardSet.eventAssets = events.ToArray();
                }
                Debug.Log("Event填充完成，位置：OneKey");
            }
            else
            {
                Debug.LogError("Event列表为空！请先 按加载按钮 或 确认Assets/Resources/Courses路径下有EventAsset资源！");
            }
        }

        /// <summary>
        /// 清除State数据
        /// </summary>
        public void ClearStates()
        {
            states.Clear();
        }

        /// <summary>
        /// 清除Events数据
        /// </summary>
        public void ClearEvents()
        {
            events.Clear();
        }
    }
}
