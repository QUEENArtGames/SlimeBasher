using UnityEngine;


public class PlayerRessourceManagement : MonoBehaviour
{
    public float Droprate = 1.0f;

    private PlayerScrapInventory _scrapInventory;
    private RessourceManagement _ressourceManagement;

    //TESTVARIABLE
    private bool testvariable = true;


    void Awake()
    {
        _scrapInventory = gameObject.GetComponent<PlayerScrapInventory>();
        _ressourceManagement = FindObjectOfType<RessourceManagement>();
    }

    void Update()
    {
        if (Input.GetKey("o"))
            DropScraps();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Spielerskript
        if (other.transform.gameObject.CompareTag("Tower"))
            FindObjectOfType<TowerBuildingUI>().ShowTowerUpgraeNotification();

        if (other.transform.gameObject.CompareTag("Scrap"))
            CollectScrap(other.transform.parent.gameObject);
    }

    //TESTEREI
    private void OnTriggerStay(Collider other)
    {
        //Spielerskript
        if (other.transform.gameObject.CompareTag("Tower") && Input.GetKey("u"))
        {
            FindObjectOfType<TowerBuildingUI>().OpenTowerBuildingMenu(other.gameObject);
        }

        if (other.transform.gameObject.CompareTag("Tower") && Input.GetKey("k"))
        {
            FindObjectOfType<TowerBuildingUI>().CloseTowerUpgradeNotification();
            FindObjectOfType<TowerBuildingUI>().CloseTowerBuildingMenu();
            other.gameObject.GetComponent<Tower>().Kill();
        }

        if (other.transform.gameObject.CompareTag("Tower") && Input.GetKey("q"))
        {
            FindObjectOfType<TowerBuildingUI>().CloseTowerUpgradeNotification();
            FindObjectOfType<TowerBuildingUI>().CloseTowerBuildingMenu();
            FindObjectOfType<TowerBuilding>().UpgradeWithAnyScrap(other.gameObject);
        }
    }

    //TESTEREI
    private void OnTriggerExit(Collider other)
    {
        //Spielerskript
        if (other.transform.gameObject.CompareTag("Tower"))
        {
            FindObjectOfType<TowerBuildingUI>().CloseTowerUpgradeNotification();
            FindObjectOfType<TowerBuildingUI>().CloseTowerBuildingMenu();
        }
    }

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
        Vector3 instanstiatePosition = transform.position;
        for (int scrapTypeIndex = 0; scrapTypeIndex < _scrapInventory.ScrapInventory.Length; scrapTypeIndex++)
        {
            for (int index = 0; index < _scrapInventory.ScrapInventory[scrapTypeIndex].Count; index++)
            {
                if (Random.Range(0.0f, 1.0f) <= Droprate)
                {
                    GameObject scrapObject = Instantiate(_ressourceManagement.GetRightScrapPrefab(scrapTypeIndex, _scrapInventory.ScrapInventory[scrapTypeIndex][index]), instanstiatePosition, new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
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
