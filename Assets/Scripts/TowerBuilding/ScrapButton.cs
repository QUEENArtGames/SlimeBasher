using UnityEngine;


public class ScrapButton : MonoBehaviour
{
    private int subTypeIndex;
    private ScrapType scrapType;

    public int SubTypeIndex
    {
        get
        {
            return subTypeIndex;
        }

        set
        {
            subTypeIndex = value;
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
        FindObjectOfType<TowerBuilding>().UpgradeWithScrap(selectedTower, (int) scrapType, subTypeIndex);
        FindObjectOfType<TowerBuildingUI>().CloseTowerBuildingMenu();
    }
}
