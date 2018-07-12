using UnityEngine;

public class GameOverUI : MonoBehaviour
{

    public GameObject GameOverPanel;

    public void ShowGameOverPanel()
    {
        GameOverPanel.SetActive(true);
    }
}
