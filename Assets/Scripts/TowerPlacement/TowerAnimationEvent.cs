using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAnimationEvent : MonoBehaviour {

    List<int>[] _playerScraps;

    private void Awake()
    {
        PlayerScrapInventory playerInventory = FindObjectOfType<PlayerScrapInventory>();
        _playerScraps = playerInventory.ScrapInventory;
    }

    public void AddScrapAfterAnimation()
    {
        Debug.Log("SCRAP ANIMATION EVENT");
        TowerRessourceManagement towermanagement = GetComponentInParent<TowerRessourceManagement>();
        towermanagement.AddAllNeededScraps(_playerScraps);
        FindObjectOfType<TowerBuilding>().RemoveRessourcesFromInventory(towermanagement);
    }


	
}
