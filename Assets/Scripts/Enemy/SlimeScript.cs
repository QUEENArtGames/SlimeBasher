using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class SlimeScript : MonoBehaviour
    {
        public SlimeType _type;
        public float _hitpoints;
        public Slider _healthSlider;
        public float _damage;
        public float _attackSpeed;
        public float _towerAggroRange;
        public float _playerAggroRange;
        public float damageByPlayer;

        private Transform _finalDestination;
        private TowerPlacement _towerplacement;
        private GameObject _player;
        private List<Color> _standardColor = new List<Color>();
        private SlimeAudio _slimeAudio;

        private NavMeshAgent _navMeshAgent;
        private GameObject _tmpTarget;
        private Player _playerdummy;
        private float _maxHitpoints;

        private GasSlimeTransformScript[] _transformEffects;

        private float _nextAttack = 0;
        private bool _blinkReady = true;
        // Use this for initialization
        void Start()
        {
            _slimeAudio = GetComponent<SlimeAudio>();
            _maxHitpoints = _hitpoints;
            if (_type == SlimeType.Gas)
                _transformEffects = this.GetComponents<GasSlimeTransformScript>();
            _towerplacement = GameObject.FindObjectOfType<TowerPlacement>();//("GameController").transform.GetComponent<TowerPlacement>();
            _finalDestination = FindObjectOfType<Game>()._finalDestination;
            _player = GameObject.Find("Main_Character");
            _playerdummy = _player.GetComponent<Player>();
            if (_type != SlimeType.Gas)
            {
                _navMeshAgent = GetComponent<NavMeshAgent>();
                _navMeshAgent.stoppingDistance = 1;
                SetTargetLocation();
            }

            foreach (var rend in GetComponentsInChildren<Renderer>())
            {
                _standardColor.Add(rend.material.color);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (SlimeType.Gas != _type)
            {
                if (CheckPlayerAggro())
                {
                    AttackPlayer();
                }
                else if (CheckTowerAggro())
                {
                    AttackTower();
                }
                else
                {

                    SetTargetLocation();
                }
            }

        }

        void transformSlime()
        {

            foreach (GasSlimeTransformScript transformEffect in _transformEffects)
            {
                transformEffect.setHpPercent((100 / _maxHitpoints) * _hitpoints);
            }
        }

        public bool CheckPlayerAggro()
        {
            if (Vector3.Distance(transform.position, _player.transform.position) <= _playerAggroRange && _playerdummy._playerHealth > 0)
            {
                _navMeshAgent.SetDestination(_player.transform.position);
                return true;
            }
            return false;
        }

        public bool CheckTowerAggro()
        {
            float shortestDistance = _towerAggroRange + 1;
            bool newTarget = false;
            foreach (GameObject tower in _towerplacement._placedTowers)
            {
                float distance = Vector3.Distance(transform.position, tower.transform.position);
                if (distance < _towerAggroRange)
                {

                    if (newTarget == false || distance < shortestDistance)
                    {
                        newTarget = true;
                        shortestDistance = distance;
                        _tmpTarget = tower;
                    }
                }
            }

            if (newTarget)
            {
                _navMeshAgent.SetDestination(_tmpTarget.transform.position);
                return true;
            }

            _tmpTarget = null;
            return false;
        }

        public void AttackPlayer()
        {
            if (Time.time > _nextAttack)
            {
                _playerdummy.Damage((int) _damage);

                _nextAttack = Time.time + _attackSpeed;
            }
        }

        public void AttackTower()
        {
            if (Time.time > _nextAttack)
            {
                Tower tower = _tmpTarget.transform.GetComponent<Tower>();
                tower.TakeDamage(_damage);

                _nextAttack = Time.time + _attackSpeed;
            }
        }

        public void TakeDamage(float damage)
        {
            _hitpoints -= damage;
            _healthSlider.value = _hitpoints;

            if (_type == SlimeType.Gas)
            {
                transformSlime();
            }
            else
            {
                if (_blinkReady)
                    StartCoroutine(DamageFlash());
            }
        }

        public void SetTargetLocation()
        {
            Vector3 destination = _finalDestination.position;
            _navMeshAgent.SetDestination(destination);
        }

        public void Kill()
        {
            Destroy(this.gameObject);
            GetComponent<SlimeRessourceManagement>().DropRessources();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag.Equals("PlayerWeapon"))
            {
                this.TakeDamage(damageByPlayer);
                _slimeAudio.PlayDamageClip();
            }
        }

        IEnumerator DamageFlash()
        {
            _blinkReady = false;

            foreach (var rend in GetComponentsInChildren<Renderer>())
            {
                rend.material.color = Color.red;
            }

            yield return new WaitForSeconds(.2f);

            for (int i = 0; i < GetComponentsInChildren<Renderer>().Length; i++)
            {
                GetComponentsInChildren<Renderer>()[i].material.color = _standardColor[i];
            }

            yield return new WaitForSeconds(.5f);

            _blinkReady = true;
        }
    }
}