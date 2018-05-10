using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrap : MonoBehaviour {

    private int _meshnumber;
    private bool _collected = true;
    private bool _inPosition = true;

    public ScrapType ScrapType;
    public Mesh[] PossibleMeshes;

    void Awake() {
        _meshnumber = ((int)Random.Range(0.0f, PossibleMeshes.Length));
        SetMesh(_meshnumber);
    }

    public ScrapType Type
    {
        get
        {
            return ScrapType;
        }

        set
        {
            ScrapType = value;
        }

    }
    
    public bool IsCollected
    {
        get
        {
            return _collected;
        }
    }

    public void ChangeCollectionState()
    {
        _collected = !_collected;
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
            return _inPosition;
        }
    }

    public void ChangeAttachementState()
    {
        _inPosition = !AttachedToSlot;
    }

    public void SetMesh(int meshnumber)
    {
        _meshnumber = meshnumber;
        gameObject.GetComponentInChildren<MeshFilter>().mesh = Instantiate(PossibleMeshes[_meshnumber]);
    }

    
}
