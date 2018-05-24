using System.Collections.Generic;
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
    private GameObject _selectedTower;
    private TowerBuilding _towerBuilding;

    public GameObject SelectedTower
    {
        get
        {
            return _selectedTower;
        }
    }

    public void ShowTowerUpgraeNotification(GameObject selectedTower)
    {
        TowerRessourceManagement towermanagement = selectedTower.GetComponent<TowerRessourceManagement>();
        if (_towerBuilding.CheckForRessources(_playerInventory.ScrapInventory, towermanagement) && towermanagement.ScrapSlotsOnTowerAreFree())
            UpgradeText.SetActive(true);
    }

    public void CloseTowerUpgradeNotification()
    {
        UpgradeText.SetActive(false);
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
            button.GetComponent<ScrapButton>().SubTypeIndex = subTypeIndex;
            _uibuttons.Add(button);

            if (!FindObjectOfType<PlayerScrapInventory>().SubTypeIsInInventory((int) scraptype, subTypeIndex))
                button.GetComponent<Button>().interactable = false;

        }
       
    }

    private void Awake()
    {
        _playerInventory = FindObjectOfType<PlayerScrapInventory>();
        _uibuttons = new List<GameObject>();
        _towerBuilding = FindObjectOfType<TowerBuilding>();
    }
}
