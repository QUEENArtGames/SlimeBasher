using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : MonoBehaviour {

    public GameObject GameOverPanel;

    public void ShowGameOverPanel()
    {
        GameOverPanel.SetActive(true);
    }
}
