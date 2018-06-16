using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleTowerScript : MonoBehaviour {

	//public Transform Target;

	public GameObject projectile;

    public float spawnDelay = 0f;

	public float angle;

	void Start () {
        //StartCoroutine(BubbleProjectile());
	}
	
	void Update () {
		
	}

	IEnumerator BubbleProjectile(){

        yield return new WaitForSeconds(spawnDelay);

		//StartCoroutine(BubbleProjectile());
		GameObject bubbleProjectile = Instantiate(projectile) as GameObject;
        //print(transform.eulerAngles.y);

        bubbleProjectile.GetComponent<BubbleProjectileScript>().angle = transform.eulerAngles.y;
		Physics.IgnoreCollision(bubbleProjectile.GetComponent<Collider>(), GetComponent<Collider>());

        //bubbleProjectile.transform.position = transform.position + new Vector3(0.1f, 0.5f, 0f);
        bubbleProjectile.GetComponent<BubbleProjectileScript>().spawnpoint = transform.GetChild(1).transform;

    }

}
