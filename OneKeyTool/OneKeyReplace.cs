using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProcessData;
using UnityEditor;
using UnityEngine;

namespace OneKeyTool
{
    [Serializable] //UI图片的替换
    public struct ReplaceSprite
    {
        [HideInInspector] public string name;
        public Sprite changeSprite;
        public GameObject changeObj;
        public string state_indexs;
        public string event_indexs;
    }

    [Serializable] //处理后数据
    public struct PretreatSprite
    {
        public int index { get; set; }
        public List<ChangingOptions> spriteChanges { get; set; }

        public PretreatSprite(int id, List<ChangingOptions> names)
        {
            index = id;
            spriteChanges = names;
        }
    }

    public class OneKeyReplace : MonoBehaviour
    {
        #region 公开配置

        public StateAsset[] states;
        public EventAsset[] eventAssets;

        [Header("替换图片配置")] public ReplaceSprite[] replaceSprites;

        #endregion

        #region 预处理后的数据

        //State中替换的列表
        private List<PretreatSprite> stateSprites = new List<PretreatSprite>();

        //Event中替换的列表
        private List<PretreatSprite> eventSprites = new List<PretreatSprite>();

        #endregion

        public void Replace()
        {
#if UNITY_EDITOR
            Pretreat();

            #region State替换部分

            foreach (var pretreatSprite in stateSprites)
            {
                try
                {
                    states[pretreatSprite.index].optionSprite = pretreatSprite.spriteChanges.ToArray();
                    EditorUtility.SetDirty(states[pretreatSprite.index]);
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

            #endregion

            #region Event替换部分

            foreach (var pretreatSprite in eventSprites)
            {
                try
                {
                    eventAssets[pretreatSprite.index].optionSprites = pretreatSprite.spriteChanges.ToArray();
                    EditorUtility.SetDirty(eventAssets[pretreatSprite.index]);
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

            #endregion

            //数据保存
            AssetDatabase.SaveAssets();

            Debug.Log("替换成功");
#endif

        }

        /// <summary>
        /// 数据预处理
        /// </summary>
        private void Pretreat()
        {
            Clear();

            for (int i = 0; i < replaceSprites.Length; i++)
            {
                #region State预处理

                stateSprites = Pretreat(replaceSprites[i].state_indexs, replaceSprites[i].changeSprite,
                    replaceSprites[i].changeObj.name, stateSprites);

                #endregion

                #region Event预处理

                eventSprites = Pretreat(replaceSprites[i].event_indexs, replaceSprites[i].changeSprite,
                    replaceSprites[i].changeObj.name, eventSprites);

                #endregion
            }
        }

        /// <summary>
        /// 单个项的预处理
        /// </summary>
        /// <param name="indexs"></param>
        /// <param name="sprite"></param>
        /// <param name="name"></param>
        /// <param name="pretreatSprites"></param>
        /// <returns></returns>
        private List<PretreatSprite> Pretreat(string indexs, Sprite sprite, string name, List<PretreatSprite> pretreatSprites)
        {
            if (!indexs.Equals(String.Empty))
            {
                //提取出下标
                string[] stateids = indexs.Split(' ');
                int[] _indexs = new int[stateids.Length];
                for (int j = 0; j < _indexs.Length; j++)
                {
                    _indexs[j] = Convert.ToInt32(stateids[j]);
                    PretreatSprite pretreatSprite;
                    //如果已经存了对应的信息
                    if (CheckPretreat(pretreatSprites, _indexs[j], out pretreatSprite))
                    {
                        pretreatSprite.spriteChanges.Add(new ChangingOptions(sprite,
                            name));
                    }
                    //如果没存
                    else
                    {
                        pretreatSprite = new PretreatSprite(_indexs[j], new List<ChangingOptions>());
                        pretreatSprite.spriteChanges.Add(new ChangingOptions(sprite,
                            name));
                        pretreatSprites.Add(pretreatSprite);
                    }
                }
            }

            return pretreatSprites;
        }

        /// <summary>
        /// 检查在预处理的列表中是否存在此项
        /// </summary>
        /// <param name="pretreatSprites">预处理完成的图像列表</param>
        /// <param name="index">State/Event下标</param>
        /// <param name="pretreat">已存在则返回对应信息，不存在则返回空对象</param>
        /// <returns></returns>
        private bool CheckPretreat(List<PretreatSprite> pretreatSprites, int index, out PretreatSprite pretreat)
        {
            foreach (var pretreatSprite in pretreatSprites)
            {
                if (pretreatSprite.index == index)
                {
                    pretreat = pretreatSprite;
                    return true;
                }
            }

            pretreat = new PretreatSprite();
            return false;
        }

        /// <summary>
        /// 清除预处理数据
        /// </summary>
        private void Clear()
        {
            stateSprites.Clear();
            eventSprites.Clear();
        }
    }

}