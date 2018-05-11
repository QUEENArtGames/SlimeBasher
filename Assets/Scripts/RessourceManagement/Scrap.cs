using UnityEngine;


public class Scrap : MonoBehaviour
{
    public ScrapType ScrapType;
    public Mesh[] PossibleMeshes;

    private int _meshnumber;
    private bool _collected = true;
    private bool _inPosition = true;
    private Rigidbody _rigidbody;


    void Awake()
    {
        _meshnumber = ((int) Random.Range(0.0f, PossibleMeshes.Length));
        _rigidbody = GetComponent<Rigidbody>();
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

    public int MeshIndex
    {
        get
        {
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

    public void ThrowScrapAway(Vector3 position, Vector3 scrapPosition, int ScrapThrowFactor)
    {
        Vector3 forceVector = (scrapPosition - position) * ScrapThrowFactor;
        _rigidbody.AddForce(forceVector, ForceMode.Impulse);
        _rigidbody.AddTorque(new Vector3(Random.Range(0.0f, ScrapThrowFactor), Random.Range(0.0f, ScrapThrowFactor), Random.Range(0.0f, ScrapThrowFactor)));
    }
}
