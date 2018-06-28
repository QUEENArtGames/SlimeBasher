using UnityEngine;

public class TowerAnimationEvent : MonoBehaviour
{

    private GameObject _newScrap;

    public GameObject NewScrap
    {
        get
        {
            return _newScrap;
        }

        set
        {
            _newScrap = value;
        }
    }

    public void ActivateScrapAfterAnimation()
    {
        _newScrap.SetActive(true);
    }

}

