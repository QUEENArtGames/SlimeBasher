using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerBuildingUI : MonoBehaviour
{
    public GameObject upgradeText;
    public GameObject upgradeButtonPrefab;
    public GameObject buttonPanel;
    public GameObject upgradeMenu;

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


    ///
    public void ShowTowerUpgraeNotification()
    {
        upgradeText.SetActive(true);
        Debug.Log("Upgrade Menu geöffnet");
    }

    ///
    public void CloseTowerUpgradeNotification()
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

    public void OpenTowerBuildingMenu(GameObject selectedTower)
    {
        _selectedTower = selectedTower;
        TowerRessourceManagement towermanagement = selectedTower.GetComponent<TowerRessourceManagement>();
        if (towermanagement.NeededBottleScrabs > 0 && towermanagement.UpgradePossible())
            InstantiateButtonForEachMesh(ScrapType.BOTTLE);
        if (towermanagement.NeededGrenadeScrabs > 0)
            InstantiateButtonForEachMesh(ScrapType.GRENADE);
        if (towermanagement.NeededMeeleScrabs > 0)
            InstantiateButtonForEachMesh(ScrapType.MELEE);
    }

    public void CloseTowerBuildingMenu()
    {
        DestoryAllMenuElements();
    }

    private void InstantiateButtonForEachMesh(ScrapType scraptype)
    {
        Time.timeScale = 0.0f;
        buttonPanel.SetActive(true);
        GameObject scrapObject = FindObjectOfType<RessourceManagement>().PossibleScrabPrefabs[(int)scraptype];
        Scrap scrap = scrapObject.GetComponent<Scrap>();

        for (int meshindex = 0; meshindex < scrap.PossibleMeshes.Length; meshindex++)
        {
            GameObject button = Instantiate(upgradeButtonPrefab, buttonPanel.transform);
            button.GetComponentInChildren<Text>().text = scraptype + " " + meshindex;
            button.GetComponent<ScrapButton>().ScrapType = scraptype;
            button.GetComponent<ScrapButton>().Meshindex = meshindex;
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
