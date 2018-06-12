using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlimeScript : MonoBehaviour {


	public float _hp;
	public float _damage;
	public float _attackSpeed;
	public Transform _targetLocation;
	public float _aggroRange;

	public TowerManagerTestScript towerManagerTestScript;

	private NavMeshAgent _navMeshAgent;
	private GameObject _tmpTarget;

	private float _nextAttack = 0;
	// Use this for initialization
	void Start () {


		_navMeshAgent = GetComponent<NavMeshAgent> ();
		_navMeshAgent.stoppingDistance = 2;
		setTargetLocation ();
	}
	
	// Update is called once per frame
	void Update () {
		checkAggro ();

		if (_tmpTarget != null) {
			attackTower ();
		}
	}
		

	public void checkAggro(){

		float shortestDistance = _aggroRange+1;
		bool newTarget = false;
		foreach (GameObject tower in towerManagerTestScript.towers) {
			float distance = Vector3.Distance (transform.position, tower.transform.position);
			if (distance < _aggroRange) {

				if(newTarget == false || distance < shortestDistance){
					newTarget = true;
					shortestDistance = distance;
					_tmpTarget = tower;
				}
			}
		}

		if (newTarget) {
			_navMeshAgent.SetDestination (_tmpTarget.transform.position);

		} else if(_tmpTarget != null){
			_tmpTarget = null;
			setTargetLocation ();
		}

	}

	public void attackTower(){
		if (Time.time > _nextAttack) {
			Tower tower = _tmpTarget.transform.GetComponent<Tower>();
			tower.attacked(_damage);

			_nextAttack = Time.time + _attackSpeed;
		}
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
