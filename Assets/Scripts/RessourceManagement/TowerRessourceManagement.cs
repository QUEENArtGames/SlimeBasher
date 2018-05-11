using System.Collections.Generic;
using UnityEngine;


public class TowerRessourceManagement : MonoBehaviour
{
    public Transform[] ScrapSlots;
    public float Droprate = 1.0f;
    public int ScrapThrowFactor;
    public int NeededMeeleScrabs;
    public int NeededBottleScrabs;
    public int NeededGrenadeScrabs;

    private List<GameObject> _attachedScraps = new List<GameObject>();
    private GameObject[] _possibleScrapPrefabs;


    void Awake()
    {
        _possibleScrapPrefabs = FindObjectOfType<RessourceManagement>().PossibleScrabPrefabs;
    }

    void Update()
    {
        //Test Destroy Tower
        if (Input.GetKey("i"))
            DestroyTower();
    }

    internal bool UpgradePossible()
    {
        return _attachedScraps.Count < ScrapSlots.Length;
    }

    /*public void AddScrap(GameObject scrap)
    {
        if(_attachedScraps.Count < ScrapSlots.Length)
        {
            Vector3 spawnposition = ScrapSlots[_attachedScraps.Count].position;
            GameObject scrapInstant = Instantiate(scrap, spawnposition , new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
            _attachedScraps.Add(scrapInstant);
        }
    }*/

    public void AddAllNeededScraps(List<int>[] scrapInventory)
    {
        for (int i = 0; i < NeededMeeleScrabs; i++)
            AddParticularScrap(ScrapType.MELEE, (int) scrapInventory[(int) ScrapType.MELEE][0]);

        for (int i = 0; i < NeededBottleScrabs; i++)
            AddParticularScrap(ScrapType.BOTTLE, (int) scrapInventory[(int) ScrapType.BOTTLE][0]);

        for (int i = 0; i < NeededGrenadeScrabs; i++)
            AddParticularScrap(ScrapType.GRENADE, (int) scrapInventory[(int) ScrapType.GRENADE][0]);
    }

    public void AddNeededScrap(List<int>[] scrapInventory)
    {
        if (NeededMeeleScrabs > 0)
            AddParticularScrap(ScrapType.MELEE, (int) scrapInventory[(int) ScrapType.MELEE][0]);
        if (NeededBottleScrabs > 0)
            AddParticularScrap(ScrapType.BOTTLE, (int) scrapInventory[(int) ScrapType.BOTTLE][0]);
        if (NeededGrenadeScrabs > 0)
            AddParticularScrap(ScrapType.GRENADE, (int) scrapInventory[(int) ScrapType.GRENADE][0]);
    }

    private void AddParticularScrap(ScrapType scraptype, int meshindex)
    {
        Vector3 spawnposition = ScrapSlots[_attachedScraps.Count].position;
        GameObject scrap = FindObjectOfType<RessourceManagement>().PossibleScrabPrefabs[(int) scraptype];
        GameObject scrapInstant = Instantiate(scrap, spawnposition, new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
        scrapInstant.GetComponent<Scrap>().SetMesh(meshindex);
        _attachedScraps.Add(scrapInstant);
    }

    public void DestroyTower()
    {
        foreach (GameObject scrap in _attachedScraps)
        {
            if (Random.Range(0.0f, 1.0f) <= Droprate)
            {
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
