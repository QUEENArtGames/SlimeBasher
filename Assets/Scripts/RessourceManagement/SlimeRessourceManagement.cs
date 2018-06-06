using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;


public class SlimeRessourceManagement : MonoBehaviour
{
    public Transform[] ScrapSlots;

    public bool SpawnsWithScraps = true;
    public float SuckSpeed = 1.0f;
    public int ScrapThrowFactor = 10;
    public float RotationValue = 1.5f;
    public float MinSpawnedScraps = 0.0f;


    private List<GameObject> _attachedScraps;
    private GameObject[] _possibleScrapPrefabs;
    private RessourceManagement _ressourceManagement;


    void Awake()
    {
        _ressourceManagement = FindObjectOfType<RessourceManagement>();
        _attachedScraps = new List<GameObject>();
        InstanstiateScrapsOnSelf();
        //Physics.IgnoreLayerCollision(8, 9);
        
    }

    // TESTUPDATE
    void Update()
    {
        if(Input.GetKeyDown("i"))
        {
            InstanstiateScrapsOnSelf();
        }
           
        if (Input.GetKeyDown("p"))
            GetComponent<EnemyDummy>().Hitpoints = 0;
        
        //ChildObjekt angucken für bessere Lösung?
        if (_attachedScraps.Count > 0)
            MakeScrapsFollowParent();
    }

    //TESTEREI
    /*private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Scrap") && !other.gameObject.GetComponent<Scrap>().IsCollected && ScrapSlots.Length > _attachedScraps.Count)
            CollectRessource(other.transform.gameObject);
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Scrap") && !other.gameObject.GetComponent<Scrap>().IsCollected && ScrapSlots.Length > _attachedScraps.Count)
            CollectRessource(other.transform.gameObject);
    }

    public void DropRessources()
    {
        foreach (GameObject scrapObject in _attachedScraps)
        {
            if (scrapObject != null)
            {
                Scrap scrap = scrapObject.GetComponent<Scrap>();
                scrap.ChangeCollectionState();
                scrapObject.GetComponent<Rigidbody>().isKinematic = false;
                scrap.ChangeAttachementState();
                scrap.ThrowScrapAway(transform.position, scrap.transform.position, ScrapThrowFactor);
            }
        }

        _attachedScraps.RemoveRange(0, _attachedScraps.Count);
    }

    private void CollectRessource(GameObject scrap)
    {
        scrap.GetComponent<Rigidbody>().isKinematic = true;
        scrap.GetComponent<Scrap>().ChangeCollectionState();
        _attachedScraps.Add(scrap);
    }

    private void InstanstiateScrapsOnSelf()
    {
        if (_attachedScraps.Count != 0 || !SpawnsWithScraps)
            return;

        for (int i = 0; i < ScrapSlots.Length; i++)
        {
            if (Random.Range(0.0f, 100.0f) <= FindObjectOfType<RessourceManagement>().ScrapSpawnProbabilityOnSlimesInPercent)
            {
                Vector3 ressourcePosition = ScrapSlots[i].position;
                _attachedScraps.Add(Instantiate(_ressourceManagement.GetRandomScrapFromPool(), ressourcePosition, new Quaternion(Random.Range(0.0f, 180f), 0.0f, Random.Range(0.0f, 180.0f), Random.Range(0.0f, 180.0f))));
            }
        }
    }

    private void MakeScrapsFollowParent()
    {
        for (int i = 0; i < _attachedScraps.Count; i++)
        {
            if ((GameObject) _attachedScraps[i] != null)
            {
                if (((GameObject) _attachedScraps[i]).GetComponent<Scrap>().AttachedToSlot)
                    ((GameObject) _attachedScraps[i]).transform.position = ScrapSlots[i].position;

                if (!((GameObject) _attachedScraps[i]).GetComponent<Scrap>().AttachedToSlot)
                    SuckScrap((GameObject) _attachedScraps[i], i);
            }
        }
    }

    private void SuckScrap(GameObject gameObject, int slotIndex)
    {
        Vector3 from = ((GameObject) _attachedScraps[slotIndex]).transform.position;
        Vector3 to = ScrapSlots[slotIndex].position;
        float step = SuckSpeed * Time.deltaTime;
        ((GameObject) _attachedScraps[slotIndex]).transform.position = Vector3.MoveTowards(from, to, step);

        if (((GameObject) _attachedScraps[slotIndex]).transform.position == ScrapSlots[slotIndex].position)
            ((GameObject) _attachedScraps[slotIndex]).GetComponent<Scrap>().ChangeAttachementState();

        RotateScrap((GameObject) _attachedScraps[slotIndex]);
    }

    private void RotateScrap(GameObject scrap)
    {
        scrap.transform.Rotate(new Vector3(RotationValue, 0, RotationValue));
    }
}
