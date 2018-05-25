using System.Collections.Generic;
using UnityEngine;


public class TowerRessourceManagement : MonoBehaviour
{
    public Transform[] ScrapSlots;
    public int ScrapThrowFactor;
    public int NeededMeeleScrabs;
    public int NeededBottleScrabs;
    public int NeededGrenadeScrabs;
    public int NeededClassicScrabs;

    private List<GameObject> _attachedScraps = new List<GameObject>();
    private float _droprate;

    public List<GameObject> AttachedScraps
    {
        get
        {
            return _attachedScraps;
        }

        set
        {
            _attachedScraps = value;
        }
    }

    void Awake()
    {
        _droprate = FindObjectOfType<RessourceManagement>().TowerScrapDropProbabilityInPercent;
    }

    internal bool ScrapSlotsOnTowerAreFree()
    {
        return AttachedScraps.Count < ScrapSlots.Length;
    }

    /*public void AddAllNeededScraps(List<int>[] scrapInventory)
    {
        for (int i = 0; i < NeededMeeleScrabs; i++)
            AddParticularScrap(ScrapType.MELEE, (int) scrapInventory[(int) ScrapType.MELEE][0]);

        for (int i = 0; i < NeededBottleScrabs; i++)
            AddParticularScrap(ScrapType.BOTTLE, (int) scrapInventory[(int) ScrapType.BOTTLE][0]);

        for (int i = 0; i < NeededGrenadeScrabs; i++)
            AddParticularScrap(ScrapType.GRENADE, (int) scrapInventory[(int) ScrapType.GRENADE][0]);
    }*/

    public GameObject AddNeededScrap(List<int>[] scrapInventory)
    {
        if (NeededMeeleScrabs > 0)
            return AddParticularScrap(ScrapType.MELEE, (int) scrapInventory[(int) ScrapType.MELEE][0]);
        if (NeededBottleScrabs > 0)
            return AddParticularScrap(ScrapType.BOTTLE, (int) scrapInventory[(int) ScrapType.BOTTLE][0]);
        if (NeededGrenadeScrabs > 0)
            return AddParticularScrap(ScrapType.GRENADE, (int) scrapInventory[(int) ScrapType.GRENADE][0]);

        return null;
    }

    public void AddNeededScrapOfCertainSubTypeIndex(List<int>[] scrapInventory, int subTypeIndex)
    {
        if (NeededMeeleScrabs > 0)
            AddParticularScrap(ScrapType.MELEE, subTypeIndex);
        if (NeededBottleScrabs > 0)
            AddParticularScrap(ScrapType.BOTTLE, subTypeIndex);
        if (NeededGrenadeScrabs > 0)
            AddParticularScrap(ScrapType.GRENADE, subTypeIndex);

    }

    private GameObject AddParticularScrap(ScrapType scraptype, int subTypeIndex)
    {
        Vector3 spawnposition = ScrapSlots[AttachedScraps.Count].position;
        GameObject scrap = FindObjectOfType<RessourceManagement>().GetScrapPrefabBySubTypeIndex((int) scraptype, subTypeIndex);
        GameObject scrapInstant = Instantiate(scrap, spawnposition, ScrapSlots[AttachedScraps.Count].rotation);
        AttachedScraps.Add(scrapInstant);
        return scrapInstant;
    }


    public void DestroyTower()
    {
        foreach (GameObject scrapObject in AttachedScraps)
        {
            if (Random.Range(0.0f, 100.0f) < _droprate)
            {
                Scrap scrap = scrapObject.GetComponent<Scrap>();
                scrapObject.GetComponent<Rigidbody>().isKinematic = false;
                scrap.ChangeAttachementState();
                scrap.GetComponent<Scrap>().ChangeCollectionState();
                scrap.ThrowScrapAway(transform.position, scrap.transform.position, ScrapThrowFactor);
            }
            else
            {
                Destroy(scrapObject);
            }
        }

        AttachedScraps.RemoveRange(0, AttachedScraps.Count);
    }
}
