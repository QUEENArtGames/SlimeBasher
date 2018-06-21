using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts{
public class PuddleScript : MonoBehaviour {

	public float _damage=10;
	public float _livetime =5;

	private float currundtime =0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(currundtime >_livetime){
			Destroy(gameObject);
		}else
		{
			currundtime+= Time.deltaTime;
		}
	}
	void OnCollisionEnter(Collider other) {
        Debug.Log(other.name);
    }

}
}
