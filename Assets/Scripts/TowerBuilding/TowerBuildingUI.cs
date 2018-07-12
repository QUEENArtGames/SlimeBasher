using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerBuildingUI : MonoBehaviour
{
    public GameObject UpgradeText;
    public GameObject KillText;
    public GameObject UpgradeButtonPrefab;
    public GameObject ButtonPanel;
    public Transform[] ButtonTransforms;

    private List<GameObject> _uibuttons;
    private PlayerScrapInventory _playerInventory;
    private GameObject _selectedTower;
    private RessourceManagement _ressourceManagement;

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
    }

    public void CloseTowerUpgradeNotification()
    {
        UpgradeText.SetActive(false);
    }


    public void ShowTowerKillNotification()
    {
        KillText.SetActive(true);
    }

    public void CloseTowerKillNotification()
    {
        KillText.SetActive(false);
    }

    private void EnableCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }


    public void OpenTowerBuildingMenu(GameObject selectedTower)
    {
        EnableCursor();
        _selectedTower = selectedTower;
        TowerRessourceManagement towermanagement = selectedTower.GetComponent<TowerRessourceManagement>();
        for (int scrapTypeIndex = 0; scrapTypeIndex < towermanagement.NeededScraps.Length; scrapTypeIndex++)
        {
            if (towermanagement.NeededScraps[scrapTypeIndex])
                InstantiateButtonForEachSubPrefab((ScrapType) scrapTypeIndex);
        }

        Time.timeScale = 0.0f;
    }

    public void CloseTowerBuildingMenu()
    {
        DestoryAllMenuElements();
    }

    private void DestoryAllMenuElements()
    {
        foreach (GameObject button in _uibuttons)
            Destroy(button);

        Time.timeScale = 1.0f;
        ButtonPanel.SetActive(false);
    }

    private void InstantiateButtonForEachSubPrefab(ScrapType scraptype)
    {
        ButtonPanel.SetActive(true);

        for (int subTypeIndex = 0; subTypeIndex < _ressourceManagement.PossiblePrefabs[(int) scraptype].Length; subTypeIndex++)
        {
            GameObject button = Instantiate(UpgradeButtonPrefab, ButtonTransforms[subTypeIndex]);
            ScrapButton scrapButton = button.GetComponent<ScrapButton>();
            button.GetComponentInChildren<Text>().text = GetSubScrapName(scraptype, subTypeIndex);
            scrapButton.ScrapType = scraptype;
            scrapButton.SubTypeIndex = subTypeIndex;
            _uibuttons.Add(button);

            if (!_playerInventory.SubTypeIsInInventory((int) scraptype, subTypeIndex))
                button.GetComponent<Button>().interactable = false;
        }
    }

    private string GetSubScrapName(ScrapType scraptype, int subTypeIndex)
    {
        string buttonname = scraptype + "";
        if (scraptype == ScrapType.MELEE)
        {
            switch (subTypeIndex)
            {
                case 0:
                    buttonname = "PLANK";
                    break;
                case 1:
                    buttonname = "TENNIS RACKET";
                    break;
                case 2:
                    buttonname = "TOILETBRUSH";
                    break;
            }
        }

        return buttonname;
    }

    private void Awake()
    {
        _ressourceManagement = FindObjectOfType<RessourceManagement>();
        _playerInventory = FindObjectOfType<PlayerScrapInventory>();
        _uibuttons = new List<GameObject>();
    }
}
