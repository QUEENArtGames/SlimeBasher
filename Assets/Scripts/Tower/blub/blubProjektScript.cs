using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blubProjektScript : MonoBehaviour {


	public bool isColliderEnabled = true;
	public float angle;

	public float range= 5;

	public float maxScale=2;

	public float bounce =1;

	public bool isbounce =false;

	private Vector3 startPosition;


	// Use this for initialization
	void Start () {
		transform.eulerAngles = new Vector3(
			transform.eulerAngles.x,
			transform.eulerAngles.y + angle,
			transform.eulerAngles.z
		);
		startPosition = transform.position;

	}
	
	// Update is called once per frame
	void Update () {

		if(transform.localScale.x <=maxScale){
			transform.localScale += new Vector3(0.01f, 0.01f, 0.01f);
		}else
		{
			transform.Translate(Vector3.forward * Time.deltaTime);
		}
		
		float dist = Vector3.Distance(startPosition, transform.position);
		//print("Distance to other: " + dist);
		if(dist >range){
			Debug.Log(dist);
			Destroy(gameObject);
		}
		
		  //Debug.Log(startPosition.x-transform);
		
		
	

	}
}
