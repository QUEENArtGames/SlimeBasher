using System.Collections.Generic;
using UnityEngine;


public class TowerBuilding : MonoBehaviour
{
    private PlayerScrapInventory _playerInventory;
    private List<int>[] _playerScraps;

    private void Awake()
    {
        _playerInventory = FindObjectOfType<PlayerScrapInventory>();
        _playerScraps = _playerInventory.ScrapInventory;
    }

    public void BuildTower(GameObject selectedTower)
    {
        TowerRessourceManagement towermanagement = selectedTower.GetComponent<TowerRessourceManagement>();
        towermanagement.AddAllNeededScraps(_playerScraps);
        RemoveRessourcesFromInventory(_playerInventory, towermanagement);
        selectedTower.tag = "Tower";
    }

    public void UpgradeWithAnyScrap(GameObject selectedTower)
    {
        TowerRessourceManagement towermanagement = selectedTower.GetComponent<TowerRessourceManagement>();
        if (!CheckForRessources(_playerScraps, towermanagement))
            return;

        if (towermanagement.ScrapSlotsOnTowerAreFree())
        {
            towermanagement.AddNeededScrap(_playerScraps);
            RemoveAnyScrapFromInventory(_playerInventory, towermanagement);
        }
    }

    //noch nicht implementiert
    public void UpgradeWithScrap(GameObject selectedTower, int scrapType, int subtypeIndex)
    {
        TowerRessourceManagement towermanagement = selectedTower.GetComponent<TowerRessourceManagement>();
        if (towermanagement.ScrapSlotsOnTowerAreFree() && _playerInventory.SubTypeIsInInventory((int)scrapType, subtypeIndex))
        {
            towermanagement.AddNeededScrapOfCertainSubTypeIndex(_playerScraps, subtypeIndex);
            _playerInventory.RemoveScrapBySubTypeIndex(scrapType, subtypeIndex);
            //  
            //bestimmte Scrap zum Tower hinzufügen
            //Bestimmte Scrap entfernen
        }
    }

    internal bool TowerBuildingAllowed(GameObject selectedTower)
    {
        TowerRessourceManagement towermanagement = selectedTower.GetComponent<TowerRessourceManagement>();
        return CheckForRessources(_playerScraps, towermanagement);
    }

    private void RemoveRessourcesFromInventory(PlayerScrapInventory playerinventory, TowerRessourceManagement towermanagement)
    {
        playerinventory.RemoveAnyScraps((int) ScrapType.MELEE, towermanagement.NeededMeeleScrabs);
        playerinventory.RemoveAnyScraps((int) ScrapType.BOTTLE, towermanagement.NeededBottleScrabs);
        playerinventory.RemoveAnyScraps((int) ScrapType.GRENADE, towermanagement.NeededGrenadeScrabs);
    }

    private void RemoveAnyScrapFromInventory(PlayerScrapInventory playerinventory, TowerRessourceManagement towermanagement)
    {
        if (towermanagement.NeededMeeleScrabs > 0)
            playerinventory.RemoveAnyScraps((int) ScrapType.MELEE, 1);
        if (towermanagement.NeededBottleScrabs > 0)
            playerinventory.RemoveAnyScraps((int) ScrapType.BOTTLE, 1);
        if (towermanagement.NeededGrenadeScrabs > 0)
            playerinventory.RemoveAnyScraps((int) ScrapType.GRENADE, 1);
    }

    private bool CheckForRessources(List<int>[] inventory, TowerRessourceManagement towermanagement)
    {
        return inventory[(int) ScrapType.MELEE].Count >= towermanagement.NeededMeeleScrabs &&
               inventory[(int) ScrapType.BOTTLE].Count >= towermanagement.NeededBottleScrabs &&
               inventory[(int) ScrapType.GRENADE].Count >= towermanagement.NeededGrenadeScrabs;
    }
}
