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

    public void BuildTower(GameObject selectedTower)
    {
        selectedTower.tag = "Tower";
        TowerRessourceManagement towermanagement = selectedTower.GetComponent<TowerRessourceManagement>();
        GameObject scrapObject = towermanagement.AddNeededScrap(_playerScraps);
        scrapObject.SetActive(false);
        selectedTower.GetComponentInChildren<TowerAnimationEvent>().NewScrap = scrapObject;
        RemoveRessourcesFromInventory(towermanagement);
       
    }

    public void UpgradeWithAnyScrap(TowerRessourceManagement towermanagement)
    {
        if (!CheckForRessources(_playerScraps, towermanagement))
            return;

        if (towermanagement.ScrapSlotsOnTowerAreFree())
        {
            towermanagement.AddNeededScrap(_playerScraps);
            RemoveAnyScrapFromInventory(towermanagement);
        }
    }

    public void UpgradeWithScrap(TowerRessourceManagement towermanagement, int scrapType, int subtypeIndex)
    {
        if (towermanagement.ScrapSlotsOnTowerAreFree() && _playerInventory.SubTypeIsInInventory(scrapType, subtypeIndex))
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
        _playerInventory.RemoveAnyScraps((int) ScrapType.MELEE, towermanagement.NeededMeeleScraps);
        _playerInventory.RemoveAnyScraps((int) ScrapType.BOTTLE, towermanagement.NeededBottleScraps);
        _playerInventory.RemoveAnyScraps((int) ScrapType.GRENADE, towermanagement.NeededGrenadeScraps);
        _playerInventory.RemoveClassicScraps(towermanagement.NeededClassicScraps);
    }

    private void RemoveAnyScrapFromInventory(TowerRessourceManagement towermanagement)
    {
        if (towermanagement.NeededMeeleScraps > 0)
            _playerInventory.RemoveAnyScraps((int) ScrapType.MELEE, 1);
        if (towermanagement.NeededBottleScraps > 0)
            _playerInventory.RemoveAnyScraps((int) ScrapType.BOTTLE, 1);
        if (towermanagement.NeededGrenadeScraps > 0)
            _playerInventory.RemoveAnyScraps((int) ScrapType.GRENADE, 1);
    }

    internal bool CheckForRessources(List<int>[] inventory, TowerRessourceManagement towermanagement)
    {
        return inventory[(int) ScrapType.MELEE].Count >= towermanagement.NeededMeeleScraps &&
               inventory[(int) ScrapType.BOTTLE].Count >= towermanagement.NeededBottleScraps &&
               inventory[(int) ScrapType.GRENADE].Count >= towermanagement.NeededGrenadeScraps &&
               _playerInventory.ClassicScraps >= towermanagement.NeededClassicScraps;
    }
}
