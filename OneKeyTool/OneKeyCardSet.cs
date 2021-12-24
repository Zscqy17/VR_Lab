using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OneKeyTool
{

    [Serializable] //卡牌的激活状态，将所有卡牌在对应的位置激活/禁用
    public struct CardActivation
    {
        [HideInInspector] public string name;

        [Tooltip("CourseCenter中的卡牌索引")] public int card_index;

        [Header("进入State激活/禁用")]
        //进入步骤
        [Tooltip("进入State后激活")] public string active_state_enter_indexs;
        [Tooltip("进入State后禁用")] public string ban_state_enter_indexs;
        [Header("退出State激活/禁用")]
        //退出步骤
        [Tooltip("退出State后激活")] public string active_state_end_indexs;
        [Tooltip("退出State后禁用")] public string ban_state_end_indexs;
        [Header("Event触发激活/禁用")]
        [Tooltip("触发Event后激活")] public string active_event_indexs;
        [Tooltip("触发Event后禁用")] public string ban_event_indexs;
    }

    [Serializable] //卡牌——State、Event绑定
    public struct PretreatCard
    {
        public int index;
        public List<int> cards;

        public PretreatCard(int index, List<int> cards)
        {
            this.index = index;
            this.cards = cards;
        }
    }

    public class OneKeyCardSet : MonoBehaviour
    {
        #region 公开配置

        public StateAsset[] states;
        public EventAsset[] eventAssets;

        [Header("卡牌激活/禁用配置")]
        public CardActivation[] cardActivations;

        #endregion

        #region 预处理后的数据

        //State-Enter 进入State的设置
        private List<PretreatCard> stateEnterActiveCards = new List<PretreatCard>();
        private List<PretreatCard> stateEnterBanCards = new List<PretreatCard>();
        //State-Exit 离开State的设置
        private List<PretreatCard> stateExitActiveCards = new List<PretreatCard>();
        private List<PretreatCard> stateExitBanCards = new List<PretreatCard>();
        //Event-Trigger 触发Event的设置
        private List<PretreatCard> eventTriggerActiveCards = new List<PretreatCard>();
        private List<PretreatCard> eventTriggerBanCards = new List<PretreatCard>();

        #endregion

        //通过参数来改变操作，默认是设置步骤开始时的卡牌激活与禁用
        public void ActiveCards(bool start = true, bool setEvent = false)
        {
#if UNITY_EDITOR
            Pretreat();
            if (setEvent)
            {
                foreach (var eventTriggerActiveCard in eventTriggerActiveCards)
                {
                    try
                    {
                        eventAssets[eventTriggerActiveCard.index].activeTargets = eventTriggerActiveCard.cards.ToArray();
                        EditorUtility.SetDirty(eventAssets[eventTriggerActiveCard.index]);
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
                Debug.Log("卡牌 激活 设置完成，位置：Event触发");
            }
            else
            {
                if (start)
                {
                    foreach (var stateEnterActiveCard in stateEnterActiveCards)
                    {
                        try
                        {
                            states[stateEnterActiveCard.index].activeTargets_Enter = stateEnterActiveCard.cards.ToArray();
                            EditorUtility.SetDirty(states[stateEnterActiveCard.index]);
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
                    Debug.Log("卡牌 激活 设置完成，位置：State进入");
                }
                else
                {
                    foreach (var stateExitActiveCard in stateExitActiveCards)
                    {
                        try
                        {
                            states[stateExitActiveCard.index].activeTargets_Exit = stateExitActiveCard.cards.ToArray();
                            EditorUtility.SetDirty(states[stateExitActiveCard.index]);
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
                    Debug.Log("卡牌 激活 设置完成，位置：State退出");
                }
            }

            //数据保存
            AssetDatabase.SaveAssets();
#endif

        }

        public void BanCards(bool start = true, bool setEvent = false)
        {
#if UNITY_EDITOR
            Pretreat();
            if (setEvent)
            {
                foreach (var eventTriggerBanCard in eventTriggerBanCards)
                {
                    try
                    {
                        eventAssets[eventTriggerBanCard.index].disableTargets = eventTriggerBanCard.cards.ToArray();
                        EditorUtility.SetDirty(eventAssets[eventTriggerBanCard.index]);
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
                Debug.Log("卡牌 禁用 设置完成，位置：Event触发");
            }
            else
            {
                if (start)
                {
                    foreach (var stateEnterBanCard in stateEnterBanCards)
                    {
                        try
                        {
                            states[stateEnterBanCard.index].disableTargets_Enter = stateEnterBanCard.cards.ToArray();
                            EditorUtility.SetDirty(states[stateEnterBanCard.index]);
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
                    Debug.Log("卡牌 禁用 设置完成，位置：State进入");
                }
                else
                {
                    foreach (var stateExitBanCard in stateExitBanCards)
                    {
                        try
                        {
                            states[stateExitBanCard.index].disableTargets_Exit = stateExitBanCard.cards.ToArray();
                            EditorUtility.SetDirty(states[stateExitBanCard.index]);
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
                    Debug.Log("卡牌 禁用 设置完成，位置：State退出");
                }
            }

            //数据保存
            AssetDatabase.SaveAssets();
#endif

        }

        /// <summary>
        /// 数据预处理
        /// </summary>
        private void Pretreat()
        {
            Clear();
            for (int i = 0; i < cardActivations.Length; i++)
            {
                #region 进入State的激活预处理

                stateEnterActiveCards = Pretreat(cardActivations[i].active_state_enter_indexs, cardActivations[i].card_index,
                    stateEnterActiveCards);

                #endregion

                #region 退出State的激活预处理

                stateExitActiveCards = Pretreat(cardActivations[i].active_state_end_indexs, cardActivations[i].card_index,
                    stateExitActiveCards);

                #endregion

                #region Event的激活预处理

                eventTriggerActiveCards = Pretreat(cardActivations[i].active_event_indexs, cardActivations[i].card_index,
                    eventTriggerActiveCards);

                #endregion

                #region 进入State的禁用预处理

                stateEnterBanCards = Pretreat(cardActivations[i].ban_state_enter_indexs, cardActivations[i].card_index,
                    stateEnterBanCards);

                #endregion

                #region 退出State的禁用预处理

                stateExitBanCards = Pretreat(cardActivations[i].ban_state_end_indexs, cardActivations[i].card_index,
                    stateExitBanCards);

                #endregion

                #region Event的禁用预处理

                eventTriggerBanCards = Pretreat(cardActivations[i].ban_event_indexs, cardActivations[i].card_index,
                    eventTriggerBanCards);

                #endregion
            }
        }

        /// <summary>
        /// 单个项的预处理
        /// </summary>
        /// <param name="indexs"></param>
        /// <param name="cardID"></param>
        /// <param name="pretreatCards"></param>
        /// <returns></returns>
        private List<PretreatCard> Pretreat(string indexs, int cardID, List<PretreatCard> pretreatCards)
        {
            if (!indexs.Equals(String.Empty))
            {
                //提取索引信息
                string[] ids = indexs.Split(' ');
                int[] _indexs = new int[ids.Length];
                for (int j = 0; j < _indexs.Length; j++)
                {
                    _indexs[j] = Convert.ToInt32(ids[j]);
                    PretreatCard pretreatCard;
                    //如果已经有预处理过的对应index信息了
                    if (CheckPretreat(pretreatCards, _indexs[j], out pretreatCard))
                    {
                        if (!pretreatCard.cards.Contains(cardID))
                        {
                            pretreatCard.cards.Add(cardID);
                        }
                    }
                    //如果还没有
                    else
                    {
                        pretreatCard = new PretreatCard(_indexs[j], new List<int>());
                        pretreatCard.cards.Add(cardID);
                        pretreatCards.Add(pretreatCard);
                    }
                }
            }

            return pretreatCards;
        }

        /// <summary>
        /// 检查在预处理的列表中是否存在此项
        /// </summary>
        /// <param name="pretreatCards">预处理完成的卡牌列表</param>
        /// <param name="index">State/Event下标</param>
        /// <param name="pretreat">已存在则返回对应信息，不存在则返回空对象</param>
        /// <returns>已存在则返回true</returns>
        private bool CheckPretreat(List<PretreatCard> pretreatCards, int index, out PretreatCard pretreat)
        {
            foreach (var pretreatCard in pretreatCards)
            {
                if (pretreatCard.index == index)
                {
                    pretreat = pretreatCard;
                    return true;
                }
            }

            pretreat = new PretreatCard();
            return false;
        }

        /// <summary>
        /// 清除之前的预处理信息
        /// </summary>
        private void Clear()
        {
            //State-Enter 进入State的设置
            stateEnterActiveCards.Clear();
            stateEnterBanCards.Clear();
            //State-Exit 离开State的设置
            stateExitActiveCards.Clear();
            stateExitBanCards.Clear();
            //Event-Trigger 触发Event的设置
            eventTriggerActiveCards.Clear();
            eventTriggerBanCards.Clear();
        }
    }

}