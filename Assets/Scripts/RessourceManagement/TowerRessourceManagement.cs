using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRessourceManagement : MonoBehaviour {

    public Transform[] ScrapSlots;
    public float Droprate = 1.0f;
    public int ScrapThrowFactor;
    public int NeededMeeleScrabs;
    public int NeededBottleScrabs;
    public int NeededGrenadeScrabs;

    private ArrayList _attachedScraps = new ArrayList();
    private GameObject[] _possibleScrapPrefabs;
    //private int[] _neededRessources = new int[3];

    // Use this for initialization
    void Awake () {
        _possibleScrapPrefabs = FindObjectOfType<RessourceManagement>().PossibleScrabPrefabs;
    }
	
	// Update is called once per frame
	void Update () {
        //Test Destroy Tower
        if (Input.GetKey("i"))
            DestroyTower();
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

    public void AddAllNeededScraps(ArrayList[] scrapInventory)
    {
        for (int i = 0; i < NeededMeeleScrabs; i++)
            AddParticularScrap(ScrapType.MELEE, (int) scrapInventory[(int)ScrapType.MELEE][0]);

        for (int i = 0; i < NeededBottleScrabs; i++)
            AddParticularScrap(ScrapType.BOTTLE, (int) scrapInventory[(int)ScrapType.BOTTLE][0]);

        for (int i = 0; i < NeededGrenadeScrabs; i++)
            AddParticularScrap(ScrapType.GRENADE, (int) scrapInventory[(int)ScrapType.GRENADE][0]);


    }

    private void AddParticularScrap(ScrapType scraptype, int meshindex)
    {
        Vector3 spawnposition = ScrapSlots[_attachedScraps.Count].position;
        GameObject scrap = FindObjectOfType<RessourceManagement>().PossibleScrabPrefabs[(int)scraptype];
        GameObject scrapInstant = Instantiate(scrap, spawnposition, new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
        scrapInstant.GetComponent<Scrap>().SetMesh(meshindex);
        _attachedScraps.Add(scrapInstant);

    }

    public void DestroyTower()
    {
        foreach (GameObject scrap in _attachedScraps)
        {
            if (Random.Range(0.0f, 1.0f) <= Droprate) { 
                scrap.GetComponent<Rigidbody>().isKinematic = false;
                scrap.GetComponent<Scrap>().ChangeAttachementState();
                scrap.GetComponent<Scrap>().ChangeCollectionState();
                FindObjectOfType<RessourceManagement>().ThrowScrapAway(transform, scrap, ScrapThrowFactor);
            }
            else
            {
                Destroy(scrap);
            }
        }

        _attachedScraps.RemoveRange(0, _attachedScraps.Count);
    }
}
