using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRessourceManagement : MonoBehaviour {

    public Transform[] ScrapSlots;
    public float Droprate = 1.0f;

    private ArrayList _attachedScraps;
    private GameObject[] _possibleScrapPrefabs;

    // Use this for initialization
    void Awake () {
        _possibleScrapPrefabs = FindObjectOfType<RessourceManagement>().PossibleScrabPrefabs;
        _attachedScraps = new ArrayList();
	}
	
	// Update is called once per frame
	void Update () {
        //Test Destroy Tower
        if (Input.GetKey("i"))
            DestroyTower();

        //TestBuildTower();
        if (Input.GetKey("u"))
        {
            PlayerScrapInventory inventory = FindObjectOfType<PlayerScrapInventory>();
            ArrayList[] scrapInventory = inventory.ScrapInventory;
           
            for (int scrapTypeIndex = 0; scrapTypeIndex < scrapInventory.Length; scrapTypeIndex++)
            {
                for (int index = 0; index < scrapInventory[scrapTypeIndex].Count; index++)
                {
                    Vector3 instanstiatePosition = ScrapSlots[_attachedScraps.Count].position;
                    GameObject scrap = Instantiate(_possibleScrapPrefabs[scrapTypeIndex], instanstiatePosition, new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
                        scrap.GetComponent<Scrap>().SetMesh((int)scrapInventory[scrapTypeIndex][index]);
                        scrap.GetComponent<Scrap>().Type = (ScrapType)scrapTypeIndex;
                        scrap.GetComponent<Rigidbody>().isKinematic = true;
                        inventory.RemoveScrap(scrapTypeIndex, index);
                        _attachedScraps.Add(scrap);


                }
            }
        }
            

    }

    public void AddScrap(GameObject scrap)
    {
        if(_attachedScraps.Count < ScrapSlots.Length)
        {
            Vector3 spawnposition = ScrapSlots[_attachedScraps.Count].position;
            GameObject scrapInstant = Instantiate(scrap, spawnposition , new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
            _attachedScraps.Add(scrapInstant);
        }
        
    }

    public void DestroyTower()
    {
        foreach (GameObject scrap in _attachedScraps)
        {
            if (Random.Range(0.0f, 1.0f) <= Droprate) { 
                scrap.GetComponent<Rigidbody>().isKinematic = false;
                scrap.GetComponent<Scrap>().ChangeAttachementState();
                scrap.GetComponent<Scrap>().ChangeCollectionState();
            }
            else
            {
                Destroy(scrap);
            }
        }

        _attachedScraps.RemoveRange(0, _attachedScraps.Count);

    }
}
