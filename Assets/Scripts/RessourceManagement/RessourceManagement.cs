using System;
using System.Collections.Generic;
using UnityEngine;

public class RessourceManagement : MonoBehaviour
{
    public GameObject[] MeelePrefabs;
    public GameObject[] GrenadePrefabs;
    public GameObject[] BottlePrefabs;
    public GameObject[] PustefixPrefabs;

    public int MeeleScrapAmountInPool = 10;
    public int BottleScrapAmountInPool = 10;
    public int GrenadeScrapAmountInPool = 10;
    public int PustefixScrapAmountInPool = 10;
    public float ScrapSpawnProbabilityOnSlimesInPercent = 100.0f;
    public float PlayerScrapDropProbabilityInPercent = 100.0f;
    public float TowerScrapDropProbabilityInPercent = 100.0f;

    private GameObject[][] _possiblePrefabs = new GameObject[Enum.GetNames(typeof(ScrapType)).Length][];
    private List<PoolScrap> _possiblePrefabPool;

    internal GameObject GetScrapPrefabBySubTypeIndex(int scrapTypeIndex, int subTypeIndex)
    {
        return PossiblePrefabs[scrapTypeIndex][subTypeIndex];
    }

    internal GameObject GetRandomScrapFromPool()
    {
        if (_possiblePrefabPool.Count < 1)
            PrepareScrapPools();


        int randomScrapObjectIndex = (int)UnityEngine.Random.Range(0.0f, _possiblePrefabPool.Count);
        PoolScrap poolscrap = _possiblePrefabPool[randomScrapObjectIndex];
        _possiblePrefabPool.Remove(poolscrap);
        return PossiblePrefabs[(int)poolscrap.ScrapType][poolscrap.SubType];
    }

    public GameObject[][] PossiblePrefabs
    {
        get
        {
            return _possiblePrefabs;
        }
    }

    private void Awake()
    {
        _possiblePrefabPool = new List<PoolScrap>();
        PossiblePrefabs[(int)ScrapType.MELEE] = MeelePrefabs;
        PossiblePrefabs[(int)ScrapType.BOTTLE] = BottlePrefabs;
        PossiblePrefabs[(int)ScrapType.GRENADE] = GrenadePrefabs;
        PossiblePrefabs[(int)ScrapType.PUSTEFIX] = PustefixPrefabs;
        PrepareScrapPools();
        
    }


    private void PrepareScrapPools()
    {
        FillPossiblePrefabs(ScrapType.MELEE, MeeleScrapAmountInPool, MeelePrefabs);
        FillPossiblePrefabs(ScrapType.BOTTLE, BottleScrapAmountInPool, BottlePrefabs);
        FillPossiblePrefabs(ScrapType.GRENADE, GrenadeScrapAmountInPool, GrenadePrefabs);
        FillPossiblePrefabs(ScrapType.PUSTEFIX, PustefixScrapAmountInPool, PustefixPrefabs);
    }

    private void FillPossiblePrefabs(ScrapType scrapType, int amountOfScrapsInPool, GameObject[] possiblePrefabs)
    {
        for(int i = 0; i < amountOfScrapsInPool; i++)
        {
            int randomScrapIndex = (int)UnityEngine.Random.Range(0.0f, possiblePrefabs.Length);
            _possiblePrefabPool.Add(new PoolScrap(scrapType, possiblePrefabs[randomScrapIndex].GetComponent<Scrap>().SubCategoryIndex) );
        }
    }
}
