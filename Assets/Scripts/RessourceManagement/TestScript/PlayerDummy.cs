using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDummy : MonoBehaviour {
    PlayerScrapDropAndCollection _playerScrapDropAndCollection;
    TowerBuilding _towerBuilding;
    PlayerScrapInventory _scrapInventory;
    TowerBuildingUI _towerBuildingUI;

    void Awake()
    {
        _playerScrapDropAndCollection = GetComponent<PlayerScrapDropAndCollection>();
        _towerBuilding = FindObjectOfType<TowerBuilding>();
        _scrapInventory = GetComponent<PlayerScrapInventory>();
        _towerBuildingUI = FindObjectOfType<TowerBuildingUI>();

    }

    //---------------------------- Testfunktionen Beginn
    void Update()
    {
        if (Input.GetKeyDown("o"))
            _playerScrapDropAndCollection.DropScraps();
    }

    private void OnTriggerEnter(Collider other)
    {
        //TowerBuildingUI nur einmal global
        //Spielerskript
        if (other.transform.gameObject.CompareTag("Tower") /*&& DefaultMode*/)
        {
            _towerBuildingUI.ShowTowerKillNotification();
            TowerRessourceManagement towermanagement = other.transform.gameObject.GetComponent<TowerRessourceManagement>();
            if (_towerBuilding.CheckForRessources(_scrapInventory.ScrapInventory, towermanagement) && towermanagement.ScrapSlotsOnTowerAreFree())
                _towerBuildingUI.ShowTowerUpgraeNotification();
        }
        ////


        if (other.transform.gameObject.CompareTag("Scrap"))
            _playerScrapDropAndCollection.CollectScrap(other.transform.gameObject);
    }

    //TESTEREI
    private void OnTriggerStay(Collider other)
    {
        //Spielerskript
        if (other.transform.gameObject.CompareTag("Tower") && Input.GetKeyDown("u") /*&& DefaultMode*/)
        {
            _towerBuildingUI.OpenTowerBuildingMenu(other.gameObject);
        }

        if (other.transform.gameObject.CompareTag("Tower") && Input.GetKeyDown("k") /*&& DefaultMode*/)
        {
            _towerBuildingUI.CloseTowerUpgradeNotification();
            _towerBuildingUI.CloseTowerBuildingMenu();
            other.gameObject.GetComponent<Tower>().Kill();
        }

        if (other.transform.gameObject.CompareTag("Tower") && Input.GetKeyDown("q") /*&& DefaultMode*/)
        {
            _towerBuildingUI.CloseTowerUpgradeNotification();
            _towerBuildingUI.CloseTowerBuildingMenu();
            _towerBuilding.UpgradeWithAnyScrap(other.gameObject.GetComponent<TowerRessourceManagement>());
        }
    }

    //TESTEREI
    private void OnTriggerExit(Collider other)
    {
        //Spielerskript
        if (other.transform.gameObject.CompareTag("Tower") /*&& DefaultMode*/)
        {
            _towerBuildingUI.CloseTowerKillNotification();
            _towerBuildingUI.CloseTowerUpgradeNotification();
            _towerBuildingUI.CloseTowerBuildingMenu();
        }
    }

    //---------------------------- Testfunktionen Ende
}
