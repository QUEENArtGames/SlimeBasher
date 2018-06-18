using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts{
	

public class SlimeScript : MonoBehaviour {


	public float _hitpoints;
	public float _damage;
	public float _attackSpeed;
	public float _aggroRange;
	public bool _hasAggro;

	private Transform _finalDestination;
	private TowerPlacement _towerplacement;

	private NavMeshAgent _navMeshAgent;
	private GameObject _tmpTarget;

	private float _nextAttack = 0;
	// Use this for initialization
	void Start () {

			//_towerplacement = GameObject.Find("GameController").transform.GetComponent<TowerPlacement>();
			//_finalDestination = FindObjectOfType<Game>().FinalDestination;


			_navMeshAgent = GetComponent<NavMeshAgent> ();
			_navMeshAgent.stoppingDistance = 2;
			SetTargetLocation ();
		}
		
		// Update is called once per frame
		void Update () {
			if (_hasAggro) {
				checkAggro ();
				if (_tmpTarget != null) {
					AttackTower ();
				}
			}
		}
			

		public void checkAggro(){

			float shortestDistance = _aggroRange+1;
			bool newTarget = false;
			foreach (GameObject tower in _towerplacement._placedTowers) {
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
				SetTargetLocation ();
			}

		}

		public void AttackTower(){
			if (Time.time > _nextAttack) {
				Tower tower = _tmpTarget.transform.GetComponent<Tower>();
				tower.TakeDamage(_damage);

				_nextAttack = Time.time + _attackSpeed;
			}
		}
			
		public void TakeDamage(int damage)
		{
			_hitpoints -= damage;
		}

		public void SetTargetLocation(){
			Vector3 destination = _finalDestination.position;
			_navMeshAgent.SetDestination (destination);
		}
			

		public void Kill()
		{
			Destroy(this.gameObject);
			GetComponent<SlimeRessourceManagement>().DropRessources();
		}

	}
}