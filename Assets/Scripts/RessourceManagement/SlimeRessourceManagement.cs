using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeRessourceManagement : MonoBehaviour {
    public Transform[] ScrapSlots;

    //public bool[] CanCollect;
    //public bool[] CanDrop;
    public float RessourcePossibility = 1;
    public float SuckSpeed = 1.0f;
    public int ScrapThrowFactor = 10;
    public float RotationValue = 1.5f;
    public float MinSpawnedScraps = 0.0f;

    private List<GameObject> _attachedScraps;
    private GameObject[] _possibleScrapPrefabs;

    void Awake () {
        _possibleScrapPrefabs = FindObjectOfType<RessourceManagement>().PossibleScrabPrefabs;
        _attachedScraps = new List<GameObject>();
        //Physics.IgnoreLayerCollision(8, 9);
        InstanstiateScrapsOnSelf();
	}

    // TESTUPDATE
    void Update()
    {
        if (Input.GetKey("p"))
            DropRessources();
     

        //ChildObjekt angucken für bessere Lösung?
        if (_attachedScraps.Count > 0)
            MakeScrapsFollowParent();
        
    }

    public void DropRessources()
    {
        foreach(GameObject scrap in _attachedScraps)
        {
            if (scrap != null)
            {
                scrap.GetComponent<Scrap>().ChangeCollectionState();
                scrap.GetComponent<Rigidbody>().isKinematic = false;
                scrap.GetComponent<Scrap>().ChangeAttachementState();
                FindObjectOfType<RessourceManagement>().ThrowScrapAway(transform, scrap, ScrapThrowFactor);
            }
        }

        _attachedScraps.RemoveRange(0, _attachedScraps.Count);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Scrap") && !other.gameObject.GetComponentInParent<Scrap>().IsCollected  && ScrapSlots.Length > _attachedScraps.Count)
                CollectRessource(other.transform.parent.gameObject); 
    }

    private void CollectRessource(GameObject scrap)
    {
        scrap.GetComponent<Rigidbody>().isKinematic = true;
        scrap.GetComponent<Scrap>().ChangeCollectionState();
        _attachedScraps.Add(scrap);
    }

    private void InstanstiateScrapsOnSelf()
    {
        int numberOfRessourcesOnSelf = Random.Range((int) MinSpawnedScraps, ScrapSlots.Length);
        for (int i = 0; i < numberOfRessourcesOnSelf; i++)
        {
            if (Random.Range(0.0f, 1.0f) <= RessourcePossibility)
            {
                int randomScrap = Random.Range((int)0.0f, _possibleScrapPrefabs.Length);
                Vector3 ressourcePosition = ScrapSlots[i].position;
                _attachedScraps.Add(Instantiate(_possibleScrapPrefabs[randomScrap], ressourcePosition, new Quaternion(0.0f, 0.0f, 0.0f, 0.0f)));
            }
        }
    }

    private void MakeScrapsFollowParent()
    {
        for (int i = 0; i < _attachedScraps.Count; i++)
        {
            if ((GameObject)_attachedScraps[i] != null)
            {
                if (((GameObject)_attachedScraps[i]).GetComponent<Scrap>().AttachedToSlot)
                    ((GameObject)_attachedScraps[i]).transform.position = ScrapSlots[i].position;

                if (!((GameObject)_attachedScraps[i]).GetComponent<Scrap>().AttachedToSlot)
                    SuckScrap((GameObject)_attachedScraps[i], i);
            }
        }
    }

    private void SuckScrap(GameObject gameObject, int slotIndex)
    {
        Vector3 from = ((GameObject)_attachedScraps[slotIndex]).transform.position;
        Vector3 to = ScrapSlots[slotIndex].position;
        float step = SuckSpeed * Time.deltaTime;
        ((GameObject)_attachedScraps[slotIndex]).transform.position = Vector3.MoveTowards(from, to, step);

        if (((GameObject)_attachedScraps[slotIndex]).transform.position == ScrapSlots[slotIndex].position)
            ((GameObject)_attachedScraps[slotIndex]).GetComponent<Scrap>().ChangeAttachementState();

        RotateScrap((GameObject)_attachedScraps[slotIndex]);
    }

    private void SuckScraps()
    {
        for (int i = 0; i < _attachedScraps.Count; i++)
        {
            Vector3 from = ((GameObject)_attachedScraps[i]).transform.position;
            Vector3 to = ScrapSlots[i].position;
            float step = SuckSpeed * Time.deltaTime;
            ((GameObject)_attachedScraps[i]).transform.position = Vector3.MoveTowards(from, to, step);

            if (((GameObject)_attachedScraps[i]).transform.position == ScrapSlots[i].position)
                ((GameObject)_attachedScraps[i]).GetComponent<Scrap>().ChangeAttachementState();

            RotateScrap((GameObject)_attachedScraps[i]);
        }

    }

    private void RotateScrap(GameObject scrap)
    {
        scrap.transform.Rotate(new Vector3(RotationValue, 0, RotationValue));
    }

}
