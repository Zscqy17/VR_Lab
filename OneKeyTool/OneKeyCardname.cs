using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Universal.Card;

namespace OneKeyTool
{
    [Serializable]
    public struct SingleCard
    {
        public string cardName;
        public bool needReceiver;
    }

    [Serializable]
    public struct RenameCard
    {
        public GameObject cardUI;
        public string newName;
    }

    public class OneKeyCardname : MonoBehaviour
    {
        [Header("模板卡牌UI")] public GameObject templateCard;

        [Header("卡牌注册器后缀")] public string handler = "Handler";

        [Header("需要生成的卡牌")] public SingleCard[] cards;

        [Header("需要重命名的卡牌")] public RenameCard[] renameCards;

        [Header("Receivers父物体")] public GameObject receivers;

        public void CreateCards()
        {
            var hasSameName = false;

            foreach (var card in cards)
            {
                GameObject existCard = GameObject.Find(card.cardName + "Card");
                if (!templateCard)
                {
                    Debug.LogError("存在需要生成的卡牌，请拖入模板卡牌！");
                    break;
                }
                if (templateCard.name.Equals(card.cardName + "Card"))
                {
                    hasSameName = true;
                }
                if (!existCard)
                {
                    GameObject card_t = Instantiate(templateCard);
                    card_t.transform.parent = templateCard.transform.parent;
                    CreateCard(card_t, card);
                }
                else
                {
                    CreateCard(existCard, card);
                }
            }

            if (!hasSameName && templateCard)
            {
                DestroyImmediate(templateCard);
            }
            templateCard = null;
            Debug.Log("卡牌命名完成");
        }

        private void CreateCard(GameObject card, SingleCard renameCard)
        {
            //左旋以及高亮
            GameObject left = card.transform.GetChild(1).gameObject;
            GameObject left_highlight = left.transform.GetChild(0).gameObject;
            //右旋以及高亮
            GameObject right = card.transform.GetChild(2).gameObject;
            GameObject right_highlight = right.transform.GetChild(0).gameObject;

            //获取场景中物体对应的Receiver
            GameObject card_receiver = GameObject.Find(card.name + handler);

            //重命名
            card.name = renameCard.cardName + "Card";
            left.name = card.name + "_Left";
            left_highlight.name = left.name + "_Highlight";
            right.name = card.name + "_Right";
            right_highlight.name = right.name + "_Highlight";

            //如果场景里已经有对应的Recevier了
            if (card_receiver)
            {
                if (!card_receiver.GetComponent<CardOptionHandler>().cardUI)
                {
                    card_receiver.GetComponent<CardOptionHandler>().cardUI = card.GetComponent<CardControl>();
                }
                card_receiver.name = card.name + handler;
            }
            //如果场景里没有有对应的Recevier
            else
            {
                if (!renameCard.needReceiver) return;
                if (!receivers)
                {
                    Debug.LogError("请挂载Receviers父物体！");
                    return;
                }

                card_receiver = new GameObject();
                card_receiver.AddComponent<CardOptionHandler>();
                card_receiver.transform.parent = receivers.transform;
                card_receiver.name = card.name + handler;
                card_receiver.GetComponent<CardOptionHandler>().cardUI =
                    card.GetComponent<CardControl>();
            }
        }

        public void Rename()
        {
            foreach (var renameCard in renameCards)
            {
                if (!renameCard.cardUI || renameCard.newName.Equals(string.Empty))
                {
                    Debug.LogError("存在未填写的卡牌物体或名称！");
                    continue;
                }
                //左旋以及高亮
                GameObject left = renameCard.cardUI.transform.GetChild(1).gameObject;
                GameObject left_highlight = left.transform.GetChild(0).gameObject;
                //右旋以及高亮
                GameObject right = renameCard.cardUI.transform.GetChild(2).gameObject;
                GameObject right_highlight = right.transform.GetChild(0).gameObject;

                //获取场景中物体对应的Receiver
                GameObject card_receiver = GameObject.Find(renameCard.cardUI.name + handler);

                //重命名
                renameCard.cardUI.name = renameCard.newName + "Card";
                left.name = renameCard.cardUI.name + "_Left";
                left_highlight.name = left.name + "_Highlight";
                right.name = renameCard.cardUI.name + "_Right";
                right_highlight.name = left.name + "_Highlight";

                //重命名Receiver
                //如果场景里已经有对应的Recevier了
                if (card_receiver)
                {
                    card_receiver.name = renameCard.cardUI.name + handler;
                }
            }
            Debug.Log("卡牌重命名完成");
        }
    }

}