using UnityEngine;


public class PlayerScrapDropAndCollection : MonoBehaviour
{
    private PlayerScrapInventory _scrapInventory;
    private RessourceManagement _ressourceManagement;
    private PlayerSounds _sounds;

    void Awake()
    {
        _scrapInventory = gameObject.GetComponent<PlayerScrapInventory>();
        _ressourceManagement = FindObjectOfType<RessourceManagement>();
        _sounds = GetComponent<PlayerSounds>();

    }

    public void CollectScrap(GameObject scrapObject)
    {
        Scrap scrap = scrapObject.GetComponent<Scrap>();
        if (!scrap.IsCollected)
        {
            ScrapType scraptype = scrap.Type;
            Destroy(scrapObject);
            _scrapInventory.AddScrap(scraptype, scrap.SubCategoryIndex);
            _sounds.PlayCollectionSound();

        }
    }

    public void DropScraps()
    {
        for (int scrapTypeIndex = 0; scrapTypeIndex < _scrapInventory.ScrapInventory.Length; scrapTypeIndex++)
        {
            for (int index = 0; index < _scrapInventory.ScrapInventory[scrapTypeIndex].Count; index++)
            {
                GameObject scrapObject = Instantiate(_ressourceManagement.GetScrapPrefabBySubTypeIndex(scrapTypeIndex, _scrapInventory.ScrapInventory[scrapTypeIndex][index]), new Vector3(transform.position.x, 1.5f, transform.position.z), Quaternion.identity);
                Scrap scrap = scrapObject.GetComponent<Scrap>();
                scrap.ChangeCollectionState();
                scrap.ChangeAttachementState();
                scrapObject.GetComponent<Rigidbody>().isKinematic = false;
                _scrapInventory.RemoveScrap(scrapTypeIndex, index);

                if (Random.Range(0.0f, 100.0f) > _ressourceManagement.PlayerScrapDropProbabilityInPercent)
                    Destroy(scrapObject);
            }
        }
    }
}
