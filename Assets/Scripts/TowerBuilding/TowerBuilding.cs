using System.Collections.Generic;
using UnityEngine;


public class TowerBuilding : MonoBehaviour
{
    private PlayerScrapInventory _playerInventory;
    private List<int>[] _playerScraps;
    private TowerRessourceManagement _towermanagement;

    private void Awake()
    {
        _playerInventory = FindObjectOfType<PlayerScrapInventory>();
        _playerScraps = _playerInventory.ScrapInventory;
    }

    public void SetTowerTag(GameObject selectedTower)
    {
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
            RemoveAnyScrapFromInventory(towermanagement);
        }
    }

    public void UpgradeWithScrap(GameObject selectedTower, int scrapType, int subtypeIndex)
    {
        TowerRessourceManagement towermanagement = selectedTower.GetComponent<TowerRessourceManagement>();
        if (towermanagement.ScrapSlotsOnTowerAreFree() && _playerInventory.SubTypeIsInInventory((int)scrapType, subtypeIndex))
        {
            towermanagement.AddNeededScrapOfCertainSubTypeIndex(_playerScraps, subtypeIndex);
            _playerInventory.RemoveScrapBySubTypeIndex(scrapType, subtypeIndex);
        }
    }

    internal bool TowerBuildingAllowed(GameObject selectedTower)
    {
        TowerRessourceManagement towermanagement = selectedTower.GetComponent<TowerRessourceManagement>();
        return CheckForRessources(_playerScraps, towermanagement);
    }

    public void RemoveRessourcesFromInventory(TowerRessourceManagement towermanagement)
    {
        _playerInventory.RemoveAnyScraps((int) ScrapType.MELEE, towermanagement.NeededMeeleScrabs);
        _playerInventory.RemoveAnyScraps((int) ScrapType.BOTTLE, towermanagement.NeededBottleScrabs);
        _playerInventory.RemoveAnyScraps((int) ScrapType.GRENADE, towermanagement.NeededGrenadeScrabs);
    }

    private void RemoveAnyScrapFromInventory(TowerRessourceManagement towermanagement)
    {
        if (towermanagement.NeededMeeleScrabs > 0)
            _playerInventory.RemoveAnyScraps((int) ScrapType.MELEE, 1);
        if (towermanagement.NeededBottleScrabs > 0)
            _playerInventory.RemoveAnyScraps((int) ScrapType.BOTTLE, 1);
        if (towermanagement.NeededGrenadeScrabs > 0)
            _playerInventory.RemoveAnyScraps((int) ScrapType.GRENADE, 1);
    }

    private bool CheckForRessources(List<int>[] inventory, TowerRessourceManagement towermanagement)
    {
        return inventory[(int) ScrapType.MELEE].Count >= towermanagement.NeededMeeleScrabs &&
               inventory[(int) ScrapType.BOTTLE].Count >= towermanagement.NeededBottleScrabs &&
               inventory[(int) ScrapType.GRENADE].Count >= towermanagement.NeededGrenadeScrabs;
    }
}
