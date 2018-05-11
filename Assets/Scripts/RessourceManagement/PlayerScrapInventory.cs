using System;
using System.Collections.Generic;
using UnityEngine;


public class PlayerScrapInventory : MonoBehaviour
{
    private List<int>[] _scrapInventory = new List<int>[Enum.GetNames(typeof(ScrapType)).Length];
    private int _classicScraps = 0;


    void Awake()
    {
        for (int i = 0; i < Enum.GetNames(typeof(ScrapType)).Length; i++)
        {
            _scrapInventory[i] = new List<int>();
        }
    }

    public int GetAmountOfScraps(ScrapType scrapType)
    {
        return _scrapInventory[(int) scrapType].Count;
    }

    public int GetAmountOfScraps()
    {
        return _classicScraps;
    }

    public void AddScrap(ScrapType scrapType, int meshIndex)
    {
        _scrapInventory[(int) scrapType].Add(meshIndex);
    }

    public void RemoveScrap(int scrapType, int index)
    {
        _scrapInventory[scrapType].RemoveAt(index);
    }

    public void RemoveAnyScraps(int scrapType, int amount)
    {
        for (int i = 0; i < amount; i++)
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

    internal bool SubTypeIsInInventory(int scrapTypeIndex, int subTypeIndex)
    {
        foreach(int i in _scrapInventory[scrapTypeIndex])
        {
            if (subTypeIndex == i)
                return true;
        }
        return false;
    }
}
