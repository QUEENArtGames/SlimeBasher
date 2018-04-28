using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScrapInventory : MonoBehaviour {

    private ArrayList[] scrapInventory = new ArrayList[3];
    private int classicScraps = 0;

    private void Awake()
    {
        scrapInventory[0] = new ArrayList();
        scrapInventory[1] = new ArrayList();
        scrapInventory[2] = new ArrayList();
    }

    public int GetAmountOfScraps(ScrapType scrapType)
    {
        if (scrapType == ScrapType.CLASSIC)
            return classicScraps;

        return scrapInventory[(int)scrapType].Count;
    }

    public void AddScrap(ScrapType scrapType, int meshIndex)
    {
        scrapInventory[(int)scrapType].Add(meshIndex);
        Debug.Log("Index: " + scrapType + scrapInventory[(int)scrapType].Count);
    }

    public void RemoveScrap(int scrapType, int index)
    {
        scrapInventory[scrapType].RemoveAt(index);
    }

    public void RemoveAnyScraps(int scrapType, int amount)
    {
        for(int i = 0; i < amount; i++)
            scrapInventory[scrapType].RemoveAt(0);
    }

    public void AddClassicScraps(int amountOfScraps)
    {
        classicScraps += amountOfScraps;
    }

    public void RemoveClassicScraps(int amountOfScrabs)
    {
        classicScraps -= amountOfScrabs;
    }

    public ArrayList[] ScrapInventory
    {
        get
        {
            return scrapInventory;
        }
    }


}
