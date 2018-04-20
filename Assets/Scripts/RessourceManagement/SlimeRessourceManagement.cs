﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeRessourceManagement : MonoBehaviour {
    
    //Collect und Drop Script trennen?

    public Transform[] ScrapSlots;

    //public bool[] CanCollect;
    //public bool[] CanDrop;
    public float RessourcePossibility = 1;
    public float suckSpeed = 1.0f;
    public int scrapThrowFactor = 10;
    public int RotationValue = 1;

    private ArrayList _attachedScraps;
    private GameObject[] _possibleScrapPrefabs;

    // Use this for initialization
    void Awake () {
        _possibleScrapPrefabs = FindObjectOfType<RessourceManagement>().PossibleScrabPrefabs;
        _attachedScraps = new ArrayList();
        Physics.IgnoreLayerCollision(0, 10);
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
            scrap.GetComponent<Scrap>().ChangeCollectionState();
            scrap.GetComponent<Rigidbody>().isKinematic = false;
            scrap.GetComponent<Scrap>().ChangeAttachementState();
            ThrowScrapAway(scrap);
        }

        _attachedScraps.RemoveRange(0, _attachedScraps.Count);

    }

    private void ThrowScrapAway(GameObject scrap)
    {
        Vector3 forceVector = (scrap.transform.position - transform.position) * scrapThrowFactor;
        scrap.GetComponent<Rigidbody>().AddForce(forceVector, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.GetComponentInParent<Scrap>().IsCollected && other.transform.parent.CompareTag("Scrap") && ScrapSlots.Length > _attachedScraps.Count)
                CollectRessource(other.transform.parent.gameObject); 
    }

    private void CollectRessource(GameObject scrap)
    {
        Transform emptySlot = ScrapSlots[_attachedScraps.Count];
        scrap.GetComponent<Rigidbody>().isKinematic = true;
        scrap.GetComponent<Scrap>().ChangeCollectionState();
        _attachedScraps.Add(scrap);
    }

    private void InstanstiateScrapsOnSelf()
    {
        int numberOfRessourcesOnSelf = ScrapSlots.Length;
        for (int i = 0; i < numberOfRessourcesOnSelf; i++)
        {
            if (Random.Range(0.0f, 1.0f) <= RessourcePossibility)
            {
                int randomScrap = Random.Range((int)0.0f, _possibleScrapPrefabs.Length);
                Vector3 ressourcePosition = ScrapSlots[i].position;
                _attachedScraps.Add(Instantiate(_possibleScrapPrefabs[i], ressourcePosition, new Quaternion(0.0f, 0.0f, 0.0f, 0.0f)));
            }
        }
    }

    private void MakeScrapsFollowParent()
    {
        for (int i = 0; i < _attachedScraps.Count; i++)
        {
            if (((GameObject)_attachedScraps[i]).GetComponent<Scrap>().AttachedToSlot)
                ((GameObject)_attachedScraps[i]).transform.position = ScrapSlots[i].position;

            if (!((GameObject)_attachedScraps[i]).GetComponent<Scrap>().AttachedToSlot)
                SuckScraps();
            


        }
    }

    
    private void SuckScraps()
    {
        for (int i = 0; i < _attachedScraps.Count; i++)
        {
            Vector3 from = ((GameObject)_attachedScraps[i]).transform.position;
            Vector3 to = ScrapSlots[i].position;
            float step = suckSpeed * Time.deltaTime;
            ((GameObject)_attachedScraps[i]).transform.position = Vector3.MoveTowards(from, to, step);
            RotateScrap((GameObject)_attachedScraps[i]);

            if (((GameObject)_attachedScraps[i]).transform.position == ScrapSlots[i].position)
                ((GameObject)_attachedScraps[i]).GetComponent<Scrap>().ChangeAttachementState();
        }

    }

    private void RotateScrap(GameObject scrap)
    {
        scrap.transform.Rotate(new Vector3(RotationValue, 0, RotationValue));
    }

}