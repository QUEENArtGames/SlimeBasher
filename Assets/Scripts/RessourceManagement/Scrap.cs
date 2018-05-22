using UnityEngine;


public class Scrap : MonoBehaviour
{
    public ScrapType ScrapType;
    public int SubCategoryIndex;

    public bool _collected = true;
    public bool _inPosition = true;
    private Rigidbody _rigidbody;


    void Awake()
    {
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
        _inPosition = !AttachedToSlot;
    }

    public void ThrowScrapAway(Vector3 position, Vector3 scrapPosition, int ScrapThrowFactor)
    {
        Vector3 forceVector = (scrapPosition - position) * ScrapThrowFactor;
        _rigidbody.AddForce(forceVector, ForceMode.Impulse);
        _rigidbody.AddTorque(new Vector3(Random.Range(0.0f, ScrapThrowFactor), Random.Range(0.0f, ScrapThrowFactor), Random.Range(0.0f, ScrapThrowFactor)));
    }
}
