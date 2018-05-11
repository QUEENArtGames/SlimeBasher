using UnityEngine;


public class PlayerRessourceManagement : MonoBehaviour
{
    public float Droprate = 1.0f;

    private GameObject[] _possibleScrapPrefabs;
    private PlayerScrapInventory _scrapInventory;

    //TESTVARIABLE
    private bool testvariable = true;


    void Awake()
    {
        _possibleScrapPrefabs = FindObjectOfType<RessourceManagement>().PossibleScrabPrefabs;
        _scrapInventory = gameObject.GetComponent<PlayerScrapInventory>();
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
        if (other.transform.gameObject.CompareTag("Tower") && Input.GetKey("u") && testvariable)
        {
            FindObjectOfType<TowerBuildingUI>().OpenTowerBuildingMenu(other.gameObject);
            //FindObjectOfType<TowerBuilding>().UpgradeWithAnyScrap(other.gameObject);
            //testvariable = false;
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
        if (!scrapObject.GetComponent<Scrap>().IsCollected)
        {
            Scrap scrap = scrapObject.GetComponent<Scrap>();
            ScrapType scraptype = scrap.Type;
            int meshindex = scrap.MeshIndex;
            Destroy(scrapObject);
            _scrapInventory.AddScrap(scraptype, meshindex);
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
                    GameObject scrap = Instantiate(_possibleScrapPrefabs[scrapTypeIndex], instanstiatePosition, new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
                    scrap.GetComponent<Scrap>().SetMesh((int) (_scrapInventory.ScrapInventory[scrapTypeIndex][index]));
                    scrap.GetComponent<Scrap>().ChangeCollectionState();
                    scrap.GetComponent<Scrap>().ChangeAttachementState();
                    scrap.GetComponent<Scrap>().Type = (ScrapType) scrapTypeIndex;
                    scrap.GetComponent<Rigidbody>().isKinematic = false;
                    _scrapInventory.RemoveScrap(scrapTypeIndex, index);
                }
            }
        }
    }
}
