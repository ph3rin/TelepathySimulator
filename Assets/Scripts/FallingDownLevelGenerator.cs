using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Object = System.Object;
using Random = System.Random;

public class FallingDownLevelGenerator
{
    private static readonly int[] HolePositionProbabilityMap;// = new[] {0, 0, 0, 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4};
    private static readonly int[] HoleSizeProbabilityMap;// = new[] {3, 3, 4, 4, 4, 5, 5};
    private static readonly int[] HoleCountProbabilityMap;// = new[] {1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 3, 4, 4};
    private const string CONFIG_PATH = "FallingDown/Configs/LevelGeneratorConfig";
    private const string BLOCK_SPRITE_PATH = "FallingDown/Images/Block";
    private GameObject m_blockPrototype;
    private GameObjectPool m_blockPool;
    private Random m_rand = new Random();
    private Queue<GameObject> m_levelQueue = new Queue<GameObject>();
    private int m_levelsBeforeItemGeneration;

    static FallingDownLevelGenerator()
    {
        var config = Resources.Load<LevelGeneratorConfig>(CONFIG_PATH);
        HoleCountProbabilityMap = config.HoleCountProbabilityMap;
        HoleSizeProbabilityMap = config.HoleSizeProbabilityMap;
        HolePositionProbabilityMap = config.HolePositionProbabilityMap;
    }
    public FallingDownLevelGenerator()
    {
        CreateBlockPrototype();
        m_blockPool = new GameObjectPool(m_blockPrototype);
        m_levelsBeforeItemGeneration = m_rand.Next(2, 4);
    }

    private void CreateBlockPrototype()
    {
        m_blockPrototype = new GameObject("block");
        m_blockPrototype.tag = "Block";
        m_blockPrototype.transform.position = new Vector3(-1000, 0, 0);
        m_blockPrototype.AddComponent<BoxCollider2D>();
        var spr = m_blockPrototype.AddComponent<SpriteRenderer>();
        spr.sprite = Sprite.Create(Resources.Load<Texture2D>(BLOCK_SPRITE_PATH), new Rect(0f, 0f, 128f, 128f),
            new Vector2(0.5f, 0.5f), 128f);
    }

    public GameObject GenerateLevel()
    {
        var level_obj = new GameObject("Level");
        var empty = new bool[25];
        Array.Clear(empty, 0, empty.Length);
        var hole_count = HoleCountProbabilityMap[m_rand.Next(HoleCountProbabilityMap.Length)];
        for (var i = 0; i < hole_count; ++i)
        {
            var hole_start = HolePositionProbabilityMap[m_rand.Next(HolePositionProbabilityMap.Length)];
            var hole_size = HoleSizeProbabilityMap[m_rand.Next(HoleSizeProbabilityMap.Length)];
            for (var j = 0; j < hole_size; ++j)
            {
                empty[hole_start * 5 + j] = true;
            }
        }

        bool generateItem = false;
        if (--m_levelsBeforeItemGeneration == 0)
        {
            generateItem = true;
            m_levelsBeforeItemGeneration = m_rand.Next(3, 6);
        }
        var generateItemCount = m_rand.Next(2, 4);
        var itemCd = m_rand.Next(4, 8);
        for (var i = 0; i < 25; ++i)
        {
            if (empty[i]) continue;
            var gm = m_blockPool.Claim();
            gm.transform.SetParent(level_obj.transform);
            gm.transform.localPosition += new Vector3Int(i, 0, 0);
            if (generateItem && (--itemCd == 0) && (generateItemCount--) > 0)
            {
                var items = FallingDownSession.Instance.ItemList;
                if (items.Length != 0)
                {
                    var item = GameObject.Instantiate(items[m_rand.Next(items.Length)], Vector3.zero, Quaternion.identity);
                    item.transform.SetParent(level_obj.transform, false);
                    item.transform.localPosition += new Vector3(i, 1.2f, 0);
                }
                itemCd = m_rand.Next(6, 10);

            }
            //gm.AddComponent<ShatteredBlockBehaviour>();
        }
        RecycleLevels();
        m_levelQueue.Enqueue(level_obj);
        return level_obj;
    }

    private void RecycleLevels()
    {
        while (m_levelQueue.Count > 0 && m_levelQueue.Peek().transform.position.y > Camera.main.transform.position.y + 30f)
        {
            var level = m_levelQueue.Dequeue();
            for (var i = level.transform.childCount - 1; i >= 0; --i)
            {
                var gm = level.transform.GetChild(i).gameObject;
                if (!gm.CompareTag("Item"))
                    m_blockPool.Recycle(gm);
            }
            GameObject.Destroy(level);
        }
    }
}