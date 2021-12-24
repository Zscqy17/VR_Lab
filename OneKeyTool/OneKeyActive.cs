using System;
using System.Collections;
using System.Collections.Generic;
using ProcessData;
using UnityEditor;
using UnityEngine;

namespace OneKeyTool
{
    [Serializable] //物体的激活状态，将所有物体在对应的位置激活/禁用
    public struct ObjectActivation
    {
        [HideInInspector] public string name;
        public GameObject gameObject;
        [Header("激活")]
        [Tooltip("在进入State时激活")] public string active_state_indexs;
        [Tooltip("在触发Event时激活")] public string active_event_indexs;
        [Header("隐藏")]
        [Tooltip("在进入State时隐藏")] public string hide_state_indexs;
        [Tooltip("在触发Event时隐藏")] public string hide_event_indexs;
    }

    [Serializable] //物体、State、Event绑定预处理
    public struct PretreatObject
    {
        public int index;
        public List<EventObjectString> objects;

        public PretreatObject(int index, List<EventObjectString> objects)
        {
            this.index = index;
            this.objects = objects;
        }
    }

    public class OneKeyActive : MonoBehaviour
    {
        #region 公开配置

        public StateAsset[] states;
        public EventAsset[] eventAssets;
        [Header("物体激活/禁用配置")] public ObjectActivation[] objectActivations;

        #endregion

        #region 预处理后的数据

        //进入state后激活、隐藏
        private List<PretreatObject> stateActivePretreatObjects = new List<PretreatObject>();
        private List<PretreatObject> stateHidePretreatObjects = new List<PretreatObject>();

        //触发Event后激活、隐藏
        private List<PretreatObject> eventActivePretreatObjects = new List<PretreatObject>();
        private List<PretreatObject> eventHidePretreatObjects = new List<PretreatObject>();

        #endregion

        public void ActiveObjects()
        {
#if UNITY_EDITOR
            Pretreat();
            //State的物体激活设置
            foreach (var stateActivePretreatObject in stateActivePretreatObjects)
            {
                try
                {
                    states[stateActivePretreatObject.index].gameObjects_Active =
                        stateActivePretreatObject.objects.ToArray();
                    EditorUtility.SetDirty(states[stateActivePretreatObject.index]);
                    //AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
                catch (IndexOutOfRangeException e)
                {
                    Debug.LogError(e.Message);
                    Console.WriteLine(e);
                    throw;
                }
            }
            //Event的物体激活设置
            foreach (var eventActivePretreatObject in eventActivePretreatObjects)
            {
                try
                {
                    eventAssets[eventActivePretreatObject.index].gameObejcts_active =
                        eventActivePretreatObject.objects.ToArray();
                    EditorUtility.SetDirty(eventAssets[eventActivePretreatObject.index]);
                    //AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
                catch (IndexOutOfRangeException e)
                {
                    Debug.LogError(e.Message);
                    Console.WriteLine(e);
                    throw;
                }
            }

            //数据保存
            AssetDatabase.SaveAssets();

            Debug.Log("物体激活设置完成");
#endif

        }

        public void HideObjects()
        {
#if UNITY_EDITOR
            Pretreat();
            //State的物体禁用设置
            foreach (var stateHidePretreatObject in stateHidePretreatObjects)
            {
                try
                {
                    states[stateHidePretreatObject.index].gameObjects_Hide =
                        stateHidePretreatObject.objects.ToArray();
                    EditorUtility.SetDirty(states[stateHidePretreatObject.index]);
                    //AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
                catch (IndexOutOfRangeException e)
                {
                    Debug.LogError(e.Message + " 索引： " + stateHidePretreatObject.index);
                    Console.WriteLine(e);
                    throw;
                }
            }
            //Event的物体禁用设置
            foreach (var eventHidePretreatObject in eventHidePretreatObjects)
            {
                try
                {
                    eventAssets[eventHidePretreatObject.index].gameObjects_hide =
                        eventHidePretreatObject.objects.ToArray();
                    EditorUtility.SetDirty(eventAssets[eventHidePretreatObject.index]);
                    //AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
                catch (IndexOutOfRangeException e)
                {
                    Debug.LogError(e.Message + " 索引： "+ eventHidePretreatObject.index);
                    Console.WriteLine(e);
                    throw;
                }
            }

            //数据保存
            AssetDatabase.SaveAssets();

            Debug.Log("物体隐藏设置完成");
#endif
        }

        /// <summary>
        /// 数据预处理
        /// </summary>
        private void Pretreat()
        {
            Clear();
            for (int i = 0; i < objectActivations.Length; i++)
            {
                if (!objectActivations[i].gameObject)
                {
                    Debug.LogError("请将物体拖拽赋值！");
                    continue;
                }

                #region State物体激活预处理

                stateActivePretreatObjects = Pretreat(objectActivations[i].active_state_indexs, objectActivations[i].gameObject.name,
                    stateActivePretreatObjects);

                #endregion

                #region State物体禁用预处理

                stateHidePretreatObjects = Pretreat(objectActivations[i].hide_state_indexs, objectActivations[i].gameObject.name,
                    stateHidePretreatObjects);

                #endregion

                #region Event物体激活预处理

                eventActivePretreatObjects = Pretreat(objectActivations[i].active_event_indexs, objectActivations[i].gameObject.name,
                    eventActivePretreatObjects);

                #endregion

                #region Event物体禁用预处理

                eventHidePretreatObjects = Pretreat(objectActivations[i].hide_event_indexs, objectActivations[i].gameObject.name,
                    eventHidePretreatObjects);

                #endregion
            }
        }

        /// <summary>
        /// 单个项的预处理
        /// </summary>
        /// <param name="indexs"></param>
        /// <param name="name"></param>
        /// <param name="pretreatObjects"></param>
        /// <returns></returns>
        private List<PretreatObject> Pretreat(string indexs, string name, List<PretreatObject> pretreatObjects)
        {
            if (!indexs.Equals(string.Empty))
            {
                //提取下标
                string[] ids = indexs.Split(' ');
                int[] _indexs = new int[ids.Length];
                for (int j = 0; j < _indexs.Length; j++)
                {
                    _indexs[j] = Convert.ToInt32(ids[j]);
                    PretreatObject pretreatObject;
                    //如果已经存在了
                    if (CheckPretreat(pretreatObjects, _indexs[j], out pretreatObject))
                    {
                        EventObjectString temp = new EventObjectString(name,
                            TriggerTiming.BeforeTimeline);
                        //不存在这个物体名时添加进去
                        if (!pretreatObject.objects.Contains(temp))
                        {
                            pretreatObject.objects.Add(temp);
                        }
                    }
                    //如果没存在
                    else
                    {
                        pretreatObject = new PretreatObject(_indexs[j], new List<EventObjectString>());
                        pretreatObject.objects.Add(new EventObjectString(name, TriggerTiming.BeforeTimeline));
                        pretreatObjects.Add(pretreatObject);
                    }
                }
            }

            return pretreatObjects;
        }

        /// <summary>
        /// 检查在预处理的列表中是否存在此项
        /// </summary>
        /// <param name="pretreatCards">预处理完成的列表</param>
        /// <param name="index">State/Event下标</param>
        /// <param name="pretreat">已存在则返回对应信息，不存在则返回空对象</param>
        /// <returns>已存在则返回true</returns>
        private bool CheckPretreat(List<PretreatObject> pretreatObjects, int index, out PretreatObject pretreat)
        {
            foreach (var pretreatObject in pretreatObjects)
            {
                if (pretreatObject.index == index)
                {
                    pretreat = pretreatObject;
                    return true;
                }
            }

            pretreat = new PretreatObject();
            return false;
        }

        /// <summary>
        /// 清除之前的预处理信息
        /// </summary>
        private void Clear()
        {
            stateActivePretreatObjects.Clear();
            stateHidePretreatObjects.Clear();
            eventActivePretreatObjects.Clear();
            eventHidePretreatObjects.Clear();
        }
    }

}