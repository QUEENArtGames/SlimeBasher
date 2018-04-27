using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrap : MonoBehaviour {

    private ScrapType _scrapType;
    private int _meshnumber;
    private bool collected = true;
    private bool inPosition = true;

    public int ScrapTypeAsInt;
    public Mesh[] possibleMeshes;

    void Awake() {
        _meshnumber = ((int)Random.Range(0.0f, possibleMeshes.Length));
        SetMesh(_meshnumber);
        _scrapType = (ScrapType) ScrapTypeAsInt;
    }

    public ScrapType Type
    {
        get
        {
            return _scrapType;
        }

        set
        {
            _scrapType = value;
        }

    }
    
    public bool IsCollected
    {
        get
        {
            return collected;
        }
    }

    public void ChangeCollectionState()
    {
        collected = !collected;
    }

    public int MeshIndex { 
        get{
            return _meshnumber;
        }
    }

    public bool AttachedToSlot
    {
        get
        {
            return inPosition;
        }
    }

    public void ChangeAttachementState()
    {
        inPosition = !AttachedToSlot;
    }

    public void SetMesh(int meshnumber)
    {
        _meshnumber = meshnumber;
        gameObject.GetComponentInChildren<MeshFilter>().mesh = Instantiate(possibleMeshes[_meshnumber]);
    }

    
}
