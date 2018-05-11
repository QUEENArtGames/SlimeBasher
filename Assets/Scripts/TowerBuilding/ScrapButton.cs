using UnityEngine;


public class ScrapButton : MonoBehaviour
{
    private int meshindex;
    private ScrapType scrapType;


    public int Meshindex
    {
        get
        {
            return meshindex;
        }

        set
        {
            meshindex = value;
        }
    }

    public ScrapType ScrapType
    {
        get
        {
            return scrapType;
        }

        set
        {
            scrapType = value;
        }
    }

    public void ButtonListener()
    {
        GameObject selectedTower = FindObjectOfType<TowerBuildingUI>().SelectedTower;
        FindObjectOfType<TowerBuilding>().UpgradeWithAnyScrap(selectedTower);
        FindObjectOfType<TowerBuildingUI>().CloseTowerBuildingMenu();
    }
}
