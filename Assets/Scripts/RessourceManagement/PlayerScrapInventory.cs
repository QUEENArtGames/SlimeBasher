using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScrapInventory : MonoBehaviour {

    private List<int>[] _scrapInventory = new List<int>[3];
    private int _classicScraps = 0;

    private void Awake()
    {
        _scrapInventory[0] = new List<int>();
        _scrapInventory[1] = new List<int>();
        _scrapInventory[2] = new List<int>();
    }

    public int GetAmountOfScraps(ScrapType scrapType)
    { 
        return _scrapInventory[(int)scrapType].Count;
    }

    public int GetAmountOfScraps()
    {
        return _classicScraps;
    }

    public void AddScrap(ScrapType scrapType, int meshIndex)
    {
        _scrapInventory[(int)scrapType].Add(meshIndex);
        Debug.Log("Index: " + scrapType + _scrapInventory[(int)scrapType].Count);
    }

    public void RemoveScrap(int scrapType, int index)
    {
        _scrapInventory[scrapType].RemoveAt(index);
    }

    public void RemoveAnyScraps(int scrapType, int amount)
    {
        for(int i = 0; i < amount; i++)
            _scrapInventory[scrapType].RemoveAt(0);
    }

    public void AddClassicScraps(int amountOfScraps)
    {
        _classicScraps += amountOfScraps;
    }

    public void RemoveClassicScraps(int amountOfScrabs)
    {
        _classicScraps -= amountOfScrabs;
    }

    public List<int>[] ScrapInventory
    {
        get
        {
            return _scrapInventory;
        }
    }


}
