using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScrapInventory : MonoBehaviour {

    private ArrayList[] _scrapInventory = new ArrayList[3];
    private int _classicScraps = 0;

    private void Awake()
    {
        _scrapInventory[0] = new ArrayList();
        _scrapInventory[1] = new ArrayList();
        _scrapInventory[2] = new ArrayList();
    }

    public int GetAmountOfScraps(ScrapType scrapType)
    {
        if (scrapType == ScrapType.CLASSIC)
            return _classicScraps;

        return _scrapInventory[(int)scrapType].Count;
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

    public ArrayList[] ScrapInventory
    {
        get
        {
            return _scrapInventory;
        }
    }


}
