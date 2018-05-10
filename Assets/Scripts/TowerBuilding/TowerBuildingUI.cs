using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerBuildingUI : MonoBehaviour {
    private List<GameObject> _uibuttons;

    private PlayerScrapInventory _playerInventory;
    private List<int>[] _playerScraps;

    public GameObject upgradeText;
    public GameObject upgradeButtonPrefab;
    public GameObject buttonPanel;
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

    private void DestoryAllMenuElements()
    {
        foreach (GameObject button in _uibuttons)
            Destroy(button);

        Time.timeScale = 1.0f;
        buttonPanel.SetActive(false);
    }

    internal void OpenTowerBuildingMenu(GameObject selectedTower)
    { 
        TowerRessourceManagement towermanagement = selectedTower.GetComponent<TowerRessourceManagement>();
        if (towermanagement.NeededBottleScrabs > 0 && towermanagement.UpgradePossible())
            InstantiateButtonsForAllScraps(ScrapType.BOTTLE, selectedTower);
        if (towermanagement.NeededGrenadeScrabs > 0) 
            InstantiateButtonsForAllScraps(ScrapType.GRENADE, selectedTower);
        if (towermanagement.NeededMeeleScrabs > 0) 
            InstantiateButtonsForAllScraps(ScrapType.MELEE, selectedTower);
    }

    internal void CloseTowerBuildingMenu()
    {
        DestoryAllMenuElements();
    }

    private void InstantiateButtonsForAllScraps(ScrapType scraptype, GameObject selectedTower)
    {
        Time.timeScale = 0.0f;
        buttonPanel.SetActive(true);
        int buttonheight = 10;
        int menuheight = 20;
        for(int meshindex = 0; meshindex < _playerScraps[(int)scraptype].Count; meshindex++)
        {
            GameObject button = Instantiate(upgradeButtonPrefab, buttonPanel.transform);
            button.GetComponentInChildren<Text>().text = scraptype + " " + _playerScraps[(int)scraptype][meshindex];
            button.GetComponent<ScrapButton>().ScrapType = scraptype;
            button.GetComponent<ScrapButton>().Meshindex = (int) _playerScraps[(int)scraptype][meshindex];
            _uibuttons.Add(button);
        }
    }

    private void Awake()
    {
        _playerInventory = FindObjectOfType<PlayerScrapInventory>();
        _playerScraps = _playerInventory.ScrapInventory;
        _uibuttons = new List<GameObject>();
    }
}
