using System;
using System.Collections.Generic;
using UnityEngine;


public class TowerRessourceManagement : MonoBehaviour
{
    public Transform[] ScrapSlots;
    public int ScrapThrowFactor;
    public bool NeedsMeeleScraps;
    public bool NeedsBottleScraps;
    public bool NeedsGrenadeScraps;
    public int NeededClassicScraps;

    private List<GameObject> _attachedScraps = new List<GameObject>();
    private float _droprate;
    private RessourceManagement _ressourceManagement;
    private bool[] _neededScraps = new bool[Enum.GetNames(typeof(ScrapType)).Length];

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

    public bool[] NeededScraps
    {
        get
        {
            return _neededScraps;
        }
    }

    void Awake()
    {
        _ressourceManagement = FindObjectOfType<RessourceManagement>();
        _droprate = _ressourceManagement.TowerScrapDropProbabilityInPercent;
        _neededScraps[(int)ScrapType.MELEE] = NeedsMeeleScraps;
        _neededScraps[(int)ScrapType.BOTTLE] = NeedsBottleScraps;
        _neededScraps[(int)ScrapType.GRENADE] = NeedsGrenadeScraps;
        Debug.Log(NeededScraps.Length);
    }

    internal bool ScrapSlotsOnTowerAreFree()
    {
        return AttachedScraps.Count < ScrapSlots.Length;
    }

    public GameObject AddNeededScrap(List<int>[] scrapInventory)
    {
        for(int scrapTypeIndex = 0; scrapTypeIndex < _neededScraps.Length; scrapTypeIndex++)
        {
            if(_neededScraps[scrapTypeIndex])
                return AddParticularScrap((ScrapType) scrapTypeIndex, scrapInventory[scrapTypeIndex][0]);
        }
        return null;
    }

    public void AddNeededScrapOfCertainSubTypeIndex(List<int>[] scrapInventory, int subTypeIndex)
    {
        for (int scrapTypeIndex = 0; scrapTypeIndex < _neededScraps.Length; scrapTypeIndex++)
        {
            if (_neededScraps[scrapTypeIndex])
                AddParticularScrap((ScrapType)scrapTypeIndex, subTypeIndex);
        }
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
            if (UnityEngine.Random.Range(0.0f, 100.0f) < _droprate)
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
