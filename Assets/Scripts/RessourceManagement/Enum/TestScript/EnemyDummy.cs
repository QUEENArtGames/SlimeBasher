using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts {
    public class EnemyDummy : MonoBehaviour {

        public int Hitpoints = 100;

        private Transform _finalDestination;
        private NavMeshAgent _agent;

        private GameObject _target;

        // Use this for initialization
        void Start() {
            _finalDestination = FindObjectOfType<Game>().FinalDestination;
            _agent = GetComponent<NavMeshAgent>();
            _agent.SetDestination(_finalDestination.position);
            
        }

        // Update is called once per frame
        void Update() {
            if (Hitpoints <= 0)
                Kill();

            if (_target != null)
            {
                if (Mathf.Sqrt((_target.transform.position.x - transform.position.x) * (_target.transform.position.x - transform.position.x) + (_target.transform.position.y - transform.position.y) * (_target.transform.position.y - transform.position.y) + (_target.transform.position.z - transform.position.z) * (_target.transform.position.z - transform.position.z)) <= 1)
                {
                    _target.GetComponent<Tower>().Hitpoints = 0;
                    _target = null;
                    _agent.SetDestination(_finalDestination.position);
                }
            }

        }

        public void TakeDamage(int damage)
        {
            Hitpoints -= damage;
        }

        private void Kill()
        {
            Destroy(this.gameObject);
            GetComponent<SlimeRessourceManagement>().DropRessources();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_target == null)
            {
                if (other.transform.gameObject.CompareTag("Tower"))
                {
                    _agent.SetDestination(other.transform.position);
                    _target = other.transform.gameObject;
                }
            }
               
        }
      
    }


}
