using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuilding : MonoBehaviour {

    public bool BuildTower(GameObject selectedTower)
    {
        PlayerScrapInventory playerinventory = FindObjectOfType<PlayerScrapInventory>();
        TowerRessourceManagement towermanagement = selectedTower.GetComponent<TowerRessourceManagement>();
        GameObject[] possibleScraps = FindObjectOfType<RessourceManagement>().PossibleScrabPrefabs;
        ArrayList[] scrapInventory = playerinventory.ScrapInventory;

        if (CheckForRessources(scrapInventory, towermanagement))
        {
            towermanagement.AddAllNeededScraps(scrapInventory);
            RemoveRessourcesFromInventory(playerinventory, towermanagement);
            return true;
        }

        return false;
        //Ressourcen prüfen
        //Ressourcen abziehen
        //Ressourcen dem Tower geben
        //Ressourcen beim Tower anzeigen lassen

        
    }

    private void RemoveRessourcesFromInventory(PlayerScrapInventory playerinventory, TowerRessourceManagement towermanagement)
    {
        playerinventory.RemoveAnyScraps((int)ScrapType.MELEE, towermanagement.NeededMeeleScrabs);
        playerinventory.RemoveAnyScraps((int)ScrapType.BOTTLE, towermanagement.NeededBottleScrabs);
        playerinventory.RemoveAnyScraps((int)ScrapType.GRENADE, towermanagement.NeededGrenadeScrabs);
    }

    private bool CheckForRessources(ArrayList[] inventory, TowerRessourceManagement towermanagement)
    {
        return inventory[(int)ScrapType.MELEE].Count >= towermanagement.NeededMeeleScrabs &&
               inventory[(int)ScrapType.BOTTLE].Count >= towermanagement.NeededBottleScrabs &&
               inventory[(int)ScrapType.GRENADE].Count >= towermanagement.NeededGrenadeScrabs;
    }

}
