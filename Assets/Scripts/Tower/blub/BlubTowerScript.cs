using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlubTowerScript : MonoBehaviour {

	//public Transform Target;

	public GameObject projec;

	public float angle;

	// Use this for initialization
	void Start () {
		StartCoroutine(BlubProjectile());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator BlubProjectile(){
		
		yield return new WaitForSeconds(4.0f);

		StartCoroutine(BlubProjectile());
		GameObject bullet = Instantiate(projec) as GameObject;
		//print(transform.eulerAngles.y);

		bullet.GetComponent<blubProjektScript>().angle = transform.eulerAngles.y;
		Physics.IgnoreCollision(bullet.GetComponent<Collider>(), GetComponent<Collider>());

		bullet.transform.position = transform.position + new Vector3(0f, 0.0f, 0);


	}
}
