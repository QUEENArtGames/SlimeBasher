using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuilding : MonoBehaviour {

    private PlayerScrapInventory _playerInventory;
    private ArrayList[] _playerScraps;

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
        if(towermanagement.UpgradePossible())
            towermanagement.AddNeededScrap(_playerScraps);
            RemoveAnyScrapFromInventory(_playerInventory, towermanagement);
    }

    internal void OpenTowerUpgradeMenu()
    {
        Debug.Log("Upgrade Menu geöffnet");
    }

    internal bool TowerBuildingAllowed(GameObject selectedTower)
    {
        TowerRessourceManagement towermanagement = selectedTower.GetComponent<TowerRessourceManagement>();
        return CheckForRessources(_playerScraps, towermanagement);
    }

    private void Awake()
    {
        _playerInventory = FindObjectOfType<PlayerScrapInventory>();
        _playerScraps = _playerInventory.ScrapInventory;
    }

    private void RemoveRessourcesFromInventory(PlayerScrapInventory playerinventory, TowerRessourceManagement towermanagement)
    {
        playerinventory.RemoveAnyScraps((int)ScrapType.MELEE, towermanagement.NeededMeeleScrabs);
        playerinventory.RemoveAnyScraps((int)ScrapType.BOTTLE, towermanagement.NeededBottleScrabs);
        playerinventory.RemoveAnyScraps((int)ScrapType.GRENADE, towermanagement.NeededGrenadeScrabs);
    }

    private void RemoveAnyScrapFromInventory(PlayerScrapInventory playerinventory, TowerRessourceManagement towermanagement)
    {
        if (towermanagement.NeededMeeleScrabs > 0)
            playerinventory.RemoveAnyScraps((int)ScrapType.MELEE, 1);
        if (towermanagement.NeededBottleScrabs > 0)
            playerinventory.RemoveAnyScraps((int)ScrapType.BOTTLE, 1);
        if (towermanagement.NeededGrenadeScrabs > 0)
            playerinventory.RemoveAnyScraps((int)ScrapType.GRENADE, 1);
    }

    private bool CheckForRessources(ArrayList[] inventory, TowerRessourceManagement towermanagement)
    {
        return inventory[(int)ScrapType.MELEE].Count >= towermanagement.NeededMeeleScrabs &&
               inventory[(int)ScrapType.BOTTLE].Count >= towermanagement.NeededBottleScrabs &&
               inventory[(int)ScrapType.GRENADE].Count >= towermanagement.NeededGrenadeScrabs;
    }

}
