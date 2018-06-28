using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts{
	

public class SlimeScript : MonoBehaviour {


	public float _hitpoints;
	public float _damage;
	public float _attackSpeed;
	public float _towerAggroRange;
	public float _playerAggroRange;


	private Transform _finalDestination;
	private TowerPlacement _towerplacement;
	private GameObject _player;

	private NavMeshAgent _navMeshAgent;
	private GameObject _tmpTarget;

	private float _nextAttack = 0;
	// Use this for initialization
	void Start () {

			_towerplacement = GameObject.FindObjectOfType<TowerPlacement>();//("GameController").transform.GetComponent<TowerPlacement>();
			_finalDestination = FindObjectOfType<Game>().FinalDestination;
			_player = GameObject.Find ("MainCharacter");


			_navMeshAgent = GetComponent<NavMeshAgent> ();
			_navMeshAgent.stoppingDistance = 1;
			SetTargetLocation ();
		}
		
		// Update is called once per frame
		void Update () {

			if(checkPlayerAggro()){
				attackPlayer ();
			}else if(checkTowerAggro()){
				AttackTower ();
			}else{
				SetTargetLocation();
			}

		}
			
		public bool checkPlayerAggro(){
			if (Vector3.Distance (transform.position, _player.transform.position) <= _playerAggroRange && _player.GetComponent<PlayerDummy>()._playerHealth > 0) {
				_navMeshAgent.SetDestination (_player.transform.position);
				return true;
			}
			return false;
		}

		public bool checkTowerAggro(){
			float shortestDistance = _towerAggroRange+1;
			bool newTarget = false;
			foreach (GameObject tower in _towerplacement._placedTowers) {
				float distance = Vector3.Distance (transform.position, tower.transform.position);
				if (distance < _towerAggroRange) {

					if(newTarget == false || distance < shortestDistance){
						newTarget = true;
						shortestDistance = distance;
						_tmpTarget = tower;
					}
				}
			}

			if (newTarget) {
				_navMeshAgent.SetDestination (_tmpTarget.transform.position);
				return true;
			}

			_tmpTarget = null;
			return false;
		}

		public void attackPlayer(){
			if (Time.time > _nextAttack) {
				PlayerDummy playerDummy = _player.transform.GetComponent<PlayerDummy> ();
                playerDummy.Damage((int)_damage);

				_nextAttack = Time.time + _attackSpeed;
			}
		}

		public void AttackTower(){
			if (Time.time > _nextAttack) {
				Tower tower = _tmpTarget.transform.GetComponent<Tower>();
				tower.TakeDamage(_damage);

				_nextAttack = Time.time + _attackSpeed;
			}
		}
			
		public void TakeDamage(float damage)
		{
			_hitpoints -= damage;
		}

		public void SetTargetLocation(){
			Vector3 destination = _finalDestination.position;
			_navMeshAgent.SetDestination (destination);
		}
			

		public void Kill()
		{
            GetComponent<SlimeAudio>().PlayDeathClip();
			Destroy(this.gameObject, 0.5f);
			GetComponent<SlimeRessourceManagement>().DropRessources();
		}

	}
}