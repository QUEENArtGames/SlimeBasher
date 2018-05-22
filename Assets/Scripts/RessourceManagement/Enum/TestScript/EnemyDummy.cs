using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts {
    public class EnemyDummy : MonoBehaviour {

        public int Hitpoints = 100;

        private Transform finalDestination;

        // Use this for initialization
        void Start() {
            finalDestination = FindObjectOfType<Game>().FinalDestination;
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.destination = finalDestination.position;
        }

        // Update is called once per frame
        void Update() {
            if (Hitpoints <= 0)
                Destroy(this.gameObject);
        }

        public void TakeDamage(int damage)
        {
            Hitpoints -= damage;
        }
    }
}
