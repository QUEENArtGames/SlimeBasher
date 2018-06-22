﻿using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

//TESTFUNKTIONEN
public class PlayerDummy : MonoBehaviour {
    private PlayerScrapDropAndCollection _playerScrapDropAndCollection;
    private TowerBuilding _towerBuilding;
    private PlayerScrapInventory _scrapInventory;
    private TowerBuildingUI _towerBuildingUI;
    public int _playerHealth = 100;
    public GameObject _healtSlider;
    public GameObject _endUI;
    public float _moveCharacterTimer = 15f;
    private float _timer;
    public Transform _playerSpawnPoint;

    private bool _isInDefaultMode = true;

    public int MaxDistanceToTower = 2;

    private void Start() {
        _healtSlider = GameObject.Find("HealthSlider");
    }

    public bool IsInDefaultMode {
        get {
            return _isInDefaultMode;
        }

        set {
            _isInDefaultMode = value;
        }
    }

    public int PlayerHealth {
        get {
            return _playerHealth;
        }
    }

    void Awake() {
        _playerScrapDropAndCollection = GetComponent<PlayerScrapDropAndCollection>();
        _towerBuilding = FindObjectOfType<TowerBuilding>();
        _scrapInventory = GetComponent<PlayerScrapInventory>();
        _towerBuildingUI = FindObjectOfType<TowerBuildingUI>();
        GetStartRessources();
 
    }

    private void GetStartRessources()
    {
        _scrapInventory.AddScrap(ScrapType.BOTTLE, 0);
    }

    void Update() {

        _healtSlider.GetComponent<Slider>().value = PlayerHealth;

        if (PlayerHealth <= 0) {
            _endUI.SetActive(true);
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            _timer += Time.deltaTime;
            _playerScrapDropAndCollection.DropScraps();
        }

        if(_timer >= _moveCharacterTimer) {
            gameObject.transform.position = _playerSpawnPoint.position;
            _endUI.SetActive(false);
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            _playerHealth = 100;
            _timer = 0f;
        }

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit)) {

            if (hit.transform.gameObject.CompareTag("Tower") && Vector3.Distance(transform.position, hit.transform.position) <= MaxDistanceToTower && IsInDefaultMode) {
                _towerBuildingUI.ShowTowerKillNotification();
                TowerRessourceManagement towermanagement = hit.transform.gameObject.GetComponent<TowerRessourceManagement>();
                if (_towerBuilding.CheckForRessources(_scrapInventory.ScrapInventory, towermanagement) && towermanagement.ScrapSlotsOnTowerAreFree())
                    _towerBuildingUI.ShowTowerUpgraeNotification();

                if (Input.GetButtonDown("Upgrade Tower") && IsInDefaultMode) {
                    _towerBuildingUI.OpenTowerBuildingMenu(hit.transform.gameObject);
                }

                if (Input.GetButtonDown("Deconstruct Tower") && IsInDefaultMode) {
                    _towerBuildingUI.CloseTowerUpgradeNotification();
                    _towerBuildingUI.CloseTowerBuildingMenu();
                    hit.transform.gameObject.GetComponent<Tower>().Kill();
                }

                if (Input.GetButtonDown("QuickUpgrade Tower") && IsInDefaultMode) {
                    _towerBuildingUI.CloseTowerUpgradeNotification();
                    _towerBuildingUI.CloseTowerBuildingMenu();
                    _towerBuilding.UpgradeWithAnyScrap(hit.transform.gameObject.GetComponent<TowerRessourceManagement>());
                }
            } else {
                _towerBuildingUI.CloseTowerKillNotification();
                _towerBuildingUI.CloseTowerUpgradeNotification();
            }

        }

    }

    private void OnCollisionEnter(Collision other) {
        if (other.transform.gameObject.CompareTag("Scrap") && _playerHealth > 0)
            _playerScrapDropAndCollection.CollectScrap(other.transform.gameObject);
    }
    
}