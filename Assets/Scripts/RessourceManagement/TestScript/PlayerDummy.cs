using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TESTFUNKTIONEN
public class PlayerDummy : MonoBehaviour {
    private PlayerScrapDropAndCollection _playerScrapDropAndCollection;
    private TowerBuilding _towerBuilding;
    private PlayerScrapInventory _scrapInventory;
    private TowerBuildingUI _towerBuildingUI;

    private bool _isInDefaultMode = true;

    public bool IsInDefaultMode
    {
        get
        {
            return _isInDefaultMode;
        }

        set
        {
            _isInDefaultMode = value;
        }
    }

    void Awake()
    {
        _playerScrapDropAndCollection = GetComponent<PlayerScrapDropAndCollection>();
        _towerBuilding = FindObjectOfType<TowerBuilding>();
        _scrapInventory = GetComponent<PlayerScrapInventory>();
        _towerBuildingUI = FindObjectOfType<TowerBuildingUI>();

    }

    void Update()
    {
        if (Input.GetKeyDown("o"))
            _playerScrapDropAndCollection.DropScraps();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.CompareTag("Tower") && IsInDefaultMode)
        {
            _towerBuildingUI.ShowTowerKillNotification();
            TowerRessourceManagement towermanagement = other.transform.gameObject.GetComponent<TowerRessourceManagement>();
            if (_towerBuilding.CheckForRessources(_scrapInventory.ScrapInventory, towermanagement) && towermanagement.ScrapSlotsOnTowerAreFree())
                _towerBuildingUI.ShowTowerUpgraeNotification();
        }


        if (other.transform.gameObject.CompareTag("Scrap"))
            _playerScrapDropAndCollection.CollectScrap(other.transform.gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.gameObject.CompareTag("Tower") && Input.GetButtonDown("Upgrade Tower") && IsInDefaultMode)
        {
            _towerBuildingUI.OpenTowerBuildingMenu(other.gameObject);
        }

        if (other.transform.gameObject.CompareTag("Tower") && Input.GetButtonDown("Deconstruct Tower") && IsInDefaultMode)
        {
            _towerBuildingUI.CloseTowerUpgradeNotification();
            _towerBuildingUI.CloseTowerBuildingMenu();
            other.gameObject.GetComponent<Tower>().Kill();
        }

        if (other.transform.gameObject.CompareTag("Tower") && Input.GetButtonDown("QuickUpgrade Tower") && IsInDefaultMode)
        {
            _towerBuildingUI.CloseTowerUpgradeNotification();
            _towerBuildingUI.CloseTowerBuildingMenu();
            _towerBuilding.UpgradeWithAnyScrap(other.gameObject.GetComponent<TowerRessourceManagement>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.gameObject.CompareTag("Tower") && IsInDefaultMode)
        {
            _towerBuildingUI.CloseTowerKillNotification();
            _towerBuildingUI.CloseTowerUpgradeNotification();
            _towerBuildingUI.CloseTowerBuildingMenu();
        }
    }
}
