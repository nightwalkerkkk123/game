using System.Collections.Generic;
using UnityEngine;

namespace scipts
{
    public class CardFactory : MonoBehaviour
    {
        public static CardFactory Instance { get; private set; }
    
        [SerializeField] private GameObject[] cardPrefabs; // 在 Inspector 中拖拽预制体
        [SerializeField] private Transform player1HandArea; // 玩家1手牌区域
        [SerializeField] private Transform player2HandArea; // 玩家2手牌区域
    
        private Dictionary<string, GameObject> cardPrefabDict = new Dictionary<string, GameObject>();
        private Dictionary<int, float> playerCardXOffsets = new Dictionary<int, float>(); // 记录每个玩家的卡牌x轴偏移量
    
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        
            // 初始化预制体字典
            foreach (GameObject prefab in cardPrefabs)
            {
                if (prefab.GetComponent<Card>() != null)
                {
                    cardPrefabDict[prefab.name] = prefab;
                    Debug.Log($"成功加载卡牌预制体：{prefab.name}");
                }
            }

            // 初始化玩家卡牌x轴偏移量
            playerCardXOffsets[0] = 0f;
            playerCardXOffsets[1] = 0f;

            // 检查手牌区域是否设置
            if (player1HandArea == null || player2HandArea == null)
            {
                Debug.LogError("手牌区域未设置！请在 Inspector 中设置 player1HandArea 和 player2HandArea");
            }
            else
            {
                Debug.Log($"手牌区域已设置 - 玩家1: {player1HandArea.name}, 玩家2: {player2HandArea.name}");
            }
        }
    
        // 创建卡牌实例
        public Card CreateCard(string cardName, int playerId = 0)
        {
            if (string.IsNullOrEmpty(cardName))
            {
                Debug.LogError("卡牌名称为空！");
                return null;
            }
        
            if (cardPrefabDict.TryGetValue(cardName, out GameObject prefab))
            {
                GameObject cardObj = Instantiate(prefab);
                Card card = cardObj.GetComponent<Card>();
            
                if (card == null)
                {
                    Debug.LogError($"预制体 {cardName} 没有Card组件！");
                    Destroy(cardObj);
                    return null;
                }

                // 获取对应玩家的手牌区域
                Transform handArea = playerId == 0 ? player1HandArea : player2HandArea;
                if (handArea == null)
                {
                    Debug.LogError($"玩家 {playerId} 的手牌区域未设置！");
                    Destroy(cardObj);
                    return null;
                }

                Debug.Log($"正在为玩家 {playerId} 创建卡牌，使用手牌区域: {handArea.name}");

                // 设置卡牌为手牌区域的子对象
                cardObj.transform.SetParent(handArea);

                // 计算新的x轴位置（基础位置 + 偏移量）
                float baseX = 3.89712787f;
                float xOffset = playerCardXOffsets[playerId];
                playerCardXOffsets[playerId] -= 1.0f; // 每次创建新卡时增加x轴偏移量

                // 设置卡牌的相对位置、旋转和缩放
                cardObj.transform.localPosition = new Vector3(baseX + xOffset, 0.219999999f, -2.29999924f);
                cardObj.transform.localRotation = Quaternion.Euler(0f, 359.889984f, 0f);
                cardObj.transform.localScale = new Vector3(1.2476635f, 0.432852656f, -0.124306753f);
            
                Debug.Log($"创建卡牌 {cardName} 在位置: {cardObj.transform.localPosition}, 父对象: {handArea.name}");
            
                return card;
            }
        
            Debug.LogError($"找不到卡牌预制体：{cardName}");
            return null;
        }
    
        // 创建随机卡牌
        public Card CreateRandomCard(int playerId = 0)
        {
            if (cardPrefabDict.Count == 0)
            {
                Debug.LogError("没有可用的卡牌预制体！");
                return null;
            }
        
            // 从所有卡牌中随机选择一张
            int randomIndex = Random.Range(0, cardPrefabDict.Count);
            string[] cardNames = new string[cardPrefabDict.Count];
            cardPrefabDict.Keys.CopyTo(cardNames, 0);
        
            return CreateCard(cardNames[randomIndex], playerId);
        }
    
        // 创建指定费用的随机卡牌
        public Card CreateRandomCardByCost(int cost, int playerId = 0)
        {
            if (cardPrefabDict.Count == 0)
            {
                Debug.LogError("没有可用的卡牌预制体！");
                return null;
            }
        
            List<string> validCards = new List<string>();
        
            foreach (var prefab in cardPrefabDict)
            {
                Card card = prefab.Value.GetComponent<Card>();
                if (card != null && card.ManaCost == cost)
                {
                    validCards.Add(prefab.Key);
                }
            }
        
            if (validCards.Count > 0)
            {
                int randomIndex = Random.Range(0, validCards.Count);
                return CreateCard(validCards[randomIndex], playerId);
            }
        
            Debug.LogWarning($"没有找到费用为 {cost} 的卡牌");
            return null;
        }
    }
} 