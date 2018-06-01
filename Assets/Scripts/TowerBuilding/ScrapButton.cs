using UnityEngine;


public class ScrapButton : MonoBehaviour
{
    private int _subTypeIndex;
    private ScrapType _scrapType;
    private TowerBuildingUI _towerBuildingUI;

    private void Awake()
    {
        _towerBuildingUI = FindObjectOfType<TowerBuildingUI>();
    }

    public int SubTypeIndex
    {
        get
        {
            return _subTypeIndex;
        }

        set
        {
            _subTypeIndex = value;
        }
    }

    public ScrapType ScrapType
    {
        get
        {
            return _scrapType;
        }

        set
        {
            _scrapType = value;
        }
    }

    public void ButtonListener()
    {
        GameObject selectedTower = _towerBuildingUI.SelectedTower;
        FindObjectOfType<TowerBuilding>().UpgradeWithScrap(selectedTower.GetComponent<TowerRessourceManagement>(), (int) _scrapType, _subTypeIndex);
        _towerBuildingUI.CloseTowerBuildingMenu();
    }
}
