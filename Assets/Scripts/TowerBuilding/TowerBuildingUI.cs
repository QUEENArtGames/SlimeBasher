using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerBuildingUI : MonoBehaviour {

    private PlayerScrapInventory _playerInventory;
    private List<int>[] _playerScraps;

    public GameObject upgradeText;
    public GameObject upgradeButtonPrefab;
    public GameObject upgradeMenu;

    ///
    internal void ShowTowerUpgraeNotification()
    {
        upgradeText.SetActive(true);
        Debug.Log("Upgrade Menu geöffnet");
    }

    ///
    internal void CloseTowerUpgradeNotification()
    {
        upgradeText.SetActive(false);
        Debug.Log("Upgrade Menu geschlossen");
    }

    internal void OpenTowerBuildingMenu(GameObject selectedTower)
    {
        
        TowerRessourceManagement towermanagement = selectedTower.GetComponent<TowerRessourceManagement>();
        if (towermanagement.NeededBottleScrabs > 0 && towermanagement.UpgradePossible())
            InstantiateButtonsForAllScraps(ScrapType.BOTTLE);
        if (towermanagement.NeededGrenadeScrabs > 0) 
            InstantiateButtonsForAllScraps(ScrapType.GRENADE);
        if (towermanagement.NeededMeeleScrabs > 0) 
            InstantiateButtonsForAllScraps(ScrapType.MELEE);
    }

    private void InstantiateButtonsForAllScraps(ScrapType scraptype)
    {
        for(int meshindex = 0; meshindex < _playerScraps[(int)scraptype].Count; meshindex++)
        {

            GameObject button = Instantiate(upgradeButtonPrefab, upgradeMenu.transform);
            button.GetComponentInChildren<Text>().text = scraptype + " " + _playerScraps[(int)scraptype][meshindex];
            button.GetComponent<ScrapButton>().ScrapType = scraptype;
            button.GetComponent<ScrapButton>().Meshindex = (int) _playerScraps[(int)scraptype][meshindex];
        }
    }

    private void Awake()
    {
        _playerInventory = FindObjectOfType<PlayerScrapInventory>();
        _playerScraps = _playerInventory.ScrapInventory;
    }
}
