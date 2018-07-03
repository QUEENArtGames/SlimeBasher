using UnityEngine;


public class Scrap : MonoBehaviour
{
    public ScrapType ScrapType;
    public int SubCategoryIndex;
    public Transform TowerAttachementPivot;
    private ParticleSystem _firstgroundhitparticle;

    private bool _collected = true;
    private bool _inPosition = true;
    private bool _wasonground = false;
    private Rigidbody _rigidbody;


    void Awake()
    {
        _firstgroundhitparticle = GetComponent<ParticleSystem>();
        _rigidbody = GetComponent<Rigidbody>();
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

    public bool AttachedToSlot
    {
        get
        {
            return _inPosition;
        }
    }

    public void ChangeAttachementState()
    {
        if (!AttachedToSlot)
            _wasonground = false;

        _inPosition = !AttachedToSlot;
    }

    public void ThrowScrapAway(Vector3 centerOfObject, Vector3 scrapSlotPosition, int ScrapThrowFactor)
    {
        Vector3 forceVector = (scrapSlotPosition - centerOfObject) * ScrapThrowFactor;
        _rigidbody.AddForce(forceVector, ForceMode.Impulse);
        _rigidbody.AddTorque(new Vector3(Random.Range(0.0f, ScrapThrowFactor), Random.Range(0.0f, ScrapThrowFactor), Random.Range(0.0f, ScrapThrowFactor)));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 8 && !_wasonground)
        {
            Debug.Log("Scrap Traf auf Boden");
            _firstgroundhitparticle.Play();
            _wasonground = true;
        }

    }
}
