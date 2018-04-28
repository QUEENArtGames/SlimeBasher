using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRessourceManagement : MonoBehaviour {

    public float droprate = 1.0f;

    private GameObject[] _possibleScrapPrefabs;
    private PlayerScrapInventory _scrapInventory;
	// Use this for initialization
	void Awake () {
        _possibleScrapPrefabs = FindObjectOfType<RessourceManagement>().PossibleScrabPrefabs;
        _scrapInventory = gameObject.GetComponent<PlayerScrapInventory>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey("o"))
            DropScraps();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.transform.gameObject.CompareTag("Scrap"))
            return;

        GameObject scrapObject =  other.transform.parent.gameObject;
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
        for(int scrapTypeIndex = 0; scrapTypeIndex < _scrapInventory.ScrapInventory.Length; scrapTypeIndex++)
        {
            for(int index = 0; index < _scrapInventory.ScrapInventory[scrapTypeIndex].Count; index++)
            {
                if(Random.Range(0.0f, 1.0f) <= droprate)
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
