using System.Collections;
using UnityEngine;
namespace Assets.Scripts
{
    public class PuddleScript : MonoBehaviour
    {

        public float _damage = 10;
        public float _livetime = 7f;
        public LayerMask IgnoreMe;
        public GameObject toxicGas;

        private float currundtime = 0;
        // Use this for initialization
        void Start()
        {
            StartCoroutine(SpawnParticleEffect());
        }

        // Update is called once per frame
        void Update()
        {
            if (currundtime > _livetime)
            {
                Destroy(gameObject);
            }
            else
            {
                currundtime += Time.deltaTime;
            }
        }
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Enemy")
            {

                other.gameObject.GetComponent<SlimeScript>().TakeDamage(_damage);
            }
        }

        IEnumerator SpawnParticleEffect()
        {
            Vector3 transformOffsetSpawn = transform.position;
            //transformOffsetSpawn.y += 10f;

            RaycastHit hit;
            if (Physics.Raycast(transformOffsetSpawn, Vector3.down, out hit, 1000f, ~IgnoreMe))
            {
                GameObject particle = Instantiate(toxicGas, hit.point, toxicGas.transform.rotation) as GameObject;
                ParticleSystem parSystem = particle.GetComponent<ParticleSystem>();
                parSystem.Play();

                yield return new WaitForSeconds(3);
                Debug.Log("DURATION: " + parSystem.main.duration);
                parSystem.Stop();
                Destroy(particle, parSystem.main.duration + 3.0f);

            }
        }

    }
}
