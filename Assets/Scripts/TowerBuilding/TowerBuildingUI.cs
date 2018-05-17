﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerBuildingUI : MonoBehaviour
{
    public GameObject UpgradeText;
    public GameObject UpgradeButtonPrefab;
    public GameObject ButtonPanel;
    public Transform[] ButtonTransforms;
    //public GameObject UpgradeMenu;

    private List<GameObject> _uibuttons;
    private PlayerScrapInventory _playerInventory;
    private List<int>[] _playerScraps;
    private GameObject _selectedTower;

    public GameObject SelectedTower
    {
        get
        {
            return _selectedTower;
        }
    }

    public void ShowTowerUpgraeNotification()
    {
        UpgradeText.SetActive(true);
        Debug.Log("Upgrade Menu geöffnet");
    }

    public void CloseTowerUpgradeNotification()
    {
        UpgradeText.SetActive(false);
        Debug.Log("Upgrade Menu geschlossen");
    }

    private void DestoryAllMenuElements()
    {
        foreach (GameObject button in _uibuttons)
            Destroy(button);

        Time.timeScale = 1.0f;
        ButtonPanel.SetActive(false);
    }

    public void OpenTowerBuildingMenu(GameObject selectedTower)
    {
        _selectedTower = selectedTower;
        TowerRessourceManagement towermanagement = selectedTower.GetComponent<TowerRessourceManagement>();
        if (towermanagement.NeededBottleScrabs > 0 && towermanagement.ScrapSlotsOnTowerAreFree())
            InstantiateButtonForEachSubPrefab(ScrapType.BOTTLE);
        if (towermanagement.NeededGrenadeScrabs > 0)
            InstantiateButtonForEachSubPrefab(ScrapType.GRENADE);
        if (towermanagement.NeededMeeleScrabs > 0)
            InstantiateButtonForEachSubPrefab(ScrapType.MELEE);
    }

    public void CloseTowerBuildingMenu()
    {
        DestoryAllMenuElements();
    }

    private void InstantiateButtonForEachSubPrefab(ScrapType scraptype)
    {
        Time.timeScale = 0.0f;
        ButtonPanel.SetActive(true);
        GameObject[] possiblePrefabsOfScrapType = FindObjectOfType<RessourceManagement>().PossiblePrefabs[(int)scraptype];

        for (int subTypeIndex = 0; subTypeIndex < possiblePrefabsOfScrapType.Length; subTypeIndex++)
        {
            
            GameObject button = Instantiate(UpgradeButtonPrefab, ButtonTransforms[subTypeIndex]);
            button.GetComponentInChildren<Text>().text = scraptype + " " + subTypeIndex;
            button.GetComponent<ScrapButton>().ScrapType = scraptype;
            button.GetComponent<ScrapButton>().Meshindex = subTypeIndex;
            _uibuttons.Add(button);

            if (!FindObjectOfType<PlayerScrapInventory>().SubTypeIsInInventory((int) scraptype, subTypeIndex))
                button.GetComponent<Button>().interactable = false;

        }
       
    }

    private void Awake()
    {
        _playerInventory = FindObjectOfType<PlayerScrapInventory>();
        _playerScraps = _playerInventory.ScrapInventory;
        _uibuttons = new List<GameObject>();
    }
}
