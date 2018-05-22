using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts {
    public class EnemyDummy : MonoBehaviour {

        public int Hitpoints = 100;

        private Transform _finalDestination;
        private NavMeshAgent _agent;

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
            if (other.transform.gameObject.CompareTag("Tower"))
            {
                _agent.SetDestination(other.transform.position);
            }
               
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.transform.gameObject.CompareTag("Tower"))
            {
                _agent.SetDestination(other.transform.position);
            }

        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.gameObject.CompareTag("Tower"))
            {
                _agent.SetDestination(other.transform.position);
            }

        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.transform.gameObject.CompareTag("Tower"))
            {
                other.gameObject.GetComponent<Tower>().Hitpoints = 0;
            }
        }


    }


}
