using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour {

    public Text MeeleScrapText;
    public Text WaterScrapText;
    public Text GrenadeScrapText;
    public Text AirScrapText;
    public Text ClassicScrapText;

    private bool _inventoryWasRecentlyChanged = false;
    private PlayerScrapInventory _playerScrapInventory;

    public void InventoryWasRecentlyChanged()
    {
        _inventoryWasRecentlyChanged = true;
    }

    private void Awake()
    {
        _playerScrapInventory = FindObjectOfType<PlayerScrapInventory>();
    }

    void Update () {
        if (_inventoryWasRecentlyChanged)
        {
            UpdateUIText();
            _inventoryWasRecentlyChanged = false;
        }
	}

    private void UpdateUIText()
    {
        MeeleScrapText.text = _playerScrapInventory.ScrapInventory[(int)ScrapType.MELEE].Count + "";
        GrenadeScrapText.text = _playerScrapInventory.ScrapInventory[(int)ScrapType.GRENADE].Count + "";
        AirScrapText.text = _playerScrapInventory.ScrapInventory[(int)ScrapType.PUSTEFIX].Count + "";
        WaterScrapText.text = _playerScrapInventory.ScrapInventory[(int)ScrapType.BOTTLE].Count + "";
        ClassicScrapText.text = _playerScrapInventory.ClassicScraps + "";
    }
}
