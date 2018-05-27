using Assets.Scripts;
using UnityEngine;


public class PlayerScrapDropAndCollection : MonoBehaviour
{
    private PlayerScrapInventory _scrapInventory;
    private RessourceManagement _ressourceManagement;
    private TowerBuilding _towerBuilding;
    private float _droprate;

    void Awake()
    {
        _scrapInventory = gameObject.GetComponent<PlayerScrapInventory>();
        _ressourceManagement = FindObjectOfType<RessourceManagement>();
        _droprate = _ressourceManagement.PlayerScrapDropProbabilityInPercent;
        _towerBuilding = FindObjectOfType<TowerBuilding>();
    }

    //---------------------------- Testfunktionen Beginn
    void Update()
    {
        if (Input.GetKeyDown("o"))
            DropScraps();
    }

    private void OnTriggerEnter(Collider other)
    {
        //TowerBuildingUI nur einmal global
        //Spielerskript
        if (other.transform.gameObject.CompareTag("Tower") /*&& DefaultMode*/)
        {
            FindObjectOfType<TowerBuildingUI>().ShowTowerKillNotification();
            TowerRessourceManagement towermanagement = other.transform.gameObject.GetComponent<TowerRessourceManagement>();
            if (_towerBuilding.CheckForRessources(_scrapInventory.ScrapInventory, towermanagement) && towermanagement.ScrapSlotsOnTowerAreFree())
                FindObjectOfType<TowerBuildingUI>().ShowTowerUpgraeNotification();
        }
        ////
            

        if (other.transform.gameObject.CompareTag("Scrap"))
            CollectScrap(other.transform.gameObject);
    }

    //TESTEREI
    private void OnTriggerStay(Collider other)
    {
        //Spielerskript
        if (other.transform.gameObject.CompareTag("Tower") && Input.GetKeyDown("u") /*&& DefaultMode*/)
        {
            FindObjectOfType<TowerBuildingUI>().OpenTowerBuildingMenu(other.gameObject);
        }

        if (other.transform.gameObject.CompareTag("Tower") && Input.GetKeyDown("k") /*&& DefaultMode*/)
        {
            FindObjectOfType<TowerBuildingUI>().CloseTowerUpgradeNotification();
            FindObjectOfType<TowerBuildingUI>().CloseTowerBuildingMenu();
            other.gameObject.GetComponent<Tower>().Kill();
        }

        if (other.transform.gameObject.CompareTag("Tower") && Input.GetKeyDown("q") /*&& DefaultMode*/)
        {
            FindObjectOfType<TowerBuildingUI>().CloseTowerUpgradeNotification();
            FindObjectOfType<TowerBuildingUI>().CloseTowerBuildingMenu();
            FindObjectOfType<TowerBuilding>().UpgradeWithAnyScrap(other.gameObject.GetComponent<TowerRessourceManagement>());
        }
    }

    //TESTEREI
    private void OnTriggerExit(Collider other)
    {
        //Spielerskript
        if (other.transform.gameObject.CompareTag("Tower") /*&& DefaultMode*/)
        {
            FindObjectOfType<TowerBuildingUI>().CloseTowerKillNotification();
            FindObjectOfType<TowerBuildingUI>().CloseTowerUpgradeNotification();
            FindObjectOfType<TowerBuildingUI>().CloseTowerBuildingMenu();
        }
    }

    //---------------------------- Testfunktionen Ende

    private void CollectScrap(GameObject scrapObject)
    {
        Scrap scrap = scrapObject.GetComponent<Scrap>(); 
        if (!scrap.IsCollected)
        {
            ScrapType scraptype = scrap.Type;
            Destroy(scrapObject);
            _scrapInventory.AddScrap(scraptype, scrap.SubCategoryIndex);
        }
    }

    public void DropScraps()
    {
        for (int scrapTypeIndex = 0; scrapTypeIndex < _scrapInventory.ScrapInventory.Length; scrapTypeIndex++)
        {
            for (int index = 0; index < _scrapInventory.ScrapInventory[scrapTypeIndex].Count; index++)
            {
                if (Random.Range(0.0f, 100.0f) < _droprate)
                {
                    GameObject scrapObject = Instantiate(_ressourceManagement.GetScrapPrefabBySubTypeIndex(scrapTypeIndex, _scrapInventory.ScrapInventory[scrapTypeIndex][index]), transform.position, new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
                    Scrap scrap = scrapObject.GetComponent<Scrap>();
                    scrap.ChangeCollectionState();
                    scrap.ChangeAttachementState();
                    scrapObject.GetComponent<Rigidbody>().isKinematic = false;
                    _scrapInventory.RemoveScrap(scrapTypeIndex, index);
                }
            }
        }
    }
}
