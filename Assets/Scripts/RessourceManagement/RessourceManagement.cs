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
    private List<GameObject> _possiblePrefabPool;
    private List<GameObject> _possiblePrefabCardDeck;

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
        _possiblePrefabPool = new List<GameObject>();

        FillPossiblePrefabs(MeeleScrapAmountInPool, MeelePrefabs);
        FillPossiblePrefabs(BottleScrapAmountInPool, BottlePrefabs);
        FillPossiblePrefabs(GrenadeScrapAmountInPool, GrenadePrefabs);

        _possiblePrefabCardDeck = _possiblePrefabPool;

        PossiblePrefabs[(int)ScrapType.MELEE] = MeelePrefabs;
        PossiblePrefabs[(int)ScrapType.BOTTLE] = BottlePrefabs;
        PossiblePrefabs[(int)ScrapType.GRENADE] = GrenadePrefabs;
    }

    private void FillPossiblePrefabs(int amountOfScrapsInPool, GameObject[] possiblePrefabs)
    {
        for(int i = 0; i < amountOfScrapsInPool; i++)
        {
            int randomScrapIndex = (int)UnityEngine.Random.Range(0.0f, possiblePrefabs.Length);
            _possiblePrefabPool.Add(possiblePrefabs[randomScrapIndex]);
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
        GameObject scrapObject = _possiblePrefabCardDeck[randomScrapObjectIndex];
        _possiblePrefabCardDeck.Remove(scrapObject);
        return scrapObject;
    }
}
