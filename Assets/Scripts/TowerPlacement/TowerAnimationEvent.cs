using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAnimationEvent : MonoBehaviour {

    private GameObject newScrap;

    public GameObject NewScrap
    {
        get
        {
            return newScrap;
        }

        set
        {
            newScrap = value;
        }
    }

    public void AddScrapAfterAnimation()
    {
        NewScrap.SetActive(true);
    }


	
}
