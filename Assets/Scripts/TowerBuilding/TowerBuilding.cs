using System.Collections.Generic;
using UnityEngine;


public class TowerBuilding : MonoBehaviour
{
    private PlayerScrapInventory _playerInventory;
    private List<int>[] _playerScraps;
    private TowerRessourceManagement _towermanagement;

    private void Awake()
    {

    }

    private void Start()
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
        RemoveAnyNeededScrapFromInventory(towermanagement);

    }

    public void UpgradeWithAnyScrap(TowerRessourceManagement towermanagement)
    {
        if (!CheckForRessources(_playerScraps, towermanagement))
            return;

        if (towermanagement.ScrapSlotsOnTowerAreFree())
        {
            towermanagement.AddNeededScrap(_playerScraps);
            RemoveAnyNeededScrapFromInventory(towermanagement);
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

    private void RemoveAnyNeededScrapFromInventory(TowerRessourceManagement towermanagement)
    {
        for (int scrapTypeIndex = 0; scrapTypeIndex < towermanagement.NeededScraps.Length; scrapTypeIndex++)
        {
            if (towermanagement.NeededScraps[scrapTypeIndex])
                _playerInventory.RemoveAnyScrap(scrapTypeIndex);
        }
    }

    internal bool CheckForRessources(List<int>[] inventory, TowerRessourceManagement towermanagement)
    {
        return HasEnoughSpecialScraps(inventory, towermanagement) &&
               _playerInventory.ClassicScraps >= towermanagement.NeededClassicScraps;
    }

    private bool HasEnoughSpecialScraps(List<int>[] inventory, TowerRessourceManagement towermanagement)
    {
        return (inventory[(int) ScrapType.MELEE].Count >= 1 && towermanagement.NeedsMeeleScraps) ||
               (inventory[(int) ScrapType.BOTTLE].Count >= 1 && towermanagement.NeedsBottleScraps) ||
               (inventory[(int) ScrapType.GRENADE].Count >= 1 && towermanagement.NeedsGrenadeScraps) ||
               (inventory[(int) ScrapType.PUSTEFIX].Count >= 1 && towermanagement.NeedsPustefixScraps);

    }
}
