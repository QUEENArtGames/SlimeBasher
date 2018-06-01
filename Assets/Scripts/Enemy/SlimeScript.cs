using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlimeScript : MonoBehaviour {


	public float _hp;
	public float _damage;
	public Transform _targetLocation;

	private NavMeshAgent _navMeshAgent; 

	// Use this for initialization
	void Start () {

		_navMeshAgent = GetComponent<NavMeshAgent> ();
		setTargetLocation ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter (Collision col)
	{
		//TODO: Synchronize with Tuan projectiles
		if(col.gameObject.tag == "Projectile")
		{
			_hp -= col.gameObject.GetComponent<Projectile> ()._damage;
			isDeath ();
		}
	}

	public void hit(float damage){
		_hp -=damage;
		isDeath();
	}

	public void setTargetLocation(){
		Vector3 destination = _targetLocation.transform.position;
		_navMeshAgent.SetDestination (destination);
	}

	public void isDeath(){
		if(_hp <= 0){
			Destroy (this.gameObject);
		}
	}
}
