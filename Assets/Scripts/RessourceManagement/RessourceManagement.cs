using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RessourceManagement : MonoBehaviour {

    public GameObject[] PossibleScrabPrefabs;

    public void ThrowScrapAway(Transform trans, GameObject scrap, int ScrapThrowFactor)
    {
        Vector3 forceVector = (scrap.transform.position - trans.position) * ScrapThrowFactor;
        scrap.GetComponent<Rigidbody>().AddForce(forceVector, ForceMode.Impulse);
        scrap.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(0.0f, ScrapThrowFactor), Random.Range(0.0f, ScrapThrowFactor), Random.Range(0.0f, ScrapThrowFactor)));
    }
}
