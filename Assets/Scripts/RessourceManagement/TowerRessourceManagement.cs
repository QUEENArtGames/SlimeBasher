using System.Collections.Generic;
using UnityEngine;


public class TowerRessourceManagement : MonoBehaviour
{
    public Transform[] ScrapSlots;
    public int ScrapThrowFactor;
    public int NeededMeeleScraps;
    public int NeededBottleScraps;
    public int NeededGrenadeScraps;
    public int NeededClassicScraps;

    private List<GameObject> _attachedScraps = new List<GameObject>();
    private float _droprate;
    private RessourceManagement _ressourceManagement;

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
        _ressourceManagement = FindObjectOfType<RessourceManagement>();
        _droprate = _ressourceManagement.TowerScrapDropProbabilityInPercent;
    }

    internal bool ScrapSlotsOnTowerAreFree()
    {
        return AttachedScraps.Count < ScrapSlots.Length;
    }

    public GameObject AddNeededScrap(List<int>[] scrapInventory)
    {
        if (NeededMeeleScraps > 0)
            return AddParticularScrap(ScrapType.MELEE, (int) scrapInventory[(int) ScrapType.MELEE][0]);
        if (NeededBottleScraps > 0)
            return AddParticularScrap(ScrapType.BOTTLE, (int) scrapInventory[(int) ScrapType.BOTTLE][0]);
        if (NeededGrenadeScraps > 0)
            return AddParticularScrap(ScrapType.GRENADE, (int) scrapInventory[(int) ScrapType.GRENADE][0]);

        return null;
    }

    public void AddNeededScrapOfCertainSubTypeIndex(List<int>[] scrapInventory, int subTypeIndex)
    {
        if (NeededMeeleScraps > 0)
            AddParticularScrap(ScrapType.MELEE, subTypeIndex);
        if (NeededBottleScraps > 0)
            AddParticularScrap(ScrapType.BOTTLE, subTypeIndex);
        if (NeededGrenadeScraps > 0)
            AddParticularScrap(ScrapType.GRENADE, subTypeIndex);

    }

    private GameObject AddParticularScrap(ScrapType scraptype, int subTypeIndex)
    {
        Vector3 slotPosition = ScrapSlots[AttachedScraps.Count].position;
        GameObject scrap = _ressourceManagement.GetScrapPrefabBySubTypeIndex((int) scraptype, subTypeIndex);
        GameObject scrapInstant = Instantiate(scrap, slotPosition, ScrapSlots[AttachedScraps.Count].rotation);
        Vector3 pivotPosition = ScrapSlots[AttachedScraps.Count].rotation * scrap.GetComponent<Scrap>().TowerAttachementPivot.position;
        scrapInstant.transform.position -= pivotPosition;
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
                scrap.ChangeCollectionState();
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
