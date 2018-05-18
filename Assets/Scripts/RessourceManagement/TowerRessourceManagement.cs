using System.Collections.Generic;
using UnityEngine;


public class TowerRessourceManagement : MonoBehaviour
{
    public Transform[] ScrapSlots;
    public int ScrapThrowFactor;
    public int NeededMeeleScrabs;
    public int NeededBottleScrabs;
    public int NeededGrenadeScrabs;

    private List<GameObject> _attachedScraps = new List<GameObject>();
    private float _droprate;


    void Awake()
    {
        _droprate = FindObjectOfType<RessourceManagement>().TowerScrapDropProbabilityInPercent;
    }

    internal bool ScrapSlotsOnTowerAreFree()
    {
        return _attachedScraps.Count < ScrapSlots.Length;
    }

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

    public void AddNeededScrapOfCertainSubTypeIndex(List<int>[] scrapInventory, int subTypeIndex)
    {
        if (NeededMeeleScrabs > 0)
            AddParticularScrap(ScrapType.MELEE, subTypeIndex);
        if (NeededBottleScrabs > 0)
            AddParticularScrap(ScrapType.BOTTLE, subTypeIndex);
        if (NeededGrenadeScrabs > 0)
            AddParticularScrap(ScrapType.GRENADE, subTypeIndex);

    }

    private void AddParticularScrap(ScrapType scraptype, int subTypeIndex)
    {
        Vector3 spawnposition = ScrapSlots[_attachedScraps.Count].position;
        GameObject scrap = FindObjectOfType<RessourceManagement>().GetScrapPrefabBySubTypeIndex((int) scraptype, subTypeIndex);
        GameObject scrapInstant = Instantiate(scrap, spawnposition, new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
        _attachedScraps.Add(scrapInstant);
    }


    public void DestroyTower()
    {
        foreach (GameObject scrapObject in _attachedScraps)
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

        _attachedScraps.RemoveRange(0, _attachedScraps.Count);
    }
}
