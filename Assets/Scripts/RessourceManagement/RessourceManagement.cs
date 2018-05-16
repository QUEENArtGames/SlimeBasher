using System;
using System.Collections.Generic;
using UnityEngine;

public class RessourceManagement : MonoBehaviour
{
    public GameObject[] MeelePrefabs;
    public GameObject[] GrenadePrefabs;
    public GameObject[] BottlePrefabs;

    private GameObject[][] _possiblePrefabs = new GameObject[Enum.GetNames(typeof(ScrapType)).Length][];

    //Nicht GameObject Klasse? Struct?
    private List<PoolScrap> _possiblePrefabPool;
    private List<PoolScrap> _possiblePrefabCardDeck;

    public int MeeleScrapAmountInPool = 10;
    public int BottleScrapAmountInPool = 10;
    public int GrenadeScrapAmountInPool = 10;

    public float RessourceProbabilityInPercent = 100.0f;

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

        FillPossiblePrefabs(ScrapType.MELEE, MeeleScrapAmountInPool, MeelePrefabs);
        FillPossiblePrefabs(ScrapType.BOTTLE, BottleScrapAmountInPool, BottlePrefabs);
        FillPossiblePrefabs(ScrapType.GRENADE, GrenadeScrapAmountInPool, GrenadePrefabs);

        _possiblePrefabCardDeck = _possiblePrefabPool;

        PossiblePrefabs[(int)ScrapType.MELEE] = MeelePrefabs;
        PossiblePrefabs[(int)ScrapType.BOTTLE] = BottlePrefabs;
        PossiblePrefabs[(int)ScrapType.GRENADE] = GrenadePrefabs;
    }

    private void FillPossiblePrefabs(ScrapType scrapType, int amountOfScrapsInPool, GameObject[] possiblePrefabs)
    {
        for(int i = 0; i < amountOfScrapsInPool; i++)
        {
            int randomScrapIndex = (int)UnityEngine.Random.Range(0.0f, possiblePrefabs.Length);
            _possiblePrefabPool.Add(new PoolScrap(scrapType, possiblePrefabs[randomScrapIndex].GetComponent<Scrap>().SubCategoryIndex) );
        }
    }

    internal GameObject GetRightScrapPrefab(int scrapTypeIndex, int subTypeIndex)
    {
        return PossiblePrefabs[scrapTypeIndex][subTypeIndex];
    }

    internal GameObject GetRandomScrapFromPool()
    {
        if (_possiblePrefabCardDeck.Count < 1)
            _possiblePrefabCardDeck = _possiblePrefabPool;

        int randomScrapObjectIndex = (int) UnityEngine.Random.Range(0.0f, _possiblePrefabCardDeck.Count);
        PoolScrap poolscrap = _possiblePrefabCardDeck[randomScrapObjectIndex];
        _possiblePrefabCardDeck.Remove(poolscrap);
        return PossiblePrefabs[(int)poolscrap.ScrapType][poolscrap.SubType]; 
    }
}
